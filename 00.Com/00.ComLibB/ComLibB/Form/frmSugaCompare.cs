using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSugaCompare.cs
    /// Description     : 표준수가와 비교하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : 델리게이트사용 -> NullReferenceException 오류발생
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\BuSuga25.frm(FrmSugaComPare) => frmSugaCompare.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\BuSuga25.frm(FrmSugaComPare)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmSugaCompare : Form
    {
        //이벤트를 전달할 경우
        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmSugaCompare()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            //this.Close();
            rEventClosed();
        }

        void frmSugaCompare_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR();
        }

        void SCREEN_CLEAR()
        {
            btnSearch.Enabled = true;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;

            int i, j;

            for(i = 0; i < ssSugaCompare_Sheet1.Rows.Count; i++)
            {
                for(j = 0; j < ssSugaCompare_Sheet1.Columns.Count; j++)
                {
                    ssSugaCompare_Sheet1.Cells[i, j].Text = "";
                }
            }
            ssSugaCompare.Enabled = false;            
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i = 0;

            string SQL = string.Empty;
            string SqlErr = "";
            DataTable dt = null;
            ssSugaCompare_Sheet1.RowCount = 0;

            ssSugaCompare.Enabled = true;
            btnCancel.Enabled = true;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "   A.BUN, A.NU, A.SUCODE, A.SUNEXT, A.SUNAMEK,A.HCODE, A.IAMT, A.TAMT,";
                SQL = SQL + ComNum.VBLF + "   A.BAMT, B.CODE, B.PRICE1,TO_CHAR(A.SUDATE,'YYYY-MM-DD') SUDATE, SPEC,";
                SQL = SQL + ComNum.VBLF + "   TO_CHAR(A.DELDATE,'YYYY-MM-DD') DELDATE, TO_CHAR(B.JDATE1,'YYYY-MM-DD') JDATE1,";
                SQL = SQL + ComNum.VBLF + "   A.BCODE, A.SUHAM";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE A, " + ComNum.DB_PMPA + "EDI_SUGA B";
                SQL = SQL + ComNum.VBLF + "WHERE A.SUGBF ='0'";
                SQL = SQL + ComNum.VBLF + " AND A.DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + " AND A.BCODE = B.CODE";
                SQL = SQL + ComNum.VBLF + " AND A.BAMT / DECODE(A.SUHAM, 0,1, A.SUHAM) - B.PRICE1 <> '0'";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.BUN, B.JDATE1 DESC , A.BUN, A.NU, A.SUNEXT";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssSugaCompare_Sheet1.RowCount = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSugaCompare_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Bun"].ToString().Trim();
                    ssSugaCompare_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Nu"].ToString().Trim() + " " + dt.Rows[i]["Spec"].ToString().Trim();
                    ssSugaCompare_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    ssSugaCompare_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssSugaCompare_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ssSugaCompare_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SUDATE"].ToString().Trim();

                    ssSugaCompare_Sheet1.Cells[i, 6].Text = String.Format("{0:#,##0}", dt.Rows[i]["BAmt"]);
                    ssSugaCompare_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SUHAM"].ToString().Trim();
                    ssSugaCompare_Sheet1.Cells[i, 8].Text = dt.Rows[i]["JDate1"].ToString().Trim();
                    ssSugaCompare_Sheet1.Cells[i, 9].Text = String.Format("{0:#,##0}", dt.Rows[i]["PRICE1"]);
                    ssSugaCompare_Sheet1.Cells[i, 10].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
        }

        void ssSugaCompare_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            ssSugaCompare_Sheet1.Cells[0, 0, ssSugaCompare_Sheet1.RowCount - 1, ssSugaCompare_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssSugaCompare_Sheet1.Cells[e.Row, 0, e.Row, ssSugaCompare_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

           if(e.Column == 2)
            {
                rSetHelpCode(ssSugaCompare_Sheet1.Cells[e.Row, 2].Text);
                rEventClosed();
            }
           
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strHead1 = "/l/f1" + VB.Space(35) + "약, 주사 표준수가 와 비교 LIST " + "/n/n";

            strFont2 = "/fn\"굴림체\" /fz\"10\" /fs2";
            strHead2 = "/l/f2" + "인쇄일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            strHead2 = strHead2 + VB.Space(90) + "PAGE : /p";

            ssSugaCompare_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;

            ssSugaCompare_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssSugaCompare_Sheet1.PrintInfo.Margin.Top = 50;
            ssSugaCompare_Sheet1.PrintInfo.Margin.Bottom = 2000;
            ssSugaCompare_Sheet1.PrintInfo.Margin.Left = 0;
            ssSugaCompare_Sheet1.PrintInfo.Margin.Right = 0;

            ssSugaCompare_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssSugaCompare_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;

            ssSugaCompare_Sheet1.PrintInfo.ShowBorder = true;
            ssSugaCompare_Sheet1.PrintInfo.ShowColor = true;
            ssSugaCompare_Sheet1.PrintInfo.ShowGrid = false;
            ssSugaCompare_Sheet1.PrintInfo.ShowShadows = true;
            ssSugaCompare_Sheet1.PrintInfo.UseMax = false;
            ssSugaCompare_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssSugaCompare_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSugaCompare_Sheet1.PrintInfo.Preview = true;
            ssSugaCompare.PrintSheet(0);
            
        }
    }
}
