using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrtong\nrtong18.frm >> frmTuksu.cs 폼이름 재정의" />


    public partial class frmTuksu : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();
        //string nFlag = "";

        public frmTuksu()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dtpDate.Select();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strNowDate = "";
            string strWardCode = "";

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 1;



            Cursor.Current = Cursors.WaitCursor;

            strNowDate = dtpDate.Text;
            strWardCode = ComboDept.Text.Trim();

            try
            {
                SQL = "";
                SQL = " SELECT A.PANO, A.SNAME, b.ROOM, B.REMARK, B.STIME, B.ROWID";
                SQL = SQL + ComNum.VBLF + " FROM bas_patient a, NUR_SPECIAL B ";// ',NUR_JINDAN C";

                if (strWardCode == "NR")
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE b.WARDCODE  IN ('IQ','NR')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE b.WARDCODE = '" + strWardCode + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "   AND B.ACTDATE = TO_DATE('" + strNowDate + "','YYYY-MM-DD')";

                if (OptJob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND B.GUBUN='1'";
                    //nFlag = "1";// '수술
                }
                if (OptJob1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND B.GUBUN='2'";
                    //nFlag = "2";//     '특수검사"
                }
                if (OptJob2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND B.GUBUN='3'";
                    //nFlag = "3";//     '전염병
                }
                if (OptJob3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND B.GUBUN='4'";
                    //nFlag = "4";//    '사망
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY b.ROOM, A.PANO";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }


                SS1_Sheet1.Rows.Count = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Room"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Stime"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void frmTuksu_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ComboDept.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT Code,Name FROM NUR_CODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun = '2' ";
                SQL = SQL + ComNum.VBLF + " AND Code in ('2W','3A','3B','4A','5W','6W','7W','8W','DR','NR','ND','MICU','SICU','ER','HU') ";//    '~j
                SQL = SQL + ComNum.VBLF + " ORDER By PrintRanking ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("부서가 설정되어 있지 않습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ComboDept.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                }

                ComboDept.SelectedIndex = 0;
                ComboDept.Enabled = true;

                dt.Dispose();
                dt = null;
                dtpDate.Value = Convert.ToDateTime(strDTP);

                SS1_Sheet1.Columns[0].Visible = false;
                SS1_Sheet1.Columns[6].Visible = false;
                SS1_Sheet1.Columns[7].Visible = false;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
