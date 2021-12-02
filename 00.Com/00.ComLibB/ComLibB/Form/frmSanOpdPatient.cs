using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSanOpdPatient
    /// File Name : frmSanOpdPatient.cs
    /// Title or Description : 산재 외래 통원환자 조회
    /// Author : 박창욱
    /// Create Date : 2017-06-02
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    ///     06-15 박창욱 : 전역변수 GstrSysDate, GstrJobMan 사용
    /// </summary>
    /// <history>  
    /// VB\busanid10.frm(FrmSanOpdPatient) -> frmSanOpdPatient.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busanid\busanid10.frm(FrmSanOpdPatient)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busanid\\busanid.vbp
    /// </vbp>
    public partial class frmSanOpdPatient : Form
    {
        private string gstrJobMan = "";

        public frmSanOpdPatient()
        {
            InitializeComponent();
        }

        public frmSanOpdPatient(string strJobMan)
        {
            InitializeComponent();
            gstrJobMan = strJobMan;
        }

        private void frmSanOpdPatient_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);
            ComFunc.Form_Center(this);
            screenClear();
        }


        private void screenClear()
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 30;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            screenClear();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string printDate = "";
            string jobMan = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            printDate = clsPublic.GstrSysDate;
            jobMan = gstrJobMan;

            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs0";
            strFont2 = "/fn\"바탕체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/f1" + VB.Space(10) + "산재 외래 통원 환자 현황 ( " + dtpDate.Value.ToString("yyyy-MM") + ")" + "/n" + "          " + "/n";
            strHead2 = "/l/f2" + "작업일자 : " + printDate + VB.Space(13) + "<의료기관명: 포 항 성 모 병 원>  " + VB.Space(20) + "PAGE : " + " /P";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 10;
            ssView_Sheet1.PrintInfo.Margin.Right = 10;
            ssView_Sheet1.PrintInfo.Margin.Top = 10;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 10;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt2 = null;

            int i = 0;
            string strSdate = "";
            string strEdate = "";

            screenClear();

            strSdate = dtpDate.Value.ToString("yyyy-MM") + "-01";
            strEdate = dtpDate.Value.ToString("yyyy-MM") + "-" + DateTime.DaysInMonth(dtpDate.Value.Year, dtpDate.Value.Month);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.Pano, A.Bi, A.Sname, A.Jumin1, A.Jumin2, A.CoprName,";

                if (optBun0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(B.BDATE,'YYYY-MM-DD') BDATE, ";
                }

                SQL = SQL + ComNum.VBLF + " B.DEPTCODE, B.DRCODE ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_SANID A, OPD_MASTER B";
                SQL = SQL + ComNum.VBLF + " WHERE B.PANO = A.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND B.BDATE >=TO_DATE('" + strSdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND B.BDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND B.BI='31'";
                SQL = SQL + ComNum.VBLF + "   AND B.JIN <>'2'  ";
                SQL = SQL + ComNum.VBLF + "    AND A.GBRESULT NOT IN('2','3','4')";

                if (optBun0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE,PANO";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.Pano, A.Bi, A.Sname, A.Jumin1, A.Jumin2, A.CoprName, B.DEPTCODE, B.DRCODE ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY PANO ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = "산재 ";
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + dt.Rows[i]["JUMIN2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                    //dt2
                    SQL = " SELECT DRNAME FROM BAS_DOCTOR ";
                    SQL = SQL + ComNum.VBLF + " WHERE DRCODE = '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "'";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[i, 5].Text = dt2.Rows[0]["DRNAME"].ToString().Trim();
                    }

                    dt2.Dispose();
                    dt2 = null;
                    //end dt2

                    if (optBun0.Checked == true)
                    {
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    }

                    //dt2
                    SQL = " SELECT MAX(OPDTODATE) OPDTODATE FROM BAS_SANDTL";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[i, 7].Text = VB.Left(dt2.Rows[0]["OPDTODATE"].ToString().Trim(), 4)
                                                 + "-" + VB.Mid(dt2.Rows[0]["OPDTODATE"].ToString().Trim(), 5, 2)
                                                 + "-" + VB.Right(dt2.Rows[0]["OPDTODATE"].ToString().Trim(), 2);
                    }

                    dt2.Dispose();
                    dt2 = null;
                    //end dt2

                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["CoprName"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
