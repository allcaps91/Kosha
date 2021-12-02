using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupInfc
    /// File Name       : frmComSupInfcCODE01.cs
    /// Description     : 감염관리코드
    /// Author          : 김홍록
    /// Create Date     : 2017-06-22
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\exam\exinfect\ExInfect50.frm" />
    public partial class frmComSupInfcCODE01 : Form
    {

        clsInFcSQL inFcSql = new clsInFcSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        DateTime sysdate;

        Thread thread;

        /// <summary>생성자</summary>
        public frmComSupInfcCODE01()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnSave);
            this.btnPrint.Click += new EventHandler(eBtnPrintClick);

            this.btnAdd.Click += new EventHandler(eSpreadCellAdd);

            this.ssMain.EditChange += new EditorNotifyEventHandler(eSpreadEditChange);
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
            setCtrlDate();
            //setCtrlCombo();
            setCtrlSpread();

        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return ; //권한 확인
            }

            setSave();
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
        }

        void eBtnPrintClick(object sender, EventArgs e)
        {
            setCtrlSpreadPrint();
        }

        void eSpreadCellAdd(object sender, EventArgs e)
        {
            int nRow = this.ssMain.ActiveSheet.ActiveCell.Row.Index;

            if (nRow > -1)
            {
                this.ssMain.ActiveSheet.AddRows(nRow, 1);
            }
            else
            {
                this.ssMain.ActiveSheet.RowCount = this.ssMain.ActiveSheet.RowCount+1;
            }

            
        }

        void eSpreadEditChange(object sender, EditorNotifyEventArgs e)
        {
            string s = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.CHK].Text;

            if (e.Column > (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.CHK)
            {
                this.ssMain.ActiveSheet.Cells[e.Row, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.EDIT].Text = "Y";

                if (s == "")
                {
                    this.ssMain.ActiveSheet.Cells[e.Row, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.CHK].Text = "True";
                }


            }
        }

        void setSave()
        {
            // clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);
            string SqlErr = string.Empty;

            int nRow =0;
            string SQL = string.Empty;

            string strChk = string.Empty;
            string strTRCODE = string.Empty;
            string strCHKNAME = string.Empty;
            string strNAME = string.Empty;
            string strROWID = string.Empty;
            string strEDIT = string.Empty;

            int chkRow = 0;

            for (int i = 0; i < this.ssMain.ActiveSheet.RowCount; i++)
            {
                strChk = ssMain.Sheets[0].Cells[i, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.CHK].Text;
                strTRCODE = ssMain.Sheets[0].Cells[i, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.TRCODE].Text;
                strCHKNAME = ssMain.Sheets[0].Cells[i, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.CHKNAME].Text;
                strNAME = ssMain.Sheets[0].Cells[i, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.NAME].Text;
                strROWID = ssMain.Sheets[0].Cells[i, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.ROWID].Text;
                strEDIT = ssMain.Sheets[0].Cells[i, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.EDIT].Text;

                if (strChk == "True" && strEDIT == "Y")
                {
                    if (string.IsNullOrEmpty(strTRCODE))
                    {
                        ComFunc.MsgBox("반드시 코드를 입력하세요");
                        return;
                    }

                    if (string.IsNullOrEmpty(strCHKNAME))
                    {
                        ComFunc.MsgBox("반드시 명칭를 입력하세요");
                        return;
                    }

                    SqlErr = inFcSql.ins_EXAM_INFECTTRCODE(clsDB.DbCon, strTRCODE, strCHKNAME, strNAME,  ref nRow, strROWID, ref SQL);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return ;
                    }

                    chkRow += 1;
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
            List<string> lstCbo = new List<string>();
            List<string> lstSqlWhere = new List<string>();

            lstSqlWhere.Add("CS");
            lstSqlWhere.Add("RT");
            lstSqlWhere.Add("PT");
            lstSqlWhere.Add("II");
            lstSqlWhere.Add("AN");

            DataTable dt = comSql.sel_BAS_CLINICDEPT_COMBO(clsDB.DbCon, lstSqlWhere);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstCbo.Add(dt.Rows[i]["CODE_NAME"].ToString());
                }

                //method.setCombo_View(this.cboDept, lstCbo, clsParam.enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("데이터가 존재하지 않습니다.");
            }


        }

        void setCtrlSpread()
        {

            thread = new Thread(() => threadSetCtrlSpread());
            thread.Start();
        }

        void threadSetCtrlSpread()
        {
            DataSet ds = null;

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));

            ds = inFcSql.sel_EXAM_INFECTTRCODE(clsDB.DbCon);

            this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, inFcSql.sSel_EXAM_INFECTTRCODE, inFcSql.nSel_EXAM_INFECTTRCODE });
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
            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Text);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.CHK, clsSpread.enmSpdType.CheckBox);

            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.ROWID, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.EDIT, clsSpread.enmSpdType.Hide);


            // 4. 정렬
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            // 5. sort, filter
            spread.setSpdFilter(spd, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.CHKNAME, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.NAME, AutoFilterMode.EnhancedContextMenu, true);            

            spread.setSpdSort(spd, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.CHKNAME, true);
            spread.setSpdSort(spd, (int)clsInFcSQL.enmSel_EXAM_INFECTTRCODE.NAME, true);
        }

        void setCtrlSpreadPrint()
        {

            string strHeader = string.Empty;
            string strFoot = string.Empty;

            string s = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");


            if (this.ssMain.ActiveSheet.Rows.Count > 0)
            {

                strHeader = spread.setSpdPrint_String("감염 기초 코드 균주 리스트", new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += spread.setSpdPrint_String("출력시간:" + s, new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

                clsSpread.SpdPrint_Margin setMargin = new clsSpread.SpdPrint_Margin(1, 1, 1, 1, 1, 1);
                clsSpread.SpdPrint_Option setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

                spread.setSpdPrint(this.ssMain, true, setMargin, setOption, strHeader, strFoot);

            }


        }
    }
}
