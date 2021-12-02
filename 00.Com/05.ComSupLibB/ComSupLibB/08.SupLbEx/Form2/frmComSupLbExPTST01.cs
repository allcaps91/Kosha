using ComLibB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupLbExBST01.cs
    /// Description     : 검증 및 판독 상용구
    /// Author          : 김홍록
    /// Create Date     : 2017-05-15
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\exam\exveri\FrmCommonUse.frm" />
    public partial class frmComSupLbExPTST01 : Form
    {
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();

        /// <summary>상용구 분류</summary>
        public enum enmJong { Comments, Recommendation };

        enmJong gEnmJong;
        string gStrComments;

        string gstrUseCode;
        string gstrUseName;
        object gObject;

        DataTable gDt;
        DataTable gDt_Comments;
        DataTable gDt_ReCommend;

        public delegate void PSMH_RETURN_VALUE(object sender, string strUseName);
        /// <summary>이벤트헨들러</summary>
        public event PSMH_RETURN_VALUE ePSMH_RETURN_VALUE;

        public frmComSupLbExPTST01(enmJong pEnmJong, string pStrComments, object sender)
        {
            InitializeComponent();

            this.gEnmJong = pEnmJong;
            this.gStrComments = pStrComments;
            this.gObject = sender;

            setEvent();

        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClose);

            this.btnDelete.Click += new EventHandler(eBtnDelete);
            this.btnSave.Click += new EventHandler(eBtnSave);

            this.btnSelect.Click += new EventHandler(eBtnSelect);

            this.lv.ColumnClick += eLv_ColumnClick;
            this.lv.DoubleClick += new EventHandler(elv_DoubleClick);

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
            setCtrlTitle();
            setCtrlDt();
            setCtrlList();

        }

        void eBtnSave(object sender, EventArgs e)
        {
            int intRowAffected = 0;
            int chkRow = 0;

            if (string.IsNullOrEmpty(this.txtCode.Text.Trim()) == true || string.IsNullOrEmpty(this.txtCommetns.Text.Trim()) == true)
            {
                ComFunc.MsgBox("반드시 코드와 값을 입력 하세요");

                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            string SqlErr = string.Empty;
            string SQL = string.Empty;

            string strRowId = string.Empty;



            DataRow[] dr = null;

            if (this.gEnmJong == enmJong.Comments)
            {
                dr = this.gDt_Comments.Select("USECODE ='" + this.txtCode.Text.Trim()+ "'");
            }
            else if (this.gEnmJong == enmJong.Recommendation)
            {
                dr = this.gDt_ReCommend.Select("USECODE ='" + this.txtCode.Text.Trim() + "'");
            }

            if (dr != null && dr.Length > 0)
            {
                strRowId = dr[0][(int)clsComSupLbExSQL.enmSel_EXAM_VERIFYUSES.ROWID].ToString();

                if (string.IsNullOrEmpty(strRowId) == false)
                {
                    SqlErr = lbExSQL.up_EXAM_VERIFYUSE(clsDB.DbCon, strRowId,this.txtCommetns.Text.Trim(), ref intRowAffected);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    chkRow += 1;
                }
            }
            else
            {
                SqlErr = lbExSQL.ins_EXAM_VERIFYUSE(clsDB.DbCon, this.gEnmJong == enmJong.Comments ? "1" : "2", this.txtCode.Text, this.txtCommetns.Text.Trim(), ref intRowAffected);

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
                setCtrl();
            }
        }

        void eBtnDelete(object sender, EventArgs e)
        {
            int intRowAffected = 0;
            int chkRow = 0;

            if (string.IsNullOrEmpty(this.txtCode.Text.Trim()) == true)
            {
                ComFunc.MsgBox("삭제 대상을 선택하세요");

                return;
            }

            if (ComFunc.MsgBoxQ("해당 상용구를 삭제 하시겠습니까?","") == DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            string SqlErr = string.Empty;
            string SQL = string.Empty;

            string strRowId = string.Empty;



            DataRow[] dr = null;

            if (this.gEnmJong == enmJong.Comments)
            {
                dr = this.gDt_Comments.Select("USECODE ='" + this.txtCode.Text.Trim() + "'");
            }
            else if (this.gEnmJong == enmJong.Recommendation)
            {
                dr = this.gDt_ReCommend.Select("USECODE ='" + this.txtCode.Text.Trim() + "'");
            }

            if (dr!=null && dr.Length > 0)
            {
                strRowId = dr[0][(int)clsComSupLbExSQL.enmSel_EXAM_VERIFYUSES.ROWID].ToString();

                if (string.IsNullOrEmpty(strRowId)== false)
                {
                    SqlErr = lbExSQL.del_EXAM_VERIFYUSE(clsDB.DbCon, strRowId,  ref intRowAffected);

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
                    ComFunc.MsgBox(chkRow.ToString() + " 건을 삭제 하였습니다.");
                }
            }

            setCtrl();
        }

        void eBtnSelect(object sender, EventArgs e)
        {
            ePSMH_RETURN_VALUE(this.gObject, this.txtCommetns.Text);

            this.Close();
        }

        void eLv_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                lv.Columns[i].Text = lv.Columns[i].Text.Replace(" △", "");
                lv.Columns[i].Text = lv.Columns[i].Text.Replace(" ▽", "");
            }

            // DESC
            if (this.lv.Sorting == SortOrder.Ascending || lv.Sorting == SortOrder.None)
            {
                lv.Sorting = SortOrder.Descending;
                lv.Columns[e.Column].Text = lv.Columns[e.Column].Text + " ▽";
            }
            // ASC
            else
            {
                lv.Sorting = SortOrder.Ascending;
                lv.Columns[e.Column].Text = lv.Columns[e.Column].Text + " △";
            }
            lv.Sort();

            // 컬럼 갯수가 변경되는 구조라면 sorter를 null 처리하여야 함
            lv.ListViewItemSorter = null;
        }

        void elv_DoubleClick(object sender, EventArgs e)
        {            

            ListView.SelectedListViewItemCollection select = this.lv.SelectedItems;
            
            foreach (ListViewItem item in select)
            {
                this.gstrUseCode = item.SubItems[0].Text;                
            }

            this.txtCode.Text = this.gstrUseCode;
            DataRow[] dr = null;

            if (this.gEnmJong == enmJong.Comments)
            {
                dr = this.gDt_Comments.Select("USECODE = '" + this.gstrUseCode + "'");
            }
            else if (this.gEnmJong == enmJong.Recommendation)
            {
                dr = this.gDt_ReCommend.Select("USECODE = '" + this.gstrUseCode + "'");
            }

            this.gstrUseName = string.Empty;

            if (dr.Length > 0)
            {
                this.gstrUseName = dr[0][(int)clsComSupLbExSQL.enmSel_EXAM_VERIFYUSES.USENAME].ToString();
            }

            this.txtCommetns.Text = this.gstrUseName;
        }

        void eBtnClose(object sender, EventArgs e)
        {
            this.Close();
        }

        void setCtrlList()
        {
            DataTable dt = new DataTable();

            if (this.gEnmJong == enmJong.Comments)
            {
                dt = this.gDt_Comments.DefaultView.ToTable(false, new string[] { "USECODE" });
            }
            else if (this.gEnmJong == enmJong.Recommendation)
            {
                dt = this.gDt_ReCommend.DefaultView.ToTable(false, new string[] { "USECODE" });
            }

            setListView_Data(this.lv, dt,false, false);
        }

        void setListView_Data(ListView lst, DataTable dt, bool IsAdd, bool IsNull)
        {
            int n = 0;
            string s = null;

            if (IsAdd == false)
            {
                lst.Items.Clear();
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                n = dt.Rows.Count + 1;

                ListViewItem lv = null;

                if (IsNull == true) lst.Items.Add("");

                for (int i = 1; i < n; i++)
                {
                    lv = new ListViewItem();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j == 0)
                        {
                            s = dt.Rows[i - 1][j].ToString();
                            lv.Text = s;
                        }
                        else
                        {
                            s = dt.Rows[i - 1][j].ToString();
                            lv.SubItems.Add(s);
                        }
                    }
                    lst.Items.Add(lv);
                }
            }
        }
       
        void setCtrlTitle()
        {
            if (this.gEnmJong == enmJong.Comments)
            {
                this.lblTitle.Text = "검증/판독(Comments) 상용구";
                this.lblTitleSub0.Text = "검증/판독(Comments) 상용구 리스트";
            }
            else if (this.gEnmJong == enmJong.Recommendation)
            {
                this.lblTitle.Text = "추천(Recommendation) 상용구";
                this.lblTitleSub0.Text = "추천(Recommendation) 상용구 리스트";
            }

            this.txtCode.Text = "새로운 상용구";
            this.txtCommetns.Text = this.gStrComments;

        }

        void setCtrlDt()
        {
            string strJong = this.gEnmJong == enmJong.Comments ? "1" : "2";

            this.gDt = lbExSQL.sel_EXAM_VERIFYUSE(clsDB.DbCon, strJong);

            if (ComFunc.isDataTableNull(this.gDt) == false)
            {
                this.gDt_Comments = this.gDt.Clone();
                this.gDt_ReCommend = this.gDt.Clone();

                foreach (DataRow item in this.gDt.Select("JONG = '1'"))
                {
                    this.gDt_Comments.ImportRow(item);
                }

                foreach (DataRow item in this.gDt.Select("JONG = '2'"))
                {
                    this.gDt_ReCommend.ImportRow(item);
                }
            }
        }
        
    }
}
