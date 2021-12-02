using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComBase
{
    /// <summary>
    /// Class Name      : SupDiet
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\mtsEmr\CADEX\Frm영양평가지New" >> frmDietEvaluationNew.cs 폼이름 재정의" />

    public partial class frmDietEvaluationNew : Form
    {
        public delegate void EventClose();
        public event EventClose rEventClose;

        string GstrHelpName = "";
        string FstrGubun = "";
        string FstrROWID = "";
        string strEXEName = "";
        string GstrROWID = "";  //GstrHelpCode

        public frmDietEvaluationNew(string strEXENameX, string GstrROWIDX, string GstrHelpNameX)
        {
            strEXEName = strEXENameX;
            GstrROWID = GstrROWIDX;
            GstrHelpName = GstrHelpNameX;

            InitializeComponent();
        }

        public frmDietEvaluationNew()
        {
            InitializeComponent();
        }

        private void frmDietEvaluationNew_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (VB.UCase(strEXEName) != "DIETORDER")
            {
                btnSave.Enabled = false;
                btnPrint.Enabled = false;
            }

            switch (GstrHelpName)
            {
                case "RE":
                    FstrGubun = "2";
                    break;
                default:
                    FstrGubun = "1";
                    break;
            }

            FstrROWID = GstrROWID;

            READ_DATA(FstrROWID);

        }

        private bool Save_Data(string ArgROWID)
        {

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strDIETPROBLEM1 = "";
            string strDIETPROBLEM2 = "";
            string strDIETPROBLEM3 = "";
            string strDIETPROBLEM4 = "";
            string strCERTIDATE = "";
            string strCERTISABUN = "";
            string strMemo = "";
            string strHT = "";
            string strWT = "";
            string strHW = "";
            string strIBW = "";
            string strSEX = "";

            if (ChkHT.Checked == true)
            {
                strHT = (TxtHEIGHT.Text).Trim();
            }
            if (ChkWT.Checked == true)
            {
                strWT = (TxtWEIGHT.Text).Trim();
            }

            strDIETPROBLEM1 = Convert.ToBoolean(ss1_Sheet1.Cells[23, 4].Value) == true ? "1" : "0";
            strDIETPROBLEM2 = Convert.ToBoolean(ss1_Sheet1.Cells[23, 6].Value) == true ? "1" : "0";
            strDIETPROBLEM3 = Convert.ToBoolean(ss1_Sheet1.Cells[23, 7].Value) == true ? "1" : "0";
            strDIETPROBLEM4 = Convert.ToBoolean(ss1_Sheet1.Cells[23, 8].Value) == true ? "1" : "0";

            strMemo = ss1_Sheet1.Cells[29, 2].Text.Trim();

            strCERTIDATE = ss1_Sheet1.Cells[31, 3].Text.Trim();
            strCERTIDATE = strCERTIDATE + " " + ss1_Sheet1.Cells[31, 5].Text.Trim();

            strSEX = VB.Left(ss1_Sheet1.Cells[9, 4].Text, 1);

            if (strCERTIDATE.Trim() == "")
            {
                strCERTIDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "DM");
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";

                SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_RATING_PATIENT_NEW SET ";

                if (FstrGubun == "2")
                {
                    SQL = SQL + ComNum.VBLF + " DIETPROBLEM1 = '" + strDIETPROBLEM1 + "',";
                    SQL = SQL + ComNum.VBLF + " DIETPROBLEM2 = '" + strDIETPROBLEM2 + "',";
                    SQL = SQL + ComNum.VBLF + " DIETPROBLEM3 = '" + strDIETPROBLEM3 + "',";
                    SQL = SQL + ComNum.VBLF + " DIETPROBLEM4 = '" + strDIETPROBLEM4 + "',";
                }

                if (ChkHT.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " HEIGHT = '" + strHT + "',";
                }
                if (ChkWT.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WEIGHT = '" + strWT + "',";
                }

                if (strHT != "" && strWT != "")
                {
                    //if (VB.Val(strAge) <= 14)     strAge 변수 존재 하지 않음 , 값 마저 들어가지 않음
                    //{
                    //    strHW = READ_CHILD_HWEIGHT(strHT, strWT, strSEX)


                    //     If strHW <> "" Then


                    //         strIBW = Format((Val(strWT) / Val(strHW)) * 100, "0")


                    //     End If
                    //}

                    strHW = ((VB.Val(strHT) * 0.01) * (VB.Val(strHT) * 0.01) * VB.Val((strSEX == "M" ? "22" : "21"))).ToString("0.0");
                    strIBW = (VB.Val(strWT) / ((VB.Val(strHT) * 0.01) * (VB.Val(strHT) * 0.01) * VB.Val((strSEX == "M" ? "22" : "21"))) * 100).ToString("0.0");

                    SQL = SQL + ComNum.VBLF + " HWEIGHT = '" + strHW + "',";
                    SQL = SQL + ComNum.VBLF + " PIBW = '" + strIBW + "',";
                }
                SQL = SQL + ComNum.VBLF + " CERTIDATE = TO_DATE('" + (strCERTIDATE).Trim() + "','YYYY-MM-DD HH24:MI'), ";
                SQL = SQL + ComNum.VBLF + " CERTISABUN = '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + " MEMO = '" + (strMemo).Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + ArgROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (rEventClose != null)
            {
                rEventClose();
            }
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);


        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (VB.UCase(strEXEName) != "DIETORDER")
            {
                MessageBox.Show("사용 불가능 합니다.");
                return;
            }

            Save_Data(FstrROWID);
            READ_DATA(FstrROWID);
        }

        private void READ_DATA(string ArgROWID)
        {
            CLEAR_SS();


            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(INDATE,'YYYY-MM-DD HH24:MI') INDATE, PANO, SNAME, SEX, AGE,";
                SQL = SQL + ComNum.VBLF + " DEPTCODE, ROOMCODE, DIAGNOSIS, HEIGHT, WEIGHT,";
                SQL = SQL + ComNum.VBLF + " HWEIGHT, PIBW, ALBUMIN, HB, ";
                SQL = SQL + ComNum.VBLF + " TLC, TCHOL, DIETNAME, DIETPROBLEM1, ";
                SQL = SQL + ComNum.VBLF + " DIETPROBLEM2, DIETPROBLEM3, DIETPROBLEM4,TO_CHAR(CERTIDATE,'YYYY-MM-DD HH24:MI') CERTIDATE, ";
                SQL = SQL + ComNum.VBLF + " CERTISABUN, MEMO, RATING";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_RATING_PATIENT_NEW";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + ArgROWID + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.Cells[4, 4].Text = dt.Rows[0]["INDATE"].ToString().Trim();

                    ss1_Sheet1.Cells[8, 4].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ss1_Sheet1.Cells[8, 8].Text = dt.Rows[0]["SNAME"].ToString().Trim();

                    ss1_Sheet1.Cells[9, 4].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" +
                         dt.Rows[0]["AGE"].ToString().Trim();
                    ss1_Sheet1.Cells[9, 8].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + "/" +
                        dt.Rows[0]["ROOMCODE"].ToString().Trim();

                    ss1_Sheet1.Cells[10, 4].Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();

                    ss1_Sheet1.Cells[14, 2].Text = "▣Ht:" + dt.Rows[0]["HEIGHT"].ToString().Trim() +
                        "cm  ▣Wt:" +
                        dt.Rows[0]["WEIGHT"].ToString().Trim() + "kg  " +
                    "▣표준체중:" + dt.Rows[0]["HWEIGHT"].ToString().Trim() +
                    "kg  ▣%IBW:" + dt.Rows[0]["PIBW"].ToString().Trim();

                    ss1_Sheet1.Cells[18, 4].Text = dt.Rows[0]["PIBW"].ToString().Trim();
                    ss1_Sheet1.Cells[18, 8].Text = dt.Rows[0]["ALBUMIN"].ToString().Trim();

                    ss1_Sheet1.Cells[19, 4].Text = dt.Rows[0]["HB"].ToString().Trim();
                    ss1_Sheet1.Cells[19, 8].Text = dt.Rows[0]["TLC"].ToString().Trim();

                    ss1_Sheet1.Cells[20, 4].Text = dt.Rows[0]["TCHOL"].ToString().Trim();
                    ss1_Sheet1.Cells[20, 8].Text = dt.Rows[0]["AGE"].ToString().Trim();

                    ss1_Sheet1.Cells[21, 4].Text = dt.Rows[0]["DIETNAME"].ToString().Trim();

                    if (dt.Rows[0]["DIETPROBLEM1"].ToString().Trim() == "0" &&
                        dt.Rows[0]["DIETPROBLEM2"].ToString().Trim() == "0" &&
                        dt.Rows[0]["DIETPROBLEM3"].ToString().Trim() == "0" &&
                        dt.Rows[0]["DIETPROBLEM4"].ToString().Trim() == "0")
                    {
                        ss1_Sheet1.Cells[23, 3].Value = true;
                        ss1_Sheet1.Cells[23, 4].Value = false;
                        ss1_Sheet1.Cells[23, 6].Value = false;
                        ss1_Sheet1.Cells[23, 7].Value = false;
                        ss1_Sheet1.Cells[23, 8].Value = false;
                    }
                    else
                    {
                        ss1_Sheet1.Cells[23, 3].Value = false;
                        ss1_Sheet1.Cells[23, 4].Value = dt.Rows[0]["DIETPROBLEM1"].ToString().Trim() == "1" ? true : false;
                        ss1_Sheet1.Cells[23, 6].Value = dt.Rows[0]["DIETPROBLEM2"].ToString().Trim() == "1" ? true : false;
                        ss1_Sheet1.Cells[23, 7].Value = dt.Rows[0]["DIETPROBLEM3"].ToString().Trim() == "1" ? true : false;
                        ss1_Sheet1.Cells[23, 8].Value = dt.Rows[0]["DIETPROBLEM4"].ToString().Trim() == "1" ? true : false;
                    }

                    switch (dt.Rows[0]["RATING"].ToString().Trim())
                    {
                        case "1":
                            ss1_Sheet1.Cells[27, 7].Text = "양호군";
                            break;
                        case "2":
                            ss1_Sheet1.Cells[27, 7].Text = "중위험";
                            break;
                        case "3":
                            ss1_Sheet1.Cells[27, 7].Text = "고위험";
                            break;
                    }
                }
                ss1_Sheet1.Cells[29, 2].Text = dt.Rows[0]["MEMO"].ToString().Trim();

                ss1_Sheet1.Cells[31, 3].Text = VB.Left(dt.Rows[0]["CERTIDATE"].ToString().Trim(), 10);
                ss1_Sheet1.Cells[31, 5].Text = VB.Right(dt.Rows[0]["CERTIDATE"].ToString().Trim(), 5);
                ss1_Sheet1.Cells[31, 8].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["CERTISABUN"].ToString().Trim());

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

        private void CLEAR_SS()
        {
            if (VB.UCase(strEXEName) == "DIETORDER")
            {
                btnSave.Enabled = true;
            }

            TxtHEIGHT.Text = "";
            TxtWEIGHT.Text = "";

            if (FstrGubun == "2")
            {
                ss1_Sheet1.Cells[2, 2].Text = "영양재평가";
            }
            else
            {
                ss1_Sheet1.Cells[2, 2].Text = "영양초기평가";
            }

            ss1_Sheet1.Cells[4, 4].Text = "";

            ss1_Sheet1.Cells[8, 4].Text = "";
            ss1_Sheet1.Cells[8, 8].Text = "";

            ss1_Sheet1.Cells[9, 4].Text = "";
            ss1_Sheet1.Cells[9, 8].Text = "";

            ss1_Sheet1.Cells[10, 4].Text = "";
            ss1_Sheet1.Cells[10, 8].Text = "";

            ss1_Sheet1.Cells[14, 2].Text = "";

            ss1_Sheet1.Cells[18, 4].Text = "";
            ss1_Sheet1.Cells[18, 8].Text = "";

            ss1_Sheet1.Cells[19, 4].Text = "";
            ss1_Sheet1.Cells[19, 8].Text = "";

            ss1_Sheet1.Cells[20, 4].Text = "";
            ss1_Sheet1.Cells[20, 8].Text = "";

            ss1_Sheet1.Cells[21, 4].Text = "";
            ss1_Sheet1.Cells[21, 8].Text = "";

            ss1_Sheet1.Cells[23, 3].Text = "";
            ss1_Sheet1.Cells[23, 4].Text = "";
            ss1_Sheet1.Cells[23, 6].Text = "";
            ss1_Sheet1.Cells[23, 7].Text = "";
            ss1_Sheet1.Cells[23, 8].Text = "";

            ss1_Sheet1.Cells[27, 7].Text = "";

            ss1_Sheet1.Cells[29, 2].Text = "";

            ss1_Sheet1.Cells[31, 3].Text = "";
            ss1_Sheet1.Cells[31, 5].Text = "";
            ss1_Sheet1.Cells[31, 8].Text = "";
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            //TODO 카덱스 폴더 없음
            //Frm영양평가지표.Show 1
        }

        private string READ_CHILD_HWEIGHT(string argHeight, string argWEIGHT, string ArgSex)
        {
            string strWEIGHT;
            string strVal = "";

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WEIGHT_F, WEIGHT_M ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_CHILD_HWEIGHT";
                SQL = SQL + ComNum.VBLF + " WHERE HEIGHT = '" + (int)(VB.Val(argHeight)) + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count > 0)
                {
                    switch (ArgSex)
                    {
                        case "F":
                            strVal = strWEIGHT = dt.Rows[0]["WEIGHT_F"].ToString().Trim();
                            break;
                        case "M":
                            strVal = strWEIGHT = dt.Rows[0]["WEIGHT_M"].ToString().Trim();
                            break;
                    }

                }

                dt.Dispose();
                dt = null;

                return strVal;
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
                return strVal;
            }
        }
    }
}
