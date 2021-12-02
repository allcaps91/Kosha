using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-01-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrstd\Frm통계형성2.frm >> frmNrstdSTS02.cs 폼이름 재정의" />

    public partial class frmNrstdSTS02 : Form
    {
        string strDate = "";
        double nIpdNo = 0;
        string strOK = "";
        string strICU = "";
        string strWARD = "";
        double nRoom = 0;
        string strPano = "";
        double[] nInwonTot = new double[17];
        int nTotal = 0;
        double[,] nBunCnt = new double[17, 17];
        double[,] nApacheCnt = new double[17, 17];
        int nSeqNo = 0;
        string strROWID = "";
        double nBunInwon = 0;
        double nApacheInwon = 0;
        double nInWonCnt = 0;
        string strGUBUN = "";
        double[,] nDeptCnt = new double[21, 21];
        string strDeptCode = "";
        string strDeptCode2 = "";

        string FstrDeptCode = "";

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmNrstdSTS02()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "통계형성 작업중입니다";

            if (OptGubun0.Checked == true)
            {
                if (Tong_Build1() == true)
                {
                    lblMsg.Text = "통계형성 완료";
                }
            }
            else
            {
                if (Tong_Build2() == true)
                {
                    lblMsg.Text = "통계형성 완료";
                }
            }

            lblMsg.Text = "통계형성 실패";
        }

        private void frmNrstdSTS02_Load(object sender, EventArgs e)
        {
            int nYY = 0;
            int nMM = 0;
            int nYYMM = 0;
            //int i = 0;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = (int)VB.Val(VB.Mid(strDTP, 6, 2));
            nYYMM = Convert.ToInt32(VB.Val(VB.Left(strDTP, 4)).ToString("0000") + VB.Val(VB.Mid(strDTP, 6, 2)).ToString("00"));

            //'2008-02-03 김현욱 NE,22 추가
            FstrDeptCode = "MD,1;HD,2;GS,3;OG,4;PD,5;OS,6;NS,7;NP,8;EN,9;OT,10;UR,11;DM,12;DT,13;PC,14;RM,15;TO,16;HR,17;PT,18;OC,19;ER,20;CS,21;NE,22";

            ComboYYMM.Items.Clear();
            clsVbfunc.SetCboDate(clsDB.DbCon, ComboYYMM, 24, strDTP, "2");

            ComboYYMM.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Tong_Build1

        private bool Tong_Build1()
        {
            int i = 0;
            //int J = 0;
            //int nREAD = 0;
            //int nRead2 = 0;
            string strDate1 = "";
            //int nScore = 0;
            string SQL = "";
            //string SqlErr = "";
            //int intRowAffected = 0;

            bool rtnVal = false;

            strGUBUN = "1";
            strDate = VB.Left(ComboYYMM.Text, 4) + "-" + VB.Mid(ComboYYMM.Text, 7, 2) + "-01";
            if (VB.Left(ComboYYMM.Text, 4) + "-" + VB.Mid(ComboYYMM.Text, 7, 2) == strDate)
            {
                if (clsPublic.GstrSabun != "4349")
                {
                    ComFunc.MsgBox("현재 월은 통계 형성을 할 수 없습니다. 이전 달을 선택하세요.", "확인");
                    return false;
                }
                strDate1 = strDTP;
            }
            else
            {
                strDate1 = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(ComboYYMM.Text, 4) + "-" + VB.Mid(ComboYYMM.Text, 7, 2) + "-01");
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                do
                {
                    for (i = 0; i <= 16; i++)
                    {
                        nBunCnt[1, i] = 0;
                        nBunCnt[2, i] = 0;
                        nBunCnt[3, i] = 0;
                        nBunCnt[4, i] = 0;
                        nBunCnt[5, i] = 0;
                        nBunCnt[6, i] = 0;
                        nBunCnt[7, i] = 0;
                        nBunCnt[8, i] = 0;
                        nBunCnt[9, i] = 0;
                        nBunCnt[10, i] = 0;
                        nBunCnt[11, i] = 0;
                        nBunCnt[12, i] = 0;
                        nBunCnt[13, i] = 0;
                        nBunCnt[14, i] = 0;
                        nBunCnt[15, i] = 0;
                        nBunCnt[16, i] = 0;
                        nApacheCnt[1, i] = 0;
                        nApacheCnt[2, i] = 0;
                        nApacheCnt[3, i] = 0;
                        nApacheCnt[4, i] = 0;
                        nApacheCnt[5, i] = 0;
                        nApacheCnt[6, i] = 0;
                        nApacheCnt[7, i] = 0;
                        nApacheCnt[8, i] = 0;
                        nApacheCnt[9, i] = 0;
                        nApacheCnt[10, i] = 0;
                        nApacheCnt[11, i] = 0;
                        nApacheCnt[12, i] = 0;
                        nApacheCnt[13, i] = 0;
                        nApacheCnt[14, i] = 0;
                        nApacheCnt[15, i] = 0;
                        nApacheCnt[16, i] = 0;
                        nInwonTot[i] = 0;
                    }

                    lblMsg.Text = "☞ " + strDate + "【 통계형성 작업중 】";

                    //JepCnt_Read();      //'인원계산
                    //Data_Read_Cnt();    //'1일분의 자료를 Dispay

                    if (JepCnt_Read() == false)
                    {
                        return rtnVal;
                    }

                    if (Data_Read_Cnt(SQL) == false)
                    {
                        return rtnVal;
                    }

                    strDate = CF.DATE_ADD(clsDB.DbCon, strDate, 1);

                } while (string.Compare(strDate, strDate1) > 0);


                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        private bool JepCnt_Read()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = " SELECT a.WardCode, a.IPDNO,a.RoomCode,a.PANO ";
                SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER a,Bas_Room b ";
                SQL = SQL + ComNum.VBLF + "  WHERE (a.OutDate IS NULL OR a.OutDate>=TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strDate, 1) + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "  AND a.IpwonTime < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strDate, 1) + "','YYYY-MM-DD')   AND a.Amset4 <> '3' ";
                SQL = SQL + ComNum.VBLF + "  AND a.Pano < '90000000'   AND a.Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "  AND a.RoomCode = b.RoomCode(+) ";
                SQL = SQL + ComNum.VBLF + "  AND (a.GbSuDay IS NULL OR a.GbSuDay <> 'Y') "; // '일일 수술 센터 제외
                SQL = SQL + ComNum.VBLF + "   ORDER BY a.WardCode,a.IPDNO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }


                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "OK";
                    strICU = "N";
                    nIpdNo = VB.Val(dt.Rows[i]["IPDNO"].ToString().Replace(",", ""));
                    strWARD = dt.Rows[i]["WardCode"].ToString().Trim();
                    nRoom = VB.Val(dt.Rows[i]["RoomCode"].ToString().Trim().Replace(",", ""));
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();

                    switch (strWARD)
                    {
                        case "IQ":
                        case "ND":
                            strWARD = "ND";
                            break;
                        case "IU":
                            strICU = "Y";
                            if (nRoom == 233)
                            {
                                strWARD = "SICU";
                            }

                            else if (nRoom == 234)
                            {
                                strWARD = "MICU";
                            }
                            break;
                    }

                    // '간호부 환자마스타를 읽어 작업일 현재 재원자인지 Check

                    SQL = "";
                    SQL = "SELECT TO_CHAR(OutDate,'YYYY-MM-DD') OutDate,Gb24HYn,Grade,Bun13,TOTAL ";
                    SQL = SQL + ComNum.VBLF + " FROM NUR_MASTER ";
                    SQL = SQL + ComNum.VBLF + "WHERE IPDNO=" + nIpdNo + " ";

                    SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }
                    if (dt3.Rows.Count > 0)
                    {
                        if (dt3.Rows[0]["Gb24hYN"].ToString().Trim() == "N")
                        {
                            strOK = "NO";
                        }
                        if (dt3.Rows[0]["OutDate"].ToString().Trim() != "")
                        {
                            if (string.Compare(dt3.Rows[0]["OutDate"].ToString().Trim(), strDate) < 0)
                            {
                                strOK = "NO";
                            }
                        }

                        if (strOK == "OK")
                        {
                            switch (strWARD)
                            {
                                case "3A":
                                    nInwonTot[1] = nInwonTot[1] + 1;
                                    break;
                                case "3B":
                                    nInwonTot[2] = nInwonTot[2] + 1;
                                    break;
                                case "3C":
                                    nInwonTot[3] = nInwonTot[3] + 1;
                                    break;
                                case "4A":
                                    nInwonTot[4] = nInwonTot[4] + 1;
                                    break;
                                case "5W":
                                    nInwonTot[5] = nInwonTot[5] + 1;
                                    break;
                                case "6W":
                                    nInwonTot[6] = nInwonTot[6] + 1;
                                    break;
                                case "7W":
                                    nInwonTot[7] = nInwonTot[7] + 1;
                                    break;
                                case "8W":
                                    nInwonTot[8] = nInwonTot[8] + 1;
                                    break;
                                case "HU":
                                    nInwonTot[9] = nInwonTot[9] + 1;
                                    break;
                                case "SICU":
                                    nInwonTot[10] = nInwonTot[10] + 1;
                                    break;
                                case "MICU":
                                    nInwonTot[11] = nInwonTot[11] + 1;
                                    break;
                                case "ND":
                                    nInwonTot[12] = nInwonTot[12] + 1;
                                    break;
                                case "3W":
                                    nInwonTot[13] = nInwonTot[13] + 1;
                                    break;
                                case "4W":
                                    nInwonTot[14] = nInwonTot[14] + 1;
                                    break;
                                case "6A":
                                    nInwonTot[15] = nInwonTot[15] + 1;
                                    break;
                                case "NR":
                                    nInwonTot[16] = nInwonTot[16] + 1;
                                    break;
                            }

                            SQL = "";
                            SQL = " SELECT GRADE,BUN13, GbICU,Total ";

                            if (string.Compare(strDate, "2011-01-01") <= 0)
                            {
                                SQL = SQL + " FROM NUR_SERIOUS ";
                            }
                            else
                            {
                                SQL = SQL + " FROM NUR_SERIOUSK ";
                            }

                            SQL = SQL + ComNum.VBLF + "    WHERE JOBTIME >=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "     AND JOBTIME <TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strDate, 1) + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "     AND Pano ='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "    AND GBICU ='" + strICU + "' ";
                            SQL = SQL + ComNum.VBLF + "    ORDER BY JOBTIME DESC ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return false;
                            }
                            if (dt2.Rows.Count > 0)
                            {
                                nTotal = (int)VB.Val(dt2.Rows[0]["TOTAL"].ToString().Replace(",", ""));
                            }
                            if (dt2.Rows.Count > 0 && nTotal > 0)
                            {
                                switch (strWARD)
                                {
                                    case "3A":
                                        nBunCnt[1, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[1, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "3B":
                                        nBunCnt[2, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[2, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "3C":
                                        nBunCnt[3, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[3, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "4A":
                                        nBunCnt[4, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[4, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "5W":
                                        nBunCnt[5, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[5, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "6W":
                                        nBunCnt[6, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[6, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "7W":
                                        nBunCnt[7, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[7, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "8W":
                                        nBunCnt[8, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[8, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "HU":
                                        nBunCnt[9, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[9, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "SICU":
                                        if (dt2.Rows[0]["GbICU"].ToString().Trim() == "Y")
                                        {
                                            nBunCnt[10, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[10, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;

                                            if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 0 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 9)
                                            {
                                                nApacheCnt[10, 1] = nApacheCnt[10, 1] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 10 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 19)
                                            {
                                                nApacheCnt[10, 2] = nApacheCnt[10, 2] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 20 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 29)
                                            {
                                                nApacheCnt[10, 3] = nApacheCnt[10, 3] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 30 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 39)
                                            {
                                                nApacheCnt[10, 4] = nApacheCnt[10, 4] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 40 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 49)
                                            {
                                                nApacheCnt[10, 5] = nApacheCnt[10, 5] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 50 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 59)
                                            {
                                                nApacheCnt[10, 6] = nApacheCnt[10, 6] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 60 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 69)
                                            {
                                                nApacheCnt[10, 7] = nApacheCnt[10, 7] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 70 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 79)
                                            {
                                                nApacheCnt[10, 8] = nApacheCnt[10, 8] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 80 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 89)
                                            {
                                                nApacheCnt[10, 9] = nApacheCnt[10, 9] + 1;
                                            }
                                            else
                                            {
                                                nApacheCnt[10, 10] = nApacheCnt[10, 10] + 1;
                                            }
                                        }
                                        break;
                                    case "MICU":
                                        if (dt2.Rows[0]["GbICU"].ToString().Trim() == "Y")
                                        {
                                            nBunCnt[11, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[11, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;

                                            if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 0 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 9)
                                            {
                                                nApacheCnt[11, 1] = nApacheCnt[11, 1] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 10 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 19)
                                            {
                                                nApacheCnt[11, 2] = nApacheCnt[11, 2] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 20 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 29)
                                            {
                                                nApacheCnt[11, 3] = nApacheCnt[11, 3] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 30 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 39)
                                            {
                                                nApacheCnt[11, 4] = nApacheCnt[11, 4] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 40 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 49)
                                            {
                                                nApacheCnt[11, 5] = nApacheCnt[11, 5] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 50 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 59)
                                            {
                                                nApacheCnt[11, 6] = nApacheCnt[11, 6] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 60 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 69)
                                            {
                                                nApacheCnt[11, 7] = nApacheCnt[11, 7] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 70 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 79)
                                            {
                                                nApacheCnt[11, 8] = nApacheCnt[11, 8] + 1;
                                            }
                                            else if (VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) >= 80 || VB.Val(dt2.Rows[0]["Bun13"].ToString().Trim()) <= 89)
                                            {
                                                nApacheCnt[11, 9] = nApacheCnt[11, 9] + 1;
                                            }
                                            else
                                            {
                                                nApacheCnt[10, 10] = nApacheCnt[10, 10] + 1;
                                            }
                                        }
                                        break;
                                    case "ND":
                                        nBunCnt[12, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[12, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "3W":
                                        nBunCnt[13, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[13, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "4W":
                                        nBunCnt[14, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[14, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "6A":
                                        nBunCnt[15, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[15, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                    case "NR":
                                        nBunCnt[16, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] = nBunCnt[16, (int)VB.Val(dt2.Rows[0]["Grade"].ToString().Replace(",", ""))] + 1;
                                        break;
                                }
                                dt2.Dispose();
                                dt2 = null;
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;
                return true;
            }
            catch (System.Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }

        }

        private bool Data_Read_Cnt(string SQL)
        {
            DataTable dt = null;
            int intRowAffected = 0;
            string SqlErr = "";
            int J = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                for (int i = 1; i <= 16; i++)
                {
                    switch (i)
                    {
                        case 1:
                            strWARD = "3A";
                            nSeqNo = 1;
                            break;
                        case 2:
                            strWARD = "3B";
                            nSeqNo = 2;
                            break;
                        case 3:
                            strWARD = "3C";
                            nSeqNo = 3;
                            break;
                        case 4:
                            strWARD = "4A";
                            nSeqNo = 4;
                            break;
                        case 5:
                            strWARD = "5W";
                            nSeqNo = 5;
                            break;
                        case 6:
                            strWARD = "6W";
                            nSeqNo = 6;
                            break;
                        case 7:
                            strWARD = "7W";
                            nSeqNo = 7;
                            break;
                        case 8:
                            strWARD = "8W";
                            nSeqNo = 8;
                            break;
                        case 9:
                            strWARD = "HU";
                            nSeqNo = 10;
                            break;
                        case 10:
                            strWARD = "SICU";
                            nSeqNo = 11;
                            break;
                        case 11:
                            strWARD = "MICU";
                            nSeqNo = 12;
                            break;
                        case 12:
                            strWARD = "ND";
                            nSeqNo = 9;
                            break;
                        case 13:
                            strWARD = "3W";
                            nSeqNo = 13;
                            break;
                        case 14:
                            strWARD = "4W";
                            nSeqNo = 14;
                            break;
                        case 15:
                            strWARD = "6A";
                            nSeqNo = 15;
                            break;
                        case 16:
                            strWARD = "NR";
                            nSeqNo = 16;
                            break;
                    }

                    SQL = "";
                    SQL = " SELECT ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM NUR_STD_TONG2 ";
                    SQL = SQL + ComNum.VBLF + "  WHERE ActDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND WardCode ='" + strWARD + "' ";
                    SQL = SQL + ComNum.VBLF + " AND Gubun ='1' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strROWID = "";
                        strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                        nBunInwon = 0;
                        nApacheInwon = 0;

                        switch (strWARD)
                        {
                            case "3A":
                                nInWonCnt = nInwonTot[1];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[1, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[1, J];
                                }
                                break;

                            case "3B":
                                nInWonCnt = nInwonTot[2];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[2, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[2, J];
                                }
                                break;


                            case "3C":
                                nInWonCnt = nInwonTot[3];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[3, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[3, J];
                                }
                                break;
                            case "4A":
                                nInWonCnt = nInwonTot[4];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[4, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[4, J];
                                }
                                break;
                            case "5W":
                                nInWonCnt = nInwonTot[5];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[5, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[5, J];
                                }
                                break;
                            case "6W":
                                nInWonCnt = nInwonTot[6];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[6, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[6, J];
                                }
                                break;
                            case "7W":
                                nInWonCnt = nInwonTot[7];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[7, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[7, J];
                                }
                                break;
                            case "8W":
                                nInWonCnt = nInwonTot[8];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[8, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[8, J];
                                }
                                break;
                            case "HU":
                                nInWonCnt = nInwonTot[9];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[9, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[9, J];
                                }
                                break;
                            case "SICU":
                                nInWonCnt = nInwonTot[10];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[10, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[10, J];
                                }
                                break;
                            case "MICU":
                                nInWonCnt = nInwonTot[11];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[11, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[11, J];
                                }
                                break;
                            case "ND":
                                nInWonCnt = nInwonTot[12];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[12, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[12, J];
                                }
                                break;
                            case "3W":
                                nInWonCnt = nInwonTot[13];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[13, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[13, J];
                                }
                                break;
                            case "4W":
                                nInWonCnt = nInwonTot[14];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[14, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[14, J];
                                }
                                break;
                            case "6A":
                                nInWonCnt = nInwonTot[15];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[15, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[15, J];
                                }
                                break;
                            case "NR":
                                nInWonCnt = nInwonTot[16];
                                for (J = 0; J <= 6; J++)
                                {
                                    nBunInwon = nBunInwon + nBunCnt[16, J];
                                }
                                for (J = 1; J <= 10; J++)
                                {
                                    nApacheInwon = nApacheInwon + nApacheCnt[16, J];
                                }
                                break;
                        }

                        if (strROWID == "")
                        {
                            SQL = "";
                            SQL = " INSERT INTO NUR_STD_TONG2 (ActDate,Gubun,Bun1,Bun2,Bun3,Bun4,Bun5,Bun6,Etc,";
                            SQL = SQL + ComNum.VBLF + " Apache1,Apache2,Apache3,Apache4,Apache5,Apache6,Apache7,Apache8,Apache9,Apache10,Total,Ap_Total, ";
                            SQL = SQL + ComNum.VBLF + " WardCode,seqno,EntTime,JOBSABUN )";
                            SQL = SQL + ComNum.VBLF + " VALUES [TO_DATE['" + strDate + "','YYYY-MM-DD'],'" + strGUBUN + "', ";
                            SQL = SQL + ComNum.VBLF + " " + nBunCnt[i, 1] + "," + nBunCnt[i, 2] + "," + nBunCnt[i, 3] + ", ";
                            SQL = SQL + ComNum.VBLF + " " + nBunCnt[i, 4] + "," + nBunCnt[i, 5] + "," + nBunCnt[i, 6] + ", " + nBunCnt[i, 0] + ", ";
                            SQL = SQL + ComNum.VBLF + " " + nApacheCnt[i, 1] + "," + nApacheCnt[i, 2] + "," + nApacheCnt[i, 3] + "," + nApacheCnt[i, 4] + "," + nApacheCnt[i, 5] + ", ";
                            SQL = SQL + ComNum.VBLF + " " + nApacheCnt[i, 6] + "," + nApacheCnt[i, 7] + "," + nApacheCnt[i, 8] + "," + nApacheCnt[i, 9] + "," + nApacheCnt[i, 10] + ", ";
                            SQL = SQL + ComNum.VBLF + " " + nInwonTot[i] + "," + nApacheInwon + ",'" + strWARD + "', " + nSeqNo + ", ";
                            SQL = SQL + ComNum.VBLF + " TO_DATE['" + Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A")) + "','YYYY-MM-DD HH24:MI'] ," + clsPublic.GnJobSabun + " ] ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = " UPDATE NUR_STD_TONG2 SET Gubun='" + strGUBUN + "',";
                            SQL = SQL + ComNum.VBLF + " Bun1 =" + nBunCnt[i, 1] + ", ";
                            SQL = SQL + ComNum.VBLF + " Bun2 =" + nBunCnt[i, 2] + ", ";
                            SQL = SQL + ComNum.VBLF + " Bun3 =" + nBunCnt[i, 3] + ", ";
                            SQL = SQL + ComNum.VBLF + " Bun4 =" + nBunCnt[i, 4] + ", ";
                            SQL = SQL + ComNum.VBLF + " Bun5 =" + nBunCnt[i, 5] + ", ";
                            SQL = SQL + ComNum.VBLF + " Bun6 =" + nBunCnt[i, 6] + ", ";
                            SQL = SQL + ComNum.VBLF + " Etc =" + nBunCnt[i, 0] + ", ";
                            SQL = SQL + ComNum.VBLF + " Apache1 =" + nApacheCnt[i, 1] + ", ";
                            SQL = SQL + ComNum.VBLF + " Apache2 =" + nApacheCnt[i, 2] + ",";
                            SQL = SQL + ComNum.VBLF + " Apache3 =" + nApacheCnt[i, 3] + ",";
                            SQL = SQL + ComNum.VBLF + " Apache4 =" + nApacheCnt[i, 4] + ",";
                            SQL = SQL + ComNum.VBLF + " Apache5 =" + nApacheCnt[i, 5] + ",";
                            SQL = SQL + ComNum.VBLF + " Apache6 =" + nApacheCnt[i, 6] + ",";
                            SQL = SQL + ComNum.VBLF + " Apache7 =" + nApacheCnt[i, 7] + ",";
                            SQL = SQL + ComNum.VBLF + " Apache8 =" + nApacheCnt[i, 8] + ",";
                            SQL = SQL + ComNum.VBLF + " Apache9 =" + nApacheCnt[i, 9] + ",";
                            SQL = SQL + ComNum.VBLF + " Apache10 =" + nApacheCnt[i, 10] + ",";
                            SQL = SQL + ComNum.VBLF + " Total =" + nInwonTot[i] + ", ";
                            SQL = SQL + ComNum.VBLF + " Ap_Total =" + nApacheInwon + ", ";
                            SQL = SQL + ComNum.VBLF + " WardCode = '" + strWARD + "' , ";
                            SQL = SQL + ComNum.VBLF + " Seqno = " + nSeqNo + ",  ";
                            SQL = SQL + ComNum.VBLF + " EntTime = TO_DATE['" + Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A")) + "','YYYY-MM-DD HH24:MI'] ,";
                            SQL = SQL + ComNum.VBLF + " JOBSABUN = " + clsPublic.GnJobSabun + "  ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID='" + strROWID + "' ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            return true;
        }

        #endregion

        private bool Tong_Build2()
        {
            int i = 0;
            int J = 0;

            string strDate1 = "";
            //int nScore = 0;
            string SQL = "";
            //string SqlErr = "";
            //int intRowAffected = 0;

            bool rtnVal = false;

            //과별병동 통계
            strGUBUN = "2";
            strDate = VB.Left(ComboYYMM.Text, 4) + "-" + VB.Mid(ComboYYMM.Text, 7, 2) + "-01";
            if (VB.Left(ComboYYMM.Text, 4) + "-" + VB.Mid(ComboYYMM.Text, 7, 2) == strDate)
            {
                if (clsPublic.GstrSabun != "4349")
                {
                    ComFunc.MsgBox("현재 월은 통계 형성을 할 수 없습니다. 이전 달을 선택하세요.", "확인");
                    return false;
                }
                strDate1 = strDTP;
            }
            else
            {
                strDate1 = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(ComboYYMM.Text, 4) + "-" + VB.Mid(ComboYYMM.Text, 7, 2) + "-01");
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                do
                {
                    for (i = 0; i <= 16; i++)
                    {
                        nInwonTot[i] = 0;
                    }
                    for (i = 0; i <= 20; i++)
                    {
                        nDeptCnt[i, J] = 0;
                    }

                    lblMsg.Text = "☞ " + strDate + "【 통계형성 작업중 】";

                    //JepCnt_Read();      //'인원계산
                    //Data_Read_Cnt();    //'1일분의 자료를 Dispay

                    if (JepCnt_Read2() == false)
                    {
                        return rtnVal;
                    }

                    if (Data_Read_Cnt2(SQL) == false)
                    {
                        return rtnVal;
                    }

                    strDate = CF.DATE_ADD(clsDB.DbCon, strDate, 1);

                } while (string.Compare(strDate, strDate1) > 0);


                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        private bool JepCnt_Read2()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = " SELECT a.WardCode, a.IPDNO,a.RoomCode ,a.DeptCode,a.Pano ";
                SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER a,Bas_Room b, BAS_CLINICDEPT c   ";
                SQL = SQL + ComNum.VBLF + "  WHERE (a.OutDate IS NULL OR a.OutDate>=TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strDate, 1) + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "  AND a.IpwonTime < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strDate, 1) + "','YYYY-MM-DD')   AND a.Amset4 <> '3' ";
                SQL = SQL + ComNum.VBLF + "  AND a.Pano < '90000000'   AND a.Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "  AND a.RoomCode = b.RoomCode(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.DeptCode = c.DeptCode(+) ";
                SQL = SQL + ComNum.VBLF + "  AND (a.GbSuDay IS NULL OR a.GbSuDay <> 'Y') ";//  '일일 수술 센터 제외
                SQL = SQL + ComNum.VBLF + "   ORDER BY a.WardCode,c.PrintRanking,a.DeptCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }


                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "OK";
                    strICU = "N";
                    nIpdNo = VB.Val(dt.Rows[i]["IPDNO"].ToString().Replace(",", ""));
                    strWARD = dt.Rows[i]["WardCode"].ToString().Trim();
                    nRoom = VB.Val(dt.Rows[i]["RoomCode"].ToString().Trim().Replace(",", ""));
                    strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();

                    switch (strWARD)
                    {
                        // '=======2010-02-09 김현욱 수정 NR 별도 병동
                        // 'Case "IQ", "NR", "ND": strWard = "ND"

                        case "IQ":
                        case "ND":
                            strWARD = "ND";
                            break;
                        case "IU":
                            if (nRoom == 233)
                            {
                                strWARD = "SICU";
                            }
                            else if (nRoom == 234)
                            {
                                strWARD = "MICU";
                            }
                            break;
                    }

                    // '간호부 환자마스타를 읽어 작업일 현재 재원자인지 Check

                    SQL = "";
                    SQL = "SELECT TO_CHAR(OutDate,'YYYY-MM-DD') OutDate,Gb24HYn,Grade,Bun13,InDept,Total ";
                    SQL = SQL + ComNum.VBLF + " FROM NUR_MASTER ";
                    SQL = SQL + ComNum.VBLF + "WHERE IPDNO=" + nIpdNo + " ";

                    SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }
                    if (dt3.Rows.Count > 0)
                    {
                        if (dt3.Rows[0]["Gb24hYN"].ToString().Trim() == "N")
                        {
                            strOK = "NO";
                        }
                        if (dt3.Rows[0]["OutDate"].ToString().Trim() != "")
                        {
                            if (string.Compare(dt.Rows[0]["OutDate"].ToString().Trim(), strDate) < 0)
                            {
                                strOK = "NO";
                            }
                        }

                        if (strOK == "OK")
                        {
                            switch (strWARD)
                            {
                                case "3A":
                                    nInwonTot[1] = nInwonTot[1] + 1;
                                    break;
                                case "3B":
                                    nInwonTot[2] = nInwonTot[2] + 1;
                                    break;
                                case "3C":
                                    nInwonTot[3] = nInwonTot[3] + 1;
                                    break;
                                case "4A":
                                    nInwonTot[4] = nInwonTot[4] + 1;
                                    break;
                                case "5W":
                                    nInwonTot[5] = nInwonTot[5] + 1;
                                    break;
                                case "6W":
                                    nInwonTot[6] = nInwonTot[6] + 1;
                                    break;
                                case "7W":
                                    nInwonTot[7] = nInwonTot[7] + 1;
                                    break;
                                case "8W":
                                    nInwonTot[8] = nInwonTot[8] + 1;
                                    break;
                                case "HU":
                                    nInwonTot[9] = nInwonTot[9] + 1;
                                    break;
                                case "SICU":
                                    nInwonTot[10] = nInwonTot[10] + 1;
                                    break;
                                case "MICU":
                                    nInwonTot[11] = nInwonTot[11] + 1;
                                    break;
                                case "ND":
                                    nInwonTot[12] = nInwonTot[12] + 1;
                                    break;
                                case "3W":
                                    nInwonTot[13] = nInwonTot[13] + 1;
                                    break;
                                case "4W":
                                    nInwonTot[14] = nInwonTot[14] + 1;
                                    break;
                                case "6A":
                                    nInwonTot[15] = nInwonTot[15] + 1;
                                    break;
                                case "NR":
                                    nInwonTot[16] = nInwonTot[16] + 1;
                                    break;
                            }

                            SQL = "";
                            SQL = " SELECT GRADE,BUN13, GbICU ";

                            if (string.Compare(strDate, "2011-01-01") <= 0)
                            {
                                SQL = SQL + " FROM NUR_SERIOUS ";
                            }
                            else
                            {
                                SQL = SQL + " FROM NUR_SERIOUSK ";
                            }

                            SQL = SQL + "    WHERE JOBTIME >=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL = SQL + "     AND JOBTIME <TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strDate, 1) + "','YYYY-MM-DD') ";
                            SQL = SQL + "     AND Pano ='" + strPano + "' ";
                            SQL = SQL + "    AND GBICU ='" + strICU + "' ";
                            SQL = SQL + "    ORDER BY JOBTIME DESC ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return false;
                            }
                            if (dt2.Rows.Count > 0)
                            {
                                nTotal = (int)VB.Val(dt2.Rows[0]["TOTAL"].ToString().Replace(",", ""));
                            }
                            if (dt2.Rows.Count > 0 && nTotal > 0)
                            {
                                switch (strWARD)
                                {
                                    case "3A":
                                        nDeptCnt[1, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[1, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "3B":
                                        nDeptCnt[2, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[2, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "3C":
                                        nDeptCnt[3, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[3, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "4A":
                                        nDeptCnt[4, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[4, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "5W":
                                        nDeptCnt[5, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[5, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "6W":
                                        nDeptCnt[6, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[6, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "7W":
                                        nDeptCnt[7, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[7, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "8W":
                                        nDeptCnt[8, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[8, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "HU":
                                        nDeptCnt[9, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[9, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "SICU":
                                        nDeptCnt[10, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[10, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "MICU":
                                        nDeptCnt[11, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[11, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "ND":
                                        nDeptCnt[12, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[12, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "3W":
                                        nDeptCnt[13, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[13, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "4W":
                                        nDeptCnt[14, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[14, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "6A":
                                        nDeptCnt[15, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[15, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "NR":
                                        nDeptCnt[16, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nDeptCnt[16, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                }
                                dt2.Dispose();
                                dt2 = null;
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;
                return true;
            }
            catch (System.Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }
        }

        private bool Data_Read_Cnt2(string SQL)
        {
            DataTable dt = null;
            int intRowAffected = 0;
            string SqlErr = "";
            int J = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                for (int i = 1; i <= 16; i++)
                {
                    switch (i)
                    {
                        case 1:
                            strWARD = "3A";
                            nSeqNo = 1;
                            break;
                        case 2:
                            strWARD = "3B";
                            nSeqNo = 2;
                            break;
                        case 3:
                            strWARD = "3C";
                            nSeqNo = 3;
                            break;
                        case 4:
                            strWARD = "4A";
                            nSeqNo = 4;
                            break;
                        case 5:
                            strWARD = "5W";
                            nSeqNo = 5;
                            break;
                        case 6:
                            strWARD = "6W";
                            nSeqNo = 6;
                            break;
                        case 7:
                            strWARD = "7W";
                            nSeqNo = 7;
                            break;
                        case 8:
                            strWARD = "8W";
                            nSeqNo = 8;
                            break;
                        case 9:
                            strWARD = "HU";
                            nSeqNo = 10;
                            break;
                        case 10:
                            strWARD = "SICU";
                            nSeqNo = 11;
                            break;
                        case 11:
                            strWARD = "MICU";
                            nSeqNo = 12;
                            break;
                        case 12:
                            strWARD = "ND";
                            nSeqNo = 9;
                            break;
                        case 13:
                            strWARD = "3W";
                            nSeqNo = 13;
                            break;
                        case 14:
                            strWARD = "4W";
                            nSeqNo = 14;
                            break;
                        case 15:
                            strWARD = "6A";
                            nSeqNo = 15;
                            break;
                        case 16:
                            strWARD = "NR";
                            nSeqNo = 16;
                            break;
                    }

                    SQL = "";
                    SQL = " SELECT ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM NUR_STD_TONG2 ";
                    SQL = SQL + ComNum.VBLF + "  WHERE ActDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND WardCode ='" + strWARD + "' ";
                    SQL = SQL + ComNum.VBLF + " AND Gubun ='2' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strROWID = "";
                        strROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                        strDeptCode2 = "";

                        switch (strWARD)
                        {
                            case "3A":
                                nInWonCnt = nInwonTot[1];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[1, J].ToString().Trim() + ";";
                                }
                                break;

                            case "3B":
                                nInWonCnt = nInwonTot[2];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[2, J].ToString().Trim() + ";";
                                }
                                break;


                            case "3C":
                                nInWonCnt = nInwonTot[3];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[3, J].ToString().Trim() + ";";
                                }
                                break;
                            case "4A":
                                nInWonCnt = nInwonTot[4];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[4, J].ToString().Trim() + ";";
                                }
                                break;
                            case "5W":
                                nInWonCnt = nInwonTot[5];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[5, J].ToString().Trim() + ";";
                                }
                                break;
                            case "6W":
                                nInWonCnt = nInwonTot[6];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[6, J].ToString().Trim() + ";";
                                }
                                break;
                            case "7W":
                                nInWonCnt = nInwonTot[7];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[7, J].ToString().Trim() + ";";
                                }
                                break;
                            case "8W":
                                nInWonCnt = nInwonTot[8];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[8, J].ToString().Trim() + ";";
                                }
                                break;
                            case "HU":
                                nInWonCnt = nInwonTot[9];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[9, J].ToString().Trim() + ";";
                                }
                                break;
                            case "SICU":
                                nInWonCnt = nInwonTot[10];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[10, J].ToString().Trim() + ";";
                                }
                                break;
                            case "MICU":
                                nInWonCnt = nInwonTot[11];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[11, J].ToString().Trim() + ";";
                                }
                                break;
                            case "ND":
                                nInWonCnt = nInwonTot[12];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[12, J].ToString().Trim() + ";";
                                }
                                break;
                            case "3W":
                                nInWonCnt = nInwonTot[13];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[13, J].ToString().Trim() + ";";
                                }
                                break;
                            case "4W":
                                nInWonCnt = nInwonTot[14];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[14, J].ToString().Trim() + ";";
                                }
                                break;
                            case "6A":
                                nInWonCnt = nInwonTot[15];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[15, J].ToString().Trim() + ";";
                                }
                                break;
                            case "NR":
                                nInWonCnt = nInwonTot[16];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)) + "," + nDeptCnt[16, J].ToString().Trim() + ";";
                                }
                                break;
                        }

                        if (strROWID == "")
                        {
                            SQL = "";
                            ;
                            SQL = " INSERT INTO NUR_STD_TONG2 (ActDate,Gubun,Total,WardCode,seqno,DeptCode,EntTime,JOBSABUN ) ";
                            SQL = SQL + ComNum.VBLF + " VALUES (TO_DATE('" + strDate + "','YYYY-MM-DD'),'" + strGUBUN + "', ";
                            SQL = SQL + ComNum.VBLF + " " + nInwonTot[i] + ",'" + strWARD + "', " + nSeqNo + ",'" + strDeptCode2 + "',  ";
                            SQL = SQL + ComNum.VBLF + " TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A")) + "','YYYY-MM-DD HH24:MI') ," + clsPublic.GnJobSabun + " ) ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = " UPDATE NUR_STD_TONG2 SET Gubun='" + strGUBUN + "',";
                            SQL = SQL + ComNum.VBLF + " Total =" + nInwonTot[i] + ", ";
                            SQL = SQL + ComNum.VBLF + " WardCode = '" + strWARD + "' , ";
                            SQL = SQL + ComNum.VBLF + " Seqno = " + nSeqNo + ",  ";
                            SQL = SQL + ComNum.VBLF + " DeptCode = '" + strDeptCode2 + "',  ";
                            SQL = SQL + ComNum.VBLF + " EntTime = TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A")) + "','YYYY-MM-DD HH24:MI') ,";
                            SQL = SQL + ComNum.VBLF + " JOBSABUN = " + clsPublic.GnJobSabun + "  ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID='" + strROWID + "' ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            return true;
        }
    }
}
