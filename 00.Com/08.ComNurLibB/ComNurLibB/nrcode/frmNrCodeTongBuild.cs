using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    public partial class frmNrCodeTongBuild : Form
    {
        string[,] nDATA = new string[3, 61];
        string[,] nDATA2 = new string[31, 7];
        string[,,] nDATA3 = new string[27, 43, 21];
        string[,,] nDATA4 = new string[4, 38, 41];

        const int MB_ICONQUESTION = 32;
        const int MB_YESNO = 4;
        const int MB_DEFBUTTON1 = 0;
        const int IDYES = 6;

        int nJusaCount = 0;
        int nGumuCount = 0;
        string GstrMsgTitle = "";
        string GstrMsgList = "";
        //int GnMsgType = 0;
        //int GnMsgReturn = 0;

        string strSdate = "";
        string strEdate = "";
        string strYYMM = "";

        ComFunc CF = new ComFunc();


        public frmNrCodeTongBuild()
        {
            InitializeComponent();
        }

        private void frmNrCodeTongBuild_Load(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ComFunc.ReadSysDate(clsDB.DbCon);

            cboDate.Items.Clear();
            clsVbfunc.SetCboDate(clsDB.DbCon, cboDate, 14, "", "2");

            READ_LOG();


            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    CODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun= '3' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("주사코드 setting error");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nJusaCount = nJusaCount + 1;
                        nDATA[1, i + 1] = dt.Rows[i]["Code"].ToString().Trim();
                    }
                }


                // 근무형태 setting
                //SQL = "";
                //SQL = SQL + ComNum.VBLF + "SELECT";
                //SQL = SQL + ComNum.VBLF + "    CODE ";
                //SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
                //SQL = SQL + ComNum.VBLF + " WHERE Gubun= '4' ";
                //SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SQL = "";
                SQL = " SELECT NURCODE CODE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_GUNTAECODE A, KOSMOS_PMPA.NUR_CODE B";
                SQL = SQL + ComNum.VBLF + "  WHERE a.NURCODE Is Not Null";
                SQL = SQL + ComNum.VBLF + "    AND A.DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND B.DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND A.NURCODE = B.CODE";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN = '4'";
                SQL = SQL + ComNum.VBLF + "    ORDER BY B.PRINTRANKING";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nGumuCount = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nGumuCount = nGumuCount + 1;
                    nDATA4[1, 1, i + 1] = dt.Rows[i]["Code"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int i, j, k;

            if (chkGun.Checked == false && chkInjection.Checked == false && chkPRN.Checked == false && chkW.Checked == false)
            {
                ComFunc.MsgBox("작업구분을 선택해주세요.");
            }
            txtMessage.Text = "BUILD 중입니다 ....";

            strYYMM = VB.Left(cboDate.SelectedItem.ToString().Trim(), 4) + VB.Mid(cboDate.SelectedItem.ToString().Trim(), 7, 2);
            strSdate = VB.Left(cboDate.SelectedItem.ToString().Trim(), 4) + "-" + VB.Mid(cboDate.SelectedItem.ToString().Trim(), 7, 2) + "-01";
            strEdate = CF.READ_LASTDAY(clsDB.DbCon, strSdate);

            //clear
            for (i = 0; i < 28; i++)
            {
                for (j = 0; j < 7; j++)
                {
                    nDATA2[i, j] = "";
                }
            }

            for (i = 0; i < 43; i++) //주사실
            {
                nDATA[2, i] = "";
            }

            for (i = 0; i < 27; i++)
            {
                for (j = 0; j < 43; j++)
                {
                    for (k = 0; k < 21; k++)
                    {
                        nDATA3[i, j, k] = "";
                    }
                }
            }

            if (chkW.Checked == true)
            {
                txtMessage.Text = "간호부 통계 BUILD중....";
                Ganho_Tong_Build();
                InsertLog(strYYMM, "간호부통계");
            }

            if (chkPRN.Checked == true)
            {
                txtMessage.Text = "외래 간호부 통계 BUILD중....";
                Opd_Tong_Build();
                InsertLog(strYYMM, "외래간호부");
                chkPRN.ForeColor = Color.Blue;
            }

            if (chkInjection.Checked == true)
            {
                txtMessage.Text = "주사실 통계 BUILD중....";
                Jusa_Tong_Build();
                InsertLog(strYYMM, "주사실");
                chkInjection.ForeColor = Color.Blue;
            }

            if (chkGun.Checked == true)
            {
                txtMessage.Text = "근무형태 통계 BUILD중....";
                Gunmu_Tong_Build();
                InsertLog(strYYMM, "근무형태");
                chkGun.ForeColor = Color.Blue;
            }

            ComFunc.MsgBox("빌드 완료");
            txtMessage.Text = "작업완료....";
        }


        private void InsertLog(string strYYMM, string argGubun)
        {

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.NUR_CODE(GUBUN, CODE, NAME) VALUES  ";
                SQL = SQL + ComNum.VBLF + " ('X','" + strYYMM + "','" + argGubun + " : " + "' || TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI')) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        #region //간호부 통계 BUILD
        bool Ganho_Tong_Build()
        {
            bool rtVal = false;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //string[][] str = null;

            int j = 0 ;
            //int k = 0 ;
            //int nCount = 0;
            int nDept = 0;
            int nWard = 0;
            //string strSabun = "";
            //int nGubun = 0;
            string strDEPT = "";
            string strWard = "";
            //string strJik = "";
            //int nIlsu = 0;

            string cYYMM = "";
            string cWardCode = "";
            string cDeptCode = "";
            string cIlsu = "";
            string cTotbed = "";
            //string cTotal = "";
            string cIpInwon = "";
            string cJewon = "";
            string cTewon = "";
            string cTrans = "";
            string cOp = "";
            string cDied = "";
            string cKeep = "";
            string cDelivery = "";
            string cGoOut = "";
            string cNomalBaByM = "";
            string cNomalBaByF = "";
            string cDelivery2 = "";
            string cDelivery3 = "";


            // 병동 통계 Build 여부 Check            
            if (NUR_TONG1_BUILD_CHECK() == false)
                return rtVal;

            //먼저 build 된 것을 삭제
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ";
                SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_TONG1 ";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM = '" + strYYMM + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }


            // Debug.Rpint SQLRET
            // BED 수 통계 build
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT CODE, JIK ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_CODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '7' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월에는 build할 BED수 데이타가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strWard = dt.Rows[i]["Code"].ToString().Trim();
                        nWard = Ward_Gubun(strWard);
                        nDATA3[0, nWard, 1] = (VB.Val(nDATA3[0, nWard, 1]) + VB.Val(dt.Rows[i]["JIK"].ToString().Trim())).ToString();
                    }
                }
                dt.Dispose();
                dt = null;


                //'병동별 입퇴원,재원 통계
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE, WARDCODE, CNT11+CNT12 CNT1, CNT21+CNT22 CNT2, CNT31+CNT32 CNT3, ";
                SQL = SQL + ComNum.VBLF + "        CNT51, CNT52, CNT51+CNT52 CNT5 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_JEWON ";
                SQL = SQL + ComNum.VBLF + "WHERE ACTDATE >=TO_DATE('" + strSdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN ('MD','GS','OG','GY','PD','OS','NS','CS','NP','EN','OT','UR',";
                SQL = SQL + ComNum.VBLF + "                    'DM','DT','NB','IQ','DB', 'NE',";
                SQL = SQL + ComNum.VBLF + "                    'MC','ME','MG','MN','MP','MR','MI','MO', 'HU')";  //2019-01-08 박창욱 : 'HU' 추가
                SQL = SQL + ComNum.VBLF + " AND WARDCODE <> 'ER'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월에는 build할 병동 입/퇴/재원 데이타가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDEPT = dt.Rows[i]["Deptcode"].ToString().Trim();
                        nDept = Dept_Gubun(strDEPT);
                        strWard = dt.Rows[i]["Wardcode"].ToString().Trim();

                        if (string.Compare(strYYMM, "200504") >= 0 && (string.Compare(strYYMM, "200912") < 0))
                        {
                            if (strWard == "NR")
                                strWard = "ND";
                        }
                        else if (string.Compare(strYYMM, "200407") >= 0 && (string.Compare(strYYMM, "200504") < 0))
                        {
                            if (strWard == "NR")
                                strWard = "ND";
                            if (strWard == "DR")
                                strWard = "55";
                        }

                        nWard = Ward_Gubun(strWard);

                        if (strDEPT == "NB")
                        {
                            //'정상아 남
                            nDATA3[nDept, nWard, 12] = (VB.Val(nDATA3[nDept, nWard, 12]) + VB.Val(dt.Rows[i]["Cnt51"].ToString())).ToString();
                            //'정상아 여
                            nDATA3[nDept, nWard, 13] = (VB.Val(nDATA3[nDept, nWard, 13]) + VB.Val(dt.Rows[i]["Cnt52"].ToString())).ToString();
                        }

                        //'입원
                        nDATA3[nDept, nWard, 2] = (VB.Val(nDATA3[nDept, nWard, 2]) + VB.Val(dt.Rows[i]["Cnt1"].ToString())).ToString();
                        //'재원
                        nDATA3[nDept, nWard, 3] = (VB.Val(nDATA3[nDept, nWard, 3]) + VB.Val(dt.Rows[i]["Cnt5"].ToString())).ToString();
                        //'퇴원
                        nDATA3[nDept, nWard, 4] = (VB.Val(nDATA3[nDept, nWard, 4]) + VB.Val(dt.Rows[i]["Cnt2"].ToString())).ToString();
                        //'이실
                        nDATA3[nDept, nWard, 5] = (VB.Val(nDATA3[nDept, nWard, 5]) + VB.Val(dt.Rows[i]["Cnt3"].ToString())).ToString();
                    }
                }
                dt.Dispose();
                dt = null;


                // 수술, 사망, KEEP, 정상분만, 이상분만, 제왕절개 통계 BUILD
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT WARDCODE, SUM(QTY1+QTY2+QTY3+QTY4) QTY , CODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_INOUT ";
                SQL = SQL + ComNum.VBLF + " WHERE CODE IN ('05','06','07','08','12','13') ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDATE >=TO_DATE('" + strSdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND ACTDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND WARDCODE <> 'EM' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY WARDCODE,CODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월에는 build할 수술,사망, keep,delivery 데이타가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strWard = dt.Rows[i]["Wardcode"].ToString().Trim();
                        nWard = Ward_Gubun(strWard);

                        switch (dt.Rows[i]["Code"].ToString().Trim())
                        {
                            case "05":
                                //'수술
                                nDATA3[0, nWard, 6] = (VB.Val(nDATA3[0, nWard, 6]) + VB.Val(dt.Rows[i]["Qty"].ToString())).ToString();
                                break;
                            case "06":
                                //'사망
                                nDATA3[0, nWard, 7] = (VB.Val(nDATA3[0, nWard, 7]) + VB.Val(dt.Rows[i]["Qty"].ToString())).ToString();
                                break;
                            case "07":
                                //'KEEP
                                nDATA3[0, nWard, 8] = (VB.Val(nDATA3[0, nWard, 8]) + VB.Val(dt.Rows[i]["Qty"].ToString())).ToString();
                                break;
                            case "08":
                                //'정상분만
                                nDATA3[0, nWard, 9] = (VB.Val(nDATA3[0, nWard, 9]) + VB.Val(dt.Rows[i]["Qty"].ToString())).ToString();
                                break;
                            case "12":
                                //'이상분만
                                nDATA3[0, nWard, 10] = (VB.Val(nDATA3[0, nWard, 10]) + VB.Val(dt.Rows[i]["Qty"].ToString())).ToString();
                                break;
                            case "13":
                                //'제왕절개
                                nDATA3[0, nWard, 11] = (VB.Val(nDATA3[0, nWard, 11]) + VB.Val(dt.Rows[i]["Qty" + ""].ToString())).ToString();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }



            for (i = 0; i < 27; i++)
            {
                switch (i)
                {
                    case 0:
                        cDeptCode = "";
                        break;
                    case 1:
                        cDeptCode = "MD";
                        break;
                    case 2:
                        cDeptCode = "GD";
                        break;
                    case 3:
                        cDeptCode = "OG";
                        break;
                    case 4:
                        cDeptCode = "PD";
                        break;
                    case 5:
                        cDeptCode = "OS";
                        break;
                    case 6:
                        cDeptCode = "NS";
                        break;
                    case 7:
                        cDeptCode = "CS";
                        break;
                    case 8:
                        cDeptCode = "NP";
                        break;
                    case 9:
                        cDeptCode = "EN";
                        break;
                    case 10:
                        cDeptCode = "OT";
                        break;
                    case 11:
                        cDeptCode = "UR";
                        break;
                    case 12:
                        cDeptCode = "DM";
                        break;
                    case 13:
                        cDeptCode = "DT";
                        break;
                    case 14:
                        cDeptCode = "IQ";
                        break;
                    case 15:
                        cDeptCode = "DB";
                        break;
                    case 16:
                        cDeptCode = "PC";
                        break;
                    case 17:
                        cDeptCode = "NE";
                        break;
                    case 18:
                        cDeptCode = "MC";
                        break;
                    case 19:
                        cDeptCode = "ME";
                        break;
                    case 20:
                        cDeptCode = "MG";
                        break;
                    case 21:
                        cDeptCode = "MN";
                        break;
                    case 22:
                        cDeptCode = "MP";
                        break;
                    case 23:
                        cDeptCode = "MR";
                        break;
                    case 24:
                        cDeptCode = "MI";
                        break;
                    case 25:
                        cDeptCode = "HU";
                        break;
                    case 26:
                        cDeptCode = "MO";
                        break;
                    default:
                        break;
                }

                for (j = 1; j < 17; j++)
                {
                    cYYMM = strYYMM.Trim();

                    switch (j)
                    {
                        case 1:
                            cWardCode = "33";
                            break;
                        case 2:
                            cWardCode = "35";
                            break;
                        case 3:
                            cWardCode = "40";
                            break;
                        case 4:
                            cWardCode = "4H";
                            break;
                        case 5:
                            cWardCode = "50";
                            break;
                        case 6:
                            cWardCode = "53";
                            break;
                        case 7:
                            cWardCode = "55";
                            break;
                        case 8:
                            cWardCode = "60";
                            break;
                        case 9:
                            cWardCode = "63";
                            break;
                        case 10:
                            cWardCode = "65";
                            break;
                        case 11:
                            cWardCode = "70";
                            break;
                        case 12:
                            cWardCode = "73";
                            break;
                        case 13:
                            cWardCode = "75";
                            break;
                        case 14:
                            cWardCode = "80";
                            break;
                        case 15:
                            cWardCode = "83";
                            break;
                        case 16:
                            cWardCode = "NR";
                            break;

                        default:
                            break;
                    }

                    //nIlsu = CF.DATE_ILSU(clsDB.DbCon, strEdate, strSdate);
                    //cIlsu = Convert.ToString(nIlsu);
                    cIlsu = VB.Right(strEdate, 2);
                    cTotbed = Convert.ToString(VB.Val(nDATA3[0, j, 1]));
                    cIpInwon = Convert.ToString(VB.Val(nDATA3[i, j, 2]));
                    cJewon = nDATA3[i, j, 3].Trim();
                    cTewon = Convert.ToString(VB.Val(nDATA3[i, j, 4]));
                    cTrans = nDATA3[i, j, 5].Trim();

                    cOp = nDATA3[i, j, 6].Trim();
                    cDied = nDATA3[i, j, 7].Trim();
                    cKeep = nDATA3[i, j, 8].Trim();
                    cDelivery = nDATA3[i, j, 9].Trim();
                    cDelivery2 = nDATA3[i, j, 10].Trim();
                    cDelivery3 = nDATA3[i, j, 11].Trim();
                    cGoOut = "0";
                    cNomalBaByM = nDATA3[i, j, 12].Trim();
                    cNomalBaByF = nDATA3[i, j, 13].Trim();

                    Cursor.Current = Cursors.WaitCursor;
                    clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "INSERT INTO ";
                        SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_TONG1 ";
                        SQL = SQL + ComNum.VBLF + " (YYMM, WARDCODE, DEPTCODE, ILSU, TOTBED, IPINWON, JEWON, TEWON, TRANS, ";
                        SQL = SQL + ComNum.VBLF + " OP, DIED, KEEP, DELIVERY, GOOUT, NOMALBABYM, NOMALBABYF,DELIVERY2,DELIVERY3) ";
                        SQL = SQL + ComNum.VBLF + " VALUES ('" + cYYMM + "','" + cWardCode + "','" + cDeptCode + "',";
                        SQL = SQL + ComNum.VBLF + "         '" + cIlsu + "','" + cTotbed + "','" + cIpInwon + "',";
                        SQL = SQL + ComNum.VBLF + "         '" + cJewon + "','" + cTewon + "', '" + cTrans + "',";
                        SQL = SQL + ComNum.VBLF + "         '" + cOp + "','" + cDied + "','" + cKeep + "','" + cDelivery + "',";
                        SQL = SQL + ComNum.VBLF + "         '" + cGoOut + "','" + cNomalBaByM + "','" + cNomalBaByF + "',";
                        SQL = SQL + ComNum.VBLF + "         '" + cDelivery2 + "','" + cDelivery3 + "')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }

                        //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                        clsDB.setCommitTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(ex.Message);
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                } //for j
            } //for i

            chkW.ForeColor = Color.Blue;

            rtVal = true;
            return rtVal;
        }
        #endregion

        #region //외래 간호부 통계 BUILD
        private bool Opd_Tong_Build()
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            int i = 0;
            //int j = 0;
            //int k = 0;
            //int nCount = 0;
            int nDept = 0;
            string strSabun = "";
            int nGubun = 0;

            string strDeptcode = "";
            string strSinInwon = "";
            string strGuInwon = "";
            string strRnInwon = "";
            string strNaInwon = "";
            string strDnnaInwon = "";
            string strEtcInwon = "";

            if (NUR_TONG2_BUILD_CHECK() == false)
                return rtVal;

            //먼저 build 된 것을 삭제
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ";
                SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_TONG2 ";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM = '" + strYYMM + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }


            try
            {
                //'외래 통계 build                
                SQL = "SELECT DEPT, DRCODE, ANSABUN1, ANSABUN2, SININWON, GUINWON";
                SQL = SQL + ComNum.VBLF + " FROM NUR_OPDILJI";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >=TO_DATE('" + strSdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월에는 build할 BED수 데이타가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (VB.UCase(dt.Rows[i]["Dept"].ToString().Trim()))
                        {
                            case "MD":
                                nDept = 1;
                                break;
                            case "GS":
                                nDept = 2;
                                break;
                            case "OG":
                                nDept = 3;  //'~j
                                break;
                            case "PD":
                                nDept = 4;
                                break;
                            case "OS":
                                nDept = 5;
                                break;
                            case "NS":
                                nDept = 6;
                                break;
                            case "CS":
                                nDept = 7;
                                break;
                            case "NE":
                                nDept = 8;
                                break;
                            case "NP":
                                nDept = 9;
                                break;
                            case "EN":
                                nDept = 10;
                                break;
                            case "OT":
                                nDept = 11;
                                break;
                            case "UR":
                                nDept = 12;
                                break;
                            case "DM":
                                nDept = 13;
                                break;
                            case "DT":
                                nDept = 14;    //'~j
                                break;
                            case "PC":
                                nDept = 15;    //'~j
                                break;
                            case "JU":
                                nDept = 16; //'주사실
                                break;
                            case "SI":
                                nDept = 17; //'심전도실
                                break;
                            case "ED":
                                nDept = 18; //'내시경
                                break;
                            case "RM":
                                nDept = 19; //'재활의학
                                break;
                            case "MC":
                                nDept = 20;
                                break;
                            case "ME":
                                nDept = 21;
                                break;
                            case "MG":
                                nDept = 22;
                                break;
                            case "MN":
                                nDept = 23;
                                break;
                            case "MP":
                                nDept = 24;
                                break;
                            case "MR":
                                nDept = 25;
                                break;
                            case "MI":
                                nDept = 26;
                                break;
                            case "MO":
                                nDept = 27;
                                break;
                            case "HU":
                                nDept = 28;
                                break;
                        }

                        nDATA2[nDept, 1] = Convert.ToString(VB.Val(nDATA2[nDept, 1]) + VB.Val(dt.Rows[i]["SinInwon"].ToString().Trim()));
                        nDATA2[nDept, 2] = Convert.ToString(VB.Val(nDATA2[nDept, 2]) + VB.Val(dt.Rows[i]["GuInwon"].ToString().Trim()));

                        strSabun = dt.Rows[i]["Ansabun1"].ToString().Trim();
                        if (VB.Len(VB.Trim(strSabun)) != 0)
                        {
                            nGubun = RN_AN_CHECK(strSabun);
                            nDATA2[nDept, nGubun] = Convert.ToString(VB.Val(nDATA2[nDept, nGubun]) + 1);
                            strSabun = "";
                        }

                        strSabun = dt.Rows[i]["Ansabun2"].ToString().Trim();
                        if (VB.Len(VB.Trim(strSabun)) != 0)
                        {
                            nGubun = RN_AN_CHECK(strSabun);
                            nDATA2[nDept, nGubun] = Convert.ToString(VB.Val(nDATA2[nDept, nGubun]) + 1);
                            strSabun = "";
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }



            for (i = 1; i <= 26; i++)
            {
                strYYMM = VB.Trim(strYYMM);

                switch (i)
                {
                    case 1:
                        strDeptcode = "MD";
                        break;
                    case 2:
                        strDeptcode = "GS";
                        break;
                    case 3:
                        strDeptcode = "OG";    //'~j
                        break;
                    case 4:
                        strDeptcode = "PD";
                        break;
                    case 5:
                        strDeptcode = "OS";
                        break;
                    case 6:
                        strDeptcode = "NS";
                        break;
                    case 7:
                        strDeptcode = "CS";
                        break;
                    case 8:
                        strDeptcode = "NE";
                        break;
                    case 9:
                        strDeptcode = "NP";
                        break;
                    case 10:
                        strDeptcode = "EN";
                        break;
                    case 11:
                        strDeptcode = "OT";
                        break;
                    case 12:
                        strDeptcode = "UR";
                        break;
                    case 13:
                        strDeptcode = "DM";
                        break;
                    case 14:
                        strDeptcode = "DT";    //'~j
                        break;
                    case 15:
                        strDeptcode = "PC";    //'~j
                        break;
                    case 16:
                        strDeptcode = "JU";
                        break;
                    case 17:
                        strDeptcode = "SI";
                        break;
                    case 18:
                        strDeptcode = "ED";     //'내시경
                        break;
                    case 19:
                        strDeptcode = "RM";
                        break;
                    case 20:
                        strDeptcode = "MC";
                        break;
                    case 21:
                        strDeptcode = "ME";
                        break;
                    case 22:
                        strDeptcode = "MG";
                        break;
                    case 23:
                        strDeptcode = "MN";
                        break;
                    case 24:
                        strDeptcode = "MP";
                        break;
                    case 25:
                        strDeptcode = "MR";
                        break;
                    case 26:
                        strDeptcode = "MI";
                        break;
                    case 27:
                        strDeptcode = "MO";
                        break;
                    case 28:
                        strDeptcode = "HU";
                        break;
                }

                strSinInwon = VB.Trim(nDATA2[i, 1]);
                strGuInwon = VB.Trim(nDATA2[i, 2]);
                strRnInwon = VB.Trim(nDATA2[i, 3]);
                strNaInwon = VB.Trim(nDATA2[i, 4]);
                strDnnaInwon = VB.Trim(nDATA2[i, 5]);
                strEtcInwon = VB.Trim(nDATA2[i, 6]);


                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "INSERT INTO NUR_TONG2 (YYMM, DEPTCODE, SININWON, GUINWON,";
                    SQL = SQL + ComNum.VBLF + " RNINWON, NAINWON, DNNAINWON, ETCINWON)";
                    SQL = SQL + ComNum.VBLF + " VALUES ('" + strYYMM + "', '" + strDeptcode + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strSinInwon + "', '" + strGuInwon + "', '" + strRnInwon + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strNaInwon + "', '" + strDnnaInwon + "', '" + strEtcInwon + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    rtVal = true;
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

            chkPRN.ForeColor = Color.FromArgb(0, 0, 255);

            return rtVal;
        }
        #endregion

        #region //주사실 통계 BUILD
        private bool Jusa_Tong_Build()
        {
            bool rtVal = false;
            int i = 0;
            int j = 0;
            //int k = 0;
            int nCount = 0;

            string strCode = "";
            string strQty = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (NUR_TONG3_BUILD_CHECK() == false)
                return rtVal;

            //'먼저 build 된것을 삭제
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "DELETE NUR_TONG3 WHERE YYMM='" + strYYMM + "' AND WARDCODE = 'JU'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }


            //'주사실 통계 build
            try
            {
                SQL = "SELECT CODE, QTY";
                SQL = SQL + ComNum.VBLF + " FROM NUR_JUSASIL";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >=TO_DATE('" + strSdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN ='2'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월에는 build할 주사실 데이타가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        for (j = 0; j < nJusaCount; j++)
                        {
                            if (nDATA[1, j] == dt.Rows[i]["Code"].ToString().Trim())
                            {
                                nCount = j;
                                nDATA[2, j] = Convert.ToString(VB.Val(nDATA[2, j]) + VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                                break;
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


            for (i = 1; i <= nJusaCount; i++)
            {
                strYYMM = VB.Trim(strYYMM);
                strCode = VB.Trim(nDATA[1, i]);
                strQty = VB.Trim(nDATA[2, i]);

                if (VB.Len(nDATA[2, i]) != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {
                        SQL = "INSERT INTO NUR_TONG3 (YYMM, WARDCODE, CODE, QTY1)";
                        SQL = SQL + ComNum.VBLF + " VALUES ('" + strYYMM + "', 'JU', '" + strCode + "', '" + strQty + "' )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
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
            }

            chkInjection.ForeColor = Color.FromArgb(0, 0, 255);
            txtMessage.Text = "작업완료....";
            rtVal = true;
            return rtVal;
        }
        #endregion

        #region //근무형태 통계 BUILD
        private bool Gunmu_Tong_Build()
        {
            bool rtVal = false;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int j, K = 0;
            //int nCount = 0;
            int nDept = 0;
            //string strSabun = "";
            //int nGubun = 0;
            string strDEPT = "";
            string strJik = "";
            //int nIlsu = 0;
            string[] strCode = new string[31];
            int nCCount = 0;

            string cYYMM = "";
            string cWardCode = "";
            string cCode = "";
            string cQty1 = "";
            string cQty2 = "";

            if (NUR_TONG31_BUILD_CHECK() == false)
                return rtVal;

            //'먼저 build 된것을 삭제
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "DELETE NUR_TONG3 WHERE YYMM ='" + strYYMM + "' AND WARDCODE <> 'JU'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }


            //'병동별 schedule 통계 build
            try
            {
                SQL = "SELECT WARDCODE, JIKCODE, SCHEDULE";
                SQL = SQL + ComNum.VBLF + " FROM NUR_SCHEDULE1";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월에는 build할 주사실 데이타가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strDEPT = dt.Rows[i]["Wardcode"].ToString().Trim();
                    nDept = Dept1_Gubun(strDEPT);
                    strJik = dt.Rows[i]["Jikcode"].ToString().Trim();


                    for (j = 1; j <= 31; j++)
                    {
                        strCode[j] = "";
                    }


                    nCCount = 1;
                    for (j = 1; j <= 31; j++)
                    {
                        strCode[j] = VB.Trim(ComFunc.MidH(dt.Rows[i]["Schedule"].ToString().Trim(), nCCount, 4));

                        for (K = 1; K <= nGumuCount; K++)
                        {
                            if (nDATA4[1, 1, K] == strCode[j])
                                break;
                        }

                        if (strJik == "04" || strJik == "31" || strJik == "32" || strJik == "33" || strJik == "34" || strJik == "38" || strJik == "61" || strJik == "64")
                        {
                            nDATA4[2, nDept, K] = Convert.ToString(VB.Val(nDATA4[2, nDept, K]) + 1);  //'간호사
                        }
                        else
                        {
                            nDATA4[3, nDept, K] = Convert.ToString(VB.Val(nDATA4[3, nDept, K]) + 1);  //'간호조무사
                        }

                        nCCount = nCCount + 4;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }



            for (i = 1; i <= 30; i++)
            {
                cYYMM = VB.Trim(strYYMM);

                if (string.Compare(strYYMM, "200912") >= 0)
                {
                    switch (i)
                    {
                        case 1:
                            cWardCode = "3A";
                            break;
                        case 2:
                            cWardCode = "4W";
                            break;
                        case 3:
                            cWardCode = "4A";
                            break;
                        case 4:
                            cWardCode = "5W";
                            break;
                        case 5:
                            cWardCode = "6W";
                            break;
                        case 6:
                            cWardCode = "7W";
                            break;
                        case 7:
                            cWardCode = "8W";
                            break;
                        case 8:
                            cWardCode = "MICU";
                            break;
                        case 9:
                            cWardCode = "SICU";
                            break;
                        case 10:
                            cWardCode = "ER";   //'~j
                            break;
                        case 11:
                            cWardCode = "NR";
                            break;
                        case 12:
                            cWardCode = "HD";
                            break;
                        case 13:
                            cWardCode = "CSR";
                            break;
                        case 14:
                            cWardCode = "OPD";
                            break;
                        case 15:
                            cWardCode = "OR";
                            break;
                        case 16:
                            cWardCode = "GAN";
                            break;
                        case 17:
                            cWardCode = "HU";
                            break;
                        case 18:
                            cWardCode = "6A";
                            break;
                        case 19:
                            cWardCode = "3W";
                            break;
                        case 20:
                            cWardCode = "3C";
                            break;
                        case 21:
                            cWardCode = "32";
                            break;
                        case 22:
                            cWardCode = "52";
                            break;
                        case 23:
                            cWardCode = "53";
                            break;
                        case 24:
                            cWardCode = "62";
                            break;
                        case 25:
                            cWardCode = "63";
                            break;
                        case 26:
                            cWardCode = "72";
                            break;
                        case 27:
                            cWardCode = "73";
                            break;
                        case 28:
                            cWardCode = "51";
                            break;
                        case 29:
                            cWardCode = "41";
                            break;
                        case 30:
                            cWardCode = "DS";
                            break;
                        case 31:
                            cWardCode = "71";
                            break;
                        case 32:
                            cWardCode = "81";
                            break;
                        case 33:
                            cWardCode = "33";
                            break;
                        case 34:
                            cWardCode = "35";
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 1:
                            cWardCode = "3A";
                            break;
                        case 2:
                            cWardCode = "3B";
                            break;
                        case 3:
                            cWardCode = "4A";
                            break;
                        case 4:
                            cWardCode = "5W";
                            break;
                        case 5:
                            cWardCode = "6W";
                            break;
                        case 6:
                            cWardCode = "7W";
                            break;
                        case 7:
                            cWardCode = "8W";
                            break;
                        case 8:
                            cWardCode = "MICU";
                            break;
                        case 9:
                            cWardCode = "SICU";
                            break;
                        case 10:
                            cWardCode = "ER";   //'~j
                            break;
                        case 11:
                            cWardCode = "ND";
                            break;
                        case 12:
                            cWardCode = "HD";
                            break;
                        case 13:
                            cWardCode = "CSR";
                            break;
                        case 14:
                            cWardCode = "OPD";
                            break;
                        case 15:
                            cWardCode = "OR";
                            break;
                        case 16:
                            cWardCode = "GAN";
                            break;
                        case 17:
                            cWardCode = "HU";
                            break;
                    }
                }


                for (j = 1; j <= nGumuCount; j++)
                {
                    cCode = VB.Trim(nDATA4[1, 1, j]);
                    cQty1 = Convert.ToString(VB.Val(VB.Trim(nDATA4[2, i, j])));
                    cQty2 = Convert.ToString(VB.Val(VB.Trim(nDATA4[3, i, j])));


                    Cursor.Current = Cursors.WaitCursor;
                    clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {
                        SQL = "INSERT INTO NUR_TONG3 (YYMM, WARDCODE,CODE, QTY1, QTY2)";
                        SQL = SQL + ComNum.VBLF + " VALUES ('" + cYYMM + "','" + cWardCode + "','" + cCode + "',";
                        SQL = SQL + ComNum.VBLF + "        '" + cQty1 + "','" + cQty2 + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
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
            }

            chkGun.ForeColor = Color.FromArgb(0, 0, 255);

            rtVal = true;
            return rtVal;
        }
        #endregion


        //'병동통계 Build 여부 Check        
        private bool NUR_TONG1_BUILD_CHECK()
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + " * FROM " + ComNum.DB_PMPA + "NUR_TONG1";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    rtnVal = true;
                    return rtnVal;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


            GstrMsgList = "이미 당월의 간호부 통계자료가 형성되어 있읍니다" + ComNum.VBLF;
            GstrMsgList = GstrMsgList + "다시 형성작업을 하겠읍니까?";

            if (ComFunc.MsgBoxQ(GstrMsgList, "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        //'외래통계 Build여부 Check
        private bool NUR_TONG2_BUILD_CHECK()
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT * FROM NUR_TONG2 ";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM    = '" + strYYMM + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            GstrMsgTitle = "확 인";
            GstrMsgList = "이미 당월의 외래 통계자료가 형성되어 있읍니다" + ComNum.VBLF;
            GstrMsgList = GstrMsgList + "다시 형성작업을 하겠읍니까? ";

            if (ComFunc.MsgBoxQ(GstrMsgList, GstrMsgTitle, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        //'주사통계 Build여부 Check
        private bool NUR_TONG3_BUILD_CHECK()
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT * FROM NUR_TONG3 ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM    = '" + strYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'JU' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            GstrMsgTitle = "확 인";
            GstrMsgList = "이미 당월의 주사실 통계자료가 형성되어 있습니다" + ComNum.VBLF;
            GstrMsgList = GstrMsgList + "다시 형성작업을 하겠읍니까? ";

            if (ComFunc.MsgBoxQ(GstrMsgList, GstrMsgTitle, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        //'근무통계 Build여부 Check
        private bool NUR_TONG31_BUILD_CHECK()
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT * FROM NUR_TONG3 ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM    = '" + strYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE <> 'JU'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            GstrMsgTitle = "확 인";
            GstrMsgList = "이미 당월의 근무형태 통계자료가 형성되어 있습니다" + ComNum.VBLF;
            GstrMsgList = GstrMsgList + "다시 형성작업을 하겠읍니까? ";

            if (ComFunc.MsgBoxQ(GstrMsgList, GstrMsgTitle, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                rtnVal = true;
            }

            return rtnVal;
        }


        private int Ward_Gubun(string strWard)
        {
            int nWard = 0;

            switch (VB.UCase(VB.Trim(strWard)))
            {
                //As-Is Ward

                //case "2W":
                //    nWard = 1;
                //    break;
                //case "3A":
                //    nWard = 2;
                //    break;
                //case "3B":
                //    nWard = 3;
                //    break;
                //case "4A":
                //    nWard = 4;
                //    break;
                //case "5W":
                //    nWard = 5;
                //    break;
                //case "6W":
                //    nWard = 6;
                //    break;
                //case "7W":
                //    nWard = 7;
                //    break;
                //case "8W":
                //    nWard = 8;
                //    break;
                //case "MICU":
                //    nWard = 9;
                //    break;
                //case "SICU":
                //    nWard = 10;
                //    break;
                //case "ER":
                //    nWard = 11;     //'~j
                //    break;
                //case "DR":
                //    nWard = 12;
                //    break;
                //case "NR":
                //    nWard = 13;
                //    break;
                //case "HD":
                //    nWard = 14;
                //    break;
                //case "HU":
                //    nWard = 15;
                //    break;
                //case "ND":
                //    nWard = 16;
                //    break;
                //case "3C":
                //    nWard = 17;
                //    break;
                //case "3W":
                //    nWard = 18;
                //    break;
                //case "4W":
                //    nWard = 19;
                //    break;
                //case "6A":
                //    nWard = 20;
                //    break;
                //case "32":
                //    nWard = 21;
                //    break;
                //case "52":
                //    nWard = 22;
                //    break;
                //case "53":
                //    nWard = 23;
                //    break;
                //case "62":
                //    nWard = 24;
                //    break;
                //case "63":
                //    nWard = 25;
                //    break;
                //case "72":
                //    nWard = 26;
                //    break;
                //case "73":
                //    nWard = 27;
                //    break;
                //case "51":
                //    nWard = 28;
                //    break;
                //case "41":
                //    nWard = 29;
                //    break;
                //case "DS":
                //    nWard = 30;
                //    break;
                //case "71":
                //    nWard = 31;
                //    break;
                //case "81":
                //    nWard = 32;
                //    break;
                //case "50":
                //    nWard = 33;
                //    break;
                //case "60":
                //    nWard = 34;
                //    break;
                //case "70":
                //    nWard = 35;
                //    break;
                //case "80":
                //    nWard = 36;
                //    break;
                //case "55":
                //    nWard = 37;
                //    break;
                //case "65":
                //    nWard = 38;
                //    break;
                //case "75":
                //    nWard = 39;
                //    break;
                //case "33":
                //    nWard = 40;
                //    break;
                //case "35":
                //    nWard = 41;
                //    break;
                //default:
                //    nWard = 0;
                //    break;


                //To-Be Ward
                case "33":
                    nWard = 1;
                    break;
                case "35":
                    nWard = 2;
                    break;
                case "40":
                    nWard = 3;
                    break;
                case "4H":
                    nWard = 4;
                    break;
                case "50":
                    nWard = 5;
                    break;
                case "53":
                    nWard = 6;
                    break;
                case "55":
                    nWard = 7;
                    break;
                case "60":
                    nWard = 8;
                    break;
                case "63":
                    nWard = 9;
                    break;
                case "65":
                    nWard = 10;
                    break;
                case "70":
                    nWard = 11;
                    break;
                case "73":
                    nWard = 12;
                    break;
                case "75":
                    nWard = 13;
                    break;
                case "80":
                    nWard = 14;
                    break;
                case "83":
                    nWard = 15;
                    break;
                case "NR":
                    nWard = 16;
                    break;

                default:
                    nWard = 0;
                    break;
            }

            return nWard;
        }

        private int Dept_Gubun(string strDEPT)
        {
            int nDept = 0;

            switch (VB.UCase(VB.Trim(strDEPT)))
            {
                case "MD":
                    nDept = 1;
                    break;
                case "GS":
                    nDept = 2;
                    break;
                case "OG":
                case "GY":
                    nDept = 3;       //'~j
                    break;
                case "PD":
                    nDept = 4;
                    break;
                case "OS":
                    nDept = 5;
                    break;
                case "NS":
                    nDept = 6;
                    break;
                case "CS":
                    nDept = 7;
                    break;
                case "NP":
                    nDept = 8;
                    break;
                case "EN":
                    nDept = 9;
                    break;
                case "OT":
                    nDept = 10;
                    break;
                case "UR":
                    nDept = 11;
                    break;
                case "DM":
                    nDept = 12;
                    break;
                case "DT":
                    nDept = 13;     //'~j
                    break;
                case "IQ":
                    nDept = 14;
                    break;
                case "DB":
                    nDept = 15;
                    break;
                case "PC":
                    nDept = 16;      //'~j
                    break;
                case "NE":
                    nDept = 17;
                    break;
                case "MC":
                    nDept = 18;
                    break;
                case "ME":
                    nDept = 19;
                    break;
                case "MG":
                    nDept = 20;
                    break;
                case "MN":
                    nDept = 21;
                    break;
                case "MP":
                    nDept = 22;
                    break;
                case "MR":
                    nDept = 23;
                    break;
                case "MI":
                    nDept = 24;
                    break;
                case "HU":
                    nDept = 25;
                    break;
                case "MO":
                    nDept = 26;
                    break;
                default:
                    nDept = 0;
                    break;
            }
            return nDept;
        }

        private int RN_AN_CHECK(string strSabun)
        {
            int rtnVal = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT JIK FROM KOSMOS_ADM.INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN IN ('" + strSabun + "')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    rtnVal = 6;
                }
                else
                {
                    if (dt.Rows[0]["Jik"].ToString().Trim() == "57")
                    {
                        rtnVal = 5; //'치위생사
                    }
                    if (dt.Rows[0]["Jik"].ToString().Trim() == "36" || dt.Rows[0]["Jik"].ToString().Trim() == "37"
                        || dt.Rows[0]["Jik"].ToString().Trim() == "39" || dt.Rows[0]["Jik"].ToString().Trim() == "53")
                    {
                        rtnVal = 4;
                    }
                    else
                    {
                        rtnVal = 3;
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return rtnVal;
        }

        private int Dept1_Gubun(string strDEPT)
        {
            int nDept = 0;

            switch (VB.UCase(VB.Trim(strDEPT)))
            {
                case "3A":
                    nDept = 1;
                    break;
                case "3B":
                case "4W":
                    nDept = 2;
                    break;
                case "4A":
                    nDept = 3;
                    break;
                case "5W":
                    nDept = 4;
                    break;
                case "6W":
                    nDept = 5;
                    break;
                case "7W":
                    nDept = 6;
                    break;
                case "8W":
                    nDept = 7;
                    break;
                case "MICU":
                    nDept = 8;
                    break;
                case "SICU":
                    nDept = 9;
                    break;
                case "ER":
                    nDept = 10;    //'~j
                    break;
                case "NR":
                case "ND":
                    nDept = 11;
                    break;
                case "HD":
                    nDept = 12;
                    break;
                case "CSR":
                    nDept = 13;
                    break;
                case "OPD":
                    nDept = 14;
                    break;
                case "OR":
                    nDept = 15;
                    break;
                case "GAN":
                    nDept = 16;
                    break;
                case "HU":
                    nDept = 17;
                    break;
                case "3C":
                case "6A":
                case "DR":
                    nDept = 18;
                    break;
                case "3W":
                    nDept = 19;
                    break;
                //case "3C":
                //    nDept = 20;
                //    break;
                case "32":
                    nDept = 21;
                    break;
                case "52":
                    nDept = 22;
                    break;
                case "53":
                    nDept = 23;
                    break;
                case "62":
                    nDept = 24;
                    break;
                case "63":
                    nDept = 25;
                    break;
                case "72":
                    nDept = 26;
                    break;
                case "73":
                    nDept = 27;
                    break;
                case "51":
                    nDept = 28;
                    break;
                case "41":
                    nDept = 29;
                    break;
                case "DS":
                    nDept = 30;
                    break;
                case "71":
                    nDept = 31;
                    break;
                case "81":
                    nDept = 32;
                    break;
                case "33":
                    nDept = 33;
                    break;
                case "35":
                    nDept = 34;
                    break;
                default:
                    nDept = 0;
                    break;
            }

            return nDept;
        }

        private void cboDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                chkW.Focus();
            }
        }

        private void cboDate_Enter(object sender, EventArgs e)
        {
            chkW.Checked = false;
            chkPRN.Checked = false;
            chkInjection.Checked = false;
            chkGun.Checked = false;
            txtMessage.Text = "";
            chkW.ForeColor = Color.FromArgb(0, 0, 0);
            chkPRN.ForeColor = Color.FromArgb(0, 0, 0);
            chkInjection.ForeColor = Color.FromArgb(0, 0, 0);
            chkGun.ForeColor = Color.FromArgb(0, 0, 0);
        }

        private void cboDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            READ_LOG();
        }

        private void READ_LOG()
        {
            int i = 0;
            //int rtnVal = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssLog_Sheet1.RowCount = 0;

            try
            {
                SQL = "SELECT CODE, NAME FROM KOSMOS_PMPA.NUR_CODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'X' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + VB.Left(cboDate.SelectedItem.ToString().Trim(), 4) + VB.Mid(cboDate.SelectedItem.ToString().Trim(), 7, 2) + "'";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssLog_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssLog_Sheet1.Cells[i, 0].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssLog_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return;
        }
    }
}
