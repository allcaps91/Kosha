using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmNHicSub : Form
    {
        int FnTimerCNT = 0;
        string FstrSName = "";
        string FstrJumin = "";
        string FstrGJong = "";
        string FstrPANO = ""; //'외래번호
        string FstrGbn = "";
        string FstrYear = "";

        public frmNHicSub()
        {
            InitializeComponent();
        }

        private void frmNHicSub_Load(object sender, EventArgs e)
        {
            FnTimerCNT = 0;
            FstrSName = "";
            FstrJumin = "";
            FstrYear = "";
            FstrGbn = "H";

            timer1.Enabled = true;

            if (clsPublic.GstrRetValue != "")
            {
                FstrSName = VB.Trim(VB.Pstr(clsPublic.GstrRetValue, "^^", 1));
                FstrJumin = VB.Trim(VB.Pstr(clsPublic.GstrRetValue, "^^", 2));
                FstrGJong = VB.Trim(VB.Pstr(clsPublic.GstrRetValue, "^^", 3));
                FstrPANO = VB.Trim(VB.Pstr(clsPublic.GstrRetValue, "^^", 4));
                FstrGbn = VB.Trim(VB.Pstr(clsPublic.GstrRetValue, "^^", 5));
                FstrYear = VB.Trim(VB.Pstr(clsPublic.GstrRetValue, "^^", 6));
                if (FstrGbn != "C") FstrGbn = "H";
            }
            else
            {
                timer1.Enabled = false;
                this.Close();
            }

            if (FstrYear == "")
            {
                ComFunc.ReadSysDate(clsDB.DbCon);
                FstrYear = VB.Left(clsPublic.GstrSysDate, 4);
            }

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            FnTimerCNT = FnTimerCNT + 1;
            
            switch (FnTimerCNT)
            {
                case 1:
                case 2:
                    lblTime.Text = "1";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 3:
                case 4:
                    lblTime.Text = "1";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;
                case 5:
                case 6:
                    lblTime.Text = "1";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 7:
                case 8:
                    lblTime.Text = "2";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;
                case 9:
                case 10:
                    lblTime.Text = "2";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;

                case 11:
                case 12:
                    lblTime.Text = "2";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;
                case 13:
                case 14:
                    lblTime.Text = "3";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 15:
                case 16:
                    lblTime.Text = "3";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;
                case 17:
                case 18:
                    lblTime.Text = "3";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 19:
                case 20:
                    lblTime.Text = "4";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;                    
                case 21:
                case 22: lblTime.Text = "4";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 23:
                case 24: lblTime.Text = "4";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;
                case 25:
                case 26: lblTime.Text = "5";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 27:
                case 28: lblTime.Text = "5";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;
                case 29:
                case 30: lblTime.Text = "6";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 31:
                case 32: lblTime.Text = "6";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;                    
                case 33:
                case 34: lblTime.Text = "7";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 35:
                case 36: lblTime.Text = "7";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;
                case 37:
                case 38: lblTime.Text = "8";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 39:
                case 40: lblTime.Text = "8";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;                    
                case 41:
                case 42: lblTime.Text = "9";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 43:
                case 44: lblTime.Text = "9";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;
                case 45:
                case 46: lblTime.Text = "10";
                    lblTime.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 47:
                case 48: lblTime.Text = "10";
                    lblTime.BackColor = Color.FromArgb(255, 0, 255);
                    break;
            }
            
            if (FnTimerCNT >= 48)
            {
                FnTimerCNT = 0;
                this.Close();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'자격조회된 자료- 가장 최근자료
                SQL = "SELECT SName FROM ADMIN.WORK_NHIC ";
                SQL = SQL + ComNum.VBLF + " WHERE Jumin2='" + clsAES.AES(FstrJumin) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SNAME = '" + FstrSName + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CTime>=TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "   AND GBSTS ='1' ";
                if (FstrYear != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND Year='" + FstrYear + "' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY CTime DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    this.Close();
                    return;
                }
                else
                {
                    SQL = "SELECT SName FROM ADMIN.WORK_NHIC ";
                    SQL = SQL + ComNum.VBLF + "WHERE Jumin2='" + clsAES.AES(FstrJumin) + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND TRUNC(RTime)>=TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + "  AND GBSTS ='0' ";
                    if (FstrYear != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND Year='" + FstrYear + "' ";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY CTime DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt2.Rows.Count == 0)
                    {
                        //'한번도 조회한적이 없고 실패가 없는 수검자
                        SQL = "SELECT SName FROM ADMIN.WORK_NHIC ";
                        SQL = SQL + ComNum.VBLF + "WHERE Jumin2='" + clsAES.AES(FstrJumin) + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND TRUNC(RTime)>=TRUNC(SYSDATE) ";
                        SQL = SQL + ComNum.VBLF + "  AND GBSTS ='2' ";
                        if (FstrYear != "")
                        {
                            SQL = SQL + " AND Year='" + FstrYear + "' ";
                        }
                        SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt3.Rows.Count == 0)
                        {
                            SQL = " INSERT INTO WORK_NHIC (Rtime,Gubun,SName,Jumin,Jumin2,Pano,GBSTS,Year) VALUES ( ";
                            SQL = SQL + ComNum.VBLF + " SYSDATE,";
                            SQL = SQL + ComNum.VBLF + "'" + FstrGbn + "',";
                            SQL = SQL + ComNum.VBLF + "'" + FstrSName + "',";
                            SQL = SQL + ComNum.VBLF + "'" + VB.Left(FstrJumin, 7) + "******',";
                            SQL = SQL + ComNum.VBLF + "'" + clsAES.AES(FstrJumin) + "',";
                            SQL = SQL + ComNum.VBLF + "'" + FstrPANO + "',";
                            SQL = SQL + ComNum.VBLF + "'0',";
                            SQL = SQL + ComNum.VBLF + "'" + FstrYear + "') ";
                            
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        dt3.Dispose();
                        dt3 = null;
                    }
                    dt2.Dispose();
                    dt2 = null;
                }
                dt.Dispose();
                dt = null;
                
                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}
