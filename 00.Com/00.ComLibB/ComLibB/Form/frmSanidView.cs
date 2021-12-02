using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSanidView
    /// File Name : frmSanidView.cs
    /// Title or Description : 후유, 특진환자 조회 - 외래
    /// Author : 박창욱
    /// Create Date : 2017-06-02
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    ///     06-15 박창욱 : txtFdate, txtTdate를 dtpFdate, dtpTdate로 변환
    /// </summary>
    /// <history>  
    /// VB\busanid12.frm(FrmSanidView) -> frmSanidView.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busanid\busanid12.frm(FrmSanidView)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busanid\\busanid.vbp
    /// </vbp>
    public partial class frmSanidView : Form
    {
        public frmSanidView()
        {
            InitializeComponent();
        }

        private void frmSanidView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFdate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpTdate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";

            string strHead1 = "";
            string strHead2 = "";

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"20\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = strHead1 + "/c" + "후유, 특진환자 조회(외래)";
            strHead2 = "/l/f2" + "작업일자 : " + dtpFdate.Value.ToString("yyyy-MM-dd") + "~" + dtpTdate.Value.ToString("yyyy-MM-dd")
                               + VB.Space(90) + " PAGE : " + " /P" + "/n";

            if (rdoTreat.Checked == true)
            {
                strHead2 = strHead2 + "구분 :  특진" + "/n";
            }
            if (rdoSequela.Checked == true)
            {
                strHead2 = strHead2 + "구분 :  후유" + "/n";
            }

            //Print Body
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowColumnFooter = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ACTDATE, A.DEPTCODE , A.PANO, A.SNAME, ";
                SQL = SQL + ComNum.VBLF + "  DECODE(A.BI, '31','산재','55','자보') BI, ";
                SQL = SQL + ComNum.VBLF + "  B.DEPT1, B.DEPT2, B.DEPT3, TO_CHAR(B.DATE1,'YYYY-MM-DD') BDATE1, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(B.Date2,'YYYY-MM-DD') BDATE2, ";
                
                //GBRESULT    CHAR(1)     결과: 1.치유 2.사망 3.전원 4.중지 5.계속
                SQL = SQL + ComNum.VBLF + "  DECODE(B.GBRESULT, '1','치유', '2','사망','3','전원','4','중지','계속') GBRESULT,";
                SQL = SQL + ComNum.VBLF + "  DECODE(B.JONG,'1','특진','2','후유','3','진폐','일반') JONG ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER A, KOSMOS_PMPA.BAS_SANID B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE >= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND A.BI ='31' ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO";

                if (rdoTreat.Checked == true) { SQL = SQL + ComNum.VBLF + " AND B.JONG  = '1' "; }    //특진
                if (rdoSequela.Checked == true) { SQL = SQL + ComNum.VBLF + " AND B.JONG  = '1' "; }  //후유
                if (rdoConiosis.Checked == true) { SQL = SQL + ComNum.VBLF + " AND B.JONG  = '1' "; } //진폐

                SQL = SQL + ComNum.VBLF + " ORDER BY A.ACTDATE, A.BI, A.DEPTCODE ";

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

                for (i = 0; i<dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["JONG"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DEPT1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DEPT2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DEPT3"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["BDATE2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["GBRESULT"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
