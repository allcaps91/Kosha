using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaViewOutsideOderList : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaViewOutsideOderList.cs
        /// Description     : 원외처방내역 조회
        /// Author          : 김효성
        /// Create Date     : 2017-08-17
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "\IPD\ipdSim2\ipdsim2.vbp(Frm원외처방내역 조회) >> frmPmpaViewOutsideOderList.cs 폼이름 재정의" />	


        ComFunc CF = new ComFunc();

        string FstrPtno = "";
        string FstrBi = "";
        string FstrInDate = "";
        string FstrDEPT = "";
        string FstrSName = "";
        string FstrWard = "";
        string FstrRoom = "";
        string FstrDept = "";

        public frmPmpaViewOutsideOderList()
        {
            InitializeComponent();
        }

        public frmPmpaViewOutsideOderList(string strPano, string strBi, string strInDate, string strDeptCode, string strSname, string strWardCode, string strRoomCode)
        {
            InitializeComponent();

            FstrPtno = strPano;
            FstrBi = CF.Read_Bi_Name(clsDB.DbCon, strBi, "1");
            FstrInDate = strInDate;
            FstrDEPT = strDeptCode;
            FstrSName = strSname;
            FstrWard = strWardCode;
            FstrRoom = FstrWard + strRoomCode;
        }


        private void frmPmpaViewOutsideOderList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //SSSlip_Sheet1.RowCount = 0;
            //SSSlip_Sheet1.RowCount = 2;


            FstrPtno = clsPmpaType.TIT.Pano;
            FstrBi = clsPmpaType.TIT.Bi;
            FstrInDate = clsPmpaType.TIT.InDate;
            FstrDept = clsPmpaType.TIT.DeptCode;
            FstrSName = clsPmpaType.TIT.Sname;
            FstrWard = clsPmpaType.TIT.WardCode;
            FstrRoom = clsPmpaType.TIT.RoomCode;

            lblPano.Text = FstrPtno;
            lblFstrBi.Text = FstrBi;
            lblFstrDEPT.Text = FstrDEPT;
            lblFstrSName.Text = FstrSName;
            lblFstrInDate.Text = FstrInDate;
            lblFstrWard_FstrRoom.Text = FstrRoom;

            //SSSlip_Sheet1.Cells[0, 1].Text = FstrPtno;
            //SSSlip_Sheet1.Cells[0, 3].Text = FstrSName;
            //SSSlip_Sheet1.Cells[0, 5].Text = CF.Read_Bi_Name(clsDB.DbCon, FstrBi, "1");

            //SSSlip_Sheet1.Cells[1, 1].Text = FstrInDate;
            //SSSlip_Sheet1.Cells[1, 3].Text = FstrDept;
            //SSSlip_Sheet1.Cells[1, 5].Text = FstrWard + " / " + FstrRoom;
            Screen_display();
        }


        private void Screen_display()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strSEQNO = "";
            int i = 0;
            int j = 0;
            string strBDATE = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            strBDATE = Convert.ToDateTime(FstrInDate).ToString("yyyy-MM-dd");


            SSSlip_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT O.DeptCode, Sucode, SunameK, BaseAmt, Qty, Nal,";
                SQL = SQL + ComNum.VBLF + "       GbSpc, GbNgt, GbGisul, GbSelf, GbChild,";
                SQL = SQL + ComNum.VBLF + "       Amt1, Amt2, SeqNo, Part";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_SLIP O,  " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE BDate  >= TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND BDate  <  TO_DATE('" + Convert.ToDateTime(strBDATE).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Pano     = '" + FstrPtno + "' ";

                switch (FstrBi)
                {
                    case "11":
                    case "12":
                    case "13":
                        SQL = SQL + ComNum.VBLF + "    AND Bi IN ('11','12','13')          ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "    AND Bi = '" + FstrBi + "'                 ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "   AND O.BUN IN ('11','12','20') ";
                SQL = SQL + ComNum.VBLF + "   AND O.Part <> '#' ";//   '2016-07-14 외래 #조 제외요청 (심경순)
                SQL = SQL + ComNum.VBLF + "   AND O.Sunext = B.Sunext ";
                SQL = SQL + ComNum.VBLF + " ORDER BY  SeqNo, O.DeptCode, O.Bun, O.Sucode, O.Sunext";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                SSSlip_Sheet1.RowCount = dt.Rows.Count;
                SSSlip_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SSSlip_Sheet1.RowCount = i + 1;

                    if (strSEQNO != dt.Rows[i]["SEQNO"].ToString().Trim())
                    {
                        strSEQNO = dt.Rows[i]["SEQNO"].ToString().Trim();
                    }

                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 3].Text = (VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim())).ToString("##,###,##0");
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 4].Text = (VB.Val(dt.Rows[i]["Qty"].ToString().Trim())).ToString("#0.0");
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 5].Text = (VB.Val(dt.Rows[i]["Nal"].ToString().Trim())).ToString("##0");
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["GbChild"].ToString().Trim();
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 11].Text = (VB.Val(dt.Rows[i]["Amt1"].ToString().Trim())).ToString("##,###,##0");
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 12].Text = (VB.Val(dt.Rows[i]["Amt2"].ToString().Trim())).ToString("####,##0");
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 13].Text = (VB.Val(dt.Rows[i]["SeqNo"].ToString().Trim()).ToString("###0"));
                    SSSlip_Sheet1.Cells[SSSlip_Sheet1.RowCount - 1, 14].Text = dt.Rows[i]["Part"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
