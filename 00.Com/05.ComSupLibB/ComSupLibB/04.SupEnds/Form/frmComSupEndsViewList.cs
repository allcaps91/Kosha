using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupFnExCopyList.cs
    /// Description     : 내시경관리 검사내역 결과 조회
    /// Author          : 윤조연
    /// Create Date     : 2017-06-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\ekg\ENDRES07.frm(FrmDaily) >> frmComSupEndsViewList.cs 폼이름 재정의" />
    public partial class frmComSupEndsViewList : Form
    {
        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupEndsSQL endsSql = new clsComSupEndsSQL();

        string gstrBuse = "";

        //시트정보
        enum enmResultview { BDate,No, SName, SexAge,Pano, Fiberscopy,Biopsy, Remark};

        string[] sSpdResultview = {"날짜","No","성 명","성별/나이","병록번호",
                                   "FiberScopy","Biopsy","Remark" };
        int[] nSpdResultview = { 80,30,70,50,70,
                                300,150,200 };

        public frmComSupEndsViewList(string argBuse)
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
                setSpd(ssList, sSpdResultview, nSpdResultview, 1, 0);

                ComFunc.SetFormInit(clsDB.DbCon,this, "Y", "Y", "Y"); //폼 기본값 세팅 등

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

        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            dtpFDate.Text = cpublic.strSysDate;
            dtpTDate.Text = cpublic.strSysDate;

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate)
        {

            int i = 0;
            int nCnt = 0;
            DataTable dt = null;            
            string strJob = "";
            string strDate = "";


            if (opt1.Checked == true)
            {
                strJob = "2";
            }
            else if (opt2.Checked == true)
            {
                strJob = "3";
            }
            else if (opt3.Checked == true)
            {
                strJob = "1";
            }
            else if (opt4.Checked == true)
            {
                strJob = "4";
            }

            Spd.ActiveSheet.RowCount = 0;

            //쿼리실행      
            dt = endsSql.sel_ResultView(pDbCon,strJob, argSDate, argTDate,gstrBuse);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if ( strDate != dt.Rows[i]["ResultDate"].ToString().Trim())
                    {
                        strDate = dt.Rows[i]["ResultDate"].ToString().Trim();
                        Spd.ActiveSheet.Cells[i, (int)enmResultview.BDate].Text = strDate;
                        if (txtNo.Text.Trim() !="") nCnt = Convert.ToInt16 ( txtNo.Text);
                        
                    }
                    Spd.ActiveSheet.Cells[i, (int)enmResultview.No].Text = nCnt.ToString();
                    Spd.ActiveSheet.Cells[i, (int)enmResultview.SexAge].Text = dt.Rows[i]["Sex"].ToString().Trim() + "/" +  dt.Rows[i]["BirthDate"].ToString().Trim(); //TODO 윤조연 Age_Gesan
                    Spd.ActiveSheet.Cells[i, (int)enmResultview.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmResultview.Pano].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    
                    if (strJob =="1" || strJob =="4")
                    {
                        Spd.ActiveSheet.Cells[i, (int)enmResultview.Fiberscopy].Text = dt.Rows[i]["Remark3"].ToString().Trim();//Bron
                        Spd.ActiveSheet.Cells[i, (int)enmResultview.Biopsy].Text = dt.Rows[i]["Remark4"].ToString().Trim();//Bron
                    }
                    else if (strJob == "2")
                    {
                        Spd.ActiveSheet.Cells[i, (int)enmResultview.Fiberscopy].Text = dt.Rows[i]["Remark4"].ToString().Trim();//위
                        Spd.ActiveSheet.Cells[i, (int)enmResultview.Biopsy].Text = dt.Rows[i]["Remark5"].ToString().Trim();//위
                    }
                    else if (strJob == "3")
                    {
                        Spd.ActiveSheet.Cells[i, (int)enmResultview.Fiberscopy].Text = dt.Rows[i]["Remark2"].ToString().Trim();//대장
                        Spd.ActiveSheet.Cells[i, (int)enmResultview.Biopsy].Text = dt.Rows[i]["Remark3"].ToString().Trim();//대장
                    }

                    if (strJob == "4")
                    {
                        Spd.ActiveSheet.Cells[i, (int)enmResultview.Remark].Text = dt.Rows[i]["Remark3"].ToString().Trim();
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)enmResultview.Remark].Text = (VB.Mid( dt.Rows[i]["Remark"].ToString().Trim(),1,50) + "\r\n" + VB.Mid(dt.Rows[i]["Remark"].ToString().Trim(), 51, 50) + "\r\n" + VB.Mid(dt.Rows[i]["Remark"].ToString().Trim(), 101, 50) ).Trim()  ;
                    }

                    nCnt++;
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
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmResultview)).Length;

            spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmOrderview.Pano, clsSpread.enmSpdType.Text);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmResultview.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmResultview.Bun, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmResultview.Gubun, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmResultview.GbIO, true);
            methodSpd.setSpdFilter(spd, (int)enmResultview.BDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }
    }
}
