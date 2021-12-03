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
    /// File Name       : frmViewCoprName.cs
    /// Description     : 회사명별 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-14
    /// Update History  : try-catch문 수정
    /// <history>       
    /// D:\타병원\PSMHH\basic\busanid\busanid3.frm(FrmViewCoprName) => frmViewCoprName.cs 으로 변경함
    /// ComFunc.MidH 사용시 음수 관련 오류 발생 확인필요 -> 기존 Mid로 구현해놓음
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busanid\busanid3.frm(FrmViewCoprName)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busanid\busanid.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmViewCoprName : Form
    {
        string strChangeFLAG = "";
        string[] strIllCode = new string[5];
        string[] strGbIlls = new string[3];

        public frmViewCoprName()
        {
            InitializeComponent();
            
        }

        void frmViewCoprName_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtSname.Text = "";

            SCREEN_CLEAR();

            this.Activate();
            txtSname.Focus();
        }

        string READ_Bas_ILL(string ArgSang)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     IllNameK ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ILLS";
            SQL = SQL + ComNum.VBLF + "WHERE IllCode = '" + ArgSang + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
            //if (dt.Rows.Count == 0)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    ComFunc.MsgBox("해당 DATA가 없습니다.");
            //    return "";
            //}

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["IllNameK"].ToString().Trim();
            }
            return rtnVal;
        }

        void SCREEN_CLEAR()
        {
            int i, j;

            for (i = 0; i < 9; i++)
            {
                ssList2_Sheet1.Cells[i, 3].Text = "";

                if (i >= 4)
                {
                    ssList2_Sheet1.Cells[i, 0].Text = "";
                }

                if (i != 3)
                {
                    ssList2_Sheet1.Cells[i, 1].Text = "";
                }
            }

            ssList3_Sheet1.RowCount = 5;
            for (i = 0; i < ssList3_Sheet1.RowCount; i++)
            {
                for (j = 0; j < ssList3_Sheet1.ColumnCount; j++)
                {
                    ssList3_Sheet1.Cells[i, j].Text = "";
                }
            }

            ssList4_Sheet1.RowCount = 5;
            for (i = 0; i < ssList4_Sheet1.RowCount; i++)
            {
                for (j = 0; j < ssList4_Sheet1.ColumnCount; j++)
                {
                    ssList4_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i, j;

            string strShowCopr = "";
            string strShowPano = "";
            string strShowSname = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (strChangeFLAG == "**")
            {
                for (i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    for (j = 0; j < ssList_Sheet1.ColumnCount; j++)
                    {
                        ssList_Sheet1.Cells[i, j].Text = "";
                    }
                }

                strChangeFLAG = "";
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + " Sname, Pano, CoprName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SANID";
                SQL = SQL + ComNum.VBLF + "WHERE CoprName LIKE '%" + txtSname.Text.Trim() + "%'";
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

                ssList_Sheet1.RowCount = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strShowCopr = ComFunc.LeftH(dt.Rows[i]["CoprName"].ToString().Trim() + VB.Space(20), 20);
                    strShowPano = ComFunc.LeftH(dt.Rows[i]["Pano"].ToString().Trim() + VB.Space(10), 10);
                    strShowSname = ComFunc.LeftH(dt.Rows[i]["Sname"].ToString().Trim() + VB.Space(10), 10);

                    ssList_Sheet1.Cells[i, 0].Text = strShowCopr;
                    ssList_Sheet1.Cells[i, 1].Text = strShowPano;
                    ssList_Sheet1.Cells[i, 2].Text = strShowSname;
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

            
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SCREEN_CLEAR();

            string strGetPano = "";
            
            strGetPano = ssList_Sheet1.Cells[e.Row, 1].Text;
            San_IDS_Display(strGetPano);
            San_DTL_Display(strGetPano);
            San_JIN_Display(strGetPano);

        }

        void ssList_EnterCell(object sender, FarPoint.Win.Spread.EnterCellEventArgs e)
        {
            SCREEN_CLEAR();

            string strGetPano = "";

            strGetPano = ssList_Sheet1.Cells[e.Row, 1].Text;
            San_IDS_Display(strGetPano);
            San_DTL_Display(strGetPano);
            San_JIN_Display(strGetPano);
        }

        /// <summary>
        /// 산재 ID 사항 Display
        /// </summary>
        /// <param name="strGetPano"></param>
        void San_IDS_Display(string strGetPano)
        {
            string strGbResult = "";
            string strGBIll = "";
            string strSang = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strSang = "";
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "PANO,BI,SNAME,JUMIN1,JUMIN2,COPRNAME,COPRNO,DEPT1,DEPT2,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(DATE1,'YYYY-MM-DD') DATE1,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(DATE2,'YYYY-MM-DD') DATE2,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(DATE3,'YYYY-MM-DD') DATE3,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(DateRequest,'YYYY-MM-DD') DATEREQUEST ,";
                SQL = SQL + ComNum.VBLF + "GbResult, GbIll, IllCode1, IllCode2, IllCode3, IllCode4, IllCode5";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SANID";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strGetPano + "'";
                SQL = SQL + ComNum.VBLF + "     AND ROWNUM ='1' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                if (dt.Rows.Count == 1)
                {
                    strGBIll = dt.Rows[0]["GbIll"].ToString().Trim();
                    strGbResult = dt.Rows[0]["GbResult"].ToString().Trim();
                    strIllCode[0] = dt.Rows[0]["IllCode1"].ToString().Trim();
                    strIllCode[1] = dt.Rows[0]["IllCode2"].ToString().Trim();
                    strIllCode[2] = dt.Rows[0]["IllCode3"].ToString().Trim();
                    strIllCode[3] = dt.Rows[0]["IllCode4"].ToString().Trim();
                    strIllCode[4] = dt.Rows[0]["IllCode5"].ToString().Trim();

                    switch (strGbResult)
                    {
                        case "1":
                            strGbResult += ".치유";
                            break;
                        case "2":
                            strGbResult += ".사망";
                            break;
                        case "3":
                            strGbResult += ".전원";
                            break;
                        case "4":
                            strGbResult += ".중지";
                            break;
                        case "5":
                            strGbResult += ".계속";
                            break;
                        default:
                            break;
                    }

                    if (VB.Mid(strGBIll, 1, 1) == "1") strSang += "두부";
                    if (VB.Mid(strGBIll, 2, 1) == "1") strSang += "상지";
                    if (VB.Mid(strGBIll, 3, 1) == "1") strSang += "체부";
                    if (VB.Mid(strGBIll, 4, 1) == "1") strSang += "하지";
                    if (VB.Mid(strGBIll, 5, 1) == "1") strSang += "수족";

                    switch (VB.Len(strSang))
                    {
                        case 20:
                            strGbIlls[0] = VB.Mid(strSang, 1, 4) + "," + VB.Mid(strSang, 5, 4);
                            strGbIlls[1] = VB.Mid(strSang, 9, 4) + "," + VB.Mid(strSang, 13, 4);
                            strGbIlls[2] = VB.Mid(strSang, 17, 4);
                            break;
                        case 16:
                            strGbIlls[0] = VB.Mid(strSang, 1, 4) + "," + VB.Mid(strSang, 5, 4);
                            strGbIlls[1] = VB.Mid(strSang, 9, 4);
                            strGbIlls[2] = VB.Mid(strSang, 13, 4);
                            break;
                        default:
                            strGbIlls[0] = VB.Mid(strSang, 1, 4);
                            strGbIlls[1] = VB.Mid(strSang, 5, 4);
                            strGbIlls[2] = VB.Mid(strSang, 9, 4);
                            break;
                    }

                    ssList2_Sheet1.Cells[0, 1].Text = dt.Rows[0]["Jumin1"].ToString().Trim() + " - " + dt.Rows[0]["Jumin2"].ToString().Trim();
                    ssList2_Sheet1.Cells[1, 1].Text = dt.Rows[0]["CoprName"].ToString().Trim();
                    ssList2_Sheet1.Cells[2, 2].Text = dt.Rows[0]["CoprNo"].ToString().Trim();

                    ssList2_Sheet1.Cells[4, 0].Text = strIllCode[0];
                    ssList2_Sheet1.Cells[5, 0].Text = strIllCode[1];
                    ssList2_Sheet1.Cells[6, 0].Text = strIllCode[2];
                    ssList2_Sheet1.Cells[7, 0].Text = strIllCode[3];
                    ssList2_Sheet1.Cells[8, 0].Text = strIllCode[4];

                    ssList2_Sheet1.Cells[4, 1].Text = READ_Bas_ILL(strIllCode[0]);
                    ssList2_Sheet1.Cells[5, 1].Text = READ_Bas_ILL(strIllCode[1]);
                    ssList2_Sheet1.Cells[6, 1].Text = READ_Bas_ILL(strIllCode[2]);
                    ssList2_Sheet1.Cells[7, 1].Text = READ_Bas_ILL(strIllCode[3]);
                    ssList2_Sheet1.Cells[8, 1].Text = READ_Bas_ILL(strIllCode[4]);

                    ssList2_Sheet1.Cells[0, 3].Text = dt.Rows[0]["Date1"].ToString().Trim();
                    ssList2_Sheet1.Cells[1, 3].Text = dt.Rows[0]["Date2"].ToString().Trim();
                    ssList2_Sheet1.Cells[2, 3].Text = dt.Rows[0]["Date3"].ToString().Trim();
                    ssList2_Sheet1.Cells[3, 3].Text = dt.Rows[0]["Dept1"].ToString().Trim() + " " + dt.Rows[0]["Dept2"].ToString().Trim();
                    ssList2_Sheet1.Cells[4, 3].Text = strGbResult;
                    ssList2_Sheet1.Cells[5, 3].Text = strGbIlls[0];
                    ssList2_Sheet1.Cells[6, 3].Text = strGbIlls[1];
                    ssList2_Sheet1.Cells[7, 3].Text = strGbIlls[2];
                    ssList2_Sheet1.Cells[8, 3].Text = dt.Rows[0]["DateRequest"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// 산재 승인신청 내역 Display
        /// </summary>
        /// <param name="strGetPano"></param>
        void San_DTL_Display(string strGetPano)
        {
            string strIpdDate = "";
            string strOpdDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DateApproval, 'yyyy-mm-dd') DateApp, ";
                SQL = SQL + ComNum.VBLF + " IpdFrDate, IpdToDate, IpdTerm,  ";
                SQL = SQL + ComNum.VBLF + " OpdFrDate, OpdToDate, OpdTerm    ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SANDTL";
                SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strGetPano + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                ssList3_Sheet1.RowCount = dt.Rows.Count;

                for (int i = 0; i < ssList3_Sheet1.RowCount; i++)
                {
                    strIpdDate = dt.Rows[i]["IpdFrDate"].ToString().Trim() + "-" + dt.Rows[i]["IpdToDate"].ToString().Trim();
                    strOpdDate = dt.Rows[i]["OpdFrDate"].ToString().Trim() + "-" + dt.Rows[i]["OpdToDate"].ToString().Trim();

                    ssList3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DateApp"].ToString().Trim();
                    ssList3_Sheet1.Cells[i, 1].Text = strIpdDate;
                    ssList3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["IpdTerm"].ToString().Trim();
                    ssList3_Sheet1.Cells[i, 3].Text = strOpdDate;
                    ssList3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["OpdTerm"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// 산재 휴업급여 진단서 발급 내역 Display
        /// </summary>
        /// <param name="strGetPano"></param>
        void San_JIN_Display(string strGetPano)
        {
            string strFromTo = "";
            string SQL = "";
            string SqlErr = "";

            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DateBal, 'yyyy-mm-dd') DateBal,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DateReq, 'yyyy-mm-dd') DateReq,";
                SQL = SQL + ComNum.VBLF + " FrDate, ToDate, Term ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SANJIN";
                SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strGetPano + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                ssList4_Sheet1.RowCount = dt.Rows.Count;

                for (int i = 0; i < ssList4_Sheet1.RowCount; i++)
                {
                    strFromTo = dt.Rows[i]["FrDate"].ToString().Trim() + "-" + dt.Rows[i]["ToDate"].ToString().Trim();

                    ssList4_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DateBal"].ToString().Trim();
                    ssList4_Sheet1.Cells[i, 1].Text = strFromTo;
                    ssList4_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Term"].ToString().Trim();
                    ssList4_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DateReq"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        void txtSname_KeyPress(object sender, KeyPressEventArgs e)
        {
            strChangeFLAG = "**";

            txtSname.ImeMode = ImeMode.Hangul;

            if(e.KeyChar == 13)
            {
                btnOK.Focus();
            }
        }

        void txtSname_Leave(object sender, EventArgs e)
        {
            txtSname.ImeMode = ImeMode.Alpha;
        }
    }
}
