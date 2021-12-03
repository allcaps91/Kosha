using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class frmMedViewExam : Form
    {
        private string GstrPtno = "";
        private string GstrInDate = "";
        private string GstrBDate = "";

        /// <summary>
        /// 생성자 필수
        /// </summary>
        /// <param name="strPtno">등록번호</param>
        /// <param name="strInDate">입원일자</param>
        /// <param name="strBDate">처방일자</param>
        public frmMedViewExam(string strPtno, string strInDate, string strBDate)
        {
            InitializeComponent();

            GstrPtno = strPtno;
            GstrInDate = strInDate;
            GstrBDate = strBDate;
        }

        private void frmMedViewExam_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            this.Location = new Point(250, 300);

            lblTitleSub0.Text = "";
            lblTitleSub0.Text = GstrPtno + " " + clsVbfunc.GetPatientName(clsDB.DbCon, GstrPtno);

            ssView_Sheet1.RowCount = 0;

            GetData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoExam_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                GetData();
            }
        }

        private void GetData()
        {
            string strSpecCode = "";
            string strSpecNo = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (rdoExam3.Checked == true)
                {
                    SQL = "";
                    SQL = "SELECT /* INDEX(EXAM_SPECMST INDEX_EXAMSPECMST3) */ "; //'채혈일시;
                    SQL = SQL + ComNum.VBLF + "      A.SPECNO, A.WORKSTS, A.SPECCODE, A.STATUS, ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.BLOODDATE,'YYYY-MM-DD HH24:MI') BLOODDATE,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.ORDERDATE,'YYYY-MM-DD HH24:MI') ORDERDATE,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') RECEIVEDATE,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE ";
                    SQL = SQL + ComNum.VBLF + " ,    (SELECT NAME FROM ADMIN.EXAM_SPECODE WHERE GUBUN = '14' AND CODE = A.SPECCODE AND ROWNUM = 1) AS BASCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST A";
                    SQL = SQL + ComNum.VBLF + " WHERE A.IPDOPD IN ('O') ";

                    if (Convert.ToDateTime(GstrBDate).AddDays(-21) >= Convert.ToDateTime(GstrInDate))
                    {
                        SQL = SQL + ComNum.VBLF + "AND A.BLOODDATE >= TO_DATE('" + Convert.ToDateTime(GstrBDate).AddDays(-21).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "AND A.BLOODDATE >= TO_DATE('" + GstrInDate + "','YYYY-MM-DD') ";
                    }

                    SQL = SQL + ComNum.VBLF + "  AND A.PANO = '" + GstrPtno + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND (A.CANCEL IS NULL OR A.CANCEL = '' OR A.CANCEL = ' ') ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.SPECNO, A.WORKSTS, A.SPECCODE, A.STATUS, ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.BDATE,'YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.BLOODDATE,'YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.ORDERDATE,'YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') DESC, A.SPECNO";
                }
                else
                {
                    SQL = "";
                    SQL = "SELECT /* INDEX(EXAM_SPECMST INDEX_EXAMSPECMST3) */ "; //'채혈일시;
                    SQL = SQL + ComNum.VBLF + "      A.SPECNO, A.WORKSTS, A.SPECCODE, A.STATUS, ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.BLOODDATE,'YYYY-MM-DD HH24:MI') BLOODDATE,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.ORDERDATE,'YYYY-MM-DD HH24:MI') ORDERDATE,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') RECEIVEDATE,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE, SUM(B.QTY*B.NAL) ";
                    SQL = SQL + ComNum.VBLF + " ,    (SELECT NAME FROM ADMIN.EXAM_SPECODE WHERE GUBUN = '14' AND CODE = A.SPECCODE AND ROWNUM = 1) AS BASCODE ";

                    if (rdoExam0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST A, ADMIN.OCS_IORDER B";
                        SQL = SQL + ComNum.VBLF + " WHERE A.IPDOPD = 'I'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST A, ADMIN.OCS_IORDER B";
                        SQL = SQL + ComNum.VBLF + " WHERE A.IPDOPD IN ('O','I') ";
                    }

                    if (rdoExam0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.BLOODDATE >= TO_DATE('" + GstrInDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND A.STATUS ='00' ";         //'미접수;
                    }
                    else
                    {
                        //if (Convert.ToDateTime(mstrMedFrDate).AddDays(-21) >= Convert.ToDateTime(ComFunc.FormatStrToDate(GstrInDate, "D")))
                        //{
                        //    SQL = SQL + ComNum.VBLF + "AND A.BLOODDATE >= TO_DATE('" + Convert.ToDateTime(mstrMedFrDate).AddDays(-21).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        //}
                        //else
                        //{
                        //    SQL = SQL + ComNum.VBLF + "AND A.BLOODDATE >= TO_DATE('" + GstrInDate + "','YYYY-MM-DD') ";
                        //}

                        SQL = SQL + ComNum.VBLF + "AND A.BLOODDATE >= TO_DATE('" + GstrInDate + "','YYYY-MM-DD') ";

                        if (rdoExam1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND A.STATUS IN ('04','05') ";         //'검사완료
                        }

                        if (rdoExam2.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND A.STATUS IN ('01', '02', '03') ";         //'접수/검사중
                        }
                    }

                    if (rdoExam3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND B.ORDERSITE LIKE 'OPD%' ";
                    }

                    SQL = SQL + ComNum.VBLF + "  AND A.PANO = '" + GstrPtno + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND (A.CANCEL IS NULL OR A.CANCEL = '' OR A.CANCEL = ' ') ";
                    SQL = SQL + ComNum.VBLF + "  AND A.PANO = B.PTNO";
                    SQL = SQL + ComNum.VBLF + "  AND A.BDATE = B.BDATE ";
                    SQL = SQL + ComNum.VBLF + "  AND A.ORDERNO = B.ORDERNO ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.SPECNO, A.WORKSTS, A.SPECCODE, A.STATUS, ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.BDATE,'YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.BLOODDATE,'YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.ORDERDATE,'YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "      HAVING SUM(B.QTY * B.NAL) > 0";
                    SQL = SQL + ComNum.VBLF + "ORDER BY TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') DESC, A.SPECNO";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSpecNo = dt.Rows[i]["SPECNO"].ToString().Trim();
                        strSpecCode = dt.Rows[i]["SpecCode"].ToString().Trim();

                        if (VB.IsDate(dt.Rows[i]["BDATE"].ToString().Trim()) == true)
                        {
                            ssView_Sheet1.Cells[i, 0].Text = Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }

                        if (VB.IsDate(dt.Rows[i]["BLOODDATE"].ToString().Trim()) == true)
                        {
                            ssView_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["BLOODDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }

                        ssView_Sheet1.Cells[i, 2].Text = READ_Specno_ExamName(strSpecNo);
                        if (ssView_Sheet1.Cells[i, 2].Text.Trim().IndexOf("배양(호기)") > -1)
                        {
                            //ssView_Sheet1.Cells[i, 2].Text = "균배양검사(검체:" + READ_BasCode(strSpecCode) + ")";
                            ssView_Sheet1.Cells[i, 2].Text = "균배양검사(검체:" + dt.Rows[i]["BASCODE"].ToString().Trim() + ")";
                        }

                        ssView_Sheet1.Cells[i, 3].Text = READ_BasCode(strSpecCode);
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BASCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = strSpecNo;
                        ssView_Sheet1.Cells[i, 5].Text = strSpecCode;
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["RESULTDATE"].ToString().Trim();

                        switch (dt.Rows[i]["STATUS"].ToString().Trim())
                        {
                            case "00":
                                ssView_Sheet1.Rows.Get(i).BackColor = rdoExam0.BackColor;
                                break;
                            case "04":
                            case "05":
                                ssView_Sheet1.Rows.Get(i).BackColor = rdoExam1.BackColor;
                                break;
                            case "01":
                                ssView_Sheet1.Rows.Get(i).BackColor = rdoExam2.BackColor;
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
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

        /// <summary>
        /// '검체번호별 검사명칭을 찾기
        /// </summary>
        /// <param name="strSpecNo"></param>
        /// <returns></returns>
        private string READ_Specno_ExamName(string strSpecNo)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT B.WSCODE1,B.WSCODE1POS,A.MASTERCODE,B.EXAMNAME,COUNT(A.MASTERCODE) CNT ";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EXAM_RESULTC A,ADMIN.EXAM_MASTER B ";
            SQL = SQL + ComNum.VBLF + " WHERE A.SPECNO = '" + strSpecNo + "' ";
            SQL = SQL + ComNum.VBLF + "   AND A.MASTERCODE = A.SUBCODE ";
            SQL = SQL + ComNum.VBLF + "   AND A.MASTERCODE = B.MASTERCODE(+) ";
            SQL = SQL + ComNum.VBLF + " GROUP BY B.WSCODE1,B.WSCODE1POS,A.MASTERCODE,B.EXAMNAME ";
            SQL = SQL + ComNum.VBLF + " ORDER BY B.WSCODE1,B.WSCODE1POS,A.MASTERCODE,B.EXAMNAME ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rtnVal = rtnVal + dt.Rows[i]["EXAMNAME"].ToString().Trim();
                if (VB.Val(dt.Rows[i]["CNT"].ToString()) > 1)
                {
                    rtnVal = rtnVal + "*" + VB.Val(dt.Rows[i]["CNT"].ToString()) + ",";
                }
                else
                {
                    rtnVal = rtnVal + ",";
                }
            }

            if (rtnVal != "")
            {
                rtnVal = VB.Left(rtnVal, rtnVal.Length - 1);
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        private string READ_BasCode(string argCode)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT NAME      ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EXAM_SPECODE ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN ='14' "; //'검체;
                SQL = SQL + ComNum.VBLF + "    AND CODE = '" + argCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
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
                return RtnVal;
            }
        }
    }
}
