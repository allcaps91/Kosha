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

namespace ComLibB
{
    /// <summary> 환자 등록번호 찾기 </summary>
    public partial class frmSearchPano : Form
    {
        string GstrHelpCode = ""; //gloval
        string GstrBarDept = ""; //global
        string GstrSysDate = ""; //global

        public delegate void GetData(string GstrHelpCode); //델리게이트 선언문
        public event GetData rGetData; //델리게이트 이벤트 선언문

        public delegate void EventClose();
        public event EventClose rEventClose;

        /// <summary> 환자 등록번호 찾기 </summary>
        public frmSearchPano()
        {
            InitializeComponent();
        }

        void frmPanoSearch_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            //ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            string strIpdOpd = string.Empty;
            int intViewNo = 0;
            string strData = string.Empty;

            //if (GstrHelpCode == "")
            //{
            //    return;
            //}

            strIpdOpd = VB.Pstr(GstrHelpCode, "|", 1);
            intViewNo = Convert.ToInt32(VB.Val(VB.Pstr(GstrHelpCode, "|", 2)));
            strData = VB.Pstr(GstrHelpCode, "|", 3);

            if (strIpdOpd == "IPD")
            {
                optIO0.Checked = true;
            }

            if (strIpdOpd == "OPD")
            {
                optIO1.Checked = true;
            }

            if (strIpdOpd == "GUNJIN")
            {
                optIO3.Checked = true;
            }

            if (strIpdOpd == "ETCPANO")
            {
                optIO4.Checked = true;
            }

            if (strIpdOpd == "HICPATIENT")
            {
                optIO5.Checked = true;
            }

            if (optIO0.Checked == true)
            {
                optView3.Checked = true;
                optView2.Enabled = true;
                optView3.Text = "입원일자";
            }
            else if (optIO1.Checked == true)
            {
                optView3.Checked = true;
                optView2.Enabled = false;
                optView3.Text = "접수일자";
            }
            else if (optIO3.Checked == true)
            {
                optView3.Checked = true;
                optView2.Enabled = false;
                optView3.Text = "건진일자";
            }


            switch (intViewNo)
            {
                case 0:
                    optView0.Checked = true;
                    break;
                case 4:
                    optView4.Checked = true;
                    break;
                case 2:
                    optView2.Checked = true;
                    break;
                case 3:
                    optView3.Checked = true;
                    break;
            }

            if (GstrBarDept != "")
            {
                optIO1.Checked = true;
            }


            txtFind.Text = strData;

            if (txtFind.Text != "")
            {
                btnView();
            }

            GstrHelpCode = "";
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            btnView();
        }

        //TODO: READ_DrName 모듈(Exam00.bas)
        void btnView()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int intRow = 0;
            string strFDate = string.Empty;
            string strData = string.Empty;
            string strJumin1 = string.Empty;
            string strJumin2 = string.Empty;
            int intAge = 0;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            int i = 0;

            try
            {
                ComFunc.ReadSysDate(clsDB.DbCon);
                strData = txtFind.Text.Trim();

                if (strData == "")
                {
                    ComFunc.MsgBox("찾으실 자료가 공란입니다.", "자료오류");
                    txtFind.Focus();
                    return;
                }

                strFDate = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-10).ToShortDateString();

                if (optIO0.Checked == true)
                {
                    SQL = "";
                    SQL = "SELECT Pano,Sname,Age,Sex,RoomCode,WardCode,";
                    SQL = SQL + ComNum.VBLF + " Bi,DeptCode,DrCode ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";

                    if (optView0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Sname LIKE'%" + strData + "%' ";
                    }
                    else if (optView2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE WardCode='" + VB.UCase(strData) + "' ";
                    }
                    else if (optView3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE TO_CHAR(InDate,'YYYY-MM-DD')='" + strData + "' ";
                    }
                    else if (optView4.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strData + "' ";
                    }
                    else
                    {
                        ComFunc.MsgBox("찾기방법 선택오류", "선택오류");
                        return;
                    }

                    SQL = SQL + ComNum.VBLF + "  AND AmSet1 = '0' ";
                    SQL = SQL + ComNum.VBLF + "  AND GBSTS IN ('0', '2', '3', '4') ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY Sname,Pano ";
                }
                else if (optIO1.Checked == true)
                {
                    SQL = "";
                    SQL = "SELECT Pano,Sname,Age,Sex,Bi,DeptCode,DrCode, TO_CHAR(ActDate, 'YYYY-MM-DD') ADate ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_Master ";

                    if (optView0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND Sname = '" + strData + "' ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY Sname,Pano,JTime DESC ";
                    }
                    else if (optView3.Checked == true)
                    {
                        SQL = SQL + "WHERE ActDate = TO_DATE('" + strData + "','YYYY-MM-DD') ";
                        SQL = SQL + "ORDER BY Sname,Pano,DeptCode ";
                    }
                    else if (optView4.Checked == true)
                    {
                        SQL = SQL + "WHERE Pano = '" + strData + "' ";
                        SQL = SQL + "  AND ActDate >= TO_DATE('" + strFDate + "', 'YYYY-MM-DD') ";
                        SQL = SQL + "ORDER BY Pano,Sname,JTime DESC ";
                    }
                    else
                    {
                        ComFunc.MsgBox("찾기방법 선택오류", "선택오류");
                        return;
                    }
                }
                else if (optIO2.Checked == true)
                {
                    SQL = "";
                    SQL = "SELECT Pano,Sname,Jumin1,Jumin2, JUMIN3, Sex ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";

                    if (optView0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Sname='" + strData + "' ";
                    }
                    else if (optView4.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strData + "' ";
                    }
                    else
                    {
                        ComFunc.MsgBox("찾기방법 선택오류", "선택오류");
                        return;
                    }

                    SQL = SQL + ComNum.VBLF + "ORDER BY Sname,Pano ";
                }
                else if (optIO3.Checked == true)
                {
                    SQL = "";
                    SQL = "     SELECT PTno Pano,Sname,AGE,Sex ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HEA_JEPSU ";

                    if (optView0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE SName = '" + strData + "' ";
                    }
                    else if (optView4.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Ptno = '" + strData + "' ";
                    }
                    else
                    {
                        ComFunc.MsgBox("찾기방법 선택오류", "선택오류");
                        return;
                    }

                    SQL = SQL + ComNum.VBLF + "ORDER BY Sname,WRTNO ";
                }
                else if (optIO4.Checked == true)
                {
                    SQL = "";
                    SQL = "SELECT Pano Pano,SName,Sex,Jumin1,Jumin2, JUMIN3  ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_ETCPANO ";

                    if (optView0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE SName = '" + strData + "' ";
                    }
                    else if (optView4.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Pano = " + strData + " ";
                    }
                    else
                    {
                        ComFunc.MsgBox("찾기방법 선택오류", "선택오류");
                        return;
                    }

                    SQL = SQL + ComNum.VBLF + "ORDER BY Sname,Pano ";
                }
                else if (optIO5.Checked == true)
                {
                    SQL = "";
                    SQL = "     SELECT PTno Pano,Sname,AGE,Sex ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_JEPSU ";

                    if (optView0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE SName = '" + strData + "' ";
                    }
                    else if (optView4.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strData + "' ";
                    }
                    else
                    {
                        ComFunc.MsgBox("찾기방법 선택오류", "선택오류");
                        return;
                    }

                    SQL = SQL + ComNum.VBLF + "ORDER BY Sname,WRTNO ";
                }

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

                intRow = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (ssView_Sheet1.RowCount < intRow)
                        {
                            ssView_Sheet1.RowCount = intRow + 10;
                        }
                        ssView_Sheet1.Cells[intRow, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();

                        if (optIO0.Checked == true)
                        {
                            ssView_Sheet1.Cells[intRow, 2].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 3].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[intRow, 2].Text = "";
                            ssView_Sheet1.Cells[intRow, 3].Text = "";
                        }

                        ssView_Sheet1.Cells[intRow, 4].Text = "";

                        if (optIO0.Checked == true || optIO1.Checked == true)
                        {
                            ssView_Sheet1.Cells[intRow, 5].Text = dt.Rows[i]["Age"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 6].Text = dt.Rows[i]["Sex"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 7].Text = dt.Rows[i]["Bi"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 8].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 9].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                            //ssView_Sheet1.Cells[intRow, 10].Text = READ_DrName(dt.Rows[i]["DrCode"].ToString().Trim());  TODO: READ_DrName 모듈(Exam00.bas)
                        }
                        else if (optIO2.Checked == true || optIO4.Checked == true)
                        {
                            strJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();

                            if (dt.Rows[i]["Jumin3"].ToString().Trim() == "")
                            {
                                strJumin2 = dt.Rows[i]["Jumin2"].ToString().Trim();
                            }
                            else
                            {
                                strJumin2 = clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());
                            }

                            //intAge = clsVbfunc.AGE_YEAR_GESAN(strJumin1 & strJumin2, GstrSysDate);  //TODO:AGE_YEAR_GESAN 모듈(VbFunction)

                            intAge = ComFunc.AgeCalc(clsDB.DbCon, strJumin1 + strJumin2);

                            ssView_Sheet1.Cells[intRow, 4].Text = strJumin1 + "-" + strJumin2;
                            ssView_Sheet1.Cells[intRow, 5].Text = intAge.ToString("00");
                            ssView_Sheet1.Cells[intRow, 6].Text = "M";

                            if (VB.Left(strJumin2, 1) == "2")
                            {
                                ssView_Sheet1.Cells[intRow, 6].Text = "F";
                            }

                            ssView_Sheet1.Cells[intRow, 7].Text = "";
                            ssView_Sheet1.Cells[intRow, 8].Text = "";
                            ssView_Sheet1.Cells[intRow, 9].Text = "";
                            ssView_Sheet1.Cells[intRow, 10].Text = "";
                        }
                        else if (optIO3.Checked == true)
                        {
                            ssView_Sheet1.Cells[intRow, 0].Text = VB.Format(dt.Rows[i]["Pano"].ToString().Trim(), "00000000");
                            intAge = Convert.ToInt32(dt.Rows[i]["Age"].ToString().Trim());
                            ssView_Sheet1.Cells[intRow, 5].Text = intAge.ToString();
                            ssView_Sheet1.Cells[intRow, 6].Text = "M";

                            if (VB.Left(strJumin2, 1) == "2")
                            {
                                ssView_Sheet1.Cells[intRow, 6].Text = "F";
                            }

                            ssView_Sheet1.Cells[intRow, 7].Text = "61";
                            ssView_Sheet1.Cells[intRow, 8].Text = "TO";
                            ssView_Sheet1.Cells[intRow, 9].Text = "7102";
                            //ssView_Sheet1.Cells[intRow, 10].Text = READ_DrName("7102");  TODO: READ_DrName 모듈(Exam00.bas)
                        }
                        else if (optIO5.Checked == true)
                        {
                            ssView_Sheet1.Cells[intRow, 5].Text = dt.Rows[i]["Age"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 6].Text = dt.Rows[i]["Sex"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 7].Text = "";
                            ssView_Sheet1.Cells[intRow, 8].Text = "";
                            ssView_Sheet1.Cells[intRow, 9].Text = "";
                            ssView_Sheet1.Cells[intRow, 10].Text = "";
                        }

                        if (optIO1.Checked == true)
                        {
                            ssView_Sheet1.Cells[intRow, 11].Text = dt.Rows[i]["ADate"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[intRow, 11].Text = "";
                        }

                        intRow += 1;
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = intRow;

                if (intRow == 0)
                {
                    ComFunc.MsgBox("원하시는 자료가 1건도 없습니다.", "자료없슴");
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void optIO_CheckedChanged(object sender, EventArgs e)
        {
            if (optIO0.Checked == true)
            {
                optView2.Enabled = true;
                optView3.Text = "입원일자";
            }
            else if (optIO1.Checked == true)
            {
                optView2.Enabled = false;
                optView3.Text = "접수일자";
            }
            else if (optIO2.Checked == true)
            {
                optView2.Enabled = false;
                optView3.Text = "";
            }
            else if (optIO3.Checked == true)
            {
                optView2.Enabled = false;
                optView3.Text = "";
            }
        }

        void optView_CheckedChanged(object sender, EventArgs e)
        {
            if (optView0.Checked == true || optView2.Checked == true || optView4.Checked == true)
            {
                txtFind.Text = "";
            }
            else if (optView3.Checked == true)
            {
                txtFind.Text = GstrSysDate;
            }
        }

        void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            GstrHelpCode = "";
            
            for (i=0;i<11;i++)
            {
                GstrHelpCode = GstrHelpCode + ssView_Sheet1.Cells[e.Row, i].Text + "|";
            }

            if (GstrHelpCode != "")
            {
                GstrHelpCode = VB.Left(GstrHelpCode, VB.Len(GstrHelpCode) - 1);
            }

            rGetData(GstrHelpCode);

            rEventClose();
        }

        void txtFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
