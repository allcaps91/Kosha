using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 신규 입사자 등록
/// Author : 김형범
/// Create Date : 2017.06.16
/// </summary>
/// <history>
/// 조회 확인완료 인설트 업데이트 확인 필요
/// </history>
namespace ComLibB
{
    /// <summary> 신규입사자 인사마스타 기준으로 자동 형성 </summary>
    public partial class frmPassAuto : Form
    {
        /// <summary> 신규입사자 인사마스타 기준으로 자동 형성 </summary>
        public frmPassAuto()
        {
            InitializeComponent();
        }

        void frmPassAuto_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int intREAD = 0;
            int intRow = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            try
            {
                SQL = "";
                SQL = "SELECT Sabun,KorName,TO_CHAR(IpsaDay,'YYYY-MM-DD') IpsaDay,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(BalDay,'YYYY-MM-DD') BalDay ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL = SQL + ComNum.VBLF + "WHERE Sabun <= '600000' "; //영일제외
                SQL = SQL + ComNum.VBLF + "  AND ToiDay IS NULL "; //퇴사자제외
                SQL = SQL + ComNum.VBLF + "  AND IpsaDay >=TRUNC(SYSDATE-90) "; //90일이후에 입사자만

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

                intREAD = dt.Rows.Count;
                intRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    //비밀번호가 이미 등록되었는지 Check
                    SQL = "";
                    SQL = "SELECT IDnumber ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PASS ";
                    SQL = SQL + ComNum.VBLF + "WHERE IdNumber=" + VB.Val(dt.Rows[i]["Sabun"].ToString().Trim()) + " ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        if (intRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = intRow;
                        }

                        ssView_Sheet1.Cells[intRow, 0].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["KorName"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 2].Text = dt.Rows[i]["IpsaDay"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 3].Text = dt.Rows[i]["BalDay"].ToString().Trim();

                        intRow += 1;
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;
                ssView_Sheet1.RowCount = intRow;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnStart_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "C") == false || ComQuery.IsJobAuth(this, "U") == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            int j = 0;
            int intSabun = 0;
            string strKorName = "";
            string strName = "";
            string strSabun = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    intSabun = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 0].Text.Trim()));
                    strKorName = ssView_Sheet1.Cells[i, 1].Text;

                    strName = "";

                    for (j=0; j< VB.Len(strKorName); j++)
                    {
                        if (VB.Mid(strKorName, j, 1) != " ")
                        {
                            strName = strName + VB.Mid(strKorName, j, 1);
                        }
                    }

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_PASS (PROGRAMID,IDNUMBER,NAME,PASSWARD,CLASS,GRADE,CHARGE,PART)";
                    SQL = SQL + ComNum.VBLF + " VALUES (' '," + intSabun + ",'" + strName + "','" + intSabun + "',";
                    SQL = SQL + ComNum.VBLF + "'PMPA_IPD','ENTRY','0',' ') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    strSabun = VB.Format(intSabun, "00000");
                    
                    if (intSabun > 99999)
                    {
                        strSabun = VB.Format(intSabun, "000000");
                    }

                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_ERP + "INSA_MST SET WebSend='*' ";
                    SQL = SQL + ComNum.VBLF + " WHERE Sabun='" + strSabun + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");

                ComFunc.MsgBox("작업이 종료됨", "확인");

                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }
    }
}
