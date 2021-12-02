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
    public partial class frmComSupLbExRSLT02 : Form, MainFormMessage
    {
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


        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등
                setCtrl();
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        #endregion


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

        public frmComSupLbExRSLT02(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmComSupLbExRSLT02()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load               += new EventHandler(eFormLoad);

            this.Activated          += new EventHandler(eFormActivated);
            this.FormClosed         += new FormClosedEventHandler(eFormClosed);

            this.Move               += new EventHandler(eFormMove);
            this.btnSearch.Click    += new EventHandler(eBtnSearch);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnClear.Click     += new EventHandler(eBtnClick);
            this.btnPrint.Click     += new EventHandler(eBtnPrint);

            this.btnSave.Click      += new EventHandler(eBtnSave);

            this.dtpFDate.KeyPress  += new KeyPressEventHandler(eCtrlKeyPress);
            this.dtpTDate.KeyPress  += new KeyPressEventHandler(eCtrlKeyPress);
            this.cboWS.KeyPress     += new KeyPressEventHandler(eCtrlKeyPress);

            this.rdoPtInfo.Click    += new EventHandler(eRdoClick);
            this.rdoSpecNo.Click    += new EventHandler(eRdoClick);


            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpreadDoubleClick);
            
                       
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == this.rdoPtInfo)
            {
                this.ucPT_INFO.PSMH_TYPE = ComSupLibB.UcSupComPtSearch.enmType.PTINFO;
                this.ucPT_INFO.lblPtNo.Text = "환자정보";

            }
            else if (sender == this.rdoSpecNo)
            {
                this.ucPT_INFO.PSMH_TYPE = ComSupLibB.UcSupComPtSearch.enmType.SPECINFO;
                this.ucPT_INFO.lblPtNo.Text = "검체정보";
            }
        }

        void setCtrl()
        {
            setCtrlDate();
            setCtrlCombo();
            setCtrlSpread();
        }

        void eFormMove(object sender, EventArgs e)
        {
            Point p = new Point();

            p.X = this.Location.X + this.ucPT_INFO.Location.X;
            p.Y = this.Location.Y + this.panel1.Location.Y + this.ucPT_INFO.Location.Y + this.ucPT_INFO.Height * 3 + 5;

            this.ucPT_INFO.pPSMH_LPoint = p;
        }

        void eBtnClick(object sender, EventArgs e)
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

        void eBtnSave(object sender, EventArgs e)
        {
            clsDB.setBeginTran(clsDB.DbCon);

            string SqlErr   = string.Empty;
            string SQL          = string.Empty;
            int intRowAffected  = 0;
            int chkRow = 0;

            string strCODE   = "";
            string strROWID     = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (int i = 0; i < this.ssMain.ActiveSheet.Rows.Count; i++)
                {
                    if (this.ssMain.ActiveSheet.Cells[i,(int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.CHK].Text.Trim().Equals("True") == true)
                    {
                        strCODE     = method.getGubunText(this.ssMain.ActiveSheet.Cells[i, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.SAYU_NM].Text.Trim(),".").Trim();
                        strROWID    = this.ssMain.ActiveSheet.Cells[i, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.ROWID].Text.Trim();

                        lbExSQL.UP_EXAM_HISRESULTC(clsDB.DbCon, strROWID, strCODE, ref intRowAffected);

                        if (string.IsNullOrEmpty(SqlErr) == false)
                        {
                            method.setTranction(clsDB.DbCon, true, SqlErr, SQL, chkRow);
                            return;
                        }
                    }                
                }

                method.setTranction(clsDB.DbCon, false, SqlErr, SQL, chkRow, false);
                Cursor.Current = Cursors.Default;

                setCtrlSpread();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message.ToString());
            }

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

        void eSpreadDoubleClick(object sender, CellClickEventArgs e)
        { 
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(this.ssMain, e.Column);
                return;
            }

            frmComSupLbExHELP01 f = new frmComSupLbExHELP01(clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.RESULT_CHANGE, null, true);
            f.ePsmhReturnValue += F_ePsmhReturnValue;
            f.ShowDialog();
        }

        void F_ePsmhReturnValue(object sender, string code, string name, string Yname)
        {

            if (string.IsNullOrEmpty(code.Trim()) == true)
            {
                this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.SAYU_NM].Text = "";
            }
            else
            {
                this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.SAYU_NM].Text = code + "." + name;
            }

            this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.CHK].Text = "True";
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

            string strPano = "";
            string strSpecNo = "";

            if (this.rdoPtInfo.Checked == true)
            {
                strPano = this.ucPT_INFO.txtSearch_PtInfo.Text;
            }
            else if (this.rdoSpecNo.Checked == true)
            {
                strSpecNo = this.ucPT_INFO.txtSearch_PtInfo.Text;
            }

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

                strFoot = "/c/f1포항성모병원 진단검사의학과";

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

            spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.CHK, clsSpread.enmSpdType.CheckBox);
            spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.ROWID, clsSpread.enmSpdType.Hide);

            //spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_EXAM_HISRESULTC.SAYU_NM, clsSpread.enmSpdType.Text,null,null,null,null,false);

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
