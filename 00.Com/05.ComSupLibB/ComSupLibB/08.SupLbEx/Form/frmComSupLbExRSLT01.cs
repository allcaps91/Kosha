using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupInfc
    /// File Name       : frmComSupInfcCODE01.cs
    /// Description     : Foot Note
    /// Author          : 김홍록
    /// Create Date     : 2017-06-22
    /// Update History  : 
    /// </summary>
    /// <history>       
    ///                 2017.06.26.김홍록 : 이름을 변경하는 루틴을 필요 없음.
    /// </history>
    /// <seealso cref= "d:\psmh\exam\excode\EXCODE13.frm" />
    public partial class frmComSupLbExRSLT01 : Form
    {

        clsComSupLbExSQL lbExSql = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        List<string> gStrFootNote;

        public delegate void ePSMH_RETURN(object sender, List<string> strFootNote);
        /// <summary>버튼을 클릭할경우 조회된 내용이 반영</summary>
        public event ePSMH_RETURN ePSMH_FootNote;

        /// <summary>생성자</summary>
        public frmComSupLbExRSLT01(List<string> strFootNote)
        {
            InitializeComponent();

            this.gStrFootNote = strFootNote;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnOk.Click += new EventHandler(eBtnClick);
            this.ss_EXAM_SPECODE.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnOk)
            {
                string[] arrSplite = new string[] { "\r\n" };
                string s = this.txtFootNote.Text;
                string[] strFootNote = s.Split(arrSplite, StringSplitOptions.None);

                List<string> lstFootNote = new List<string>();

                for (int i = 0; i < strFootNote.Length; i++)
                {
                    if (string.IsNullOrEmpty(strFootNote[i].Replace("\r\n", "").Trim()) == false)
                    {
                        lstFootNote.Add(strFootNote[i]);
                    }
                }

                ePSMH_FootNote(this, lstFootNote);

                this.Close();
            }

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            this.txtFootNote.Text += "\r\n" + this.ss_EXAM_SPECODE.ActiveSheet.Cells[e.Row, (int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_FOOTNOTE.NAME].Text;
        }

        void setCtrl()
        {

            for (int i = 0; i < this.gStrFootNote.Count; i++)
            {
                this.txtFootNote.Text += this.gStrFootNote[i] + "\r\n";
            }


            setCtrlSpread();
        }

        void setCtrlSpread()
        {
            DataSet ds = lbExSql.sel_EXAM_SPECODE_FOOTNOTE(clsDB.DbCon);

            setSpdStyle(this.ss_EXAM_SPECODE, ds,lbExSql.sSel_EXAM_SPECODE_FOOTNOTE, lbExSql.nSel_EXAM_SPECODE_FOOTNOTE);
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
        
            spread.setSpdFilter(spd, (int)clsComSupLbExSQL.enmSel_EXAM_SPECODE_FOOTNOTE.NAME, AutoFilterMode.EnhancedContextMenu, true);
            
        }

    }
}
