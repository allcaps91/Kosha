using ComLibB;
using ComSupLibB.Com;
using System.Windows.Forms;
using System;
using System.Data;
using System.Drawing;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupLbExSpecInfo.cs
    /// Description     : 검체번호 현황
    /// Author          : 김홍록
    /// Create Date     : 2017-06-12
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "\IPD\iusent\IUSENT06.frm, \IPD\ipdSim2\IUSENT06.frm" />
    public partial class frmComSupLbExVIEW01 : Form
    {

        bool isInit = true;
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
        clsParam param = new clsParam();

        DateTime sysdate;

        string gStrPano = string.Empty;
        string gStrFDate = string.Empty;

        /// <summary>생성자</summary>
        /// <param name="strPano">환자번호</param>
        /// <param name="strFdate">조회시작일자</param>
        public frmComSupLbExVIEW01(string strPano, string strFdate)
        {
            InitializeComponent();

            this.gStrPano = strPano;
            this.gStrFDate = strFdate;

            setEvent();
            

        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Move += new EventHandler(eFormMove);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnClear.Click += new EventHandler(eBtnEvent);
            this.supComPtInfo1.ePSMH_UcSupComPtSearch_VALUE += new UcSupComPtSearch.PSMH_RETURN_VALUE(ePSMH_ReturnValue);
            //this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.dtpFDate.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.cboWS.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
                       
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                //clsSpread.gSpdSortRow(this.ssMain, e.Column);
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
                setCtrl();
            }

            this.dtpFDate.Focus();            
        }

        void setCtrl()
        {
            setCtrlText();
            setCtrlDate();
            setCtrlCombo();
            setCtrlSpread();
        }

        void eFormMove(object sender, EventArgs e)
        {
            Point p = new Point();

            p.X = this.Location.X + this.supComPtInfo1.Location.X;
            p.Y = this.Location.Y + this.supComPtInfo1.Location.Y + this.supComPtInfo1.Height * 3 + 5;

            this.supComPtInfo1.pPSMH_LPoint = p;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnClear)
            {
                //this.ssMain.ActiveSheet.Rows.Count = 0;                
            }
            
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            setCtrlSpread();
        }

        void eCtrlKeyPress(object sender, KeyPressEventArgs e)
        {
            SendKeys.Send("{TAB}");
        }

        void ePSMH_ReturnValue(object sender, string pano, string sname)
        {
            if (isInit == false)
            {
                setCtrlSpread();

            }

            this.isInit = false;
            
        }

        void setCtrlText()
        {
            supComPtInfo1.txtSearch_PtInfo.Text = this.gStrPano;
            supComPtInfo1.setSname();

        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            this.dtpFDate.Value = sysdate;
            this.dtpTDate.Value = sysdate;

            if (string.IsNullOrEmpty(this.gStrFDate) == false)
            {
                this.dtpFDate.Value = Convert.ToDateTime(this.gStrFDate);
                this.dtpTDate.Value = Convert.ToDateTime(this.gStrFDate);
            }
        }

        void setCtrlCombo()
        {
            DataTable dt = comSql.sel_BAS_BCODE_COMBO(clsDB.DbCon, "EXAM_검체번호현황_WS");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboWS, dt, clsParam.enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }

            method.setCombo_View(this.cboWS, dt, clsParam.enmComParamComboType.ALL);
            
        }

        void setCtrlSpread()
        {
            DataSet ds = null;
            string strPano = this.supComPtInfo1.txtSearch_PtInfo.Text;
            string strWs = method.getGubunText(this.cboWS.Text, ".");

            if (string.IsNullOrEmpty(strPano) == true)
            {
                ComFunc.MsgBox("반드시 환자를 선택하세요");
                this.supComPtInfo1.txtSearch_PtInfo.Focus();
            }
            else
            {
                ds = lbExSQL.sel_EXAM_SPECMST_Viewer(clsDB.DbCon, strPano, this.dtpFDate.Value.ToString("yyyy-MM-dd"), this.dtpTDate.Value.ToString("yyyy-MM-dd"), strWs);
            }

            if (ComFunc.isDataSetNull(ds) == true)
            {
                //ComFunc.MsgBox("조회조건에 해당하는 값이 존재 하지 않습니다.");
            }

            //setSpdStyle(this.ssMain, ds, lbExSQL.sSel_EXAM_SPECMST_Viewer, lbExSQL.nSel_EXAM_SPECMST_Viewer);
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

            // 4. 정렬
            spread.setColAlign(spd, -1, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            // 5. sort, filter
            //spread.setSpdFilter(spd, 0, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdSort(spd, (int)clsComSupLbExSQL.enmSel_EXAM_SPECMST_Viewer.SPECNO, true);
            spread.setSpdSort(spd, (int)clsComSupLbExSQL.enmSel_EXAM_SPECMST_Viewer.PANO, true);
            spread.setSpdSort(spd, (int)clsComSupLbExSQL.enmSel_EXAM_SPECMST_Viewer.SNAME, true);
            spread.setSpdSort(spd, (int)clsComSupLbExSQL.enmSel_EXAM_SPECMST_Viewer.RECEIVEDATE, true);

            // 6. 조건부서식
            UnaryComparisonConditionalFormattingRule unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "검사완료", false);
            unary.BackColor = param.cPaleGreen;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupLbExSQL.enmSel_EXAM_SPECMST_Viewer.STATUS, unary);

            UnaryComparisonConditionalFormattingRule unary2 = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "부분완료", false);
            unary2.BackColor = param.cPalePurple;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupLbExSQL.enmSel_EXAM_SPECMST_Viewer.STATUS, unary2);


        }


    }
}
