using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

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
    /// <seealso cref= "d:\psmh\nurse\nrstd\Frm투약오류리스트.frm >> frmMedicationList.cs 폼이름 재정의" />

    public partial class frmMedicationList : Form
    {
        string strBuse = "";

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmMedicationList()
        {
            InitializeComponent();
        }


        /// <summary>
        /// "" : 병동, 적정관리실 : QI팀
        /// </summary>
        /// <param name="strBuse">부서명</param>
        public frmMedicationList(string strBuse, string strTemp)
        {
            InitializeComponent();
            this.strBuse = strBuse;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                strTitle = "투약오류리스트";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회기간 : " + dtpDate.Text + " ~ " + dtpDate1.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strToDate = "";
            string strNextDate = "";
            string strWARD = "";

            string strGubun = rdoGubun0.Checked ? "Bdate" : "JepDate";

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 1;

            strToDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strNextDate = strBuse == "적정관리실" ? dtpDate1.Text : CF.DATE_ADD(clsDB.DbCon, dtpDate1.Text, 1);
            strWARD = ComboWard.Text;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(BDate,'YYYY-MM-DD HH24:MI') BDATE, TO_CHAR(ActDate,'YYYY-MM-DD HH24:MI') ActDate, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,";
                SQL = SQL + ComNum.VBLF + " Pano,SName,Sex,Age,WardCode,Ipdno,DeptCode,RoomCode,Drug_J,ROWID ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_STD_DRUG ";
                SQL = SQL + ComNum.VBLF + " WHERE " +  strGubun + " >= TO_DATE('" + strToDate   + " 00:00','YYYY-MM-DD HH24:MI')   ";
                SQL = SQL + ComNum.VBLF + "  AND  " +  strGubun + " <= TO_DATE('" + strNextDate + " 23:59','YYYY-MM-DD HH24:MI') ";

                if (strWARD != "전체" && strWARD != "")
                    SQL = SQL + ComNum.VBLF + " AND WardCode = '" + strWARD + "' ";

                SQL = SQL + ComNum.VBLF + " ORDER BY " + strGubun;

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
                //SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                SS1_Sheet1.SetRowHeight(-1, 28);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JepDate"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();

                    SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SName"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DRUG_J"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();

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

        private void frmMedicationList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            ComboWard_SET();

            dtpDate.Value = Convert.ToDateTime(CF.DATE_ADD(clsDB.DbCon, strDTP, -10));
            dtpDate1.Value = Convert.ToDateTime(strDTP);

            ComboWard.Enabled = ComQuery.NURSE_System_Manager_Check(VB.Val(clsType.User.Sabun));

            rdoGubun1.Checked = true;

            if (strBuse == "적정관리실")
            {
                //rdoGubun0.Checked = true;
                rdoGubun1.Checked = true;
                ComboWard.Enabled = true;
                ComboWard.SelectedIndex = 0;
                panel1.Visible = false;
            }
            else
            {
                //rdoGubun1.Checked = true;
                panel1.Visible = true;
            }

        }

        private void ComboWard_SET()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string gsWard = "";

            gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','NR','DR','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "   AND USED = 'Y'";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

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
                ComboWard.Items.Clear();
                ComboWard.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ComboWard.Items.Add(dt.Rows[i]["Wardcode"].ToString().Trim());
                }
                //ComboWard.Items.Add("SICU");
                //ComboWard.Items.Add("MICU");

                ComboWard.SelectedIndex = 0;

                for (i = 0; i < ComboWard.Items.Count; i++)
                {
                    if (ComboWard.Items.IndexOf(gsWard) == i)
                    {
                        ComboWard.SelectedIndex = i;
                        ComboWard.Enabled = false;
                        return;
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            frmAdministrationEorrReport frm = new frmAdministrationEorrReport(SS1_Sheet1.Cells[e.Row, 9].Text); //ROWID
            frm.ShowDialog();
            frm.Dispose();
            frm = null;
        }

        private void btnEMR_Click(object sender, EventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0) return;

            clsVbEmr.EXECUTE_TextEmrViewEx(SS1_Sheet1.Cells[SS1_Sheet1.ActiveRowIndex, 3].Text.Trim(), clsType.User.Sabun);
            return;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            clsSpread CS = new clsSpread();
            CS.ExportToXLS(SS1);
            CS = null;
        }
    }
}
