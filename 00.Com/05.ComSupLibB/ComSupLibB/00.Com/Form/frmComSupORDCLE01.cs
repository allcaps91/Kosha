using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupORDVIEW01.cs
    /// Description     : 오더정리
    /// Author          : 김홍록
    /// Create Date     : 2017-06-30
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\exam\exmain\EXMAIN33.frm" />
    public partial class frmComSupORDCLE01 : Form
    {
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod(); 
        clsSpread spread = new clsSpread();

        Thread thread;

        public enum enmOrderType { DEMENTIA,ENDO,XRAY,EXAM,PT,ETC };

        string gStrPtNo = "";
        enmOrderType gEnmOrderType;

        bool isChk = true;

        /// <summary>생성자</summary>
        public frmComSupORDCLE01(enmOrderType _enmOrderType, string strPtNo)
        {
            InitializeComponent();

            this.gStrPtNo = strPtNo;
            this.gEnmOrderType = _enmOrderType;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnCUD);

            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
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
            setCtrlPtInfo();
            setCtrlSpread();
        }

        void eBtnCUD(object sender, EventArgs e)
        {
            if (sender == this.btnDelete)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return;
                }

                setDelete();
            }

        }

        void setDelete()
        {
            // clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);
            string SqlErr = string.Empty;

            int nRow = 0;
            string SQL = string.Empty;

            int chkRow = 0;

            if (ComFunc.MsgBoxQ("삭제 후에는 검사 결과를 조회 할 수 없습니다. 삭제 하시겠습니까?","삭제메시지") == DialogResult.No)
            {
                return;

            }

            for (int i = 0; i < this.ssMain.ActiveSheet.RowCount; i++)
            {
                string strChk = ssMain.Sheets[0].Cells[i, (int)clsComSQL.enmSel_EXAM_ORDER_CLEAR.CHK].Text;
                string strROWID = ssMain.Sheets[0].Cells[i, (int)clsComSQL.enmSel_EXAM_ORDER_CLEAR.ROWID].Text;


                if (strChk == "True")
                {

                    //TODO : 2017.06.30.김홍록:현재는 진단검사의학과에서만 하고 있는 일을 타 부서에서도 해야 할지 고민
                    SqlErr = comSql.up_EXAM_ORDER(clsDB.DbCon, clsType.User.IdNumber, strROWID,true, ref nRow);
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

            if (chkRow > 0)
            {
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox(chkRow.ToString() + " 건을 삭제 하였습니다.");

                setCtrlSpread();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this,"R", clsDB.DbCon) == false)
            {
                this.Close(); //폼 권한 조회
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

            if (e.ColumnHeader == true && e.Column != (int)clsComSQL.enmSel_EXAM_ORDER_CLEAR.CHK)
            {
                clsSpread.gSpdSortRow(this.ssMain, e.Column);
            }
            else if (e.ColumnHeader == true && e.Column == (int)clsComSQL.enmSel_EXAM_ORDER_CLEAR.CHK)
            {
                spread.setSpdCellChk_All(this.ssMain, (int)clsComSQL.enmSel_EXAM_ORDER_CLEAR.CHK, this.isChk);

                if (this.isChk == true)
                {                    
                    this.isChk = false;
                }
                else
                {                 
                    this.isChk = true;
                }
            }
        }        

        void setCtrlPtInfo()
        {
            this.uPtInfo.txtSearch_PtInfo.Text = this.gStrPtNo;
            this.uPtInfo.setSname();
        }
      
        void setCtrlSpread()
        {
            if (string.IsNullOrEmpty(this.gStrPtNo) == true)
            {
                ComFunc.MsgBox("반드시 환자를 선택하여야 합니다.");
                return;
            }
            thread = new Thread(() => threadSetCtrlSpread(this.gStrPtNo));
            thread.Start();
        }

        void threadSetCtrlSpread(string strPtNo)
        {
            DataSet ds = null;

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));

            ds = comSql.sel_EXAM_ORDER_CLEAR(clsDB.DbCon, strPtNo);

            this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, comSql.sSel_EXAM_ORDER_CLEAR, comSql.nSel_EXAM_ORDER_CLEAR });
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
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_EXAM_ORDER_CLEAR.CHK, clsSpread.enmSpdType.CheckBox);
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_EXAM_ORDER_CLEAR.ROWID, clsSpread.enmSpdType.Hide);

            // 4. 정렬
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            // 5. sort, filter
            spread.setSpdFilter(spd, (int)clsComSQL.enmSel_EXAM_ORDER_CLEAR.BDATE, AutoFilterMode.EnhancedContextMenu, true);

        }
    }
}
