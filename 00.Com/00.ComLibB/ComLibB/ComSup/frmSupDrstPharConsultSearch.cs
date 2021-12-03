using ComBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmSupDrstPharConsultSearch : Form
    {
        //이벤트를 전달할 경우
        public delegate void GetInfo(List<string> strDx);
        public event GetInfo rGetInfo;
        
        public frmSupDrstPharConsultSearch()
        {
            InitializeComponent();
        }

        private void frmSupDrstPharConsultSearch_Load(object sender, EventArgs e)
        {
            GetMedicinePart();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> temp = new List<string>();

            temp.Clear();
            for (int i = 0; i < ss2_Sheet1.RowCount; i++)
            {
                if (ss2_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                {
                    temp.Add(ss2_Sheet1.Cells[i, 1].Text.Trim());
                }
            }

            if (temp.Count > 3)
            {
                ComFunc.MsgBox("최대 선택 가능한 약품수는 3개 입니다.");
                return;
            }
            else
            {
                rGetInfo(temp);
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetMedicinePart()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PART, COMMENTS            ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.DRUG_SETCODE   ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '11'              ";
                SQL = SQL + ComNum.VBLF + "   AND JEPCODE = '분류명칭'      ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL           ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PART                    ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PART"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["COMMENTS"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;                
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
            }
        }

        private void GetMedicineList(string strPart)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                           ";
                SQL = SQL + ComNum.VBLF + "     A.PART, B.JEPCODE, C.SNAME, B.COMMENTS      ";
                SQL = SQL + ComNum.VBLF + " FROM                                            ";
                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM ADMIN.DRUG_SETCODE      ";
                SQL = SQL + ComNum.VBLF + "         WHERE GUBUN = '11'                      ";
                SQL = SQL + ComNum.VBLF + "             AND JEPCODE = '분류명칭'            ";
                SQL = SQL + ComNum.VBLF + "             AND DELDATE IS NULL                 ";
                SQL = SQL + ComNum.VBLF + "     ORDER BY PART) A,                           ";
                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM ADMIN.DRUG_SETCODE      ";
                SQL = SQL + ComNum.VBLF + "         WHERE GUBUN = '11'                      ";
                SQL = SQL + ComNum.VBLF + "             AND JEPCODE <> '분류명칭'           ";
                SQL = SQL + ComNum.VBLF + "             AND DELDATE IS NULL                 ";
                SQL = SQL + ComNum.VBLF + "     ORDER BY JEPCODE) B,                        ";
                SQL = SQL + ComNum.VBLF + "     ADMIN.OCS_DRUGINFO_NEW C               ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PART = B.PART                           ";
                SQL = SQL + ComNum.VBLF + "   AND B.JEPCODE = C.SUNEXT                      ";

                //2021-02-10 추가, 삭제된 코드 안보이도록
                SQL = SQL + ComNum.VBLF + "   AND C.SUNEXT NOT IN (                             ";
                SQL = SQL + ComNum.VBLF + "                           SELECT SUCODE             ";
                SQL = SQL + ComNum.VBLF + "                           FROM ADMIN.BAS_SUT  ";
                SQL = SQL + ComNum.VBLF + "                           WHERE 1=1                 ";
                SQL = SQL + ComNum.VBLF + "                             AND DELDATE IS NOT NULL "; 
                SQL = SQL + ComNum.VBLF + "                       )                             ";

                if (strPart != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.PART = '" + strPart + "'            ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY PART, JEPCODE                          ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss2_Sheet1.RowCount = dt.Rows.Count;
                    ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //ss2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PART"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["COMMENTS"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
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
            }
        }

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPart = ss1_Sheet1.Cells[e.Row, 0].Text.Trim();
            GetMedicineList(strPart);
        }
    }
}
