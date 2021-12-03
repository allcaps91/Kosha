using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmBuGamfVillage
    /// File Name : frmBuGamfVillage.cs
    /// Title or Description : 나자렛, 마리아, 햇빛마을 환자
    /// Author : 박창욱
    /// Create Date : 2017-06-06
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    /// </summary>
    /// <history>  
    /// VB\frmBugamf2.frm(FrmBuGamf2) -> frmBuGamfVillage.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\bugamf\frmBugamf2.frm(FrmBuGamf2)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\bugamf\\bugamf.vbp
    /// </vbp>
    public partial class frmBuGamfVillage : Form
    {
        public frmBuGamfVillage()
        {
            InitializeComponent();
        }

        private void frmBuGamfVillage_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT Code,Name FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='재단산하_구분' ";
                SQL = SQL + ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate ='') ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY SORT ";

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

                cboGuBun.Items.Clear();

                for (i = 0; i < dt.Rows.Count - 1; i++)
                {
                    cboGuBun.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + dt.Rows[i]["Name"].ToString().Trim());
                }
                cboGuBun.SelectedIndex = 0;

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            cancle();

        }

        private void cancle()
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            btnPrint.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            selectDate();
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            btnPrint.Enabled = true;
            btnDelete.Enabled = true;
            ssView.Enabled = true;
        }

        private void selectDate()
        {
            int i = 0;
            int nRead = 0;
            string strGubun = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;


            strGubun = VB.Left(cboGuBun.Text, 1);

            try
            {
                SQL = "";
                SQL = " SELECT JUMIN,JUMIN_new, NAME, TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE,";
                SQL = SQL + ComNum.VBLF + " MESSAGE, ROWID, PANO ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_NAHOMEGAM ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN  = '" + strGubun + "' ";
                if (rdoJumin.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY JUMIN, NAME ";
                }
                else if(rdoName.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY NAME, JUMIN ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY PANO ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = nRead + 100;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead - 1; i++)
                {
                    ssView_Sheet1.Cells[i, 1].Text = VB.Left(clsAES.DeAES(dt.Rows[i]["JUMIN_new"].ToString().Trim()), 6);
                    ssView_Sheet1.Cells[i, 2].Text = VB.Right(clsAES.DeAES(dt.Rows[i]["JUMIN_new"].ToString().Trim()), 7);
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MESSAGE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancle();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string strJumin = "";
            string strName = "";
            string strMess = "";
            string strDate = "";
            string strChk = "";
            string strRowId = "";
            string strGubun = "";
            string strPano = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            strJumin = ssView_Sheet1.Cells[0, 1].Text.Trim();
            if(strJumin == "")
            {
                ComFunc.MsgBox("자료를 저장할 수 없습니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                strGubun = VB.Left(cboGuBun.Text, 1);

                for (i = 1; i < ssView_Sheet1.RowCount; i++)
                {
                    strChk = ssView_Sheet1.Cells[i - 1, 0].Text;
                    strRowId = ssView_Sheet1.Cells[i - 1, 7].Text.Trim();
                    strJumin = ssView_Sheet1.Cells[i - 1, 1].Text;
                    strJumin = strJumin + "-" + ssView_Sheet1.Cells[i - 1, 2].Text;
                    strName = ssView_Sheet1.Cells[i - 1, 3].Text;
                    strMess = ssView_Sheet1.Cells[i - 1, 4].Text;
                    strDate = ssView_Sheet1.Cells[i - 1, 5].Text;
                    strPano = ssView_Sheet1.Cells[i - 1, 6].Text;
                    if (strRowId == "")
                    {
                        //GoSub Insert_Date
                        #region Insert_Date

                        SQL = "";
                        SQL = " SELECT GUBUN FROM BAS_NAHOMEGAM ";
                        SQL = SQL + ComNum.VBLF + " WHERE JUMIN_new = '" + clsAES.AES(strJumin) + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND NAME = '" + strName + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            SQL = "";
                            SQL = " INSERT INTO BAS_NAHOMEGAM (GUBUN,JUMIN,JUMIN_new,NAME,ENTDATE,MESSAGE,PANO) VALUES (";
                            SQL = SQL + ComNum.VBLF + " '" + strGubun + "','" + VB.Left(strJumin, 8) + "******','" + clsAES.AES(strJumin) + "', '" + strName + "', ";
                            SQL = SQL + ComNum.VBLF + " TO_DATE('" + strDate + "','YYYY-MM-DD'), '" + strMess + "', ";
                            SQL = SQL + ComNum.VBLF + " '" + strPano + "') ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                            }
                        }
                        else
                        {
                            switch (dt.Rows[0]["GUBUN"].ToString().Trim())
                            {
                                case "J":
                                    ComFunc.MsgBox("(환자명 : " + strName + ") 나자렛집환자에 등록 되어 있습니다. 확인 요망");
                                    break;
                                case "L":
                                    ComFunc.MsgBox("(환자명 : " + strName + ") 마리아집환자에 등록 되어 있습니다. 확인 요망");
                                    break;
                                case "N":
                                    ComFunc.MsgBox("(환자명 : " + strName + ") 햇빛마을환자에 등록 되어 있습니다. 확인 요망");
                                    break;
                                case "M":
                                    ComFunc.MsgBox("(환자명 : " + strName + ") 나자렛노인요양원에 등록 되어 있습니다. 확인 요망");
                                    break;
                            }
                        }
                        dt.Dispose();
                        dt = null;

                        #endregion
                    }
                    else
                    {
                        //GoSub UpDate_Data
                        #region UpDate_Data

                        SQL = "";
                        SQL = " UPDATE BAS_NAHOMEGAM SET ";
                        SQL = SQL + ComNum.VBLF + " JUMIN = '" + VB.Left(strJumin, 8) + "******',";
                        SQL = SQL + ComNum.VBLF + " JUMIN_new = '" + clsAES.AES(strJumin) + "', ";
                        SQL = SQL + ComNum.VBLF + " NAME = '" + strName + "', ";
                        SQL = SQL + ComNum.VBLF + " ENTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD'), ";
                        SQL = SQL + ComNum.VBLF + " MESSAGE = '" + strMess + "', ";
                        SQL = SQL + ComNum.VBLF + " PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID  =  '" + strRowId + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                        }

                        #endregion
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            selectDate();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strGuName = "";

            if(ssView_Sheet1.RowCount < 1) { return; }

            strGuName = VB.Mid(cboGuBun.Text, 3, cboGuBun.Text.Length);

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"20\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = strHead1 + "/c" + strGuName + " List";
            strHead2 = "/n/l/f2" + "인쇄일자 : " + clsPublic.GstrSysDate;

            //Print Body
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 100;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string strRowId = "";
            bool boolChk = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i=1 ; i<ssView_Sheet1.RowCount; i++)
                {
                    boolChk = Convert.ToBoolean(ssView_Sheet1.Cells[i - 1, 0].Value);
                    strRowId = ssView_Sheet1.Cells[i - 1, 7].Text;
                    if(boolChk == true)
                    {
                        SQL = "";
                        SQL = " DELETE BAS_NAHOMEGAM WHERE ROWID = '" + strRowId + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        ssView_Sheet1.Cells[i - 1, 0].Value = false;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
