using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using ComBase;
using System.IO;

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmEMC.cs
    /// Description     : 응급의료자료전송
    /// Author          : 유진호
    /// Create Date     : 2018-05-07
    /// <history>       
    /// D:\참고소스\포항성모병원 VB Source(2018.04.01)\OPD\emc\frmEmc(dgemc.frm)
    /// </history>
    /// </summary>
    public partial class frmEMC : Form
    {
        //중앙응급의료센터 FTP서버
        private string EMC_SERVERIP = "211.112.137.14";
        private string EMC_SERVERID = "gyeongsangbuk";
        private string EMC_SERVERPW = "gyeongsangbuk";


        int FnTimer = 0;
        //int FnSS = 0;

        public frmEMC()
        {
            InitializeComponent();
        }

        private void frmEMC_Load(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            FnTimer = 0;
            txtDate.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            txtWait.Text = FnTimer + "(분)";
            txtLog.Text = "";
           
            if (clsType.User.BuseCode != "033109")
            {
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
            }

            if (clsType.User.Sabun == "40024") timer1.Enabled = true;

            Data_Display();
        }

        private void Data_Display()
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            clsSpread CS = new clsSpread();
            int nRow = 0;
            //int nREAD = 0;
            int nCnt1 = 0;
            int nCNT2 = 0;
            int nLastRow = 0;
            int nTot1 = 0;
            int nTot2 = 0;
            int nTot3 = 0;
            int nIcuBed = 0;
            int nWardBed = 0;

            string strDATA = "";
            string strDrName = "";
            string strTIME = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            try
            {
                if (Convert.ToDateTime(clsPublic.GstrSysDate) >= Convert.ToDateTime("2006-10-01"))
                {
                    SQL = " SELECT  TO_CHAR(a.TDate,'YYYY-MM-DD') ToDate,a.DNAME1 ToDName1, a.DNAME2 ToDName2, a.DNAME3 ToDName3,";
                    SQL = SQL + ComNum.VBLF + "   TO_CHAR(b.TDate,'YYYY-MM-DD') OldDate,b.DNAME1 OldDName1, b.DNAME2 OldDName2, b.DNAME3 OldDName3 ";
                    SQL = SQL + ComNum.VBLF + " FROM ETC_DANGJIK A ";
                    SQL = SQL + ComNum.VBLF + " , (SELECT TDate,DNAME1 , DNAME2 , DNAME3 ";
                    SQL = SQL + ComNum.VBLF + "      FROM ETC_DANGJIK ";
                    SQL = SQL + ComNum.VBLF + "     WHERE GUBUN  = '11' ";
                    SQL = SQL + ComNum.VBLF + "     AND TDATE = " + ComFunc.ConvOraToDate(Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-1), "D") + ") b ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.GUBUN  = '11' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.TDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (string.Compare(clsPublic.GstrSysTime, "09:01") >= 0 && string.Compare(clsPublic.GstrSysTime, "17:00") <= 0)
                        {
                            strDrName = dt.Rows[0]["ToDName1"].ToString().Trim();
                        }
                        else if (string.Compare(clsPublic.GstrSysTime, "17:01") >= 0 && string.Compare(clsPublic.GstrSysTime, "23:00") <= 0)
                        {
                            strDrName = dt.Rows[0]["ToDName2"].ToString().Trim();
                        }
                        else if (string.Compare(clsPublic.GstrSysTime, "23:01") >= 0 && string.Compare(clsPublic.GstrSysTime, "23:59") <= 0)
                        {
                            strDrName = dt.Rows[0]["ToDName3"].ToString().Trim();
                        }
                        else if (string.Compare(clsPublic.GstrSysTime, "00:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "09:00") <= 0)
                        {
                            strDrName = dt.Rows[0]["OldDName3"].ToString().Trim();
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    SQL = " SELECT  DNAME1, DNAME2, DNAME3 FROM ETC_DANGJIK ";
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN  = '11' ";
                    SQL = SQL + ComNum.VBLF + "   AND TDATE = " + ComFunc.ConvOraToDate(Convert.ToDateTime(clsPublic.GstrSysDate), "D");

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (string.Compare(clsPublic.GstrSysTime, "07:01") >= 0 && string.Compare(clsPublic.GstrSysTime, "15:00") <= 0)
                        {
                            strDrName = dt.Rows[0]["DNAME1"].ToString().Trim();
                        }
                        else if (string.Compare(clsPublic.GstrSysTime, "15:01") >= 0 && string.Compare(clsPublic.GstrSysTime, "23:00") <= 0)
                        {
                            strDrName = dt.Rows[0]["DNAME2"].ToString().Trim();
                        }
                        else
                        {
                            strDrName = dt.Rows[0]["DNAME3"].ToString().Trim();
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {

                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            CS.Spread_Clear_Range(ss1, 0, 1, ss1_Sheet1.RowCount, 1);
            CS.Spread_Clear_Range(ss1, 0, 4, ss1_Sheet1.RowCount, ss1_Sheet1.ColumnCount - 4);

            strTIME = VB.Left(clsPublic.GstrSysDate, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2);
            strTIME = strTIME + VB.Left(clsPublic.GstrSysTime, 2) + VB.Right(clsPublic.GstrSysTime, 2) + "00";


            //'현재 시간을 표시함            
            ss1_Sheet1.Cells[0, 1].Text = strTIME;
            ss1_Sheet1.Cells[1, 1].Text = "37100068";
            ss1_Sheet1.Cells[4, 1].Text = "00";     //'소아중환자실            
            ss1_Sheet1.Cells[6, 1].Text = "00";     //'흉부외과중환자실                
            if (ss1_Sheet1.Cells[9, 1].Text == "") ss1_Sheet1.Cells[9, 1].Text = "사용가능"; //'CT
            if (ss1_Sheet1.Cells[10, 1].Text == "") ss1_Sheet1.Cells[10, 1].Text = "사용가능"; //'MRI
            if (ss1_Sheet1.Cells[11, 1].Text == "") ss1_Sheet1.Cells[11, 1].Text = "사용가능"; //'ANGIO
            if (ss1_Sheet1.Cells[12, 1].Text == "") ss1_Sheet1.Cells[12, 1].Text = "사용가능"; //'V/T


            try
            {
                //'병상현황을 읽음
                SQL = "SELECT WardCode,SUM(TBed) TBed ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_ROOM ";
                SQL = SQL + ComNum.VBLF + " WHERE TBed > 0 ";
                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE NOT IN ('564','565')  "; //'신생아중환자실 2013-06-27  , '6층 특실 제외
                SQL = SQL + ComNum.VBLF + "   AND WardCode NOT IN ('2W','3A','IQ','IU','ND','NR','32','33','35') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY WardCode ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nRow = 0;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow = nRow + 1;
                        if (nRow > ss1_Sheet1.RowCount)
                        {
                            ss1_Sheet1.RowCount = nRow;
                        }
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["TBed"].ToString().Trim();
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
            nLastRow = nRow;


            try
            {
                //'병동별 입원,퇴원,재원환자수
                SQL = "SELECT WardCode,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano <  '90000000' ";     //'지병환자
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' ";     //'전산실연습                
                SQL = SQL + ComNum.VBLF + "   AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND OUTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND GBSTS = '0' ";
                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE NOT IN ('564','565')  "; //'신생아중환자실 2013-06-27
                SQL = SQL + ComNum.VBLF + "   AND WardCode NOT IN ('2W','3A','IQ','IU','ND','NR','32','33','35') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY WardCode ";
                if (chkIpwonWait.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " Union All";
                    SQL = SQL + ComNum.VBLF + " SELECT '대기', COUNT(*) CNT";
                    SQL = SQL + ComNum.VBLF + " From KOSMOS_PMPA.NUR_ER_PATIENT_IPWON_WAIT";
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '일반병실'";
                    SQL = SQL + ComNum.VBLF + " AND DELDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + " Group By '대기'";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDATA = dt.Rows[i]["WardCode"].ToString().Trim();
                        nCnt1 = (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        //'해당 병동을 찾음
                        nRow = 0;
                        for (j = 0; j < ss1_Sheet1.RowCount; j++)
                        {
                            if (ss1_Sheet1.Cells[j, 4].Text == strDATA)
                            {
                                nRow = j;
                                break;
                            }
                        }

                        if (nRow == 0)
                        {
                            nLastRow = nLastRow + 1;
                            nRow = nLastRow;
                            ss1_Sheet1.Cells[j, 4].Text = strDATA;
                            ss1_Sheet1.Cells[j, 5].Text = "0";
                        }

                        ss1_Sheet1.Cells[i, 6].Text = nCnt1.ToString();
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


            //'합계를 구함
            nTot1 = 0;
            nTot2 = 0;
            nTot3 = 0;
            nIcuBed = 0;
            nWardBed = 0;
            for (i = 0; i < nLastRow; i++)
            {
                nCnt1 = (int)VB.Val(ss1_Sheet1.Cells[i, 5].Text);
                nCNT2 = (int)VB.Val(ss1_Sheet1.Cells[i, 6].Text);
                ss1_Sheet1.Cells[i, 7].Text = (nCnt1 - nCNT2).ToString();

                if (ss1_Sheet1.Cells[i, 4].Text == "IU" || ss1_Sheet1.Cells[i, 4].Text == "32" ||
                   ss1_Sheet1.Cells[i, 4].Text == "33" || ss1_Sheet1.Cells[i, 4].Text == "35")
                {
                    nIcuBed = nIcuBed + (nCnt1 - nCNT2);   //'ICU 여유병상
                }
                else if (ss1_Sheet1.Cells[i, 4].Text != "IQ" && ss1_Sheet1.Cells[i, 4].Text != "ND" &&
                        ss1_Sheet1.Cells[i, 4].Text != "HU" && ss1_Sheet1.Cells[i, 4].Text != "NR")
                {
                    nWardBed = nWardBed + (nCnt1 - nCNT2); //'일반병동 여유병상 (신생아실,인큐베이타,호스피스제외)
                }

                nTot1 = nTot1 + nCnt1;
                nTot2 = nTot2 + nCNT2;
                nTot3 = nTot1 - nTot2;
            }

            ss1_Sheet1.Cells[nLastRow, 4].Text = "합계";
            ss1_Sheet1.Cells[nLastRow, 5].Text = nTot1.ToString();
            ss1_Sheet1.Cells[nLastRow, 6].Text = nTot2.ToString();
            ss1_Sheet1.Cells[nLastRow, 7].Text = nTot3.ToString();


            try
            {
                //'일반중환자실                
                SQL = "SELECT SUM(TBed) TBed ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_ROOM ";
                SQL = SQL + ComNum.VBLF + " WHERE TBed > 0 ";
                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE IN ('233','234','321','320','301','302','303',";
                SQL = SQL + ComNum.VBLF + "  '304','305','330','331','332','333','334','335','350','351','352','353','354') ";
                SQL = SQL + ComNum.VBLF + "   AND WardCode NOT IN ('2W') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nCnt1 = (int)VB.Val(dt.Rows[0]["TBED"].ToString().Trim());
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



            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUM(CNT) CNT";
                SQL = SQL + ComNum.VBLF + "  FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT COUNT(PANO) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano <  '90000000' ";      //'지병환자
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' ";      //'전산실연습
                SQL = SQL + ComNum.VBLF + "   AND GBSTS NOT IN ('1','7','9')";
                SQL = SQL + ComNum.VBLF + "   AND ( OUTDATE IS NULL OR OUTDATE >=TRUNC(SYSDATE) ) "; //'당일은 보임.2011-09-29 
                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE IN ('233','234','321','320','301','302','303',";
                SQL = SQL + ComNum.VBLF + " '304','305','330','331','332','333','334','335','350','351','352','353','354') ";

                if (chkIpwonWait.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " Union All";
                    SQL = SQL + ComNum.VBLF + " SELECT  COUNT(*) CNT";
                    SQL = SQL + ComNum.VBLF + " From KOSMOS_PMPA.NUR_ER_PATIENT_IPWON_WAIT";
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '중환자실'";
                    SQL = SQL + ComNum.VBLF + " AND DELDATE IS NULL";
                }
                SQL = SQL + ComNum.VBLF + " )";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nCnt1 = (int)VB.Val(dt.Rows[0]["CNT"].ToString().Trim());
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

            nIcuBed = nCnt1 - nCNT2;

            //'전송용 자료에 Display
            ss1_Sheet1.Cells[7, 1].Text = nIcuBed.ToString();           //'일반중환자실 여유병상
            ss1_Sheet1.Cells[13, 1].Text = nWardBed.ToString();         //'입원실가용

            //'ICU 현황
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 4, 1].Text = "ICU";
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 4, 1].Text = nCnt1.ToString();
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 4, 1].Text = nCNT2.ToString();
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 4, 1].Text = nIcuBed.ToString();


            try
            {
                //'신생아 중환자실
                SQL = "SELECT WardCode,SUM(TBed) TBed ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_ROOM ";
                SQL = SQL + ComNum.VBLF + " WHERE TBed > 0 ";
                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE = '564' "; //'신생아중환자실 2013-06-27
                SQL = SQL + ComNum.VBLF + "   AND WardCode NOT IN ('2W') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY WardCode ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nCnt1 = (int)VB.Val(dt.Rows[0]["TBED"].ToString().Trim());
                }
                else
                {
                    nCnt1 = 0;
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


            try
            {
                SQL = "SELECT COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano <  '90000000' "; //'지병환자
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' "; //'전산실연습
                SQL = SQL + ComNum.VBLF + "   AND AmSet1 = '0' ";       //'재원환자
                SQL = SQL + ComNum.VBLF + "   AND AmSet4 <> '3' ";      //'정상애기
                //'=====================================================================
                //'2015-01-09 계장 김현욱 수정(의뢰서)
                //'    SQL = SQL & "  AND OUTDATE IS NULL "
                //'    SQL = SQL & "  AND GBSTS = '0' "
                SQL = SQL + ComNum.VBLF + "   AND GBSTS NOT IN ('1','7','9')";
                SQL = SQL + ComNum.VBLF + "   AND ( OUTDATE IS NULL OR OUTDATE >=TRUNC(SYSDATE) ) ";  //'당일은 보임.2011-09-29
                //'=====================================================================
                //'SQL = SQL & "  AND AmSet6 <> ' * ' "      '통계제외
                //'SQL = SQL & "  AND ROOMCODE = '368' "  '신생아중환자실
                //'SQL = SQL & "  AND ROOMCODE = '641' "  '신생아중환자실 2009 - 12 - 12
                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE = '564' ";   //'신생아중환자실 2013-06-27
                SQL = SQL + ComNum.VBLF + " GROUP BY WardCode ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nCNT2 = (int)VB.Val(dt.Rows[0]["CNT"].ToString().Trim());
                    ss1_Sheet1.Cells[5, 1].Text = (nCnt1 - nCNT2).ToString();
                }
                else
                {
                    nCNT2 = 0;
                    ss1_Sheet1.Cells[5, 1].Text = nCnt1.ToString();
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



            //'신생아중환자 현황
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 4, 4].Text = "IU(신)";
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 4, 5].Text = nCnt1.ToString();
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 4, 6].Text = nCNT2.ToString();
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 4, 7].Text = (nCnt1 - nCNT2).ToString();


            try
            {
                //'수술실 현황
                SQL = "SELECT COUNT(GUBUN) Cnt FROM KOSMOS_PMPA.ORAN_GUIDE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN IN('2','3','4','5') ";  //'대기중, 회복중 이외 전체 '2014 - 03 - 25 김현욱 수정
                //'SQL = SQL & "WHERE GUBUN IN('1','2','4') " '검사중, 수술중, 대기중

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nCnt1 = (int)VB.Val(dt.Rows[0]["Cnt"].ToString().Trim());
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

            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 3, 4].Text = "수술실";
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 3, 5].Text = "9";
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 3, 6].Text = nCnt1.ToString();
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 3, 7].Text = (9 - nCnt1).ToString();
            ss1_Sheet1.Cells[3, 1].Text = (9 - nCnt1).ToString();



            try
            {
                //'응급실 병상수
                SQL = "SELECT CODE,JIK FROM KOSMOS_PMPA.NUR_CODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN ='7' ";
                SQL = SQL + ComNum.VBLF + " AND CODE = 'ER' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nCnt1 = (int)VB.Val(dt.Rows[0]["Jik"].ToString().Trim());
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

            try
            {
                //'응급실 환자 구함
                SQL = " SELECT COUNT(PANO) Cnt From KOSMOS_PMPA.OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TRUNC(SYSDATE-2) ";
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = 'ER' ";
                SQL = SQL + ComNum.VBLF + "   AND (OcsJin Is Null Or OcsJin <>'#')";   //'접수하면 바로 인원추가됨
                //'''SQL = SQL & "  AND OCSJIN = ' * ' "  2009-03-12

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nCNT2 = (int)VB.Val(dt.Rows[0]["Cnt"].ToString().Trim());
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

            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 2, 4].Text = "응급실";
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 2, 5].Text = nCnt1.ToString();
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 2, 6].Text = nCNT2.ToString();
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 2, 7].Text = (nCnt1 - nCNT2).ToString();
            ss1_Sheet1.Cells[2, 1].Text = (nCnt1 - nCNT2).ToString();


            try
            {
                //'장례식장 현재 안치자
                SQL = "SELECT COUNT(*) CNT1 FROM KOSMOS_PMPA.ETC_JANGGUIDE ";
                SQL = SQL + ComNum.VBLF + " WHERE GbOut='N' ";
                SQL = SQL + ComNum.VBLF + "   AND Date2>=TRUNC(SYSDATE-10) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nCnt1 = (int)VB.Val(dt.Rows[0]["CNT1"].ToString().Trim());
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

            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 4].Text = "장례";
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 6].Text = nCnt1.ToString();

        }

        private void mnuView_Click(object sender, EventArgs e)
        {            
            ComFunc.ReadSysDate(clsDB.DbCon);
            txtDate.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            Data_Display();
        }

        private void mnuSend_Click(object sender, EventArgs e)
        {            
            ComFunc.ReadSysDate(clsDB.DbCon);
            txtDate.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            Data_Display();
            Data_SEND();
            FnTimer = 0;
        }

        //'자료를 전송함
        private bool Data_SEND()
        {
            bool rtnVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            bool bRet = false;
            string strREC = "";
            string strDat = "";
            string strTIME = "";
            string strFilename = "";
            string[] strSend = new string[14];

            Ftpedt FTP = new Ftpedt();

            //'현재 표시된 내용을 옆칸으로 이동 및 변수에 보관
            strREC = "";
            for (i = 0; i < 14; i++)
            {
                strDat = ss1_Sheet1.Cells[i, 1].Text;
                ss1_Sheet1.Cells[i, 2].Text = strDat;

                if (i <= 8 || i > 12)
                {
                    strREC = strREC + "#" + (strDat == "00" ? "" : strDat);
                    strSend[i] = strDat;
                }
                else
                {
                    if (strDat == "사용가능")
                    {
                        strREC = strREC + "#Y";
                        strSend[i] = "Y";
                    }
                    else
                    {
                        strREC = strREC + "#N";
                        strSend[i] = "N";
                    }
                }
                if (i == 13) strREC = strREC + "#";
            }


            //'전송할 자료를 파일에 저장함
            strSend[0] = VB.Left(strSend[0], 4) + "-" + VB.Mid(strSend[0], 5, 2) + "-" + VB.Mid(strSend[0], 7, 2) + " ";
            strSend[0] = strSend[0] + VB.Mid(strSend[0], 9, 2) + ":" + VB.Mid(strSend[0], 11, 2);
            strTIME = VB.Right(ss1_Sheet1.Cells[0, 1].Text, 6);

            //파일생성
            strFilename = "C:\\37100068" + strTIME;
            FileInfo file = new FileInfo(strFilename);
            if (!file.Exists)
            {
                FileStream fc = file.Create();
                fc.Close();
            }

            //파일쓰기
            FileStream fs = file.OpenWrite();
            TextWriter tw = new StreamWriter(fs);
            tw.Write(strREC);
            tw.Close();
            fs.Close();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            
            if (FTP.FtpConnetBatch(EMC_SERVERIP, EMC_SERVERID, EMC_SERVERPW) == false)            
            {
                try
                {
                    txtLog.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + " : 중앙응급의료센터 서버 접속 오류" + ComNum.VBLF + txtLog.Text;
                    if (VB.Len(txtLog.Text) > 2000) txtLog.Text = VB.Left(txtLog.Text, 2000);

                    SQL = " INSERT INTO ETC_EMC(SDATE,ERCNT,ORCNT,PDIU,NRIU,CSIU,ICU,ERNAME,CT,";
                    SQL = SQL + ComNum.VBLF + " MRI,ANGIO,VT,ROOM,SREMARK,SEND) VALUES (";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strSend[1] + "','YYYY-MM-DD HH24:MI'), '" + strSend[3] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSend[4] + "',  '" + strSend[5] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSend[6] + "', '" + strSend[7] + "', '" + strSend[8] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSend[9] + "', '" + strSend[10] + "', '" + strSend[11] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSend[12] + "', '" + strSend[13] + "', '" + strSend[14] + "', ";
                    SQL = SQL + ComNum.VBLF + " '1339 서버 접속 오류','0') ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("자료 추가시 에러 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
            }
            
            bRet = FTP.FtpUploadBatch(strFilename, VB.Trim(VB.Mid(strFilename, 4, 11)), "/");            
            if (bRet == false)
            {
                try
                {
                    txtLog.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + " : 1339 서버 ftp 전송 오류" + ComNum.VBLF + txtLog.Text;
                    if (VB.Len(txtLog.Text) > 2000) txtLog.Text = VB.Left(txtLog.Text, 2000);


                    SQL = " INSERT INTO ETC_EMC(SDATE,ERCNT,ORCNT,PDIU,NRIU,CSIU,ICU,ERNAME,CT,";
                    SQL = SQL + ComNum.VBLF + " MRI,ANGIO,VT,ROOM,SREMARK,SEND) VALUES (";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strSend[1] + "','YYYY-MM-DD HH24:MI'), '" + strSend[3] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSend[4] + "',  '" + strSend[5] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSend[6] + "', '" + strSend[7] + "', '" + strSend[8] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSend[9] + "', '" + strSend[10] + "', '" + strSend[11] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSend[12] + "', '" + strSend[13] + "', '" + strSend[14] + "', ";
                    SQL = SQL + ComNum.VBLF + " '1339 서버 ftp 전송 오류', '0') ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("자료 추가시 에러 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
            }
                        
            file.Delete();            
            try
            {
                SQL = " INSERT INTO ETC_EMC(SDATE,ERCNT,ORCNT,PDIU,NRIU,CSIU,ICU,ERNAME,CT,";
                SQL = SQL + ComNum.VBLF + " MRI,ANGIO,VT,ROOM,SREMARK, SEND) VALUES (";
                SQL = SQL + ComNum.VBLF + " TO_DATE('" + strSend[1] + "','YYYY-MM-DD HH24:MI'), '" + strSend[3] + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strSend[4] + "',  '" + strSend[5] + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strSend[6] + "', '" + strSend[7] + "', '" + strSend[8] + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strSend[9] + "', '" + strSend[10] + "', '" + strSend[11] + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strSend[12] + "', '" + strSend[13] + "', '" + strSend[14] + "', ";
                SQL = SQL + ComNum.VBLF + " '전송 완료','1') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("자료 추가시 에러 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
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
                return rtnVal;
            }
                        
            FTP.FtpDisConnetBatch();
            
            txtLog.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + " : 전송 OK ..." + ComNum.VBLF + txtLog.Text;
            if (VB.Len(txtLog.Text) > 2000) txtLog.Text = VB.Left(txtLog.Text, 2000);

            rtnVal = true;
            return rtnVal;
        }

        private void mnuSendSearch_Click(object sender, EventArgs e)
        {
            frmSendSearchProgram frmSendSearchprogramX = new frmSendSearchProgram();
            frmSendSearchprogramX.StartPosition = FormStartPosition.CenterParent;
            frmSendSearchprogramX.ShowDialog();
        }
    }
}
