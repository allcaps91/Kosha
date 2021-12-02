using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsVIEW05.cs
    /// Description     : 내시경용 건진 조직검사 수납여부 체크 
    /// Author          : 윤조연
    /// Create Date     : 2017-09-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\ocs\endo\endo_new\Frm건진수납여부.frm(Frm건진수납여부) >> frmComSupEndsVIEW05.cs 폼이름 재정의" />
    public partial class frmComSupEndsVIEW05 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsQuery Query = new clsQuery();
        clsComSupEndsSpd cendsSpd = new clsComSupEndsSpd();
        clsComSupEndsSQL cendsSql = new clsComSupEndsSQL();
        
        #endregion

        public frmComSupEndsVIEW05()
        {
            InitializeComponent();

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            
        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch1.Click += new EventHandler(eBtnSearch);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }            
            else
            {                                
                cendsSpd.sSpd_enmHicSunapView(ssList, cendsSpd.sSpdHicSunapView, cendsSpd.nSpdHicSunapView, 1, 0);
                
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();

                screen_clear();

            }


        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch1)
            {
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(o, e.Column);
                return;
            }
            
        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            dtpFDate.Text = cpublic.strSysDate;            

        }

        void screen_display()
        {
            //조회
            GetData(clsDB.DbCon,ssList, dtpFDate.Text.Trim());
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate)
        {

            int i = 0;  
            DataTable dt = null;
            DataTable dt2 = null;

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            string s = chkAll.Checked == true ? "ALL" : "";

            //쿼리실행               
            dt = cendsSql.sel_ENDO_JUPMST_HIC( pDbCon, argSDate, "HR", s);
           
            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    dt2 = cendsSql.sel_HIC_JEPSU_RESULT(pDbCon, dt.Rows[i]["JDate"].ToString().Trim(), dt.Rows[i]["Ptno"].ToString().Trim(), " 'TX24','TX25','TX37','TX21' ");
                    if (ComFunc.isDataTableNull(dt2) == false)
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmHicSunapView.HicWRTNO].Text = dt2.Rows[0]["HPano"].ToString().Trim();                 
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmHicSunapView.Sunap].Text = "수납";
                        Spd.ActiveSheet.Rows.Get(i).BackColor = method.cPaleGreen; 
                        
                    }
                    else
                    {                     
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmHicSunapView.Sunap].Text = "미수납";
                        Spd.ActiveSheet.Rows.Get(i).BackColor = Color.White;
                    }

                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmHicSunapView.Gubun].Text = exCode2Name(dt.Rows[i]["SuCode"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmHicSunapView.Ptno].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmHicSunapView.JepDate].Text = dt.Rows[i]["JDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmHicSunapView.SName].Text = dt.Rows[i]["SName"].ToString().Trim();                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmHicSunapView.ExJong].Text = clsVbfunc.GetBCODENameCode( pDbCon, "1", "ENDO_내시경분류", dt.Rows[i]["GbJob"].ToString().Trim()); 
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmHicSunapView.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim();
                                        

                }

                // 화면상의 정렬표시 Clear
                Spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, Spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

        string exCode2Name(string s)
        {
            
            if (s=="TX21")
            {
                return "(대장)조직";
            }
            else if (s == "TX24")
            {
                return "(위)조직";
            }
            else if (s == "TX25")
            {
                return "(위)미생물";
            }
            else if (s == "TX24TX25")
            {
                return "(위)조직+미생";
            }
            else
            {
                return "";
            }
            
        }


    }
}
