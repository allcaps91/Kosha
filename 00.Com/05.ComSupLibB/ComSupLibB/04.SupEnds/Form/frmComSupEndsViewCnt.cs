using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComLibB;
using ComSupLibB.Com;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsViewCnt.cs
    /// Description     : 내시경관리 섬사 및 주사 건수 조회
    /// Author          : 윤조연
    /// Create Date     : 2017-07-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\ekg\endres20.frm(FrmView2) >> frmComSupEndsViewCnt.cs 폼이름 재정의" />
    public partial class frmComSupEndsViewCnt : Form
    {
        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupEndsSQL endsSql = new clsComSupEndsSQL();

        string gstrBuse = "";

        //시트정보
        enum enmExam     { ExName,Cnt        };
        string[] sSpdExam = {"검  사  명","수  량" };
        int[] nSpdExam = { 300,150};

        enum enmJusa { ExName, Cnt };
        string[] sSpdJusa = { "검  사  명", "수  량" };
        int[] nSpdJusa = { 300, 150 };


        public frmComSupEndsViewCnt(string argBuse)
        {
            InitializeComponent();

            gstrBuse = argBuse;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;



        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);


            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSearch1.Click += new EventHandler(eBtnEvent);
            

            //this.ssList.CellDoubleClick += ssList_CellDoubleClick;


        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                //
                setSpdExam(ssList, sSpdExam, nSpdExam, 1, 0);
                setSpdJusa(ssList2, sSpdJusa, nSpdJusa, 1, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();

                screen_clear();
            }
            
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }            
            else if (sender == this.btnSearch1)
            {
                //조회
                GetData_exam(clsDB.DbCon, ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());

                GetData_jusa(clsDB.DbCon, ssList2, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
            }

        }
               
        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            dtpFDate.Text = cpublic.strSysDate;
            dtpTDate.Text = cpublic.strSysDate;

        }

        void GetData_exam(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate)
        {

            int i = 0;
            DataTable dt = null;
            long nTot = 0;
            

            Spd.ActiveSheet.RowCount = 0;

            //쿼리실행      
            dt = endsSql.sel_ViewCnt(pDbCon, "1",argSDate, argTDate,gstrBuse);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    Spd.ActiveSheet.Cells[i, (int)enmExam.ExName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmExam.Cnt].Text = dt.Rows[i]["Count"].ToString().Trim();

                    nTot += Convert.ToInt16(dt.Rows[i]["Count"].ToString().Trim());
                }
            }

            Spd.ActiveSheet.RowCount = dt.Rows.Count+1;
            Spd.ActiveSheet.Cells[i, (int)enmExam.ExName].Text = "    합      계";
            Spd.ActiveSheet.Cells[i, (int)enmExam.Cnt].Text = nTot.ToString();

            dt.Dispose();
            dt = null;
            
            #endregion


            Cursor.Current = Cursors.Default;

        }

        void GetData_jusa(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate)
        {

            int i = 0;
            DataTable dt = null;
            long nTot = 0;

            Spd.ActiveSheet.RowCount = 0;

            //쿼리실행      
            dt = endsSql.sel_ViewCnt(pDbCon, "2", argSDate, argTDate,gstrBuse);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    Spd.ActiveSheet.Cells[i, (int)enmJusa.ExName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmJusa.Cnt].Text = dt.Rows[i]["Count"].ToString().Trim();

                    nTot += Convert.ToInt16(dt.Rows[i]["Count"].ToString().Trim());

                }
            }

            Spd.ActiveSheet.RowCount = dt.Rows.Count + 1;
            Spd.ActiveSheet.Cells[i, (int)enmJusa.ExName].Text = "    합      계";
            Spd.ActiveSheet.Cells[i, (int)enmJusa.Cnt].Text = nTot.ToString();

            dt.Dispose();
            dt = null;

            #endregion


            Cursor.Current = Cursors.Default;

        }
        
        void setSpdExam(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmExam)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmExam.Pano, clsSpread.enmSpdType.Text);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmExam.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmExam.DrCode, clsSpread.enmSpdType.Hide);
            

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmExam.SName, true);
            //methodSpd.setSpdFilter(spd, (int)enmExam.Pano, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }

        void setSpdJusa(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmJusa)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmExam.Pano, clsSpread.enmSpdType.Text);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmJusa.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmJusa.DrCode, clsSpread.enmSpdType.Hide);
            

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmJusa.SName, true);
            //methodSpd.setSpdFilter(spd, (int)enmJusa.Pano, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }
    }
}
