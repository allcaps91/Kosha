using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 공휴일 등록
/// Author : 김형범
/// Create Date : 2017.06.19
/// </summary>
/// <history>
/// 완료
/// </history>
namespace ComLibB
{
    /// <summary> 공휴일 등록 </summary>
    public partial class frmSaveHoliday : Form
    {
        /// <summary> 공휴일 등록 </summary>
        public frmSaveHoliday()
        {
            InitializeComponent();
        }

        void frmSaveHoliday_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            //ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등           

            ssView_Sheet1.Columns[15].Visible = false;
            ssView_Sheet1.Columns[16].Visible = false;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            int intRow = 0;
            int intCol = 0;
            string strDate = string.Empty;
            string strFDate = string.Empty;
            string strTDate = string.Empty;
            string strLundar = string.Empty;
            int intInsertCNT = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                ssClear(ssView);

                strFDate = dtpDate.Value.ToString("yyyy-MM") + "-01";
                strTDate = dtpDate.Value.ToString("yyyy-MM") + "-" + DateTime.DaysInMonth(dtpDate.Value.Year, dtpDate.Value.Month);

                intInsertCNT = 0;

                for (i = 1; i <= VB.Val(VB.Right(strTDate, 2)); i++)
                {
                    if (i < 17)
                    {
                        intRow = i - 1;
                        intCol = 1 - 1;
                    }
                    else
                    {
                        intRow = i - 17;
                        intCol = 8;
                    }

                    strDate = VB.Left(strFDate, 8) + VB.Format(i, "00");
                    ssView_Sheet1.Cells[intRow, intCol].Text = VB.Format(i, "00");
                    ssView_Sheet1.Cells[intRow, intCol + 1].Text = VB.Left(clsVbfunc.GetYoIl(strDate), 3);

                    SQL = "";
                    SQL = "SELECT ROWID,HolyDay,OpdMagam,IpdMagam,ArcJob,TempHolyDay ";
                    SQL = SQL + ComNum.VBLF + " FROM BAS_JOB ";
                    SQL = SQL + ComNum.VBLF + "WHERE JobDate=TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[intRow, intCol + 2].Text = dt.Rows[0]["HolyDay"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, intCol + 3].Text = dt.Rows[0]["OpdMagam"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, intCol + 4].Text = dt.Rows[0]["IpdMagam"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, intCol + 5].Text = dt.Rows[0]["ArcJob"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, intCol + 6].Text = dt.Rows[0]["TempHolyDay"].ToString().Trim();

                        if (i < 17)
                        {
                            ssView_Sheet1.Cells[intRow, 15].Text = dt.Rows[0]["ROWID"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[intRow, 16].Text = dt.Rows[0]["ROWID"].ToString().Trim();
                        }
                    }
                    else
                    {
                        intInsertCNT = intInsertCNT + 1;

                        if (ssView_Sheet1.Cells[intRow, intCol].Text == "일")
                        {
                            ssView_Sheet1.Cells[intRow, intCol + 2].Text = "*";
                        }
                        else
                        {
                            switch (VB.Right(strDate, 5))
                            {
                                case "01-01":
                                case "01-02":
                                case "03-01":
                                case "04-05":
                                    ssView_Sheet1.Cells[intRow, intCol + 2].Text = "*";
                                    break;
                                case "05-05":
                                case "06-06":
                                case "07-17":
                                case "08-15":
                                    ssView_Sheet1.Cells[intRow, intCol + 2].Text = "*";
                                    break;
                                case "10-03":
                                case "12-25":
                                    ssView_Sheet1.Cells[intRow, intCol + 2].Text = "*";
                                    break;
                                default:
                                    strLundar =ComFunc.ToLunar(strDate);
                                    switch (VB.Right(strLundar, 5))
                                    {
                                        case "01-01":
                                        case "01-02":
                                            ssView_Sheet1.Cells[intRow, intCol + 2].Text = "*";
                                            break;
                                        case "04-08":
                                            ssView_Sheet1.Cells[intRow, intCol + 2].Text = "*";
                                            break;
                                        case "08-14":
                                        case "08-15":
                                        case "08-16":
                                            ssView_Sheet1.Cells[intRow, intCol + 2].Text = "*";
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                }

                if (intInsertCNT > 0)
                {
                    ComFunc.MsgBox("신규등록 입니다", "확인");
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 모듈 Spread Clear
        /// </summary>
        /// <param name="ssSpread"></param>
        void ssClear(FpSpread ssSpread)
        {
            ssSpread.ActiveSheet.Cells[0, 0, ssSpread.ActiveSheet.Rows.Count - 1, ssSpread.ActiveSheet.Columns.Count - 1].Text = string.Empty;
            ssSpread.ActiveSheet.SetActiveCell(0, 0);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            int intRow = 0;
            int intCol = 0;
            string strDate = string.Empty;
            string strFDate = string.Empty;
            string strTDate = string.Empty;

            string strHolyDay = string.Empty;
            string strOpdMagam = string.Empty;
            string strIpdMagam = string.Empty;
            string strArcJob = string.Empty;
            string strTempHoly = string.Empty;

            string strROWID = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                strFDate = dtpDate.Value.ToString("yyyy-MM").Trim() + "-01";
                strTDate = dtpDate.Value.ToString("yyyy-MM") + "-" + DateTime.DaysInMonth(dtpDate.Value.Year, dtpDate.Value.Month);

                for (i = 1; i <= VB.Val(VB.Right(strTDate, 2)); i++)
                {
                    if (i < 17)
                    {
                        intRow = i - 1;
                        intCol = 0;
                    }
                    else
                    {
                        intRow = i - 17;
                        intCol = 8;

                    }

                    strHolyDay = ssView_Sheet1.Cells[intRow, intCol + 2].Text;
                    strOpdMagam = ssView_Sheet1.Cells[intRow, intCol + 3].Text;
                    strIpdMagam = ssView_Sheet1.Cells[intRow, intCol + 4].Text;
                    strArcJob = ssView_Sheet1.Cells[intRow, intCol + 5].Text;
                    strTempHoly = ssView_Sheet1.Cells[intRow, intCol + 6].Text;

                    if (intCol == 0)
                    {
                        strROWID = ssView_Sheet1.Cells[intRow, 15].Text;
                    }
                    else
                    {
                        strROWID = ssView_Sheet1.Cells[intRow, 16].Text;
                    }

                    if (strROWID == "")
                    {
                        //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                        //{
                        //    return; //권한 확인
                        //}

                        strDate = VB.Left(strFDate, 8) + VB.Format(i, "00");

                        SQL = "INSERT INTO BAS_JOB (JobDate,HolyDay,OpdMagam,IpdMagam,ArcJob,TempHolyDay) ";
                        SQL = SQL + ComNum.VBLF + "VALUES (TO_DATE('" + strDate + "','YYYY-MM-DD'),'" + strHolyDay + "','";
                        SQL = SQL + ComNum.VBLF + strOpdMagam + "','" + strIpdMagam + "','" + strArcJob + "','" + strTempHoly + "') ";
                    }
                    else
                    {
                        //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                        //{
                        //    return; //권한 확인
                        //}

                        SQL = "UPDATE BAS_JOB SET HolyDay='" + strHolyDay + "',";
                        SQL = SQL + ComNum.VBLF + "OpdMagam='" + strOpdMagam + "',IpdMagam='" + strIpdMagam + "',";
                        SQL = SQL + ComNum.VBLF + "ArcJob='" + strArcJob + "', ";
                        SQL = SQL + ComNum.VBLF + "TempHolyDay ='" + strTempHoly + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + strROWID + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                //clsDB.setCommitTran(clsDB.DbCon);

                ssClear(ssView);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.Column < 3  || (e.Column >= 8 && e.Column <=10))
            {
                return;
            }

            if (ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim() == "")
            {
                ssView_Sheet1.Cells[e.Row, e.Column].Text = "*";
            }
            else
            {
                ssView_Sheet1.Cells[e.Row, e.Column].Text = "";
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
