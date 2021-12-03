using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupMedLibB
{
    public partial class frmVolunteerApplication : Form
    {
        frmVolunteerApplicationTong frmVolunteerApplicationTongX;

        public frmVolunteerApplication()
        {
            InitializeComponent();
        }

        private void frmVolunteerApplication_Load(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strActiveNo = "";

            Cursor.Current = Cursors.WaitCursor;

            if (ComFunc.MsgBoxQ("저장하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (txtSeqno.Text == "")
                {
                    SQL = "SELECT KOSMOS_ADM.SEQ_SOCIAL_VOLUNTEER.NEXTVAL FROM DUAL";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strActiveNo = dt.Rows[0]["NEXTVAL"].ToString().Trim();
                    }

                    txtSeqno.Text = strActiveNo;

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "INSERT INTO KOSMOS_ADM.SOCIAL_VOLUNTEER";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "     SEQNO, SNAME, JUMIN1, JUMIN2, PHONE, HPHONE, RELIGION, SERENAME, SCHOOL, GRADE, BONDANG, CHUKDAY, BIRTHDAY, JUSO, ACTIVITYDAY, ACTIVITYTIME, VOLUNCAREER, VOLUNTEXT, VOLUNDONGI, INVITEROUTE, FAMILYNAME, FAMILYSERE, FAMILYREL, FAMILYAGE, FAMILYHAK, FAMILYJOB, FAMILYNAME1, FAMILYSERE1, FAMILYREL1, FAMILYAGE1, FAMILYHAK1, FAMILYJOB1, FAMILYNAME2, FAMILYSERE2, FAMILYREL2, FAMILYAGE2, FAMILYHAK2, FAMILYJOB2, FAMILYNAME3, FAMILYSERE3, FAMILYREL3, FAMILYAGE3, FAMILYHAK3, FAMILYJOB3, ETCDATA, DELYN";
                    SQL = SQL + ComNum.VBLF + ")";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "     '" + txtSeqno.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[3, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + VB.Left(ssView_Sheet1.Cells[3, 5].Text, 6) + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + VB.Right(ssView_Sheet1.Cells[3, 5].Text, 7) + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[4, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[4, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[5, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[5, 3].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[5, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[5, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[6, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[6, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[6, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[7, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[8, 3].Text + "',";

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[8, 4].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '1',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[8, 6].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '2',";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     '0',";
                    }

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[9, 2].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '1',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[9, 3].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '2',";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     '0',";
                    }

                    SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[9, 5].Text + "',";

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[10, 2].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '1',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[10, 4].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '2',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[10, 6].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '3',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[11, 2].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '4',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[11, 4].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '5',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[11, 6].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '6',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[12, 2].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '7',";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     '0',";
                    }

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[13, 2].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '1',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[13, 3].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '2',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[13, 4].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '3',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[13, 5].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '4',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[13, 6].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     '5',";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     '0',";
                    }

                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[15, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[15, 3].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[15, 4].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[15, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[15, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[15, 7].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[16, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[16, 3].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[16, 4].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[16, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[16, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[16, 7].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[17, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[17, 3].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[17, 4].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[17, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[17, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[17, 7].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[18, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[18, 3].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[18, 4].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[18, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[18, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[18, 7].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[19, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     'N'";
                    SQL = SQL + ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE KOSMOS_ADM.SOCIAL_VOLUNTEER SET";
                    SQL = SQL + ComNum.VBLF + "     SEQNO = '" + txtSeqno.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     SNAME = '" + ssView_Sheet1.Cells[3, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     JUMIN1 = '" + VB.Left(ssView_Sheet1.Cells[3, 5].Text, 6) + "',";
                    SQL = SQL + ComNum.VBLF + "     JUMIN2 = '" + VB.Right(ssView_Sheet1.Cells[3, 5].Text, 7) + "',";
                    SQL = SQL + ComNum.VBLF + "     PHONE = '" + ssView_Sheet1.Cells[4, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     HPHONE = '" + ssView_Sheet1.Cells[4, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     RELIGION = '" + ssView_Sheet1.Cells[5, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     SERENAME = '" + ssView_Sheet1.Cells[5, 3].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     SCHOOL = '" + ssView_Sheet1.Cells[5, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     GRADE = '" + ssView_Sheet1.Cells[5, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     BONDANG = '" + ssView_Sheet1.Cells[6, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     CHUKDAY = '" + ssView_Sheet1.Cells[6, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     BIRTHDAY = '" + ssView_Sheet1.Cells[6, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     JUSO = '" + ssView_Sheet1.Cells[7, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     ACTIVITYDAY = '" + ssView_Sheet1.Cells[8, 3].Text + "',";

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[8, 4].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     ACTIVITYTIME = '1',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[8, 6].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     ACTIVITYTIME = '2',";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     ACTIVITYTIME = '0',";
                    }

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[9, 2].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNCAREER = '1',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[9, 3].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNCAREER = '2',";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNCAREER = '0',";
                    }

                    SQL = SQL + ComNum.VBLF + "     VOLUNTEXT = '" + ssView_Sheet1.Cells[9, 5].Text + "',";

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[10, 2].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNDONGI = '1',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[10, 4].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNDONGI = '2',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[10, 6].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNDONGI = '3',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[11, 2].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNDONGI = '4',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[11, 4].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNDONGI = '5',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[11, 6].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNDONGI = '6',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[12, 2].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNDONGI = '7',";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     VOLUNDONGI = '0',";
                    }

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[13, 2].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     INVITEROUTE = '1',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[13, 3].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     INVITEROUTE = '2',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[13, 4].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     INVITEROUTE = '3',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[13, 5].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     INVITEROUTE = '4',";
                    }
                    else if (Convert.ToBoolean(ssView_Sheet1.Cells[13, 6].Value) == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     INVITEROUTE = '5',";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     INVITEROUTE = '0',";
                    }

                    SQL = SQL + ComNum.VBLF + "     FAMILYNAME = '" + ssView_Sheet1.Cells[15, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYSERE = '" + ssView_Sheet1.Cells[15, 3].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYREL = '" + ssView_Sheet1.Cells[15, 4].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYAGE = '" + ssView_Sheet1.Cells[15, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYHAK = '" + ssView_Sheet1.Cells[15, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYJOB = '" + ssView_Sheet1.Cells[15, 7].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYNAME1 = '" + ssView_Sheet1.Cells[16, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYSERE1 = '" + ssView_Sheet1.Cells[16, 3].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYREL1 = '" + ssView_Sheet1.Cells[16, 4].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYAGE1 = '" + ssView_Sheet1.Cells[16, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYHAK1 = '" + ssView_Sheet1.Cells[16, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYJOB1 = '" + ssView_Sheet1.Cells[16, 7].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYNAME2 = '" + ssView_Sheet1.Cells[17, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYSERE2 = '" + ssView_Sheet1.Cells[17, 3].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYREL2 = '" + ssView_Sheet1.Cells[17, 4].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYAGE2 = '" + ssView_Sheet1.Cells[17, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYHAK2 = '" + ssView_Sheet1.Cells[17, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYJOB2 = '" + ssView_Sheet1.Cells[17, 7].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYNAME3 = '" + ssView_Sheet1.Cells[18, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYSERE3 = '" + ssView_Sheet1.Cells[18, 3].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYREL3 = '" + ssView_Sheet1.Cells[18, 4].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYAGE3 = '" + ssView_Sheet1.Cells[18, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYHAK3 = '" + ssView_Sheet1.Cells[18, 6].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     FAMILYJOB3 = '" + ssView_Sheet1.Cells[18, 7].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     ETCDATA = '" + ssView_Sheet1.Cells[19, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     DELYN = 'N'";
                    SQL = SQL + ComNum.VBLF + "WHERE SEQNO = '" + txtSeqno.Text + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                Screen_Clear();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            //strHeader += CS.setSpdPrint_String("등록기간 : " + dtpFdate.Text + " ~ " + dtpTdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            //strHeader += CS.setSpdPrint_String("증빙서류 : " + cboBun.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            //strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both);
            CS = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (ComFunc.MsgBoxQ("삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE KOSMOS_ADM.SOCIAL_VOLUNTEER SET ";
                SQL = SQL + ComNum.VBLF + "     DELYN = 'Y'";
                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = '" + txtSeqno.Text + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Screen_Clear()
        {
            txtSeqno.Text = "";
            ssView_Sheet1.Cells[3, 2].Text = "";
            ssView_Sheet1.Cells[3, 5].Text = "";
            ssView_Sheet1.Cells[4, 2].Text = "";
            ssView_Sheet1.Cells[4, 5].Text = "";
            ssView_Sheet1.Cells[5, 2].Text = "";
            ssView_Sheet1.Cells[5, 3].Text = "";
            ssView_Sheet1.Cells[5, 5].Text = "";
            ssView_Sheet1.Cells[5, 6].Text = "";
            ssView_Sheet1.Cells[6, 2].Text = "";
            ssView_Sheet1.Cells[6, 5].Text = "";
            ssView_Sheet1.Cells[6, 6].Text = "";
            ssView_Sheet1.Cells[7, 2].Text = "";
            ssView_Sheet1.Cells[8, 3].Text = "";
            ssView_Sheet1.Cells[8, 4].Value = false;
            ssView_Sheet1.Cells[8, 6].Value = false;
            ssView_Sheet1.Cells[9, 2].Value = false;
            ssView_Sheet1.Cells[9, 3].Value = false;
            ssView_Sheet1.Cells[9, 5].Text = "";
            ssView_Sheet1.Cells[10, 2].Value = false;
            ssView_Sheet1.Cells[10, 4].Value = false;
            ssView_Sheet1.Cells[10, 6].Value = false;
            ssView_Sheet1.Cells[11, 2].Value = false;
            ssView_Sheet1.Cells[11, 4].Value = false;
            ssView_Sheet1.Cells[11, 6].Value = false;
            ssView_Sheet1.Cells[12, 2].Value = false;
            ssView_Sheet1.Cells[13, 2].Value = false;
            ssView_Sheet1.Cells[13, 3].Value = false;
            ssView_Sheet1.Cells[13, 4].Value = false;
            ssView_Sheet1.Cells[13, 5].Value = false;
            ssView_Sheet1.Cells[13, 6].Value = false;
            ssView_Sheet1.Cells[15, 2].Text = "";
            ssView_Sheet1.Cells[15, 3].Text = "";
            ssView_Sheet1.Cells[15, 4].Text = "";
            ssView_Sheet1.Cells[15, 5].Text = "";
            ssView_Sheet1.Cells[15, 6].Text = "";
            ssView_Sheet1.Cells[15, 7].Text = "";
            ssView_Sheet1.Cells[16, 2].Text = "";
            ssView_Sheet1.Cells[16, 3].Text = "";
            ssView_Sheet1.Cells[16, 4].Text = "";
            ssView_Sheet1.Cells[16, 5].Text = "";
            ssView_Sheet1.Cells[16, 6].Text = "";
            ssView_Sheet1.Cells[16, 7].Text = "";
            ssView_Sheet1.Cells[17, 2].Text = "";
            ssView_Sheet1.Cells[17, 3].Text = "";
            ssView_Sheet1.Cells[17, 4].Text = "";
            ssView_Sheet1.Cells[17, 5].Text = "";
            ssView_Sheet1.Cells[17, 6].Text = "";
            ssView_Sheet1.Cells[17, 7].Text = "";
            ssView_Sheet1.Cells[18, 2].Text = "";
            ssView_Sheet1.Cells[18, 3].Text = "";
            ssView_Sheet1.Cells[18, 4].Text = "";
            ssView_Sheet1.Cells[18, 5].Text = "";
            ssView_Sheet1.Cells[18, 6].Text = "";
            ssView_Sheet1.Cells[18, 7].Text = "";
            ssView_Sheet1.Cells[19, 2].Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            frmVolunteerApplicationTongX = new frmVolunteerApplicationTong();
            frmVolunteerApplicationTongX.rGetPatientInfo += new frmVolunteerApplicationTong.GetInfo(frmVolunteerApplicationTongX_rGetPatientInfo);
            frmVolunteerApplicationTongX.rEventClosed += new frmVolunteerApplicationTong.EventClosed(frmVolunteerApplicationTongX_rEventClosed);
            frmVolunteerApplicationTongX.ShowDialog();
            frmVolunteerApplicationTongX = null;
        }

        private void frmVolunteerApplicationTongX_rEventClosed()
        {
            frmVolunteerApplicationTongX.Dispose();
            frmVolunteerApplicationTongX = null;
        }

        private void frmVolunteerApplicationTongX_rGetPatientInfo(string strSeqno)
        {
            if (frmVolunteerApplicationTongX == null)
            {
                frmVolunteerApplicationTongX.Dispose();
                frmVolunteerApplicationTongX = null;
            }

            GetInfo(strSeqno);
        }

        private void GetInfo(string strSeqno)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM KOSMOS_ADM.SOCIAL_VOLUNTEER";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO = '" + strSeqno + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtSeqno.Text = strSeqno;
                    ssView_Sheet1.Cells[3, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[3, 5].Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + dt.Rows[0]["JUMIN2"].ToString().Trim();
                    ssView_Sheet1.Cells[4, 2].Text = dt.Rows[0]["PHONE"].ToString().Trim();
                    ssView_Sheet1.Cells[4, 5].Text = dt.Rows[0]["HPHONE"].ToString().Trim();
                    ssView_Sheet1.Cells[5, 2].Text = dt.Rows[0]["RELIGION"].ToString().Trim();
                    ssView_Sheet1.Cells[5, 3].Text = dt.Rows[0]["SERENAME"].ToString().Trim();
                    ssView_Sheet1.Cells[5, 5].Text = dt.Rows[0]["SCHOOL"].ToString().Trim();
                    ssView_Sheet1.Cells[5, 6].Text = dt.Rows[0]["GRADE"].ToString().Trim();
                    ssView_Sheet1.Cells[6, 2].Text = dt.Rows[0]["BONDANG"].ToString().Trim();
                    ssView_Sheet1.Cells[6, 5].Text = dt.Rows[0]["CHUKDAY"].ToString().Trim();
                    ssView_Sheet1.Cells[6, 6].Text = dt.Rows[0]["BIRTHDAY"].ToString().Trim();
                    ssView_Sheet1.Cells[7, 2].Text = dt.Rows[0]["JUSO"].ToString().Trim();
                    ssView_Sheet1.Cells[8, 3].Text = dt.Rows[0]["ACTIVITYDAY"].ToString().Trim();

                    if (dt.Rows[0]["ACTIVITYTIME"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[8, 4].Value = true;
                    }
                    else if (dt.Rows[0]["ACTIVITYTIME"].ToString().Trim() == "2")
                    {
                        ssView_Sheet1.Cells[8, 6].Value = true;
                    }
                    else if (dt.Rows[0]["ACTIVITYTIME"].ToString().Trim() == "0")
                    {
                        ssView_Sheet1.Cells[8, 4].Value = false;
                        ssView_Sheet1.Cells[8, 6].Value = false;
                    }

                    if (dt.Rows[0]["VOLUNCAREER"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[9, 2].Value = true;
                    }
                    else if (dt.Rows[0]["VOLUNCAREER"].ToString().Trim() == "2")
                    {
                        ssView_Sheet1.Cells[9, 3].Value = true;
                    }
                    else if (dt.Rows[0]["VOLUNCAREER"].ToString().Trim() == "0")
                    {
                        ssView_Sheet1.Cells[9, 2].Value = false;
                        ssView_Sheet1.Cells[9, 3].Value = false;
                    }

                    ssView_Sheet1.Cells[9, 5].Text = dt.Rows[0]["VOLUNTEXT"].ToString().Trim();

                    if (dt.Rows[0]["VOLUNDONGI"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[10, 2].Value = true;
                    }
                    else if (dt.Rows[0]["VOLUNDONGI"].ToString().Trim() == "2")
                    {
                        ssView_Sheet1.Cells[10, 4].Value = true;
                    }
                    else if (dt.Rows[0]["VOLUNDONGI"].ToString().Trim() == "3")
                    {
                        ssView_Sheet1.Cells[10, 6].Value = true;
                    }
                    else if (dt.Rows[0]["VOLUNDONGI"].ToString().Trim() == "4")
                    {
                        ssView_Sheet1.Cells[11, 2].Value = true;
                    }
                    else if (dt.Rows[0]["VOLUNDONGI"].ToString().Trim() == "5")
                    {
                        ssView_Sheet1.Cells[11, 4].Value = true;
                    }
                    else if (dt.Rows[0]["VOLUNDONGI"].ToString().Trim() == "6")
                    {
                        ssView_Sheet1.Cells[11, 4].Value = true;
                    }
                    else if (dt.Rows[0]["VOLUNDONGI"].ToString().Trim() == "7")
                    {
                        ssView_Sheet1.Cells[11, 6].Value = true;
                    }
                    else if (dt.Rows[0]["VOLUNDONGI"].ToString().Trim() == "0")
                    {
                        ssView_Sheet1.Cells[10, 2].Value = false;
                        ssView_Sheet1.Cells[10, 4].Value = false;
                        ssView_Sheet1.Cells[10, 6].Value = false;
                        ssView_Sheet1.Cells[11, 2].Value = false;
                        ssView_Sheet1.Cells[11, 4].Value = false;
                        ssView_Sheet1.Cells[11, 6].Value = false;
                        ssView_Sheet1.Cells[12, 2].Value = false;
                    }

                    if (dt.Rows[0]["INVITEROUTE"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[13, 2].Value = true;
                    }
                    else if (dt.Rows[0]["INVITEROUTE"].ToString().Trim() == "2")
                    {
                        ssView_Sheet1.Cells[13, 3].Value = true;
                    }
                    else if (dt.Rows[0]["INVITEROUTE"].ToString().Trim() == "3")
                    {
                        ssView_Sheet1.Cells[13, 4].Value = true;
                    }
                    else if (dt.Rows[0]["INVITEROUTE"].ToString().Trim() == "4")
                    {
                        ssView_Sheet1.Cells[13, 5].Value = true;
                    }
                    else if (dt.Rows[0]["INVITEROUTE"].ToString().Trim() == "5")
                    {
                        ssView_Sheet1.Cells[13, 6].Value = true;
                    }
                    else if (dt.Rows[0]["INVITEROUTE"].ToString().Trim() == "0")
                    {
                        ssView_Sheet1.Cells[13, 2].Value = false;
                        ssView_Sheet1.Cells[13, 3].Value = false;
                        ssView_Sheet1.Cells[13, 4].Value = false;
                        ssView_Sheet1.Cells[13, 5].Value = false;
                        ssView_Sheet1.Cells[13, 6].Value = false;
                    }

                    ssView_Sheet1.Cells[15, 2].Text = dt.Rows[i]["FAMILYNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[15, 3].Text = dt.Rows[i]["FAMILYSERE"].ToString().Trim();
                    ssView_Sheet1.Cells[15, 4].Text = dt.Rows[i]["FAMILYREL"].ToString().Trim();
                    ssView_Sheet1.Cells[15, 5].Text = dt.Rows[i]["FAMILYAGE"].ToString().Trim();
                    ssView_Sheet1.Cells[15, 6].Text = dt.Rows[i]["FAMILYHAK"].ToString().Trim();
                    ssView_Sheet1.Cells[15, 7].Text = dt.Rows[i]["FAMILYJOB"].ToString().Trim();
                    ssView_Sheet1.Cells[16, 2].Text = dt.Rows[i]["FAMILYNAME1"].ToString().Trim();
                    ssView_Sheet1.Cells[16, 3].Text = dt.Rows[i]["FAMILYSERE1"].ToString().Trim();
                    ssView_Sheet1.Cells[16, 4].Text = dt.Rows[i]["FAMILYREL1"].ToString().Trim();
                    ssView_Sheet1.Cells[16, 5].Text = dt.Rows[i]["FAMILYAGE1"].ToString().Trim();
                    ssView_Sheet1.Cells[16, 6].Text = dt.Rows[i]["FAMILYHAK1"].ToString().Trim();
                    ssView_Sheet1.Cells[16, 7].Text = dt.Rows[i]["FAMILYJOB1"].ToString().Trim();
                    ssView_Sheet1.Cells[17, 2].Text = dt.Rows[i]["FAMILYNAME2"].ToString().Trim();
                    ssView_Sheet1.Cells[17, 3].Text = dt.Rows[i]["FAMILYSERE2"].ToString().Trim();
                    ssView_Sheet1.Cells[17, 4].Text = dt.Rows[i]["FAMILYREL2"].ToString().Trim();
                    ssView_Sheet1.Cells[17, 5].Text = dt.Rows[i]["FAMILYAGE2"].ToString().Trim();
                    ssView_Sheet1.Cells[17, 6].Text = dt.Rows[i]["FAMILYHAK2"].ToString().Trim();
                    ssView_Sheet1.Cells[17, 7].Text = dt.Rows[i]["FAMILYJOB2"].ToString().Trim();
                    ssView_Sheet1.Cells[18, 2].Text = dt.Rows[i]["FAMILYNAME3"].ToString().Trim();
                    ssView_Sheet1.Cells[18, 3].Text = dt.Rows[i]["FAMILYSERE3"].ToString().Trim();
                    ssView_Sheet1.Cells[18, 4].Text = dt.Rows[i]["FAMILYREL3"].ToString().Trim();
                    ssView_Sheet1.Cells[18, 5].Text = dt.Rows[i]["FAMILYAGE3"].ToString().Trim();
                    ssView_Sheet1.Cells[18, 6].Text = dt.Rows[i]["FAMILYHAK3"].ToString().Trim();
                    ssView_Sheet1.Cells[18, 7].Text = dt.Rows[i]["FAMILYJOB3"].ToString().Trim();
                    ssView_Sheet1.Cells[19, 2].Text = dt.Rows[i]["ETCDATA"].ToString().Trim(); ;
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
