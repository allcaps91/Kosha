using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmOvicuPatientSearch
    /// File Name : frmOvicuPatientSearch.cs
    /// Title or Description : 집중치료실 조회 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-02
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmOvicuPatientSearch : Form
    {
        public frmOvicuPatientSearch()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 방번호 읽어서 병동 반환
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        private string ReadRoomCode(string argCode)
        {
            string rtnVal = "";

            switch (argCode)
            {
                case "233": rtnVal = "SICU"; break;
                case "234": rtnVal = "MICU"; break;
            }
            return rtnVal; 
        }

        /// <summary>
        /// 코드(병동명) 반환
        /// </summary>
        /// <param name="argCode">병동코드</param>
        /// <returns></returns>
        private string ReadWardCode(string argCode)
        {
            string rtnVal = "";

            switch (argCode)
            {
                case "33": rtnVal = "33(SICU)"; break;
                case "35": rtnVal = "35(MICU)"; break;
            }
            return rtnVal;
        }

        /// <summary>
        /// 의사명 반환
        /// </summary>
        /// <param name="argDate">의사코드</param>
        /// <returns></returns>
        private string ReadDrCode(string argCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "  SELECT DRNAME FROM BAS_DOCTOR ";
            SQL += ComNum.VBLF + "WHERE DRCODE = '" + argCode + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["Drname"].ToString();
            }
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        /// <summary>
        /// 코드로 주소 반환
        /// </summary>
        /// <param name="argCode">MAILCODE</param>
        /// <returns></returns>
        private string ReadZipCode(string argCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = " SELECT MAIlJUSO FROM BAS_MAIL ";
            SQL += ComNum.VBLF + "WHERE MAILCODE = '" + argCode + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return rtnVal;
            }
            if ( dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["MailJuso"].ToString();
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        private string ReadSex(string argDate)
        {
            string rtnVal = "";
            switch (argDate)
            {
                case "M": rtnVal = "남"; break;
                case "F": rtnVal = "여"; break;
            }
            return rtnVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string strDrCode = "";
            string strZipCode = "";
            string strRoomCode = "";
            string strWard = "";
            string strSex = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ";
                SQL += ComNum.VBLF + " b.RoomCode, b.Sname, b.Age, b.Sex, a.Jumin1, a.JuMin2,b.wardcode,  ";
                SQL += ComNum.VBLF + " a.Juso, b.DeptCode, b.DrCode, b.Pname, a.ZipCode1, a.ZipCode2, b.Pano ";
                SQL += ComNum.VBLF + " FROM BAS_PATIENT a, IPD_NEW_MASTER b ";
                SQL += ComNum.VBLF + " WHERE a.pano = b.pano ";
                SQL += ComNum.VBLF + " AND b.wardcode in ('33','35') ";
                SQL += ComNum.VBLF + " AND b.amset6 <> '*' ";
                SQL += ComNum.VBLF + " AND B.OUTDATE IS NULL ";
                SQL += ComNum.VBLF + " AND B.GBSTS = '0' ";
                SQL += ComNum.VBLF + " ORDER BY ROOMCODE, Sname ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }
                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("자료가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                ssIPD_Sheet1.Rows.Count = dt.Rows.Count;
                ssIPD_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strRoomCode = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strSex = dt.Rows[i]["Sex"].ToString().Trim();
                    strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                    strZipCode = dt.Rows[i]["ZipCode1"].ToString().Trim();
                    strZipCode += dt.Rows[i]["ZipCode2"].ToString().Trim();

                    if (strWard.Trim() != ReadWardCode(dt.Rows[i]["wardcode"].ToString().Trim()))
                    {
                        strWard = ReadWardCode(dt.Rows[i]["wardcode"].ToString().Trim());
                        ssIPD_Sheet1.Cells[i, 0].Text = strWard;
                    }

                    ssIPD_Sheet1.Cells[i, 0].Text = strWard;
                    ssIPD_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString();
                    ssIPD_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Age"].ToString();
                    ssIPD_Sheet1.Cells[i, 3].Text = ReadSex(strSex);
                    ssIPD_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Jumin1"].ToString() + "-" + dt.Rows[i]["Jumin2"].ToString();
                    ssIPD_Sheet1.Cells[i, 5].Text = ReadZipCode(strZipCode) + " " + dt.Rows[i]["Juso"].ToString();
                    ssIPD_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DeptCode"].ToString();
                    ssIPD_Sheet1.Cells[i, 7].Text = ReadDrCode(strDrCode);
                    ssIPD_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Pname"].ToString();

                    strWard = ReadWardCode(dt.Rows[i]["wardcode"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
                ssIPD.ResumeLayout();
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void frmOvicuPatientSearch_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }
    }
}
