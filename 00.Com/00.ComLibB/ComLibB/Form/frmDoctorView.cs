using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmDoctorView
    /// File Name : frmDoctorView.cs
    /// Title or Description : 의사 조회
    /// Author : 박창욱
    /// Create Date : 2017-06-01
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    ///     06-15 박창욱 : 디자인 수정, 공통함수 SetComboDept 적용
    /// </summary>
    /// <history>  
    /// VB\FrmDoctorView.frm(FrmDoctorView) -> frmDoctorView.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basefile\FrmDoctorview.frm(FrmDoctorView)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\nurse\nrstd\nrstd.vbp
    /// seealso : VB\PSMHH\exam\exinfect\exinfect.vbp
    /// </vbp>
    public partial class frmDoctorView : Form
    {
        public frmDoctorView()
        {
            InitializeComponent();
        }

        private void frmDoctorView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboDept.Items.Clear();
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "", 1);
            cboDept.SelectedIndex = 0;
            getSearch();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            getSearch();
        }

        private void getSearch()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "   SELECT A.DEPTCODE, B.DeptNamek, A.SABUN, A.DRNAME, A.DRBUNHO";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_DOCTOR A, KOSMOS_PMPA.BAS_CLINICDEPT B";
                SQL = SQL + ComNum.VBLF + " WHERE A.GBOUT ='N'";

                if (VB.Left(cboDept.SelectedItem.ToString(), 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.DEPTCODE = '" + VB.Left(cboDept.SelectedItem.ToString(), 2) + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = B.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY B.PRINTRANKING , A.DEPTCODE, A.DRNAME";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DRBUNHO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

       
    }
}
