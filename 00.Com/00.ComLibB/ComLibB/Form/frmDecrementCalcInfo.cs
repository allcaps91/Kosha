using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmDecrementCalcInfo
    /// File Name : frmDecrementCalcInfo.cs
    /// Title or Description : 감액계산 기준정보 등록 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-06
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmDecrementCalcInfo : Form
    {
        //스프레드 SS1의 칼럼 항목 NULL은 빈 항목
        enum Col_SS1 { Code, Name, Jin, Bohum, Gongsang, Ilban, ChkJongum, Jongum, NULL, Sono, Mri, Food, Room, Er, SuCode, SuCode2, SuCode3, ROWID }

        public frmDecrementCalcInfo()
        {
            InitializeComponent();

        }

        void Screen_Clear()
        {
            grbGbn.Enabled = true; //감액구분
            cboDate.Enabled = true;
            dtpDate.Enabled = true;
            btnView.Enabled = true;

            dtpDate.Text = "";
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            btnNew.Enabled = true;
            ss1_Sheet1.Rows.Count = 50;
            ss1_Sheet1.ClearRange(0, 0, ss1_Sheet1.Rows.Count, ss1_Sheet1.Columns.Count, true);
            ss1.Enabled = false;
            lblMsg.Enabled = false;
            lblMsg.Text = "";
        }

        void ComboDate_Set()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate,COUNT(*) CNT ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_GAMCODE ";
            if (optGbn0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " WHERE GbDT='1' ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " WHERE GbDT='2' ";
            }
            SQL = SQL + ComNum.VBLF + "GROUP BY SDate ";
            SQL = SQL + ComNum.VBLF + "ORDER BY SDate DESC ";

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

            cboDate.Items.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cboDate.Items.Add(dt.Rows[i]["SDate"].ToString().Trim());
            }

            cboDate.Enabled = true;
            cboDate.SelectedIndex = 0;

            dt.Dispose();
            dt = null;
        }

        bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            int nRow = 0;
            string strCode = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (cboDate.Text.Trim() == "") { MessageBox.Show("기준적용일이 공란입니다.", "오류"); return false; }

            dtpDate.Text = cboDate.Text;

            try
            {
                //감액코드를 표시
                SQL = "SELECT SORT,Code,Name FROM BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun='BAS_감액코드명' ";
                SQL = SQL + ComNum.VBLF + "  AND (JDate<=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') OR JDate IS NULL) ";
                SQL = SQL + ComNum.VBLF + "  AND (DelDate>TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') OR DelDate IS NULL) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SORT,Code ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = " " + dt.Rows[i]["Name"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                //감액정보 읽음
                SQL = "SELECT CODE,GBDT,JIN,BOHUM,GONGSANG,ILBAN,DT1,DT2,DT3,SONO,MRI,FOOD,ROOM,";
                SQL = SQL + ComNum.VBLF + " ER,GBHEA,HEARATE,HEAAMT,SuCode,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_GAMCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE SDate=TO_DATE('" + cboDate.Text + "','YYYY-MM-DD') ";
                if (optGbn0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND GbDT='1' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND GbDT='2' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY Code ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strCode = dt.Rows[i]["Code"].ToString().Trim();
                    nRow = 0;
                    for (int j = 0; j < ss1_Sheet1.Rows.Count; j++)
                    {
                        if (ss1_Sheet1.Cells[j, 0].Text == strCode) { nRow = j; break; }
                    }

                    //해당코드가 없으면
                    if (nRow == 0)
                    {
                        ss1_Sheet1.Rows.Count++;
                        nRow = ss1_Sheet1.Rows.Count - 1;
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.Code].Text = strCode;
                        //TODO:READ_BCODE_Name 공통함수 임시구현
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.Name].Text = Read_Bcode_Name("BAS_감액코드명", strCode);
                    }

                    ss1_Sheet1.Cells[nRow, (int)Col_SS1.Jin].Text = dt.Rows[i]["Jin"].ToString();
                    ss1_Sheet1.Cells[nRow, (int)Col_SS1.Bohum].Text = dt.Rows[i]["Bohum"].ToString();
                    ss1_Sheet1.Cells[nRow, (int)Col_SS1.Gongsang].Text = dt.Rows[i]["Gongsang"].ToString();
                    ss1_Sheet1.Cells[nRow, (int)Col_SS1.Ilban].Text = dt.Rows[i]["Ilban"].ToString();
                    if (dt.Rows[i]["GbHEA"].ToString() == "1")
                    {
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.ChkJongum].Text = "True";
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.Jongum].Text = dt.Rows[i]["HeaAmt"].ToString();
                    }
                    else
                    {
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.ChkJongum].Text = "";
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.Jongum].Text = dt.Rows[i]["HeaRate"].ToString();
                    }
                    //비급여 할인
                    ss1_Sheet1.Cells[nRow, (int)Col_SS1.Sono].Text = dt.Rows[i]["Sono"].ToString();
                    ss1_Sheet1.Cells[nRow, (int)Col_SS1.Mri].Text = dt.Rows[i]["Mri"].ToString();
                    ss1_Sheet1.Cells[nRow, (int)Col_SS1.Food].Text = dt.Rows[i]["Food"].ToString();
                    ss1_Sheet1.Cells[nRow, (int)Col_SS1.Room].Text = dt.Rows[i]["Room"].ToString();
                    ss1_Sheet1.Cells[nRow, (int)Col_SS1.Er].Text = dt.Rows[i]["Er"].ToString();
                    if (optGbn0.Checked == true)
                    {
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.SuCode].Text = dt.Rows[i]["SuCode"].ToString();
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.SuCode2].Text = "";
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.SuCode3].Text = "";
                    }
                    else
                    {
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.SuCode].Text = dt.Rows[i]["DT1"].ToString();
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.SuCode2].Text = dt.Rows[i]["DT2"].ToString();
                        ss1_Sheet1.Cells[nRow, (int)Col_SS1.SuCode3].Text = dt.Rows[i]["DT3"].ToString();
                    }
                    ss1_Sheet1.Cells[nRow, (int)Col_SS1.ROWID].Text = dt.Rows[i]["ROWID"].ToString();
                }
                dt.Dispose();
                dt = null;

                grbGbn.Enabled = false;
                cboDate.Enabled = false;
                btnView.Enabled = false;

                dtpDate.Enabled = true;
                ss1.Enabled = true;
                lblMsg.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }
        }

        bool SaveData()
        {
            int nJin = 0;
            int nBohum = 0;
            int nGongsang = 0;
            int nIlban = 0;
            int nDT1 = 0;
            int nDT2 = 0;
            int nDT3 = 0;
            int nSONO = 0;
            int nMRI = 0;
            int nFood = 0;
            int nRoom = 0;
            int nEr = 0;
            int nHeaRate = 0;
            string strROWID = "";
            string strSuCode = "";
            string strCode = "";
            string strGbDt = "";
            string strGbHea = "";
            long nHeaAmt = 0;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (int i = 0; i < ss1_Sheet1.Rows.Count; i++)
                {
                    strROWID = ss1_Sheet1.Cells[i, (int)Col_SS1.ROWID].Text.Trim();
                    strCode = ss1_Sheet1.Cells[i, (int)Col_SS1.Code].Text.Trim();
                    nJin = Convert.ToInt32(VB.Val((ss1_Sheet1.Cells[i, (int)Col_SS1.Jin].Text)));
                    nBohum = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Bohum].Text);
                    nGongsang = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Gongsang].Text);
                    nIlban = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Ilban].Text);
                    strGbHea = "2";
                    if (ss1_Sheet1.Cells[i, (int)Col_SS1.ChkJongum].Text == "True") { strGbHea = "1"; }
                    if (strGbHea == "1")
                    {
                        nHeaRate = 0;
                        nHeaAmt = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Jongum].Text);
                    }
                    else
                    {
                        nHeaAmt = 0;
                        nHeaRate = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Jongum].Text);
                    }
                    nSONO = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Sono].Text);
                    nMRI = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Mri].Text);
                    nFood = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Food].Text);
                    nRoom = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Room].Text);
                    nEr = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Er].Text);
                    if (optGbn0.Checked == true)
                    {
                        strSuCode = ss1_Sheet1.Cells[i, (int)Col_SS1.SuCode].Text;
                        nDT1 = 0;
                        nDT2 = 0;
                        nDT3 = 0;
                    }
                    else
                    {
                        strSuCode = "";
                        nDT1 = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.SuCode].Text);
                        nDT2 = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.SuCode2].Text);
                        nDT3 = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.SuCode3].Text);
                    }

                    //오류점검
                    if (nJin > 100) { MessageBox.Show(i + "번줄 진찰료 감액율이 100% 초과!!", "오류"); return false; }
                    if (nBohum > 100) { MessageBox.Show(i + "번줄 보험(보호) 감액율이 100% 초과!!!", "오류"); return false; }
                    if (nGongsang > 100) { MessageBox.Show(i + "번줄 산재공상 감액율이 100% 초과!!", "오류"); return false; }
                    if (nIlban > 100) { MessageBox.Show(i + "번줄 일반 감액율이 100% 초과!!", "오류"); return false; }
                    if (nHeaRate > 100) { MessageBox.Show(i + "번줄 종검 감액율이 100% 초과!!", "오류"); return false; }
                    if (nSONO > 100) { MessageBox.Show(i + "번줄 초음파 감액율이 100% 초과!!", "오류"); return false; }
                    if (nMRI > 100) { MessageBox.Show(i + "번줄 MRI 감액율이 100% 초과!!", "오류"); return false; }
                    if (nFood > 100) { MessageBox.Show(i + "번줄 식대 감액율이 100% 초과!!", "오류"); return false; }
                    if (nRoom > 100) { MessageBox.Show(i + "번줄 병실차액 감액율이 100% 초과!!", "오류"); return false; }
                    if (nEr > 100) { MessageBox.Show(i + "번줄 응급관리료 감액율이 100% 초과!!", "오류"); return false; }

                    if (nDT1 > 100) { MessageBox.Show(i + "번줄 치과 비급여 감액율이 100% 초과!!", "오류"); return false; }
                    if (nDT2 > 100) { MessageBox.Show(i + "번줄 치과 보철료 감액율이 100% 초과!!", "오류"); return false; }
                    if (nDT3 > 100) { MessageBox.Show(i + "번줄 치과 임플란트 감액율이 100% 초과!!", "오류"); return false; }
                }

                //자료를 DB에 저장함
                strGbDt = "1";
                if (optGbn1.Checked == true) { strGbDt = "2"; }

                for (int i = 0; i < ss1_Sheet1.Rows.Count; i++)
                {
                    strROWID = ss1_Sheet1.Cells[i, (int)Col_SS1.ROWID].Text.Trim();
                    strCode = ss1_Sheet1.Cells[i, (int)Col_SS1.Code].Text.Trim();
                    nJin = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Jin].Text);
                    nBohum = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Bohum].Text);
                    nGongsang = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Gongsang].Text);
                    nIlban = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Ilban].Text);
                    strGbHea = "2";
                    if (ss1_Sheet1.Cells[i, (int)Col_SS1.ChkJongum].Text == "True") { strGbHea = "1"; }
                    if (strGbHea == "1")
                    {
                        nHeaRate = 0;
                        nHeaAmt = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Jongum].Text);
                    }
                    else
                    {
                        nHeaAmt = 0;
                        nHeaRate = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Jongum].Text);
                    }
                    nSONO = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Sono].Text);
                    nMRI = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Mri].Text);
                    nFood = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Food].Text);
                    nRoom = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Room].Text);
                    nEr = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.Er].Text);
                    if (optGbn0.Checked == true)
                    {
                        strSuCode = ss1_Sheet1.Cells[i, (int)Col_SS1.SuCode].Text;
                        nDT1 = 0;
                        nDT2 = 0;
                        nDT3 = 0;
                    }
                    else
                    {
                        strSuCode = "";
                        nDT1 = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.SuCode].Text);
                        nDT2 = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.SuCode2].Text);
                        nDT3 = (int)VB.Val(ss1_Sheet1.Cells[i, (int)Col_SS1.SuCode3].Text);
                    }

                    if (strROWID == "")
                    {
                        if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //저장 권한 확인
                        SQL = "INSERT INTO BAS_GAMCODE (";
                        SQL = SQL + ComNum.VBLF + " CODE,SDATE,GBDT,JIN,BOHUM,GONGSANG,ILBAN,DT1,DT2,DT3,SONO,MRI,FOOD,ROOM,";
                        SQL = SQL + ComNum.VBLF + " ER,GBHEA,HEARATE,HEAAMT,SuCode,ENTTIME,ENTSABUN) VALUES ('";
                        SQL = SQL + ComNum.VBLF + strCode + "',TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'),'";
                        SQL = SQL + ComNum.VBLF + strGbDt + "'," + nJin + "," + nBohum + "," + nGongsang + ",";
                        SQL = SQL + ComNum.VBLF + nIlban + "," + nDT1 + "," + nDT2 + "," + nDT3 + ",";
                        SQL = SQL + ComNum.VBLF + nSONO + "," + nMRI + "," + nFood + "," + nRoom + ",";
                        SQL = SQL + ComNum.VBLF + nEr + ",'" + strGbHea + "'," + nHeaRate + "," + nHeaAmt + ",'";
                        SQL = SQL + ComNum.VBLF + strSuCode + "',SYSDATE," + clsPublic.GnJobSabun + ") ";
                    }
                    else
                    {
                        if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //수정 권한 확인
                        SQL = "UPDATE BAS_GAMCODE SET ";
                        SQL = SQL + ComNum.VBLF + " SDate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + " Jin=" + nJin + ",";
                        SQL = SQL + ComNum.VBLF + " Bohum=" + nBohum + ",";
                        SQL = SQL + ComNum.VBLF + " Gongsang=" + nGongsang + ",";
                        SQL = SQL + ComNum.VBLF + " Ilban=" + nIlban + ",";
                        SQL = SQL + ComNum.VBLF + " DT1=" + nDT1 + ",";
                        SQL = SQL + ComNum.VBLF + " DT2=" + nDT2 + ",";
                        SQL = SQL + ComNum.VBLF + " DT3=" + nDT3 + ",";
                        SQL = SQL + ComNum.VBLF + " SONO=" + nSONO + ",";
                        SQL = SQL + ComNum.VBLF + " MRI=" + nMRI + ",";
                        SQL = SQL + ComNum.VBLF + " Food=" + nFood + ",";
                        SQL = SQL + ComNum.VBLF + " Room=" + nRoom + ",";
                        SQL = SQL + ComNum.VBLF + " Er=" + nEr + ",";
                        SQL = SQL + ComNum.VBLF + " GbHea='" + strGbHea + "',";
                        SQL = SQL + ComNum.VBLF + " HeaRate=" + nHeaRate + ",";
                        SQL = SQL + ComNum.VBLF + " HeaAmt=" + nHeaAmt + ",";
                        SQL = SQL + ComNum.VBLF + " SuCode='" + strSuCode + "',";
                        SQL = SQL + ComNum.VBLF + " EntTime=SYSDATE,";
                        SQL = SQL + ComNum.VBLF + " EntSabun=" + clsPublic.GnJobSabun + " ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID='" + strROWID + "' ";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComboDate_Set();
                Screen_Clear();
                Cursor.Current = Cursors.Default;
                return true;
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        //TODO:READ_BCODE_Name 공통함수 임시구현
        string Read_Bcode_Name(string gubun, string code)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strVat = "";

            try
            {
                SQL = "";
                SQL = "SELECT Name FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun='" + gubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Code='" + code.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVat;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return strVat;
                }

                strVat = dt.Rows[0]["Name"].ToString().Trim();
                dt.Dispose();
                dt = null;

                return strVat;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return strVat;
            }
        }

        private void frmDecrementCalcInfo_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            ComFunc.ReadSysDate(clsDB.DbCon);
            Screen_Clear();
            ComboDate_Set();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            grbGbn.Enabled = false;
            cboDate.Enabled = false;
            btnView.Enabled = false;

            dtpDate.Enabled = true;
            dtpDate.Text = clsPublic.GstrSysDate;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnPrint.Enabled = false;
            btnNew.Enabled = false;
            ss1_Sheet1.Rows.Count = 50;
            ss1_Sheet1.ClearRange(0, 0, ss1_Sheet1.Rows.Count, ss1_Sheet1.Columns.Count, true);
            ss1.Enabled = true;
            lblMsg.Enabled = true;

            //감액코드 표시
            SQL = "SELECT SORT,Code,Name FROM BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + "WHERE Gubun='BAS_감액코드명' ";
            SQL = SQL + ComNum.VBLF + "  AND (JDate<=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') OR JDate IS NULL) ";
            SQL = SQL + ComNum.VBLF + "  AND (DelDate>TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') OR DelDate IS NULL) ";
            SQL = SQL + ComNum.VBLF + "ORDER BY SORT,Code ";

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

            ss1_Sheet1.Rows.Count = dt.Rows.Count;
            ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Code"].ToString().Trim();
                ss1_Sheet1.Cells[i, 1].Text = " " + dt.Rows[i]["Name"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (optGbn0.Checked == true)
            {
                strTitle = "감액계산 기준정보 (일반)";
            }
            else if (optGbn1.Checked == true)
            {
                strTitle = "감액계산 기준정보 (치과)";
            }
            

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "  Page: /p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, true, true, false);

            SPR.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private void optGbn0_CheckedChanged(object sender, EventArgs e)
        {
            ComboDate_Set();
            Screen_Clear();
            //감액 구분에 따른 스프레드 칼럼헤더 표시
            if (optGbn0.Checked == true)
            {
                ss1_Sheet1.ColumnHeader.Columns[(int)Col_SS1.SuCode].Label = "수가 코드";
                ss1_Sheet1.ColumnHeader.Columns[(int)Col_SS1.SuCode2].Visible = false;
                ss1_Sheet1.ColumnHeader.Columns[(int)Col_SS1.SuCode3].Visible = false;
                //ss1_Sheet1.ColumnHeader.Columns[(int)Col_SS1.SuCode2].Label = "";
                //ss1_Sheet1.ColumnHeader.Columns[(int)Col_SS1.SuCode3].Label = "";
            }
            else
            {
                ss1_Sheet1.ColumnHeader.Columns[(int)Col_SS1.SuCode2].Visible = true;
                ss1_Sheet1.ColumnHeader.Columns[(int)Col_SS1.SuCode3].Visible = true;
                ss1_Sheet1.ColumnHeader.Columns[(int)Col_SS1.SuCode].Label = "치과 비급여";
                ss1_Sheet1.ColumnHeader.Columns[(int)Col_SS1.SuCode2].Label = "치과 보철료";
                ss1_Sheet1.ColumnHeader.Columns[(int)Col_SS1.SuCode3].Label = "임플란트";
            }
        }

        private void ss1_LeaveCell(object sender, LeaveCellEventArgs e)
        {
            if (optGbn1.Checked == true) { return; }
            if (e.Column != (int)Col_SS1.SuCode) { return; }

            string strCode = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            //감액코드명을 읽음
            strCode = ss1_Sheet1.Cells[e.Row, e.Column].Text.Trim();
            if (strCode == "") { return; }

            SQL = "SELECT SuNameK FROM BAS_SUN ";
            SQL = SQL + ComNum.VBLF + "WHERE SuNext='" + strCode + "' ";
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

            lblMsg.Text = dt.Rows[0]["SuNameK"].ToString();

            dt.Dispose();
            dt = null;
            return;
        }
    }
}
