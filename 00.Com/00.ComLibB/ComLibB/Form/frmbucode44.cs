using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmbucode44 : Form
    {
        public frmbucode44 ()
        {
            InitializeComponent ();
        }

        private void frmbucode44_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등
            ComFunc.ReadSysDate (clsDB.DbCon);
            ScreenDisplay ();
        }

        private string READDeptName (string ArgCode)
        {
            string strVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "SELECT DeptNameK FROM BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "WHERE DeptCode = '" + ArgCode + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows [0] ["DeptNameK"].ToString ().Trim ();
                }
                else strVal = "";

                dt.Dispose ();
                dt = null;
                return strVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }
        }

        private string READIllName (string ArgCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "SELECT IllNameK FROM BAS_ILLS ";
                SQL = SQL + ComNum.VBLF + "WHERE IllCode = '" + ArgCode + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows [0] ["IllNameK"].ToString ().Trim ();
                }
                else strVal = "";

                dt.Dispose ();
                dt = null;
                return strVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }

        }


        private bool ScreenDisplay ()
        {
            int i = 0;
            bool rtnVal = false;
            string SqlErr = "";
            string SQL = "";
            string [] arraySetting = new string [3];

            DataTable dt = null;
            ssView_Sheet1.RowCount = 0;

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return rtnVal;//권한 확인

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = SQL + ComNum.VBLF + "SELECT  GBN, DeptCode, ILLCODE, IllCode1,IllCode2,illCode3, ILLCODE4,ILLCODE5,ILLCODE6,ILLCODE7,ILLCODE8,ILLCODE9,ILLCODE10,";
                SQL = SQL + ComNum.VBLF + "ILLCODE11, ILLCODE12,illCode13, ILLCODE14,ILLCODE15,ILLCODE16,ILLCODE17,ILLCODE18,ILLCODE19,ILLCODE20,";
                SQL = SQL + ComNum.VBLF + "ILLCODE21, ILLCODE22,illCode23, ILLCODE24,ILLCODE25,ILLCODE26,ILLCODE27,ILLCODE28,ILLCODE29,ILLCODE30, ";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(EntDate,'YYYY-MM-DD') EntDate,ROWID,Bi ";
                SQL = SQL + ComNum.VBLF + "FROM BAS_MILL_AUTO  ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 10;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                arraySetting [0] = "";
                arraySetting [1] = "1.윗상병남김";
                arraySetting [2] = "2.특정상병남김";

                clsSpread.gSpreadComboDataSetEx1 (ssView , 0 , 1 , ssView_Sheet1.RowCount - 1 , 1 , arraySetting , false);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    switch (dt.Rows [i] ["Gbn"].ToString ().Trim ())
                    {
                        case "1":
                            clsSpread.gSpreadComboListFind (ssView , i , 1 , 20 , "1.윗상병남김");
                            break;
                        case "2":
                            clsSpread.gSpreadComboListFind (ssView , i , 1 , 20 , "2.특정상병남김");
                            break;
                        default:
                            ssView_Sheet1.Cells [i , 1].Text = "";
                            break;
                    }

                    ssView_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["illcode"].ToString ().Trim ();

                    if (dt.Rows [i] ["illcode"].ToString ().Trim () + "" != "") ssView_Sheet1.Cells [i , 3].Text = READIllName (dt.Rows [i] ["illcode"].ToString ().Trim () + "");

                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Bi"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 4 + 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();

                    if (dt.Rows [i] ["DeptCode"].ToString ().Trim () + "" != "")
                    {
                        if (dt.Rows [i] ["DeptCode"].ToString ().Trim () + "" == "**")
                        {
                            ssView_Sheet1.Cells[i, 5 + 1].Text = "전체과";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 5 + 1].Text = READDeptName(dt.Rows[i]["DeptCode"].ToString().Trim() + "");
                        }
                    }
                    ssView_Sheet1.Cells[i, 6 + 1].Text = dt.Rows[i]["ILLCODE1"].ToString().Trim() + "";

                    if (dt.Rows [i] ["ILLCODE1"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 7 + 1].Text = READIllName(dt.Rows[i]["ILLCODE1"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 8 + 1].Text = dt.Rows[i]["ILLCODE2"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE2"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 9 + 1].Text = READIllName(dt.Rows[i]["ILLCODE2"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 10 + 1].Text = dt.Rows[i]["ILLCODE3"].ToString().Trim() + "";
                    }
                    if (dt.Rows [i] ["ILLCODE3"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 11 + 1].Text = READIllName(dt.Rows[i]["ILLCODE3"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 12 + 1].Text = dt.Rows[i]["ILLCODE4"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE4"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 13 + 1].Text = READIllName(dt.Rows[i]["ILLCODE4"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 14 + 1].Text = dt.Rows[i]["ILLCODE5"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE5"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 15 + 1].Text = READIllName(dt.Rows[i]["ILLCODE5"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 16 + 1].Text = dt.Rows[i]["ILLCODE6"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE6"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 17 + 1].Text = READIllName(dt.Rows[i]["ILLCODE6"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 18 + 1].Text = dt.Rows[i]["ILLCODE7"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE7"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 19 + 1].Text = READIllName(dt.Rows[i]["ILLCODE7"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 20 + 1].Text = dt.Rows[i]["ILLCODE8"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE8"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 21 + 1].Text = READIllName(dt.Rows[i]["ILLCODE8"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 22 + 1].Text = dt.Rows[i]["ILLCODE9"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE9"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 23 + 1].Text = READIllName(dt.Rows[i]["ILLCODE9"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 24 + 1].Text = dt.Rows[i]["ILLCODE10"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE10"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 25 + 1].Text = READIllName(dt.Rows[i]["ILLCODE10"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 26 + 1].Text = dt.Rows[i]["ILLCODE11"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE11"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 27 + 1].Text = READIllName(dt.Rows[i]["ILLCODE11"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 28 + 1].Text = dt.Rows[i]["ILLCODE12"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE12"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 29 + 1].Text = READIllName(dt.Rows[i]["ILLCODE12"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 30 + 1].Text = dt.Rows[i]["ILLCODE13"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE13"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 31 + 1].Text = READIllName(dt.Rows[i]["ILLCODE13"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 32 + 1].Text = dt.Rows[i]["ILLCODE14"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE14"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 33 + 1].Text = READIllName(dt.Rows[i]["ILLCODE14"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 34 + 1].Text = dt.Rows[i]["ILLCODE15"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE15"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 35 + 1].Text = READIllName(dt.Rows[i]["ILLCODE15"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 36 + 1].Text = dt.Rows[i]["ILLCODE16"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE16"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 37 + 1].Text = READIllName(dt.Rows[i]["ILLCODE16"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 38 + 1].Text = dt.Rows[i]["ILLCODE17"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE17"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 39 + 1].Text = READIllName(dt.Rows[i]["ILLCODE17"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 40 + 1].Text = dt.Rows[i]["ILLCODE18"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE18"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 41 + 1].Text = READIllName(dt.Rows[i]["ILLCODE18"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 42 + 1].Text = dt.Rows[i]["ILLCODE19"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE19"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 43 + 1].Text = READIllName(dt.Rows[i]["ILLCODE19"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 44 + 1].Text = dt.Rows[i]["ILLCODE20"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE20"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 45 + 1].Text = READIllName(dt.Rows[i]["ILLCODE20"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 46 + 1].Text = dt.Rows[i]["ILLCODE21"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE21"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 47 + 1].Text = READIllName(dt.Rows[i]["ILLCODE21"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 48 + 1].Text = dt.Rows[i]["ILLCODE22"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE22"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 49 + 1].Text = READIllName(dt.Rows[i]["ILLCODE22"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 50 + 1].Text = dt.Rows[i]["ILLCODE23"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE23"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 51 + 1].Text = READIllName(dt.Rows[i]["ILLCODE23"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 52 + 1].Text = dt.Rows[i]["ILLCODE24"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE24"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 53 + 1].Text = READIllName(dt.Rows[i]["ILLCODE24"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 54 + 1].Text = dt.Rows[i]["ILLCODE25"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE25"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 55 + 1].Text = READIllName(dt.Rows[i]["ILLCODE25"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 56 + 1].Text = dt.Rows[i]["ILLCODE26"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE26"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 57 + 1].Text = READIllName(dt.Rows[i]["ILLCODE26"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 58 + 1].Text = dt.Rows[i]["ILLCODE27"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE27"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 59 + 1].Text = READIllName(dt.Rows[i]["ILLCODE27"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 60 + 1].Text = dt.Rows[i]["ILLCODE28"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE28"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 61 + 1].Text = READIllName(dt.Rows[i]["ILLCODE28"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 62 + 1].Text = dt.Rows[i]["ILLCODE29"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE29"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 63 + 1].Text = READIllName(dt.Rows[i]["ILLCODE29"].ToString().Trim() + "");
                        ssView_Sheet1.Cells[i, 64 + 1].Text = dt.Rows[i]["ILLCODE30"].ToString().Trim();
                    }
                    if (dt.Rows [i] ["ILLCODE30"].ToString ().Trim () + "" != "")
                    {
                        ssView_Sheet1.Cells[i, 65 + 1].Text = READIllName(dt.Rows[i]["ILLCODE30"].ToString().Trim() + "");
                    }
                    ssView_Sheet1.Cells[i, 66 + 1].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 67 + 1].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 68 + 1].Text = "";
                }

                dt.Dispose ();
                dt = null;

                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private bool DateSave ()
        {
            int i = 0;
            string SqlErr = ""; //에러문 받는 변수
            bool bolChk = false;
            string strChange = "";
            string strROWID = "";
            string strGbn = "";
            string strDeptCode = "";
            string strIllCode = "";
            string strIllCode1 = "";
            string strIllCode2 = "";
            string strIllCode3 = "";
            string strIllCode4 = "";
            string strIllCode5 = "";
            string strIllCode6 = "";
            string strIllCode7 = "";
            string strIllCode8 = "";
            string strIllCode9 = "";
            string strIllCode10 = "";
            string strIllCode11 = "";
            string strIllCode12 = "";
            string strIllCode13 = "";
            string strIllCode14 = "";
            string strIllCode15 = "";
            string strIllCode16 = "";
            string strIllCode17 = "";
            string strIllCode18 = "";
            string strIllCode19 = "";
            string strIllCode20 = "";
            string strIllCode21 = "";
            string strIllCode22 = "";
            string strIllCode23 = "";
            string strIllCode24 = "";
            string strIllCode25 = "";
            string strIllCode26 = "";
            string strIllCode27 = "";
            string strIllCode28 = "";
            string strIllCode29 = "";
            string strIllCode30 = "";
            string strBi = "";
            string SQL = "";
            bool rtVal = false;
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false) return rtVal; //rtnVal;

            clsDB.setBeginTran(clsDB.DbCon);
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    bolChk = Convert.ToBoolean (ssView_Sheet1.Cells [i , 0].Value);
                    strGbn = VB.Left (ssView_Sheet1.Cells [i , 1].Text.ToString ().Trim () , 1);
                    strIllCode = ssView_Sheet1.Cells [i , 2].Text.ToString ().Trim ();

                    strBi = ssView_Sheet1.Cells[i, 4].Text.ToString().Trim();
                    strDeptCode = ssView_Sheet1.Cells[i, 4 + 1].Text.ToString().Trim();
                    strIllCode1 = ssView_Sheet1.Cells[i, 6 + 1].Text.ToString().Trim();
                    strIllCode2 = ssView_Sheet1.Cells[i, 8 + 1].Text.ToString().Trim();
                    strIllCode3 = ssView_Sheet1.Cells[i, 10 + 1].Text.ToString().Trim();
                    strIllCode4 = ssView_Sheet1.Cells[i, 12 + 1].Text.ToString().Trim();
                    strIllCode5 = ssView_Sheet1.Cells[i, 14 + 1].Text.ToString().Trim();
                    strIllCode6 = ssView_Sheet1.Cells[i, 16 + 1].Text.ToString().Trim();
                    strIllCode7 = ssView_Sheet1.Cells[i, 18 + 1].Text.ToString().Trim();
                    strIllCode8 = ssView_Sheet1.Cells[i, 20 + 1].Text.ToString().Trim();
                    strIllCode9 = ssView_Sheet1.Cells[i, 22 + 1].Text.ToString().Trim();
                    strIllCode10 = ssView_Sheet1.Cells[i, 24 + 1].Text.ToString().Trim();
                    strIllCode11 = ssView_Sheet1.Cells[i, 26 + 1].Text.ToString().Trim();
                    strIllCode12 = ssView_Sheet1.Cells[i, 28 + 1].Text.ToString().Trim();

                    strIllCode13 = ssView_Sheet1.Cells[i, 30 + 1].Text.ToString().Trim();
                    strIllCode14 = ssView_Sheet1.Cells[i, 32 + 1].Text.ToString().Trim();
                    strIllCode15 = ssView_Sheet1.Cells[i, 34 + 1].Text.ToString().Trim();
                    strIllCode16 = ssView_Sheet1.Cells[i, 36 + 1].Text.ToString().Trim();
                    strIllCode17 = ssView_Sheet1.Cells[i, 38 + 1].Text.ToString().Trim();
                    strIllCode18 = ssView_Sheet1.Cells[i, 40 + 1].Text.ToString().Trim();
                    strIllCode19 = ssView_Sheet1.Cells[i, 42 + 1].Text.ToString().Trim();
                    strIllCode20 = ssView_Sheet1.Cells[i, 44 + 1].Text.ToString().Trim();

                    strIllCode21 = ssView_Sheet1.Cells[i, 46 + 1].Text.ToString().Trim();
                    strIllCode22 = ssView_Sheet1.Cells[i, 48 + 1].Text.ToString().Trim();
                    strIllCode23 = ssView_Sheet1.Cells[i, 50 + 1].Text.ToString().Trim();
                    strIllCode24 = ssView_Sheet1.Cells[i, 52 + 1].Text.ToString().Trim();
                    strIllCode25 = ssView_Sheet1.Cells[i, 54 + 1].Text.ToString().Trim();
                    strIllCode26 = ssView_Sheet1.Cells[i, 56 + 1].Text.ToString().Trim();
                    strIllCode27 = ssView_Sheet1.Cells[i, 58 + 1].Text.ToString().Trim();
                    strIllCode28 = ssView_Sheet1.Cells[i, 60 + 1].Text.ToString().Trim();
                    strIllCode29 = ssView_Sheet1.Cells[i, 62 + 1].Text.ToString().Trim();
                    strIllCode30 = ssView_Sheet1.Cells[i, 64 + 1].Text.ToString().Trim();
                    strROWID = ssView_Sheet1.Cells[i, 67 + 1].Text.ToString().Trim();
                    strChange = ssView_Sheet1.Cells[i, 68 + 1].Text.ToString().Trim();

                    SQL = "";
                    if (bolChk == true)
                    {
                        if (strROWID != "") SQL = SQL + ComNum.VBLF + "DELETE BAS_MILL_AUTO WHERE ROWID = '" + strROWID + "' ";
                    }
                    else if (strChange == "Y")
                    {
                        if (strROWID == "" && strGbn != "")
                        {
                            SQL = "INSERT INTO BAS_MILL_AUTO (GBN, ILLCODE , DeptCode,IllCode1,IllCode2,IllCode3, ILLCODE4, ILLCODE5, ILLCODE6, ILLCODE7,ILLCODE8,ILLCODE9,ILLCODE10,";
                            SQL = SQL + ComNum.VBLF + "                                             ILLCODE11,ILLCODE12,IllCode13, ILLCODE14, ILLCODE15, ILLCODE16, ILLCODE17,ILLCODE18,ILLCODE19,ILLCODE20,";
                            SQL = SQL + ComNum.VBLF + "                                             ILLCODE21,ILLCODE22,IllCode23, ILLCODE24, ILLCODE25, ILLCODE26, ILLCODE27,ILLCODE28,ILLCODE29,ILLCODE30,                ";
                            SQL = SQL + ComNum.VBLF + " EntDate, Bi ) ";
                            SQL = SQL + ComNum.VBLF + "VALUES ('" + strGbn + "', '" + strIllCode + "', '" + strDeptCode + "', ";
                            SQL = SQL + ComNum.VBLF + " '" + strIllCode1 + "','" + strIllCode2 + "','" + strIllCode3 + "', '" + strIllCode4 + "', '" + strIllCode5 + "','" + strIllCode6 + "','" + strIllCode7 + "','" + strIllCode8 + "','" + strIllCode9 + "','" + strIllCode10 + "', ";
                            SQL = SQL + ComNum.VBLF + "  '" + strIllCode11 + "', '" + strIllCode12 + "','" + strIllCode13 + "', '" + strIllCode14 + "', '" + strIllCode15 + "','" + strIllCode16 + "','" + strIllCode17 + "','" + strIllCode18 + "','" + strIllCode19 + "','" + strIllCode20 + "', ";
                            SQL = SQL + ComNum.VBLF + "  '" + strIllCode21 + "', '" + strIllCode22 + "','" + strIllCode23 + "', '" + strIllCode24 + "', '" + strIllCode25 + "','" + strIllCode26 + "','" + strIllCode27 + "','" + strIllCode28 + "','" + strIllCode29 + "','" + strIllCode30 + "', ";
                            SQL = SQL + ComNum.VBLF + "  SYSDATE, '" + strBi + "' ) ";
                        }
                        else if (strROWID != "")
                        {
                            SQL = "UPDATE BAS_MILL_AUTO SET ";
                            SQL = SQL + ComNum.VBLF + " GBN='" + strGbn + "',";   
                            SQL = SQL + ComNum.VBLF + " ILLCODE='" + strIllCode + "',";
                            SQL = SQL + ComNum.VBLF + " DeptCode='" + strDeptCode + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode1='" + strIllCode1 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode2='" + strIllCode2 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode3='" + strIllCode3 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode4='" + strIllCode4 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode5='" + strIllCode5 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode6='" + strIllCode6 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode7='" + strIllCode7 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode8='" + strIllCode8 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode9='" + strIllCode9 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode10='" + strIllCode10 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode11='" + strIllCode11 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode12='" + strIllCode12 + "',";


                            SQL = SQL + ComNum.VBLF + " IllCode13='" + strIllCode13 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode14='" + strIllCode14 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode15='" + strIllCode15 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode16='" + strIllCode16 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode17='" + strIllCode17 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode18='" + strIllCode18 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode19='" + strIllCode19 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode20='" + strIllCode20 + "',";


                            SQL = SQL + ComNum.VBLF + " IllCode21='" + strIllCode21 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode22='" + strIllCode22 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode23='" + strIllCode23 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode24='" + strIllCode24 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode25='" + strIllCode25 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode26='" + strIllCode26 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode27='" + strIllCode27 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode28='" + strIllCode28 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode29='" + strIllCode29 + "',";
                            SQL = SQL + ComNum.VBLF + " IllCode30='" + strIllCode30 + "',";

                            SQL = SQL + ComNum.VBLF + " ENTDATE = SYSDATE, ";
                            SQL = SQL + ComNum.VBLF + " Bi = '" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                        }
                    }
                    if(SQL != "")
                    {
                        SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.setRollbackTran (clsDB.DbCon);
                            ComFunc.MsgBox ("DB에 Update중 오류가 발생함.");
                            clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                }
                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return true;
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

        private void btnSave_Click (object sender , EventArgs e)
        {
            if (DateSave () == true)
            {
                ScreenDisplay ();
            }
        }

        private void Print ()
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            if (ComQuery.IsJobAuth(this , "P", clsDB.DbCon) == false) return;//권한 확인

            strFont1 = "/fn\"굴림체\" /fz\"15";
            strFont2 = "/fn\"굴림체\" /fz\"10";
            strHead1 = "/n" + "/l/f1" + VB.Space (16) + "청구상병 자동정리 내역(BAS_MILL_AUTO)" + "/n";
            strHead2 = "/l/f2" + "출력일자 : " + clsPublic.GstrSysDate + VB.Space (65) + "PAGE:" + "/p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.PrintShapes = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet (0);

        }

        private void btnRegist_Click (object sender , EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Print ();
            Cursor.Current = Cursors.Default;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string[] arraySetting = { "", "1.윗상병남김", "2.특정상병남김" };
            if (rdoDown.Checked)
            {
                ssView_Sheet1.Rows.Add(ssView_Sheet1.RowCount, (int)VB.Val(txtLine.Text));

                clsSpread.gSpreadComboDataSetEx1(ssView, ssView_Sheet1.RowCount - (int)VB.Val(txtLine.Text), 1, ssView_Sheet1.RowCount - 1, 1, arraySetting, false);
            }
            else if (rdoUp.Checked)
            {
                ssView_Sheet1.Rows.Add(0, (int)VB.Val(txtLine.Text));

                clsSpread.gSpreadComboDataSetEx1(ssView, 0, 1, (int)VB.Val(txtLine.Text), 1, arraySetting, false);
            }
        }

        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strData = "";

            if (e.Column > 0) ssView_Sheet1.Cells[e.Row, 68 + 1].Text = "Y";

            strData = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();
            ssView_Sheet1.Cells[e.Row, e.Column].Text = strData;

            switch (e.Column)
            {
                case 4+1:
                    ssView_Sheet1.Cells[e.Row, 5 + 1].Text = strData != "**" ? clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strData) : "전체과";
                    break;
                case 2:
                case 6+1:
                case 8 + 1:
                case 10+1:
                case 12+1:
                case 14+1:
                case 16+1:
                case 18+1:
                case 20+1:
                case 22+1:
                case 24+1:
                case 26+1:
                case 28+1:
                case 30+1:
                case 32+1:
                case 34+1:
                case 36+1:
                case 38+1:
                case 40+1:
                case 42+1:
                case 44+1:
                case 46+1:
                case 48+1:
                case 50+1:
                case 52+1:
                case 54+1:
                case 56+1:
                case 58+1:
                case 60+1:
                case 62+1:
                case 64 + 1:
                    ssView_Sheet1.Cells[e.Row, e.Column + 1].Text = clsVbfunc.READ_ILLName(clsDB.DbCon, strData);
                    break;
            }
        }

        private void ssView_ClipboardPasted(object sender, FarPoint.Win.Spread.ClipboardPastedEventArgs e)
        {
            if (e.CellRange.RowCount == 0) return;

            string strData = "";

            if (e.CellRange.Column > 0) ssView_Sheet1.Cells[e.CellRange.Row, 68 + 1].Text = "Y";

            strData = ssView_Sheet1.Cells[e.CellRange.Row, e.CellRange.Column].Text.Trim().ToUpper();
            ssView_Sheet1.Cells[e.CellRange.Row, e.CellRange.Column].Text = strData;

            switch (e.CellRange.Column)
            {
                case 4:
                    ssView_Sheet1.Cells[e.CellRange.Row, 5 + 1].Text = strData != "**" ? clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strData) : "전체과";
                    break;
                case 2:
                case 6:
                case 8:
                case 10:
                case 12:
                case 14:
                case 16:
                case 18:
                case 20:
                case 22:
                case 24:
                case 26:
                case 28:
                case 30:
                case 32:
                case 34:
                case 36:
                case 38:
                case 40:
                case 42:
                case 44:
                case 46:
                case 48:
                case 50:
                case 52:
                case 54:
                case 56:
                case 58:
                case 60:
                case 62:
                case 64:
                    ssView_Sheet1.Cells[e.CellRange.Row, e.CellRange.Column + 1].Text = clsVbfunc.READ_ILLName(clsDB.DbCon, strData);
                    break;
            }
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) return;

            if(e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }
        }
    }
}
