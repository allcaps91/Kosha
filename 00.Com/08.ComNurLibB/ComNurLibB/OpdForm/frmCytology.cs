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
    /// File Name       : frmCytology.cs
    /// Description     : Cytology 검사 의뢰지 재출력
    /// Author          : 유진호
    /// Create Date     : 2017-01-25
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\FrmCytology
    /// </history>
    /// </summary>
    public partial class frmCytology : Form
    {
        ComFunc CF = new ComFunc();

        private string sPtno = "";
        private string sSpecNo = "";
        private string sSname = "";
        private string sBDate = "";
        private string sAnatNo = "";
        private int sOrderNo = 0;
        private string sGbIO = "";
        private string sDept = "";
        private string sDr = "";
        private string FstrROWID = "";

        public frmCytology()
        {
            InitializeComponent();
        }

        private void frmCytology_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            txtPtno.Text = "";
            btnSave.Enabled = false;

            setEvent();
        }

        private void setEvent()
        {
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인
            btnPrintClick();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (FstrROWID == "")
            {
                ComFunc.MsgBox("대상을 다시 선택하세요");
                return;
            }

            if (btnSaveClick() == true)
            {
                btnSearchClick();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();
        }

        private void btnSearchClick()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strFDate = "";
            string strTDate = "";

            try
            {
                strFDate = dtpDate.Text;
                strTDate = dtpDate.Value.AddDays(1).ToShortDateString();

                txtRemark1.Text = "";
                txtRemark2.Text = "";
                txtRemark3.Text = "";
                txtRemark4.Text = "";
                lblInfo.Text = "";
                FstrROWID = "";

                ssView_Sheet1.RowCount = 0;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.PtNo, B.SName, A.DeptCode, A.DrCode, A.MasterCode, A.SpecNo, ";
                SQL = SQL + ComNum.VBLF + "  A.AnatNo, A.RowID, A.OrderNo ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.EXAM_ANATMST A, KOSMOS_OCS.EXAM_SPECMST B ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.BDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND A.BDate <  TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                if (VB.Trim(txtPtno.Text) != "")
                {
                    SQL = SQL + ComNum.VBLF + "AND A.Ptno = '" + VB.Trim(txtPtno.Text) + "'";
                }
                SQL = SQL + ComNum.VBLF + "    AND A.SpecNo = B.SpecNo ";
                SQL = SQL + ComNum.VBLF + "    AND (A.AnatNo LIKE 'C%' OR A.AnatNo LIKE 'P%')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("대상자 없음");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAme"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 4].Text = READ_MasterName_new(dt.Rows[i]["MasterCode"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SpecNo"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["AnatNo"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["RowID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["OrderNo"].ToString().Trim();
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

        private bool btnSaveClick()
        {
            return false;
            //bool rtVal = false;
            //int i = 0;
            //string SQL = "";
            //string SqlErr = "";
            //int intRowAffected = 0;

            //Cursor.Current = Cursors.WaitCursor;
            //clsDB.setBeginTran(clsDB.DbCon);

            //try
            //{

            //    SQL = "";
            //    SQL = SQL + ComNum.VBLF + "UPDATE  KOSMOS_OCS.EXAM_ANATMST  SET ";
            //    SQL = SQL + ComNum.VBLF + "  REMARK1 = '" + txtRemark1.Text + "', ";
            //    SQL = SQL + ComNum.VBLF + "  REMARK2 = '" + txtRemark2.Text + "', ";
            //    SQL = SQL + ComNum.VBLF + "  REMARK3 = '" + txtRemark3.Text + "', ";
            //    SQL = SQL + ComNum.VBLF + "  REMARK4 = '" + txtRemark4.Text + "'";
            //    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

            //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            //    if (SqlErr != "")
            //    {
            //        clsDB.setRollbackTran(clsDB.DbCon);
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
            //        Cursor.Current = Cursors.Default;
            //        return rtVal;
            //    }

            //    clsDB.setCommitTran(clsDB.DbCon);
            //    Cursor.Current = Cursors.Default;
            //    rtVal = true;
            //    return rtVal;
            //}
            //catch (Exception ex)
            //{
            //    clsDB.setRollbackTran(clsDB.DbCon);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //    ComFunc.MsgBox(ex.Message);
            //    Cursor.Current = Cursors.Default;
            //    return rtVal;
            //}
        }

        private void btnPrintClick()
        {
        }

        private string READ_MasterName_new(string ArgCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ArgCode == "") return rtnVal;

                //'BAS_SUN에서 해당 자료를 READ
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ExamName FROM KOSMOS_OCS.Exam_Master ";
                SQL = SQL + ComNum.VBLF + " WHERE MasterCode = '" + ArgCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ExamName"].ToString().Trim();
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

            return rtnVal;
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strSname = "";
            string strROWID = "";

            if (e.RowHeader == true || e.ColumnHeader == true) return;

            strSname = ssView_Sheet1.Cells[e.Row, 1].Text;
            strROWID = ssView_Sheet1.Cells[e.Row, 7].Text;
            FstrROWID = strROWID;
            txtPtno.Text = ssView_Sheet1.Cells[e.Row, 0].Text;
            txtRemark1.Text = "";
            txtRemark2.Text = "";
            txtRemark3.Text = "";
            lblInfo.Text = strSname + " / " + ssView_Sheet1.Cells[e.Row, 4].Text;

            Remark_Display(strSname, strROWID);
        }

        private void Remark_Display(string ArgSName, string ArgROWID)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            sPtno = "";
            sSpecNo = "";
            sSname = "";
            sBDate = "";
            sGbIO = "";
            sDept = "";
            sDr = "";
            sOrderNo = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Ptno,OrderCode, GbIO, SpecNo, AnatNo, DeptCode,OrderNo,";
                SQL = SQL + ComNum.VBLF + "  DrCode, TO_CHAR(BDate, 'YYYY-MM-DD') BDate, ";
                SQL = SQL + ComNum.VBLF + "  Remark1,Remark2,Remark3,Remark4 ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.EXAM_ANATMST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + ArgROWID + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    sPtno = dt.Rows[0]["Ptno"].ToString().Trim();
                    sSpecNo = dt.Rows[0]["SpecNo"].ToString().Trim();
                    sSname = ArgSName;
                    sBDate = dt.Rows[0]["BDate"].ToString().Trim();
                    sAnatNo = dt.Rows[0]["AnatNo"].ToString().Trim();
                    sGbIO = dt.Rows[0]["GbIO"].ToString().Trim();
                    sDept = dt.Rows[0]["DeptCode"].ToString().Trim();
                    sDr = dt.Rows[0]["DrCode"].ToString().Trim();
                    sOrderNo = Convert.ToInt32(VB.Val(dt.Rows[0]["OrderNo"].ToString().Trim()));
                    txtRemark1.Text = VB.Trim(dt.Rows[0]["Remark1"].ToString().Trim());
                    txtRemark2.Text = VB.Trim(dt.Rows[0]["Remark2"].ToString().Trim());
                    txtRemark3.Text = VB.Trim(dt.Rows[0]["Remark3"].ToString().Trim());
                    txtRemark4.Text = VB.Trim(dt.Rows[0]["Remark4"].ToString().Trim());
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
    }
}
