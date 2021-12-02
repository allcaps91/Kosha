using ComBase; //기본 클래스
using ComDbB; //DB연결
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupEXRSLT01.cs
    /// Description     : Glucose 입력
    /// Author          : 김홍록
    /// Create Date     : 2017-06-30 : 협의 하에 수정 루틴 추가함.
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\exam\FrmGlucose.frm" />
    public partial class frmComSupEXRSLT01 : Form
    {
        clsComSQL comSql = new clsComSQL();
        ComSupLibB.SupLbEx.clsComSupLbExSQL lbExSql = new SupLbEx.clsComSupLbExSQL();
        clsMethod method = new clsMethod(); 
        clsSpread spread = new clsSpread();

        Thread thread;

        string gStrRowId = ""; 
        string gStrSabun = "";

        /// <summary>생성자</summary>
        public frmComSupEXRSLT01(string strRowId, string strSabun)
        {
            InitializeComponent();

            this.gStrRowId = strRowId;
            this.gStrSabun = strSabun;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnCUD);           
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
            setCtrlSpread();
        }

        void eBtnCUD(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return;
                }

                setSave(clsDB.DbCon);
            }

        }

        bool isSave()
        {
            bool b = true;

            if (this.ssMain.ActiveSheet.Rows.Count < 1)
            {
                ComFunc.MsgBox("데이터가 존재 하지 않습니다.");
                b = false;
            }

            string strResult = ssMain.Sheets[0].Cells[0, (int)clsComSQL.enmSel_EXAM_ORDER_CR59B.RESULT].Text;

            if (string.IsNullOrEmpty(strResult) == true)
            {
                ComFunc.MsgBox("측정 결과를 입력하세요.");
                b = false;

            }

            return b;
        }

        void setSave(PsmhDb pDbCon)
        {
            if (isSave() == false)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            string SqlErr = string.Empty;
            int intRowAffected = 0;

            string SQL = string.Empty;

            int chkRow = 0;

            string strResult    = ssMain.ActiveSheet.Cells[0, (int)clsComSQL.enmSel_EXAM_ORDER_CR59B.RESULT].Text;
            string strROWID     = ssMain.ActiveSheet.Cells[0, (int)clsComSQL.enmSel_EXAM_ORDER_CR59B.ROWID].Text;
            string strSpecNo    = ssMain.ActiveSheet.Cells[0, (int)clsComSQL.enmSel_EXAM_ORDER_CR59B.SPECNO].Text;

            if (string.IsNullOrEmpty(strSpecNo) == true)
            {
                string strNewSpecNO = lbExSql.sel_SpecNO(pDbCon);

                SqlErr = comSql.ins_EXAM_SPECMST(clsDB.DbCon, strROWID, strNewSpecNO, ref intRowAffected);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SqlErr = comSql.ins_EXAM_REULTC_CR59B(clsDB.DbCon, strROWID, strNewSpecNO, strResult,this.gStrSabun, ref intRowAffected);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SqlErr = comSql.up_EXAM_ORDER_CR59B(clsDB.DbCon, strROWID, strNewSpecNO, ref intRowAffected);
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
            else
            {
                SqlErr = comSql.up_EXAM_REULTC_CR59B(clsDB.DbCon, strSpecNo, strResult, this.gStrSabun, ref intRowAffected);

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

            if (chkRow > 0)
            {
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox(chkRow.ToString() + " 건을 저장 하였습니다.");

                this.Close();

                //setCtrlSpread();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }
      
        void setCtrlSpread()
        {
            if (string.IsNullOrEmpty(this.gStrRowId) == true)
            {
                ComFunc.MsgBox("선택된 처방이 존재 하지 않습니다.");
                return;
            }
            thread = new Thread(() => threadSetCtrlSpread(this.gStrRowId));
            thread.Start();
        }

        void threadSetCtrlSpread(string strRowId)
        {
            DataSet ds = null;

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);

            ds = comSql.sel_EXAM_ORDER_CR59B(clsDB.DbCon, strRowId);

            this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, comSql.sSel_EXAM_ORDER_CR59B, comSql.nSel_EXAM_ORDER_CR59B });
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
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_EXAM_ORDER_CR59B.ROWID, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_EXAM_ORDER_CR59B.SPECNO, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_EXAM_ORDER_CR59B.RESULT, clsSpread.enmSpdType.Text);

            // 4. 정렬
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);
            

        }
    }
}
