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
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);

            this.ssWS.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssEQU.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssExam.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.ssWS.ButtonClicked += new EditorNotifyEventHandler(eSpreadBtnClick);
            this.ssEQU.ButtonClicked += new EditorNotifyEventHandler(eSpreadBtnClick);

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
            if ( e.ColumnHeader == true && e.Column == (int)enmSpd.CHK)
            {
                if (sender == this.ssWS)
                {
                    setCtrlSpreadChkWS();

                }
                else if (sender == this.ssEQU)
                {
                    setCtrlSpreadChkEQU();
                }
                else if (sender == this.ssExam)
                {
                    setCtrlSpreadChkEXAM();
                }
            }
            else if (e.ColumnHeader == true && e.Column != (int)enmSpd.CHK)
            {
                clsSpread.gSpdSortRow((FpSpread)sender, e.Column);
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

            spread.setColStyle(this.ssWS, -1, 2, clsSpread.enmSpdType.Hide);
            spread.setColStyle(this.ssEQU, -1, 2, clsSpread.enmSpdType.Hide);
            spread.setColStyle(this.ssExam, -1, 2, clsSpread.enmSpdType.Hide);

            setCtrlSpread_WS();


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

                    if (this.ssExam.ActiveSheet.Cells[i, (int)enmSpd.CODE].Text.Substring(2, 1) == "0")
                    {
                        this.ssExam.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "";
                    }
                    else
                    {
                        this.ssExam.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "True";
                    }
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

                    if (this.ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CODE].Text.Substring(2, 1) == "0")
                    {
                        this.ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "";
                    }
                    else
                    {
                        this.ssEQU.ActiveSheet.Cells[i, (int)enmSpd.CHK].Text = "True";
                    }
                }
                this.isEQUchk = false;
            }


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

            if (string.IsNullOrEmpty(strWSCODE) == false)
            {
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

        }
    }
}
