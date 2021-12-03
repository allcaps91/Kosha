using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewSecret.cs
    /// Description     : 사생활보호 대상자 명단 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iument\FrmSeCret.frm(FrmSeCret.frm) >> frmPmpaViewSecret.cs 폼이름 재정의" />	
    public partial class frmPmpaViewSecret : Form
    {
        clsPmpaFunc cpf = new clsPmpaFunc();

        public frmPmpaViewSecret()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano, Sname, Age || '/' || Sex as SAge,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(InDate,'YYYY-MM-DD') INDATE, DeptCode, DrCode,";
                SQL = SQL + ComNum.VBLF + "        WardCode, RoomCode, SeCret_Sabun,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(SeCretInDate,'YYYY-MM-DD HH24:MI') SeCretInDate,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(SeCretOutDate,'YYYY-MM-DD HH24:MI') SeCretOutDate";
                SQL = SQL + ComNum.VBLF + "  From " + ComNum.DB_PMPA + "IPD_NEW_MASTER   ";
                SQL = SQL + ComNum.VBLF + " Where 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND SeCret = '1' ";
                SQL = SQL + ComNum.VBLF + "   AND InDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND InDate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND JDate= TO_DATE('1900-01-01','YYYY-MM-DD') ";
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

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead;

                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nRow += 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SAge"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["SeCretInDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["SeCretOutDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = "";
                        if (cpf.Get_BasPatient(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()).Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 10].Text = cpf.Get_BasPatient(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()).Rows[0]["Gbinfor2"].ToString().Trim();
                        }
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["SeCret_Sabun"].ToString().Trim()) + " (" +
                                                                 clsVbfunc.GetSaBunBuSeName(clsDB.DbCon, dt.Rows[i]["SeCret_Sabun"].ToString().Trim()) + ")";
                    }
                }
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewSecret_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            dtpFDate.Value = Convert.ToDateTime(strSysDate).AddDays(-30);
            dtpTDate.Value = Convert.ToDateTime(strSysDate);
        }
    }
}
