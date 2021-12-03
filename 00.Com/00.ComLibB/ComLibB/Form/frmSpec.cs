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
    /// File Name       : frmSpec.cs
    /// Description     : 특정기호 등록하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : try-catch문 수정  
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\BuSuga60.frm(FrmSpec) => frmSpec.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\BuSuga60.frm(FrmSpec)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmSpec : Form
    {
        public frmSpec()
        {
            InitializeComponent();
        }

        void frmSpec_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssSpec_Sheet1.Columns[2].Visible = false;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ssSpec_Sheet1.RowCount = 0;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    SPECCODE, SPECNAME, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SPECCODE";
                SQL = SQL + ComNum.VBLF + " ORDER BY SPECCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssSpec_Sheet1.RowCount = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {

                    ssSpec_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SPECCODE"].ToString().Trim();
                    ssSpec_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SPECNAME"].ToString().Trim();
                    ssSpec_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            SaveData();
        }

        void SaveData()
        {
            string strSpecCode = "";
            string strSpecName = "";
            string strROWID = "";
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssSpec_Sheet1.RowCount; i++)
                {
                    strSpecCode = ssSpec_Sheet1.Cells[i, 0].Text;
                    strSpecName = ssSpec_Sheet1.Cells[i, 1].Text;
                    strROWID = ssSpec_Sheet1.Cells[i, 2].Text;

                    if (strROWID == "")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "INSERT INTO";
                        SQL = SQL + ComNum.VBLF + "    BAS_SPECCODE ( SPECCODE, SPECNAME)";
                        SQL = SQL + ComNum.VBLF + "VALUES('" + strSpecCode + "','" + strSpecName + "')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE BAS_SPECCODE SET";
                        SQL = SQL + ComNum.VBLF + "     SPECCODE = '" + strSpecCode + "' ,";
                        SQL = SQL + ComNum.VBLF + "     SPECNAME = '" + strSpecName + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            string strSysDate = "";
            string strSysTime = "";
            strSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            strSysTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";

            strHead1 = "/f1" + VB.Space(20);
            strHead1 = strHead1 + "특정 기호 LIST";
            strHead2 = "/f2" + "인쇄일자 : " + strSysDate + " " + strSysTime;
            

            ssSpec_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;

            ssSpec_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssSpec_Sheet1.PrintInfo.Margin.Top = 50;
            ssSpec_Sheet1.PrintInfo.Margin.Bottom = 1000;
            ssSpec_Sheet1.PrintInfo.Margin.Left = 0;
            ssSpec_Sheet1.PrintInfo.Margin.Right = 0;

            ssSpec_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssSpec_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;

            ssSpec_Sheet1.PrintInfo.ShowBorder = true;
            ssSpec_Sheet1.PrintInfo.ShowColor = true;
            ssSpec_Sheet1.PrintInfo.ShowGrid = false;
            ssSpec_Sheet1.PrintInfo.ShowShadows = false;
            ssSpec_Sheet1.PrintInfo.UseMax = false;
            ssSpec_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSpec_Sheet1.PrintInfo.Preview = true;
            
            ssSpec.PrintSheet(0);
        }

        void ssSpec_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("기존 DATA를 삭제하시겠습니까?", "삭제여부", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            string strROWID = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            strROWID = ssSpec_Sheet1.Cells[e.Row, 2].Text;

            try
            {
                if (strROWID.Trim() != "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "DELETE BAS_SPECCODE ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("삭제하였습니다.");
                    ssSpec_Sheet1.RowCount = ssSpec_Sheet1.RowCount - 1;
                    Cursor.Current = Cursors.Default;
                }
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }
    
}
