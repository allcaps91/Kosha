using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

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
    public partial class frmComSupLbExHELP02 : Form
    {
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        DataTable gDt;
        DataTable gDt_WS;
        DataTable gDt_EQU;

        enum enmSpd {CHK,NAME,CODE };

        bool isWSchk = true;
        bool isEQUchk = true;
        bool isExamChk = true;

        public frmComSupLbExHELP02()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load                       += new EventHandler(eFormLoad);
            this.btnExit.Click              += new EventHandler(eBtnClick);

            this.ssWS.CellDoubleClick       += new CellClickEventHandler(eSpreadDClick);
            this.ssEQU.CellDoubleClick      += new CellClickEventHandler(eSpreadDClick);
            this.ssExam.CellDoubleClick     += new CellClickEventHandler(eSpreadDClick);

            this.ssWS.ButtonClicked         += new EditorNotifyEventHandler(eSpreadBtnClick);
            this.ssEQU.ButtonClicked        += new EditorNotifyEventHandler(eSpreadBtnClick);

            this.btnSelect.Click            += new EventHandler(eBtnSave);

            this.btn_CHK_ALL_WS.Click       += new EventHandler(eBtnChkALL);
            this.btn_CHK_ALL_EQU.Click      += new EventHandler(eBtnChkALL);
            this.btn_CHK_ALL_EXAM.Click     += new EventHandler(eBtnChkALL);

            this.btn_CHK_NONE_WS.Click      += new EventHandler(eBtnChkNone);
            this.btn_CHK_NONE_EQU.Click     += new EventHandler(eBtnChkNone);
            this.btn_CHK_NONE_EXAM.Click    += new EventHandler(eBtnChkNone);

        }

        void eBtnChkNone(object sender, EventArgs e)
        {
            if (sender == this.btn_CHK_NONE_WS)
            {
                this.isWSchk = false;
                setCtrlSpreadChkWS();
            }
            else if (sender == this.btn_CHK_NONE_EQU)
            {
                this.isEQUchk = false;
                setCtrlSpreadChkEQU();
            }
            else if (sender == this.btn_CHK_NONE_EXAM)
            {
                this.isExamChk = false;
                setCtrlSpreadChkEXAM();
            }
        }

        void eBtnChkALL(object sender, EventArgs e)
        {
            if (sender == this.btn_CHK_ALL_WS)
            {
                this.isWSchk = true;
                setCtrlSpreadChkWS();
            }
            else if (sender == this.btn_CHK_ALL_EQU)
            {
                this.isEQUchk = true;
                setCtrlSpreadChkEQU();
            }
            else if (sender == this.btn_CHK_ALL_EXAM)
            {
                this.isExamChk = true;
                setCtrlSpreadChkEXAM();
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
        }

        void setCtrl()
        {
            setCtrlDt();
            setCtrlSpread();
            setInit();
        }

        void setInit()
        {
            string[] strArrWS;
            string[] strArrCODE;
            string[] strArrEQU;

            string strWS    = comSql.sel_BAS_PCCONFIG(clsDB.DbCon, clsComSQL.enmSel_BAS_PCCONFIG.WS, false);
            string strCODE  = comSql.sel_BAS_PCCONFIG(clsDB.DbCon, clsComSQL.enmSel_BAS_PCCONFIG.CODE, false);
            string strEQU   = comSql.sel_BAS_PCCONFIG(clsDB.DbCon, clsComSQL.enmSel_BAS_PCCONFIG.EQU, false);

            if (string.IsNullOrEmpty(strWS.Trim()) == false)
            {
                strArrWS = strWS.Split(',');

                for (int i = 0; i < strArrWS.Length; i++)
                {
                    for (int j = 0; j < this.ssWS.ActiveSheet.RowCount; j++)
                    {
                        if (this.ssWS.ActiveSheet.Cells[j,2].Text == strArrWS[i])
                        {
                            this.ssWS.ActiveSheet.Cells[j, 0].Text = "True";
                            break;
                        }

                    }
                }
                setCtrlSpread_EQU();
            }

            if (string.IsNullOrEmpty(strEQU.Trim()) == false)
            {
                strArrEQU = strEQU.Split(',');

                for (int i = 0; i < strArrEQU.Length; i++)
                {
                    for (int j = 0; j < this.ssEQU.ActiveSheet.RowCount; j++)
                    {
                        if (this.ssEQU.ActiveSheet.Cells[j, 2].Text == strArrEQU[i])
                        {
                            this.ssEQU.ActiveSheet.Cells[j, 0].Text = "True";
                            break;
                        }

                    }
                }

                setCtrlSpread_Exam();
            }

            if (string.IsNullOrEmpty(strCODE.Trim()) == false)
            {
                strArrCODE = strCODE.Split(',');

                for (int i = 0; i < strArrCODE.Length; i++)
                {
                    for (int j = 0; j < this.ssExam.ActiveSheet.RowCount; j++)
                    {
                        if (this.ssExam.ActiveSheet.Cells[j, 2].Text == strArrCODE[i])
                        {
                            this.ssExam.ActiveSheet.Cells[j, 0].Text = "True";
                            break;
                        }

                    }
                }
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eSpreadBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (sender == this.ssWS)
            {
                setCtrlSpread_EQU();
            }
            else if (sender == this.ssEQU)
            {
                setCtrlSpread_Exam();
            }
            
        }

        void ePSMH_RETURN_VALUE(object sender, string code, string name, string Yname)
        {
            
        }

        void setCtrlDt()
        {
            this.gDt = lbExSQL.sel_EXAM_SPECODE_ALL(clsDB.DbCon);

            if (ComFunc.isDataTableNull(this.gDt) == false)
            {
                this.gDt_WS = this.gDt.Clone();
                foreach (DataRow item in gDt.Select("GUBUN = '12'"))
                {
                    this.gDt_WS.ImportRow(item);
                }

                this.gDt_EQU = this.gDt.Clone();
                foreach (DataRow item in gDt.Select("GUBUN = '13'"))
                {
                    this.gDt_EQU.ImportRow(item);
                }
            }
        }

        void setCtrlSpread()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            this.ssWS.ActiveSheet.RowCount = 0;
            this.ssEQU.ActiveSheet.RowCount = 0;
            this.ssExam.ActiveSheet.RowCount = 0;

            spread.setColStyle(this.ssWS, -1, -1, clsSpread.enmSpdType.Label);
            spread.setColStyle(this.ssEQU, -1, -1, clsSpread.enmSpdType.Label);
            spread.setColStyle(this.ssEQU, -1,(int)enmSpd.CHK, clsSpread.enmSpdType.CheckBox);

            spread.setColStyle(this.ssExam, -1, -1, clsSpread.enmSpdType.Label);
            spread.setColStyle(this.ssExam, -1, (int)enmSpd.CHK, clsSpread.enmSpdType.CheckBox);
         
            setCtrlSpread_WS();

            for (int i = 0; i < ssWS.ActiveSheet.Rows.Count; i++)
            {
                if (ssWS.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text.Equals("True") == true)
                {
                    ssWS.ActiveSheet.Cells[i, (int)enmSpd.CHK].BackColor = method.cSpdRowSumColor;
                    ssWS.ActiveSheet.Cells[i, (int)enmSpd.CHK].ForeColor = method.cSpdCellImpact_Fore;
                }
                else
                {
                    ssWS.ActiveSheet.Cells[i, (int)enmSpd.CHK].BackColor = Color.White;
                    ssWS.ActiveSheet.Cells[i, (int)enmSpd.CHK].ForeColor = Color.Black;
                }
            }

            for (int i = 0; i < ssEQU.ActiveSheet.Rows.Count; i++)
            {
                if (ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text.Equals("True") == true)
                {
                    ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CHK].BackColor = method.cSpdRowSumColor;
                    ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CHK].ForeColor = method.cSpdCellImpact_Fore;
                }
                else
                {
                    ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CHK].BackColor = Color.White;
                    ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CHK].ForeColor = Color.Black;
                }
            }

            for (int i = 0; i < ssExam.ActiveSheet.Rows.Count; i++)
            {
                if (ssExam.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text.Equals("True") == true)
                {
                    ssExam.ActiveSheet.Cells[i, (int)enmSpd.CHK].BackColor = method.cSpdRowSumColor;
                    ssExam.ActiveSheet.Cells[i, (int)enmSpd.CHK].ForeColor = method.cSpdCellImpact_Fore;
                }
                else
                {
                    ssExam.ActiveSheet.Cells[i, (int)enmSpd.CHK].BackColor = Color.White;
                    ssExam.ActiveSheet.Cells[i, (int)enmSpd.CHK].ForeColor = Color.Black;
                }
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true && e.Column == (int)enmSpd.CHK)
            {
                if (sender == this.btn_CHK_ALL_WS)
                {
                    this.isWSchk = true;
                    setCtrlSpreadChkWS();

                }
                else if (sender == this.btn_CHK_ALL_EQU)
                {
                    this.isEQUchk = true;
                    setCtrlSpreadChkEQU();
                }
                else if (sender == this.btn_CHK_ALL_EXAM)
                {
                    setCtrlSpreadChkEXAM();
                }

                return;
            }

            
            if (sender == this.ssWS)
            {
                string strChk;

                if (this.ssWS.ActiveSheet.Cells[e.Row, 2].Text.Substring(2, 1) == "0")
                {

                    if (this.ssWS.ActiveSheet.Cells[e.Row + 1, 0].Text == "True")
                    {
                        strChk = "";
                    }
                    else
                    {
                        strChk = "True";
                    }

                    for (int i = e.Row + 1; i < this.ssWS.ActiveSheet.Rows.Count; i++)
                    {
                        if (this.ssWS.ActiveSheet.Cells[i, 2].Text.Substring(2, 1) == "0")
                        {
                            break;
                        }
                        else
                        {

                            this.ssWS.ActiveSheet.Cells[i, 0].Text = strChk;
                        }
                    }
                }

                setCtrlSpread_EQU();
            }

        }

        void eBtnSave(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strWS = "";

            if (this.isWSchk == true)
            {
                for (int i = 0; i < this.ssWS.ActiveSheet.Rows.Count; i++)
                {
                    if (this.ssWS.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strWS += this.ssWS.ActiveSheet.Cells[i, 2].Text.Trim() + ",";
                    }
                }

                if (strWS.Length > 1)
                {
                    strWS = strWS.Substring(0, strWS.Length - 1);
                }
            }


            string strCODE = "";

            for (int i = 0; i < this.ssExam.ActiveSheet.Rows.Count; i++)
            {
                if (this.ssExam.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strCODE += this.ssExam.ActiveSheet.Cells[i, 2].Text.Trim() + ",";
                }
            }

            if (strCODE.Length > 1)
            {
                strCODE = strCODE.Substring(0, strCODE.Length - 1);
            }

            string strEQU = "";

            for (int i = 0; i < this.ssEQU.ActiveSheet.Rows.Count; i++)
            {
                if (this.ssEQU.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strEQU += this.ssEQU.ActiveSheet.Cells[i, 2].Text.Trim() + ",";
                }
            }

            if (strEQU.Length > 1)
            {
                strEQU = strEQU.Substring(0, strEQU.Length - 1);
            }

            string strNAME = "";

            for (int i = 0; i < this.ssExam.ActiveSheet.Rows.Count; i++)
            {
                if (this.ssExam.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strNAME += this.ssExam.ActiveSheet.Cells[i, 1].Text.Trim() + ",";
                }
            }

            if (strNAME.Length > 1)
            {
                strNAME = strNAME.Substring(0, strNAME.Length - 1);
            }

            string strEQU_NAME = "";

            for (int i = 0; i < this.ssEQU.ActiveSheet.Rows.Count; i++)
            {
                if (this.ssEQU.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strEQU_NAME += this.ssEQU.ActiveSheet.Cells[i, 1].Text.Trim() + ",";
                }
            }

            if (strEQU_NAME.Length > 1)
            {
                strEQU_NAME = strEQU_NAME.Substring(0, strEQU_NAME.Length - 1);
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                string SqlErr = string.Empty;
                string SQL = string.Empty;
                int intRowAffected = 0;
                int chkRow = 0;

                SqlErr = comSql.del_BAS_PCCONFIG_EQU(clsDB.DbCon, ref intRowAffected);

                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    method.setTranction(clsDB.DbCon, true, SqlErr, SQL, chkRow);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SqlErr = comSql.ins_BAS_PCCONFIG_WS(clsDB.DbCon, strWS, ref intRowAffected);

                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    method.setTranction(clsDB.DbCon, true, SqlErr, SQL, chkRow);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SqlErr = comSql.ins_BAS_PCCONFIG_EQU(clsDB.DbCon, strEQU, ref intRowAffected);

                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    method.setTranction(clsDB.DbCon, true, SqlErr, SQL, chkRow);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SqlErr = comSql.ins_BAS_PCCONFIG_CODE(clsDB.DbCon, strCODE, ref intRowAffected);

                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    method.setTranction(clsDB.DbCon, true, SqlErr, SQL, chkRow);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SqlErr = comSql.ins_BAS_PCCONFIG_NAME(clsDB.DbCon, strNAME, ref intRowAffected);

                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    method.setTranction(clsDB.DbCon, true, SqlErr, SQL, chkRow);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SqlErr = comSql.ins_BAS_PCCONFIG_EQU_NAME(clsDB.DbCon, strEQU_NAME, ref intRowAffected);

                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    method.setTranction(clsDB.DbCon, true, SqlErr, SQL, chkRow);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                method.setTranction(clsDB.DbCon, false, SqlErr, SQL, chkRow, false);
                Cursor.Current = Cursors.Default;

                this.Close();

            }
            catch (Exception ex)
            {

                Cursor.Current = Cursors.Default;
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message.ToString());
            }
        }

        void setCtrlSpreadChkWS()
        {
            if (this.isWSchk == false)
            {
                for (int i = 0; i < this.ssWS.ActiveSheet.Rows.Count; i++)
                {
                    this.ssWS.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "";
                }

                this.isWSchk = true;
            }
            else
            {
                for (int i = 0; i < this.ssWS.ActiveSheet.Rows.Count; i++)
                {

                    if (this.ssWS.ActiveSheet.Cells[i, (int)enmSpd.CODE].Text.Substring(2, 1) == "0")
                    {
                        this.ssWS.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "";
                    }
                    else
                    {
                        this.ssWS.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "True";
                    }
                }
                this.isWSchk = false;
            }

            //if (this.isWSchk == true)
            //{
                this.ssEQU.ActiveSheet.RowCount = 0;
                this.ssExam.ActiveSheet.RowCount = 0;
            //}

        }

        void setCtrlSpreadChkEXAM()
        {
            if (this.isExamChk == false)
            {
                for (int i = 0; i < this.ssExam.ActiveSheet.Rows.Count; i++)
                {
                    this.ssExam.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "";
                }

                this.isExamChk = true;
            }
            else
            {
                for (int i = 0; i < this.ssExam.ActiveSheet.Rows.Count; i++)
                {
                    this.ssExam.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "True";
                }
                this.isExamChk = false;
            }

        }

        void setCtrlSpreadChkEQU()
        {
            if (this.isEQUchk == false)
            {
                for (int i = 0; i < this.ssEQU.ActiveSheet.Rows.Count; i++)
                {
                    this.ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "";
                }

                this.isEQUchk = true;
            }
            else
            {
                for (int i = 0; i < this.ssEQU.ActiveSheet.Rows.Count; i++)
                {
                    this.ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "True";
                }
                this.isEQUchk = false;
            }

            setCtrlSpread_Exam();
        }

        void setCtrlSpread_WS()
        {
            string s = string.Empty;
            string s1 = string.Empty;

            if (ComFunc.isDataTableNull(this.gDt_WS)== false)
            {
                this.ssWS.ActiveSheet.Rows.Count = 0;
                this.ssWS.ActiveSheet.Rows.Count = this.gDt_WS.Rows.Count;
                for (int i = 0; i < this.gDt_WS.Rows.Count; i++)
                {
                    this.ssWS.ActiveSheet.Cells[i, (int)enmSpd.NAME].Text = this.gDt_WS.Rows[i][enmSpd.NAME.ToString()].ToString();
                    this.ssWS.ActiveSheet.Cells[i, (int)enmSpd.CODE].Text = this.gDt_WS.Rows[i][enmSpd.CODE.ToString()].ToString();

                    if (string.IsNullOrEmpty(this.gDt_WS.Rows[i][enmSpd.CODE.ToString()].ToString()) == false && (this.gDt_WS.Rows[i][enmSpd.CODE.ToString()].ToString().Substring(2, 1) == "0"))
                    {
                        spread.setColStyle(this.ssWS, i, (int)enmSpd.CHK, clsSpread.enmSpdType.Label);
                        ssWS.ActiveSheet.Cells[i, 0, i, this.ssWS.ActiveSheet.ColumnCount-1].BackColor = Color.FromArgb(192, 255, 255);
                    }
                    else
                    {
                        spread.setColStyle(this.ssWS, i, (int)enmSpd.CHK, clsSpread.enmSpdType.CheckBox);
                        ssWS.ActiveSheet.Cells[i, 0, i, this.ssWS.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                    }
                }
            }
        }

        void setCtrlSpread_EQU()
        {
            string strWSCODE = string.Empty;
            string strEQU = setCtrlSpread_EQU_Where(ref strWSCODE);            

            DataRow[] dr = null;


            if (string.IsNullOrEmpty(strWSCODE.Trim()) == true)
            {
                this.ssEQU.ActiveSheet.Rows.Count = 0;
                this.ssExam.ActiveSheet.Rows.Count = 0;
                return;
            }


            if (string.IsNullOrEmpty(strEQU) == false)
            {
                dr = this.gDt_EQU.Select("CODE IN (" + strEQU + ")");
                this.ssEQU.ActiveSheet.RowCount = 0;
                this.ssEQU.ActiveSheet.RowCount = dr.Length;
                for (int i = 0; i < dr.Length; i++)
                {
                    this.ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CODE].Text = dr[i][(int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_ALL.CODE].ToString();
                    this.ssEQU.ActiveSheet.Cells[i, (int)enmSpd.NAME].Text = dr[i][(int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_ALL.NAME].ToString();

                }
            }

            DataTable dt = lbExSQL.sel_EXAM_MASTER_EQU(clsDB.DbCon, strWSCODE, false, strEQU);

            if (ComFunc.isDataTableNull(dt) == true)
            {
                return;
            }
            this.ssExam.ActiveSheet.RowCount = 0;
            this.ssExam.ActiveSheet.RowCount = dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.ssExam.ActiveSheet.Cells[i, (int)enmSpd.CODE].Text = dt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_SPECODE_ALL.CODE.ToString()].ToString();
                this.ssExam.ActiveSheet.Cells[i, (int)enmSpd.NAME].Text = dt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_SPECODE_ALL.NAME.ToString()].ToString();
            }
        }

        string setCtrlSpread_EQU_Where(ref string strWSCODE)
        {            
            string strChk = string.Empty;
            string strCode = string.Empty;
            string strWhere = string.Empty;
            string[] arrEQU = null;

            DataRow[] dr = null;

            for (int i = 0; i < this.ssWS.ActiveSheet.Rows.Count; i++)
            {
                strChk = this.ssWS.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text;
                strCode = this.ssWS.ActiveSheet.Cells[i, (int)enmSpd.CODE].Text;
                

                if (string.IsNullOrEmpty(strChk) == false && strChk != "False")
                {
                    strWSCODE += ComFunc.covSqlstr(strCode,false) + ",";

                    dr = this.gDt_WS.Select("CODE = '" + strCode + "'");

                    if (dr != null && string.IsNullOrEmpty(dr[0][(int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_ALL.WSEQU].ToString()) == false)
                    {
                        arrEQU = dr[0][(int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_ALL.WSEQU].ToString().Split(',');
                        

                        for (int j = 0; j < arrEQU.Length; j++)
                        {
                            strWhere += ComFunc.covSqlstr(arrEQU[j], false) + ",";
                        }
                    }

                }

            }

            if (string.IsNullOrEmpty(strWhere) == false)
            {
                strWhere = strWhere.Substring(0, strWhere.Length - 1);
            }

            if (string.IsNullOrEmpty(strWSCODE) == false)
            { 
                strWSCODE = strWSCODE.Substring(0, strWSCODE.Length - 1);
            }
            return strWhere;    
        }

        void setCtrlSpread_Exam()
        {
            string strEQU = string.Empty;
            string strWSCODE = setCtrlSpread_Exam_Where(ref strEQU);

            if (string.IsNullOrEmpty(strEQU.Trim()) == true)
            {
                this.ssExam.ActiveSheet.Rows.Count = 0;
                return;
            }

            if (string.IsNullOrEmpty(strWSCODE) == false)
            {
                DataTable dt = lbExSQL.sel_EXAM_MASTER_EQU(clsDB.DbCon, strWSCODE, true, strEQU);
                if (ComFunc.isDataTableNull(dt) == true)
                {
                    return;
                }

                this.ssExam.ActiveSheet.RowCount = 0;
                this.ssExam.ActiveSheet.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.ssExam.ActiveSheet.Cells[i, (int)enmSpd.CODE].Text = dt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_SPECODE_ALL.CODE.ToString()].ToString();
                    this.ssExam.ActiveSheet.Cells[i, (int)enmSpd.NAME].Text = dt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_SPECODE_ALL.NAME.ToString()].ToString();
                }


            }
        }

        string setCtrlSpread_Exam_Where(ref string strEQU)
        {
            string strWSCODE = string.Empty;
            string strChk = string.Empty;
            string strCode = string.Empty;

            for (int i = 0; i < this.ssWS.ActiveSheet.Rows.Count; i++)
            {
                strChk = this.ssWS.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text;
                strCode = this.ssWS.ActiveSheet.Cells[i, (int)enmSpd.CODE].Text;


                if (string.IsNullOrEmpty(strChk) == false && strChk != "False")
                {
                    strWSCODE += ComFunc.covSqlstr(strCode, false) + ",";
                }

            }

            for (int i = 0; i < this.ssEQU.ActiveSheet.Rows.Count; i++)
            {
                strChk = this.ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text;
                strCode = this.ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CODE].Text;

                if (string.IsNullOrEmpty(strChk) == false && strChk != "False")
                {
                    strEQU += ComFunc.covSqlstr(strCode, false) + ",";
                }

            }

            if (string.IsNullOrEmpty(strWSCODE) == false)
            {
                strWSCODE = strWSCODE.Substring(0, strWSCODE.Length - 1);                
            }

            if (string.IsNullOrEmpty(strEQU) == false)
            {
                strEQU = strEQU.Substring(0, strEQU.Length - 1);
            }

            return strWSCODE;
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
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            // 5. sort, filter
            spread.setSpdFilter(spd, 0 , AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, 1, AutoFilterMode.EnhancedContextMenu, true);

            spread.setSpdSort(spd, 0, true);
            spread.setSpdSort(spd, 1, true);


            UnaryComparisonConditionalFormattingRule unary5;

            unary5 = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "True", false);
            unary5.BackColor = method.cSpdRowSumColor;
            unary5.ForeColor = method.cSpdCellImpact_Fore;

            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSpd.CHK, unary5);

        }
    }
}
