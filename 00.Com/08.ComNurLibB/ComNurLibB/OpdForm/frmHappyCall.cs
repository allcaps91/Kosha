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


namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmHappyCall.cs
    /// Description     : 외래해피콜통계및임의입력
    /// Author          : 유진호
    /// Create Date     : 2018-01-16
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\FrmHappyCall
    /// </history>
    /// </summary>
    public partial class frmHappyCall : Form
    {
        ComFunc CF = new ComFunc();
        private string strROWID = "";


        public frmHappyCall()
        {
            InitializeComponent();
        }

        private void frmHappyCall_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);
            initFrm();
            setCombo();
        }

        private void initFrm()
        {
            dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-10);
            dtpEDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpTDATE.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            txtPTNO.Text = "";
            txtNAME.Text = "";

            cboGubun.Items.Clear();
            cboGubun.Items.Add("01.예약부도");
            cboGubun.Items.Add("02.예약안내");
            cboGubun.Items.Add("03.환자안부");
            cboGubun.SelectedIndex = 0;
        }

        private void setCombo()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                cboDept.Items.Clear();

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }
                }

                cboDept.SelectedIndex = 0;

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

        private void CLEAR_INPUT()
        {
            txtPTNO.Text = "";
            txtNAME.Text = "";
            cboDept.SelectedIndex = 0;
            cboGubun.SelectedIndex = 0;
            dtpTDATE.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            strROWID = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            CLEAR_INPUT();
            ssView1_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;
            btnSearchClick();
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            CLEAR_INPUT();
            ssView2_Sheet1.RowCount = 0;
            btnSearchAllClick();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return;    //권한 확인

            if (btnSaveClick() == true)
            {
                CLEAR_INPUT();
                btnSearchClick();
            }            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return;    //권한 확인

            if (strROWID == "")
            {
                ComFunc.MsgBox("삭제할 내역이 없습니다");
                return;
            }

            btnDeleteClick();
            btnSearchClick();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearchClick()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, DEPTCODE, GUBUN2, SUM(1) CNT ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_HAPPYCALL_OPD";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = '03'";
                SQL = SQL + ComNum.VBLF + "    AND BDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "    AND BDATE <= " + ComFunc.ConvOraToDate(dtpEDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "    AND GUBUN2 > ' '";
                SQL = SQL + ComNum.VBLF + "  GROUP BY BDATE, GUBUN2, DEPTCODE";
                SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE, DEPTCODE, GUBUN2 ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        //ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GUBUN2"].ToString().Trim();

                        if (dt.Rows[i]["GUBUN2"].ToString().Trim() == "01") ssView1_Sheet1.Cells[i, 2].Text = "예약부도";
                        else if (dt.Rows[i]["GUBUN2"].ToString().Trim() == "02") ssView1_Sheet1.Cells[i, 2].Text = "예약안내";
                        else if (dt.Rows[i]["GUBUN2"].ToString().Trim() == "03") ssView1_Sheet1.Cells[i, 2].Text = "환자안부";
                        else ssView1_Sheet1.Cells[i, 2].Text = "";

                        ssView1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    }
                }
                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSearchAllClick()
        {
            READ_DETAIL("", "", "Y");
        }

        private void READ_DETAIL(string ArgBDate, string ArgGubun, string ArgAll = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strPano = "";

            
            try
            {

                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, ";
                SQL = SQL + ComNum.VBLF + "         PANO, DEPTCODE, GUBUN2, (SELECT JUMIN2 FROM " + ComNum.DB_PMPA + "BAS_PATIENT B WHERE A.PANO = B.PANO) JUMIN2,";
                SQL = SQL + ComNum.VBLF + "         DECODE(CONTEXT,'임의등록','Y', '') CONTEXT, ROWID  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_HAPPYCALL_OPD A";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = '03'";
                if (ArgAll == "Y")
                {
                    SQL = SQL + ComNum.VBLF + "    AND GUBUN2 IN ('01','02','03') ";
                    SQL = SQL + ComNum.VBLF + "    AND BDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                    SQL = SQL + ComNum.VBLF + "    AND BDATE <= " + ComFunc.ConvOraToDate(dtpEDate.Value, "D");
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND GUBUN2 = '" + ArgGubun + "'";
                    SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                }
                SQL = SQL + ComNum.VBLF + "    AND GUBUN2 > ' '";
                SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE, GUBUN2, DEPTCODE ";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 1].Text = strPano;
                        ssView2_Sheet1.Cells[i, 2].Text = CF.Read_Patient(clsDB.DbCon, strPano, "2");
                        ssView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 4].Text = clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPano) + "/" + clsVbfunc.READ_SEX(clsDB.DbCon, strPano);
                        //ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GUBUN2"].ToString().Trim();

                        if (dt.Rows[i]["GUBUN2"].ToString().Trim() == "01") ssView2_Sheet1.Cells[i, 5].Text = "예약부도";
                        else if (dt.Rows[i]["GUBUN2"].ToString().Trim() == "02") ssView2_Sheet1.Cells[i, 5].Text = "예약안내";
                        else if (dt.Rows[i]["GUBUN2"].ToString().Trim() == "03") ssView2_Sheet1.Cells[i, 5].Text = "환자안부";
                        else ssView2_Sheet1.Cells[i, 5].Text = "";

                        ssView2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["CONTEXT"].ToString().Trim();

                        if (dt.Rows[i]["CONTEXT"].ToString().Trim() == "Y")
                        {
                            ssView2_Sheet1.Rows[i].ForeColor = Color.FromArgb(0, 0, 250);
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private bool btnSaveClick()
        {
            bool rtnVal = false;
            string strPtNo = "";
            string strGubun= "";
            string strDate = "";
            string strDEPT = "";

            strPtNo = VB.Trim(txtPTNO.Text);
            strGubun = VB.Left(cboGubun.Text, 2);
            strDate = VB.Trim(dtpTDATE.Value.ToShortDateString());
            strDEPT = VB.Trim(cboDept.Text);

            if (strPtNo == "" || strGubun == "" || strDate == "" || strDEPT == "")
            {
                ComFunc.MsgBox("입력 항목이 누락되었습니다.");
                return rtnVal;
            }


            if (clsOpdNr.INSERT_HappyCall_OPD(clsDB.DbCon, "03", strPtNo, strGubun, strDEPT, strDate) == true)
            {
                rtnVal = true;
            }

            return rtnVal;
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
                SQL = SQL + ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "NUR_HAPPYCALL_OPD ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                
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

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strBDATE = "";
            string strGubun = "";

            if (e.RowHeader == true || e.ColumnHeader == true) return;

            strBDATE = ssView1_Sheet1.Cells[e.Row, 0].Text;
            if (ssView1_Sheet1.Cells[e.Row, 2].Text == "예약부도") strGubun = "01";
            else if (ssView1_Sheet1.Cells[e.Row, 2].Text == "예약안내") strGubun = "02";
            else if (ssView1_Sheet1.Cells[e.Row, 2].Text == "환자안부") strGubun = "03";

            READ_DETAIL(strBDATE, strGubun);
        }

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strROWID = "";

            if (e.RowHeader == true || e.ColumnHeader == true) return;
            if (ssView2_Sheet1.Cells[e.Row, 7].Text != "Y") return;
            
            try
            {
                strROWID = ssView2_Sheet1.Cells[e.Row, 6].Text;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PANO, GUBUN2, BDATE, DEPTCODE, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_HAPPYCALL_OPD ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    txtPTNO.Text = dt.Rows[0]["PANO"].ToString().Trim();
                    txtNAME.Text = CF.Read_Patient(clsDB.DbCon, txtPTNO.Text,"2");
                    cboGubun.SelectedIndex = Convert.ToInt32(VB.Val(dt.Rows[0]["GUBUN2"].ToString().Trim())) - 1;
                    cboDept.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    this.strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
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

        private void txtPTNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPTNO.Text = txtPTNO.Text.PadLeft(8, '0');
                txtNAME.Text = CF.Read_Patient(clsDB.DbCon, txtPTNO.Text,"2");
            }
        }

        private void txtPTNO_Leave(object sender, EventArgs e)
        {
            txtPTNO.Text = txtPTNO.Text.PadLeft(8, '0');
            txtNAME.Text = CF.Read_Patient(clsDB.DbCon, txtPTNO.Text,"2");
        }
    }
}
