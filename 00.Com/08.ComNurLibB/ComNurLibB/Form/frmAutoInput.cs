using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmAutoInput.cs
    /// Description     : 간호부 스케쥴 일괄입력
    /// Author          : 유진호
    /// Create Date     : 2018-02-02
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\nurse\nropd\FrmAutoInput
    /// </history>
    /// </summary>
    public partial class frmAutoInput : Form
    {
        ComFunc CF = new ComFunc();

        public frmAutoInput()
        {
            InitializeComponent();
        }

        private void frmAutoInput_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpTDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            txtSabun.Text = "";
            txtName.Text = "";

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            string strSDATE = "";
            string strEDATE = "";
            string strSABUN = "";
            string strGubun = "";

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return; //권한 확인

            if (dtpFDate.Value == dtpTDate.Value)
            {
                ComFunc.MsgBox("년월이 동일할 경우 스케쥴 등록 화면에서 입력하시기 바랍니다.");
                return;
            }

            if (optGubun1.Checked == true)
            {
                strGubun = "TGGG";
            }
            else if (optGubun2.Checked == true)
            {
                strGubun = "TRRR";
            }
            else if (optGubun3.Checked == true)
            {
                strGubun = "TUUU";
            }
            else if (optGubun4.Checked == true)
            {
                strGubun = "TZZZ";
            }
            if (strGubun == "")
            {
                ComFunc.MsgBox("구분이 선택되지 않았습니다.");
                return;
            }

            strSABUN = txtSabun.Text.PadLeft(5, '0');
            strSDATE = dtpFDate.Value.ToShortDateString();
            strEDATE = dtpTDate.Value.ToShortDateString();

            if (strGubun == "")
            {
                ComFunc.MsgBox("구분을 선택하십시요.");
                return;
            }

            if (VB.Trim(txtName.Text) == "")
            {
                ComFunc.MsgBox("사번이 정확하지 않습니다.");
                return;
            }

            InputNurSchedule(strGubun, strSDATE, strEDATE, strSABUN);
            ComFunc.MsgBox("완료되었습니다.");
        }

        private bool InputNurSchedule(string argGUBUN, string argSDATE, string argEDATE, string ArgSabun)
        {
            bool rtVal = false;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            string strJik = "";
            string strName = "";
            string strBuse = "";

            //int nMonth = 0;
            string strSYYMM = "";
            string strEYYMM = "";
            string strGubun = "";
            string strROWID = "";
            int nLastDay = 0;
            string strSCHEDULE = "";
            string strTotSchedule = "";
            string strNurseYYMM = "";

            int i = 0;
            int j = 0;
            //int nREAD = 0;
            int nTEMP = 0;
            //int nDD = 0;

            //bool bA = false;
            //bool bB = false;

            string[] strYYMM = new string[32];
            string[] str_BunpYo = new string[32];       //'insa변수번표
            string[] strBunpYo = new string[32];        //'insa변수번표


            switch (argGUBUN)
            {
                case "TGGG":
                    strGubun = "분휴";
                    break;
                case "TRRR":
                    strGubun = "휴직";
                    break;
                case "TUUU":
                    strGubun = "파견";
                    break;
                case "TZZZ":
                    strGubun = "산재";
                    break;
            }

            strSYYMM = VB.Replace(VB.Left(argSDATE, 7), "-", "");
            strEYYMM = VB.Replace(VB.Left(argEDATE, 7), "-", "");

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT JIK, BUSE, KORNAME ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + ArgSabun + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strJik = dt.Rows[0]["JIK"].ToString().Trim();
                    strBuse = getDeptName(dt.Rows[0]["BUSE"].ToString().Trim());
                    strName = dt.Rows[0]["KORNAME"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                ssView1_Sheet1.Cells[0, 0].Text = strBuse;
                ssView1_Sheet1.Cells[0, 1].Text = ArgSabun;
                ssView1_Sheet1.Cells[0, 2].Text = strName;
                ssView1_Sheet1.Cells[0, 3].Text = strJik;


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(ADD_MONTHS(TO_DATE('" + strSYYMM + "','YYYYMM'), LEVEL-1), 'YYYYMM') MONTH";
                SQL = SQL + ComNum.VBLF + " FROM DUAL";
                SQL = SQL + ComNum.VBLF + " CONNECT BY ADD_MONTHS(TO_DATE('" + strSYYMM + "','YYYYMM'), LEVEL-1) <= TO_DATE('" + strEYYMM + "','YYYYMM')";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strYYMM = new string[dt.Rows.Count];
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strYYMM[i] = dt.Rows[i]["MONTH"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;


                for (i = 0; i < strYYMM.Length; i++)
                {
                    for (j = 4; j < 37; j++)
                    {
                        ssView1_Sheet1.Cells[0, j].Text = "";
                    }
                    ssView1_Sheet1.Cells[0, 35].Text = strYYMM[i];

                    nLastDay = DateTime.DaysInMonth(Convert.ToInt32(VB.Val(VB.Left(strYYMM[i], 4))), Convert.ToInt32(VB.Val(VB.Right(strYYMM[i], 2))));


                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT SCHEDULE, ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE1 ";
                    SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM[i] + "'";
                    SQL = SQL + ComNum.VBLF + "   AND SABUN = '" + ArgSabun + "' ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strSCHEDULE = dt.Rows[0]["SCHEDULE"].ToString().Trim();
                        nTEMP = 1;
                        for (j = 0; j < nLastDay; j++)
                        {
                            ssView1_Sheet1.Cells[0, j + 4].Text = VB.Trim(ComFunc.MidH(strSCHEDULE, nTEMP, 4));
                            nTEMP = nTEMP + 4;
                        }
                        ssView1_Sheet1.Cells[0, 36].Text = dt.Rows[0]["ROWID"].ToString().Trim();

                    }
                    else
                    {
                        SetOFF(strYYMM[i]);
                    }
                    dt.Dispose();
                    dt = null;



                    for (j = 1; j <= nLastDay; j++)
                    {
                        //bA = false;
                        //bB = false;
                        if (strYYMM[i] == VB.Replace(VB.Left(argSDATE, 7), "-", ""))
                        {
                            if (j >= VB.Val(VB.Right(argSDATE, 2)))
                            {
                                ssView1_Sheet1.Cells[0, j + 3].Text = strGubun;
                                //bA = true;
                            }
                        }
                        else if (strYYMM[i] == VB.Replace(VB.Left(argEDATE, 7), "-", ""))
                        {
                            if (j <= VB.Val(VB.Right(argEDATE, 2)))
                            {
                                ssView1_Sheet1.Cells[0, j + 3].Text = strGubun;
                                //bB = true;
                            }
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[0, j + 3].Text = strGubun;
                        }
                    }

                    strROWID = ssView1_Sheet1.Cells[0, 36].Text;
                    strNurseYYMM = ssView1_Sheet1.Cells[0, 35].Text;
                    strTotSchedule = "";

                    for (j = 5; j <= nLastDay + 4; j++)
                    {
                        strSCHEDULE = ComFunc.MidH(VB.Trim(ssView1_Sheet1.Cells[0, j-1].Text) + VB.Space(4), 1, 4);
                        strTotSchedule = strTotSchedule + strSCHEDULE;
                    }

                    if (strROWID != "")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE1 SET ";
                        SQL = SQL + ComNum.VBLF + " SCHEDULE = '" + strTotSchedule + "'";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.NUR_SCHEDULE1(";
                        SQL = SQL + ComNum.VBLF + "  YYMM, WARDCODE, SABUN, SNAME,";
                        SQL = SQL + ComNum.VBLF + "  JIKCODE, SCHEDULE) VALUES (";
                        SQL = SQL + ComNum.VBLF + " '" + strNurseYYMM + "','" + clsOpdNr.READ_BUSECODE(clsDB.DbCon, strBuse) + "','" + ArgSabun + "','" + strName + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strJik + "','" + strTotSchedule + "') ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }


                    for (j = 5; j <= nLastDay + 4; j++)
                    {
                        strSCHEDULE = ComFunc.MidH(VB.Trim(ssView1_Sheet1.Cells[0, j-1].Text) + VB.Space(4), 1, 4);
                        str_BunpYo[j - 4] = VB.Trim(BunpYo_Data(strSCHEDULE));
                        if (str_BunpYo[j - 4] == "")
                        {
                            ComFunc.MsgBox("(" + (j - 4) + "일 사번은 " + ArgSabun + ") Schedule을 다시 확인 하세요.!!");
                        }
                    }



                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT SABUN, YYMM, ";
                    for (j = 1; j <= nLastDay; j++)
                    {
                        SQL = SQL + ComNum.VBLF + "BUNPYO" + j + ", CHULTIME" + j + ", CHULGBN" + j + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " RowId FROM KOSMOS_ADM.INSA_CHULTIME ";
                    SQL = SQL + ComNum.VBLF + " WHERE SaBUN = '" + ArgSabun + "' ";
                    SQL = SQL + ComNum.VBLF + " AND YYMM = '" + strYYMM[i] + "' ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        SQL = "INSERT INTO KOSMOS_ADM.INSA_CHULTIME(SABUN, YYMM, ";
                        for (j = 1; j < nLastDay; j++)
                        {
                            SQL = SQL + ComNum.VBLF + " BUNPYO" + j + ", ";
                            if (j == nLastDay - 1)
                                SQL = SQL + ComNum.VBLF + " BUNPYO" + (j + 1) + ") values ";
                        }

                        SQL = SQL + ComNum.VBLF + " ('" + ArgSabun + "', '" + strYYMM[i] + "',";
                        for (j = 1; j < nLastDay; j++)
                        {
                            SQL = SQL + ComNum.VBLF + " '" + str_BunpYo[j] + "', ";
                            if (j == nLastDay - 1)
                                SQL = SQL + ComNum.VBLF + " '" + str_BunpYo[j + 1] + "') ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                    else
                    {
                        for (j = 1; j <= nLastDay; j++)
                        {
                            strBunpYo[j] = dt.Rows[0]["BunpYo" + j.ToString()].ToString().Trim();
                            if (str_BunpYo[j] != strBunpYo[j])
                                strBunpYo[j] = str_BunpYo[j];
                        }
                        SQL = " UPDATE KOSMOS_ADM.INSA_CHULTIME SET ";
                        for (j = 1; j < nLastDay; j++)
                        {
                            SQL = SQL + ComNum.VBLF + " BUNPYO" + j + " = '" + strBunpYo[j] + "',";
                            if (j == nLastDay - 1)
                                SQL = SQL + " BUNPYO" + (j + 1) + " = '" + strBunpYo[j + 1] + "'";
                        }
                        SQL = SQL + ComNum.VBLF + " WHERE SABUN  = '" + ArgSabun + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND YYMM  = '" + strYYMM[i] + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                    dt.Dispose();
                    dt = null;
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

        private string getDeptName(string argDeptCode)
        {
            string rtnVar = "";

            switch (argDeptCode)
            {
                case "033101":
                    rtnVar = "간호부";
                    break;
                case "033102":
                    rtnVar = "OR";
                    break;
                case "033103":
                    rtnVar = "마취과";
                    break;
                case "033104":
                    rtnVar = "NR";
                    break;
                case "033105":
                    rtnVar = "DR";
                    break;
                case "033106":
                    rtnVar = "SICU";
                    break;
                case "033107":
                    rtnVar = "공급실";
                    break;
                case "033108":
                    rtnVar = "HD";
                    break;
                case "033109":
                    rtnVar = "ER";
                    break;
                case "033110":
                    rtnVar = "외래";
                    break;
                case "033111":
                    rtnVar = "정신과";
                    break;
                case "033112":
                    rtnVar = "3A";
                    break;
                case "033113":
                    rtnVar = "3B";
                    break;
                case "033114":
                    rtnVar = "4A";
                    break;
                case "033116":
                    rtnVar = "5W";
                    break;
                case "033121":
                    rtnVar = "호스피스";
                    break;
                case "033117":
                    rtnVar = "6W";
                    break;
                case "033118":
                    rtnVar = "7W";
                    break;
                case "033119":
                    rtnVar = "8W";
                    break;
                //case "033106":      rtnVar = "MICU"   ;     break;
                //case "033121":      rtnVar = "HU"     ;     break;
                case "033126":
                    rtnVar = "6A";
                    break;
                case "033127":
                    rtnVar = "3W";
                    break;
                case "033125":
                    rtnVar = "4W";
                    break;
                case "044501":
                    rtnVar = "종합건진";
                    break;
                case "101743":
                    rtnVar = "32";
                    break;
                case "101744":
                    rtnVar = "33";
                    break;
                case "101745":
                    rtnVar = "34";
                    break;
                case "101764":
                    rtnVar = "35";
                    break;
                case "101752":
                    rtnVar = "40";
                    break;
                case "101753":
                    rtnVar = "50";
                    break;
                case "101746":
                    rtnVar = "53";
                    break;
                case "101747":
                    rtnVar = "55";
                    break;
                case "101754":
                    rtnVar = "60";
                    break;
                case "101748":
                    rtnVar = "63";
                    break;
                case "101749":
                    rtnVar = "65";
                    break;
                case "101755":
                    rtnVar = "70";
                    break;
                case "101750":
                    rtnVar = "73";
                    break;
                case "101751":
                    rtnVar = "75";
                    break;
                case "101756":
                    rtnVar = "80";
                    break;
                case "101776":
                    rtnVar = "83";
                    break;
                case "101781":
                    rtnVar = "85";
                    break; 
                case "101757":
                    rtnVar = "DS";
                    break;
                case "100410":
                    rtnVar = "치과";
                    break;
                default:
                    rtnVar = "TEST";
                    break;
            }

            return rtnVar;
        }

        private string BunpYo_Data(string ArgData)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (string.Compare(VB.Left(ArgData, 1), "가") >= 0)
                {
                    switch (ArgData)
                    {
                        case "휴가":
                            rtnVal = "TAAA";
                            break;
                        case "월차":
                            rtnVal = "TBBB";
                            break;
                        case "특일":
                            rtnVal = "TCCC";
                            break;
                        case "경조":
                            rtnVal = "TCCC";
                            break;
                        case "교육":
                            rtnVal = "TDDD";
                            break;
                        case "출장":
                            rtnVal = "TEEE";
                            break;
                        case "병가":
                            rtnVal = "TFFF";
                            break;
                        case "분휴":
                            rtnVal = "TGGG";
                            break;
                        case "피정":
                            rtnVal = "THHH";
                            break;
                        case "훈련":
                            rtnVal = "TIII";
                            break;
                        case "생휴":
                            rtnVal = "TJJJ";
                            break;
                        case "학회":
                            rtnVal = "TKKK";
                            break;
                        case "결근":
                            rtnVal = "TLLL";
                            break;
                        case "지각":
                            rtnVal = "TMMM";
                            break;
                        case "조퇴":
                            rtnVal = "TNNN";
                            break;
                        case "반휴":
                            rtnVal = "TPPP";
                            break;
                        case "휴직":
                            rtnVal = "TRRR";
                            break;
                        case "파견":
                            rtnVal = "TUUU";
                            break;
                        case "난휴":
                            rtnVal = "TVVV";
                            break;
                        case "가휴":
                            rtnVal = "TWWW";
                            break;
                        case "무휴":
                            rtnVal = "TYYY";
                            break;
                        case "산재":
                            rtnVal = "TZZZ";
                            break;
                        case "무반":
                            rtnVal = "TQQQ";
                            break;
                    }
                }
                else
                {
                    if (ArgData == "    ")
                    {
                        rtnVal = "D083";
                    }
                    else
                    {

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT CODE From KOSMOS_ADM.INSA_GUNTAECODE ";
                        SQL = SQL + ComNum.VBLF + " WHERE NURCODE = '" + ArgData + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVal;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            rtnVal = dt.Rows[0]["CODE"].ToString().Trim();
                        }
                        else
                        {
                            rtnVal = "";
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }
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


        private void SetOFF(string argYYMM)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSDATE = "";
            string strYYMM = "";
            string strLastDay = "";

            strYYMM = VB.Left(argYYMM, 4) + "-" + VB.Right(argYYMM, 2) + "-";
            strSDATE = VB.Left(argYYMM, 4) + "-" + VB.Right(argYYMM, 2) + "-01";

            strLastDay = Convert.ToString(DateTime.DaysInMonth(Convert.ToInt32(VB.Left(argYYMM, 4)), Convert.ToInt32(VB.Right(argYYMM, 2))));

            try
            {
                for (i = 1; i < VB.Val(VB.Right(strLastDay, 2)); i++)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " Select TO_CHAR(JobDate,'YYYY-MM-DD') JOBDATE  ";
                    SQL = SQL + ComNum.VBLF + "  FROM BAS_JOB ";
                    SQL = SQL + ComNum.VBLF + " WHERE HolyDay = '*' ";
                    SQL = SQL + ComNum.VBLF + "   AND JobDate = TO_DATE('" + strYYMM + i.ToString("00") + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssView1_Sheet1.Cells[0, i + 3].Text = "OFF";
                    }
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

        private void txtSabun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, txtSabun.Text.PadLeft(5, '0'));
            }
        }
    }
}
