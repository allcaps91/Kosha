using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-11-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\IPD\iument\iument.vbp\입원환자명부FRM" >> frmPmpaIPDPatientList.cs 폼이름 재정의" />

    public partial class frmPmpaIPDPatientList : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaIPDPatientList()
        {
            InitializeComponent();
        }

        private void frmPmpaIPDPatientList_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
          //  if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
          //  {
          //      this.Close();
          //      return;
          //  }
            //폼 기본값 세팅 등
          //  ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpDate.Value = Convert.ToDateTime(strDTP);

            cboGubun.Items.Clear();
            cboGubun.Items.Add("전체");//'ALL
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
            cboGubun.Items.Add("TA일반");//'55
            cboGubun.SelectedIndex = 0;

            SS1_Sheet1.Columns[15].Visible = true;
            SS1_Sheet1.Rows.Count = 0;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
          
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            string strJuso = "";
            btnPrint.Enabled = false;
            try
            {
                SQL = "";
                SQL = "SELECT a.Pano,b.SName,a.Bi,a.DeptCode,a.DrCode,c.DrName,a.WardCode,a.RoomCode,a.Sex,a.GbGamek,";
                SQL = SQL + ComNum.VBLF + " a.Age,a.Religion,TO_CHAR(a.OutDate,'YYYY-MM-DD') OutDate,b.Jumin1,b.Jumin2,b.Jumin3,";
                SQL = SQL + ComNum.VBLF + " b.ZipCode1,b.ZipCode2,b.Juso,b.Tel,b.Remark,a.IPDNO,a.JobSabun ";
                SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER a,BAS_PATIENT b,BAS_DOCTOR c ";
                SQL = SQL + ComNum.VBLF + "WHERE a.InDate>=TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.InDate<=TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "  AND a.GbSTS <> '9' ";// '입원취소는 제외

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
                SQL = SQL + ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";

                if (rdoOptSORT0.Checked == true)// Then '성명순
                    SQL = SQL + ComNum.VBLF + " ORDER BY b.SName,a.Pano ";
                else if (rdoOptSORT1.Checked == true)// Then '병동,호실
                    SQL = SQL + ComNum.VBLF + " ORDER BY a.WardCode,a.RoomCode,b.SName ";
                else
                    SQL = SQL + ComNum.VBLF + " ORDER BY a.DeptCode,a.DrCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                SS1_Sheet1.Rows.Count = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 0].Text = i + 1.ToString();
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["pano"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["sname"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", dt.Rows[i]["bi"].ToString().Trim());
                    SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["deptcode"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["drname"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["roomcode"].ToString().Trim();

                    SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["age"].ToString().Trim() + "/" + dt.Rows[i]["sex"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 8].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_종교구분", dt.Rows[i]["Religion"].ToString().Trim());
                    SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + dt.Rows[i]["Jumin2"].ToString().Trim() + "******";

                    strJuso = clsVbfunc.GetBASMail(clsDB.DbCon, dt.Rows[i]["ZipCode1"].ToString().Trim() + dt.Rows[i]["ZipCode2"].ToString().Trim());

                    SS1_Sheet1.Cells[i, 10].Text = strJuso + " " + dt.Rows[i]["juso"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 11].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["JobSabun"].ToString().Trim());
                    SS1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Tel"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 14].Text = dt.Rows[i]["outdate"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 15].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 16].Text = dt.Rows[i]["GbGamek"].ToString().Trim();
                }

                btnPrint.Enabled = true;
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
                btnPrint.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


                strTitle = "재  원  자  명  부";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
