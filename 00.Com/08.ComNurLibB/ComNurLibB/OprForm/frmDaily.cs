using ComBase;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-01-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\Ocs\oproom\opsche\OPSCHE03.FRM >> frmDaily.cs 폼이름 재정의" />

    public partial class frmDaily : Form
    {
        FarPoint.Win.Spread.FpSpread ssSpread = null;
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        int LiRowCnt = 0;
        double nIPDNO = 0;
        string strPrtDept = "";
        string strPrtRoom = "";
        string strPrtPano = "";
        string strPrtName = "";
        string strTemp = "";
        string strPrtSA = "";
        string strPrtDr = "";
        string strPrtOp = "";
        string strPrtIll = "";
        string strWARD = "";
        //string strDrg = "";
        string strWard = "";
        string nRoomCode = "";
        string strOpRoom = "";
        string strOpSeq = "";
        //string strAge = "";

        public frmDaily() 
        {
            InitializeComponent();
        }

        #region READ_EXAM_INFECT_YN

        private string READ_EXAM_INFECT_YN(string ArgPano, string ArgGbn)
        {


            string rtnVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT EXNAME ,TO_CHAR(RDate,'YYYY-MM-DD') RDate,ROWID";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.EXAM_INFECT_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN ='" + ArgGbn + "' ";
                SQL = SQL + ComNum.VBLF + "    AND ODATE IS NULL";
                SQL = SQL + ComNum.VBLF + "  ORDER BY RDate DESC ,EXNAME ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "Y";
                    return rtnVal;
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return rtnVal;
        }

        #endregion

        #region EXAM_INFECTMASTER

        private string EXAM_INFECTMASTER(string ArgPano, string ArgExam)
        {
            string rtnVal = "";
            string strFDate = "";
            string strTdate = "";
            string strRDate = "";
            string strSpecCode = "";
            string SQL = "";
            DataTable dt = null;
            DataTable dtRs = null;
            string SqlErr = "";
            int i = 0;

            strFDate = Convert.ToDateTime(strDTP).AddDays(-42).ToString("yyyy-MM-dd");
            strTdate = strDTP;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ArgExam == "MRSA")
                {
                    SQL = "";
                    SQL = " SELECT A.PANO,A.SNAME,A.SPECCODE, B.NAME,                           ";
                    SQL = SQL + ComNum.VBLF + "MAX(TO_CHAR(A.RDATE,'YYYY-MM-DD')) RDATE                       ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION A,                             ";
                    SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_MED + "EXAM_SPECODE B                                ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')     ";
                    SQL = SQL + ComNum.VBLF + "   AND A.RDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD')     ";
                    SQL = SQL + ComNum.VBLF + "    AND A.PANO = '" + ArgPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SPECCODE = B.CODE                                     ";
                    SQL = SQL + ComNum.VBLF + "   AND B.GUBUN ='14'                                           ";
                    SQL = SQL + ComNum.VBLF + "   AND A.MRSA  = '*'                                        ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO,A.SNAME,A.SPECCODE,B.NAME                     ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PANO ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        rtnVal = "";
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strSpecCode = dt.Rows[i]["SPECCODE"].ToString().Trim();
                            strRDate = dt.Rows[i]["RDATE"].ToString().Trim();

                            SQL = "";
                            SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,VRE                   ";
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION                            ";
                            SQL = SQL + ComNum.VBLF + " WHERE RDATE > TO_DATE('" + strRDate + "','YYYY-MM-DD')   ";
                            SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD')   ";
                            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + ArgPano + "'                            ";
                            SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                    ";
                            SQL = SQL + ComNum.VBLF + "   AND (MRSA <> '*' OR MRSA IS NULL) ";
                            SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),VRE                 ";

                            SqlErr = clsDB.GetDataTableEx(ref dtRs, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                rtnVal = "";
                                return rtnVal;
                            }

                            if (dtRs.Rows.Count == 0)
                            {
                                rtnVal = "◎";
                            }
                            else if (dtRs.Rows.Count <= 2)
                            {
                                rtnVal = "◎";
                            }
                            else if (dtRs.Rows.Count >= 3)
                            {
                                rtnVal = "";
                            }

                            dtRs.Dispose();
                            dtRs = null;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }


                if (ArgExam == "VRE")
                {
                    SQL = " SELECT A.PANO,A.SNAME,A.SPECCODE, B.NAME,                           ";
                    SQL = SQL + ComNum.VBLF + "MAX(TO_CHAR(A.RDATE,'YYYY-MM-DD')) RDATE                       ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION A,                             ";
                    SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_MED + "EXAM_SPECODE B                                ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')     ";
                    SQL = SQL + ComNum.VBLF + "   AND A.RDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD')     ";
                    SQL = SQL + ComNum.VBLF + "    AND A.PANO = '" + ArgPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SPECCODE = B.CODE                                     ";
                    SQL = SQL + ComNum.VBLF + "   AND B.GUBUN ='14'                                           ";
                    SQL = SQL + ComNum.VBLF + "   AND A.VRE  = '*'                                        ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO,A.SNAME,A.SPECCODE,B.NAME                     ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PANO ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        rtnVal = "";
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strSpecCode = dt.Rows[i]["SPECCODE"].ToString().Trim();
                            strRDate = dt.Rows[i]["RDATE"].ToString().Trim();

                            SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,VRE                   ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_INFECTION                            ";
                            SQL = SQL + ComNum.VBLF + " WHERE RDATE > TO_DATE('" + strRDate + "','YYYY-MM-DD')   ";
                            SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD')   ";
                            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + ArgPano + "'                            ";
                            SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                    ";
                            SQL = SQL + ComNum.VBLF + "   AND (VRE <> '*' OR VRE IS NULL) ";
                            SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),VRE                 ";

                            SqlErr = clsDB.GetDataTableEx(ref dtRs, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                rtnVal = "";
                                return rtnVal;
                            }

                            if (dtRs.Rows.Count == 0)
                            {
                                rtnVal = "◎";
                            }
                            else if (dtRs.Rows.Count <= 2)
                            {
                                rtnVal = "◎";
                            }
                            else if (dtRs.Rows.Count >= 3)
                            {
                                rtnVal = "";
                            }
                            dtRs.Dispose();
                            dtRs = null;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }

                if (ArgExam == "PSEDUO")
                {

                    SQL = " SELECT A.PANO,A.SNAME,A.SPECCODE, B.NAME,                           ";
                    SQL = SQL + ComNum.VBLF + "MAX(TO_CHAR(A.RDATE,'YYYY-MM-DD')) RDATE                       ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_INFECTION A,                             ";
                    SQL = SQL + ComNum.VBLF + "      KOSMOS_OCS.EXAM_SPECODE B                                ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')     ";
                    SQL = SQL + ComNum.VBLF + "   AND A.RDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD')     ";
                    SQL = SQL + ComNum.VBLF + "    AND A.PANO = '" + ArgPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SPECCODE = B.CODE                                     ";
                    SQL = SQL + ComNum.VBLF + "   AND B.GUBUN ='14'                                           ";
                    SQL = SQL + ComNum.VBLF + "   AND A.RESULT IN ( 'ZZ171','ZZ171','ZZ171','ZZ175','ZZ181','ZZ180','ZZ190','ZZ191','ZZ195','ZZ203','ZZ203','ZZ203','ZZ203','ZZ203','ZZ203','ZZ206')     ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO,A.SNAME,A.SPECCODE,B.NAME                     ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PANO ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        rtnVal = "";
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strSpecCode = dt.Rows[i]["SPECCODE"].ToString().Trim();
                            strRDate = dt.Rows[i]["RDATE"].ToString().Trim();


                            SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,VRE                   ";
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION                            ";
                            SQL = SQL + ComNum.VBLF + " WHERE RDATE > TO_DATE('" + strRDate + "','YYYY-MM-DD')   ";
                            SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD')   ";
                            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + ArgPano + "'                            ";
                            SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                    ";
                            SQL = SQL + ComNum.VBLF + "   AND A.RESULT IN ( 'ZZ171','ZZ171','ZZ171','ZZ175','ZZ181','ZZ180','ZZ190','ZZ191','ZZ195','ZZ203','ZZ203','ZZ203','ZZ203','ZZ203','ZZ203','ZZ206')     ";
                            SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),VRE                 ";

                            SqlErr = clsDB.GetDataTableEx(ref dtRs, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                rtnVal = "";
                                return rtnVal;
                            }

                            if (dtRs.Rows.Count == 0)
                            {
                                rtnVal = "◎";
                            }
                            else if (dtRs.Rows.Count <= 2)
                            {
                                rtnVal = "◎";
                            }
                            else if (dtRs.Rows.Count >= 3)
                            {
                                rtnVal = "";
                            }
                            dtRs.Dispose();
                            dtRs = null;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }


            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            return rtnVal;
        }

        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            string strOpRoom = "";

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            Search();

            Delay(1000);

            //FarPoint.Win.ComplexBorder border1 = new FarPoint.Win.ComplexBorder(
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),  //LEFT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),  //TOP
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //BOM
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            //FarPoint.Win.ComplexBorder border2 = new FarPoint.Win.ComplexBorder(
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            //FarPoint.Win.ComplexBorder border3 = new FarPoint.Win.ComplexBorder(
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),  //LEFT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),  //TOP
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),  //BOM
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            //FarPoint.Win.ComplexBorder border4 = new FarPoint.Win.ComplexBorder(
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dotted, Color.Black),
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            Cursor.Current = Cursors.WaitCursor;



            if (rdoOptGbn0.Checked == true)
            {
                ssSpread = ssView0;
            }
            else
            {
                ssSpread = ssView1;
            }

            try
            {
                if (ComFunc.MsgBoxQ("검색된 자료를 출력하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                #region  GoSub SS_A4_Set

                SQL = "";
                SQL = " SELECT S.OpRoom ";
                SQL = SQL + ComNum.VBLF + " FROM   " + ComNum.DB_MED + "OCS_OPSCHE  S ";
                SQL = SQL + ComNum.VBLF + " WHERE  S.OpDate  = TO_DATE('" + DTP.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND  ( S.GbDel  <> '*' Or S.GbDel Is Null )       ";
                if (ComboDept.Text != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND S.DeptCode = '" + (ComboDept.Text).Trim() + "'";
                }

                SQL = SQL + ComNum.VBLF + " GROUP BY S.OpRoom ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //strMaxRow = ssSpread.ActiveSheet.RowCount + (dt.Rows.Count * 2);

                dt.Dispose();
                dt = null;

                strOpRoom = "";

                //if (rdoOptGbn0.Checked == true)
                //{
                for (i = 2; i < ssSpread.ActiveSheet.RowCount; i++)
                {
                    if (VB.Left(ssSpread.ActiveSheet.Cells[i, 2].Text.Trim(), 1) != VB.Left(strOpRoom.Trim(), 1) && strOpRoom.Trim() != "")
                    {
                        if (VB.Left(strOpRoom.Trim(), 1) == "4")
                        {
                            ssSpread.ActiveSheet.Rows.Add(i, 4);
                            i += 4;

                        }
                        else
                        {
                            ssSpread.ActiveSheet.Rows.Add(i, 2);
                            i += 2;

                        }
                    }
                    strOpRoom = ssSpread.ActiveSheet.Cells[i, 2].Text;
                }
                //}
                //else
                //{
                //    for (i = 2; i < ssSpread.ActiveSheet.RowCount; i++)
                //    {
                //        if (VB.Left(ssSpread.ActiveSheet.Cells[i, 2].Text, 1) != VB.Left(strOpRoom, 1) && strOpRoom.Trim() != "")
                //        {
                //            ssSpread.ActiveSheet.Rows.Add(i, 1);
                //            i += 1;
                //        }
                //        strOpRoom = ssSpread.ActiveSheet.Cells[i - 1, 2].Text;
                //    }
                //    ssSpread.ActiveSheet.RowCount = ssSpread.ActiveSheet.RowCount + 1;

                //}

                #endregion

                btnPrint.Enabled = false;

                strTitle = "OPERATING  SCHEDULE";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("수술일자 : " + DTP.Value.ToString("yyyy-MM-dd") + "    과 : " + ComboDept.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                if (rdoOptGbn0.Checked == true)
                {
                    int j = 0;

                    SS0Print0_Sheet1.RowCount = 8;
                    SS0Print0_Sheet1.Cells[4, 2].Text = "수술일자 : " + DTP.Value.ToString("yyyy-MM-dd") + "    과 : " + ComboDept.Text;

                    for (i = 2; i < ssSpread.ActiveSheet.RowCount; i++)
                    {

                        for (j = 0; j < ssSpread.ActiveSheet.ColumnCount; j++)
                        {
                            SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, j].CellType = ssSpread.ActiveSheet.Cells[i, j].CellType;
                            SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, j].Value = ssSpread.ActiveSheet.Cells[i, j].Value;
                            SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, j].Text = ssSpread.ActiveSheet.Cells[i, j].Text;
                        }

                        if (i == (ssSpread.ActiveSheet.RowCount - 1))
                        {
                            //    SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, 0, SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 2].Border = border3;
                            //    SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 1].Border = border2;
                        }
                        else
                        {
                            //    SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, 0, SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 2].Border = border1;
                            //    SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 1].Border = border2;
                            SS0Print0_Sheet1.RowCount += 1;
                        }

                        //SS0Print0_Sheet1.AddSpanCell(ssSpread.ActiveSheet.RowCount - 1, 11, 1, 6);
                        //SS0Print1_Sheet1.SetRowHeight(i, Convert.ToInt32(ssSpread.ActiveSheet.GetPreferredRowHeight(i)) + 10);
                    }

                    SS0Print0_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    if (SS0Print0_Sheet1.RowCount == 38)
                    {
                        SS0Print0_Sheet1.RowCount = 38 + 33;
                    }
                    else if (SS0Print0_Sheet1.RowCount < 38)
                    {
                        SS0Print0_Sheet1.RowCount = 38;
                    }
                    else
                    {
                        SS0Print0_Sheet1.RowCount = (((SS0Print0_Sheet1.RowCount - 38) / 33 + 1) * 33) + 38;
                    }
                    SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 1].Text = "     ";
                }
                else
                {
                    int j = 0;

                    SS0Print1_Sheet1.RowCount = 8;
                    SS0Print1_Sheet1.Cells[4, 2].Text = "수술일자 : " + DTP.Value.ToString("yyyy-MM-dd") + "    과 : " + ComboDept.Text;

                    for (i = 2; i < ssSpread.ActiveSheet.RowCount; i++)
                    {

                        for (j = 0; j < ssSpread.ActiveSheet.ColumnCount; j++)
                        {
                            SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, j].Text = ssSpread.ActiveSheet.Cells[i, j].Text;
                        }

                        if (i == (ssSpread.ActiveSheet.RowCount - 1))
                        {
                            //    SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, 0, SS0Print1_Sheet1.RowCount - 1, SS0Print1_Sheet1.ColumnCount - 2].Border = border3;
                            //    SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, SS0Print1_Sheet1.ColumnCount - 1].Border = border4;
                        }
                        else
                        {
                            //SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, 0, SS0Print1_Sheet1.RowCount - 1, SS0Print1_Sheet1.ColumnCount - 2].Border = border1;
                            //SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, SS0Print1_Sheet1.ColumnCount - 1].Border = border2;
                            SS0Print1_Sheet1.RowCount += 1;
                        }

                    }

                    SS0Print1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }

                setMargin = new clsSpread.SpdPrint_Margin(10, 0, 0, 10, 0, 0);


                if (rdoOptGbn0.Checked == true)
                {
                    setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, true, false, false, false);
                    CS.setSpdPrint(SS0Print0, false, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);

                    for (i = 1; i < Convert.ToInt32(txtPritntCount.Text.Trim()); i++)
                    {
                        Delay(1000);
                        SS0Print0.PrintSheet(0);
                    }
                }
                else
                {
                    setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, true, false, false, false);
                    CS.setSpdPrint(SS0Print1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);

                    for (i = 1; i < Convert.ToInt32(txtPritntCount.Text.Trim()); i++)
                    {
                        Delay(1000);
                        SS0Print1.PrintSheet(0);
                    }
                }
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
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


        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Search();
        }

        private void Search()
        {
            //GoSub ssView_Display_1 = ss0
            //GoSub ssView_Display_1_R = ss1

            Cursor.Current = Cursors.WaitCursor;

            ssView0_Sheet1.RowCount = 2;
            ssView1_Sheet1.RowCount = 2;


            if (rdoOptGbn0.Checked == true)
            {
                ssView0.Visible = true;
                ssView1.Visible = false;
                ssSpread = ssView0;
            }
            else
            {
                ssView1.Visible = true;
                ssView0.Visible = false;
                ssSpread = ssView1;
            }
            ssSpread.Dock = DockStyle.Fill;

            //GoSub Opsche_Disp
            Opsche_Disp();

            btnPrint.Enabled = true;
        }

        private void Opsche_Disp()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL = " SELECT S.OpRoom, S.OpSeq,  S.OpTime,  S.Pano,        S.PreDiagnosis,";
            SQL = SQL + ComNum.VBLF + "        S.SName,    S.Sex,     S.Age,         S.RoomCode,    ";
            SQL = SQL + ComNum.VBLF + "        S.DeptCode, S.OpStaff, O.DrName OpDr, S.OpIll,      S.GBDRG,   ";
            SQL = SQL + ComNum.VBLF + "        S.Anesth,   S.Remark,  S.reference,   S.Position,    D.DrName AnDr, ";
            SQL = SQL + ComNum.VBLF + "        S.GbDay,    S.LeftRight , TO_CHAR(S.OPDATE,'YYYY-MM-DD') AS OPDATE,   S.GBIO, s.gber, S.GBANTI  ";
            SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_OPSCHE  S, ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_DOCTOR O, ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_DOCTOR D ";
            SQL = SQL + ComNum.VBLF + " WHERE  S.OpDate  = TO_DATE('" + DTP.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "     AND  ( S.GbDel  <> '*' Or S.GbDel Is Null )       ";
            SQL = SQL + ComNum.VBLF + "     AND  ( S.GbAngio IS NULL OR S.GbAngio='N') ";
            SQL = SQL + ComNum.VBLF + "     AND  S.OPROOM <>'*' ";
            SQL = SQL + ComNum.VBLF + "     AND  S.OPROOM <>'N' ";
            SQL = SQL + ComNum.VBLF + "     AND    S.OpStaff = O.DrCode(+)  ";
            SQL = SQL + ComNum.VBLF + "     AND    TRUNC(S.AnDrCode) = D.DOCCODE(+)  ";

            if (ComboDept.Text.Trim() != "전체")
            {
                SQL = SQL + ComNum.VBLF + " AND S.DeptCode = '" + (ComboDept.Text).Trim() + "'";
            }

            if (chkEnd.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "     AND (S.WRTNO IN (SELECT WRTNO     ";
                SQL = SQL + ComNum.VBLF + "                     FROM KOSMOS_PMPA.ORAN_MASTER     ";
                SQL = SQL + ComNum.VBLF + "                     WHERE OPDATE = TO_DATE('" + DTP.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')     ";
                SQL = SQL + ComNum.VBLF + "                         AND OPETIME IS NULL OR OPETIME = ':'      ) ";
                SQL = SQL + ComNum.VBLF + "         OR S.WRTNO IS NULL)";
            }

            SQL = SQL + ComNum.VBLF + " ORDER  BY S.OpRoom, S.OpSeq, S.OpTime ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                LiRowCnt = dt.Rows.Count;

                if (rdoOptGbn0.Checked == true)
                {
                    ssView_Display_1(dt);
                }
                else
                {
                    ssView_Display_1_R(dt);
                }
            }
        }

        private void ssView_Display_1(DataTable dt)
        {


            DataTable dtFn = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0;

            #region GoSub ssView_Display_1

            for (i = 0; i < dt.Rows.Count; i++)
            {

                nRoomCode = "";

                strOpRoom = dt.Rows[i]["OpRoom"].ToString().Trim();
                strOpSeq = dt.Rows[i]["OpSeq"].ToString().Trim();

                ssSpread.ActiveSheet.RowCount = ssSpread.ActiveSheet.RowCount + 1;

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 1].Text = (i + 1).ToString();
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 2].Text = VB.Space(2 - VB.Len(strOpRoom)) + strOpRoom + "-" + strOpSeq + VB.Space(5 - VB.Len(strOpSeq));

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                strPrtDept = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 3].Text;

                if (dt.Rows[i]["GBDRG"].ToString().Trim() == "Y")
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 4].Text = "●";
                }
                else
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 4].Text = "";
                }

                //2020-10-28 안정수 추가, 예방적항생제표기 추가
                if (dt.Rows[i]["GBANTI"].ToString().Trim() == "Y")
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 5].Text = "●";
                }
                else
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 5].Text = "";
                }

                if (dt.Rows[i]["GbDay"].ToString().Trim() == "Y")
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text = "DSC";
                    strPrtRoom = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text;
                }
                else
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    //strPrtRoom = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 5].Text;
                }

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 7].Text = dt.Rows[i]["PANO"].ToString().Trim();
                strPrtPano = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 7].Text;

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 8].Text = dt.Rows[i]["SName"].ToString().Trim();
                strPrtName = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 8].Text;

                nIPDNO = 0;
                strWard = "";

                SQL = "";
                SQL = " SELECT IPDNO,WARDCODE, ROOMCODE FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "    AND OUTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY INDATE DESC  ";

                SqlErr = clsDB.GetDataTableEx(ref dtFn, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dtFn.Rows.Count > 0)
                {
                    nIPDNO = Convert.ToDouble(dtFn.Rows[0]["IPDNO"].ToString().Trim());
                    strWard = dtFn.Rows[0]["WARDCODE"].ToString().Trim();
                    nRoomCode = dtFn.Rows[0]["ROOMCODE"].ToString().Trim();
                }

                dtFn.Dispose();
                dtFn = null;

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text = nRoomCode;

                if (nRoomCode == "0")
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text = "OPD";
                }

                strPrtRoom = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text;

                //If AdoGetString(Rs, "PANO", i) = "07807437" Then
                //   i = i
                //End If


                strTemp = clsOpMain.Read_Pano_SELECT_MST_OP(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["GBIO"].ToString().Trim(), dt.Rows[i]["OpStaff"].ToString().Trim(), dt.Rows[i]["OPDATE"].ToString().Trim(), nIPDNO);

                if ("OK" == VB.Left(strTemp, 2))
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 8].Text = ("(선)" + dt.Rows[i]["SName"].ToString().Trim());
                }

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 9].Text = dt.Rows[i]["Sex"].ToString().Trim() == "M" ? "남" : "여";
                strPrtSA = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 9].Text;

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 10].Text = dt.Rows[i]["Age"].ToString().Trim();
                strPrtSA = strPrtSA + "/" + ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 10].Text;
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 11].Text = dt.Rows[i]["OpTime"].ToString().Trim();
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 12].Text = dt.Rows[i]["OpDr"].ToString().Trim();
                strPrtDr = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 12].Text;
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 13].Text = dt.Rows[i]["AnDr"].ToString().Trim();

                cboAnesth.SelectedIndex = dt.Rows[i]["Anesth"].ToString().Trim() == "" ? -1 : Convert.ToInt32(dt.Rows[i]["Anesth"].ToString().Trim());
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 14].Text = VB.Left(cboAnesth.Text, 1);

                switch (cboAnesth.Text)
                {
                    case "General Mask":
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 14].Text = "Mask";
                        break;
                    case "General Iv":
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 14].Text = "Iv";
                        break;
                    case "General Tube":
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 14].Text = "Tube";
                        break;
                }

                cboPosition.SelectedIndex = dt.Rows[i]["Position"].ToString().Trim() == "" ? -1 : (Convert.ToInt32(dt.Rows[i]["Position"].ToString().Trim()));

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 15].Text = cboPosition.Text;

                switch (dt.Rows[i]["LEFTRIGHT"].ToString().Trim())
                {
                    case "0":
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text = "N/A";
                        break;
                    case "1":
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text = "(Rt)";
                        break;
                    case "2":
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text = "(Lt)";
                        break;
                    case "3":
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text = "(OD)";
                        break;
                    case "4":
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text = "(OS)";
                        break;
                    case "5":
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text = "Both";
                        break;
                }

                strPrtOp = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text;


                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 17].Text = clsOpMain.Chan_String(dt.Rows[i]["PreDiagnosis"].ToString().Trim(), VB.Chr(10).ToString(), VB.Chr(13).ToString(), " ");
                strPrtIll = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 17].Text;

                if (dt.Rows[i]["GBER"].ToString().Trim() == "*")
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 17].Text = "●[응급]" + ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 17].Text;
                    ssSpread.ActiveSheet.Rows.Get(ssSpread.ActiveSheet.RowCount - 1).ForeColor = Color.Red;
                }

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 18].Text = dt.Rows[i]["OpIll"].ToString().Trim();
                strPrtOp = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 18].Text + " " + strPrtOp;

                //'엔트값을 없애는 문장임 아래
                //'For SS = 1 To Len(Trim(strOplll))
                //'    If Mid(Trim(strOplll), SS, 1) = Chr(10) Or Mid(Trim(strOplll), SS, 1) = Chr(13) Then
                //'        strOplll = Left(Trim(strOplll), SS - 1) & " " & Right(Trim(strOplll), Len(Trim(strOplll)) - SS)
                //'        SS = SS - 1
                //'    End If
                //'Next SS

                #region 표시 하지 않아서 주석 해둠

                //SQL = " SELECT VDRL, HCV_IGG, HBS_AG  FROM KOSMOS_OCS.EXAM_INFECTMASTER ";
                //SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";

                //SqlErr = clsDB.GetDataTableEx(ref dtFn, SQL, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    return;
                //}
                //if (dtFn.Rows.Count > 0)
                //{
                //    if (dtFn.Rows[0]["VDRL"].ToString().Trim() != "" || dtFn.Rows[0]["HCV_IGG"].ToString().Trim() != "" || dtFn.Rows[0]["HBS_AG"].ToString().Trim() != "")
                //    {
                //        ssSpread.ActiveSheet.Rows[ssSpread.ActiveSheet.RowCount - 1].BackColor = Color.FromArgb(230, 220, 255);
                //        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 18].Text = dtFn.Rows[0]["VDRL"].ToString().Trim() != "" ? "◎" : "";
                //        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 19].Text = dtFn.Rows[0]["HCV_IGG"].ToString().Trim() != "" ? "◎" : "";
                //        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 20].Text = dtFn.Rows[0]["HBS_AG"].ToString().Trim() != "" ? "◎" : "";
                //    }
                //    dtFn.Dispose();
                //    dtFn = null;
                //}



                ////'MRSA
                //ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 21].Text = EXAM_INFECTMASTER(dt.Rows[i]["Pano"].ToString().Trim(), "MRSA");

                ////'VRE
                //ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 22].Text = EXAM_INFECTMASTER(dt.Rows[i]["Pano"].ToString().Trim(), "VRE");

                ////'pseduo
                //ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 23].Text = EXAM_INFECTMASTER(dt.Rows[i]["Pano"].ToString().Trim(), "PSEUDO");

                #endregion

                if (READ_EXAM_INFECT_YN(dt.Rows[i]["Pano"].ToString().Trim(), "01") == "Y") //'혈액
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 25].Value = global::ComLibB.Properties.Resources.I00100;
                }

                if (READ_EXAM_INFECT_YN(dt.Rows[i]["Pano"].ToString().Trim(), "02") == "Y") // '접촉
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 26].Value = global::ComLibB.Properties.Resources.I01000;
                }

                if (READ_EXAM_INFECT_YN(dt.Rows[i]["Pano"].ToString().Trim(), "03") == "Y") //'공기
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 27].Value = global::ComLibB.Properties.Resources.I10000;
                }

                if (READ_EXAM_INFECT_YN(dt.Rows[i]["Pano"].ToString().Trim(), "04") == "Y") //'비말
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 28].Value = global::ComLibB.Properties.Resources.I00010;
                }

                if (dt.Rows[i]["GBIO"].ToString().Trim() == "O")
                {
                    if (clsVbfunc.READ_낙상고위험체크_OPD(clsDB.DbCon, dt.Rows[i]["pano"].ToString().Trim(), DTP.Value.ToString("yyyy-MM-dd"), dt.Rows[i]["Age"].ToString().Trim()) != "")
                    {
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 29].Text = "◈";
                    }
                }
                else
                {
                    if (nIPDNO > 0)
                    {
                        if (clsVbfunc.READ_WARNING_FALL(clsDB.DbCon, dt.Rows[i]["pano"].ToString().Trim(), DTP.Value.ToString("yyyy-MM-dd"), nIPDNO, dt.Rows[i]["age"].ToString().Trim(), "") != "")
                        {
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 29].Text = "◈";
                        }
                    }
                }

                if (nIPDNO > 0)
                {
                    //TODO DB 펑션으로 바꿔야할 수도 있음
                    if (clsOpMain.READ_WARNING_BRADEN(clsDB.DbCon, dt.Rows[i]["pano"].ToString().Trim(), DTP.Value.ToString("yyyy-MM-dd"), nIPDNO.ToString(), dt.Rows[i]["age"].ToString().Trim(), strWARD) != "")
                    {
                        ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 30].Text = "◈";
                    }
                }

                //'2016-05-16
                if (clsOpMain.READ_ALLERGY_OP(clsDB.DbCon, dt.Rows[i]["pano"].ToString().Trim()) == "OK")
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 31].Text = "◈";
                }

                ssSpread.ActiveSheet.SetRowHeight(i, Convert.ToInt32(ssSpread.ActiveSheet.GetPreferredRowHeight(i)) + 10);
            }

            //ssSpread.ActiveSheet.RowCount = ssSpread.ActiveSheet.RowCount + 5;



            #endregion

        }

        private void ssView_Display_1_R(DataTable dt)
        {

            DataTable dtFn = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0;

            #region GoSub ssView_Display_1_R


            for (i = 0; i < dt.Rows.Count; i++)
            {
                strOpRoom = dt.Rows[i]["oproom"].ToString().Trim();
                strOpSeq = dt.Rows[i]["opseq"].ToString().Trim();

                ssSpread.ActiveSheet.Rows.Count = ssSpread.ActiveSheet.Rows.Count + 1;
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 1].Text = i + 1.ToString();
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 2].Text = VB.Space(2 - VB.Len(strOpRoom)) + strOpRoom + "-" + strOpSeq + VB.Space(5 - VB.Len(strOpSeq));
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 3].Text = dt.Rows[i]["deptcode"].ToString().Trim();
                strPrtDept = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 3].Text;

                if (dt.Rows[i]["GBDRG"].ToString().Trim() == "Y")
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 4].Text = "●";
                }
                else
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 4].Text = "";
                }

                //2020-10-28 안정수 추가, 예방적항생제표기 추가
                if (dt.Rows[i]["GBANTI"].ToString().Trim() == "Y")
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 5].Text = "●";
                }
                else
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 5].Text = "";
                }

                if (dt.Rows[i]["Gbday"].ToString().Trim() == "Y")
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 6].Text = "DSC";
                    strPrtRoom = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 6].Text;
                }
                else
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 6].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strPrtRoom = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 6].Text;
                }

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 7].Text = dt.Rows[i]["PANO"].ToString().Trim();
                strPrtPano = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 7].Text;

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 8].Text = dt.Rows[i]["SName"].ToString().Trim();
                strPrtName = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 8].Text;

                nIPDNO = 0;

                SQL = "";
                SQL = " SELECT IPDNO, ROOMCODE FROM KOSMOS_PMPA.IPD_NEW_MASTER WHERE PANO ='" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY INDATE DESC  ";

                SqlErr = clsDB.GetDataTableEx(ref dtFn, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dtFn.Rows.Count > 0)
                {
                    nIPDNO = Convert.ToDouble(dtFn.Rows[0]["IPDNO"].ToString().Trim());
                    nRoomCode = dtFn.Rows[0]["Roomcode"].ToString().Trim();

                    dtFn.Dispose();
                    dtFn = null;
                }

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text = nRoomCode;

                if (nRoomCode == "0")
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text = "OPD";
                }
                strPrtRoom = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text;

                strTemp = clsOpMain.Read_Pano_SELECT_MST_OP(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["GBIO"].ToString().Trim(), dt.Rows[i]["OpStaff"].ToString().Trim(), dt.Rows[i]["OPDATE"].ToString().Trim(), nIPDNO);

                if ("OK" == VB.Left(strTemp, 2))
                {
                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 8].Text = ("(선)" + dt.Rows[i]["SName"].ToString().Trim());
                }

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 9].Text = dt.Rows[i]["SEX"].ToString().Trim() == "M" ? "남" : "여";
                strPrtSA = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 9].Text;
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 10].Text = dt.Rows[i]["age"].ToString().Trim();
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 11].Text = dt.Rows[i]["remark"].ToString().Trim();
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 12].Text = dt.Rows[i]["reFerence"].ToString().Trim();

                ssSpread.ActiveSheet.SetRowHeight(i, Convert.ToInt32(ssSpread.ActiveSheet.GetPreferredRowHeight(i)) + 10);
            }

            #endregion
        }

        private void frmDaily_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboAnesth, "OP_마취방법", 3, true, "N");//'마취방법
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboPosition, "OP_Position", 3, true, "");

            //cboAnesth.Items.Clear();
            //cboAnesth.Items.Add("General");
            //cboAnesth.Items.Add("General Mask");
            //cboAnesth.Items.Add("General Iv");
            //cboAnesth.Items.Add("General Tube");
            //cboAnesth.Items.Add("Local");
            //cboAnesth.Items.Add("Regional");
            //cboAnesth.Items.Add("Regional Spinal");
            //cboAnesth.Items.Add("Regional Epidural");
            //cboAnesth.Items.Add("Regional Axillary");
            //cboAnesth.Items.Add("Regional Caudal");
            //cboAnesth.Items.Add("MAC");
            //cboAnesth.Items.Add("NB");

            //cboPosition.Items.Clear();
            //cboPosition.Items.Add("Supine");
            //cboPosition.Items.Add("Prone");
            //cboPosition.Items.Add("Lateral");
            //cboPosition.Items.Add("Lithotomy");
            //cboPosition.Items.Add("Jack Knife");



            //'조회범위 과별

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT DeptCode, DeptNameK FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE  DeptCode  NOT IN ('II','R6','HR','PT', 'TO' ) ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY  PrintRanking ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ComboDept.Items.Add("전체");
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ComboDept.Items.Add(dt.Rows[i]["deptcode"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT DeptCode FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE  Sabun = '" + clsType.User.Sabun + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 1)
                {
                    ComboDept.SelectedIndex = 0;
                    for (i = 1; i < ComboDept.Items.Count; i++)
                    {
                        ComboDept.SelectedIndex = i - 1;
                        if (ComboDept.Text == dt.Rows[0]["deptcode"].ToString().Trim())
                        {
                            break;
                        }
                        ComboDept.SelectedIndex = 0;
                    }
                    //TEST
                    //clsVbfunc.SetDrCodeCombo(clsDB.DbCon, ComboDept, dt.Rows[0]["deptcode"].ToString().Trim(), "**", 2, "");
                }
                else
                {
                    ComboDept.SelectedIndex = 0;
                }



                dt.Dispose();
                dt = null;

                if (clsPublic.GstrHelpCode == "의료기관평가")
                {
                    DTP.Value = Convert.ToDateTime(strDTP).AddDays(1);
                    DTP.Enabled = false;
                    clsPublic.GstrHelpCode = "";
                }
                else
                {
                    DTP.Value = Convert.ToDateTime(strDTP).AddDays(1);
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void rdoOptGbn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOptGbn0.Checked == true)
            {
                ssView0.Visible = true;
                ssView1.Visible = false;
                ssSpread = ssView0;
            }
            else
            {
                ssView1.Visible = true;
                ssView0.Visible = false;
                ssSpread = ssView1;
            }
            ssSpread.Dock = DockStyle.Fill;


        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strPano = "";
            string strIpdNo = "";
            string strWARD = "";
            string strInDate = "";

            if (rdoOptGbn0.Checked == true)
            {
                ssSpread = ssView0;
            }
            else
            {
                ssSpread = ssView1;
            }

            if (e.RowHeader == true || e.Row < 1)
            {
                return;
            }

            strPano = "";
            strIpdNo = "";
            strWARD = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (e.Column == 29 || e.Column == 30)   //'낙상 or 욕창
                {
                    if (ssSpread.ActiveSheet.Cells[e.Row, e.Column].Text != "◈")
                    {
                        return;
                    }

                    strPano = ssSpread.ActiveSheet.Cells[e.Row, 6].Text.Trim();

                    SQL = "";
                    SQL = " SELECT IPDNO,WARDCODE,TO_CHAR(INDATE,'YYYYMMDD') InDate ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY INDATE DESC  ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strIpdNo = dt.Rows[0]["IPDNO"].ToString().Trim();
                        strWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();
                        strInDate = dt.Rows[0]["InDate"].ToString().Trim();
                    }
                    else
                    {
                        dt.Dispose();
                        dt = null;

                        return;
                    }

                    dt.Dispose();
                    dt = null;

                    //if (File.Exists(@"C:\cmc\ocsexe\careplan.exe") == false)
                    //{
                    //    Ftpedt FtpedtX = new Ftpedt();
                    //    FtpedtX.FtpDownload("192.168.100.33", "pcnfs", "pcnfs1", @"C:\cmc\ocsexe\careplan.exe", "careplan.exe", "/pcnfs/ocsexe");
                    //    FtpedtX = null;
                    //    ComFunc.MsgBox("Care Plan 설치 중입니다. 버튼을 다시 클릭하십시오", "확인");
                    //}

                    //VB.Shell(@"C:\cmc\ocsexe\careplan.exe " + strPano + "|" + strInDate + "|" + strIpdNo + "|" + clsType.User.Sabun + " ");

                    using (Form frm = new frmCarePlan(strPano, strInDate, strIpdNo, clsType.User.IdNumber))
                    {
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog(this);
                    }
                }
                else if (e.Column == 31)
                {
                    if (ssSpread.ActiveSheet.Cells[e.Row, 31].Text.Trim() == "◈")
                    {
                        clsOpMain.READ_ALLERGY_POPUP(clsDB.DbCon, ssSpread.ActiveSheet.Cells[e.Row, 7].Text.Trim());
                    }
                }
                else
                {
                    frmViewInfect frm = new frmViewInfect(ssSpread.ActiveSheet.Cells[e.Row, 7].Text.Trim());
                    frm.TopMost = true;
                    frm.StartPosition = FormStartPosition.CenterScreen;

                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void DTP_ValueChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Search();
        }

        private void btnPrint1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            int i = 0;
            int k = 0;

            ssPrt_Sheet1.RowCount = 5;

            if (ssView0_Sheet1.RowCount == 0)
            {
                return;
            }

            ssPrt_Sheet1.SetColumnWidth(0, 150);
            ssPrt_Sheet1.SetColumnWidth(1, 100);
            ssPrt_Sheet1.SetColumnWidth(2, 150);
            ssPrt_Sheet1.SetColumnWidth(3, 150);
            ssPrt_Sheet1.SetColumnWidth(4, 170);
            ssPrt_Sheet1.SetColumnWidth(6, 150);
            ssPrt_Sheet1.SetColumnWidth(7, 150);

            ssPrt_Sheet1.RowCount = (ssView0_Sheet1.RowCount - 2) * 5;

            ssPrt_Sheet1.Cells.Get(0, 0, ssPrt_Sheet1.RowCount - 1, ssPrt_Sheet1.ColumnCount - 1).Font = new System.Drawing.Font("굴림", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

            i = 2;


            for (k = 0; k < ssPrt_Sheet1.RowCount; k = k + 5)
            {

                ssPrt_Sheet1.Cells.Get(k, 0, k, ssPrt_Sheet1.ColumnCount - 1).Font = new System.Drawing.Font("굴림", 22F, System.Drawing.FontStyle.Regular,
                    System.Drawing.GraphicsUnit.Point, ((byte)(129)));

                if (ssView0_Sheet1.Cells[i, 3].Text != "")
                {
                    if (ssView0_Sheet1.Cells[i, 1].Text == "1")
                    {
                        ssPrt_Sheet1.Cells[(k), 0].Text = "번호";
                        //ssPrt_Sheet1.Set

                        ssPrt_Sheet1.Cells[(k) + 1, 0].Text = ssView0_Sheet1.Cells[i, 1].Text;//번호

                        ssPrt_Sheet1.Cells[(k), 1].Text = "과";
                        ssPrt_Sheet1.Cells[(k) + 1, 1].Text = ssView0_Sheet1.Cells[i, 3].Text;//과

                        ssPrt_Sheet1.Cells[(k), 2].Text = "DRG";
                        ssPrt_Sheet1.Cells[(k) + 1, 2].Text = ssView0_Sheet1.Cells[i, 4].Text;//DRG

                        //2020-10-28 안정수 추가
                        ssPrt_Sheet1.Cells[(k), 3].Text = "예방적항생제";
                        ssPrt_Sheet1.Cells[(k) + 1, 3].Text = ssView0_Sheet1.Cells[i, 5].Text;//예방적항생제

                        ssPrt_Sheet1.Cells[(k), 4].Text = "병실";
                        ssPrt_Sheet1.Cells[(k) + 1, 4].Text = ssView0_Sheet1.Cells[i, 6].Text;//병실

                        ssPrt_Sheet1.Cells[(k), 5].Text = "등록번호";
                        ssPrt_Sheet1.Cells[(k) + 1, 5].Text = ssView0_Sheet1.Cells[i, 7].Text;//등록번호

                        ssPrt_Sheet1.Cells[(k), 6].Text = "성명";
                        ssPrt_Sheet1.Cells[(k) + 1, 6].Text = ssView0_Sheet1.Cells[i, 8].Text;//성명

                        ssPrt_Sheet1.Cells[(k), 7].Text = "집도의";
                        ssPrt_Sheet1.Cells[(k) + 1, 7].Text = ssView0_Sheet1.Cells[i, 12].Text;//집도의

                        ssPrt_Sheet1.Cells[(k) + 2, 0].Text = "진단명";
                        ssPrt_Sheet1.Cells[(k) + 2, 1].ColumnSpan = 6;
                        ssPrt_Sheet1.Cells[(k) + 2, 1].Text = ssView0_Sheet1.Cells[i, 17].Text;//진단명

                        ssPrt_Sheet1.Cells[(k) + 3, 0].Text = "수술명";
                        ssPrt_Sheet1.Cells[(k) + 3, 1].ColumnSpan = 6;
                        ssPrt_Sheet1.Cells[(k) + 3, 1].Text = ssView0_Sheet1.Cells[i, 18].Text;//수술명

                        ssPrt_Sheet1.Cells[(k) + 4, 0].ColumnSpan = 7;
                    }
                    else
                    {
                        ssPrt_Sheet1.Cells[(k), 0].Text = "번호";
                        ssPrt_Sheet1.Cells[(k) + 1, 0].Text = ssView0_Sheet1.Cells[i, 1].Text;//번호

                        ssPrt_Sheet1.Cells[(k), 1].Text = "과";
                        ssPrt_Sheet1.Cells[(k) + 1, 1].Text = ssView0_Sheet1.Cells[i, 3].Text;//과

                        ssPrt_Sheet1.Cells[(k), 2].Text = "DRG";
                        ssPrt_Sheet1.Cells[(k) + 1, 2].Text = ssView0_Sheet1.Cells[i, 4].Text;//DRG

                        //2020-10-28 안정수 추가
                        ssPrt_Sheet1.Cells[(k), 3].Text = "DRG";
                        ssPrt_Sheet1.Cells[(k) + 1, 3].Text = ssView0_Sheet1.Cells[i, 5].Text;//DRG

                        ssPrt_Sheet1.Cells[(k), 4].Text = "병실";
                        ssPrt_Sheet1.Cells[(k) + 1, 4].Text = ssView0_Sheet1.Cells[i, 6].Text;//병실

                        ssPrt_Sheet1.Cells[(k), 5].Text = "등록번호";
                        ssPrt_Sheet1.Cells[(k) + 1, 5].Text = ssView0_Sheet1.Cells[i, 7].Text;//등록번호

                        ssPrt_Sheet1.Cells[(k), 6].Text = "성명";
                        ssPrt_Sheet1.Cells[(k) + 1, 6].Text = ssView0_Sheet1.Cells[i, 8].Text;//성명

                        ssPrt_Sheet1.Cells[(k), 7].Text = "집도의";
                        ssPrt_Sheet1.Cells[(k) + 1, 7].Text = ssView0_Sheet1.Cells[i, 12].Text;//집도의

                        ssPrt_Sheet1.Cells[(k) + 2, 0].Text = "진단명";
                        ssPrt_Sheet1.Cells[(k) + 2, 1].ColumnSpan = 6;
                        ssPrt_Sheet1.Cells[(k) + 2, 1].Text = ssView0_Sheet1.Cells[i, 17].Text;//진단명

                        ssPrt_Sheet1.Cells[(k) + 3, 0].Text = "수술명";
                        ssPrt_Sheet1.Cells[(k) + 3, 1].ColumnSpan = 6;
                        ssPrt_Sheet1.Cells[(k) + 3, 1].Text = ssView0_Sheet1.Cells[i, 18].Text;//수술명

                        ssPrt_Sheet1.Cells[(k) + 4, 0].ColumnSpan = 7;
                    }
                }

                i = i + 1;

            }

            ssPrt_Sheet1.SetRowHeight(-1, 80); 


            setMargin = new clsSpread.SpdPrint_Margin(10, 0, 0, 10, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssPrt, true, setMargin, setOption, "", "", Centering.Horizontal);
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            SS0Print0_Sheet1.RowCount = 7;
            SS0Print0_Sheet1.RowCount = 40;

            SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 1].Text = "    ";


            SS0Print0_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            setMargin = new clsSpread.SpdPrint_Margin(10, 0, 0, 10, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(SS0Print0, true, setMargin, setOption, "", "", Centering.Horizontal);
        }

        private void txtPritntCount_TextChanged(object sender, EventArgs e)
        {
            if (VB.IsNumeric(txtPritntCount.Text.Trim()) == false)
            {
                ComFunc.MsgBox("숫자만 입력 가능 합니다.", "오류");
                txtPritntCount.Text = "1";
                return;
            }

            if (Convert.ToInt32(txtPritntCount.Text.Trim()) <= 0)
            {
                ComFunc.MsgBox("매수는 1 보다 작을 수 없습니다.", "오류");
                txtPritntCount.Text = "1";
                return;
            }
        }
    }
}
