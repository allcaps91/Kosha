using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using System.Drawing;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedErNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-03-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\nurse\nrer\FrmNIHSS_ER.frm" >> frmNIHSS_ER.cs 폼이름 재정의" />

    public partial class frmNIHSS_ER : Form
    {
        string FstrBDATE = "";
        string FstrPano = "";
        string FstrROWID = "";

        string SQL = "";
        DataTable dt = null;
        string SqlErr = "";
        //string strBDATE = "";
        //string strPano = "";
        string[] str = new string[3];
        ComFunc CF = new ComFunc();

        public frmNIHSS_ER()
        {
            InitializeComponent();
        }

        private void frmNIHSS_ER_Load(object sender, EventArgs e)
        {
            int i = 0;
            

            str = VB.Split(clsPublic.GstrHelpCode, "|");

            for (i = 1; i <= 62; i++)
            {
                ss1_Sheet1.Cells[i, 3].Text = "";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT AGE, SEX, ROWID";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_PATIENT ";
                SQL += ComNum.VBLF + " WHERE JDATE = TO_DATE('" + str[1] + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "     AND PANO  = '" + str[0] + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                FstrPano = str[0];
                FstrBDATE = str[1];

                ss2_Sheet1.Rows.Count = dt.Rows.Count;
                ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ss2_Sheet1.Cells[0, 1].Text = FstrPano;
                    ss2_Sheet1.Cells[0, 3].Text = dt.Rows[0]["SEX"].ToString().Trim() + " / " + dt.Rows[0]["Age"].ToString().Trim();
                    ss2_Sheet1.Cells[0, 5].Text = CF.Read_Patient(clsDB.DbCon, FstrPano, "2");
                }

                dt.Dispose();
                dt = null;
                //Search();
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

            fn_Read_Data();
        }

        void fn_Read_Data()
        {
            int nScore = 0;

            try
            {
                SQL = "";
                SQL += " SELECT BDATE, PANO                                                 \r";
                SQL += "      , SCORE01, SCORE02, SCORE03, SCORE04                          \r";
                SQL += "      , SCORE05, SCORE06, SCORE07, SCORE08                          \r";
                SQL += "      , SCORE09, SCORE10, SCORE11, SCORE12                          \r";
                SQL += "      , SCORE13, SCORE14, SCORE15, TOTAL, ROWID                     \r";
                SQL += "      , AMBUTATION07, AMBUTATION08, AMBUTATION09, AMBUTATION10      \r";
                SQL += "   FROM KOSMOS_PMPA.NUR_ER_PATIENT_NIHSS                            \r";
                SQL += "  WHERE BDATE = TO_DATE('" + FstrBDATE + "','YYYY-MM-DD')           \r";
                SQL += "    AND PANO = '" + FstrPano + "'                                   \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nScore = int.Parse(dt.Rows[0]["SCORE01"].ToString());
                    ss1.ActiveSheet.Cells[nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE02"].ToString());
                    ss1.ActiveSheet.Cells[4 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE03"].ToString());
                    ss1.ActiveSheet.Cells[7 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE04"].ToString());
                    ss1.ActiveSheet.Cells[10 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE05"].ToString());
                    ss1.ActiveSheet.Cells[13 + nScore, 3].Text = nScore.ToString();
                    
                    nScore = int.Parse(dt.Rows[0]["SCORE06"].ToString());
                    ss1.ActiveSheet.Cells[17 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE07"].ToString());
                    ss1.ActiveSheet.Cells[21 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE08"].ToString());
                    ss1.ActiveSheet.Cells[27 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE09"].ToString());
                    ss1.ActiveSheet.Cells[33 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE10"].ToString());
                    ss1.ActiveSheet.Cells[39 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE11"].ToString());
                    ss1.ActiveSheet.Cells[45 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE12"].ToString());
                    ss1.ActiveSheet.Cells[48 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE13"].ToString());
                    ss1.ActiveSheet.Cells[51 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE14"].ToString());
                    ss1.ActiveSheet.Cells[55 + nScore, 3].Text = nScore.ToString();

                    nScore = int.Parse(dt.Rows[0]["SCORE15"].ToString());
                    ss1.ActiveSheet.Cells[58 + nScore, 3].Text = nScore.ToString();


                    nScore = int.Parse(dt.Rows[0]["TOTAL"].ToString());
                    ss1.ActiveSheet.Cells[61, 3].Text = nScore.ToString();

                    ss1.ActiveSheet.Cells[26, 3].Text = dt.Rows[0]["AMBUTATION07"].ToString();
                    ss1.ActiveSheet.Cells[32, 3].Text = dt.Rows[0]["AMBUTATION08"].ToString();
                    ss1.ActiveSheet.Cells[38, 3].Text = dt.Rows[0]["AMBUTATION09"].ToString();
                    ss1.ActiveSheet.Cells[44, 3].Text = dt.Rows[0]["AMBUTATION10"].ToString();
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void ss1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //int i = 0;
            int x = 0;
            int nTot = 0;
            string strOK = "";
            int P = 0;
            //string strACTDATE = "";
            //double nIpdNo = 0;

            if (e.Column != 1)
            {
                return;
            }

            nTot = 0;
            strOK = "";

            switch (e.Row)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    x = 1;
                    break;
                case 4:
                case 5:
                case 6:
                    x = 2;
                    break;
                case 7:
                case 8:
                case 9:
                    x = 3;
                    break;

                case 10:
                case 11:
                case 12:
                    x = 4;
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                    x = 5;
                    break;
                case 17:
                case 18:
                case 19:
                case 20:
                    x = 6;
                    break;
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    x = 7;
                    break;
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                    x = 8;
                    break;

                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                    x = 9;
                    break;
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                    x = 10;
                    break;
                case 45:
                case 46:
                case 47:
                    x = 11;
                    break;
                case 48:
                case 49:
                case 50:
                    x = 12;
                    break;
                case 51:
                case 52:
                case 53:
                case 54:
                    x = 13;
                    break;

                case 55:
                case 56:
                case 57:
                    x = 14;
                    break;
                case 58:
                case 59:
                case 60:
                    x = 15;
                    break;
                case 26:
                    x = 17;
                    break;
                case 32:
                    x = 18;
                    break;
                case 38:
                    x = 19;
                    break;
                case 44:
                    x = 20;
                    break;
            }

            switch (e.Row)
            {
                case 26:
                case 32:
                case 38:
                case 44:
                    if (ss1_Sheet1.Cells[e.Row, 3].Text == "" || ss1_Sheet1.Cells[e.Row, 3].Text == "X")
                    {
                        ss1_Sheet1.Cells[e.Row, 3].Text = "O";
                    }
                    else
                    {
                        ss1_Sheet1.Cells[e.Row, 3].Text = "X";
                    }
                    strOK = ss1_Sheet1.Cells[e.Row, 3].Text;
                    break;
                default:
                    nTot = (int)VB.Val(VB.Left(ss1_Sheet1.Cells[e.Row, 2].Text, 1));
                    break;
            }

            if (nTot < 0)
            {
                return;
            }
            else
            {
                switch (x)
                {
                    case 1:
                        for (P = 0; P <= 3; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 2:
                        for (P = 4; P <= 6; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 3:
                        for (P = 7; P <= 9; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 4:
                        for (P = 10; P <= 12; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 5:
                        for (P = 13; P <= 16; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 6:
                        for (P = 17; P <= 20; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 7:
                        for (P = 21; P <= 25; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 8:
                        for (P = 27; P <= 31; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 9:
                        for (P = 33; P <= 37; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 10:
                        for (P = 39; P <= 43; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 11:
                        for (P = 45; P <= 47; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 12:
                        for (P = 48; P <= 50; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 13:
                        for (P = 51; P <= 54; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 14:
                        for (P = 55; P <= 57; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                    case 15:
                        for (P = 58; P <= 60; P++)
                        {
                            ss1_Sheet1.Cells[P, 3].Text = "";
                        }
                        break;
                }
            }

            switch (e.Row)
            {
                case 26:
                case 32:
                case 38:
                case 44:
                    break;
                default:
                    ss1_Sheet1.Cells[e.Row, 3].Text = nTot.ToString();
                    break;
            }
            Tot_Sub();
        }

        private void ss1_EditModeOff(object sender, EventArgs e)
        {
            Tot_Sub();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        ////private void Search()
        ////{
        ////    int i = 0;
        ////    string SQL = "";
        ////    DataTable dt = null;
        ////    string SqlErr = "";
        ////    int nScore = 0;
        ////    int nREAD = 0;

        ////    Cursor.Current = Cursors.WaitCursor;

        ////    try
        ////    {
        ////        SQL = "";
        ////        SQL = " SELECT BDATE, PANO, ";
        ////        SQL += ComNum.VBLF + " SCORE01, SCORE02, SCORE03, SCORE04, ";
        ////        SQL += ComNum.VBLF + " SCORE05, SCORE06, SCORE07, SCORE08, ";
        ////        SQL += ComNum.VBLF + " SCORE09, SCORE10, SCORE11, SCORE12, ";
        ////        SQL += ComNum.VBLF + " SCORE13, SCORE14, SCORE15, TOTAL, ROWID, ";
        ////        SQL += ComNum.VBLF + " AMBUTATION07, AMBUTATION08, AMBUTATION09, AMBUTATION10 ";
        ////        SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT_NIHSS ";
        ////        SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + FstrBDATE + "','YYYY-MM-DD')";
        ////        SQL += ComNum.VBLF + "     AND PANO = '" + FstrPano + "' ";

        ////        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

        ////        if (SqlErr != "")
        ////        {
        ////            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        ////            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        ////            return;
        ////        }
        ////        if (dt.Rows.Count > 0)
        ////        {
        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE01"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(1 + nScore) - 1, 3].Text = nScore.ToString();

        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE02"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(5 + nScore) - 1, 3].Text = nScore.ToString();

        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE03"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(8 + nScore) - 1, 3].Text = nScore.ToString();

        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE04"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(11 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE05"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(14 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE06"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(18 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE07"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(22 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE08"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(28 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE09"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(34 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE10"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(40 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE11"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(46 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE12"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(49 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE13"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(52 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE14"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(56 + nScore) - 1, 3].Text = nScore.ToString();


        ////            nScore = (int)VB.Val(dt.Rows[0]["SCORE15"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(59 + nScore) - 1, 3].Text = nScore.ToString();



        ////            nScore = (int)VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim());
        ////            ss1_Sheet1.Cells[(62 + nScore) - 1, 3].Text = nScore.ToString();

        ////            ss1_Sheet1.Cells[26, 3].Text = dt.Rows[0]["AMBUTATION07"].ToString().Trim();
        ////            ss1_Sheet1.Cells[32, 3].Text = dt.Rows[0]["AMBUTATION08"].ToString().Trim();
        ////            ss1_Sheet1.Cells[38, 3].Text = dt.Rows[0]["AMBUTATION09"].ToString().Trim();
        ////            ss1_Sheet1.Cells[44, 3].Text = dt.Rows[0]["AMBUTATION10"].ToString().Trim();
        ////        }

        ////        dt.Dispose();
        ////        dt = null;

        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        if (dt != null)
        ////        {
        ////            dt.Dispose();
        ////            dt = null;
        ////            Cursor.Current = Cursors.Default;
        ////        }

        ////        ComFunc.MsgBox(ex.Message);
        ////        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
        ////    }

        //}

        void Save()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            //bool rtnVal = false;
            //int i = 0;
            DataTable dt = null;

            string str01 = "";
            string str02 = "";
            string str03 = "";
            string str04 = "";
            string str05 = "";
            string str06 = "";
            string str07 = "";
            string str08 = "";
            string str09 = "";
            string str10 = "";
            string str11 = "";
            string str12 = "";
            string str13 = "";
            string str14 = "";
            string str15 = "";
            //string strMemo = "";
            string strTOTAL = "";
            string strAM07 = "";
            string strAM08 = "";
            string strAM09 = "";
            string strAM10 = "";
            string strROWID = "";
            int P = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " SELECT ROWID ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT_NIHSS ";
                SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + FstrBDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "      AND PANO = '" + FstrPano + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                for (P = 1; P <= 4; P++)
                {
                    str01 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str01 != "")
                    {
                        break;
                    }
                }
                for (P = 5; P <= 7; P++)
                {
                    str02 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str02 != "")
                    {
                        break;
                    }
                }

                for (P = 8; P <= 10; P++)
                {
                    str03 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str03 != "")
                    {
                        break;
                    }
                }

                for (P = 11; P <= 13; P++)
                {
                    str04 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str04 != "")
                    {
                        break;
                    }
                }

                for (P = 14; P <= 17; P++)
                {
                    str05 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str05 != "")
                    {
                        break;
                    }
                }

                for (P = 18; P <= 21; P++)
                {
                    str06 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str06 != "")
                    {
                        break;
                    }
                }

                for (P = 22; P <= 26; P++)
                {
                    str07 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str07 != "")
                    {
                        break;
                    }
                }

                strAM07 = ss1_Sheet1.Cells[26, 3].Text.Trim();

                for (P = 28; P <= 32; P++)
                {
                    str08 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str08 != "")
                    {
                        break;
                    }
                }

                //SS1.Row = 33
                strAM08 = ss1_Sheet1.Cells[32, 3].Text.Trim();

                for (P = 34; P <= 38; P++)
                {
                    str09 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str09 != "")
                    {
                        break;
                    }
                }

                // SS1.Row = 39
                strAM09 = ss1_Sheet1.Cells[38, 3].Text.Trim();

                for (P = 40; P <= 44; P++)
                {
                    str10 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str10 != "")
                    {
                        break;
                    }
                }

                //SS1.Row = 45
                strAM10 = ss1_Sheet1.Cells[44, 3].Text.Trim();

                for (P = 46; P <= 48; P++)
                {
                    str11 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str11 != "")
                    {
                        break;
                    }
                }

                for (P = 49; P <= 51; P++)
                {
                    str12 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str12 != "")
                    {
                        break;
                    }
                }

                for (P = 52; P <= 55; P++)
                {
                    str13 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str13 != "")
                    {
                        break;
                    }
                }

                for (P = 56; P <= 58; P++)
                {
                    str14 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str14 != "")
                    {
                        break;
                    }
                }

                for (P = 59; P <= 61; P++)
                {
                    str15 = ss1_Sheet1.Cells[P - 1, 3].Text.Trim();
                    if (str15 != "")
                    {
                        break;
                    }
                }

                //SS1.Row = 62
                strTOTAL = ss1_Sheet1.Cells[61, 3].Text.Trim();

                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_ER_PATIENT SET ";
                SQL += ComNum.VBLF + " NIHSS = '" + strTOTAL + "' ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (strROWID != "")
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_ER_PATIENT_NIHSS SET";
                    SQL += ComNum.VBLF + " SCORE01 = '" + str01 + "', ";
                    SQL += ComNum.VBLF + " SCORE02 = '" + str02 + "', ";
                    SQL += ComNum.VBLF + " SCORE03 = '" + str03 + "', ";
                    SQL += ComNum.VBLF + " SCORE04 = '" + str04 + "', ";
                    SQL += ComNum.VBLF + " SCORE05 = '" + str05 + "', ";
                    SQL += ComNum.VBLF + " SCORE06 = '" + str06 + "', ";
                    SQL += ComNum.VBLF + " SCORE07 = '" + str07 + "', ";
                    SQL += ComNum.VBLF + " SCORE08 = '" + str08 + "', ";
                    SQL += ComNum.VBLF + " SCORE09 = '" + str09 + "', ";
                    SQL += ComNum.VBLF + " SCORE10 = '" + str10 + "', ";
                    SQL += ComNum.VBLF + " SCORE11 = '" + str11 + "', ";
                    SQL += ComNum.VBLF + " SCORE12 = '" + str12 + "', ";
                    SQL += ComNum.VBLF + " SCORE13 = '" + str13 + "', ";
                    SQL += ComNum.VBLF + " SCORE14 = '" + str14 + "', ";
                    SQL += ComNum.VBLF + " SCORE15 = '" + str15 + "', ";
                    SQL += ComNum.VBLF + " TOTAL = '" + strTOTAL + "', ";
                    SQL += ComNum.VBLF + " AMBUTATION07 = '" + strAM07 + "', ";
                    SQL += ComNum.VBLF + " AMBUTATION08 = '" + strAM08 + "', ";
                    SQL += ComNum.VBLF + " AMBUTATION09 = '" + strAM09 + "', ";
                    SQL += ComNum.VBLF + " AMBUTATION10 = '" + strAM10 + "' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                }
                else
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_ER_PATIENT_NIHSS(";
                    SQL += ComNum.VBLF + " BDATE, PANO, WRITEDATE, WRITESABUN, ";
                    SQL += ComNum.VBLF + " SCORE01, SCORE02, SCORE03, SCORE04,";
                    SQL += ComNum.VBLF + " SCORE05, SCORE06, SCORE07, SCORE08,";
                    SQL += ComNum.VBLF + " SCORE09, SCORE10, SCORE11, SCORE12,";
                    SQL += ComNum.VBLF + " SCORE13, SCORE14, SCORE15, TOTAL, ";
                    SQL += ComNum.VBLF + " AMBUTATION07, AMBUTATION08, AMBUTATION09, AMBUTATION10) VALUES (";
                    SQL += ComNum.VBLF + "TO_DATE('" + FstrBDATE + "','YYYY-MM-DD'),'" + FstrPano + "', SYSDATE, " + clsType.User.Sabun + ", ";
                    SQL += ComNum.VBLF + "'" + str01 + "','" + str02 + "','" + str03 + "','" + str04 + "', ";
                    SQL += ComNum.VBLF + "'" + str05 + "','" + str06 + "','" + str07 + "','" + str08 + "', ";
                    SQL += ComNum.VBLF + "'" + str09 + "','" + str10 + "','" + str11 + "','" + str12 + "', ";
                    SQL += ComNum.VBLF + "'" + str13 + "','" + str14 + "','" + str15 + "','" + strTOTAL + "', ";
                    SQL += ComNum.VBLF + "'" + strAM07 + "','" + strAM08 + "','" + strAM09 + "','" + strAM10 + "') ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                //rtnVal = true;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void Tot_Sub()
        {
            int k = 0;
            double nTot = 0;

            //분류결과
            for (k = 0; k < 60; k++)
            {
                switch (k)
                {
                    case 26:
                    case 32:
                    case 38:
                    case 44:
                        break;
                    default:
                        nTot += VB.Val(ss1_Sheet1.Cells[k, 3].Text);
                        break;
                }
            }

            ss1_Sheet1.Cells[61, 3].Text = nTot.ToString();
        }
    }
}
