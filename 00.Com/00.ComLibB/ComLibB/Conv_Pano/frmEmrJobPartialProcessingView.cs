using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : EmrJob
    /// File Name       : frmEmrJobPartialProcessingView
    /// Description     : 부분이중처리 - 작업등록 및 보기
    /// Author          : 이현종
    /// Create Date     : 2020-02-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "PSMH\mid\midchdl\Frm부분처리등록보기(Frm부분처리등록보기.frm) >> frmEmrJobPartialProcessingView.cs 폼이름 재정의" />
    public partial class frmEmrJobPartialProcessingView : Form
    {
        public frmEmrJobPartialProcessingView()
        {
            InitializeComponent();
        }

        private void frmEmrJobPartialProcessingView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            SS1_Sheet1.Columns[4, 6].Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        private void GetSearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            SS1_Sheet1.RowCount = 0;

            #region 변수
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            #endregion


            Cursor.Current = Cursors.WaitCursor;

            #region 현재 자료를 READ
            SQL = "SELECT Code,Name,TO_CHAR(JDate,'YYYY-MM-DD') JDate,  ";
            SQL += ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,ROWID   ";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE                    ";
            SQL += ComNum.VBLF + "WHERE Gubun = '부분이중처리내역'                 ";
 
            if (string.IsNullOrWhiteSpace(txtPano.Text.Trim()) == false)
            {
                txtPano.Text = VB.Val(txtPano.Text.Trim()).ToString("00000000");
                SQL += ComNum.VBLF + "  AND CODE = '" + txtPano.Text.Trim() + "'";
            }
            SQL += ComNum.VBLF + "ORDER BY JDate DESC                            ";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    SS1_Sheet1.RowCount += 1;

                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = "";
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = reader.GetValue(0).ToString().Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = reader.GetValue(1).ToString().Trim();

                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = reader.GetValue(2).ToString().Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 4].Text = reader.GetValue(3).ToString().Trim();

                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 5].Text = reader.GetValue(4).ToString().Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 6].Text = "";

                }
            }

            reader.Dispose();

            SS1_Sheet1.RowCount += 20;
            Cursor.Current = Cursors.Default;

            #endregion
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;

            string strHeader = string.Empty;

            SS1_Sheet1.Columns[0].Visible = false;

            using (clsSpread cSp = new clsSpread())
            {
                Font fontTitle = new Font("굴림체", 16);
                Font fontSub = new Font("굴림체", 10);
                strHeader = cSp.setSpdPrint_String(VB.Space(18) + "부분이중처리 작업내용", fontTitle, clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += cSp.setSpdPrint_String("출력일자 : " + ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10), fontSub, clsSpread.enmSpdHAlign.Left, false, false);

                fontTitle.Dispose();
                fontSub.Dispose();
            }

            SS1_Sheet1.PrintInfo.Header = strHeader;
            SS1_Sheet1.PrintInfo.Margin.Left = 50;
            SS1_Sheet1.PrintInfo.Margin.Right = 0;
            SS1_Sheet1.PrintInfo.Margin.Top = 30;
            SS1_Sheet1.PrintInfo.Margin.Bottom = 180;

            SS1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            SS1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            SS1_Sheet1.PrintInfo.ShowBorder = true;
            SS1_Sheet1.PrintInfo.ShowColor = false;
            SS1_Sheet1.PrintInfo.ShowGrid = true;
            SS1_Sheet1.PrintInfo.ShowShadows = true;
            SS1_Sheet1.PrintInfo.UseMax = true;
            SS1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            SS1.PrintSheet(0);

            ComFunc.Delay(100);

            SS1_Sheet1.Columns[0].Visible = true;
            SS1_Sheet1.RowCount = SS1_Sheet1.NonEmptyRowCount + 20;

            Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;
            }

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            #endregion
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                int RowAffected = 0;

                for (int i = 0; i < SS1_Sheet1.NonEmptyRowCount; i++)
                {
                    string strDel = SS1_Sheet1.Cells[i, 0].Text.Trim();
                    string strCode = SS1_Sheet1.Cells[i, 1].Text.Trim();
                    string strName = SS1_Sheet1.Cells[i, 2].Text.Trim();
                    string strJDate = SS1_Sheet1.Cells[i, 3].Text.Trim();
                    string strDeldate = SS1_Sheet1.Cells[i, 4].Text.Trim();
                    string strROWID = SS1_Sheet1.Cells[i, 5].Text.Trim();
                    string strChange = SS1_Sheet1.Cells[i, 6].Text.Trim();

                    if (strDel.Equals("True"))
                    {

                    }
                    else if(strChange.Equals("Y"))
                    {
                        if (string.IsNullOrWhiteSpace(strROWID))
                        {
                            #region 신규등록
                            SQL = "INSERT INTO KOSMOS_PMPA.BAS_BCODE (Gubun,Code,Name,JDate,DelDate,EntSabun,EntDate) ";
                            SQL += ComNum.VBLF + "VALUES ('부분이중처리내역','" + strCode + "','" + strName + "',";
                            SQL += ComNum.VBLF + "TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + "TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + clsType.User.IdNumber + ",SYSDATE) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                Cursor.Current = Cursors.Default;
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                            #endregion
                        }
                        else
                        {
                            #region UPDATE '자료를 변경
                            SQL = "UPDATE KOSMOS_PMPA.BAS_BCODE SET Code='" + strCode + "',";
                            SQL += ComNum.VBLF + "      Name = '" + strName + "',";
                            SQL += ComNum.VBLF + "      JDate = TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + "      DelDate = TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + "      EntSabun = " + clsType.User.IdNumber + ",";
                            SQL += ComNum.VBLF + "      EntDate = SYSDATE ";
                            SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                Cursor.Current = Cursors.Default;
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                            #endregion
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                Screen_Clear();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Screen_Clear()
        {
            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 50;
        }

        private void SS1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 0)
                return;

            SS1_Sheet1.Cells[e.Row, 6].Text = "Y";
        }

        private void SS1_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            SS1_Sheet1.Cells[e.Row, 6].Text = "Y";
        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(SS1, e.Column);
                return;
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetSearchData();
            }
        }
    }
}
