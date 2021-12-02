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
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrstd\Frm통계형성3.frm >> frmNrstdSTS03.cs 폼이름 재정의" />

    public partial class frmNrstdSTS03 : Form
    {

        string strDate = "";
        double nIpdNo = 0;
        string strOK = "";
        //string strICU = "";
        string strWARD = "";
        double nRoom = 0;
        //string strPano = "";
        double[] nInwonTot = new double[17];
        //int nTotal = 0;
        double[,] nBunCnt = new double[17, 17];
        double[,] nApacheCnt = new double[17, 17];
        int nSeqNo = 0;
        string strROWID = "";
        //double nBunInwon = 0;
        //double nApacheInwon = 0;
        double nInWonCnt = 0;
        string strGUBUN = "";
        double[,] nDeptCnt = new double[21, 21];
        string strDeptCode = "";
        string strDeptCode2 = "";
        double nOpInwon = 0;
        double[,] nOpCnt = new double[21, 23];
        string strOpInwon2 = "";
        string FstrDeptCode = "";

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmNrstdSTS03()
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
        }

        private void frmNrstdSTS03_Load(object sender, EventArgs e)
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
            int J = 0;
            string strDate1 = "";
            string SQL = "";
            //string SqlErr = "";
            //int intRowAffected = 0;

            bool rtnVal = false;

            // '과별병동 통계

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
                    nOpInwon = 0;
                    for (i = 0; i <= 16; i++)
                    {
                        nInwonTot[i] = 0;
                        for (J = 0; J <= 22; J++)
                        {
                            nDeptCnt[i, J] = 0;
                            nOpCnt[i, J] = 0;
                        }
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
                SQL = " SELECT a.WardCode, a.IPDNO,a.RoomCode ,a.DeptCode,a.Pano ";
                SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER a,Bas_Room b, BAS_CLINICDEPT c   ";
                SQL = SQL + ComNum.VBLF + "  WHERE (a.OutDate IS NULL OR a.OutDate>=TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strDate, 0) + "','YYYY-MM-DD')) ";
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
                    nIpdNo = VB.Val(dt.Rows[i]["IPDNO"].ToString().Replace(",", ""));
                    strWARD = dt.Rows[i]["WardCode"].ToString().Trim();
                    nRoom = VB.Val(dt.Rows[i]["RoomCode"].ToString().Trim().Replace(",", ""));
                    strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();

                    switch (strWARD)
                    {
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
                    SQL = "SELECT TO_CHAR(OutDate,'YYYY-MM-DD') OutDate,Gb24HYn,Grade,Bun13,InDept ";
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

                            dt3.Dispose();
                            dt3 = null;

                            // '수술인원 check
                            strOK = "";
                            SQL = "";
                            SQL = "SELECT ROWID From ORAN_MASTER ";
                            SQL = SQL + ComNum.VBLF + " WHERE OpDate >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + " AND OpDate <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + " AND OPBUN IN ('1','2','3','4') ";
                            SQL = SQL + ComNum.VBLF + " AND Pano ='" + dt.Rows[i]["PANO"].ToString().Trim() + "'  ";
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
                                strOK = "OK";
                            }
                            if (strOK == "OK")
                            {
                                switch (strWARD)
                                {
                                    case "3A":
                                        nOpCnt[1, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[1, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "3B":
                                        nOpCnt[2, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[2, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "3C":
                                        nOpCnt[3, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[3, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "4A":
                                        nOpCnt[4, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[4, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "5W":
                                        nOpCnt[5, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[5, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "6W":
                                        nOpCnt[6, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[6, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "7W":
                                        nOpCnt[7, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[7, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "8W":
                                        nOpCnt[8, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[8, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "HU":
                                        nOpCnt[9, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[9, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "SICU":
                                        nOpCnt[10, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[10, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "MICU":
                                        nOpCnt[11, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[11, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "ND":
                                        nOpCnt[12, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[12, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "3W":
                                        nOpCnt[13, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[13, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "4W":
                                        nOpCnt[14, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[14, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "6A":
                                        nOpCnt[15, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[15, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
                                        break;
                                    case "NR":
                                        nOpCnt[16, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] = nOpCnt[16, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptCode, strDeptCode, 2), ",", 2), ";", 1))] + 1;
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
                    SQL = SQL + ComNum.VBLF + " FROM NUR_STD_TONG3 ";
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

                        strDeptCode2 = "";
                        strOpInwon2 = "";
                        nOpInwon = 0;

                        switch (strWARD)
                        {
                            case "3A":
                                nInWonCnt = nInwonTot[1];

                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[1, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[1, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[1, J];
                                }
                                break;

                            case "3B":
                                nInWonCnt = nInwonTot[2];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[2, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[2, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[2, J];
                                }
                                break;


                            case "3C":
                                nInWonCnt = nInwonTot[3];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[3, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[3, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[3, J];
                                }
                                break;
                            case "4A":
                                nInWonCnt = nInwonTot[4];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[4, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[4, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[4, J];
                                }
                                break;
                            case "5W":
                                nInWonCnt = nInwonTot[5];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[5, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[5, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[5, J];
                                }
                                break;
                            case "6W":
                                nInWonCnt = nInwonTot[6];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[6, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[6, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[6, J];
                                }
                                break;
                            case "7W":
                                nInWonCnt = nInwonTot[7];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[7, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[7, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[7, J];
                                }
                                break;
                            case "8W":
                                nInWonCnt = nInwonTot[8];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[8, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[8, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[8, J];
                                }
                                break;
                            case "HU":
                                nInWonCnt = nInwonTot[9];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[9, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[9, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[9, J];
                                }
                                break;
                            case "SICU":
                                nInWonCnt = nInwonTot[10];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[10, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[10, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[10, J];
                                }
                                break;
                            case "MICU":
                                nInWonCnt = nInwonTot[11];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[11, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[11, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[11, J];
                                }
                                break;
                            case "ND":
                                nInWonCnt = nInwonTot[12];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[12, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[12, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[12, J];
                                }
                                break;
                            case "3W":
                                nInWonCnt = nInwonTot[13];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[13, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[13, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[13, J];
                                }
                                break;
                            case "4W":
                                nInWonCnt = nInwonTot[14];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[14, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[14, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[14, J];
                                }
                                break;
                            case "6A":
                                nInWonCnt = nInwonTot[15];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[15, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[15, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[15, J];
                                }
                                break;
                            case "NR":
                                nInWonCnt = nInwonTot[16];
                                for (J = 1; J <= 22; J++)
                                {
                                    strDeptCode2 = strDeptCode2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nDeptCnt[16, J]).ToString() + ";";
                                    strOpInwon2 = strOpInwon2 + (VB.Pstr(VB.Pstr(FstrDeptCode, ";", J), ",", 1)).Trim() + "," + (nOpCnt[16, J]).ToString() + ";";
                                    nOpInwon = nOpInwon + nOpCnt[16, J];
                                }
                                break;
                        }

                        if (strROWID == "")
                        {
                            SQL = "";

                            SQL = " INSERT INTO NUR_STD_TONG3 (ActDate,Gubun,Total,WardCode,seqno,DeptCode,OpCode,OpCnt,EntTime,JOBSABUN ) ";
                            SQL = SQL + " VALUES (TO_DATE('" + strDate + "','YYYY-MM-DD'),'" + strGUBUN + "', ";
                            SQL = SQL + " " + nInwonTot[i] + ",'" + strWARD + "', " + nSeqNo + ",'" + strDeptCode2 + "','" + strOpInwon2 + "'," + nOpInwon + ",  ";
                            SQL = SQL + " TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A")) + "','YYYY-MM-DD HH24:MI') ," + clsPublic.GnJobSabun + " ) ";

                        }
                        else
                        {
                            SQL = "";
                            SQL = " UPDATE NUR_STD_TONG3 SET Gubun='" + strGUBUN + "',";
                            SQL = SQL + " Total =" + nInwonTot[i] + ", ";
                            SQL = SQL + " WardCode = '" + strWARD + "', ";
                            SQL = SQL + " Seqno = " + nSeqNo + ", ";
                            SQL = SQL + " DeptCode = '" + strDeptCode2 + "', ";
                            SQL = SQL + " OpCode = '" + strOpInwon2 + "', ";
                            SQL = SQL + " OpCnt = " + nOpInwon + ", ";
                            SQL = SQL + " EntTime = TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A")) + "','YYYY-MM-DD HH24:MI') ,";
                            SQL = SQL + " JOBSABUN = " + clsPublic.GnJobSabun + "  ";
                            SQL = SQL + " WHERE ROWID='" + strROWID + "' ";
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
    }
}
