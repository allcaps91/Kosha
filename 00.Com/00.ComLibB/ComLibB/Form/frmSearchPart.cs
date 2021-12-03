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

/// <summary>
/// Description : Part 조회 및 출력
/// Author : 김형범
/// Create Date : 2017.06.19
/// </summary>
/// <history>
/// 완료
/// </history>
namespace ComLibB
{
    /// <summary> 조(Part) 조회 & 출력 </summary>
    public partial class frmSearchPart : Form
    {
        /// <summary> 조(Part) 조회 & 출력 </summary>
        public frmSearchPart()
        {
            InitializeComponent();
        }

        void frmSearchPart_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            //ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            int j = 0;
            int k = 0;
            string strPart = "";
            string strID = "";
            string strName = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            try
            {
                SQL = "";
                SQL = "SELECT IDNUMBER, NAME, PART  FROM " + ComNum.DB_PMPA + "BAS_PASS ";
                SQL = SQL + ComNum.VBLF + "WHERE ProgramID = ' ' ";
                SQL = SQL + ComNum.VBLF + "  AND Part > ' ' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Part,IdNumber ";

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strID = VB.Format(dt.Rows[i]["Idnumber"].ToString().Trim(), "00000");
                    strName = dt.Rows[i]["Name"].ToString().Trim();
                    strPart = dt.Rows[i]["Part"].ToString().Trim();

                    for (j = 0; j < 12; j++)
                    {
                        for (k = 0; k < 3; k++)
                        {
                            if (strPart.Trim() == ssView_Sheet1.Cells[j, (3 * k)].Text.Trim())
                            {
                                SQL = "";
                                SQL = " SELECT * FROM " + ComNum.DB_ERP + "INSA_MST";
                                SQL = SQL + ComNum.VBLF + " WHERE SABUN IN ( '" + strID + "')";
                                SQL = SQL + ComNum.VBLF + " AND  TOIDAY IS NOT NULL";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                //if (dt.Rows.Count == 0)
                                //{
                                //    dt.Dispose();
                                //    dt = null;
                                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                                //    return;
                                //}

                                if (dt.Rows.Count > 0)
                                {
                                    //ssView_Sheet1.Cells[j, (3 * k)].ForeColor = Color.FromArgb(255, 0, 0);
                                    //ssView_Sheet1.Cells[j, (3 * k) + 1].ForeColor = Color.FromArgb(255, 0, 0);
                                    ssView_Sheet1.Cells[j, (3 * k) + 1].Text = strID;
                                    //ssView_Sheet1.Cells[j, (3 * k) + 2].ForeColor = Color.FromArgb(255, 0, 0);
                                    ssView_Sheet1.Cells[j, (3 * k) + 2].Text = strName;
                                }
                                else
                                {
                                    //ssView_Sheet1.Cells[j, (3 * k)].ForeColor = Color.FromArgb(255, 255, 255);
                                    //ssView_Sheet1.Cells[j, (3 * k) + 1].ForeColor = Color.FromArgb(0, 0, 0);
                                    ssView_Sheet1.Cells[j, (3 * k) + 1].Text = strID;
                                    //ssView_Sheet1.Cells[j, (3 * k) + 2].ForeColor = Color.FromArgb(0, 0, 0);
                                    ssView_Sheet1.Cells[j, (3 * k) + 2].Text = strName;
                                }


                            }
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                }

                dt.Dispose();
                dt = null;
                return;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/l/f1" + "PART 내 역";
            strHead2 = "/l/f2" + "인쇄일자 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":")) + "PAGE: " + "/p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
