using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComLibB;
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
    /// <seealso cref= "d:\psmh\nurse\nrstd\Frm욕창발생리스트.frm >> frmNrstdSTS03.cs 폼이름 재정의" />

    public partial class frmbedsoreList : Form
    {

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmbedsoreList()
        {
            InitializeComponent();
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
                strTitle = "욕창발생리스트";

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
            int j = 0;
            string strTemp = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 1;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT Pano,IPDNO,SName,Sex,Age,DIAGNOSYS,RoomCode,DeptCode,Remark , EntDate, EntSabun, ";
                SQL = SQL + ComNum.VBLF + "   Grade,Total,P_BALBUI,P_BALBUI_ETC,P_STEP,P_HAPBUNG,P_PROGRESS,P_YOIN,P_PRE, ";
                SQL = SQL + ComNum.VBLF + "   TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,TO_CHAR(InDate,'YYYY-MM-DD') InDate ,  ";
                SQL = SQL + ComNum.VBLF + "   TO_CHAR(ActDate,'YYYY-MM-DD') BDate, TO_CHAR(SEEKDate,'YYYY-MM-DD HH24:MI') SEEKDate, TO_CHAR(RETURNDate,'YYYY-MM-DD HH24:MI') RETURNDate, ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM NUR_PRESSURE_SORE ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >=TO_DATE('" + (dtpDate.Text) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <=TO_DATE('" + (dtpDate1.Text) + " 23:59','YYYY-MM-DD HH24:MI') ";

                if (ComboWard.Text != "")
                {
                    switch (ComboWard.Text)
                    {
                        case "전체":
                            break;
                        case "SICU":
                            SQL = SQL + ComNum.VBLF + " AND RoomCode = '233' ";
                            break;
                        case "MICU":
                            SQL = SQL + ComNum.VBLF + " AND RoomCode = '234' ";
                            break;
                        case "ND":
                            SQL = SQL + ComNum.VBLF + " AND RoomCode IN('369','358','398') ";
                            break;
                        default:
                            SQL = SQL + ComNum.VBLF + " AND WardCode='" + (ComboWard.Text) + "' ";
                            break;
                    }

                }

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
                    
                    SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SEEKDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["RETURNDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Indate"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = CF.DATE_ILSU(clsDB.DbCon, dt.Rows[i]["BDATE"].ToString().Trim(), dt.Rows[i]["InDate"].ToString().Trim(), "").ToString();
                    SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SName"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["RoomCode"].ToString().Trim();

                    strTemp = "";

                    for (j = 0; j < VB.L(dt.Rows[i]["P_BALBUI"].ToString().Trim(), "^^"); j++)
                    {
                        if (VB.Pstr(dt.Rows[i]["P_BALBUI"].ToString().Trim(), "^^", j + 1) == "1")
                        {
                            switch (j)
                            {
                                case 0:
                                    strTemp = strTemp + "미골, ";
                                    break;
                                case 1:
                                    strTemp = strTemp + "둔부, ";
                                    break;
                                case 2:
                                    strTemp = strTemp + "후두, ";
                                    break;
                                case 3:
                                    strTemp = strTemp + "견갑골, ";
                                    break;
                                case 4:
                                    strTemp = strTemp + "상지관절, ";
                                    break;
                                case 5:
                                    strTemp = strTemp + "하지관절, ";
                                    break;
                                case 6:
                                    strTemp = strTemp + "늑골, ";
                                    break;
                                case 7:
                                    strTemp = strTemp + "어깨, ";
                                    break;
                                case 8:
                                    strTemp = strTemp + "연골부위, ";
                                    break;
                                case 9:
                                    strTemp = strTemp + "기타, ";
                                    break;
                            }
                        }
                    }

                    //SS1_Sheet1.Cells[i, j].Text = VB.Mid(strTemp, 1, VB.Len((Convert.ToInt32(strTemp) - 2).ToString()));
                    SS1_Sheet1.Cells[i, 10].Text = VB.Mid(strTemp, 1, (strTemp.Length - 2));
                    SS1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
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

        private void frmbedsoreList_Load(object sender, EventArgs e)
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

            if (VB.Pstr(clsPublic.GstrHelpCode, "{}", 1) != "")
            {
                dtpDate.Value = Convert.ToDateTime(VB.Pstr(clsPublic.GstrHelpCode, "{}", 1));
                dtpDate1.Value = Convert.ToDateTime(VB.Pstr(clsPublic.GstrHelpCode, "{}", 2));
            }

            ComboWard.Enabled = ComQuery.NURSE_System_Manager_Check(VB.Val(clsType.User.Sabun));

            if (clsType.User.BuseCode == "076001")
            {
                ComboWard.Enabled = true;
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
            frmBedsoreReport frmBedsoreReportX = new frmBedsoreReport(ComboWard.Text.Trim(), SS1_Sheet1.Cells[e.Row, 11].Text.Trim());
            frmBedsoreReportX.StartPosition = FormStartPosition.CenterParent;
            frmBedsoreReportX.ShowDialog();
            frmBedsoreReportX.Dispose();
            frmBedsoreReportX = null;

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
