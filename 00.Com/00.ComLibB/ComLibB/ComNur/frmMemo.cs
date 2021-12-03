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
    /// File Name       : frmMemo.cs
    /// Description     : 환자별 참고사항
    /// Author          : 유진호
    /// Create Date     : 2018-01-12
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\FrmMemo
    /// </history>
    /// </summary>
    public partial class frmMemo : Form
    {
        ComFunc CF = new ComFunc();
        private string strPano = "";
        private string strDept = "";
        private string strDrCode = "";  // Ex) '0000','1111'
        private string strRowId = "";
        private string strRowId2 = "";

        public frmMemo()
        {
            InitializeComponent();
        }
        
        public frmMemo(string strPano, string strDept, string strDrCode)
        {
            InitializeComponent();
            this.strPano = strPano;
            this.strDept = strDept;
            this.strDrCode = strDrCode;
        }

        private void frmMemo_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (strDrCode != "")
            {
                strDrCode = strDrCode.Replace("'", "");
                string[] strDrTemp = strDrCode.Split(',');
                strDrCode = "";
                foreach (string ss in strDrTemp)
                {
                    strDrCode += "'" + ss + "',";
                }
                strDrCode = strDrCode.Substring(0, strDrCode.Length - 1);

                setCboDept();
            }

            txtPaname.Text = "";
            txtPano.Text = strPano;
            cboDept1.Text = strDept;
            
            btnSearch1.PerformClick();
        }

        private void setCboDept()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                cboDept1.Items.Clear();

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PRINTRANKING,DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE IN (";
                // 2018-03-02 변경
                SQL = SQL + ComNum.VBLF + "   SELECT DRDEPT1 FROM BAS_DOCTOR WHERE DRCODE IN (" + strDrCode + ") ";
                //if (strDrCode != "")
                //{
                //    SQL = SQL + ComNum.VBLF + "   SELECT DRDEPT1 FROM BAS_DOCTOR WHERE DRCODE IN (" + strDrCode + ") ";
                //}
                //else
                //{
                //    SQL = SQL + ComNum.VBLF + "   SELECT DRDEPT1 FROM BAS_DOCTOR WHERE DRCODE IN (" + clsOpdNr.GstrEmrViewDoct + ") ";
                //}                
                SQL = SQL + ComNum.VBLF + "    GROUP BY DRDEPT1) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept1.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());                        
                    }
                    cboDept1.SelectedIndex = 0;
                }

                cboDept2.Items.Clear();

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PrintRanking,DeptCode ";
                SQL = SQL + ComNum.VBLF + "   FROM BAS_CLINICDEPT ";
                if (strDept != "")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ( 'MD', '" + strDept + "' ) ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept2.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                    }
                    cboDept2.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }            
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
            READ_SName(txtPano.Text);

            strPano = txtPano.Text;
            strDept = cboDept1.Text;
            
            btnSearch1Click();
            if (strRowId != "")
            {
                btnDelete1.Enabled = true;
            }
            else
            {
                btnDelete1.Enabled = false;
            }
        }

        private void btnSearch1Click()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                txtMemo.Text = "";
                txtMemo2.Text = "";
                strRowId = "";

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SName FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano='" + strPano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    txtPaname.Text = dt.Rows[i]["SName"].ToString().Trim();                
                }

                //'기존에 자료가 있으면 읽음
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Memo,ROWID, MEMO2 FROM KOSMOS_OCS.OCS_MEMO ";
                SQL = SQL + ComNum.VBLF + " WHERE PtNo='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode='" + strDept + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtMemo.Text = dt.Rows[i]["Memo"].ToString().Trim();
                    txtMemo2.Text = dt.Rows[i]["Memo2"].ToString().Trim();
                    strRowId = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;                
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearch2Click();
        }

        private void btnSearch2Click()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                //'기존에 자료가 있으면 읽음
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Memo,ROWID FROM KOSMOS_OCS.OCS_MEMO2 ";
                SQL = SQL + ComNum.VBLF + " WHERE PtNo='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode='" + VB.Trim(cboDept2.Text) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode2='" + strDept + "' ";
                    
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    txtMemo3.Text = dt.Rows[i]["Memo"].ToString().Trim();                    
                    strRowId2 = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인
            btnSave1Click();

            this.Close();
        }

        private bool btnSave1Click()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (txtMemo.Text.Length > 2000)
            {
                ComFunc.MsgBox("메모는 2000 글자만 가능함");
                return rtVal;
            }
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strRowId == "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_OCS.OCS_MEMO (Ptno,DeptCode,Memo, MEMO2) ";
                    SQL = SQL + ComNum.VBLF + " VALUES ('" +strPano+ "','" +strDept+ "', ";
                    SQL = SQL + ComNum.VBLF + "' " + VB.Replace(VB.Trim(txtMemo.Text),"'","\"") + "' , '" + VB.Replace(VB.Trim(txtMemo2.Text),"'","\"") + "') ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_OCS.OCS_MEMO SET Memo='" + VB.Replace(VB.Trim(txtMemo.Text), "'", "\"") + "', ";
                    SQL = SQL + ComNum.VBLF + " Memo2='" + VB.Replace(VB.Trim(txtMemo2.Text), "'", "\"") + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID='" +strRowId+ "' ";
                }
                
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

        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인
            btnSave2Click();

            this.Close();
        }

        private bool btnSave2Click()
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            if (txtMemo3.Text.Length > 2000)
            {
                ComFunc.MsgBox("메모는 2000 글자만 가능함");
                return rtVal;
            }

            if (VB.Trim(cboDept2.Text) == "")
            {
                ComFunc.MsgBox("타과 공란!!");
                return rtVal;
            }
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'기존에 자료가 있으면 읽음
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Memo,ROWID FROM KOSMOS_OCS.OCS_MEMO2 ";
                SQL = SQL + ComNum.VBLF + "  WHERE PtNo='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DeptCode='" + VB.Trim(cboDept2.Text) + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DeptCode2='" + strDept + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                strRowId = "";

                if (dt.Rows.Count > 0)
                {
                    strRowId = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                if (strRowId == "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_OCS.OCS_MEMO2 (Ptno,DeptCode,DeptCode2,Memo) ";
                    SQL = SQL + ComNum.VBLF + " VALUES ('" +strPano+ "','" + VB.Trim(cboDept2.Text) + "','" +strDept+ "',";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Replace(VB.Trim(txtMemo3.Text), "'", "\"") + "' )  ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_OCS.OCS_MEMO2 SET Memo='" + VB.Replace(VB.Trim(txtMemo3.Text), "'", "\"") + "'  ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID='" + strRowId + "' ";
                }
                
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

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인
            btnDeleteClick();

            this.Close();
        }

        private bool btnDeleteClick()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE KOSMOS_OCS.OCS_MEMO WHERE ROWID='" + strRowId + "' ";

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

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인
            btnDelete2Click();

            txtMemo3.Text = "";
        }

        private bool btnDelete2Click()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE KOSMOS_OCS.OCS_MEMO2 SET Memo =''  ";
                SQL = SQL + ComNum.VBLF + " WHERE PtNo='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode='" + VB.Trim(cboDept2.Text) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode2='" + strDept + "' ";

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


        private void txtPano_Leave(object sender, EventArgs e)
        {
            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
            READ_SName(txtPano.Text);

            strPano = txtPano.Text;
            strDept = cboDept1.Text;
            btnSearch1.PerformClick();
        }


        void READ_SName(string strPano)
        {
            string sPano = ComFunc.SetAutoZero(strPano, 8);

            txtPaname.Text = CF.Read_Patient(clsDB.DbCon, txtPano.Text, "2");

            SendKeys.Send("{TAB}");
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                READ_SName(txtPano.Text);

                strPano = txtPano.Text;
                strDept = cboDept1.Text;
                btnSearch1.PerformClick();
            }
        }
    }
}
