using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmPaJewonFind
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\IPD\iunpsl\iunpsl.vbp\frm재원자찾기.frm >> frmPmPaJewonFind.cs 폼이름 재정의" />

    public partial class frmPmPaJewonFind : Form
    {
        string strDTP = "";
        string strOptSql = "";
        string strInDate = "";
        string strInDate1 = "";

        int nTab = 0;

        public frmPmPaJewonFind()
        {
            InitializeComponent();
            SetEvent();
        }
        
        void SetEvent()
        {
            this.txtSname.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtPname.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnView.Focus();
            }
        }

        public frmPmPaJewonFind(string strName)
        {
            InitializeComponent();
            SetEvent();

            if (strName.Trim() != "")
            {
                txtSname.Text = strName;
            }
        }

        private void frmPmPaJewonFind_Load(object sender, EventArgs e)
        {
            
            ComFunc CF = new ComFunc();
            clsSpread CS = new clsSpread();
            strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpinDate.Value = Convert.ToDateTime(strDTP);
            strOptSql = " AND i.Sname >= '" + rdo0.Text + "'";
            txtPname.ImeMode = ImeMode.Hangul;
            txtSname.ImeMode = ImeMode.Hangul;
        }

        #region Patient_Sql

        private void Patient_Sql()
        {
            int i = 0;
            int j = 0;
            string strBi = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT i.WardCode,i.RoomCode,i.Pano,i.Bi,i.Sname,i.Pname,i.Sex,i.Age,c.Gbinfor2,       ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(i.InDate, 'yyyy-mm-dd') InDate,i.DeptCode,k.DrName,i.Religion,I.SECRET      ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER i," + ComNum.DB_PMPA + "BAS_DOCTOR k, " + ComNum.DB_PMPA + "bas_patient c";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                                ";
                SQL = SQL + ComNum.VBLF + "    AND i.OutDate IS NULL                                                  ";
                SQL = SQL + ComNum.VBLF + "    AND I.AMSET4 NOT IN ('3')";//  '允(2005-08-29) 정상애기 제외


                switch (nTab)
                {
                    case 0:
                        SQL = SQL + ComNum.VBLF + "    AND i.Sname LIKE '%" + txtSname.Text + "%'                 ";
                        break;
                    case 1:
                        SQL = SQL + ComNum.VBLF + strOptSql;
                        break;
                    case 2:
                        SQL = SQL + ComNum.VBLF + "    AND i.Pname LIKE '%" + txtPname.Text + "%'                 ";
                        break;
                    case 3:
                        SQL = SQL + ComNum.VBLF + "    AND i.InDate >= TO_DATE('" + strInDate + "','yyyy-mm-dd')  ";
                        SQL = SQL + ComNum.VBLF + "    AND i.InDate <  TO_DATE('" + strInDate1 + "','yyyy-mm-dd') ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "    AND i.DrCode = k.DrCode(+)                                             ";
                SQL = SQL + ComNum.VBLF + "    AND i.Pano=c.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY i.Sname                                                           ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 1 - 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2 - 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3 - 1].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4 - 1].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5 - 1].Text = " " + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strBi);
                    ssView_Sheet1.Cells[i, 6 - 1].Text = " " + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_종교구분", dt.Rows[i]["Religion"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 7 - 1].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8 - 1].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9 - 1].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10 - 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11 - 1].Text = dt.Rows[i]["DrName"].ToString().Trim();

                    if (dt.Rows[i]["Gbinfor2"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 12 - 1].Text = dt.Rows[i]["Gbinfor2"].ToString();
                        ssView_Sheet1.Rows[i].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i, 12 - 1].Text = "";
                        ssView_Sheet1.Rows[i].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    }

                    ssView_Sheet1.Cells[i, 13 - 1].Text = "";

                    if (dt.Rows[i]["SECRET"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 13 - 1].Text = "요청자";
                        ssView_Sheet1.Rows[i].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            strInDate = dtpinDate.Value.ToString("yyyy-MM-dd");
            strInDate1 = dtpinDate.Value.AddDays(1).ToString("yyyy-MM-dd");
            Patient_Sql();
        }

        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdo_CheckedChanged(object sender, EventArgs e)
        {
            Control[] controls = ComFunc.GetAllControls(this);    //모든 control을 받아온다

            foreach (Control ctl in controls)   //foreach문을 사용하여 control을 하나씩 ctl에 넣는다.
            {
                if (ctl is RadioButton)     //한 control이 Radio버튼이라면 ?
                {
                    if (VB.Left(((RadioButton)ctl).Name, 3) == "rdo") //라디오버튼의 이름을 10글자 잘라서 rdoZipName인지 확인
                    {
                        if (((RadioButton)ctl).Checked == true)     //Check 여부 확인
                        {
                            strOptSql = " AND i.Sname >= '" + ((RadioButton)ctl).Text + "'";
                            break;  //foreach문 나가기
                        }
                    }
                }
            }
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int eRow = ssView_Sheet1.ActiveRowIndex;
            lblinfo.Text = "";

            if (ssView_Sheet1.Cells[e.Row, 13 - 1].Text != "")
            {
                ComFunc.MsgBox("사생활보호 대상요청자 입니다...안내시 주의하십시오.", "확인");
            }

            lblinfo.Text = ssView_Sheet1.Cells[eRow, 12 - 1].Text;
        }

        private void tabMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;

            for (i = 0; i < 4; i++)
            {
                if (tabMenu.SelectedIndex == i)
                {
                    nTab = i;
                    break;
                }
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            clsPublic.GstrPANO = ssView_Sheet1.Cells[e.Row, 0].Text;
            this.Close();
            return;
        }
    }
}
