using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmInpeulluinja : Form
    {
        /// Class Name      : ComLibB.dll
        /// File Name       : frmyoyang02.cs
        /// Description     : 신종인플루인자 제외자관리
        /// Author          : 김효성
        /// Create Date     : 2017-06-20
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// VB\basic\buppat\Frm인플루인자관리.frm => frmInpeulluinja.cs 으로 변경함
        /// </history>
        /// <seealso> 
        /// VB\basic\buppat\Frm인플루인자관리.frm(frmInpeulluinja)
        /// </seealso>
        /// <vbp>
        /// default : VB\basic\buppat\buppat.vbp
        /// </vbp>
        string FstrROWID = "";

        public frmInpeulluinja ()
        {
            InitializeComponent ();
        }

        private void frmInpeulluinja_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ssDel_Sheet1.Columns [7].Visible = false;

            string strDate = "";

            strDate = ComFunc.FormatStrToDateEx (ComQuery.CurrentDateTime (clsDB.DbCon, "D") , "D" , "-");

            dtpS.Value = Convert.ToDateTime (strDate);
            dtpE.Value = Convert.ToDateTime (strDate);

            FstrROWID = "";
            lblInfo.Text = "";

        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private void btnDelete_Click (object sender , EventArgs e)
        {
            if (ComQuery.IsJobAuth(this , "D", clsDB.DbCon) == false) return;//권한 확인

            if (Delet () == true)
            {
                DelSeaech ();
            }
        }

        private bool Delet ()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (FstrROWID == "") ComFunc.MsgBox ("삭제할 환자를 먼저 선택하세요" , "");

            clsDB.setBeginTran(clsDB.DbCon);
            

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " UPDATE " + ComNum.DB_PMPA + "ETC_INFLU_OK  SET DELDATE =SYSDATE ";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID ='" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran (clsDB.DbCon);
                    ComFunc.MsgBox (SqlErr);
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("삭제 하였습니다.");
                Cursor.Current = Cursors.Default;
                btnDelete.Enabled = false;
                FstrROWID = "";
                lblInfo.Text = "";
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool DelSeaech ()
        {
            int i = 0;
            string SQL = "";
            bool rtVat = false;
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return rtVat; //권한 확인

            btnDelete.Enabled = false;
            FstrROWID = "";
            lblInfo.Text = "";

            try
            {
                SQL = "";
                SQL = " SELECT a.Pano,b.SName,a.EntSabun,a.Remark,a.ROWID,b.Jumin1 || '-' || b.Jumin2 Jumin , ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.SDate,'YYYY-MM-DD') SDate, TO_CHAR(a.DelDate,'YYYY-MM-DD') DelDate ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ETC_INFLU_OK a, ADMIN.BAS_PATIENT b";
                SQL = SQL + ComNum.VBLF + "  WHERE a.PANO=b.PANO(+)";
                SQL = SQL + ComNum.VBLF + "    AND a.SDate >=TO_DATE('" + dtpS.Value.ToString ("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND a.SDate <=TO_DATE('" + dtpE.Value.ToString ("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (chkall.Checked == false) SQL = SQL + ComNum.VBLF + "  AND ( a.DELDATE IS NULL OR a.DELDATE ='') ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return rtVat;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return rtVat;
                }

                ssDel_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssDel_Sheet1.Cells [i , 0].Text = dt.Rows [i] ["Pano"].ToString ().Trim ();
                    ssDel_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["SName"].ToString ().Trim ();
                    ssDel_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["SDate"].ToString ().Trim ();
                    ssDel_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["EntSabun"].ToString ().Trim ();
                    ssDel_Sheet1.Cells [i , 4].Text = dt.Rows [i] ["Remark"].ToString ().Trim ();
                    ssDel_Sheet1.Cells [i , 5].Text = dt.Rows [i] ["DelDate"].ToString ().Trim ();
                    ssDel_Sheet1.Cells [i , 6].Text = dt.Rows [i] ["Jumin"].ToString ().Trim ();
                    ssDel_Sheet1.Cells [i , 7].Text = dt.Rows [i] ["ROWID"].ToString ().Trim ();
                }

                dt.Dispose ();
                dt = null;
                rtVat = true;
                return rtVat;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox (ex.Message);
                return rtVat;
            }
        }

        private void btnBottomSearch_Click (object sender , EventArgs e)
        {
            DelSeaech ();
        }

        private void BtnTopSearch ()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = ""; //에러문 받는 변수

            btnDelete.Enabled = false;
            FstrROWID = "";
            lblInfo.Text = "";

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return; //권한 확인

            try
            {
                SQL = " SELECT a.Pano,a.INFLUAG,a.INFLUAPR, b.SName ";
                SQL = SQL + ComNum.VBLF + " FROM  " + ComNum.DB_MED + "EXAM_INFECTMASTER a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.PANO=b.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "   AND ( a.INFLUAG IS NOT NULL Or a.INFLUAPR IS NOT NULL ) ";
                SQL = SQL + ComNum.VBLF + "  AND a.PANO <> '81000004' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }

                ssDel_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSearch_Sheet1.Cells [i , 0].Text = dt.Rows [i] ["Pano"].ToString ().Trim ();
                    ssSearch_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["SName"].ToString ().Trim ();
                    ssSearch_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["INFLUAG"].ToString ().Trim ();
                    ssSearch_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["INFLUAPR"].ToString ().Trim ();

                    SQL = "";
                    SQL = " SELECT a.Pano,b.SName,a.EntSabun,a.Remark,b.Jumin1 || '-' || b.Jumin2 Jumin , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.SDate,'YYYY-MM-DD') SDate, TO_CHAR(a.DelDate,'YYYY-MM-DD') DelDate ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_INFLU_OK a, ADMIN.BAS_PATIENT b";
                    SQL = SQL + ComNum.VBLF + "  WHERE a.PANO=b.PANO(+)";
                    SQL = SQL + ComNum.VBLF + "  AND a.PANO ='" + dt.Rows [i] ["Pano"].ToString ().Trim () + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND ( a.DELDATE IS NULL OR a.DELDATE ='') ";

                    SqlErr = clsDB.GetDataTable (ref dt1 , SQL, clsDB.DbCon);

                    if (dt1.Rows.Count > 0)
                    {
                        ssSearch_Sheet1.Cells [i , 4].Text = "◎";
                        ssSearch_Sheet1.Cells [i , 5].Text = dt.Rows [0] ["SDate"].ToString ().Trim ();
                        ssSearch_Sheet1.Cells [i , 6].Text = clsVbfunc.GetInSaName (clsDB.DbCon, dt.Rows [0] ["EntSabun"].ToString ().Trim ());
                        ssSearch_Sheet1.Cells [i , 7].Text = dt.Rows [0] ["Remark"].ToString ().Trim ();
                    }
                    dt1.Dispose ();
                    dt1 = null;
                }
                dt.Dispose ();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnTopSearch_Click (object sender , EventArgs e)
        {
            BtnTopSearch ();
        }

        private void ssDel_CellDoubleClick (object sender , FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string StrSname = "";
            string strPano = "";
            string strJumin = "";

            if (ssDel_Sheet1.ColumnCount == 0 || ssDel_Sheet1.RowCount == 0) return;

            strPano = ssDel_Sheet1.Cells [e.Row , 0].Text;
            StrSname = ssDel_Sheet1.Cells [e.Row , 1].Text;
            strJumin = ssDel_Sheet1.Cells [e.Row , 6].Text;
            FstrROWID = ssDel_Sheet1.Cells [e.Row , 7].Text;

            lblInfo.Text = "[등록번호 :" + strPano + " 성명 : " + StrSname + " 주민번호 : " + strJumin + "]";

            if (FstrROWID != "") btnDelete.Enabled = true;
        }
    }
}
