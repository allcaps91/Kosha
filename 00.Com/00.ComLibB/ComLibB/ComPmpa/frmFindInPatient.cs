using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDiet
    /// File Name       : frmFindInPatient.cs
    /// Description     : 입원 재원자 찾기
    /// Author          : 박창욱
    /// Create Date     : 2018-08-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iviewa\frm재원자찾기.frm(frm재원자찾기.frm) >> frmFindInPatient.cs 폼이름 재정의" />	
    public partial class frmFindInPatient : Form
    {
        string strInDate = "";
        string strInDate1 = "";
        string strOptSql = "";

        public frmFindInPatient()
        {
            InitializeComponent();
        }

        void Patient_SQL()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strBi = "";

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (tab.SelectedTabIndex == 4)  //생일자 검색
                {
                    SQL = "";
                    SQL = " SELECT WardCode, RoomCode, Pano, Bi, Sname, Pname, Sex, Age, Gbinfor2,";
                    SQL = SQL + ComNum.VBLF + "        InDate, DeptCode, DrName, Religion, SECRET, BIRTHDAY";
                    SQL = SQL + ComNum.VBLF + "FROM (SELECT";
                    SQL = SQL + ComNum.VBLF + "      i.WardCode,i.RoomCode,i.Pano,i.Bi,i.Sname,i.Pname,i.Sex,i.Age,c.Gbinfor2,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(i.InDate, 'yyyy-mm-dd') InDate,i.DeptCode,k.DrName,i.Religion,I.SECRET,";
                    SQL = SQL + ComNum.VBLF + "      SUBSTR(C.JUMIN1,3,4) AS BIRTHDAY, '" + dtpBirth.Value.ToString("MMdd") + "' AS TODAY";
                    SQL = SQL + ComNum.VBLF + "      FROM IPD_NEW_MASTER i,BAS_DOCTOR k, bas_patient c";
                    SQL = SQL + ComNum.VBLF + "      WHERE i.OutDate IS NULL";
                    SQL = SQL + ComNum.VBLF + "      AND I.AMSET4 NOT IN ('3')";
                    SQL = SQL + ComNum.VBLF + "      AND i.DrCode = k.DrCode(+)";
                    SQL = SQL + ComNum.VBLF + "      AND i.Pano=c.Pano(+))";
                    SQL = SQL + ComNum.VBLF + "WHERE BIRTHDAY = TODAY";
                    SQL = SQL + ComNum.VBLF + "ORDER BY Sname";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT i.WardCode,i.RoomCode,i.Pano,i.Bi,i.Sname,i.Pname,i.Sex,i.Age,c.Gbinfor2,       ";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(i.InDate, 'yyyy-mm-dd') InDate,i.DeptCode,k.DrName,i.Religion,I.SECRET      ";
                    SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_MASTER i,BAS_DOCTOR k, bas_patient c";
                    SQL = SQL + ComNum.VBLF + "  WHERE i.OutDate IS NULL";
                    SQL = SQL + ComNum.VBLF + "    AND I.AMSET4 NOT IN ('3')";  //允(2005-08-29) 정상애기 제외

                    switch (tab.SelectedTabIndex)
                    {
                        case 0:
                            SQL = SQL + ComNum.VBLF + "    AND i.Sname LIKE '%" + txtSName.Text + "%'";
                            break;
                        case 1:
                            SQL = SQL + ComNum.VBLF + strOptSql;
                            break;
                        case 2:
                            SQL = SQL + ComNum.VBLF + "    AND i.Pname LIKE '%" + txtPName.Text + "%'";
                            break;
                        case 3:
                            SQL = SQL + ComNum.VBLF + "    AND i.InDate >= TO_DATE('" + strInDate + "','yyyy-mm-dd')  ";
                            SQL = SQL + ComNum.VBLF + "    AND i.InDate <  TO_DATE('" + strInDate1 + "','yyyy-mm-dd') ";
                            break;
                    }

                    SQL = SQL + ComNum.VBLF + "    AND i.DrCode = k.DrCode(+)";
                    SQL = SQL + ComNum.VBLF + "    AND i.Pano=c.Pano(+) ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY i.Sname";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count < 20)
                {
                    ssView_Sheet1.RowCount = 20;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBi = dt.Rows[i]["BI"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strBi);
                    ssView_Sheet1.Cells[i, 5].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_종교구분", dt.Rows[i]["Religion"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DrName"].ToString().Trim();

                    if (dt.Rows[i]["GBINFOR2"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["GBINFOR2"].ToString().Trim();
                        ssView_Sheet1.Rows[i].BackColor = Color.Red;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i, 11].Text = "";
                        ssView_Sheet1.Rows[i].BackColor = Color.White;
                    }

                    ssView_Sheet1.Cells[i, 12].Text = "";

                    if (dt.Rows[i]["SECRET"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 12].Text = "요청자";
                        ssView_Sheet1.Rows[i].BackColor = Color.Red;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void frmFindInPatient_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            dtpInDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo00.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo00.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo01.Text + "'";
            }
            else if (rdo01.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo01.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo02.Text + "'";
            }
            else if (rdo02.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo02.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo03.Text + "'";
            }
            else if (rdo03.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo03.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo04.Text + "'";
            }
            else if (rdo04.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo04.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo05.Text + "'";
            }
            else if (rdo05.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo05.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo06.Text + "'";
            }
            else if (rdo06.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo06.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo07.Text + "'";
            }
            else if (rdo07.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo07.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo08.Text + "'";
            }
            else if (rdo08.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo08.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo09.Text + "'";
            }
            else if (rdo09.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo09.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo10.Text + "'";
            }
            else if (rdo10.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo10.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo11.Text + "'";
            }
            else if (rdo11.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo11.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo12.Text + "'";
            }
            else if (rdo12.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo12.Text + "'";
                strOptSql += " AND   i.Sname < '" + rdo13.Text + "'";
            }
            else if (rdo13.Checked == true)
            {
                strOptSql = " AND i.Sname >= '" + rdo13.Text + "'";
            }

            Patient_SQL();
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            lblInfo.Text = "";
            if (ssView_Sheet1.Cells[e.Row, 12].Text.Trim() != "")
            {
                ComFunc.MsgBox("사생활보호 대상요청자입니다. 안내시 주의하십시오.");
            }

            lblInfo.Text = ssView_Sheet1.Cells[e.Row, 12].Text.Trim();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            //SSIPD.Col = 1
            //SSIPD.Row = Row
            //GstrPANO = SSIPD.Text
            //Unload Me
        }

        private void tab_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            ssView_Sheet1.RowCount = 0;

            switch (tab.SelectedTabIndex)
            {
                case 0:
                    txtSName.Focus();
                    break;
                case 1:
                    rdo00.Checked = true;
                    break;
                case 2:
                    txtPName.Focus();
                    break;
                case 3:
                    dtpInDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                    break;
                case 4:
                    //dtpBirth.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(1);
                    dtpInDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                    Patient_SQL();
                    break;
                default:
                    break;
            }
        }

        private void dtpInDate_ValueChanged(object sender, EventArgs e)
        {
            strInDate = dtpInDate.Value.ToString("yyyy-MM-dd");
            strInDate1 = dtpInDate.Value.AddDays(1).ToString("yyyy-MM-dd");

            Patient_SQL();
        }

        private void txtPName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPName.Text.Trim() != "")
                {
                    Patient_SQL();
                }
            }
        }

        private void txtSName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSName.Text.Trim() != "")
                {
                    Patient_SQL();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Patient_SQL();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = dtpBirth.Value.ToString("yyyy-MM-dd") + " 생일자 명단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false, 0.9f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }
    }
}
