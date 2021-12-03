using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class frmErOpList : Form
    {
        public frmErOpList()
        {
            InitializeComponent();
        }

        private void frmErOpList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView1_Sheet1.RowCount = 0;

            dtpEDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            dtpSDATE.Value = dtpEDATE.Value.AddDays(-20);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //'ROOM

                cboOpRoom.Items.Clear();

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT CODE, NAME FROM OPR_CODE   ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = '1'               ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CODE                   ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboOpRoom.Items.Add("전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboOpRoom.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboOpRoom.SelectedIndex = 0;

                //'진료과

                cboDept.Items.Clear();

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT CODE FROM OPR_CODE    ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = '2'            ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CODE                ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboDept.Items.Add("전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["CODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboDept.SelectedIndex = 0;


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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strPath = "";
            string strROWID = "";

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ssView1_Sheet1.RowCount == 0)
            {
                ComFunc.MsgBox("저장 할 데이터가 없습니다.", "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {


                for (i = 0; i < ssView1_Sheet1.RowCount; i++)
                {
                    strROWID = ssView1_Sheet1.Cells[i, 16].Text.Trim();
                    strPath = ssView1_Sheet1.Cells[i, 7].Text.Trim();

                    SQL = "";
                    SQL = " UPDATE KOSMOS_PMPA.ORAN_MASTER SET ";
                    SQL = SQL + ComNum.VBLF + " PATH_IN = '" + strPath + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                READ_DATA();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            READ_DATA();
        }

        private void READ_DATA()
        {
            int i = 0;
            string strInDate = "";
            string strOutDate = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "  SELECT DISTINCT TO_CHAR( INDATE ,'YYYY-MM-DD HH24:MI') INDATE ,OPROOM, A.SNAME, A.PANO, A.OUTDATE, KTASLEVL, ROOMREMK ,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(OPDATE,'YYYY-MM-DD')  OPDATE,TO_CHAR(OPDATE+1,'YYYY-MM-DD')  OPDATE1,DR_STIME, OPETIME, B.DEPTCODE,B.DIAGNOSIS, B.ROWID, B.WARDCODE, B.PATH_IN   ";
                SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER A , ORAN_MASTER B   ";
                SQL = SQL + ComNum.VBLF + " WHERE KTASLEVL IS NOT NULL AND AMSET7 IN ('3','4','5') ";
                SQL = SQL + ComNum.VBLF + " AND GBSTS <> '9'  ";
                SQL = SQL + ComNum.VBLF + " AND B.OPDATE >= TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND B.OPDATE <= TO_DATE('" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND A.PANO =B.PANO  ";
                SQL = SQL + ComNum.VBLF + " AND   TO_DATE(TO_CHAR(B.OPDATE,'YYYY-MM-DD')||' '||B.SWARDTIME,'YYYY-MM-DD HH24:MI')  BETWEEN A.INDATE AND A.INDATE+1  ";
                SQL = SQL + ComNum.VBLF + " AND KTASLEVL > 0 AND KTASLEVL < 4  ";

                if (cboOpRoom.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND OPROOM = '" + VB.Left(cboOpRoom.Text.Trim(), 1) + "'  ";
                }

                if (cboDept.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND B.DEPTCODE = '" + cboDept.Text.Trim() + "'  ";
                }

                SQL = SQL + ComNum.VBLF + " AND OPROOM NOT IN ('A','B','G','N')   ";
                SQL = SQL + ComNum.VBLF + " AND (B.GBANGIO IS NULL OR B.GBANGIO <> 'Y') ";
                SQL = SQL + ComNum.VBLF + " AND B.OPROOM <> 'N' AND B.OPCANCEL IS NULL  ";
                SQL = SQL + ComNum.VBLF + "      ORDER BY OPDATE DESC  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    //'ROOMCODE
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["OPROOM"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();

                        READ_INDATE_OUTDATE_ER(dt.Rows[i]["PANO"].ToString().Trim()
                        , Convert.ToDateTime(dt.Rows[i]["INDATE"].ToString().Trim()).ToString("yyyy-MM-dd")
                        , ref strInDate
                        , ref strOutDate);

                        ssView1_Sheet1.Cells[i, 3].Text = VB.Left(strInDate, 10); //'응급실입실시간
                        ssView1_Sheet1.Cells[i, 4].Text = VB.Right(strInDate, 5); //'응급실입실시간
                        ssView1_Sheet1.Cells[i, 5].Text = VB.Left(strOutDate, 10); // '응급실퇴실시간
                        ssView1_Sheet1.Cells[i, 6].Text = VB.Right(strOutDate, 5); //'응급실퇴실시간
                        ssView1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PATH_IN"].ToString().Trim();

                        if (ssView1_Sheet1.Cells[i, 7].Text.Trim() == "" || chkReLoad.Checked == true)
                        {
                            ssView1_Sheet1.Cells[i, 7].Text = clsErNr.READ_NEDIS_IPWON_GUBUN_OP(dt.Rows[i]["PANO"].ToString().Trim(), VB.Left(strInDate, 10), VB.Left(strOutDate, 10));
                        }

                        //ssView1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();

                        ssView1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["OPDATE"].ToString().Trim(); //'수술시작
                        ssView1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DR_STIME"].ToString().Trim(); //'수술시작

                        if (VB.Val(dt.Rows[i]["DR_STIME"].ToString().Trim()) > VB.Val(dt.Rows[i]["OPETIME"].ToString().Trim()))
                        {
                            ssView1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["OPDATE1"].ToString().Trim(); // '수술종료
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["OPDATE"].ToString().Trim(); // '수술종료
                        }

                        ssView1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["OPETIME"].ToString().Trim(); //'수술종료
                        ssView1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim(); //'의뢰과
                        ssView1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["DIAGNOSIS"].ToString().Trim(); //'수술후진단명
                        ssView1_Sheet1.Cells[i, 14].Text = dt.Rows[i]["KTASLEVL"].ToString().Trim(); //'KTAS

                        if (dt.Rows[i]["ROOMREMK"].ToString().Trim() == "Y")
                        {
                            ssView1_Sheet1.Cells[i, 15].Text = "주과 수술방 사용";
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[i, 15].Text = "";
                        }

                        ssView1_Sheet1.Cells[i, 16].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
        /// READ_INDATE_ER, READ_OUTDATE_ER 대신 한번의 조회로 변수에 값을 넣어주는 방식으로 변경
        /// </summary>
        /// <param name="agePano"></param>
        /// <param name="ageInDate">yyyy-MM-dd 형식으로 넣기</param>
        /// <param name="strInDate">yyyy-MM-dd HH:mm 형식으로 반환함</param>
        /// <param name="strOutDate">yyyy-MM-dd HH:mm 형식 또는 yyyy-MM-dd    HH:mm 형식으로 반환함</param>
        private void READ_INDATE_OUTDATE_ER(string agePano, string ageInDate, ref string strInDate, ref string strOutDate)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT   ";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(TO_DATE(CHARTDATE || ' ' || SUBSTR(CHARTTIME, 1, 4), 'YYYY-MM-DD HH24:MI'), 'YYYY-MM-DD HH24:MI') AS CHARTDATE,";
                SQL = SQL + ComNum.VBLF + "NVL(EXTRACTVALUE(CHARTXML, '//it104'), EXTRACTVALUE(CHARTXML, '//dt4') || ' ' || EXTRACTVALUE(CHARTXML, '//it106')) AS OUTTIME";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + agePano + "'";
                SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = REPLACE('" + ageInDate + "', '-', '')";
                SQL = SQL + ComNum.VBLF + "    AND FORMNO IN('2506', '2678')";
                SQL = SQL + ComNum.VBLF + "ORDER BY  CHARTTIME ASC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strInDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    strOutDate = dt.Rows[0]["OUTTIME"].ToString().Trim();
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + " TO_char(intime,'YYYY-MM-DD HH24:MI') intime,TO_char(outtime,'YYYY-MM-DD HH24:MI') outtime  ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "nur_er_patient";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND PANO = '" + agePano + "'";
                    SQL += ComNum.VBLF + "      AND JDATE = to_date('" + ageInDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strInDate = dt.Rows[0]["intime"].ToString().Trim();
                        strOutDate = dt.Rows[0]["outtime"].ToString().Trim();


                    }

                    dt.Dispose();
                    dt = null;

                }
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


        ///// <summary>
        ///// READ_INDATE_ER, READ_OUTDATE_ER 대신 한번의 조회로 변수에 값을 넣어주는 방식으로 변경
        ///// </summary>
        ///// <param name="agePano"></param>
        ///// <param name="ageInDate">yyyy-MM-dd 형식으로 넣기</param>
        ///// <param name="strInDate">yyyy-MM-dd HH:mm 형식으로 반환함</param>
        ///// <param name="strOutDate">yyyy-MM-dd HH:mm 형식 또는 yyyy-MM-dd    HH:mm 형식으로 반환함</param>
        //private void READ_INDATE_OUTDATE_ER(string agePano, string ageInDate, ref string strInDate, ref string strOutDate)
        //{
        //    DataTable dt = null;
        //    string SQL = "";
        //    string SqlErr = ""; //에러문 받는 변수
        //    Cursor.Current = Cursors.WaitCursor;

        //    try
        //    {
        //        SQL = "";
        //        SQL = "SELECT   ";
        //        SQL = SQL + ComNum.VBLF + "TO_CHAR(TO_DATE(CHARTDATE || ' ' || SUBSTR(CHARTTIME, 1, 4), 'YYYY-MM-DD HH24:MI'), 'YYYY-MM-DD HH24:MI') AS CHARTDATE,";
        //        SQL = SQL + ComNum.VBLF + "NVL(EXTRACTVALUE(CHARTXML, '//it104'), EXTRACTVALUE(CHARTXML, '//dt4') || ' ' || EXTRACTVALUE(CHARTXML, '//it106')) AS OUTTIME";
        //        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML";
        //        SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + agePano + "'";
        //        SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = REPLACE('" + ageInDate + "', '-', '')";
        //        SQL = SQL + ComNum.VBLF + "    AND FORMNO IN('2506', '2678')";
        //        SQL = SQL + ComNum.VBLF + "ORDER BY  CHARTTIME ASC";

        //        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

        //        if (SqlErr != "")
        //        {
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            Cursor.Current = Cursors.Default;
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            return;
        //        }

        //        if (dt.Rows.Count > 0)
        //        {
        //            strInDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
        //            strOutDate = dt.Rows[0]["OUTTIME"].ToString().Trim();
        //            dt.Dispose();
        //            dt = null;
        //        }
        //        else
        //        {
        //            dt.Dispose();
        //            dt = null;

        //            SQL = "";
        //            SQL += ComNum.VBLF + "SELECT ";
        //            SQL += ComNum.VBLF + "  MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME";
        //            SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
        //            SQL += ComNum.VBLF + "WHERE 1=1";
        //            SQL += ComNum.VBLF + "      AND PTNO = '" + agePano + "'";
        //            SQL += ComNum.VBLF + "      AND MEDFRDATE = REPLACE('" + ageInDate + "', '-', '')";
        //            SQL += ComNum.VBLF + "      AND FORMNO = '2049'";
        //            SQL += ComNum.VBLF + "ORDER BY WRITETIME DESC";

        //            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

        //            if (SqlErr != "")
        //            {
        //                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //                Cursor.Current = Cursors.Default;
        //                ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //                return;
        //            }

        //            if (dt.Rows.Count > 0)
        //            {
        //                strInDate = VB.Left(dt.Rows[0]["MEDFRDATE"].ToString().Trim(), 4) + "-"
        //                        + VB.Mid(dt.Rows[0]["MEDFRDATE"].ToString().Trim(), 5, 2) + "-"
        //                        + VB.Mid(dt.Rows[0]["MEDFRDATE"].ToString().Trim(), 7, 8) + " "
        //                        + VB.Left(dt.Rows[0]["MEDFRTIME"].ToString().Trim(), 2) + ":"
        //                        + VB.Mid(dt.Rows[0]["MEDFRTIME"].ToString().Trim(), 3, 2);

        //                if (dt.Rows[0]["MEDENDDATE"].ToString().Trim() != "")
        //                {
        //                    if (dt.Rows[0]["MEDENDTIME"].ToString().Trim() != "")
        //                    {
        //                        strOutDate = VB.Left(dt.Rows[0]["MEDENDDATE"].ToString().Trim(), 4) + "-"
        //                            + VB.Mid(dt.Rows[0]["MEDENDDATE"].ToString().Trim(), 5, 2) + "-"
        //                            + VB.Mid(dt.Rows[0]["MEDENDDATE"].ToString().Trim(), 7, 8) + " "
        //                            + VB.Left(dt.Rows[0]["MEDENDTIME"].ToString().Trim(), 2) + ":"
        //                            + VB.Mid(dt.Rows[0]["MEDENDTIME"].ToString().Trim(), 3, 2);
        //                    }
        //                    else
        //                    {
        //                        strOutDate = VB.Left(dt.Rows[0]["MEDENDDATE"].ToString().Trim(), 4) + "-"
        //                           + VB.Mid(dt.Rows[0]["MEDENDDATE"].ToString().Trim(), 5, 2) + "-"
        //                           + VB.Mid(dt.Rows[0]["MEDENDDATE"].ToString().Trim(), 7, 8);
        //                    }
        //                }
        //            }

        //            dt.Dispose();
        //            dt = null;

        //        }
        //        Cursor.Current = Cursors.Default;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (dt != null)
        //        {
        //            dt.Dispose();
        //            dt = null;
        //        }
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
        //        Cursor.Current = Cursors.Default;
        //        ComFunc.MsgBox(ex.Message);
        //    }
        //}

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            FarPoint.Win.ComplexBorder borderWhite = new FarPoint.Win.ComplexBorder(	//ㄱ
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border1 = new FarPoint.Win.ComplexBorder(	//ㄷ
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border2 = new FarPoint.Win.ComplexBorder(	//ㄷ
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border3 = new FarPoint.Win.ComplexBorder(	//ㄴ
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border4 = new FarPoint.Win.ComplexBorder(	//ㅁ
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);


            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            int i = 0;

            strTitle = "응급전용 수술대장";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            SS1_Sheet1.Cells[7, 0, SS1.ActiveSheet.Rows.Count - 1, SS1.ActiveSheet.Columns.Count - 1].Text = "";
            SS1_Sheet1.RowCount = ssView1_Sheet1.RowCount + 8;            

            for (i = 0; i < ssView1_Sheet1.RowCount; i++)
            {
                //12 -> 14
                SS1_Sheet1.Cells[i + 7, 0].Text = (i + 1).ToString();
                SS1_Sheet1.Cells[i + 7, 1].Text = ssView1_Sheet1.Cells[i, 0].Text;
                SS1_Sheet1.Cells[i + 7, 2].Text = ssView1_Sheet1.Cells[i, 1].Text;
                SS1_Sheet1.Cells[i + 7, 3].Text = ssView1_Sheet1.Cells[i, 2].Text;
                SS1_Sheet1.Cells[i + 7, 4].Text = ssView1_Sheet1.Cells[i, 3].Text;//'응급실입실시간
                SS1_Sheet1.Cells[i + 7, 5].Text = ssView1_Sheet1.Cells[i, 4].Text;//'응급실입실시간
                SS1_Sheet1.Cells[i + 7, 6].Text = ssView1_Sheet1.Cells[i, 5].Text; // '응급실퇴실시간
                SS1_Sheet1.Cells[i + 7, 7].Text = ssView1_Sheet1.Cells[i, 6].Text; //'응급실퇴실시간
                SS1_Sheet1.Cells[i + 7, 8].Text = ssView1_Sheet1.Cells[i, 7].Text;
                SS1_Sheet1.Cells[i + 7, 9].Text = ssView1_Sheet1.Cells[i, 8].Text; //'수술시작
                SS1_Sheet1.Cells[i + 7, 10].Text = ssView1_Sheet1.Cells[i, 9].Text;//'수술시작
                SS1_Sheet1.Cells[i + 7, 11].Text = ssView1_Sheet1.Cells[i, 10].Text;//'수술종료
                SS1_Sheet1.AddSpanCell(i + 7, 12, 1, 2);
                SS1_Sheet1.Cells[i + 7, 12].Text = ssView1_Sheet1.Cells[i, 11].Text;//'수술종료
                SS1_Sheet1.Cells[i + 7, 14].Text = ssView1_Sheet1.Cells[i, 12].Text;////'의뢰과
                SS1_Sheet1.Cells[i + 7, 15].Text = ssView1_Sheet1.Cells[i, 13].Text;// //'수술후진단명
                SS1_Sheet1.Cells[i + 7, 16].Text = ssView1_Sheet1.Cells[i, 14].Text;////'KTAS
                SS1_Sheet1.Cells[i + 7, 17].Text = ssView1_Sheet1.Cells[i, 15].Text;
            }

            SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT + 1);

            //SS1_Sheet1.Cells[7, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = borderWhite;
            //SS1_Sheet1.Cells[7, 0, SS1_Sheet1.RowCount - 2, SS1_Sheet1.ColumnCount - 2].Border = border1;
            //SS1_Sheet1.Cells[7, SS1_Sheet1.ColumnCount - 1, SS1_Sheet1.RowCount - 2, SS1_Sheet1.ColumnCount - 1].Border = border2;
            //SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 2].Border = border3;
            //SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = border4;

            SS1_Sheet1.Cells[7, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = borderWhite;
            SS1_Sheet1.Cells[7, 0, 7, SS1_Sheet1.ColumnCount - 2].Border = border1;
            SS1_Sheet1.Cells[8, SS1_Sheet1.ColumnCount - 1, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = border2;
            SS1_Sheet1.Cells[8, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 2].Border = border3;
            SS1_Sheet1.Cells[7, SS1_Sheet1.ColumnCount - 1].Border = border4;

            //SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 2].Border = border3;
            //SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = border4;

            for (i = 1; i < SS1_Sheet1.RowCount; i++)
            { 
                if (i % 32 == 0)
                {
                    SS1_Sheet1.Cells[i, 0, i, SS1_Sheet1.ColumnCount - 2].Border = border1;
                    SS1_Sheet1.Cells[i, SS1_Sheet1.ColumnCount - 1].Border = border4;
                }
            }

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            Application.DoEvents();
            strFooter += CS.setSpdPrint_String(" ", new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);
            Application.DoEvents();
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            Application.DoEvents();
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, false, false, false, false, false, 0.9f);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            clsSpread CS = new clsSpread();
            CS.ExportToXLS(ssView1);
            CS = null;

            //if (ssView1_Sheet1.RowCount > 0)
            //{
            //    try
            //    {
            //        string strExcel = "";

            //        strExcel = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            //        strExcel = "C:\\CMC\\" + strExcel + ".xls";

            //        System.IO.FileStream S = new System.IO.FileStream(strExcel, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);

            //        ssView1.SaveExcel(S, FarPoint.Excel.ExcelSaveFlags.NoFlagsSet);

            //        S.Dispose();
            //        S = null;

            //        ComFunc.MsgBox(strExcel + "파일생성 완료되었습니다.", "작업확인");

            //        Process.Start(strExcel);
            //    }
            //    catch (Exception ex)
            //    {
            //        Cursor.Current = Cursors.Default;
            //        ComFunc.MsgBox("엑셀 파일 생성 중 오류가 발생하였습니다." + ComNum.VBLF + ComNum.VBLF + ex.Message, "오류");
            //    }
            //}
            //else
            //{
            //    ComFunc.MsgBox("조회된 정보가 없습니다. 확인 후 다시 시도 하세요.");
            //}
        }
    }
}
