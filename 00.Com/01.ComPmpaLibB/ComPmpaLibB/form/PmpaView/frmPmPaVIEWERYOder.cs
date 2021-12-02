using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\IPD\iument\Frm오더보기.frm(입원당일-입원및응급실오더확인작업.frm) >> frmPmPaVIEWERYOder.cs 폼이름 재정의" />

    public partial class frmPmPaVIEWERYOder : Form
    {
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaVIEWERYOder()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWERYOder_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtpano.Text = "";

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT Pano,SName,TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,      ";
                SQL = SQL + ComNum.VBLF + " DECODE(GBSTS,'9','취소','7','퇴원','재원') GBSTS2,DeptCode  ";
                SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER                                   ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + txtpano.Text + "'            ";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(INDATE) =TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

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

                lblpanel.Text = dt.Rows[0]["Pano"].ToString().Trim() + VB.Space(2) + dt.Rows[0]["SName"].ToString().Trim() + VB.Space(2) + dt.Rows[0]["InDate"].ToString().Trim() + VB.Space(2) + dt.Rows[0]["DeptCode"].ToString().Trim() + VB.Space(2) + "현재상태:" + dt.Rows[0]["GBSTS2"].ToString().Trim();

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "  SELECT PTNO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, TO_CHAR(EntDATE,'YY/MM/DD HH24:MI') EntDATE,";
                SQL = SQL + ComNum.VBLF + " DEPTCODE,SUCODE,QTY,NAL,GBACT,GBSEND,GBPICKUP,PICKUPDATE,GBIOE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + txtpano.Text + "' ";
                SQL = SQL + ComNum.VBLF + " AND BDATE =TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (chkSuga.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND SuCode IS NOT NULL ";
                }
                if (rdogubun1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND (GBIOE IS NULL OR GBIOE='' OR GBIOE ='I' ) ";
                }

                else if (rdogubun2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND  GBIOE IN ('E','EI') ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY DEPTCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);


                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }


                //스프레드 출력문
                ssView0_Sheet1.RowCount = dt.Rows.Count;
                ssView0_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView0_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    ssView0_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    ssView0_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView0_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    ssView0_Sheet1.Cells[i, 4].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString();
                    ssView0_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["NAL"].ToString().Trim()).ToString();

                    switch (dt.Rows[i]["GBIOE"].ToString().Trim())
                    {
                        case "E":
                        case "EI":
                            switch (dt.Rows[i]["GBACT"].ToString().Trim())
                            {
                                case "*":
                                    ssView0_Sheet1.Cells[i, 6].Text = "미수납";
                                    break;
                                case "":
                                    ssView0_Sheet1.Cells[i, 6].Text = "수납";
                                    break;
                                default:
                                    ssView0_Sheet1.Cells[i, 6].Text = "수납?";
                                    break;
                            }
                            break;
                        default:
                            ssView0_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GBACT"].ToString().Trim();
                            break;
                    }

                    switch (dt.Rows[i]["GBSEND"].ToString().Trim())
                    {
                        case "*":
                            ssView0_Sheet1.Cells[i, 7].Text = "미전송";
                            break;
                        case "":
                            ssView0_Sheet1.Cells[i, 7].Text = "전송";
                            break;
                        default:
                            ssView0_Sheet1.Cells[i, 7].Text = "전송?";
                            break;
                    }

                    ssView0_Sheet1.Cells[i, 8].Text = dt.Rows[i]["GBPICKUP"].ToString().Trim();

                    switch (dt.Rows[i]["GBIOE"].ToString().Trim())
                    {
                        case "E":
                            ssView0_Sheet1.Cells[i, 9].Text = "ER";
                            break;
                        case "EI":
                            ssView0_Sheet1.Cells[i, 9].Text = "ER+";
                            break;
                        default:
                            ssView0_Sheet1.Cells[i, 9].Text = "입원";
                            break;
                    }

                    ssView0_Sheet1.Cells[i, 10].Text = dt.Rows[i]["EntDate"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;

                //'외래오더
                SQL = "  SELECT PTNO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,TO_CHAR(EntDATE,'YY/MM/DD HH24:MI') EntDATE,";
                SQL = SQL + ComNum.VBLF + " DEPTCODE,SUCODE,QTY,NAL,GBSUNAP ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + txtpano.Text + "' ";
                SQL = SQL + ComNum.VBLF + " AND BDATE =TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (chkSuga.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND SuCode IS NOT NULL ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY DEPTCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }


                ssView1_Sheet1.RowCount = dt.Rows.Count;
                ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 4].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString();
                    ssView1_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["NAL"].ToString().Trim()).ToString();

                    switch (dt.Rows[i]["GbSunap"].ToString().Trim())
                    {
                        case "0":
                            ssView1_Sheet1.Cells[i, 6].Text = "미수납";
                            break;
                        case "1":
                            ssView1_Sheet1.Cells[i, 6].Text = "수납";
                            break;
                        case "2":
                            ssView1_Sheet1.Cells[i, 6].Text = "취소";
                            break;
                    }
                    ssView1_Sheet1.Cells[i, 7].Text = "외래";
                    ssView1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["EntDate"].ToString().Trim();
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
    }
}
