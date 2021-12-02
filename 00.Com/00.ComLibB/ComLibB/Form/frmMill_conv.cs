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
    /// File Name       : frmMill_conv.cs
    /// Description     : 처방상병 >> 청구상병 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-14
    /// Update History  : 
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\frmMill_conv.frm(FrmMill_conv) => frmMill_conv.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\frmMill_conv.frm(FrmMill_conv)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>   
    public partial class frmMill_conv : Form
    {
        public frmMill_conv()
        {
            InitializeComponent();

        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmMill_conv_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            // 로우 높이 조절
            for (int i = 0; i < ssMill_Sheet1.RowCount; i++)
            {
                ssMill_Sheet1.Rows.Get(i).Height = 22;
            }

            // 사용하지 않는 칼럼 안보이게
            for (int j = 0; j < 19; j++)
            {
                ssMill_Sheet1.Columns[j].Visible = false;
                if(j == 0 || j == 7 || j == 10)
                {
                    ssMill_Sheet1.Columns[j].Visible = true;
                }
            }

            Screen_display();
        }

        void Screen_display()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;

            string strDeptCode = "";
            string strIllCode1 = "";
            string strIllCode2 = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssMill_Sheet1.RowCount = 0;

            Spread_clear(ssMill_Sheet1);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    a.IllCode1, a.IllCode2,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(a.EntDate,'YYYY-MM-DD') EntDate, a.ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MILL_CONV a";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.IllCode1";
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

                ssMill_Sheet1.RowCount = dt.Rows.Count + 20;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strIllCode1 = dt.Rows[i]["ILLCODE1"].ToString().Trim() + "";
                    strIllCode2 = dt.Rows[i]["ILLCODE2"].ToString().Trim() + "";

                    ssMill_Sheet1.Cells[i, 0].Text = "";

                    if (strDeptCode != "" && strDeptCode != "**")
                    {
                        ssMill_Sheet1.Cells[i, 4].Text = READ_DeptName(strDeptCode);
                    }

                    ssMill_Sheet1.Cells[i, 7].Text = strIllCode1;
                    if (strIllCode1 != "")
                    {
                        ssMill_Sheet1.Cells[i, 8].Text = READ_IllName(strIllCode1);
                    }

                    ssMill_Sheet1.Cells[i, 10].Text = strIllCode2;
                    if (strIllCode2 != "")
                    {
                        ssMill_Sheet1.Cells[i, 11].Text = READ_IllName(strIllCode2);
                    }

                    ssMill_Sheet1.Cells[i, 15].Text = dt.Rows[i]["EntDate"].ToString().Trim() + "";
                    ssMill_Sheet1.Cells[i, 16].Text = dt.Rows[i]["ROWID"].ToString().Trim() + "";
                    ssMill_Sheet1.Cells[i, 17].Text = "";
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            

        }

        /// <summary>
        /// 스프레드 셀값 초기화
        /// </summary>
        /// <param name="Spd"></param>
        void Spread_clear(FarPoint.Win.Spread.SheetView Spd)
        {
            for(int i = 0; i < Spd.RowCount; i++)
            {
                for(int j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }

        /// <summary>
        /// 부서명을 읽어온다
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        string READ_DeptName(string ArgCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    DeptNameK";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + "WHERE DeptCode = '" + ArgCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["DeptNameK"].ToString().Trim();
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
                
        }

        /// <summary>
        /// 상병코드를 읽어온다.
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        string READ_IllName(string ArgCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    IllNameK";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                SQL = SQL + ComNum.VBLF + "WHERE IllCode = '" + ArgCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }                

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["IllNameK"].ToString().Trim();
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            SaveData();
        }

        void SaveData()
        {
            int i = 0;

            string strChk = "";
            string strChange = "";
            string strROWID = "";

            string strSuCode = "";
            string strDeptCode = "";
            string strIllCode1 = "";
            string strIllCode2 = "";
            string strIllCode3 = "";
            string strSex = "";
            string strRO1 = "";
            string strRO2 = "";
            string strRO3 = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssMill_Sheet1.RowCount; i++)
                {
                    strChk = ssMill_Sheet1.Cells[i, 0].Text;
                    strSuCode = ssMill_Sheet1.Cells[i, 1].Text;
                    strDeptCode = ssMill_Sheet1.Cells[i, 3].Text;
                    strSex = ssMill_Sheet1.Cells[i, 5].Text;
                    strRO1 = ssMill_Sheet1.Cells[i, 6].Text;
                    strIllCode1 = ssMill_Sheet1.Cells[i, 7].Text;

                    strRO2 = ssMill_Sheet1.Cells[i, 9].Text;
                    strIllCode2 = ssMill_Sheet1.Cells[i, 10].Text;

                    strRO3 = ssMill_Sheet1.Cells[i, 12].Text;
                    strIllCode3 = ssMill_Sheet1.Cells[i, 13].Text;

                    strROWID = ssMill_Sheet1.Cells[i, 16].Text;
                    strChange = ssMill_Sheet1.Cells[i, 17].Text;

                    strSex = "*";


                    if (strChk == "True")
                    {
                        if (strROWID != "")
                        {

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "DELETE BAS_MILL_conv ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        }
                    }

                    else if (strChange == "Y")
                    {
                        if (strROWID == "" && strIllCode1 != "")
                        {

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO";
                            SQL = SQL + ComNum.VBLF + "    BAS_MILL_conv ( SuCode,DeptCode,IllCode1,IllCode2,EntDate,SEX, RO1,GBIO)";
                            SQL = SQL + ComNum.VBLF + "VALUES ('" + strSuCode + "','" + strDeptCode + "', ";
                            SQL = SQL + ComNum.VBLF + " '" + strIllCode1 + "','" + strIllCode2 + "',SYSDATE,'" + strSex + "', '" + strRO1 + "', '*') ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        }

                        else if (strROWID != "")
                        { 

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE BAS_MILL_conv SET";
                            SQL = SQL + ComNum.VBLF + "     SuCode = '" + strSuCode + "' ,";
                            SQL = SQL + ComNum.VBLF + "     DeptCode = '" + strDeptCode + "', ";
                            SQL = SQL + ComNum.VBLF + "     IllCode1 = '" + strIllCode1 + "', ";
                            SQL = SQL + ComNum.VBLF + "     RO1 = '" + strRO1 + "', ";
                            SQL = SQL + ComNum.VBLF + "     IllCode2 = '" + strIllCode2 + "', ";
                            SQL = SQL + ComNum.VBLF + "     SEX = '" + strSex + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;


                Screen_display();
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void ssMill_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if (e.Column > 0)
            {
                ssMill_Sheet1.Cells[e.Row, 17].Text = "Y";
            }

            string strData = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";


            strData = ssMill_Sheet1.Cells[e.Row, e.Column].Text.ToUpper();

            if (e.Column == 1)
            {
                try
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "    SuNameK";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN";
                    SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + strData + "' ";
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

                    if (dt.Rows.Count > 0)
                    {
                        ssMill_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["SuNameK"].ToString().Trim();
                    }
                    else
                    {
                        ssMill_Sheet1.Cells[e.Row, 1].Text = "";
                        ssMill_Sheet1.Cells[e.Row, 2].Text = "";
                        ComFunc.MsgBox("수가코드 등록 않됨");
                    }

                    if (e.Column == 3)
                    {
                        ssMill_Sheet1.Cells[e.Row, 4].Text = READ_DeptName(strData);
                    }

                    if (e.Column == 5)
                    {
                        if (strData != "M" || strData != "F" || strData != "*")
                        {
                            ComFunc.MsgBox("해당하는란에는 M F * 값중에서 하나 입력해주세요.");
                            ssMill_Sheet1.Cells[e.Row, e.Column].Text = "*";
                        }
                    }

                    if (e.Column == 7 || e.Column == 10 | e.Column == 3)
                    {
                        ssMill_Sheet1.Cells[e.Row, e.Column + 1].Text = READ_IllName(strData);
                    }

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
        }

        void ssMill_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            // 셀 더블클릭시 로우 수 증가
            ssMill_Sheet1.RowCount += 1;

            if(e.Column == 6 || e.Column == 9 || e.Column == 12)
            {
                if(ssMill_Sheet1.Cells[e.Row, e.Column].Text == "")
                {
                    ssMill_Sheet1.Cells[e.Row, e.Column].Text = "*";
                    ssMill.BackColor = Color.AliceBlue;
                    ssMill_Sheet1.Cells[e.Row, 17].Text = "Y";
                }
                else
                {
                    ssMill_Sheet1.Cells[e.Row, e.Column].Text = "";
                    ssMill.BackColor = Color.White;
                    ssMill_Sheet1.Cells[e.Row, 17].Text = "Y";
                }
            }
        }
    }
}
