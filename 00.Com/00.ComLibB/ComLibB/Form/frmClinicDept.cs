using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmClinicDept.cs
    /// Description     : 진료과 코드 등록하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-05
    /// Update History  : try-catch문 수정
    /// <history>       
    /// D:\타병원\PSMHH\basic\bucode\BuCode15.frm(FrmClinicDept) => frmClinicDept.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\bucode\BuCode15.frm(FrmClinicDept)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\bucode\bucode.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmClinicDept : Form
    {
        ComFunc CF = new ComFunc();
        string fstrROWID = "";
        public frmClinicDept()
        {
            InitializeComponent();
        }

        void frmClinicDept_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            optSeo.Checked = true;

            SCREEN_CLEAR();
            GetData();
        }

        void SCREEN_CLEAR()
        {
            txtCode.Text = "";
            txtRank.Text = "";
            txtNameK.Text = "";
            txtNameE.Text = "";
            fstrROWID = "";

            //btnSave.Enabled = false;
            //btnDelete.Enabled = false;
            //btnCancel.Enabled = false;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtCode.Focus();
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            DelData();
        }

        void DelData()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }      

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + "WHERE DeptCode='" + txtCode.Text + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;

                GetData();
                SCREEN_CLEAR();
                txtCode.Focus();
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
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
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            if (txtCode.Text.Trim() == "")
            {
                ComFunc.MsgBox("진료과코드가 공란입니다.");
            }

            if(Convert.ToInt16(txtRank.Text) == 0)
            {
                ComFunc.MsgBox("출력시 서열이 공란입니다.");
            }

            if(txtNameK.Text.Trim() == "")
            {
                ComFunc.MsgBox("한글명칭이 공란입니다.");
            }

            try
            {
                if (fstrROWID == "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO";
                    SQL = SQL + ComNum.VBLF + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                    SQL = SQL + ComNum.VBLF + "(DeptCode,PrintRanking,DeptNameK,DeptNameE)";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF + "'" + txtCode.Text + "'," + Convert.ToInt32(VB.Val(txtRank.Text.Trim())) + ",'";                    
                    SQL = SQL + ComNum.VBLF + txtNameK.Text.Trim() + "', '" + txtNameE.Text.Trim() + "') ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE ";
                    SQL = SQL + ComNum.VBLF + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                    SQL = SQL + ComNum.VBLF + "SET";
                    SQL = SQL + ComNum.VBLF + " PrintRanking =" + Convert.ToInt32(VB.Val(txtRank.Text.Trim())) + ", ";
                    SQL = SQL + ComNum.VBLF + " DeptNameK = '" + txtNameK.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + " DeptNameE = '" + txtNameE.Text.Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + fstrROWID + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");

                CF.EMR_CLINIC_RTN(clsDB.DbCon, txtCode.Text, txtNameK.Text);

                Cursor.Current = Cursors.Default;

                GetData();
                SCREEN_CLEAR();
                txtCode.Focus();
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

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
            string SqlErr = "";

            ssDept_Sheet1.RowCount = 0;
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "DeptCode,PrintRanking,DeptnameK,DeptNameE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            if (optSeo.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking,DeptCode ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY DeptCode ";
            }
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            ssDept_Sheet1.RowCount = dt.Rows.Count;

            for(i = 0; i < dt.Rows.Count; i++)
            {
                ssDept_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PrintRanking"].ToString().Trim();
                ssDept_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                ssDept_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptNameK"].ToString().Trim();
                ssDept_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptNameE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            btnPrint.Enabled = true;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"15\"";
            strHead1 = "/n/" + "/l/f1" + VB.Space(24) + "진료과 코드집" + "/n";

            strFont2 = "/fn\"굴림체\" /fz\"10\"";
            strHead2 = "/l/f2" + "인쇄일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + VB.Space(58) + "PAGE: " + "/p";
            strHead2 = strHead2 + VB.Space(90) + "PAGE : /p";

            ssDept_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;

            ssDept_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssDept_Sheet1.PrintInfo.Margin.Top = 300;
            ssDept_Sheet1.PrintInfo.Margin.Bottom = 1800;
            ssDept_Sheet1.PrintInfo.Margin.Left = 500;
            ssDept_Sheet1.PrintInfo.Margin.Right = 0;

            ssDept_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssDept_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;

            ssDept_Sheet1.PrintInfo.ShowBorder = true;
            ssDept_Sheet1.PrintInfo.ShowColor = false;
            ssDept_Sheet1.PrintInfo.ShowGrid = true;
            ssDept_Sheet1.PrintInfo.ShowShadows = true;
            ssDept_Sheet1.PrintInfo.UseMax = true;
            ssDept_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDept_Sheet1.PrintInfo.Preview = true;
            ssDept.PrintSheet(0);
        }

        void Screen_Display()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            txtCode.Text = txtCode.Text.Trim().ToUpper();
            if(txtCode.Text == "")
            {
                SCREEN_CLEAR();
            }

            if(VB.Len(txtCode.Text) != 2)
            {
                ComFunc.MsgBox("진료과코드 2자리를 입력하세요.");
            }

            //자료를 READ
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + " ROWID,DeptCode,PrintRanking,DeptnameK,DeptNameE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode = '" + txtCode.Text + "' ";
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

                    txtRank.Text = "";
                    txtNameK.Text = "";
                    txtNameE.Text = "";
                    fstrROWID = "";

                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnDelete.Enabled = false;
                    txtRank.Focus();
                }

                fstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                txtRank.Text = dt.Rows[0]["PrintRanking"].ToString().Trim();
                txtNameK.Text = dt.Rows[0]["DeptNameK"].ToString().Trim();
                txtNameE.Text = dt.Rows[0]["DeptNameE"].ToString().Trim();
                dt.Dispose();
                dt = null;

                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnDelete.Enabled = true;

                txtRank.Focus();

            }
            
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                Screen_Display();
            }
        }

        void ssDept_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtCode.Text = ssDept_Sheet1.Cells[e.Row, 1].Text;
            Screen_Display();
        }

        void txtNameK_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtNameE_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnSave.Focus();
            }
        }

        void txtRank_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
