using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupInfc
    /// File Name       : frmComSupInfcCODE01.cs
    /// Description     : ORDERSlip별검체번호
    /// Author          : 김홍록
    /// Create Date     : 2017-06-22
    /// Update History  : 
    /// </summary>
    /// <history>       
    ///                 2017.06.26.김홍록 : 이름을 변경하는 루틴을 필요 없음.
    /// </history>
    /// <seealso cref= "d:\psmh\exam\excode\EXCODE13.frm" />
    public partial class frmComSupLbExCODE01 : Form, MainFormMessage
    {

        clsComSupLbExSQL lbExSql = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread(); 

        DateTime sysdate;

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

#endregion

        /// <summary>생성자</summary>
        public frmComSupLbExCODE01()
        {
            InitializeComponent();
            setEvent();
        }

        public frmComSupLbExCODE01(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnClear.Click += new EventHandler(eBtnClick);

            this.btnSave.Click += new EventHandler(eBtnSave);
            this.btnDelete.Click += new EventHandler(eBtnSave);         

            cboSlip.SelectedIndexChanged += new EventHandler(eCboIndexChange);
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
            //setCtrlDate();
            setCtrlCombo();
            setCtrlSpread();

        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return ; //권한 확인
            }

            if (sender == this.btnSave)
            {
                setSave(true);
            }
            else if (sender == this.btnDelete)
            {
                setSave(false);
            }

            
        }

        void eBtnCahngeName(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

        }

        void eBtnSearch(object sender, EventArgs e)
        {
            setCtrlSpread();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            if (sender == this.btnClear)
            {
                this.ssMain.ActiveSheet.Rows.Count = 0;
            }
        }      

        void eCboIndexChange(object sender, EventArgs e)
        {
            setCtrlSpread();
        }

        void setSave(bool isSave)
        {
            // clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            string SqlErr = string.Empty;
            string SQL = string.Empty;

            string strChk = string.Empty;
            string strCODE = string.Empty;
            string strName = string.Empty;
            string strSlipNo = string.Empty;
            string strRowID = string.Empty;

            int intRowAffected = 0;
            int chkRow = 0;

            strSlipNo = method.getGubunText(this.cboSlip.Text,".");

            if (string.IsNullOrEmpty(strSlipNo) == true)
            {
                ComFunc.MsgBox("슬립번호가 선택 되어 있지 않습니다. 반드시 선택하세요");
                return;
            }

            // 저장
            if (isSave == true)
            {
                for (int i = 0; i < this.ss_SpecCode.ActiveSheet.RowCount; i++)
                {
                    strChk = this.ss_SpecCode.ActiveSheet.Cells[i, (int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_OCS_OSPECIMAN.CHK].Text;
                    strCODE = this.ss_SpecCode.ActiveSheet.Cells[i, (int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_OCS_OSPECIMAN.CODE].Text;
                    strName = this.ss_SpecCode.ActiveSheet.Cells[i, (int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_OCS_OSPECIMAN.NAME].Text;

                    if (strChk == "True")
                    {
                        SqlErr = lbExSql.ins_OCS_OSPECIMAN(clsDB.DbCon, strSlipNo, strCODE, strName, ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        chkRow += 1;
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.ssMain.ActiveSheet.RowCount; i++)
                {
                    strChk = this.ssMain.ActiveSheet.Cells[i, (int)clsComSupLbExSQL.enmSel_OCS_OSPECIMAN.CHK].Text;
                    strCODE = this.ssMain.ActiveSheet.Cells[i, (int)clsComSupLbExSQL.enmSel_OCS_OSPECIMAN.SPECCODE].Text;
                    strName = this.ssMain.ActiveSheet.Cells[i, (int)clsComSupLbExSQL.enmSel_OCS_OSPECIMAN.SPECNAME].Text;
                    strRowID = this.ssMain.ActiveSheet.Cells[i, (int)clsComSupLbExSQL.enmSel_OCS_OSPECIMAN.ROWID].Text;

                    if (strChk == "True")
                    {
                        SqlErr = lbExSql.del_OCS_OSPECIMAN(clsDB.DbCon, strRowID, ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        chkRow += 1;
                    }
                }

            }

            if (chkRow > 0)
            {
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox(chkRow.ToString() + " 건을 저장하였습니다.");

                setCtrlSpread();
            }
            
        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            //this.dtpFDate.Value = sysdate.AddDays(-7);
            //this.dtpTDate.Value = sysdate.AddDays(-1);
        }

        void setCtrlCombo()
        {
            DataTable dt = comSql.sel_OCS_ORDERCODE_EXAM_COMBO(clsDB.DbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {

                method.setCombo_View(this.cboSlip, dt, clsParam.enmComParamComboType.None);
            }
            else
            {
                ComFunc.MsgBox("데이터가 존재하지 않습니다.");
            }
        }

        void setCtrlSpread()
        {
            setCtrlSpread_Main();
            setCtrlSpread_SpecCode();                   
        }

        void setCtrlSpread_Main()
        {
            string s = method.getGubunText(this.cboSlip.Text, ".");
            DataSet ds = lbExSql.sel_OCS_OSPECIMAN(clsDB.DbCon, s);
            setSpdStyle(this.ssMain, ds, lbExSql.sSel_OCS_OSPECIMAN, lbExSql.nSel_OCS_OSPECIMAN);

        }

        void setCtrlSpread_SpecCode()
        {
            string s = method.getGubunText(this.cboSlip.Text, ".");

            DataSet ds = lbExSql.sel_EXAM_SPECODE_OCS_OSPECIMAN(clsDB.DbCon,"14", s);
            setSpdStyle(this.ss_SpecCode, ds, lbExSql.sSel_EXAM_SPECODE_OCS_OSPECIMAN, lbExSql.nSel_EXAM_SPECODE_OCS_OSPECIMAN);
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

            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            if (spd == this.ss_SpecCode)
            {
                spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_OCS_OSPECIMAN.CHK, clsSpread.enmSpdType.CheckBox);
                spread.setSpdFilter(spd, (int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_OCS_OSPECIMAN.NAME, AutoFilterMode.EnhancedContextMenu, true);
            }
            else if (spd == this.ssMain)
            {
                spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_OCS_OSPECIMAN.CHK, clsSpread.enmSpdType.CheckBox);
                spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_OCS_OSPECIMAN.ROWID, clsSpread.enmSpdType.Hide);
            }

            // 4. 정렬
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

        }
    }
}
