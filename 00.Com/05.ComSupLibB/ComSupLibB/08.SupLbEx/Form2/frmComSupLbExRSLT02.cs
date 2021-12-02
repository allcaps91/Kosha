using ComLibB;
using ComSupLibB.Com;
using System.Windows.Forms;
using System;
using System.Data;
using System.Drawing;
using FarPoint.Win.Spread;
using System.Threading;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupLbExRSLT02.cs
    /// Description     : 검사결과수정내역
    /// Author          : 김홍록
    /// Create Date     : 2017-06-28
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\exam\exmain\EXMAIN37.frm" />
    public partial class frmComSupLbExRSLT02 : Form
    {

        bool isInit = true;
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
        clsParam param = new clsParam();

        DateTime sysdate;

        Thread thread;

        string gStrPano = string.Empty;
        string gStrFDate = string.Empty;
        string gStrSecNo = string.Empty;
        string gStrRowId = string.Empty;


        /// <summary>생성자</summary>
        /// <param name="strPano">환자번호</param>
        /// <param name="strFdate">조회시작일자</param>
        public frmComSupLbExRSLT02(string pStrSpecNo, string pStrPtno = "",  string pStrFDate ="")
        {
            InitializeComponent();

            this.gStrPano = pStrPtno;
            this.gStrSecNo = pStrSpecNo;

            //this.gStrFDate = strFdate;

            setEvent();            
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.Move += new EventHandler(eFormMove);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnClear.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.uPtInfo.ePSMH_UcSupComPtSearch_VALUE += new UcSupComPtSearch.PSMH_RETURN_VALUE(ePSMH_ReturnValue);

            this.dtpFDate.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.cboWS.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);

            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpreadDoubleClick);
                       
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
            setCtrlText(true);
            setCtrlDate();
            setCtrlCombo();
            setCtrlSpread();
        }

        void eFormMove(object sender, EventArgs e)
        {
            Point p = new Point();

            p.X = this.Location.X + this.uPtInfo.Location.X;
            p.Y = this.Location.Y + this.uPtInfo.Location.Y + this.uPtInfo.Height * 3 + 5;

            this.uPtInfo.pPSMH_LPoint = p;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnClear)
            {
                this.ssMain.ActiveSheet.Rows.Count = 0;
                this.btnSearch.Focus();
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

        void eBtnPrint(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            setCtrlSpreadPrint();
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

        void eSpreadDoubleClick(object sender, CellClickEventArgs e)
        { 
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(this.ssMain, e.Column);
                return;
            }
        }

        void ePSMH_RETURN_VALUE(object sender, string code, string name, string Yname)
        {
            int intRowAffected = 0;
            int chkRow = 0;

            // clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            string SqlErr = string.Empty;
            string SQL = string.Empty;
            string strRowId = string.Empty;

            if (string.IsNullOrEmpty(name) == false)
            {
                lbExSQL.UP_EXAM_HISRESULTC(clsDB.DbCon, this.gStrRowId, code, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                chkRow += 1;
                if (chkRow > 0)
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox(chkRow.ToString() + " 건을 저장 하였습니다.");
                    setCtrl();
                }
            }
        }

        void setCtrlText(bool isInit)
        {
            if (isInit == true)
            {
                if (string.IsNullOrEmpty(this.gStrPano) == false)
                {
                    this.uPtInfo.txtSearch_PtInfo.Text = this.gStrPano;
                    this.uPtInfo.setSname();
                }

                this.txtSpecNo.Text = this.gStrSecNo;
            }else
            {
                this.uPtInfo.txtSearch_PtInfo.Text = string.Empty;
                this.uPtInfo.setSname();

                this.txtSpecNo.Text = string.Empty;
            }
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
            DataTable dt = comSql.sel_EXAM_SPECODE_WS_COMBO(clsDB.DbCon);

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

            string strPano = this.uPtInfo.txtSearch_PtInfo.Text;
            string strSpecNo = this.txtSpecNo.Text;
            string strWs = method.getGubunText(this.cboWS.Text, ".");
            string strFDate = this.dtpFDate.Value.ToString("yyyy-MM-dd");
            string strTDate = this.dtpTDate.Value.ToString("yyyy-MM-dd");
            

            thread = new Thread(() => threadSetCtrlSpread(strFDate,strTDate,this.rdoUpdate.Checked,strWs,strSpecNo,strPano));
            thread.Start();                


        }

        void setCtrlSpreadPrint()
        {
            string strHeader = string.Empty;
            string strFoot = string.Empty;

            string s = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");

            if (this.ssMain.ActiveSheet.Rows.Count > 0)
            {

                strHeader = spread.setSpdPrint_String("검사 결과 수정내역", new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += spread.setSpdPrint_String("조회기간:" + dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += spread.setSpdPrint_String("출력시간:" + s + "       PAGE : /p / /pc" , new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

                strFoot = "/c/f1포항성모병원 임상병리과";

                clsSpread.SpdPrint_Margin setMargin = new clsSpread.SpdPrint_Margin(1, 1, 1, 1, 1, 1);
                clsSpread.SpdPrint_Option setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, false, false, false, true);

                spread.setSpdPrint(this.ssMain, true, setMargin, setOption, strHeader, strFoot);

            }


        }

        bool isDateGap(string strFDate, string strTDate)
        {
            bool b = true;

            if (method.getDate_Gap(Convert.ToDateTime(strFDate), Convert.ToDateTime(strTDate)) > 60)
            {
                ComFunc.MsgBox("일자는 60일을 넘을 수 없습니다.");
                this.dtpFDate.Focus();
                return b;
            }

            return b;
        }

        void threadSetCtrlSpread(string strFDate, string strTDate, bool isGbn, string strWs, string strSpecNo, string strPano)
        {
            DataSet ds = null;
            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));

            ds = lbExSQL.sel_EXAM_HISRESULTC(clsDB.DbCon, strFDate, strTDate, isGbn,strWs,strSpecNo,strPano);
            this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, lbExSQL.sSel_EXAM_HISRESULTC, lbExSQL.nSel_EXAM_HISRESULTC });
            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), false);

            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));            
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
            spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.ROWID, clsSpread.enmSpdType.Hide);

            // 4. 정렬
            spread.setColAlign(spd, -1, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            // 5. sort, filter
            //spread.setSpdFilter(spd, 0, AutoFilterMode.EnhancedContextMenu, true);

            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.PANO);
            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.SNAME);
            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.SPECNO);



        }

        


    }
}
