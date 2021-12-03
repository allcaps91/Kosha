using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmJunwontonggeylist : Form
    {
        //글로발 변수
        string GstrSysDate = "";
        public frmJunwontonggeylist()
        {
            InitializeComponent();
        }
        private void frmJunwontonggeylist_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            cboDect.Items.Clear();
            cboDect.Items.Add("전체");
            cboDect.Items.Add("입원");
            cboDect.Items.Add("외래");
            cboDect.SelectedIndex = 0;

            //strDate = VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), 4);

            txtSDATE.Text = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            txtSDATE.Text = GstrSysDate;
            
            // mnuView_Click ??
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSuch_Click(object sender, EventArgs e)
        {
            ReadDATA();
        }
        private void ReadDATA()
        {
            int i = 0;
            int intnREAD = 0;
            ssView_Sheet1.RowCount = 0;
            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";
            DataTable dt = null;

            try
            {
                SQL = " SELECT PANO, DEPTCODE, SEX, INTIME, OUTTIME, HOSPITAL, SAYU1, SAYU2, DISE1, DISE2, DISE3, GUBUN";
                SQL = SQL + ComNum.VBLF + " FROM (";
                SQL = SQL + ComNum.VBLF + "      SELECT PANO, DEPTCODE, SEX,";
                SQL = SQL + ComNum.VBLF + "       INTIME, OUTTIME, HUSONGNAME HOSPITAL, HUSONGSAYU SAYU1, '' SAYU2, DISEASE DISE1, '' DISE2, '' DISE3, 'ER' GUBUN";
                SQL = SQL + ComNum.VBLF + "      From ADMIN.NUR_ER_PATIENT";
                SQL = SQL + ComNum.VBLF + " WHERE OUTTIME >= TO_DATE('" + txtSDATE.Text + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "      AND OUTTIME <= TO_DATE('" + txtEDATE.Text + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "      AND OUTGBN = '6'";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT PANO, DEPTCODE, SEX, INDATE INTIME, TDATE OUTTIME, THOSNAME,";
                SQL = SQL + ComNum.VBLF + "      CASE";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUP1 = '1' THEN '연고지'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUP2 = '1' THEN '3차 병원 진료 원해'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUP3 = '1' THEN '기존 진료받던 병원'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUP4 = '1' THEN '지정병원'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUP5 = '1' THEN '지인(병원)'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUPETC IS NOT NULL THEN '기타'";
                SQL = SQL + ComNum.VBLF + "     END SAYU1,";
                SQL = SQL + ComNum.VBLF + "     CASE";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUH1 = '1' THEN '진료과 부재'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUH2A = '1' OR SAYUH2B = '1' OR SAYUH2C = '1' OR SAYUH2D = '1' OR SAYUH2E = '1' OR SAYUH2F = '1' OR SAYUH2G = '1' OR SAYUH2ETC IS NOT NULL THEN '응급검사/처치불가'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUH3 = '1' THEN '고위험환자로 3차병원 권유'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUH4 = '1' THEN '응급수술 지연'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUH5A = '1' OR SAYUH5B = '1' OR SAYUH5C = '1' OR SAYUH5ETC IS NOT NULL THEN '전문응급의료를 요하여'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUH6 = '1'  THEN '병동, 병실부족'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUH7 = '1'  THEN '중환자실 병동 부족'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUH8 = '1' THEN '연고지 관계'";
                SQL = SQL + ComNum.VBLF + "         WHEN SAYUHETC IS NOT NULL THEN '기타'";
                SQL = SQL + ComNum.VBLF + "     END SAYU2,";
                SQL = SQL + ComNum.VBLF + "  DIAG1, DIAG2, DIAG3, '입원' GUBUN";
                SQL = SQL + ComNum.VBLF + "   From ADMIN.NUR_QI_TRANSFOR";
                SQL = SQL + ComNum.VBLF + " WHERE TDATE >= TO_DATE('" + txtSDATE + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "      AND TDATE <= TO_DATE('" + txtEDATE + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "      )";

                if (cboDect.Text == "입원")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '입원'";
                }
                else if (cboDect.Text == "외래")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'ER'";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY OUTTIME, PANO";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                intnREAD = dt.Rows.Count;
                if (intnREAD > 0)
                {
                    ssView_Sheet1.Rows.Count = intnREAD;
                }
                for (i = 0; i > intnREAD; i++)
                {
                    ssView_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["GUBUN"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 2].Text = VB.Left (dt.Rows [i] ["OUTTIME"].ToString ().Trim () , 6);
                    ssView_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["PANO"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 4].Text = VB.Left (dt.Rows [i] ["INTIME"].ToString ().Trim () , 10);
                    ssView_Sheet1.Cells [i , 5].Text = dt.Rows [i] ["PANO"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 6].Text = dt.Rows [i] ["SEX"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 7].Text = dt.Rows [i] [""].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 8].Text = dt.Rows [i] ["DEPTCODE"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 9].Text = dt.Rows [i] ["HOSPITAL"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 10].Text = dt.Rows [i] ["SAYU1"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 11].Text = dt.Rows [i] ["SAYU2"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 12].Text = dt.Rows [i] ["DISE1"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 13].Text = dt.Rows [i] ["DISE2"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 13].Text = dt.Rows [i] ["DISE3"].ToString ().Trim ();
                    ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT + 20);
                }
                dt.Dispose ();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
