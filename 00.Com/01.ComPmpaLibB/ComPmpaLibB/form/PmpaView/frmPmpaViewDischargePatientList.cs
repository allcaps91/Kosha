using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaViewDischargePatientList : Form
    {

        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaViewDischargePatientList.cs
        /// Description     : 퇴원환자명부
        /// Author          : 김효성
        /// Create Date     : 2017-08-22
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "psmh\IPD\iument\Frm퇴원환자명부.FRM(Frm퇴원환자명부) >> frmPmpaViewDischargePatientList.cs 폼이름 재정의" />	

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        ComFunc cf = new ComFunc();

        public frmPmpaViewDischargePatientList()
        {
            InitializeComponent();
        }

        private void frmPmpaViewDischargePatientList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpTdate.Value = DateTime.Parse(strDTP);

            cboGubun.Items.Clear();
            cboGubun.Items.Add("전체");// 'ALL
            cboGubun.Items.Add("보험(전체)");//'11-15, 41-45
            cboGubun.Items.Add("보험(공단)");//'11
            cboGubun.Items.Add("보험(직장)");//'12
            cboGubun.Items.Add("보험(지역)");//'13
            cboGubun.Items.Add("의료급여");//'21-25
            cboGubun.Items.Add("산재");//'31
            cboGubun.Items.Add("공상");//'32
            cboGubun.Items.Add("산재공상");//'33
            cboGubun.Items.Add("보험계약");//'45
            cboGubun.Items.Add("일반");//'51,54
            cboGubun.Items.Add("TA보험");//'52
            cboGubun.Items.Add("일반계약");//'53
            cboGubun.Items.Add("TA일반");// '55
            cboGubun.SelectedIndex = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 30;

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int nRow = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                SQL = "				   SELECT a.Pano,b.SName";
                SQL = SQL + ComNum.VBLF + "		     ,a .Bi,a.DeptCode,a.DrCode";
                SQL = SQL + ComNum.VBLF + "			,c .DrName, TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,a.Ilsu";
                SQL = SQL + ComNum.VBLF + "			,TO_CHAR(a.OutDate,'YYYY-MM-DD') OutDate,b.Jumin1";
                SQL = SQL + ComNum.VBLF + "			,b.Jumin2,b.Tel,b.Remark, ";

                if (rdoJob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "      a.Kiho,a.Gwange,a.PName,d.WardCode,d.RoomCode, a.Amset5 ";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "IPD_TRANS a," + ComNum.DB_PMPA + "BAS_PATIENT b," + ComNum.DB_PMPA + "BAS_DOCTOR c," + ComNum.DB_PMPA + "IPD_NEW_MASTER d ";
                    SQL = SQL + ComNum.VBLF + "   WHERE 1 = 1 ";
                    SQL = SQL + ComNum.VBLF + "     AND a.ActDate>=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND a.ActDate<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND a.Amt50 <> 0 ";
                    SQL = SQL + ComNum.VBLF + "     AND a.IPDNO=d.IPDNO(+) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "	    d.Kiho,d.Gwange,d.PName,a.WardCode,a.RoomCode, D.AMSET5 ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a," + ComNum.DB_PMPA + "BAS_PATIENT b," + ComNum.DB_PMPA + "BAS_DOCTOR c," + ComNum.DB_PMPA + "IPD_TRANS d ";
                    SQL = SQL + ComNum.VBLF + "   WHERE 1 = 1 ";
                    SQL = SQL + ComNum.VBLF + "     AND a.OutDate>=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND a.OutDate<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND a.GBSTS <> '9' ";
                    SQL = SQL + ComNum.VBLF + "     AND a.LASTTRS=d.TRSNO(+) ";
                }

                //'환자구분
                switch (cboGubun.Text)
                {
                    case "전체":
                        break;
                    case "보험(전체)":
                        SQL = SQL + ComNum.VBLF + " AND ((a.Bi>='11' AND a.Bi<='15') OR (a.Bi>='41' AND a.Bi<='45'))  ";
                        break;
                    case "보험(공단)":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '11' ";
                        break;
                    case "보험(직장)":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '12' ";
                        break;
                    case "보험(지역)":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '13' ";
                        break;
                    case "의료급여":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi>='21' AND a.Bi<='25' ";
                        break;
                    case "산재":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '31' ";
                        break;
                    case "공상":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '32' ";
                        break;
                    case "산재공상":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '33' ";
                        break;
                    case "보험계약":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '45' ";
                        break;
                    case "일반":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi IN ('51','54) ";
                        break;
                    case "TA보험":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '52' ";
                        break;
                    case "일반계약":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '53' ";
                        break;
                    case "TA일반":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '55' ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "	 AND a.Pano=b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "	 AND a.DrCode=c.DrCode(+) ";
                if (rdoJob0.Checked == true)
                {
                    if (rdoSORT0.Checked == true)// '성명순
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY b.SName,a.Pano ";
                    }
                    else if (rdoSORT1.Checked == true)// '병동,호실
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY d.WardCode,d.RoomCode,b.SName ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY a.DeptCode,a.DrCode ";

                    }
                }
                else
                {
                    if (rdoSORT0.Checked == true)// '성명순
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY b.SName,a.Pano ";
                    }
                    else if (rdoSORT1.Checked == true) // '병동,호실
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY a.WardCode,a.RoomCode,b.SName ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY a.DeptCode,a.DrCode ";
                    }
                }

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                nRow = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = cf.Read_Bcode_Name(clsDB.DbCon, "BAS_환자종류", dt.Rows[i]["BI"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["PNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = cf.Read_Bcode_Name(clsDB.DbCon, "BAS_피보험자관계", dt.Rows[i]["GWANGE"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["KIHO"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[i]["JUMIN2"].ToString().Trim(), 1) + "******";
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["ILSU"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["TEL"].ToString().Trim();

                    switch (dt.Rows[i]["AMSET5"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = "완쾌";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = "자의";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = "사망";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = "전원";
                            break;
                        case "5":
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = "빈사";
                            break;
                        case "6":
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = "도주";
                            break;
                        case "7":
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = "보관금";
                            break;
                        case "8":
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = "구분변경";
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 14].Text = "";
                            break;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                ssView_Sheet1.RowCount = nRow;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int j = 0;
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            strFont1 = "/c/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + " 퇴  원  자  명  부 " + cboGubun.Text + "/f1/n" + "/f1/n" + " " + "/f1/n";
            strHead2 = "/n/l/f2" + "작업기간 : " + dtpTdate.Value.ToString("yyyy-MM-dd") + "작성자 :  " + clsType.User.JobName + "   출력시간 : " + strDTP + " ";

            strHead2 = strHead2 + VB.Space(20) + "인쇄일자: " + mdtp;

            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpTdate_Leave(object sender, EventArgs e)
        {
            if (dtpTdate.Value > DateTime.Parse(strDTP))
            {
                ComFunc.MsgBox("작업 일자가 오늘보다 큽니다", "주의");
                dtpTdate.Value = DateTime.Parse(strDTP);
                dtpTdate.Focus();
                return;
            }
        }
    }
}
