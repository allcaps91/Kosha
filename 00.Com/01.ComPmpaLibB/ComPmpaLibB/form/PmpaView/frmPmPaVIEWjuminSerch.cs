using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmPaVIEWjuminSerch.cs
    /// Description     : 주민번호로 등록번호 찾기
    /// Author          : 김효성
    /// Create Date     : 2017-09-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "psmh\IPD\iument\iument.vbp\Frm주민번호검색.frm  >> frmPmpaViewSlip2MirCheck.cs 폼이름 재정의" />	

    public partial class frmPmPaVIEWjuminSerch : Form
    {
        string GstrTempPano = "";
        string GstrSysDate = "";
        long GnJobSabun = 0;

        public frmPmPaVIEWjuminSerch(string strSysDate, long nJobSabun, string strTempPano)
        {
            GstrTempPano = strTempPano;
            GnJobSabun = nJobSabun;
            GstrSysDate = strSysDate;

            InitializeComponent();
        }

        public frmPmPaVIEWjuminSerch()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWjuminSerch_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }

        private void ChkJewon_CheckedChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int k = 0;
            int nRead = 0;
            string strToDate = "";
            string strNextDate = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            if (int.Parse(txtSName.Text) < 2)
            {
                if (rdoGubun0.Checked == true)
                {
                    ComFunc.MsgBox("재원자 검색시 등록번호(2자이상)을 넣고 조회하세요", "");
                }
                else
                {
                    ComFunc.MsgBox("재원자 검색시 성명(2자이상)을 넣고 조회하세요", "");
                }
            }

            if (int.Parse(txtSName.Text) >= 2)
            {
                strToDate = GstrSysDate;
                strNextDate = Convert.ToDateTime(strToDate).AddDays(1).ToString();

                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT * FROM " + ComNum.DB_PMPA + "BBASCD";  //BAS_BASCD




                    SQL = SQL + ComNum.VBLF + " SELECT a.DeptCode || ' ' || a.WardCode || ' ' || a.RoomCode info1, a.Pano,b.SName,b.Jumin1,b.Jumin2, b.Jumin3 ,b.PName,b.Sex,b.HPhone || ' ' || b.Tel Phone, ";
                    SQL = SQL + ComNum.VBLF + " b.ZipCode1,b.ZipCode2 ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "BAS_PATIENT b  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
                    SQL = SQL + ComNum.VBLF + "     AND a.Pano=b.Pano(+) ";
                    SQL = SQL + ComNum.VBLF + "     AND (a.OutDate IS NULL OR a.OutDate>=TO_DATE('" + strNextDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "     AND a.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";

                    if (rdoGubun0.Checked == true)
                    {
                        txtSName.Text = int.Parse(txtSName.Text).ToString("00000000");
                        SQL = SQL + ComNum.VBLF + "  AND a.Pano ='" + txtSName.Text + "' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND a.SName LIKE '%" + txtSName.Text + "%' ";
                    }
                    SQL = SQL + ComNum.VBLF + "  ORDER BY a.PANO ";

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

                    nRead = dt.Rows.Count;
                    //스프레드 출력문
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < nRead; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        //주민 암호화
                        if (dt.Rows[i]["Jumin3"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + dt.Rows[i]["Jumin2"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Phone"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = "입원정보:" + dt.Rows[i]["info1"].ToString().Trim();
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
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (GnJobSabun == 4349)
            {
            }
            else
            {
                if (txtJumin1.Text == "" || txtJumin2.Text == "")
                {
                    ComFunc.MsgBox("주민번호를 확인후 다시 검색하세요", "");
                    txtJumin1.Focus();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                //'주민암호화
                SQL = SQL + ComNum.VBLF + " SELECT Pano,SName,Jumin1,Jumin2, Jumin3 ,PName,Sex,HPhone || ' ' || Tel Phone, ";
                SQL = SQL + ComNum.VBLF + " ZipCode1,ZipCode2 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND JUMIN1='" + txtJumin1.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  JUMIN3='" + clsAES.AES(txtJumin2.Text) + "' ";

                if (GnJobSabun == 4349)
                {
                    SQL = SQL + ComNum.VBLF + "  OR Pano ='81000004' ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY PANO ";

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
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Phone"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = clsVbfunc.GetJuSo(clsDB.DbCon, dt.Rows[i]["ZipCode1"].ToString().Trim(), dt.Rows[i]["ZipCode2"].ToString().Trim());
                    //주민 암호화
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,SName,Jumin1,Jumin2, Jumin3 ,PName,Sex,HPhone || ' ' || Tel Phone, ";
                SQL = SQL + ComNum.VBLF + " ZipCode1,ZipCode2 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "  WHERE JUMIN1='" + txtJumin1.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  JUMIN2='" + txtJumin2.Text + "' ";
                if (GnJobSabun == 4349)
                {
                    SQL = SQL + ComNum.VBLF + "  OR Pano ='81000004' ";

                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[i]["Jumin2"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Phone"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = clsVbfunc.GetJuSo(clsDB.DbCon, dt.Rows[i]["ZipCode1"].ToString().Trim(), dt.Rows[i]["ZipCode2"].ToString().Trim());
                    }
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

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column > 0 && e.Row > 0)
            {
                if (ssView_Sheet1.Cells[e.Row, 0].Text != "")
                {
                    GstrTempPano = ssView_Sheet1.Cells[e.Row, 0].Text;
                }
            }

        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}

