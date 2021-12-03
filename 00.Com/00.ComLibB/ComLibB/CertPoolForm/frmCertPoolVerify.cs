using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmCertPoolVerify : Form
    {
        public frmCertPoolVerify()
        {
            InitializeComponent();
        }

        private void frmCertPoolVerify_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); 
                return; 
            } //폼 권한 조회
            
            ComFunc.ReadSysDate(clsDB.DbCon);
            ssView.ActiveSheet.RowCount = 0;

            DtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-10);
            DtpEDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; 
            
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView.ActiveSheet.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.SABUN, TO_CHAR(A.UDATE, 'YYYY-MM-DD') UDATE, A.CERTIOK,b.CertPass, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.CERDATE, 'YYYY-MM-DD') CERDATE, B.BUSE, B.KORNAME, A.ROWID, A.USE, B.JUMIN3 BJUMIN3 ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.INSA_MSTS A, ADMIN.INSA_MST B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SABUN = B.SABUN ";
                SQL = SQL + ComNum.VBLF + "   AND A.UDATE >= TO_DATE('" + DtpSDate.Value.ToShortDateString() + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "   AND trunc(A.UDATE) <= TO_DATE('" + DtpEDate.Value.ToShortDateString() + "','YYYY-MM-DD')  ";
                if (TxtName.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND b.KorName LIKE '%" + TxtName.Text.Trim() + "%' ";
                }
                SQL = SQL + ComNum.VBLF + "   ORDER BY BUSE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView.ActiveSheet.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView.ActiveSheet.Cells[i, 0].Text = clsVbfunc.GetBASBuSe(clsDB.DbCon, dt.Rows[i]["BUSE"].ToString().Trim());
                        ssView.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 3].Text = clsAES.DeAES(dt.Rows[i]["BJUMIN3"].ToString().Trim());
                        ssView.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["UDATE"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["CERDATE"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["USE"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["CertPass"].ToString().Trim();
                        if (dt.Rows[i]["CERTIOK"].ToString().Trim() == "1")
                        {
                            ssView.ActiveSheet.Cells[i, 6].Text = "성공";
                        }
                        else
                        {
                            ssView.ActiveSheet.Cells[i, 6].Text = "실패";
                        }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return;

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strOK = "OK";
            string strJUMIN = string.Empty;
            string strSABUN = string.Empty;
            string strUPDATE = string.Empty;
            string strCERDATE = string.Empty;
            string strCHECK = string.Empty;
            string strCERTPASS = string.Empty;

            //1.API 초기화 : API_INIT
            if (clsCertWork.API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0) == false)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView.ActiveSheet.RowCount; i++)
                {
                    strJUMIN = ssView.ActiveSheet.Cells[i, 3].Text.Trim();
                    strSABUN = ssView.ActiveSheet.Cells[i, 1].Text.Trim();
                    strUPDATE = ssView.ActiveSheet.Cells[i, 4].Text.Trim();
                    strCERDATE = ssView.ActiveSheet.Cells[i, 5].Text.Trim();
                    strCHECK = ssView.ActiveSheet.Cells[i, 7].Text.Trim();
                    strCERTPASS = clsAES.DeAES(ssView.ActiveSheet.Cells[i, 10].Text.Trim());

                    if (string.IsNullOrEmpty(strJUMIN) || strJUMIN.Length != 13)
                    {
                        ComFunc.MsgBox("주민번호 에러");
                        return;
                    }

                    if (clsCertWork.ROAMING_NOVIEW_FORM(strJUMIN, strCERTPASS) == true)
                    {
                        SQL = " UPDATE ADMIN.INSA_MSTS SET ";
                        SQL = SQL + ComNum.VBLF + " CERDATE = TO_DATE('" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "','YYYY-MM-DD HH24:MI'), ";
                        SQL = SQL + ComNum.VBLF + " CERTIOK = '1' ";
                        SQL = SQL + ComNum.VBLF + "  WHERE SABUN = '" + strSABUN + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND TRUNC(UDATE) = TO_DATE('" + strUPDATE + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("처리 중 에러 발생", "확인");
                            clsCertWork.API_RELEASE();
                            return;
                        }

                        ssView.ActiveSheet.Cells[i, 6].Text = "성공";
                    }
                    else
                    {
                        SQL = " UPDATE ADMIN.INSA_MSTS SET ";
                        SQL = SQL + ComNum.VBLF + " CERDATE = TO_DATE('" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "','YYYY-MM-DD HH24:MI'), ";
                        SQL = SQL + ComNum.VBLF + " CERTIOK = '0' ";
                        SQL = SQL + ComNum.VBLF + "  WHERE SABUN = '" + strSABUN + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND TRUNC(UDATE) = TO_DATE('" + strUPDATE + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("처리 중 에러 발생", "확인");
                            clsCertWork.API_RELEASE();
                            return;
                        }

                        ssView.ActiveSheet.Cells[i, 6].Text = "실패";
                    }
                }

                ComFunc.MsgBox("정상적으로 처리되었습니다.", "확인");
                clsDB.setCommitTran(clsDB.DbCon);
                clsCertWork.API_RELEASE();
                BtnView.PerformClick();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("처리 중 에러 발생", "확인");
                clsCertWork.API_RELEASE();
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return;

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C 공인인증서 발급리스트" + "/n/n/n/n";
            strHead2 = "/l/f2" + "출력일자 : " + SysDate + "  PAGE : " + "/p";
            
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;   //세로
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 35;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 35;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true && e.Column == 7)
            {
                ssView.ActiveSheet.Cells[0, 7, ssView.ActiveSheet.RowCount - 1, 7].Value = true;
            }
        }


    }
}
