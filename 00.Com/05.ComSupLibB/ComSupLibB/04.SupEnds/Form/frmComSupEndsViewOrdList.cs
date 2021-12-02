using ComLibB;
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
    /// File Name       : frmComSupEndsViewOrdList.cs
    /// Description     : 내시경관리 오더완료 내역 조회
    /// Author          : 윤조연
    /// Create Date     : 2017-07-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\ekg\endres17.frm(FrmJuView) >> frmComSupEndsViewOrdList.cs 폼이름 재정의" />
    public partial class frmComSupEndsViewOrdList : Form
    {
        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupEndsSQL endsSql = new clsComSupEndsSQL();


        //시트정보
        enum enmOrdview    { BDate,Pano,SName,DeptCode,DrCode,
                             DrName,OrderCode,OrderName,DosCode,Qty,
                             Nal, STS,Chk};

        string[] sSpdOrdview = {"날짜","병록번호","성 명","과","의사",
                                  "의사명", "오더코드","오더명","용법","수량",
                                    "날수","상태","선택" };
        int[] nSpdOrdview = { 70,80,80,30,60,
                                 70,100,200,150,30,
                                 30,30,30};


        public frmComSupEndsViewOrdList()
        {
            InitializeComponent();

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
            this.btnPrint.Click += new EventHandler(eBtnEvent);

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
                setSpd(ssList, sSpdOrdview, nSpdOrdview, 1, 0);

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
            else if (sender == this.btnPrint)
            {
                ePrint();
            }
            else if (sender == this.btnSearch1)
            {
                //조회
                GetData(clsDB.DbCon, ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
            }

        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "내시경 주사 및 요청 내역 " + "(" + dtpFDate.Text + " - " + dtpTDate.Text + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            dtpFDate.Text = cpublic.strSysDate;
            dtpTDate.Text = cpublic.strSysDate;

        }

        void GetData(PsmhDb pDbCon,FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate)
        {

            int i = 0;
            DataTable dt = null;                                 

            Spd.ActiveSheet.RowCount = 0;

            //쿼리실행      
            dt = endsSql.sel_OrdView(pDbCon, argSDate, argTDate);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.BDate].Text = dt.Rows[i]["RDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.Pano].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.DeptCode].Text = dt.Rows[i]["Dept"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.DrName].Text = clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.DosCode].Text = dt.Rows[i]["DosName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.Qty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmOrdview.Nal].Text = dt.Rows[i]["Nal"].ToString().Trim();
                    if (dt.Rows[i]["DosName"].ToString().Trim()=="1")
                    {
                        Spd.ActiveSheet.Cells[i, (int)enmOrdview.STS].Text = "접수";
                    }
                    else if (dt.Rows[i]["DosName"].ToString().Trim() == "2")
                    {
                        Spd.ActiveSheet.Cells[i, (int)enmOrdview.STS].Text = "완료";
                    }
                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }
                
        void setSpd(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmOrdview)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmOrdview.Pano, clsSpread.enmSpdType.Text);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmOrdview.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmOrdview.DrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmOrdview.OrderCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmOrdview.Chk, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmOrdview.SName, true);
            //methodSpd.setSpdFilter(spd, (int)enmOrdview.Pano, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }
    }
}
