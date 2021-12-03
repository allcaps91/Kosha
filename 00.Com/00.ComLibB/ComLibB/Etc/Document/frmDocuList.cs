using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmDocuList : Form, MainFormMessage
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

        public frmDocuList()
        {
            InitializeComponent();
        }

        public frmDocuList(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmDocuList(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        private void frmDocuList_Load(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strData = "";
            string strBuCode = "";
            string strBuCodeS = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            DtpFrDate.Value = DateTime.Parse(DateTime.Parse(clsPublic.GstrSysDate).ToString("yyyy-MM-01"));
            DtpToDate.Value = DateTime.Parse(clsPublic.GstrSysDate);

            ComboFrBuse.Items.Clear();

            SQL = "SELECT BUSE FROM ADMIN.INSA_MST WHERE SABUN = '" + clsType.User.Sabun.PadLeft(5, '0') + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strBuCode = dt.Rows[0]["BUSE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            switch (clsVbfunc.BAS_BUSE_TREE_UP(clsDB.DbCon, strBuCode))
            {
                case "044510":
                case "044520":
                case "101772":
                    strBuCode = "044501";
                    break;
            }

            strBuCodeS = clsVbfunc.READ_BAS_BUSE(clsDB.DbCon, clsVbfunc.BAS_BUSE_TREE_UP(clsDB.DbCon, strBuCode));
            strBuCodeS = strBuCodeS + "               ." + clsVbfunc.BAS_BUSE_TREE_UP(clsDB.DbCon, strBuCode);

            ComboFrBuse.Items.Add(strBuCodeS);
            ComboFrBuse.SelectedIndex = 0;
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
            strHead1 = "/n/n/f1/C << 문서 접수 대장 " + DtpFrDate.Value.ToString("yyyy-MM-dd") + "/" + DtpToDate.Value.ToString("yyyy-MM-dd") + " >>" + "/n/n/n/n";
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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

            strFrDate = DtpFrDate.Value.ToString("yyyy-MM-dd");
            strToDate = DtpToDate.Value.ToString("yyyy-MM-dd");

            cFrBuse = VB.Right(VB.Trim(ComboFrBuse.Text), 6);
            //cToBuse = VB.Mid(VB.Trim(ComboToBuse.Text), 41, 6);

            ss1.ActiveSheet.RowCount = 0;

            SQL = "SELECT SEQNO, DOCUNO, PLACENAME, DOCUNAME, ADMIN.BAS_BUSE.NAME BUSENAME, OUTMAN, OUTTIME, ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(WORKDAY,'YYYY-MM-DD') WORKDAY ";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.INSA_DOCU1, ADMIN.BAS_BUSE ";
            SQL = SQL + ComNum.VBLF + " WHERE BUSE = BUCODE ";
            SQL = SQL + ComNum.VBLF + "   AND TO_CHAR(WORKDAY, 'yyyy-mm-dd') >= '" + VB.Trim(strFrDate) + "'";
            SQL = SQL + ComNum.VBLF + "   AND TO_CHAR(WORKDAY, 'yyyy-mm-dd') <= '" + VB.Trim(strToDate) + "'";
            SQL = SQL + ComNum.VBLF + "   AND GUBUN = '1'";
            SQL = SQL + ComNum.VBLF + "   AND BUSE = '" + cFrBuse + "'";
            //SQL = SQL + ComNum.VBLF + " AND   Buse <= '" + cToBuse + "' ";
            //SQL = SQL + ComNum.VBLF + " AND   BUSEGBN = '" + strBuseGbn + "' ";
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
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1.ActiveSheet.RowCount = 0;
                    ss1.ActiveSheet.RowCount = dt.Rows.Count;

                    ss1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["WORKDAY"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DOCUNO"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["PLACENAME"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DOCUNAME"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["BUSENAME"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["OUTMAN"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["OUTTIME"].ToString().Trim();
                }
            }
        }

        private void frmDocuList_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmDocuList_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }
    }
}
