using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComLibB; //기본 클래스
using ComPmpaLibB; //기본 클래스


namespace ComPmpaLibB
{
    public partial class frmPmpaSanOpdTong : Form
    {
        public frmPmpaSanOpdTong()
        {
            InitializeComponent();
        }

        private void frmSanOpdTong_Load(object sender, EventArgs e)
        {
      
            ComFunc.ReadSysDate(clsDB.DbCon);
            ComFunc.Form_Center(this);
            screenClear();
            DtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-30);
            DtpEDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
        }
        private void screenClear()
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 30;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            screenClear();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string printDate = "";
            string jobMan = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            printDate = clsPublic.GstrSysDate;
          

            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs0";
            strFont2 = "/fn\"바탕체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/f1" + VB.Space(10) + "산재 통원 확인서 ( " + DtpSDate.Value.ToString("yyyy-MM") + ")" + "/n" + "          " + "/n";
            strHead2 = "/l/f2" + "작업일자 : " + printDate + VB.Space(13) + "<의료기관명: 포 항 성 모 병 원>  " + VB.Space(20) + "PAGE : " + " /P";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 10;
            ssView_Sheet1.PrintInfo.Margin.Right = 10;
            ssView_Sheet1.PrintInfo.Margin.Top = 10;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 10;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt2 = null;

            int i = 0;
            string strSdate = "";
            string strEdate = "";

            screenClear();

            strSdate = DtpSDate.Value.ToString("yyyy-MM-dd"); //+ "-01";
            strEdate = DtpEDate.Value.ToString("yyyy-MM-dd"); //+ "-" + DateTime.DaysInMonth(dtpDate.Value.Year, dtpDate.Value.Month);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.SNAME, A.PANO, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";

           
                SQL = SQL + ComNum.VBLF + "A.DEPTCODE,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode ) DrCode, decode(A.JIN,'0','진료','물리치료') JIN, B.JUMIN1||'-'||B.JUMIN2 JUMIN  ";
                SQL = SQL + ComNum.VBLF + " FROM OPD_MASTER A, BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + " WHERE B.PANO = A.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE >=TO_DATE('" + strSdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                if (txtPano.Text !="")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.Pano =  '" + txtPano.Text.Trim() + "' ";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.JIN IN ('0','5','8','H')  ";
                SQL = SQL + ComNum.VBLF + "   AND A.REP <> '#'  ";
                SQL = SQL + ComNum.VBLF + "   AND A.BI = '31' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.Pano,a.BDATE ";
              

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JUMIN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["JIN"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
         
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                btnSearch.Focus();
            }
        }
    }
}
