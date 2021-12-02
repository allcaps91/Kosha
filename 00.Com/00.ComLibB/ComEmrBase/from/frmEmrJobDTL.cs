using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrJobDTL : Form
    {
        public frmEmrJobDTL()
        {
            InitializeComponent();
        }

        private void frmEmrJobDTL_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}
            //폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            Screen_Clear();
            GetDataList();
        }

        private void GetDataList()
        {
            ss1_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = 0;
            ss3_Sheet1.RowCount = 0;
            txtAllergy.Text = "";

            if (txtNo.Text.Trim() == "") { return; }

            int i = 0;
            string strPano = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                txtNo.Text = ComFunc.SetAutoZero(txtNo.Text, 8);

                strPano = txtNo.Text.Trim();

                lblPatient.Text = "";

                SQL = "";
                SQL = "SELECT PANO, SNAME, SEX, JUMIN1, JUMIN2";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtNo.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblPatient.Text = dt.Rows[i]["sName"].ToString().Trim() + " / " + dt.Rows[i]["Sex"].ToString().Trim() + " / " + dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + dt.Rows[i]["Jumin2"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                // 진단명 READ
                SQL = "";
                SQL = "SELECT 'O' IPDOPD, A.BDATE, A.ILLCODE,   B.ILLNAMEK ,C.DEPTCODE,  D.DRNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM   " + ComNum.DB_MED + "OCS_OILLS A, " + ComNum.DB_PMPA + "BAS_ILLS B , " + ComNum.DB_PMPA + "OPD_MASTER C, " + ComNum.DB_PMPA + "BAS_DOCTOR D";
                SQL = SQL + ComNum.VBLF + " WHERE   A.ILLCODE = B.ILLCODE ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO ='" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.SEQNO ='1'";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = C.PANO";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE = C.BDATE";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE =C.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "   AND C.DRCODE = D.DRCODE ";
                SQL = SQL + ComNum.VBLF + "UNION ALL     ";
                SQL = SQL + ComNum.VBLF + "SELECT 'I' IPDOPD, A.OUTDATE BDate, B.DIAGNOSIS1 illcode , C.ILLNAMEK, A.TDEPT DEPTCODE,  D.DRNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MID_SUMMARY A, " + ComNum.DB_PMPA + "MID_DIAGNOSIS B , " + ComNum.DB_PMPA + "BAS_ILLS C , " + ComNum.DB_PMPA + "BAS_DOCTOR D ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE = B.OUTDATE";
                SQL = SQL + ComNum.VBLF + "   AND B.SEQNO =1";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO ='" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND RTRIM(B.DIAGNOSIS1) = C.ILLCODE";
                SQL = SQL + ComNum.VBLF + "   AND A.TDOCTOR  = D.DRCODE ";
                SQL = SQL + ComNum.VBLF + "ORDER BY 2 DESC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["BDATE"].ToString().Trim(), "D");
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                // 수술명 READ
                SQL = "";
                SQL = "SELECT IPDOPD, OPDATE, DEPTCODE, DRCODE, OPTITLE , ANGBN , OPDOCT1 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ORAN_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY OPDATE DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ss2_Sheet1.RowCount = dt.Rows.Count;
                ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ss2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["OPDATE"].ToString().Trim(), "D");
                    ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["OPDOCT1"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["OPTITLE"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ANGBN"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                // 투약정보
                SQL = "";
                SQL = "SELECT 'O' IPDOPD, A.BDATE, A.DEPTCODE, A.SUNEXT, B.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP A, BAS_SUN B ";
                SQL = SQL + ComNum.VBLF + " WHERE  BUN IN ('11','12','20')";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.BDATE, A.DEPTCODE, A.SUNEXT, B.SUNAMEK";
                SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY * NAL) <> 0 ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT 'I' IPDOPD, A.BDATE,  A.DEPTCODE, A.SUNEXT, B.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP A, BAS_SUN B ";
                SQL = SQL + ComNum.VBLF + " WHERE  BUN IN ('11','12','20')";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO ='" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.BDATE, A.DEPTCODE, A.SUNEXT, B.SUNAMEK";
                SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY * NAL) <> 0 ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ss3_Sheet1.RowCount = dt.Rows.Count;
                ss3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ss3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["BDATE"].ToString().Trim(), "D");
                    ss3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //알러지 정보
                SQL = "";
                SQL = "SELECT A.PANO, A.SNAME, A.REMARK,  B.NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_ALLERGY_MST A, " + ComNum.DB_PMPA + "BAS_BCODE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.Code = B.Code";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN ='환자정보_알러지종류'";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + txtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY NAME ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    txtAllergy.Text = "[ Allergy  정보 ]----------------------------- " + ComNum.VBLF;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        txtAllergy.Text = txtAllergy.Text + "(" + (i + 1) + ") " + dt.Rows[i]["NAME"].ToString().Trim() + ComNum.VBLF;
                        if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                        {
                            txtAllergy.Text = txtAllergy.Text + "    ☞ " + dt.Rows[i]["REMARK"].ToString().Trim() + ComNum.VBLF;
                        }
                    }

                    txtAllergy.Text = txtAllergy.Text + "해당 환자는 위의 항목에 대해 Allergy가 있습니다.  " + ComNum.VBLF + ComNum.VBLF;
                    txtAllergy.Text = txtAllergy.Text + "------------------------------------------------ " + ComNum.VBLF;
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void txtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) 
                //{
                //    return;
                //}
                    
                GetDataList();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;
            //}
                
            GetDataList();
        }

        void Screen_Clear()
        {
            ss1_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = 0;
            ss3_Sheet1.RowCount = 0;
            txtAllergy.Text = "";
            txtNo.Text = "";
            lblPatient.Text = "";
        }

        private void txtNo_Enter(object sender, EventArgs e)
        {
            Screen_Clear();
        }
    }
}
