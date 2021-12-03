using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;
using ComLibB;
using System.IO;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-12-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\Ocs\oproom\opsche\OPSCHE03_Block.FRM >> frmDailyBlock.cs 폼이름 재정의" />

    public partial class frmDailyBlock : Form
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
        string strDrg = "";
        string strWard = "";

        public frmDailyBlock()
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

            //int ll = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            string strOpRoom = "";

            //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false){}

            FarPoint.Win.ComplexBorder border1 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border2 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border3 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border4 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            Cursor.Current = Cursors.WaitCursor;



            if (rdoOptGbn0.Checked == true)
            {
                ssSpread = ssView0;
                //ssSpread = SS0Print;
            }
            else
            {
                ssSpread = ssView1;
            }

            //ssSpread.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data);

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

                if (rdoOptGbn0.Checked == true)
                {
                    for (i = 2; i < ssSpread.ActiveSheet.RowCount; i++)
                    {
                        if (i < ssSpread.ActiveSheet.RowCount)
                        {
                            if (VB.Left(ssSpread.ActiveSheet.Cells[i - 1, 2].Text, 1) != VB.Left(strOpRoom, 1) && strOpRoom.Trim() != "")
                            {
                                ssSpread.ActiveSheet.Rows.Add(i, 1);
                                i += 1;
                            }
                            strOpRoom = ssSpread.ActiveSheet.Cells[i - 1, 2].Text;
                        }
                    }
                }
                else
                {

                    for (i = 2; i < ssSpread.ActiveSheet.RowCount; i++)
                    {
                        if (VB.Left(ssSpread.ActiveSheet.Cells[i - 1, 2].Text, 1) != VB.Left(strOpRoom, 1) && strOpRoom.Trim() != "")
                        {
                            ssSpread.ActiveSheet.Rows.Add(i, 1);
                            i += 1;
                        }
                        strOpRoom = ssSpread.ActiveSheet.Cells[i - 1, 2].Text;
                    }
                    ssSpread.ActiveSheet.RowCount = ssSpread.ActiveSheet.RowCount + 1;

                }

                #endregion

                btnPrint.Enabled = false;

                strTitle = "OPERATING  SCHEDULE";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("수술일자 : " + DTP.Value.ToString("yyyy-MM-dd") + "    과 : " + ComboDept.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                //strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                //strFooter += CS.setSpdPrint_String("출력일자:" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"),"A") + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);


                if (rdoOptGbn0.Checked == true)
                {

                    int j = 0;

                    SS0Print0_Sheet1.RowCount = 8;

                    for (i = 2; i < ssSpread.ActiveSheet.RowCount; i++)
                    {

                        for (j = 0; j < ssSpread.ActiveSheet.ColumnCount; j++)
                        {
                            SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, j].Text = ssSpread.ActiveSheet.Cells[i, j].Text;
                        }

                        if (i == (ssSpread.ActiveSheet.RowCount - 1))
                        {
                            SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, 0, SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 2].Border = border3;
                            SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 1].Border = border2;
                        }
                        else
                        {
                            SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, 0, SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 2].Border = border1;
                            SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 1].Border = border2;
                            SS0Print0_Sheet1.RowCount += 1;
                        }
                    }

                    //SS0Print0_Sheet1.RowCount = SS0Print0_Sheet1.RowCount + 1;
                    //SS0Print0_Sheet1.Cells[SS0Print0_Sheet1.RowCount - 1, 0, SS0Print0_Sheet1.RowCount - 1, SS0Print0_Sheet1.ColumnCount - 1].Border = border3;

                    //SS0Print0_Sheet1.Rows[6].BackColor = Color.FromArgb(224, 224, 224);

                    SS0Print0_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }
                else
                {
                    //ll = 3;

                    //for (i = 3; i <= ssSpread.ActiveSheet.RowCount; i++)
                    //{
                    //    if (ssSpread.ActiveSheet.Cells[i - 1, 2].Text.Trim() == "")
                    //    {
                    //        ssSpread.ActiveSheet.RemoveRows(i - 1, 1);
                    //        i--;
                    //    }
                    //}

                    int j = 0;

                    SS0Print1_Sheet1.RowCount = 8;

                    for (i = 2; i < ssSpread.ActiveSheet.RowCount; i++)
                    {

                        for (j = 0; j < ssSpread.ActiveSheet.ColumnCount; j++)
                        {
                            SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, j].Text = ssSpread.ActiveSheet.Cells[i, j].Text;
                        }

                        if (i == (ssSpread.ActiveSheet.RowCount - 1))
                        {
                            SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, 0, SS0Print1_Sheet1.RowCount - 1, SS0Print1_Sheet1.ColumnCount - 2].Border = border3;
                            SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, SS0Print1_Sheet1.ColumnCount - 1].Border = border4;
                        }
                        else
                        {
                            SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, 0, SS0Print1_Sheet1.RowCount - 1, SS0Print1_Sheet1.ColumnCount - 2].Border = border1;
                            SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, SS0Print1_Sheet1.ColumnCount - 1].Border = border2;
                            SS0Print1_Sheet1.RowCount += 1;
                        }
                    }

                    //SS0Print1_Sheet1.RowCount = SS0Print1_Sheet1.RowCount + 1;
                    //SS0Print1_Sheet1.Cells[SS0Print1_Sheet1.RowCount - 1, 0, SS0Print1_Sheet1.RowCount - 1, SS0Print1_Sheet1.ColumnCount - 1].Border = border3;

                    //SS0Print1_Sheet1.Rows[6].BackColor = Color.FromArgb(224, 224, 224);

                    SS0Print1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }
                setMargin = new clsSpread.SpdPrint_Margin(10, 0, 20, 0, 50, 0);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, false, false, false, false, false, 0.8f);

                if (rdoOptGbn0.Checked == true)
                {
                    CS.setSpdPrint(SS0Print0, PrePrint, setMargin, setOption, strHeader, strFooter);
                }
                else
                {
                    CS.setSpdPrint(SS0Print1, PrePrint, setMargin, setOption, strHeader, strFooter);
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

        public SheetView CopySheet(SheetView sheet)
        {
            FarPoint.Win.Spread.SheetView newSheet = null;
            if (sheet != null)
            {
                newSheet = (FarPoint.Win.Spread.SheetView)FarPoint.Win.Serializer.LoadObjectXml(sheet.GetType(), FarPoint.Win.Serializer.GetObjectXml(sheet, "CopySheet"), "CopySheet");
            }
            return newSheet;
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
            string strOpRoom = "";
            string strOpSeq = "";
            //string strAge = "";
            DataTable dt = null;
            DataTable dtFn = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0;

            try
            {


                SQL = "";
                SQL = " SELECT S.OpRoom, S.OpSeq,  S.OpTime,  S.Pano,        S.PreDiagnosis,";
                SQL = SQL + ComNum.VBLF + "        S.SName,    S.Sex,     S.Age,         S.RoomCode,    ";
                SQL = SQL + ComNum.VBLF + "        S.DeptCode, S.OpStaff, O.DrName OpDr, S.OpIll,      S.GBDRG,   ";
                SQL = SQL + ComNum.VBLF + "        S.Anesth,   S.Remark,  S.reference,   S.Position,    D.DrName AnDr, ";
                SQL = SQL + ComNum.VBLF + "        S.GbDay,    S.LeftRight , S.OPDATE,   S.GBIO, s.gber  ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_OPSCHE  S, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_DOCTOR O, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_DOCTOR D ";
                SQL = SQL + ComNum.VBLF + " WHERE  S.OpDate  = TO_DATE('" + DTP.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND  ( S.GbDel  <> '*' Or S.GbDel Is Null )       ";
                SQL = SQL + ComNum.VBLF + "   AND  S.OPROOM ='N' ";
                SQL = SQL + ComNum.VBLF + " AND    S.OpStaff = O.DrCode(+)  ";
                SQL = SQL + ComNum.VBLF + " AND    TRUNC(S.AnDrCode) = D.DOCCODE(+)  ";

                if (ComboDept.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND S.DeptCode = '" + (ComboDept.Text).Trim() + "'";
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
                        #region GoSub ssView_Display_1

                        ssPrt_Sheet1.RowCount = 0;


                        ssSpread.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strOpRoom = dt.Rows[i]["OpRoom"].ToString().Trim();
                            strOpSeq = dt.Rows[i]["OpSeq"].ToString().Trim();

                            ssSpread.ActiveSheet.RowCount = ssSpread.ActiveSheet.RowCount + 1;

                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 1].Text = i + 1.ToString();
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

                            if (dt.Rows[i]["GbDay"].ToString().Trim() == "Y")
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 5].Text = "DSC";
                                strPrtRoom = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 5].Text;
                            }
                            else
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 5].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                                strPrtRoom = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 5].Text;
                            }

                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            strPrtPano = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text;

                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 7].Text = dt.Rows[i]["SName"].ToString().Trim();
                            strPrtName = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 7].Text;

                            nIPDNO = 0;
                            strWard = "";

                            SQL = "";
                            SQL = " SELECT IPDNO,WARDCODE FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER WHERE PANO ='" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
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
                            }

                            strTemp = clsOpMain.Read_Pano_SELECT_MST_OP(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["GBIO"].ToString().Trim(), dt.Rows[i]["OpStaff"].ToString().Trim(), dt.Rows[i]["OPDATE"].ToString(), nIPDNO);

                            if ("OK" == VB.Left(strTemp, 2))
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 7].Text = ("(선)" + dt.Rows[i]["SName"].ToString().Trim());
                            }

                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 8].Text = dt.Rows[i]["Sex"].ToString().Trim() == "M" ? "남" : "여";
                            strPrtSA = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 8].Text;

                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 9].Text = dt.Rows[i]["Age"].ToString().Trim();
                            strPrtSA = strPrtSA + "/" + ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 9].Text;
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 10].Text = dt.Rows[i]["OpTime"].ToString().Trim();
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 11].Text = dt.Rows[i]["OpDr"].ToString().Trim();
                            strPrtDr = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 11].Text;
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 12].Text = dt.Rows[i]["AnDr"].ToString().Trim();

                            cboAnesth.SelectedIndex = dt.Rows[i]["Anesth"].ToString().Trim() == "" ? -1 : Convert.ToInt32(dt.Rows[i]["Anesth"].ToString().Trim());
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 13].Text = VB.Left(cboAnesth.Text, 1);

                            switch (cboAnesth.Text)
                            {
                                case "General Mask":
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 13].Text = "Mask";
                                    break;
                                case "General Iv":
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 13].Text = "Iv";
                                    break;
                                case "General Tube":
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 13].Text = "Tube";
                                    break;
                            }

                            cboPosition.SelectedIndex = dt.Rows[i]["Position"].ToString().Trim() == "" ? -1 : Convert.ToInt32(dt.Rows[i]["Position"].ToString().Trim());
                            cboPosition.SelectedIndex -= 1;
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 14].Text = cboPosition.Text;

                            switch (dt.Rows[i]["LEFTRIGHT"].ToString().Trim())
                            {
                                case "0":
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 15].Text = "N/A";
                                    break;
                                case "1":
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 15].Text = "(Rt)";
                                    break;
                                case "2":
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 15].Text = "(Lt)";
                                    break;
                                case "3":
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 15].Text = "(OD)";
                                    break;
                                case "4":
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 15].Text = "(OS)";
                                    break;
                                case "5":
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 15].Text = "Both";
                                    break;
                            }

                            strPrtOp = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 15].Text;


                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text = clsOpMain.Chan_String(dt.Rows[i]["PreDiagnosis"].ToString().Trim(), "10", "13", " ");
                            strPrtIll = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text;

                            if (dt.Rows[i]["GBER"].ToString().Trim() == "*")
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text = "●[응급]" + ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].Text;
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 16].ForeColor = Color.Red;
                            }

                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 17].Text = dt.Rows[i]["OpIll"].ToString().Trim();
                            strPrtOp = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 17].Text + " " + strPrtOp;

                            SQL = " SELECT VDRL, HCV_IGG, HBS_AG  FROM KOSMOS_OCS.EXAM_INFECTMASTER ";
                            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";

                            SqlErr = clsDB.GetDataTableEx(ref dtFn, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dtFn.Rows.Count > 0)
                            {
                                if (dtFn.Rows[0]["VDRL"].ToString().Trim() != "" || dtFn.Rows[0]["HCV_IGG"].ToString().Trim() != "" || dtFn.Rows[0]["HBS_AG"].ToString().Trim() != "")
                                {
                                    ssSpread.ActiveSheet.Rows[ssSpread.ActiveSheet.RowCount - 1].BackColor = Color.FromArgb(230, 220, 255);
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 18].Text = dtFn.Rows[0]["VDRL"].ToString().Trim() != "" ? "◎" : "";
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 19].Text = dtFn.Rows[0]["HCV_IGG"].ToString().Trim() != "" ? "◎" : "";
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 20].Text = dtFn.Rows[0]["HBS_AG"].ToString().Trim() != "" ? "◎" : "";
                                }
                                dtFn.Dispose();
                                dtFn = null;
                            }

                            //'MRSA
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 21].Text = EXAM_INFECTMASTER(dt.Rows[i]["Pano"].ToString().Trim(), "MRSA");

                            //'VRE
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 22].Text = EXAM_INFECTMASTER(dt.Rows[i]["Pano"].ToString().Trim(), "VRE");

                            //'pseduo
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 23].Text = EXAM_INFECTMASTER(dt.Rows[i]["Pano"].ToString().Trim(), "PSEUDO");

                            if (READ_EXAM_INFECT_YN(dt.Rows[i]["Pano"].ToString().Trim(), "01") == "Y") // '혈액
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 24].Value = global::ComLibB.Properties.Resources.I00100;
                            }

                            if (READ_EXAM_INFECT_YN(dt.Rows[i]["Pano"].ToString().Trim(), "02") == "Y") // '접촉
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 25].Value = global::ComLibB.Properties.Resources.I01000;
                            }

                            if (READ_EXAM_INFECT_YN(dt.Rows[i]["Pano"].ToString().Trim(), "03") == "Y") // '공기
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 26].Value = global::ComLibB.Properties.Resources.I10000;
                            }

                            if (READ_EXAM_INFECT_YN(dt.Rows[i]["Pano"].ToString().Trim(), "04") == "Y") // '비말
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 27].Value = global::ComLibB.Properties.Resources.I00010;
                            }

                            if (dt.Rows[i]["GBIO"].ToString().Trim() == "O")
                            {
                                if (clsVbfunc.READ_낙상고위험체크_OPD(clsDB.DbCon, dt.Rows[i]["pano"].ToString().Trim(), DTP.Value.ToString("yyyy-MM-dd"), dt.Rows[i]["Age"].ToString().Trim()) != "")
                                {
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 28].Text = "◈";
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
                                    ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 29].Text = "◈";
                                }
                            }


                            //'srtPrt 값 뿌리기
                            ssPrt_Sheet1.RowCount = ssPrt_Sheet1.RowCount + 5;

                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 0].Text = "번호";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 1].Text = "과";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 2].Text = "DRG";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 3].Text = "병실";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 4].Text = "등록번호";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 5].Text = "성명";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 6].Text = "성별/나이";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 7].Text = "집도의";
                            ssPrt_Sheet1.SetRowHeight((i * 5 + 1) - 1, 42);

                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 0].Text = i + 1.ToString();
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 1].Text = strPrtDept;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 2].Text = strDrg;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 3].Text = strPrtRoom;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 4].Text = strPrtPano;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 5].Text = strPrtName;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 6].Text = strPrtSA;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 7].Text = strPrtDr;
                            ssPrt_Sheet1.SetRowHeight((i * 5 + 2) - 1, 42);

                            ssPrt_Sheet1.Cells[(i * 5 + 3) - 1, 0].Text = "진단명";
                            ssPrt_Sheet1.AddSpanCell((i * 5 + 2), 1, 4, 1);
                            ssPrt_Sheet1.SetRowHeight((i * 5 + 3) - 1, 60);

                            ssPrt_Sheet1.Cells[(i * 5 + 4) - 1, 0].Text = "수술명";
                            ssPrt_Sheet1.AddSpanCell((i * 5 + 2), 1, 4, 1);
                            ssPrt_Sheet1.SetRowHeight((i * 5 + 4) - 1, 45);
                        }
                        ssSpread.ActiveSheet.RowCount = ssSpread.ActiveSheet.RowCount + 5;
                        #endregion

                    }
                    else
                    {
                        #region GoSub ssView_Display_1_R

                        ssPrt_Sheet1.RowCount = 0;

                        ssSpread.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
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

                            if (dt.Rows[i]["Gbday"].ToString().Trim() == "Y")
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 5].Text = "DSC";
                                strPrtRoom = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 5].Text;
                            }
                            else
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 5].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                                strPrtRoom = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 5].Text;
                            }

                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 6].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            strPrtPano = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 6].Text;

                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 7].Text = dt.Rows[i]["SName"].ToString().Trim();
                            strPrtName = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.Rows.Count - 1, 7].Text;

                            nIPDNO = 0;

                            SQL = "";
                            SQL = " SELECT IPDNO FROM KOSMOS_PMPA.IPD_NEW_MASTER WHERE PANO ='" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
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

                                dtFn.Dispose();
                                dtFn = null;
                            }

                            strTemp = clsOpMain.Read_Pano_SELECT_MST_OP(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["GBIO"].ToString().Trim(), dt.Rows[i]["OpStaff"].ToString().Trim(), dt.Rows[i]["OPDATE"].ToString().Trim(), nIPDNO);

                            if (VB.Left(strTemp, 2) == "OK")
                            {
                                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 7].Text = "(선)" + dt.Rows[i]["SName"].ToString().Trim();
                            }

                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 8].Text = dt.Rows[i]["SEX"].ToString().Trim() == "M" ? "남" : "여";
                            strPrtSA = ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 8].Text;
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 9].Text = dt.Rows[i]["age"].ToString().Trim();
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 10].Text = dt.Rows[i]["remark"].ToString().Trim();
                            ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 11].Text = dt.Rows[i]["reFerence"].ToString().Trim();

                            //'srtPrt 값 뿌리기

                            ssPrt_Sheet1.RowCount = ssPrt_Sheet1.RowCount + 5;

                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 0].Text = "번호";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 1].Text = "과";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 2].Text = "DRG";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 3].Text = "병실";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 4].Text = "등록번호";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 5].Text = "성명";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 6].Text = "성별/나이";
                            ssPrt_Sheet1.Cells[(i * 5 + 1) - 1, 7].Text = "집도의";

                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 0].Text = i + 1.ToString();
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 1].Text = strPrtDept;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 2].Text = strDrg;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 3].Text = strPrtRoom;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 4].Text = strPrtPano;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 5].Text = strPrtName;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 6].Text = strPrtSA;
                            ssPrt_Sheet1.Cells[(i * 5 + 2) - 1, 7].Text = strPrtDr;

                            ssPrt_Sheet1.Cells[(i * 5 + 3) - 1, 0].Text = "환자특이사항";
                            strPrtIll = ssPrt_Sheet1.Cells[(i * 5 + 3) - 1, 0].Text;

                            ssPrt_Sheet1.Cells[(i * 5 + 3), 0].Text = "수술준비사항";
                            ssPrt_Sheet1.AddSpanCell((i * 5 + 3), 1, 1, 6);

                            ssPrt_Sheet1.AddSpanCell((i * 5 + 4), 0, 1, 6);
                        }

                        #endregion
                    }
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

        private void frmDailyBlock_Load(object sender, EventArgs e)
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

            DTP.Value = Convert.ToDateTime(strDTP).AddDays(1);

            cboAnesth.Items.Clear();
            cboAnesth.Items.Add("General");
            cboAnesth.Items.Add("General Mask");
            cboAnesth.Items.Add("General Iv");
            cboAnesth.Items.Add("General Tube");
            cboAnesth.Items.Add("Local");
            cboAnesth.Items.Add("Regional");
            cboAnesth.Items.Add("Regional Spinal");
            cboAnesth.Items.Add("Regional Epidural");
            cboAnesth.Items.Add("Regional Axillary");
            cboAnesth.Items.Add("Regional Caudal");
            cboAnesth.Items.Add("MAC");
            cboAnesth.Items.Add("NB");

            cboPosition.Items.Clear();
            cboPosition.Items.Add("Supine");
            cboPosition.Items.Add("Prone");
            cboPosition.Items.Add("Lateral");
            cboPosition.Items.Add("Lithotomy");
            cboPosition.Items.Add("Jack Knife");

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
                    DTP.Value = Convert.ToDateTime(strDTP);
                    DTP.Enabled = false;
                    Search();
                    clsPublic.GstrHelpCode = "";
                }
                else
                {
                    Search();
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
                if (e.Column == 28 || e.Column == 29)   //'낙상 or 욕창
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
                else if (e.Column == 30)
                {
                    if (ssSpread.ActiveSheet.Cells[e.Row, 30].Text.Trim() == "◈")
                    {
                        clsOpMain.READ_ALLERGY_POPUP(clsDB.DbCon, ssSpread.ActiveSheet.Cells[e.Row, 6].Text.Trim());
                    }
                }
                else
                {
                    frmViewInfect frm = new frmViewInfect(ssSpread.ActiveSheet.Cells[e.Row, 6].Text.Trim());
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
    }
}
