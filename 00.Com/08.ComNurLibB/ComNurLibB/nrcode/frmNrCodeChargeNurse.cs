using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;
using ComLibB;
using ComNurLibB;


namespace ComNurLibB
{   
    public partial class frmNrCodeChargeNurse : Form
    {
                
        public frmNrCodeChargeNurse()
        {
            InitializeComponent();
        }

        private void frmNrCodeChargeNurse_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SetCbo();
            txtRowid.Visible = false;
            Set_Spread_Header_Visible(ssCN_Sheet1);

            SetClear();
            GetData();            
        }

        /// <summary>
        /// 콤보박스의 리스트를 읽어 온다.
        /// </summary>
        private void SetCbo()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                cboDept.Items.Add(" ");

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    DEPTCODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_ClinicDept";
                SQL = SQL + ComNum.VBLF + "WHERE DeptCode NOT IN ('II','HR','TO','R6','HD','PT','AN') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                cboDept.Items.Clear();

                if (dt.Rows.Count > 0)
                {                    
                    for (i= 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }
                }
                cboDept.SelectedIndex = -1;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 특정 칼럼 숨기기 위한 함수
        /// </summary>
        /// <param name="Spd"></param>
        private void Set_Spread_Header_Visible(FarPoint.Win.Spread.SheetView Spd)
        {
            Spd.Columns[5].Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SetClear();
            txtSabun.Focus();
        }

        private void SetClear()
        {
            txtSabun.Text = "";
            txtName.Text = "";
            txtHtel.Text = "";
            txtRowid.Text = "";
            cboDept.SelectedIndex = -1;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            ssCN_Sheet1.Rows.Count = 0;

            SetClear();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    SABUN, SNAME, HTEL, DEPTCODE, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CHARGE_NURSE";
                SQL = SQL + ComNum.VBLF + " WHERE DELDATE Is Null";
                SQL = SQL + ComNum.VBLF + " ORDER BY DEPTCODE ASC, SNAME ASC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssCN_Sheet1.RowCount = dt.Rows.Count + 5;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssCN_Sheet1.Rows.Count += 1;
                        ssCN_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssCN_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssCN_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssCN_Sheet1.Cells[i, 3].Text = dt.Rows[i]["HTEL"].ToString().Trim();
                        ssCN_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            GetData();
        }      

        private bool SaveData()
        {
            bool rtVal = false;            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strSabun = "";
            string strName = "";
            string strDEPT = "";
            string strHTEL = "";
            string strRowid = "";
                        
            strSabun = txtSabun.Text.Trim();
            strName = txtName.Text.Trim();
            strDEPT = cboDept.SelectedItem.ToString().Trim();
            strHTEL = txtHtel.Text.Trim();
            strRowid = txtRowid.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strRowid == "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO ";
                    SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CHARGE_NURSE ";
                    SQL = SQL + ComNum.VBLF + " (SABUN, SNAME, HTEL, DEPTCODE) ";
                    SQL = SQL + ComNum.VBLF + " VALUES ('" + strSabun + "','" + strName + "', ";
                    SQL = SQL + ComNum.VBLF + "  '" + strHTEL + "', '" + strDEPT + "' )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }                    
                }
                else if(strRowid != "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE ";
                    SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CHARGE_NURSE ";
                    SQL = SQL + ComNum.VBLF + " SET HTEL = '" + strHTEL + "', ";
                    SQL = SQL + ComNum.VBLF + " DEPTCODE = '" + strDEPT + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }                    
                }

                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DelData();
            GetData();
        }

        private bool DelData()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strRowid = "";            
            strRowid = txtRowid.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE ";
                SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CHARGE_NURSE ";
                SQL = SQL + ComNum.VBLF + " SET DELDATE = SYSDATE ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void ssCN_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SetClear();

            if (ssCN_Sheet1.Rows.Count < 1)
                return;

            txtSabun.Text = ssCN_Sheet1.Cells[e.Row, 0].Text;
            txtName.Text = ssCN_Sheet1.Cells[e.Row, 1].Text;
            cboDept.SelectedItem = ssCN_Sheet1.Cells[e.Row, 2].Text;
            txtHtel.Text = ssCN_Sheet1.Cells[e.Row, 3].Text;
            txtRowid.Text = ssCN_Sheet1.Cells[e.Row, 4].Text;
        }

        /// <summary>
        /// 사번 입력시 이름을 읽어온다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSabun_KeyPress(object sender, KeyPressEventArgs e)
        {           
            if(txtSabun.Text.Trim() !="" && e.KeyChar == 13)
            {
                txtName.Text = clsOpdNr.READ_INSA_NAME(clsDB.DbCon, txtSabun.Text);
            }
            
        }
    }
}
