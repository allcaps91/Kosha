using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB.Com
    /// File Name       : frmMedDurSet01.cs
    /// Description     : 처방DUR 사유 상용구 입력 및 관리
    /// Author          : 안정수
    /// Create Date     : 2020-12-16
    /// Update History  : 
    /// </summary>        

    public partial class frmMedDurSet01 : Form
    {
        clsSpread methodSpd = new clsSpread();
        clsPublic cpublic = new clsPublic(); //공용함수
        long gSabun = 0;

        public enum enmResultSetUse
        {
            Key, Chk, Code, Remark, ROWID
        }


        public string[] sSpdResultSetUse = { "KEY", "선택", "코드", "상용 단어", "ROWID" };


        public int[] nSpdResultSetUse = { 60, 30, 60, 600, 80 };

        public frmMedDurSet01()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        void frmMedDurSet01_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                sSpd_ResultSetUse(ssList, sSpdResultSetUse, nSpdResultSetUse, 10, 0, "XRAY");

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();

                screen_clear();

                gSabun = Convert.ToInt32(clsType.User.IdNumber);

                GetDataXray(clsDB.DbCon, ssList);

                ssList.Focus();

                ssList.Select();
            }
        }

        void GetDataXray(PsmhDb pDbCon, FpSpread Spd)
        {
            int i = 0;
            int nRow = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            string SQL = "";
            string SqlErr = "";

            //쿼리실행                  
            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  Code,WardName,ROWID                                   \r\n";           
            SQL += " FROM " + ComNum.DB_PMPA + "ETC_DURSAYU_WARD            \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND Sabun =" + gSabun + "                            \r\n";            
            SQL += "   ORDER BY CODE                                        \r\n";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            #region //데이터셋 읽어 자료 표시

            Spd.ActiveSheet.RowCount = 10;

            if (dt == null) return;

            if (dt.Rows.Count == 0)
            {
                for (i = 0; i < 10; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)enmResultSetUse.Key].Text = "F" + (i + 1).ToString();
                    Spd.ActiveSheet.Cells[i, (int)enmResultSetUse.Code].Text = "F" + (i + 1).ToString();
                }


                return;
            }

            if (dt.Rows.Count > 0)
            {

                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < 10; i++)
                {

                    if (i < dt.Rows.Count)
                    {

                        nRow = Code2Row(dt.Rows[i]["Code"].ToString().Trim());


                        Spd.ActiveSheet.Cells[nRow, (int)enmResultSetUse.Key].Text = dt.Rows[i]["Code"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)enmResultSetUse.Code].Text = dt.Rows[i]["Code"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)enmResultSetUse.Remark].Text = dt.Rows[i]["WardName"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)enmResultSetUse.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)enmResultSetUse.Key].Text = "F" + (i + 1).ToString();
                        Spd.ActiveSheet.Cells[i, (int)enmResultSetUse.Code].Text = "F" + (i + 1).ToString();
                    }

                    Spd.ActiveSheet.Rows.Get(i).Height = Spd.ActiveSheet.Rows[i].GetPreferredHeight();
                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion
        }

        void eSave(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strCODE = "";
            string strWARD = "";
            string strROWID = "";
            string strChk = "";

            long nSabun = Convert.ToInt32(clsType.User.Sabun);

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                for (int i = 0; i <= ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    strChk = ssList.ActiveSheet.Cells[i, (int)enmResultSetUse.Chk].Text.Trim();
                    strCODE = ssList.ActiveSheet.Cells[i, (int)enmResultSetUse.Code].Text.Trim();
                    strWARD = ComFunc.QuotConv(ssList.ActiveSheet.Cells[i, (int)enmResultSetUse.Remark].Text.Trim());
                    strROWID = ssList.ActiveSheet.Cells[i, (int)enmResultSetUse.ROWID].Text.Trim();

                   
                    if (strChk != "True" && strROWID == "" && strWARD != "" && strCODE != "")
                    {
                        //신규
                        SQL = " INSERT INTO  " + ComNum.DB_PMPA + "ETC_DURSAYU_WARD \r\n";
                        SQL += " ( Sabun,Code,WardName ) VALUES                     \r\n";
                        SQL += " (                                                  \r\n";
                        SQL += " " + gSabun + ",'" + strCODE + "',                  \r\n";
                        SQL += " '" + strWARD + "'                                  \r\n";
                        SQL += " )                                                  \r\n";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                    }
                    else if (strChk != "True" && strROWID != "")
                    {
                        //갱신               
                        SQL = "";
                        SQL += " UPDATE " + ComNum.DB_PMPA + "ETC_DURSAYU_WARD SET          \r\n";
                        SQL += "    WardName='" + strWARD + "'                              \r\n";
                        SQL += "  WHERE 1=1                                                 \r\n";
                        SQL += "    AND ROWID = '" + strROWID + "'                          \r\n";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                }
                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }


            //           
            GetDataXray(pDbCon, ssList);
        }

        public void sSpd_ResultSetUse(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt, string Job)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmResultSetUse)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 15;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Chk, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Code, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Remark, clsSpread.enmSpdType.Text);
            //sup.setColStyle_Text(spd, -1, (int)enmResultSetUse.Remark, true, true, false, 2000); //txt재정의

            FarPoint.Win.Spread.CellType.TextCellType spdObj = new FarPoint.Win.Spread.CellType.TextCellType();
            spdObj.Multiline = true;
            spdObj.WordWrap = true;
            spdObj.ReadOnly = false;
            spdObj.MaxLength = 2000;
            ssList.ActiveSheet.Columns[(int)enmResultSetUse.Remark].CellType = spdObj;


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmResultSetUse.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.ROWID, clsSpread.enmSpdType.Hide);
            if (Job == "TO")
            {
                methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Key, clsSpread.enmSpdType.Hide);
            }
            else if (Job == "XRAY")
            {
                methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Chk, clsSpread.enmSpdType.Hide);
                methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Code, clsSpread.enmSpdType.Hide);
            }

            // 5. sort, filter
            //methodSpd.setSpdFilter(spd, (int)enmXrayReadList.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdSort(spd, (int)enmXrayReadList.DrName, true);
            //methodSpd.setSpdFilter(spd, (int)enmXrayReadList.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary = null;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.LightGreen;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmResultSetUse.Key, unary);

        }

        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

        }

        int Code2Row(string argCode)
        {
            int i = -1;

            if (argCode == "F1")
            {
                i = 0;
            }
            else if (argCode == "F2")
            {
                i = 1;
            }
            else if (argCode == "F3")
            {
                i = 2;
            }
            else if (argCode == "F4")
            {
                i = 3;
            }
            else if (argCode == "F5")
            {
                i = 4;
            }
            else if (argCode == "F6")
            {
                i = 5;
            }
            else if (argCode == "F7")
            {
                i = 6;
            }
            else if (argCode == "F8")
            {
                i = 7;
            }
            else if (argCode == "F9")
            {
                i = 8;
            }
            else if (argCode == "F10")
            {
                i = 9;
            }
            else
            {
                i = -1;
            }

            return i;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            eSave(clsDB.DbCon);
        }
    }
}
