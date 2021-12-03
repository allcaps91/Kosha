using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmDocu_03 : Form, MainFormMessage
    {
        #region //MainFormMessage
        public string mPara1 = "";
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {
            mPara1 = strPara;
        }
        #endregion //MainFormMessage

        string Show_yn = "";
        string cCode = "";
        string cName = "";
        string strBuseGbn = "";

        public frmDocu_03()
        {
            InitializeComponent();
        }

        public frmDocu_03(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmDocu_03(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }
        private void frmDocu_03_Load(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ComFunc.ReadSysDate(clsDB.DbCon);

            FrDate.Value = DateTime.Parse(DateTime.Parse(clsPublic.GstrSysDate).ToString("yyyy-MM-01"));
            ToDate.Value = DateTime.Parse(clsPublic.GstrSysDate);


            SQL = "SELECT BUCODE, NAME FROM ADMIN.BAS_BUSE ";
            SQL = SQL + ComNum.VBLF + " WHERE BUCODE IN ('033101','044101','044201','044301','044501','055100','055200', '066101','077101', '055201', '070101',";
            //'간호부  약제과   기록실    영양실  건강관리  방사선   임상병리  관리과   비서실    검사실    기획행정과
            SQL = SQL + ComNum.VBLF + "                   '077501','078201','077601','077201','077301','077401','088100', '077701',         ";
            //'전산실   심사계   도서실   총무과   경리과   원무과   원목실    QI실
            SQL = SQL + ComNum.VBLF + "                   '044401','044411','055301','077901','078101', '078000' ,'088201', '076001','076010','101730', '078300','101768') ";
            //'정신의료 임상심리                    QI실    감염관리실  장례식장 적정관리실, 구매과 어린이집,고객지원과,이념실
            SQL = SQL + ComNum.VBLF + " ORDER BY BUCODE ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cName = dt.Rows[i]["NAME"].ToString().Trim();
                    cCode = dt.Rows[i]["BUCODE"].ToString().Trim();
                    ComboFrBuse.Items.Add(cName + VB.Space(20) + cCode);
                    ComboToBuse.Items.Add(cName + VB.Space(20) + cCode);
                }
            }
            ComboFrBuse.SelectedIndex = 0;
            ComboToBuse.SelectedIndex = 0;

            Show_yn = "n";
            if (clsType.User.Sabun.Equals("04349")) strBuseGbn = "1";

            SQL = "SELECT BUSE FROM ADMIN.INSA_MST";
            SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                switch (dt.Rows[0]["BUSE"].ToString().Trim())
                {
                    //'Case "077201": strBuseGbn = "1"                       '총무과
                    case "077201":
                        strBuseGbn = "1";       //'기획행정과
                        break;
                    case "077401":
                    case "077402":
                        strBuseGbn = "2";       //'원무과
                        break;
                    case "044520":
                    case "101772":
                        strBuseGbn = "3";       //'일반건진
                        break;
                }
            }

            dt.Dispose();
            dt = null;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strFrDate = "";
            string strToDate = "";
            string cFrBuse = "";
            string cToBuse = "";

            ss1.ActiveSheet.RowCount = 0;

            strFrDate = FrDate.Value.ToString("yyyy-MM-dd");
            strToDate = ToDate.Value.ToString("yyyy-MM-dd");

            cFrBuse = VB.Right(VB.Trim(ComboFrBuse.Text), 6);
            cToBuse = VB.Right(VB.Trim(ComboToBuse.Text), 6);

            SQL = "SELECT A.SEQNO ASEQNO, A.DOCUNO ADOCUNO, A.PLACENAME APLACENAME, A.DOCUNAME ADOCUNAME,";
            SQL = SQL + ComNum.VBLF + " B.NAME BNAME, A.OUTMAN AOUTMAN, TO_CHAR(A.WORKDAY,'YYYY-MM-DD') AWORKDAY";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.INSA_DOCU1 A, ADMIN.BAS_BUSE B";
            SQL = SQL + ComNum.VBLF + " WHERE A.BUSE = B.BUCODE";
            SQL = SQL + ComNum.VBLF + "   AND TO_CHAR(A.WORKDAY, 'YYYY-MM-DD') >= '" + VB.Trim(strFrDate) + "'";
            SQL = SQL + ComNum.VBLF + "   AND TO_CHAR(A.WORKDAY, 'YYYY-MM-DD') <= '" + VB.Trim(strToDate) + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.GUBUN = '1'";
            SQL = SQL + ComNum.VBLF + "   AND A.BUSE >= '" + cFrBuse + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.BUSE <= '" + cToBuse + "'";
            SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ss1.ActiveSheet.RowCount = 0;
                ss1.ActiveSheet.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["ASEQNO"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["AWORKDAY"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ADOCUNO"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["APLACENAME"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["ADOCUNAME"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["BNAME"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["AOUTMAN"].ToString().Trim();
                }
            }
            Show_yn = "y";
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C << 문서 발송 대장 " + FrDate.Value.ToString("yyyy-MM-dd") + "/" + ToDate.Value.ToString("yyyy-MM-dd") + " >>" + "/n/n/n/n";
            strHead2 = "/l/f2" + "포항성모병원 / 출력일자 : " + clsPublic.GstrSysDate + " / " + "PAGE:" + "/p";

            ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;  //가로
            ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;   //세로
            ss1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ss1_Sheet1.PrintInfo.Margin.Left = 35;
            ss1_Sheet1.PrintInfo.Margin.Right = 0;
            ss1_Sheet1.PrintInfo.Margin.Top = 35;
            ss1_Sheet1.PrintInfo.Margin.Bottom = 30;
            ss1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowBorder = true;
            ss1_Sheet1.PrintInfo.ShowColor = false;
            ss1_Sheet1.PrintInfo.ShowGrid = true;
            ss1_Sheet1.PrintInfo.ShowShadows = false;
            ss1_Sheet1.PrintInfo.UseMax = false;
            ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ss1.PrintSheet(0);
        }


        private void frmDocu_03_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmDocu_03_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
