using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComLibB;
using ComNurLibB;


namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmReservedMng_FM.cs
    /// Description     : 예약자 조회 및 예약변경
    /// Author          : 유진호
    /// Create Date     : 2018-01-12
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\Frm예약자관리용
    /// </history>
    /// </summary>
    public partial class frmReservedMng_FM : Form
    {
        ComFunc CF = new ComFunc();
        frmViewCsinfo frmViewCsinfoX = null;

        string FstrRDate = "";
        string FstrDrCode = "";
        string FstrROWID = "";
        string GstrDeptCode = "FM";

        public frmReservedMng_FM()
        {
            InitializeComponent();
        }

        public frmReservedMng_FM(string strDrCode)
        {
            InitializeComponent();
            FstrDrCode = strDrCode;
        }

        private void frmReservedMng_FM_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);

            ssView_Sheet1.Columns[7 + 1].Visible = false;
            ssView_Sheet1.Columns[8 + 1].Visible = false;
            ssView_Sheet1.Columns[11 + 1].Visible = false;
            ssView_Sheet1.Columns[12 + 1].Visible = false;

            ssReserve2_Sheet1.Columns[7].Visible = false;
            ssReserve2_Sheet1.Columns[8].Visible = false;

            SCREEN_CLEAR();
            SCREEN_CLEAR2();

            btnSearch.PerformClick();

            FstrROWID = "";

            dtpSDate1.Value = Convert.ToDateTime(clsPublic.GstrSysDate);


            // READ_FM_가예약조회 -> READ_FM_TEMPRESERVEDSEARCH
            READ_FM_TEMPRESERVEDSEARCH();
        }


        private string readLastComeDay(string strPano, string strBdate)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strRtn = "";

            try
            {

                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE FROM ( ";
                SQL += ComNum.VBLF + " SELECT MAX(BDATE) BDATE FROM KOSMOS_PMPA.OPD_MASTER ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                SQL += ComNum.VBLF + "   AND BDATE < TRUNC(TO_DATE('" + strBdate + "','YYYY-MM-DD') - 3) ";
                SQL += ComNum.VBLF + "   AND DEPTCODE = 'FM' ";
                SQL += ComNum.VBLF + "   AND DRCODE = '1404') ";
                //SQL += ComNum.VBLF + " WHERE TRUNC(TO_DATE('" + strBdate + "','YYYY-MM-DD') - 3) - TRUNC(BDATE) > 720 ";
                SQL += ComNum.VBLF + " WHERE TRUNC(TO_DATE('" + strBdate + "','YYYY-MM-DD') - 3) - TRUNC(BDATE) > 360 ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[i]["BDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return strRtn;

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
                return "";
            }
        }

        private void SCREEN_CLEAR()
        {
            int i = 0;
            int j = 0;

            ssList_Sheet1.Rows.Count = 0;

            for (i = 1; i < 4; i += 2)
            {
                for (j = 0; j < ssTime1_Sheet1.ColumnCount; j++)
                {
                    ssTime1_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        private void SCREEN_CLEAR2()
        {
            //txtRDate.Text = "";
            dtpRDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            txtRTime.Text = "";
            txtSname.Text = "";
            txtRemark.Text = "";
            txtDept.Text = "FM";
            txtDeptName.Text = "가정의학과";
            dtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            btnNew.Enabled = true;
            btnSet2.Enabled = false;
            btnDel.Enabled = false;
        }

        private void SCREEN_CLEAR3()
        {
            //txtRDate.Text = "";
            dtpRDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            txtRTime.Text = "";
            txtSname.Text = "";
            txtRemark.Text = "";

            FstrROWID = "";
        }

        private void READ_FM_TEMPRESERVEDSEARCH()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                ssReserve2_Sheet1.RowCount = 0;

                SQL = "";
                SQL = " SELECT SNAME,DEPTCODE,RDATE,RTIME,SDATE,REMARK,GUBUN,ENTPART,ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.ETC_FM_RESV ";
                SQL = SQL + ComNum.VBLF + "   WHERE SDATE = " + ComFunc.ConvOraToDate(dtpSDate1.Value, "D");
                SQL = SQL + ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY ENTDATE ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssReserve2_Sheet1.RowCount = dt.Rows.Count;
                    ssReserve2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssReserve2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        ssReserve2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssReserve2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["RTIME"].ToString().Trim();
                        ssReserve2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssReserve2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssReserve2_Sheet1.Cells[i, 5].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[i]["ENTPART"].ToString().Trim());
                        ssReserve2_Sheet1.Cells[i, 6].Text = "";
                        if (dt.Rows[i]["GUBUN"].ToString().Trim() == "1")
                        {
                            ssReserve2_Sheet1.Cells[i, 6].Text = "완료";
                        }
                        ssReserve2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssReserve2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;


                READ_OPD_DEPTCHOJIN();

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


        //READ_OPD_과초진 -> READ_OPD_DEPTCHOJIN
        private void READ_OPD_DEPTCHOJIN()
        {

            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSname = "";
            string strPtNo = "";
            string strDate = "";
            string strTime = "";
            //string strRCnt = "";
            //string strExam = "";
            string strChoJea = "";
            string strBiName = "";

            string strDeptCode = "";
            string strDrCode = "";

            int[,] nCnt2 = new int[4, 8];    //'과초진건수                
            int[] nSum = new int[4];

            try
            {
                for (i = 0; i < 4; i++)
                {
                    nSum[i] = 0;
                    for (j = 0; j < 8; j++)
                    {
                        nCnt2[i, j] = 0;
                    }
                }

                for (i = 1; i < 4; i += 2)
                {
                    for (j = 0; j < 9; j++)
                    {
                        ssTime2_Sheet1.Cells[i, j].Text = "";
                    }
                }

                FstrRDate = dtpSDate1.Value.ToShortDateString();

                

                SQL = "";
                SQL = "  SELECT  R.Sname, R.Pano Ptno, TO_CHAR(ACTDATE,'yy-mm-dd') Date33, R.DRCODE, R.DEPTCODE, R.BI, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(JTime,'HH24:Mi') RTime,TO_CHAR(ACTDATE,'YYYY-MM-DD') RDATE, R.Exam , TO_CHAR(L.LastDate,'YYYY-MM-DD') LASTDATE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER R, ";
                SQL = SQL + ComNum.VBLF + "      KOSMOS_PMPA.BAS_PATIENT  P, ";
                SQL = SQL + ComNum.VBLF + "      KOSMOS_PMPA.BAS_LASTEXAM L ";
                SQL = SQL + ComNum.VBLF + "WHERE ACTDATE    = TO_DATE('" + FstrRDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND R.DrCode        = '" + VB.Trim(FstrDrCode) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND R.Pano          = P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "  AND R.PANO = L.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND R.DEPTCODE = L.DEPTCODE(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY 7        ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    //ssView_Sheet1.RowCount = 0;
                    //ssView_Sheet1.RowCount = dt.Rows.Count;
                    //ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSname = dt.Rows[i]["Sname"].ToString().Trim();
                        strPtNo = dt.Rows[i]["Ptno"].ToString().Trim();
                        strDate = dt.Rows[i]["Date33"].ToString().Trim();
                        strTime = dt.Rows[i]["RTime"].ToString().Trim();
                        strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                        strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strBiName = CF.Read_Bi_Name(clsDB.DbCon, dt.Rows[i]["BI"].ToString().Trim(), "1");

                        strChoJea = "";

                        if (dt.Rows[i]["LastDate"].ToString().Trim() == "") strChoJea = "과초진";

                        if (dt.Rows[i]["LastDate"].ToString().Trim() == "")
                        {
                            strChoJea = READ_DEPT_CHOJEA(dt.Rows[i]["Ptno"].ToString().Trim(), strDeptCode, strDrCode);
                        }

                        Time_ADD2(dt, i, strChoJea, ref nCnt2);
                    }


                    //'과초진 설정값                    
                    for (i = 1; i < 4; i += 2)
                    {
                        for (j = 0; j < 8; j++)
                        {
                            if (nCnt2[i, j] != 0)
                            {
                                ssTime2_Sheet1.Cells[i, j].Text = ssTime2_Sheet1.Cells[i, j].Text + "(" + nCnt2[i, j] + ")";
                            }
                        }
                    }

                    //'소계표시
                    for (i = 1; i < 4; i += 2)
                    {
                        for (j = 0; j < 8; j++)
                        {
                            if (i == 1)
                            {
                                nSum[0] = nSum[0] + Convert.ToInt32(VB.Val(VB.Pstr(ssTime2_Sheet1.Cells[i, j].Text, "(", 1)));
                                nSum[1] = nSum[1] + Convert.ToInt32(VB.Val(VB.Pstr((VB.Pstr(ssTime2_Sheet1.Cells[i, j].Text, "(", 2)), ")", 1)));
                            }
                            else
                            {
                                nSum[2] = nSum[2] + Convert.ToInt32(VB.Val(VB.Pstr(ssTime2_Sheet1.Cells[i, j].Text, "(", 1)));
                                nSum[3] = nSum[3] + Convert.ToInt32(VB.Val(VB.Pstr((VB.Pstr(ssTime2_Sheet1.Cells[i, j].Text, "(", 2)), ")", 1)));
                            }
                        }
                    }

                    ssTime2_Sheet1.Cells[1, 8].Text = nSum[0] + "(" + nSum[1] + ")";
                    ssTime2_Sheet1.Cells[3, 8].Text = nSum[2] + "(" + nSum[3] + ")";
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

        private void Time_ADD2(DataTable dt, int index, string strChoJea, ref int[,] nCnt2)
        {

            int nRow = 0;
            int nCol = 0;
            int nCNT = 0;
            DateTime RTime;
            string strRTime = "";

            RTime = Convert.ToDateTime(dt.Rows[index]["RTime"].ToString().Trim());
            strRTime = dt.Rows[index]["RTime"].ToString().Trim();

            if (string.Compare(strRTime, "00:00") != -1 && string.Compare(strRTime, "09:00") != 1)
            {
                nRow = 1;
                nCol = 0;
            }
            else if (string.Compare(strRTime, "09:01") != -1 && string.Compare(strRTime, "09:30") != 1)
            {
                nRow = 1;
                nCol = 1;
            }
            else if (string.Compare(strRTime, "09:31") != -1 && string.Compare(strRTime, "10:00") != 1)
            {
                nRow = 1;
                nCol = 2;
            }
            else if (string.Compare(strRTime, "10:01") != -1 && string.Compare(strRTime, "10:30") != 1)
            {
                nRow = 1;
                nCol = 3;
            }
            else if (string.Compare(strRTime, "10:31") != -1 && string.Compare(strRTime, "11:00") != 1)
            {
                nRow = 1;
                nCol = 4;
            }
            else if (string.Compare(strRTime, "11:01") != -1 && string.Compare(strRTime, "11:30") != 1)
            {
                nRow = 1;
                nCol = 5;
            }
            else if (string.Compare(strRTime, "11:31") != -1 && string.Compare(strRTime, "12:00") != 1)
            {
                nRow = 1;
                nCol = 6;
            }
            else if (string.Compare(strRTime, "12:01") != -1 && string.Compare(strRTime, "12:30") != 1)
            {
                nRow = 1;
                nCol = 7;
            }
            else if (string.Compare(strRTime, "12:31") != -1 && string.Compare(strRTime, "13:00") != 1)
            {
                nRow = 3;
                nCol = 0;
            }
            else if (string.Compare(strRTime, "13:01") != -1 && string.Compare(strRTime, "13:30") != 1)
            {
                nRow = 3;
                nCol = 1;
            }
            else if (string.Compare(strRTime, "13:31") != -1 && string.Compare(strRTime, "14:00") != 1)
            {
                nRow = 3;
                nCol = 2;
            }
            else if (string.Compare(strRTime, "14:01") != -1 && string.Compare(strRTime, "14:30") != 1)
            {
                nRow = 3;
                nCol = 3;
            }
            else if (string.Compare(strRTime, "14:31") != -1 && string.Compare(strRTime, "15:00") != 1)
            {
                nRow = 3;
                nCol = 4;
            }
            else if (string.Compare(strRTime, "15:01") != -1 && string.Compare(strRTime, "15:30") != 1)
            {
                nRow = 3;
                nCol = 5;
            }
            else if (string.Compare(strRTime, "15:31") != -1 && string.Compare(strRTime, "16:00") != 1)
            {
                nRow = 3;
                nCol = 6;
            }
            else
            {
                nRow = 3;
                nCol = 7;
            }

            nCNT = (int)VB.Val(ssTime2_Sheet1.Cells[nRow, nCol].Text) + 1;

            ssTime2_Sheet1.Cells[nRow, nCol].Text = Convert.ToString(nCNT);


            if (strChoJea == "과초진")
            {
                nCnt2[nRow, nCol] = nCnt2[nRow, nCol] + 1;
            }
        }


        private string READ_DEPT_CHOJEA(string ArgPano, string ArgDeptCode, string ArgDrCode)
        {
            string rtnVal = "";
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = " SELECT ROWID FROM KOSMOS_PMPA.OPD_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = '" + ArgDeptCode + "'";
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE < TRUNC(SYSDATE) ";
                if (ArgDeptCode == "MD")
                {
                    if (ArgDrCode == "1107")
                    {
                        SQL = SQL + ComNum.VBLF + " AND DRCODE = '1107' ";
                    }
                    else if (ArgDrCode == "1125")
                    {
                        SQL = SQL + ComNum.VBLF + " AND DRCODE = '1125'  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND DRCODE NOT IN ('1107','1125') ";
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "";
                }
                else
                {
                    rtnVal = "과초진";
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
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
                return rtnVal;
            }
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR3();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인

            if (btnDelClick() == true)
            {
                SCREEN_CLEAR3();
                // READ_FM_가예약조회 -> READ_FM_TEMPRESERVEDSEARCH
                READ_FM_TEMPRESERVEDSEARCH();
            }
        }

        private bool btnDelClick()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrROWID == "")
                {
                    ComFunc.MsgBox("삭제처리할 대상을 선택후 작업하세요");
                    return rtVal;
                }

                SQL = " UPDATE KOSMOS_PMPA.ETC_FM_RESV SET  ";
                SQL = SQL + " DELDATE = SYSDATE ";
                SQL = SQL + "  WHERE ROWID ='" + FstrROWID + "' ";

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
                ComFunc.MsgBox("저장 하였습니다.");
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR3();
            btnSet2.Enabled = true;
            dtpRDate.Focus();
        }

        private void btnSet2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인

            if (btnSet2Click() == true)
            {
                SCREEN_CLEAR3();
                // READ_FM_가예약조회 -> READ_FM_TEMPRESERVEDSEARCH
                READ_FM_TEMPRESERVEDSEARCH();
            }
        }

        private bool btnSet2Click()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strRDate = "";
            string strRTime = "";
            string strSname = "";
            string strDEPT = "";
            string strSDATE = "";
            string strRemark = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                strRDate = dtpRDate.Value.ToShortDateString();
                strRTime = VB.Trim(txtRTime.Text);
                strRTime = VB.Replace(strRTime, ":", "");

                strSname = VB.Trim(txtPaName.Text);

                strDEPT = VB.Trim(txtDept.Text);
                strSDATE = dtpSDate.Value.ToShortDateString();

                strRemark = VB.Trim(txtRemark.Text);

                if (strRDate == "")
                {
                    ComFunc.MsgBox("예약일자 공란입니다..", "확인");
                    return rtVal;
                }

                if (strRTime == "")
                {
                    ComFunc.MsgBox("예약시간 공란입니다..", "확인");
                    return rtVal;
                }
                else if (VB.Len(strRTime) != 4)
                {
                    ComFunc.MsgBox("예약시간 형식오류(ex13:00)입니다..", "확인");
                    return rtVal;
                }

                if (strSname == "")
                {
                    ComFunc.MsgBox("성명 공란입니다..", "확인");
                    return rtVal;
                }

                if (FstrROWID == "")
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.ETC_FM_RESV ( DEPTCODE,SNAME,RDATE,RTIME,SDATE,REMARK,GUBUN,ENTDATE,ENTPART ) VALUES ( ";
                    SQL = SQL + " '" + strDEPT + "','" + strSname + "',TO_DATE('" + strRDate + "','YYYY-MM-DD'),'" + strRTime + "', ";
                    SQL = SQL + " TO_DATE('" + strSDATE + "','YYYY-MM-DD') ,'" + strRemark + "','0',SYSDATE," + clsType.User.Sabun + " ) ";
                }
                else
                {
                    SQL = " UPDATE KOSMOS_PMPA.ETC_FM_RESV ";
                    SQL = SQL + " RDATE =TO_DATE('" + strRDate + "','YYYY-MM-DD') , ";
                    SQL = SQL + " RTIME ='" + strRTime + "' , ";
                    SQL = SQL + " REMARK ='" + strRemark + "' , ";
                    SQL = SQL + " SNAME ='" + strSname + "' ";
                    SQL = SQL + "  WHERE ROWID ='" + FstrROWID + "' ";
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

                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
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

        private void btnView2_Click(object sender, EventArgs e)
        {
            // READ_FM_가예약조회 -> READ_FM_TEMPRESERVEDSEARCH
            READ_FM_TEMPRESERVEDSEARCH();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int nCnt1 = 0;
            int nCnt2 = 0;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SCREEN_CLEAR();

                SQL = "";
                SQL = "  SELECT TO_CHAR(DATE3,'YYYY-MM-DD') DATE11 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_RESERVED_NEW  ";
                SQL = SQL + ComNum.VBLF + " WHERE DATE3   >  TRUNC(SYSDATE + 1) ";
                SQL = SQL + ComNum.VBLF + "  AND DRCODE         =  '" + VB.Trim(FstrDrCode) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(DATE3,'YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + " UNION ";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') DATE11 ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_TELRESV";
                SQL = SQL + ComNum.VBLF + " WHERE RDATE > TRUNC(SYSDATE + 1)";
                SQL = SQL + ComNum.VBLF + " AND DRCODE         =  '" + VB.Trim(FstrDrCode) + "' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL = "  SELECT TO_CHAR(DATE3,'YYYY-MM-DD') DATE11,COUNT(PANO) R_CNT ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_RESERVED_NEW  ";
                        SQL = SQL + ComNum.VBLF + " WHERE DATE3   >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(dt.Rows[i]["DATE11"].ToString().Trim()), "D");
                        SQL = SQL + ComNum.VBLF + "  AND DATE3   < " + ComFunc.ConvOraToDate(Convert.ToDateTime(dt.Rows[i]["DATE11"].ToString().Trim()).AddDays(1), "D");
                        SQL = SQL + ComNum.VBLF + "  AND DRCODE         =  '" + VB.Trim(FstrDrCode) + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND TRANSDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "  AND RETDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(DATE3,'YYYY-MM-DD')  ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        nCnt1 = 0;
                        if (dt2.Rows.Count > 0)
                        {
                            nCnt1 = Convert.ToInt32(VB.Val(dt2.Rows[0]["R_CNT"].ToString().Trim()));
                        }
                        dt2.Dispose();
                        dt2 = null;

                        SQL = "";
                        SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, COUNT(PANO) CNT2 ";
                        SQL = SQL + "  FROM KOSMOS_PMPA.OPD_TELRESV ";
                        SQL = SQL + " WHERE RDATE = " + ComFunc.ConvOraToDate(Convert.ToDateTime(dt.Rows[i]["DATE11"].ToString().Trim()), "D");
                        SQL = SQL + " AND DRCODE         =  '" + VB.Trim(FstrDrCode) + "' ";
                        SQL = SQL + "GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD')  ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        nCnt2 = 0;
                        if (dt2.Rows.Count > 0)
                        {
                            nCnt2 = Convert.ToInt32(VB.Val(dt2.Rows[0]["CNT2"].ToString().Trim()));
                        }
                        dt2.Dispose();
                        dt2 = null;

                        ssList_Sheet1.RowCount = ssList_Sheet1.RowCount + 1;
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["DATE11"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text + ComFunc.LPAD(Convert.ToString(nCnt1 + nCnt2) + "명", 10, " ");
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text + VB.Space(10) + VB.Left(CF.READ_YOIL(clsDB.DbCon, dt.Rows[i]["DATE11"].ToString().Trim()), 1);

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

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            clearTIME1();
        }

        private void clearTIME1()
        {
            int i = 0;
            int j = 0;

            for (i = 1; i < 4; i += 2)
            {
                for (j = 0; j < ssTime1_Sheet1.ColumnCount; j++)
                {
                    ssTime1_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            int i = 0;
            int j = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSname = "";
            string strPtNo = "";
            string strDate = "";
            string strTime = "";
            //string strRCnt = "";
            string strExam = "";
            string strChoJea = "";
            string strBiName = "";

            string strDeptCode = "";
            string strDrCode = "";

            int[,] nCnt2 = new int[4, 8];    //'과초진건수                
            int[] nSum = new int[4];

            try
            {
                for (i = 0; i < 4; i++)
                {
                    nSum[i] = 0;
                    for (j = 0; j < 8; j++)
                    {
                        nCnt2[i, j] = 0;
                    }
                }

                clearTIME1();

                if (e.ColumnHeader == true) return;

                FstrRDate = VB.Left(ssList_Sheet1.Cells[e.Row, 0].Text, 10);

                gboReserved.Text = FstrRDate + " " + CF.READ_YOIL(clsDB.DbCon, FstrRDate) + " 예약현황";

                SQL = "";
                SQL = SQL + ComNum.VBLF + "";

                SQL = "  SELECT  'RES' GBN,R.SNAME, R.PANO PTNO, TO_CHAR(DATE1,'YY-MM-DD') DATE33, R.DRCODE, R.DEPTCODE, R.BI, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(DATE3,'HH24:MI') RTIME,TO_CHAR(DATE3,'YYYY-MM-DD') RDATE, R.EXAM , TO_CHAR(L.LASTDATE,'YYYY-MM-DD') LASTDATE,R.GWACHOJAE , R.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_RESERVED_NEW R, ";
                SQL = SQL + ComNum.VBLF + "      KOSMOS_PMPA.BAS_PATIENT  P, ";
                SQL = SQL + ComNum.VBLF + "      KOSMOS_PMPA.BAS_LASTEXAM L ";
                SQL = SQL + ComNum.VBLF + "WHERE DATE3    >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(FstrRDate), "D");
                SQL = SQL + ComNum.VBLF + "  AND DATE3    <  " + ComFunc.ConvOraToDate(Convert.ToDateTime(FstrRDate).AddDays(1), "D");
                SQL = SQL + ComNum.VBLF + "  AND R.DRCODE        = '" + VB.Trim(FstrDrCode) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND R.TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND R.RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND R.PANO          = P.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND R.PANO = L.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND R.DEPTCODE = L.DEPTCODE(+) ";

                if (clsType.User.DeptCode == "MD") SQL = SQL + "  AND R.DRCODE = L.DRCODE(+) ";

                SQL = SQL + ComNum.VBLF + "  UNION ALL ";

                SQL = SQL + ComNum.VBLF + "   SELECT  'TEL' GBN, R.SNAME, R.PANO PTNO, TO_CHAR(R.ENTDATE,'YY-MM-DD') DATE33, R.DRCODE, R.DEPTCODE,'00' BI,";
                SQL = SQL + ComNum.VBLF + " RTIME RTIME,TO_CHAR(R.RDATE,'YYYY-MM-DD') RDATE, '00' EXAM , TO_CHAR(L.LASTDATE,'YYYY-MM-DD') LASTDATE,R.GWACHOJAE,R.ROWID";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_TELRESV R,       KOSMOS_PMPA.BAS_PATIENT  P,       KOSMOS_PMPA.BAS_LASTEXAM L";
                SQL = SQL + ComNum.VBLF + " WHERE RDATE   = " + ComFunc.ConvOraToDate(Convert.ToDateTime(FstrRDate), "D");
                SQL = SQL + ComNum.VBLF + " AND R.DRCODE        = '" + VB.Trim(FstrDrCode) + "' ";
                SQL = SQL + ComNum.VBLF + " AND R.PANO= P.PANO(+)";
                SQL = SQL + ComNum.VBLF + " AND R.PANO = L.PANO(+)";
                SQL = SQL + ComNum.VBLF + " AND R.DEPTCODE = L.DEPTCODE(+)";
                SQL = SQL + ComNum.VBLF + "ORDER BY 8        ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSname = dt.Rows[i]["Sname"].ToString().Trim();
                        strPtNo = dt.Rows[i]["Ptno"].ToString().Trim();
                        strDate = dt.Rows[i]["Date33"].ToString().Trim();
                        strTime = dt.Rows[i]["RTime"].ToString().Trim();
                        strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                        strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strBiName = CF.Read_Bi_Name(clsDB.DbCon, dt.Rows[i]["BI"].ToString().Trim(), "1");

                        strChoJea = "";

                        //'2014-06-02
                        if (VB.Trim(dt.Rows[i]["GwaChojae"].ToString().Trim()) == "C")
                        {
                            strChoJea = "과초진";
                        }
                        else
                        {
                            if (VB.Trim(dt.Rows[i]["LastDate"].ToString().Trim()) == "")
                            {
                                strChoJea = "과초진";
                                strChoJea = READ_DEPT_CHOJEA(dt.Rows[i]["Ptno"].ToString().Trim(), strDeptCode, strDrCode);
                            }
                        }

                        strExam = (dt.Rows[i]["EXAM"].ToString().Trim() == "Y" ? "◎" : "");

                        ssView_Sheet1.Cells[i, 0].Text = strPtNo;
                        ssView_Sheet1.Cells[i, 1].Text = strSname;
                        ssView_Sheet1.Cells[i, 2].Text = strDate;
                        ssView_Sheet1.Cells[i, 3].Text = strTime;
                        ssView_Sheet1.Cells[i, 4].Text = strChoJea;
                        ssView_Sheet1.Cells[i, 5].Text = readLastComeDay(strPtNo, FstrRDate);
                        ssView_Sheet1.Cells[i, 5 + 1].Text = strExam;
                        ssView_Sheet1.Cells[i, 6 + 1].Text = strBiName;
                        ssView_Sheet1.Cells[i, 7 + 1].Text = strDeptCode;
                        ssView_Sheet1.Cells[i, 8 + 1].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 11 + 1].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 12 + 1].Text = dt.Rows[i]["GwaChojae"].ToString().Trim();


                        Time_ADD1(dt, i, strChoJea, ref nCnt2);


                        //'재원 체크
                        SQL = "SELECT PANO,DEPTCODE FROM  KOSMOS_PMPA.IPD_NEW_MASTER ";
                        SQL = SQL + ComNum.VBLF + " WHERE  PANO = '" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND OUTDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "  AND GBSTS <> '9' ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[i, 9 + 1].Text = "입원중";
                        }

                        if (VB.Trim(dt.Rows[i]["GBN"].ToString().Trim()) == "TEL")
                        {
                            ssView_Sheet1.Cells[i, 10 + 1].Text = "☎";
                        }
                    }
                }

                dt.Dispose();
                dt = null;



                //'과초진 설정값
                for (i = 1; i < 4; i += 2)
                {
                    for (j = 0; j < 8; j++)
                    {
                        if (nCnt2[i, j] != 0)
                        {
                            ssTime1_Sheet1.Cells[i, j].Text = ssTime1_Sheet1.Cells[i, j].Text + "(" + nCnt2[i, j] + ")";
                        }
                    }
                }


                //'소계표시
                for (i = 1; i < 4; i += 2)
                {
                    for (j = 0; j < 8; j++)
                    {
                        if (i == 1)
                        {
                            nSum[0] = nSum[0] + Convert.ToInt32(VB.Val(VB.Pstr(ssTime1_Sheet1.Cells[i, j].Text, "(", 1)));
                            nSum[1] = nSum[1] + Convert.ToInt32(VB.Val(VB.Pstr((VB.Pstr(ssTime1_Sheet1.Cells[i, j].Text, "(", 2)), ")", 1)));
                        }
                        else
                        {
                            nSum[2] = nSum[2] + Convert.ToInt32(VB.Val(VB.Pstr(ssTime1_Sheet1.Cells[i, j].Text, "(", 1)));
                            nSum[3] = nSum[3] + Convert.ToInt32(VB.Val(VB.Pstr((VB.Pstr(ssTime1_Sheet1.Cells[i, j].Text, "(", 2)), ")", 1)));
                        }
                    }
                }

                ssTime1_Sheet1.Cells[1, 8].Text = nSum[0] + "(" + nSum[1] + ")";
                ssTime1_Sheet1.Cells[3, 8].Text = nSum[2] + "(" + nSum[3] + ")";
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

        private void Time_ADD1(DataTable dt, int index, string strChoJea, ref int[,] nCnt2)
        {

            int nRow = 0;
            int nCol = 0;
            int nCNT = 0;
            DateTime RTime;
            string strRTime = "";

            RTime = Convert.ToDateTime(dt.Rows[index]["RTime"].ToString().Trim());
            strRTime = dt.Rows[index]["RTime"].ToString().Trim();

            if (string.Compare(strRTime, "00:00") != -1 && string.Compare(strRTime, "09:29") != 1)
            {
                nRow = 1;
                nCol = 0;
            }
            else if (string.Compare(strRTime, "09:30") != -1 && string.Compare(strRTime, "09:59") != 1)
            {
                nRow = 1;
                nCol = 1;
            }
            else if (string.Compare(strRTime, "10:00") != -1 && string.Compare(strRTime, "10:29") != 1)
            {
                nRow = 1;
                nCol = 2;
            }
            else if (string.Compare(strRTime, "10:30") != -1 && string.Compare(strRTime, "10:59") != 1)
            {
                nRow = 1;
                nCol = 3;
            }
            else if (string.Compare(strRTime, "11:00") != -1 && string.Compare(strRTime, "11:29") != 1)
            {
                nRow = 1;
                nCol = 4;
            }
            else if (string.Compare(strRTime, "11:30") != -1 && string.Compare(strRTime, "11:59") != 1)
            {
                nRow = 1;
                nCol = 5;
            }
            else if (string.Compare(strRTime, "12:00") != -1 && string.Compare(strRTime, "12:29") != 1)
            {
                nRow = 1;
                nCol = 6;
            }
            else if (string.Compare(strRTime, "12:30") != -1 && string.Compare(strRTime, "12:59") != 1)
            {
                nRow = 1;
                nCol = 7;
            }
            else if (string.Compare(strRTime, "13:00") != -1 && string.Compare(strRTime, "13:29") != 1)
            {
                nRow = 3;
                nCol = 0;
            }
            else if (string.Compare(strRTime, "13:30") != -1 && string.Compare(strRTime, "13:59") != 1)
            {
                nRow = 3;
                nCol = 1;
            }
            else if (string.Compare(strRTime, "14:00") != -1 && string.Compare(strRTime, "14:29") != 1)
            {
                nRow = 3;
                nCol = 2;
            }
            else if (string.Compare(strRTime, "14:30") != -1 && string.Compare(strRTime, "14:59") != 1)
            {
                nRow = 3;
                nCol = 3;
            }
            else if (string.Compare(strRTime, "15:00") != -1 && string.Compare(strRTime, "15:29") != 1)
            {
                nRow = 3;
                nCol = 4;
            }
            else if (string.Compare(strRTime, "15:30") != -1 && string.Compare(strRTime, "15:59") != 1)
            {
                nRow = 3;
                nCol = 5;
            }
            else if (string.Compare(strRTime, "16:00") != -1 && string.Compare(strRTime, "16:29") != 1)
            {
                nRow = 3;
                nCol = 6;
            }
            else if (string.Compare(strRTime, "16:30") != -1 && string.Compare(strRTime, "23:59") != 1)
            {
                nRow = 3;
                nCol = 7;
            }
            else
            {
                nRow = 3;
                nCol = 7;
            }

            nCNT = (int)VB.Val(ssTime1_Sheet1.Cells[nRow, nCol].Text) + 1;

            ssTime1_Sheet1.Cells[nRow, nCol].Text = Convert.ToString(nCNT);


            if (strChoJea == "과초진")
            {
                nCnt2[nRow, nCol] = nCnt2[nRow, nCol] + 1;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            if (ssView_Sheet1.RowCount == 0)
            {
                ComFunc.MsgBox("예약자가 없습니다.");
                return;
            }

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C 예약자 명단" + "/n/n/n/n";
            strHead2 = "/l/f2" + "예약일자 : " + VB.Left(gboReserved.Text, 10) + VB.Space(10) + "인쇄일자 : " + SysDate;

            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 35;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 35;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void ssReserve2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == false)
            {
                FstrROWID = "";

                FstrROWID = ssReserve2_Sheet1.Cells[e.Row, 8].Text;
                dtpRDate.Value = Convert.ToDateTime(ssReserve2_Sheet1.Cells[e.Row, 1].Text);
                txtRTime.Text = VB.Left(ssReserve2_Sheet1.Cells[e.Row, 2].Text, 2) + ":" + VB.Right(ssReserve2_Sheet1.Cells[e.Row, 2].Text, 2);
                txtSname.Text = ssReserve2_Sheet1.Cells[e.Row, 3].Text;
                txtRemark.Text = ssReserve2_Sheet1.Cells[e.Row, 7].Text;
                if (VB.Trim(ssReserve2_Sheet1.Cells[e.Row, 6].Text) == "완료")
                {
                    ComFunc.MsgBox("이미 원무에서 예약작업을 완료한 자료입니다...", "확인");
                }

                btnSet2.Enabled = true;
                btnDel.Enabled = true;
            }
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true || e.RowHeader == true) return;

            Read_Reserved(e.Row);
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인

            if (e.RowHeader == true || e.ColumnHeader == true) return;
            ssViewCellDoubleClick(e.Row, e.Column);
        }

        private bool ssViewCellDoubleClick(int row, int col)
        {
            bool rtVal = false;
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string GstrPANO = "";
            string GstrDate = "";
            string GstrTime = "";

            string strRDate1 = "";
            string strRTime1 = "";
            string strPtNo = "";
            string strDEPT = "";
            string StrJin = "";

            //string strTime = "";
            string strTelGbn = "";
            string strROWID = "";

            string strDeptChojin = "";
            string strGubun = "";

            string strMsg = "";

            
            if (col == 1)
            {
                GstrPANO = ssView_Sheet1.Cells[row, 0].Text;

                if (frmViewCsinfoX != null)
                {
                    frmViewCsinfoX.Dispose();
                    frmViewCsinfoX = null;
                }
                frmViewCsinfoX = new frmViewCsinfo(GstrPANO);
                frmViewCsinfoX.StartPosition = FormStartPosition.CenterParent;
                frmViewCsinfoX.ShowDialog();
                return rtVal;
            }

            Read_Reserved(row);


            GstrDate = FstrRDate;
            GstrTime = ssView_Sheet1.Cells[row, 3].Text;
            strPtNo = ssView_Sheet1.Cells[row, 0].Text;
            strGubun = ssView_Sheet1.Cells[row, 4].Text;
            strDEPT = ssView_Sheet1.Cells[row, 7 + 1].Text;
            strTelGbn = ssView_Sheet1.Cells[row, 8 + 1].Text;
            strTelGbn = ssView_Sheet1.Cells[row, 10 + 1].Text;
            strROWID = ssView_Sheet1.Cells[row, 11 + 1].Text;
            strDeptChojin = ssView_Sheet1.Cells[row, 12 + 1].Text;


            if (col == 4 && row >= 0)
            {
                if (strTelGbn == "☎")
                {
                    if (strDeptChojin == "C" || strGubun == "과초진")
                    {
                        if (ComFunc.MsgBoxQ("선택하신 자료를 초진->>재진으로 설정하시겠습니까??", "재진설정", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            clsDB.setBeginTran(clsDB.DbCon);

                            try
                            {
                                SQL = " UPDATE KOSMOS_PMPA.OPD_TELRESV SET ";
                                SQL = SQL + ComNum.VBLF + "       GWACHOJAE = 'J' ";
                                SQL = SQL + ComNum.VBLF + " WHERE PANO         = '" + strPtNo + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND ROWID ='" + strROWID + "' ";
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
                        else
                        {
                            return rtVal;
                        }
                    }
                    else
                    {
                        if (ComFunc.MsgBoxQ("선택하신 자료를 재진->>과초진으로 설정하시겠습니까??", "재진설정", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            clsDB.setBeginTran(clsDB.DbCon);

                            try
                            {
                                SQL = " UPDATE KOSMOS_PMPA.OPD_TELRESV SET ";
                                SQL = SQL + ComNum.VBLF + "       GWACHOJAE = 'C' ";
                                SQL = SQL + ComNum.VBLF + " WHERE PANO         = '" + strPtNo + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND ROWID ='" + strROWID + "' ";
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
                        else
                        {
                            return rtVal;
                        }
                    }
                }
                else
                {
                    ComFunc.MsgBox("전화예약만 수정 가능합니다..", "확인");
                }

                btnSearch.PerformClick();
                rtVal = true;
                return rtVal;
            }

            clsPublic.GstrDate = GstrDate + " " + GstrTime;

            frmCalendar1Time frmCalendar1TimeX = new frmCalendar1Time();
            frmCalendar1TimeX.StartPosition = FormStartPosition.CenterParent;
            frmCalendar1TimeX.ShowDialog();

            if (clsPublic.GstrDate == "취소")
            {
                return rtVal;
            }
            else
            {
                //GstrDate = Convert.ToDateTime(clsPublic.GstrDate).ToString("yyyy-MM-dd");
                //GstrTime = Convert.ToDateTime(clsPublic.GstrTime).ToString("HH:mm");

                if (VB.IsDate(clsPublic.GstrDate + " " + clsPublic.GstrTime) == true)
                {
                    DateTime dtTemp = Convert.ToDateTime(clsPublic.GstrDate + " " + clsPublic.GstrTime);

                    GstrDate = dtTemp.ToShortDateString();
                    GstrTime = dtTemp.ToString("HH:mm");
                }
            }
            

            if (GstrDeptCode != "GS")
            {
                //'공휴일 체크 및 진료 점검
                SQL = "SELECT GBJIN, GBJIN2";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + " WHERE SCHDATE= TO_DATE('" + GstrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DRCODE = '" + FstrDrCode + "'     ";
                if (VB.Val(VB.Left(GstrTime, 2)) <= VB.Val("12")) SQL = SQL + ComNum.VBLF + "  AND GBJIN <>'1' ";
                if (VB.Val(VB.Left(GstrTime, 2)) > VB.Val("12")) SQL = SQL + ComNum.VBLF + "   AND GBJIN2 <> '1'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {                    
                    //'   ------------------------------------------------
                    //'   1.진료 2.수술 3.특수검사 4.휴진,
                    //'   5.학회 6.휴가 7.출장 8.기타, 9.OFF(주40시간)

                    if (VB.Val(VB.Left(GstrTime, 2)) <= VB.Val("12"))
                    {
                        StrJin = dt.Rows[0]["GBJin"].ToString().Trim();
                    }
                    else
                    {
                        StrJin = dt.Rows[0]["GBJin1"].ToString().Trim();
                    }

                    switch (StrJin)
                    {
                        case "1": StrJin = "진료"; break;
                        case "2": StrJin = "수술"; break;
                        case "3": StrJin = "특수검사"; break;
                        case "4": StrJin = "휴진"; break;
                        case "5": StrJin = "학회"; break;
                        case "6": StrJin = "휴가"; break;
                        case "7": StrJin = "출장"; break;
                        case "8": StrJin = "기타"; break;
                        case "9": StrJin = "OFF(주40시간)"; break;
                    }

                    if (GstrDeptCode == "NP")
                    {
                        strMsg = FstrRDate + " " + GstrTime + "  에는 해당과장님이[" + StrJin + "] 입니다 다른 날짜 시간을 선택해주세요";
                        strMsg = strMsg + ComNum.VBLF + ComNum.VBLF + "무시하고 예약 변경을 하시겠습니까 ?";


                        if (ComFunc.MsgBoxQ(strMsg, "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                        {
                            return rtVal;
                        }
                    }
                    else
                    {
                        ComFunc.MsgBox(FstrRDate + " " + GstrTime + "  에는 해당과장님이 [ " + StrJin + " ] 입니다 다른 날짜 시간을 선택해주세요 ");
                        return rtVal;
                    }
                }
            }



            //'같은과에 같은 날에 예약 불가능 하게 처리함.
            if (FstrRDate != GstrDate)
            {
                SQL = " SELECT * FROM KOSMOS_PMPA.OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(DATE3) = TO_DATE('" + GstrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDEPT + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE IS NULL  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("예약일: [ " + GstrDate + "] 해당일자에 이미 같은과에 예약이 있습니다. 확인바랍니다");
                    dt.Dispose();
                    dt = null;
                    return rtVal;
                }
                dt.Dispose();
                dt = null;

                SQL = " SELECT * FROM KOSMOS_PMPA.OPD_TELRESV ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND RDATE = TO_DATE('" + GstrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDEPT + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("전화예약일: [ " + GstrDate + "] 해당일자에 이미 같은과에 전화예약이 있습니다. 확인바랍니다");
                    dt.Dispose();
                    dt = null;
                    return rtVal;
                }
                dt.Dispose();
                dt = null;
            }


            //'2013-12-04
            if (strTelGbn == "☎" && VB.Trim(GstrDate) != "" && Convert.ToDateTime(VB.Left(GstrDate, 10)) > Convert.ToDateTime(clsPublic.GstrSysDate))
            {
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    strRDate1 = VB.Left(GstrDate, 10);
                    strRTime1 = VB.Right(GstrTime, 5);

                    SQL = " UPDATE KOSMOS_PMPA.OPD_TELRESV SET ";
                    SQL = SQL + ComNum.VBLF + "       RDATE = TO_DATE('" + strRDate1 + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "       RTIME = '" + strRTime1 + "', ";
                    SQL = SQL + ComNum.VBLF + "       ENTSABUN = '" + clsType.User.Sabun + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO         = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWID ='" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }

                    ssView_Sheet1.RowCount = 0;
                        
                    clsDB.setCommitTran(clsDB.DbCon);                                    
                    Cursor.Current = Cursors.Default;
                    ssView_Sheet1.RowCount = 0;
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
            else if (strTelGbn != "☎" && VB.Trim(GstrDate) != "" && Convert.ToDateTime(VB.Left(GstrDate, 10)) > Convert.ToDateTime(clsPublic.GstrSysDate))
            {
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    strRDate1 = VB.Left(GstrDate, 10);
                    strRTime1 = VB.Right(GstrTime, 5);
                    strRDate1 = strRDate1 + " " + strRTime1;

                    strPtNo = ssView_Sheet1.Cells[row, 0].Text;

                    SQL = " UPDATE KOSMOS_PMPA.OPD_RESERVED SET ";
                    SQL = SQL + ComNum.VBLF + "       DATE3 = TO_DATE('" + strRDate1 + "','YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO         = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND DATE3 >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(FstrRDate), "D");
                    SQL = SQL + ComNum.VBLF + "   AND DATE3 <  " + ComFunc.ConvOraToDate(Convert.ToDateTime(FstrRDate).AddDays(1), "D");
                    SQL = SQL + ComNum.VBLF + "   AND DRCODE       = '" + VB.Trim(FstrDrCode) + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }

                    SQL = " UPDATE KOSMOS_PMPA.OPD_RESERVED_NEW SET ";
                    SQL = SQL + ComNum.VBLF + "       DATE3 = TO_DATE('" + strRDate1 + "','YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO         = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND DATE3 >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(FstrRDate), "D");
                    SQL = SQL + ComNum.VBLF + "   AND DATE3 <  " + ComFunc.ConvOraToDate(Convert.ToDateTime(FstrRDate).AddDays(1), "D");
                    SQL = SQL + ComNum.VBLF + "   AND DRCODE       = '" + VB.Trim(FstrDrCode) + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND TRANSDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "   AND RETDATE IS NULL  ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }

                    ssView_Sheet1.RowCount = 0;
                        
                    clsDB.setCommitTran(clsDB.DbCon);                                    
                    Cursor.Current = Cursors.Default;
                    ssView_Sheet1.RowCount = 0;
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
            else
            {
                return rtVal;
            }
            
        }


        private void Read_Reserved(int argROW)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPANO = "";
            string StrRDate = "";

            try
            {
                strPANO = ssView_Sheet1.Cells[argROW, 0].Text;
                StrRDate = ssView_Sheet1.Cells[argROW, 8 + 1].Text;

                SQL = "";
                SQL = "       SELECT  'RES' GBN,R.PANO, R.DRCODE,  R.DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(DATE3,'HH24:MI') RTIME,  B.DRNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_RESERVED_NEW R , KOSMOS_PMPA.BAS_DOCTOR B  ";
                SQL = SQL + ComNum.VBLF + "WHERE DATE3    >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(StrRDate), "D");
                SQL = SQL + ComNum.VBLF + "  AND DATE3    <  " + ComFunc.ConvOraToDate(Convert.ToDateTime(StrRDate).AddDays(1), "D");
                SQL = SQL + ComNum.VBLF + "  AND R.PANO = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND R.TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND R.RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND R.DRCODE = B.DRCODE ";
                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT 'TEL' GBN,R.PANO, R.DRCODE,  R.DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + "       RTIME RTIME,  B.DRNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_TELRESV R , KOSMOS_PMPA.BAS_DOCTOR B ";
                SQL = SQL + ComNum.VBLF + " WHERE RDATE =  " + ComFunc.ConvOraToDate(Convert.ToDateTime(StrRDate), "D");
                SQL = SQL + ComNum.VBLF + "  AND R.PANO = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND R.DRCODE = B.DRCODE ";
                SQL = SQL + ComNum.VBLF + "ORDER BY 5        ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssReserve1_Sheet1.Cells[0, i].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim() + ":" + dt.Rows[i]["DRNAME"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["RTIME"].ToString().Trim();
                        if (dt.Rows[i]["GBN"].ToString().Trim() == "TEL")
                        {
                            ssReserve1_Sheet1.Cells[0, i].Text = ssReserve1_Sheet1.Cells[0, i].Text + " ☎";
                        }                        
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


    }
}
