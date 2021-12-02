using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmDrgHelp.cs
    /// Description     : DRG 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-14
    /// Update History  : try - catch 여러개 사용한 부분 수정, 델리게이트사용
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\BuSuga22.frm(FrmDrgHelp) => frmDrgHelp.cs 으로 변경함
    /// 실제 사용여부 확인 필요함
    /// </history>
    /// /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\BuSuga22.frm(FrmDrgHelp)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary> 
    public partial class frmDrgHelp : Form
    {
        //이벤트를 전달할 경우
        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmDrgHelp()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        void frmDrgHelp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Spread_Set();
        }

        void Spread_Set()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.RowCount = 0;
            Spread_Clear(ssList_Sheet1);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  CODE, NAME ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUN";
                SQL += ComNum.VBLF + "WHERE JONG ='2' ";
                SQL += ComNum.VBLF + "ORDER BY CODE";

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

                ssList_Sheet1.RowCount = dt.Rows.Count;
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = VB.Left(dt.Rows[i]["CODE"].ToString().Trim() + VB.Space(5), 5);
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void Spread_Clear(FarPoint.Win.Spread.SheetView Spd)
        {
            for(int i = 0; i < Spd.RowCount; i++)
            {
                for(int j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            rSetHelpCode(VB.Left(ssList_Sheet1.Cells[e.Row, e.Column].Text, 4));
            rEventClosed();
        }
    }
}
