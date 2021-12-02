using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;


namespace ComNurLibB
{
    public partial class frmErOpUseList : Form
    {
        public frmErOpUseList()
        {
            InitializeComponent();
        }

        private void frmErOpUseList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

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
                //'입실경로
                cboOpRoom.Items.Clear();
                cboOpRoom.Items.Add("전체");
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

                cboEx.Items.Clear();
                cboEx.Items.Add("응급전용수술실");
                cboEx.Items.Add("예비수술실");
                cboEx.Items.Add("Angio");
                cboEx.SelectedIndex = 0;

                cboGaSan.Items.Clear();
                cboGaSan.Items.Add("주간");
                cboGaSan.Items.Add("야간");
                cboGaSan.Items.Add("공휴일");
                cboGaSan.Items.Add("야간 또는 공휴일");

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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            clsSpread CS = new clsSpread();


            int i = 0;

            ssExel_Sheet1.RowCount = 4;
            ssExel_Sheet1.RowCount = ssView1_Sheet1.RowCount + 4;

            for (i = 0; i < ssView1_Sheet1.RowCount; i++)
            {
                //12 -> 14
                ssExel_Sheet1.Cells[i + 3, 0].Text = ssView1_Sheet1.Cells[i, 1].Text;
                ssExel_Sheet1.Cells[i + 3, 1].Text = ssView1_Sheet1.Cells[i, 2].Text;
                ssExel_Sheet1.Cells[i + 3, 2].Text = ssView1_Sheet1.Cells[i, 3].Text;//'응급실입실시간
                ssExel_Sheet1.Cells[i + 3, 3].Text = ssView1_Sheet1.Cells[i, 4].Text;//'응급실입실시간
                ssExel_Sheet1.Cells[i + 3, 4].Text = ssView1_Sheet1.Cells[i, 5].Text; // '응급실퇴실시간
                ssExel_Sheet1.Cells[i + 3, 5].Text = ssView1_Sheet1.Cells[i, 6].Text; //'응급실퇴실시간
                ssExel_Sheet1.Cells[i + 3, 6].Text = ssView1_Sheet1.Cells[i, 7].Text;
                ssExel_Sheet1.Cells[i + 3, 7].Text = ssView1_Sheet1.Cells[i, 8].Text; //'수술시작
                ssExel_Sheet1.Cells[i + 3, 8].Text = ssView1_Sheet1.Cells[i, 9].Text;//'수술시작
                ssExel_Sheet1.Cells[i + 3, 9].Text = ssView1_Sheet1.Cells[i, 10].Text;//'수술종료
                ssExel_Sheet1.Cells[i + 3, 10].Text = ssView1_Sheet1.Cells[i, 11].Text;//'수술종료

                ////'의뢰과
                switch (ssView1_Sheet1.Cells[i, 12].Text.Trim())
                {
                    case "GS":
                        ssExel_Sheet1.Cells[i + 3, 11].Text = "BA";
                        break;
                    case "NS":
                        ssExel_Sheet1.Cells[i + 3, 11].Text = "BB";
                        break;
                    case "CS":
                        ssExel_Sheet1.Cells[i + 3, 11].Text = "BC";
                        break;
                    case "OS":
                        ssExel_Sheet1.Cells[i + 3, 11].Text = "BD";
                        break;
                    case "OG":
                        ssExel_Sheet1.Cells[i + 3, 11].Text = "CA";
                        break;
                    case "OT":
                        ssExel_Sheet1.Cells[i + 3, 11].Text = "GA";
                        break;
                    case "EN":
                        ssExel_Sheet1.Cells[i + 3, 11].Text = "HA";
                        break;

                    default:
                        ssExel_Sheet1.Cells[i + 3, 11].Text = ssView1_Sheet1.Cells[i, 12].Text.Trim();
                        break;
                }
               
                ssExel_Sheet1.Cells[i + 3, 12].Text = ssView1_Sheet1.Cells[i, 13].Text;// //'수술후진단명
            }

            ssExel_Sheet1.SetRowHeight(-1, 30);

            CS.ExportToXLS(ssExel);
            CS = null;
        }

        private string CHANGE_DEPTCODE(string argDEPTCODE)
        {

            string strDEPT = "";

            switch (argDEPTCODE)
            {

                case "MI":
                    strDEPT = "AF";
                    break;
                case "MO":
                    strDEPT = "AG";
                    break;
                case "MG":
                    strDEPT = "AC";
                    break;
                case "MC":
                    strDEPT = "AA";
                    break;
                case "MP":
                    strDEPT = "AB";
                    break;
                case "ME":
                    strDEPT = "AE";
                    break;
                case "MN":
                    strDEPT = "AD";
                    break;
                case "MR":
                    strDEPT = "AX";
                    break;
                case "GS":
                    strDEPT = "BA";
                    break;
                case "NS":
                    strDEPT = "BB";
                    break;
                case "OS":
                    strDEPT = "BD";
                    break;
                case "OG":
                    strDEPT = "CA";
                    break;
                case "CS":
                    strDEPT = "BC";
                    break;
                case "PD":
                    strDEPT = "DA";
                    break;
                case "NP":
                    strDEPT = "EA";
                    break;
                case "OT":
                    strDEPT = "GA";
                    break;
                case "EN":
                    strDEPT = "HA";
                    break;
                case "UR":
                    strDEPT = "IA";
                    break;
                case "ER":
                    strDEPT = "JA";
                    break;
                case "DM":
                    strDEPT = "LA";
                    break;
                case "DT":
                    strDEPT = "NA";
                    break;
                case "NE":
                    strDEPT = "FA";
                    break;
                default:
                    strDEPT = "XX";
                    break;

            }

            return strDEPT;

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
                    strROWID = ssView1_Sheet1.Cells[i, 15].Text.Trim();
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
                READ_DATA_OPD();
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
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //    return; //권한 확인


            READ_DATA_OPD();
        }

        private void READ_DATA_OPD()
        {
            int i = 0;

            string strInDate = "";
            string strAMset7 = "";

            string strRefInDate = "";
            string strRefOutDate = "";

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView1_Sheet1.RowCount = 0;


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "  SELECT   IPDOPD ,OPROOM, B.SNAME, B.PANO,   ROOMREMK , ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(OPDATE,'YYYY-MM-DD')  OPDATE,TO_CHAR(OPDATE+1,'YYYY-MM-DD')  OPDATE1,DR_STIME, OPETIME, B.DEPTCODE,B.DIAGNOSIS, B.ROWID, B.WARDCODE, B.PATH_IN  , C.HOLYDAY, C.TEMPHOLYDAY, B.OPTITLE  ";
                SQL = SQL + ComNum.VBLF + " FROM  ORAN_MASTER B   ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN BAS_JOB C    ";
                SQL = SQL + ComNum.VBLF + "     ON B.OPDATE = C.JOBDATE     ";
                SQL = SQL + ComNum.VBLF + " WHERE  B.OPDATE >= TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND  B.OPDATE <= TO_DATE('" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (cboDept.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND B.DEPTCODE = '" + cboDept.Text.Trim() + "'  ";
                }

                if (cboEx.Text.Trim() == "응급전용수술실")
                {
                    SQL = SQL + ComNum.VBLF + " AND OPROOM  IN ('9')   ";
                }
                else if (cboEx.Text.Trim() == "예비수술실")
                {
                    SQL = SQL + ComNum.VBLF + " AND OPROOM  IN ('6')   ";
                }
                //2018-11-06 안정수, Angio일 경우
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND GbAngio = 'Y'";                    
                    SQL = SQL + ComNum.VBLF + " AND OpRoom <> 'N'";
                    SQL = SQL + ComNum.VBLF + " AND OPCANCEL is null";
                }

                //2018-11-06 안정수, Angio가 아닐경우
                if (cboEx.Text.Trim() != "Angio")
                {
                    SQL = SQL + ComNum.VBLF + "   AND (B.GBANGIO IS NULL OR B.GBANGIO <> 'Y') ";
                    SQL = SQL + ComNum.VBLF + "   AND B.OPROOM <> 'N' AND B.OPCANCEL IS NULL  ";
                }

                if (cboGaSan.Text.Trim() == "주간")
                {
                    SQL = SQL + ComNum.VBLF + "   AND REPLACE(OPETIME,':', '')  <= 1800   ";
                    SQL = SQL + ComNum.VBLF + "   AND REPLACE(OPETIME,':', '')  >= 0900   ";
                }
                else if (cboGaSan.Text.Trim() == "야간")
                {
                    SQL = SQL + ComNum.VBLF + "   AND (REPLACE(OPETIME, ':', '') >= 1801 OR REPLACE(OPETIME, ':', '') <= 0859)      ";
                }
                else if (cboGaSan.Text.Trim() == "공휴일")
                {
                    SQL = SQL + ComNum.VBLF + "   AND (C.HOLYDAY = '*' OR TEMPHOLYDAY = '*')      ";
                }
                else if (cboGaSan.Text.Trim() == "야간 또는 공휴일")
                {
                    SQL = SQL + ComNum.VBLF + "   AND (REPLACE(OPETIME, ':', '') >= 1801 OR REPLACE(OPETIME, ':', '') <= 0859 OR C.HOLYDAY = '*' OR TEMPHOLYDAY = '*')      ";
                }

                SQL = SQL + ComNum.VBLF + "   ORDER BY OPDATE DESC  ";

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

                    for (i = 0; i < dt.Rows.Count; i++) //'ROOMCODE
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["OPROOM"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();

                        ssView1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PATH_IN"].ToString().Trim();

                        ssView1_Sheet1.Cells[i, 14].Text = "0"; //'KTAS 초기화

                        //'입원환자는 정보읽어서 입원일자읽기
                        strInDate = "";
                        strAMset7 = "";

                        if (dt.Rows[i]["IPDOPD"].ToString().Trim() == "I")
                        {
                            SQL = "";
                            SQL = "SELECT TO_CHAR( INDATE ,'YYYY-MM-DD') INDATE  ,AMSET7 ,KTASLEVL  ";
                            SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_MASTER";
                            SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "    AND INDATE < TO_DATE('" + Convert.ToDateTime(dt.Rows[i]["OPDATE"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "    AND (OUTDATE >=TO_DATE('" + dt.Rows[i]["OPDATE"].ToString().Trim() + "','YYYY-MM-DD')   OR  OUTDATE IS NULL ) ";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                strInDate = dt1.Rows[0]["INDATE"].ToString().Trim();
                                strAMset7 = dt1.Rows[0]["AMSET7"].ToString().Trim();

                                READ_INDATE_OUTDATE_ER(dt.Rows[i]["PANO"].ToString().Trim(), strInDate, ref strRefInDate, ref strRefOutDate);

                                ssView1_Sheet1.Cells[i, 3].Text = VB.Left(strRefInDate, 10); //'응급실입실시간
                                ssView1_Sheet1.Cells[i, 4].Text = VB.Right(strRefInDate, 5); //'응급실입실시간
                                ssView1_Sheet1.Cells[i, 5].Text = VB.Left(strRefOutDate, 10); //'응급실퇴실시간
                                ssView1_Sheet1.Cells[i, 6].Text = VB.Right(strRefOutDate, 5); //'응급실퇴실시간

                                ssView1_Sheet1.Cells[i, 14].Text = dt1.Rows[0]["KTASLEVL"].ToString().Trim(); //'KTAS 
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                        else
                        {
                            strInDate = "";

                            SQL = "";
                            SQL = "SELECT PANO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, KTASLEVL FROM OPD_MASTER";
                            SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "    AND BDATE =TO_DATE('" + dt.Rows[i]["OPDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='ER' ";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                READ_INDATE_OUTDATE_ER(dt1.Rows[0]["PANO"].ToString().Trim(), dt1.Rows[0]["BDATE"].ToString().Trim(), ref strRefInDate, ref strRefOutDate);

                                ssView1_Sheet1.Cells[i, 3].Text = VB.Left(strRefInDate, 10); //'응급실입실시간
                                ssView1_Sheet1.Cells[i, 4].Text = VB.Right(strRefInDate, 5); //'응급실입실시간
                                ssView1_Sheet1.Cells[i, 5].Text = VB.Left(strRefOutDate, 10); //'응급실퇴실시간
                                ssView1_Sheet1.Cells[i, 6].Text = VB.Right(strRefOutDate, 5); //'응급실퇴실시간

                                ssView1_Sheet1.Cells[i, 14].Text = dt1.Rows[0]["KTASLEVL"].ToString().Trim(); //'KTAS 
                            }
                            else
                            {
                                ssView1_Sheet1.Cells[i, 14].Text = "0"; //'KTAS 
                            }


                            dt1.Dispose();
                            dt1 = null;
                        }

                        ssView1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PATH_IN"].ToString().Trim();

                        //2019-01-21
                        if (ssView1_Sheet1.Cells[i, 7].Text.Trim() == "" || chkReLoad.Checked == true)
                        {
                            ssView1_Sheet1.Cells[i, 7].Text = clsErNr.READ_NEDIS_IPWON_GUBUN_OP(dt.Rows[i]["PANO"].ToString().Trim(), VB.Left(strRefInDate, 10), VB.Left(strRefOutDate, 10));
                        }

                        //2019-02-18 참조 제외 요청함. 2019년 2월 18일 10시 5분. 담당자가 책임 전가 할 수 있어서 추가 리마크 기재함.
                        // 통화, 수술실 수간호사. 김소희???
                        //if (ssView1_Sheet1.Cells[i, 7].Text.Trim() == "" || ssView1_Sheet1.Cells[i, 7].Text.Trim() == "ETC")
                        //{
                        //    switch (strAMset7)
                        //    {
                        //        case "3":
                        //        case "4":
                        //        case "5":
                        //            ssView1_Sheet1.Cells[i, 7].Text = "ER";
                        //            break;
                        //    }
                        //}

                        ssView1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["OPDATE"].ToString().Trim(); //'수술시작
                        ssView1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DR_STIME"].ToString().Trim(); //'수술시작

                        if (VB.Val(dt.Rows[i]["DR_STIME"].ToString().Trim()) > VB.Val(dt.Rows[i]["OPETIME"].ToString().Trim()))
                        {
                            ssView1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["OPDATE1"].ToString().Trim(); //'수술종료
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["OPDATE"].ToString().Trim(); //'수술종료
                        }

                        ssView1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["OPETIME"].ToString().Trim(); //'수술종료

                        ssView1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim(); //'의뢰과
                        ssView1_Sheet1.Cells[i, 16].Text = CHANGE_DEPTCODE(dt.Rows[i]["DEPTCODE"].ToString().Trim()); //'의뢰과

                        //2018-11-06 안정수, Angio일 경우 시술명으로 표기, 그 외 수술명으로 표기되도록 수정
                        if (cboEx.Text.Trim() != "Angio")
                        {
                            ssView1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["DIAGNOSIS"].ToString().Trim(); //'수술후진단명
                            ssView1.ActiveSheet.ColumnHeader.Cells[0, 13].Text = "진단명";
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["OPTITLE"].ToString().Trim(); //'시술명
                            ssView1.ActiveSheet.ColumnHeader.Cells[0, 13].Text = "시술명";
                        }

                        ssView1_Sheet1.Cells[i, 15].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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


        private void btnPrint_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            strFont1 = "/fn\"맑은 고딕\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs2";

            if (cboEx.Text == "응급전용수술실")
            {
                strHead1 = "/c/f1" + VB.Left(dtpSDATE.Text,4) + "년 " + VB.Mid(dtpSDATE.Text, 6,2) + "월 " + "응급전용수술실" + "/f1/n";
                strHead2 = "/c/f2" + "응급전용수술실 이용환자 대장" + "/f2/n";
            }
            else if ( cboEx.Text == "예비수술실")
            {
                strHead1 = "/c/f1" + "예비수술실 이용환자 대장" + "/f1/n";
            }
            else
            {
                strHead1 = "/c/f1" + "혈관조영실 이용환자 대장" + "/f1/n";
            }


            SetSS1();

            SS1_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            SS1_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            SS1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            SS1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            SS1_Sheet1.PrintInfo.Margin.Top = 20;
            SS1_Sheet1.PrintInfo.Margin.Bottom = 20;
            SS1_Sheet1.PrintInfo.Margin.Header = 10;
            SS1_Sheet1.PrintInfo.ShowColor = false;
            SS1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            SS1_Sheet1.PrintInfo.ShowBorder = false;
            SS1_Sheet1.PrintInfo.ShowGrid = true;
            SS1_Sheet1.PrintInfo.ShowShadows = false;
            SS1_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
            SS1_Sheet1.PrintInfo.UseMax = true;
            SS1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            SS1_Sheet1.PrintInfo.UseSmartPrint = true;
            SS1_Sheet1.PrintInfo.ShowPrintDialog = false;
            SS1_Sheet1.PrintInfo.Preview = false;
            SS1.PrintSheet(0);




        }

        private void SetSS1()
        {
            //FarPoint.Win.ComplexBorder borderWhite = new FarPoint.Win.ComplexBorder(	//ㄱ
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //LEFT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //TOP
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //BOM
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            //FarPoint.Win.ComplexBorder border1 = new FarPoint.Win.ComplexBorder(	//ㄱ
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //BOM
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            //FarPoint.Win.ComplexBorder border2 = new FarPoint.Win.ComplexBorder(	//ㄱ
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //RIGHT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //BOM
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            //FarPoint.Win.ComplexBorder border3 = new FarPoint.Win.ComplexBorder(	//ㄱ
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //BOM
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            //FarPoint.Win.ComplexBorder border4 = new FarPoint.Win.ComplexBorder(	//ㄱ
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //RIGHT
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //BOM
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            int i = 0;

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
                SS1_Sheet1.AddSpanCell(i + 7, 11, 1, 2);
                SS1_Sheet1.Cells[i + 7, 11].Text = ssView1_Sheet1.Cells[i, 10].Text;//'수술종료

                SS1_Sheet1.AddSpanCell(i + 7, 13, 1, 2);
                SS1_Sheet1.Cells[i + 7, 13].Text = ssView1_Sheet1.Cells[i, 11].Text;//'수술종료
                SS1_Sheet1.Cells[i + 7, 15].Text = ssView1_Sheet1.Cells[i, 12].Text;////'의뢰과
                SS1_Sheet1.Cells[i + 7, 16].Text = ssView1_Sheet1.Cells[i, 13].Text;// //'수술후진단명
                SS1_Sheet1.Cells[i + 7, 17].Text = ssView1_Sheet1.Cells[i, 14].Text;////'KTAS
                SS1_Sheet1.Cells[i + 7, 18].Text = ssView1_Sheet1.Cells[i, 16].Text;////'nedis 진료과
            }

            SS1_Sheet1.SetRowHeight(-1, 30);

            //SS1_Sheet1.Cells[7, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = borderWhite;
            //SS1_Sheet1.Cells[7, 0, SS1_Sheet1.RowCount - 2, SS1_Sheet1.ColumnCount - 2].Border = border1;
            //SS1_Sheet1.Cells[7, SS1_Sheet1.ColumnCount - 1, SS1_Sheet1.RowCount - 2, SS1_Sheet1.ColumnCount - 1].Border = border2;
            //SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 2].Border = border3;
            //SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = border4;
        }
    }
}
