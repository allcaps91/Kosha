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
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmDrHujin.cs
    /// Description     : 진료과장 휴진일정 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-05
    /// Update History  : try-catch문 수정
    /// <history>     
    /// D:\타병원\PSMHH\Etc\helpdesk\Frm진료과장휴진일정.frm(Frm진료과장휴진일정) => frmDrHujin.cs 으로 변경함  
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\Etc\helpdesk\Frm진료과장휴진일정.frm(Frm진료과장휴진일정)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\Etc\helpdesk\helpdesk.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    /// </summary>
    public partial class frmDrHujin : Form
    {
        string GstrSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
        
        public frmDrHujin()
        {
            InitializeComponent();
        }

        void frmDrHujin_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SetFormInit();

            //Screen_Display();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void SetFormInit()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            cboDoct.Items.Clear();
            cboDoct.Items.Add("****.전체");

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    DrCode, DrName";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                SQL = SQL + ComNum.VBLF + " AND DrCode NOT IN ('1109','1113')";
                SQL = SQL + ComNum.VBLF + " AND DRCODE NOT IN ('0104','0580','1402','1403','0581','2284') ";
                SQL = SQL + ComNum.VBLF + " AND DRDEPT1 NOT IN ('RD','HR')";
                SQL = SQL + ComNum.VBLF + " AND SUBSTR(DRCODE,3,2) <> '99'";
                SQL = SQL + ComNum.VBLF + " AND DRCODE < '7000'";               
                SQL = SQL + ComNum.VBLF + "ORDER BY DrCode,DrName  ";

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
                    cboDoct.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DrName"].ToString().Trim());
                }

                cboDoct.SelectedIndex = 0;

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;

            }
        }

        void btnReView_Click(object sender, EventArgs e)
        {
            Screen_Display();
        }

        void btnVeiw_Click(object sender, EventArgs e)
        {
            Screen_Display();
        }

        /// <summary>
        /// 스프레드 셀값 초기화
        /// </summary>
        void ssClear()
        {
            for (int i = 0; i < ssHujin_Sheet1.RowCount; i++)
            {
                for (int j = 0; j < ssHujin_Sheet1.ColumnCount; j++)
                {
                    ssHujin_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        void Screen_Display()
        {
            int i = 0;
            int nREAD = 0;
            int nRow = -1;

            string strOK = "";
            string strDrCode = "";
            string strSchDate = "";
            string strGbJin1 = "";
            string strGbJin2 = "";

            bool bColor = false;

            string strDrCode_Old = "";
            string strSchDate_Old = "";
            string strGbJin1_Old = "";
            string strGbJin2_Old = "";
            string strComDate = "";
            string strComDoct = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssClear();

            ssHujin_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    a.DrCode,TO_CHAR(a.SCHDATE,'YYYY-MM-DD') SchDate,a.GbJin,a.GbJin2";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE a, " + ComNum.DB_PMPA + "BAS_DOCTOR b, " + ComNum.DB_PMPA + "BAS_JOB c";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                //SQL = SQL + ComNum.VBLF + " AND a.SCHDATE>=TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND a.SCHDATE>=TO_DATE('" + 20170101 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND a.DrCode = b.DrCode    " ;
                SQL = SQL + ComNum.VBLF + " AND a.SchDate = c.JobDate ";
                SQL = SQL + ComNum.VBLF + " AND c.HOLYDAY <> '*'  ";
                SQL = SQL + ComNum.VBLF + " AND a.GbJin NOT IN ('1','2') ";
                SQL = SQL + ComNum.VBLF + " AND a.GbJin2 NOT IN ('1','2') ";
                SQL = SQL + ComNum.VBLF + " AND (b.Tour IS NULL OR b.Tour != 'Y') ";                
                if (VB.Left(cboDoct.SelectedItem.ToString().Trim(), 4) != "****")
                {
                    SQL = SQL + ComNum.VBLF + "AND a.DRCODE ='" + VB.Left(cboDoct.SelectedItem.ToString().Trim(), 4) + "'  ";
                }
                SQL = SQL + ComNum.VBLF + " AND a.DRCODE NOT IN ('0104','0580','1402','1403','0581','2284')  ";
                SQL = SQL + ComNum.VBLF + " AND b.DRDEPT1 NOT IN ('RD','HR') ";
                SQL = SQL + ComNum.VBLF + " AND SUBSTR(a.DRCODE,3,2) <> '99' "; // 진료과 대표코드는 안보임
                SQL = SQL + ComNum.VBLF + " AND a.DRCODE < '7000' "; // 응급실의사는 안보임
                SQL = SQL + ComNum.VBLF + "Group By a.DrCode,a.SCHDATE,a.GbJin,a.GbJin2  ";
                SQL = SQL + ComNum.VBLF + "Order By a.DrCode,a.SCHDATE";

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
                nREAD = dt.Rows.Count;
                if (nREAD > 0)
                {
                    ssHujin_Sheet1.RowCount = dt.Rows.Count;                   
                    //ssHujin_Sheet1.RowCount = ssHujin_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) -1;

                    for (i = 0; i < nREAD; i++)
                    {
                        strOK = "";
                        strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                        strSchDate = dt.Rows[i]["SCHDATE"].ToString().Trim();
                        strGbJin1 = dt.Rows[i]["GbJin"].ToString().Trim();
                        strGbJin2 = dt.Rows[i]["GbJin2"].ToString().Trim();

                        if (strDrCode == "2118")
                        {
                            i = i;
                        }

                        if (strDrCode_Old == "")
                        {
                            strDrCode_Old = dt.Rows[i]["DrCode"].ToString();
                        }

                        if (strSchDate_Old == "")
                        {
                            strSchDate_Old = dt.Rows[i]["SCHDATE"].ToString();
                        }

                        if (strGbJin1_Old == "")
                        {
                            strGbJin1_Old = dt.Rows[i]["GbJin"].ToString();
                        }

                        if (strGbJin2_Old == "")
                        {
                            strGbJin2_Old = dt.Rows[i]["GbJin2"].ToString();
                        }

                        if (strDrCode != strDrCode_Old)
                        {
                            strSchDate_Old = "";
                            strGbJin1_Old = "";
                            strGbJin2_Old = "";
                        }

                        if (strSchDate_Old == DATE_ADD(strSchDate, -1))
                        {
                            strOK = "OK";
                        }

                        if (strOK == "OK")
                        {
                            strComDate = ssHujin_Sheet1.Cells[ssHujin_Sheet1.RowCount -1, 1].Text;
                            strComDoct = ssHujin_Sheet1.Cells[ssHujin_Sheet1.RowCount -1, 0].Text;

                            if (strComDate != strSchDate_Old || strComDoct != READ_BAS_Doctor(strDrCode).Trim())
                            {
                                nRow = nRow + 1;
                                if(ssHujin_Sheet1.RowCount < nRow)
                                {
                                    ssHujin_Sheet1.RowCount = nRow;
                                }

                                ssHujin_Sheet1.Cells[nRow, 1].Text = strSchDate_Old;
                                ssHujin_Sheet1.Cells[nRow, 0].Text = READ_BAS_Doctor(strDrCode_Old);
                                ssHujin_Sheet1.Cells[nRow, 2].Text = READ_DRSCH_JIN_GUBUN(strGbJin1_Old);
                                ssHujin_Sheet1.Cells[nRow, 3].Text = READ_DRSCH_JIN_GUBUN(strGbJin2_Old);
                            }

                            nRow = nRow + 1;
                            if (ssHujin_Sheet1.RowCount < nRow)
                            {
                                ssHujin_Sheet1.RowCount = nRow;
                            }

                            ssHujin_Sheet1.Cells[nRow, 1].Text = strSchDate;
                            ssHujin_Sheet1.Cells[nRow, 0].Text = READ_BAS_Doctor(strDrCode);
                            ssHujin_Sheet1.Cells[nRow, 2].Text = READ_DRSCH_JIN_GUBUN(strGbJin1);
                            ssHujin_Sheet1.Cells[nRow, 3].Text = READ_DRSCH_JIN_GUBUN(strGbJin2);

                           
                        }

                        strDrCode_Old = strDrCode;
                        strSchDate_Old = strSchDate;
                        strGbJin1_Old = strGbJin1;
                        strGbJin2_Old = strGbJin2;
                    }
                }
                dt.Dispose();
                dt = null;

                strDrCode = "";

                for (i = 0; i < ssHujin_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    if (strDrCode == "")
                    {
                        strDrCode = ssHujin_Sheet1.Cells[i, 0].Text;
                    }

                    if (strDrCode != ssHujin_Sheet1.Cells[i, 0].Text)
                    {
                        strDrCode = ssHujin_Sheet1.Cells[i, 0].Text;

                        if (bColor == true)
                        {
                            bColor = false;
                        }
                        else
                            bColor = true;
                    }

                    if (bColor == true)
                    {
                        ssHujin_Sheet1.Rows[i].BackColor = Color.LightGray;

                    }
                    else
                    {
                        ssHujin_Sheet1.Rows[i].BackColor = Color.White;
                    }
                }

                for(i = 0; i < ssHujin_Sheet1.RowCount; i++)
                {
                    for(int j = 0; j < ssHujin_Sheet1.ColumnCount; j++)
                    {
                        if(ssHujin_Sheet1.Cells[i, j].Text == "")
                        {
                            i--;
                            ssHujin_Sheet1.RowCount -= 1;
                        }
                    }
                }
                
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);                
            }
        }

        string DATE_ADD(string ArDate, int ArgIlsu)
        {
            DataTable dt = null;
            string SqlErr = "";
            string SQL = "";
            string rtnVal = "";

            if(VB.Len(ArDate) != 10)
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(TO_DATE('" + ArDate + "','YYYY-MM-DD')";
                if (ArgIlsu < 0)
                {
                    SQL = SQL + ComNum.VBLF + "-" + ArgIlsu * -1;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "+" + ArgIlsu;
                }
                SQL = SQL + ComNum.VBLF + ",'YYYY-MM-DD') AddDate";
                SQL = SQL + ComNum.VBLF + "FROM DUAL";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                    
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");                   
                }


                if (dt.Rows.Count == 1)
                {
                    rtnVal = dt.Rows[0]["AddDate"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return "";
            }

        }

        string READ_BAS_Doctor(string ArgDrCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if(ArgDrCode == "")
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DrName";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "    WHERE DrCode='" + ArgDrCode + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                    
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                }


                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DrName"].ToString().Trim();
                }
                else
                    rtnVal = "";

                dt.Dispose();
                dt = null;
                return rtnVal;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return "";
            }

        }

        string READ_DRSCH_JIN_GUBUN(string ArgJin)
        {
            string rtnVal = "";

            switch (ArgJin)
            {
                case "1":
                    rtnVal = "진료";
                    break;
                case "2":
                    rtnVal = "수술";
                    break;
                case "3":
                    rtnVal = "특검";
                    break;
                case "4":
                    rtnVal = "휴진";
                    break;
                case "5":
                    rtnVal = "학회";
                    break;
                case "6":
                    rtnVal = "휴가";
                    break;
                case "7":
                    rtnVal = "출장";
                    break;
                case "8":
                    rtnVal = "기타";
                    break;
                case "9":
                    rtnVal = "OFF";
                    break;
                case "A":
                    rtnVal = "협진";
                    break;

                default:
                    rtnVal = "";
                    break;
            }
            return rtnVal;
        }
    }
}
