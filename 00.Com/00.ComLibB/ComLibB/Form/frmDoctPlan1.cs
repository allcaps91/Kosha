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
    /// File Name       : frmDocPlan1.cs
    /// Description     : 요일별 진료스케줄 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-09
    /// Update History  : try-catch문 수정
    /// <history>       
    /// D:\타병원\PSMHH\nurse\nropd\nropd11.frm(FrmDoctPlan1) => frmDocPlan1.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\nurse\nropd\nropd11.frm(FrmDoctPlan1)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\nurse\nropd\nropd.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmDoctPlan1 : Form
    {
        ComFunc CF = new ComFunc();

        string GstrHelpCode = "";
        string mstrJobSabun = "";
        string mstrIpAddress = "";
        string mstrJobPart = "";

        public frmDoctPlan1()
        {
            InitializeComponent();
        }

        public frmDoctPlan1(string GstrIpAddress, string GstrJobSabun, string GstrJobPart)
        {
            InitializeComponent();
            mstrIpAddress = GstrIpAddress;
            mstrJobSabun = GstrJobSabun;
            mstrJobPart = GstrJobPart;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmDoctPlan1_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            CF.FormInfo_History(clsDB.DbCon, this.Name, this.Text, mstrIpAddress, mstrJobSabun, mstrJobPart);

            ComboSet();           

            READ_SCH_Data();
        }

        void ComboSet()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DEPTCODE, DEPTNAMEK ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL += ComNum.VBLF + "WHERE DeptCode <> 'RD' ";
            if (GstrHelpCode != "")
            {
                if (VB.Len(GstrHelpCode) <= 2)
                {
                    SQL += ComNum.VBLF + "  AND DEPTCODE IN ";
                    SQL += ComNum.VBLF + "      ( SELECT DrDept1 FROM BAS_DOCTOR WHERE DrCode IN (" + GstrHelpCode + "))";
                }
            }
            SQL += ComNum.VBLF + "ORDER BY PRINTRANKING";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            cboDept.Items.Clear();

            cboDept.Items.Add("**.전체");

            for (i = 0; i < dt.Rows.Count; i++)
            {
                cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
            }

            cboDept.SelectedIndex = 0;

            dt.Dispose();
            dt = null;
        }

        void READ_SCH_Data()
        {
            int i, j;
            int nDay = 0;
            int nREAD = 0;
            int nCol = 0;

            string strGbn = "";
            string strGbn2 = "";
            string strGbn3 = "";
            string strDrCode = "";
            string strDeptCode = "";

            string SQL = "";
            string SqlErr = "";

            DataTable dt = null;
            DataTable dt2 = null;

            Set_Spread(ssSch_Sheet1);

            //진료과별 의사코드, 성명을 READ
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  A.DrDept1, A.DrCode, A.DrName ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B";
            SQL += ComNum.VBLF + "WHERE A.TOUR <> 'Y'";
            SQL += ComNum.VBLF + "  AND A.DrDept1 NOT IN ('HD','HR','PT','TO','R6')";
            SQL += ComNum.VBLF + "  AND A.DRDEPT1 = B.DEPTCODE";
            //TODO 부모폼의 이름을 읽어와야 할듯
            //If UCase(App.EXEName) = "NROPD" Then SQL = SQL & " AND A.DRCODE NOT IN ('0104') "
            if (this.Text.ToUpper() == "NROPD")
            {
                SQL += ComNum.VBLF + " AND A.DRCODE NOT IN ('0104') ";
            }
            if (VB.Left(cboDept.SelectedItem.ToString(), 2) != "**")
            {
                SQL += ComNum.VBLF + "AND A.DrDept1 ='" + VB.Left(cboDept.SelectedItem.ToString(), 2) + "' ";
            }
            SQL += ComNum.VBLF + "ORDER BY B.PRINTRANKING, A.DrDept1, A.PrintRanking";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            nREAD = dt.Rows.Count;
            ssSch_Sheet1.RowCount = nREAD;

            SqlErr = "";
            //의사별로 자료를 READ
            for (i = 0; i < ssSch_Sheet1.RowCount; i++)
            {
                strDeptCode = dt.Rows[i]["DrDept1"].ToString().Trim();
                strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();

                ssSch_Sheet1.Cells[i, 0].Text = strDeptCode;
                ssSch_Sheet1.Cells[i, 1].Text = strDrCode;
                ssSch_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DrName"].ToString().Trim();

                //요일별 스케줄을 READ
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  TO_CHAR(SchDate,'DD') ILJA,GbJin, GbJin2, GBJIN3  ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE";
                SQL += ComNum.VBLF + "WHERE DrCode = '" + strDrCode + "' ";
                SQL += ComNum.VBLF + "  AND SchDate >= TO_DATE('1990-01-01','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND SchDate <= TO_DATE('1990-01-06','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                for (j = 0; j < dt2.Rows.Count; j++)
                {
                    nDay = Convert.ToInt16(dt2.Rows[j]["ILJA"].ToString().Trim());

                    strGbn = dt2.Rows[j]["GbJin"].ToString().Trim();
                    strGbn2 = dt2.Rows[j]["GbJin2"].ToString().Trim();
                    strGbn3 = dt2.Rows[j]["GbJin3"].ToString().Trim();

                    nCol = (i * 3);
                    //ssSch.Sheets[0].ColumnHeader.Cells[i, (nDay * 3) +1 ].Text = strGbn;
                    ssSch.Sheets[0].Cells[i, (nDay * 3)].Text = strGbn;
                    switch (strGbn)
                    {
                        case "1":
                            ssSch_Sheet1.Cells[i, (nDay * 3)].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                            break;
                        case "2":
                            ssSch_Sheet1.Cells[i, (nDay * 3)].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                            break;
                        case "3":
                            ssSch_Sheet1.Cells[i, (nDay * 3)].BackColor = ssColor_Sheet1.Cells[0, 2].BackColor;
                            break;
                        case "4":
                            ssSch_Sheet1.Cells[i, (nDay * 3)].BackColor = ssColor_Sheet1.Cells[0, 3].BackColor;
                            break;
                        case "9":
                            ssSch_Sheet1.Cells[i, (nDay * 3)].BackColor = ssColor_Sheet1.Cells[0, 3].BackColor;
                            break;
                        case "8":
                            ssSch_Sheet1.Cells[i, (nDay * 3)].BackColor = ssColor_Sheet1.Cells[0, 6].BackColor;
                            break;
                        case "F":
                            ssSch_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_13.BackColor;
                            break;
                        default:
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = ssColor_Sheet1.Cells[0, 3].BackColor;
                            break;
                    }

                    //ssSch.Sheets[0].ColumnHeader.Cells[j, (nDay * 3) + 1].Text = strGbn2;
                    ssSch.Sheets[0].Cells[i, (nDay * 3) + 1].Text = strGbn2;
                    switch (strGbn2)
                    {
                        case "1":
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                            break;
                        case "2":
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                            break;
                        case "3":
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = ssColor_Sheet1.Cells[0, 2].BackColor;
                            break;
                        case "4":
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = ssColor_Sheet1.Cells[0, 3].BackColor;
                            break;
                        case "9":
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = ssColor_Sheet1.Cells[0, 4].BackColor;
                            break;
                        case "8":
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = ssColor_Sheet1.Cells[0, 6].BackColor;
                            break;
                        case "F":
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = lbl_13.BackColor;
                            break;
                        default:
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = ssColor_Sheet1.Cells[0, 3].BackColor;
                            break;
                    }

                    //ssSch.Sheets[0].ColumnHeader.Cells[j, (nDay * 3) + 2].Text = strGbn3;
                    ssSch.Sheets[0].Cells[i, (nDay * 3) + 2].Text = strGbn3;
                    switch (strGbn3)
                    {
                        case "1":
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 2].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                            break;
                        default:
                            ssSch_Sheet1.Cells[i, (nDay * 3) + 2].BackColor = ssColor_Sheet1.Cells[0, 3].BackColor;
                            break;
                    }
                }
                dt2.Dispose();
                dt2 = null;
            }
            dt.Dispose();
            dt = null;

        }

        void Set_Spread(FarPoint.Win.Spread.SheetView Spd)
        {
            int i = 0;
            int nCol = 0;

            Spd.RowCount = 0;

            Spd.AddColumnHeaderSpanCell(0, 0, 2, 1);
            Spd.ColumnHeader.Cells[0, 0].Value = "진료과";
            Spd.Columns[0].Width = 50;

            Spd.AddColumnHeaderSpanCell(0, 1, 2, 1);
            Spd.ColumnHeader.Cells[0, 1].Value = "의사";
            Spd.Columns[1].Width = 60;

            Spd.AddColumnHeaderSpanCell(0, 2, 2, 1);
            Spd.ColumnHeader.Cells[0, 2].Value = "의사성명";
            Spd.Columns[2].Width = 65;

            Spd.AddColumnHeaderSpanCell(0, 3, 1, 3);
            Spd.ColumnHeader.Cells[0, 3].Value = "월";

            Spd.AddColumnHeaderSpanCell(0, 6, 1, 3);
            Spd.ColumnHeader.Cells[0, 6].Value = "화";

            Spd.AddColumnHeaderSpanCell(0, 9, 1, 3);
            Spd.ColumnHeader.Cells[0, 9].Value = "수";

            Spd.AddColumnHeaderSpanCell(0, 12, 1, 3);
            Spd.ColumnHeader.Cells[0, 12].Value = "목";

            Spd.AddColumnHeaderSpanCell(0, 15, 1, 3);
            Spd.ColumnHeader.Cells[0, 15].Value = "금";

            Spd.AddColumnHeaderSpanCell(0, 18, 1, 3);
            Spd.ColumnHeader.Cells[0, 18].Value = "토";


            for (i = 1; i < 7; i++)
            {
                nCol = (i * 3) ;
                Spd.ColumnHeader.Cells[1, nCol].Value = "AM";
                Spd.ColumnHeader.Cells[1, nCol + 1].Value = "PM";
                Spd.ColumnHeader.Cells[1, nCol + 2].Value = "야";
            }
        }

        void ssSch_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strData = "";

            if (e.Column < 3)
            {
                return;
            }

            strData = ssSch_Sheet1.Cells[e.Row, e.Column].Text;

            if(e.Column + 1 % 3 != 0)
            {
                switch (strData)
                {
                    case "1":
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                        break;
                    case "2":
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                        break;
                    case "3":
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 2].BackColor;
                        break;
                    case "4":
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 3].BackColor;
                        break;
                    case "9":
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 4].BackColor;
                        break;
                    case "A":
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 5].BackColor;
                        break;
                    case "8":
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 6].BackColor;
                        break;
                    case "F":
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = lbl_13.BackColor;
                        break;
                    default:
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 3].BackColor;
                        ssSch_Sheet1.Cells[e.Row, e.Column].Text = "";
                        break;
                }
            }
            else
            {
                switch (strData)
                {
                    case "1":
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                        break;
                    default:
                        ssSch_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 3].BackColor;
                        ssSch_Sheet1.Cells[e.Row, e.Column].Text = "";
                        break;
                }
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            RegData();
        }

        void RegData()
        {
            int i, j;

            string strDrCode = "";
            string strDate = "";
            string strDay = "";
            string strGbn = "";
            string strGbn2 = "";
            string strGbn3 = "";

            string SQL = "";
            //DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                for (i = 0; i < ssSch_Sheet1.RowCount; i++)
                {
                    strDrCode = ssSch_Sheet1.Cells[i, 1].Text;
                    //기존의 자료는 삭제함
                    SQL = "";
                    SQL += ComNum.VBLF + "DELETE ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_SCHEDULE";
                    SQL += ComNum.VBLF + "WHERE DrCode = '" + strDrCode + "' ";
                    SQL += ComNum.VBLF + "  AND SchDate >= TO_DATE('1990-01-01','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  AND SchDate <= TO_DATE('1990-01-06','YYYY-MM-DD')";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                    //월~토요일까지의 내용을 INSERT
                    for (j = 1; j < 7; j++)
                    {
                        strDate = "1990-01-" + ComFunc.SetAutoZero(Convert.ToString(j), 2);
                        strDay = "1";

                        if (j == 12)
                        {
                            strDay = "2";
                        }
                        strGbn = ssSch_Sheet1.Cells[i, (j * 3) + 0].Text;
                        strGbn2 = ssSch_Sheet1.Cells[i, (j * 3) + 1].Text; 
                        strGbn3 = ssSch_Sheet1.Cells[i, (j * 3) + 2].Text;

                        SQL = "";
                        SQL += ComNum.VBLF + "INSERT INTO";
                        SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_SCHEDULE";
                        SQL += ComNum.VBLF + "(DRCODE, SCHDATE, GBDAY, GBJIN, GBJINEND, GBJIN2, GBJIN3 )";
                        SQL += ComNum.VBLF + "VALUES(";
                        SQL += ComNum.VBLF + "'" + strDrCode + "', ";
                        SQL += ComNum.VBLF + "TO_DATE('" + strDate + "','YYYY-MM-DD'), ";
                        SQL += ComNum.VBLF + "'" + strDay + "', ";
                        SQL += ComNum.VBLF + "'" + strGbn + "', ";
                        SQL += ComNum.VBLF + "' ', ";
                        SQL += ComNum.VBLF + "'" + strGbn2 + "', ";
                        SQL += ComNum.VBLF + "'" + strGbn3 + "'";
                        SQL += ComNum.VBLF + ") ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }                       
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                READ_SCH_Data();
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            READ_SCH_Data();
        }
    }
}
