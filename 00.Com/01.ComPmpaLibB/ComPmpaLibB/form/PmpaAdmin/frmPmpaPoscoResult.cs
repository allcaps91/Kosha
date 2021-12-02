using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaPoscoResult : Form

    {
        clsSpread cSpd = new clsSpread();
        clsComPmpaSpd cPmpaSpd = new clsComPmpaSpd();
        ComFunc CF = new ComFunc();
        clsPmpaFunc cPF = new clsPmpaFunc();
        clsPmpaMisu cPM = new clsPmpaMisu();
        Card CARD = new Card();
        clsPmpaPb cPb = new clsPmpaPb();

        string FstrJepDate = string.Empty;
        string FstrSName = string.Empty;
        string FstrPano = string.Empty;

       
        public frmPmpaPoscoResult()
        {
            InitializeComponent();
            SetEvent();
        }
        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.CmdExit.Click += new EventHandler(eBtnClick);
            this.CmdView.Click += new EventHandler(eBtnClick);
            this.CmdSave.Click += new EventHandler(eBtnClick);
        }
        private void READ_Posco_Exam(string ArgPart, string ArgPano, string ArgBDate, long ArgWRTNO, bool ArgSel)
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0, nRow = 0;

            try
            {
                cSpd.Spread_All_Clear(SS_Posco);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT Sort,Gubun,Code";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                SQL += ComNum.VBLF + "  WHERE (DELDATE IS NULL OR DELDATE ='') ";
                SQL += ComNum.VBLF + "    AND GUBUN <> '00000' ";  //타이틀 제외
                if (ArgPart != "")
                {
                    switch (ArgPart)
                    {
                        case "1":
                            SQL += ComNum.VBLF + "  AND Part IN ('1','2') "; //급여,비급여
                            break;
                        case "2":
                            SQL += ComNum.VBLF + "  AND Part IN ('3') ";   //공단
                            break;

                        default:
                            break;
                    }
                }
                SQL += ComNum.VBLF + "  GROUP BY Sort,Gubun,Code ";
                SQL += ComNum.VBLF + "  ORDER BY SORT,GUBUN,CODE ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = Dt.Rows.Count;

                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nRow += 1;
                        if (SS_Posco_Sheet1.Rows.Count < nRow)
                        {
                            SS_Posco_Sheet1.Rows.Count = nRow;
                        }

                        SS_Posco_Sheet1.Cells[nRow - 1, 0].Text = "False";
                        SS_Posco_Sheet1.Cells[nRow - 1, 1].Text = READ_Posco_Exam_Name("00", clsPublic.GstrSysDate, "", Dt.Rows[i]["Gubun"].ToString().Trim());
                        SS_Posco_Sheet1.Cells[nRow - 1, 2].Text = READ_Posco_Exam_Name("01", clsPublic.GstrSysDate, Dt.Rows[i]["Gubun"].ToString().Trim(), Dt.Rows[i]["Code"].ToString().Trim());
                        SS_Posco_Sheet1.Cells[nRow - 1, 3].Text = READ_Posco_Exam_Name("02", clsPublic.GstrSysDate, Dt.Rows[i]["Gubun"].ToString().Trim(), Dt.Rows[i]["Code"].ToString().Trim());
                        SS_Posco_Sheet1.Cells[nRow - 1, 4].Text = Dt.Rows[i]["Gubun"].ToString().Trim();
                        SS_Posco_Sheet1.Cells[nRow - 1, 5].Text = Dt.Rows[i]["Code"].ToString().Trim();

                        SS_Posco_Sheet1.SetRowVisible(nRow - 1, true);


                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL ";
                        SQL += ComNum.VBLF + "  WHERE GUBUN ='" + Dt.Rows[i]["Gubun"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND CODE ='" + Dt.Rows[i]["Code"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE =TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='') ";
                        SQL += ComNum.VBLF + "    AND Pano =" + ArgPano + "   ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (Dt2.Rows.Count > 0)
                        {
                            SS_Posco_Sheet1.Cells[nRow - 1, 0].Text = "True";
                            SS_Posco_Sheet1.Cells[nRow - 1, 7].Text = Dt2.Rows[0]["ROWID"].ToString().Trim();
                        }
                        else
                        {
                            if (ArgSel)
                            {
                                SS_Posco_Sheet1.SetRowVisible(nRow - 1, false);
                            }
                        }
                        Dt2.Dispose();
                        Dt2 = null;

                    }
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        private string READ_Posco_Exam_Name(string argGubun, string ArgDate, string ArgCode1, string ArgCode2)
        {
            string rtnVal = string.Empty;

            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0;

            try
            {
                SQL = "";
                //타이틀
                if (argGubun == "00")
                {
                    SQL += ComNum.VBLF + " SELECT NAME FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                    SQL += ComNum.VBLF + "  WHERE GUBUN ='00000' ";
                    SQL += ComNum.VBLF + "    AND CODE ='" + ArgCode2 + "' ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (Dt.Rows.Count > 0)
                    {
                        rtnVal = Dt.Rows[0]["Name"].ToString().Trim();
                    }
                }
                else if (argGubun == "01")
                {
                    //명칭
                    SQL += ComNum.VBLF + " SELECT NAME FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                    SQL += ComNum.VBLF + "  WHERE GUBUN = '" + ArgCode1 + "' ";
                    SQL += ComNum.VBLF + "    AND CODE = '" + ArgCode2 + "' ";
                    SQL += ComNum.VBLF + "    AND JDate <= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='')  ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (Dt.Rows.Count > 0)
                    {
                        rtnVal = Dt.Rows[0]["Name"].ToString().Trim();
                    }
                }
                else if (argGubun == "02")
                {
                    //수가
                    SQL += ComNum.VBLF + " SELECT Amt FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                    SQL += ComNum.VBLF + "  WHERE GUBUN ='" + ArgCode1 + "' ";
                    SQL += ComNum.VBLF + "    AND CODE ='" + ArgCode2 + "' ";
                    SQL += ComNum.VBLF + "    AND JDate <=TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='')  ";
                    SQL += ComNum.VBLF + "  ORDER BY JDATE DESC ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (Dt.Rows.Count > 0)
                    {
                        rtnVal = VB.Val(Dt.Rows[0]["AMT"].ToString().Trim()).ToString("###,###,###");
                    }
                }
                else
                {
                    //rowid
                    SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                    SQL += ComNum.VBLF + "  WHERE GUBUN ='" + ArgCode1 + "' ";
                    SQL += ComNum.VBLF + "    AND CODE ='" + ArgCode2 + "' ";
                    SQL += ComNum.VBLF + "    AND JDate <=TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='')  ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (Dt.Rows.Count > 0)
                    {
                        rtnVal = Dt.Rows[0]["ROWID"].ToString().Trim();
                    }
                }

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }

        }
        private void eFormLoad(object sender, EventArgs e)
        {
            int i = 0;
            int nYY = 0;
            int nMM = 0;
            string strBDate = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            //cSpd.Spread_Clear_Simple(SS1,5);
            cSpd.Spread_All_Clear(SS_Posco);
            //ComboGubun_SET(clsDB.DbCon, cboGubun, "00000",

            nYY = Convert.ToInt32(VB.Left(clsPublic.GstrSysDate, 4));
            nMM = Convert.ToInt32(VB.Mid(clsPublic.GstrSysDate,6, 2));


            ComboYYMM.Items.Clear();
 
            for (i = 1; i <= 12; i++)
            {
                ComboYYMM.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "년" + ComFunc.SetAutoZero(nMM.ToString(), 2) + "월" ) ;
              //  cboYYMM.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "-" + ComFunc.SetAutoZero(nMM.ToString(), 2));
                nMM -= 1;
                if (nMM == 0)
                {
                    nYY -= 1;
                    nMM = 12;
                }
            }

            ComboYYMM.SelectedIndex = 1;
           



        }
        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.CmdExit)             //닫기
            {
                this.Close();
            }
            else if (sender == this.CmdView)      //조회
            {
                Screen_Display();
            }

            else if (sender == this.CmdSave)       //포스코항목 저장
            {
               // Posco_Save();
            }
           

        }
        private void Screen_Display()
        {
            DataTable Dt = null;
            DataTable Dt1 = null;
            DataTable Dt2 = null;
            DataTable Dt3 = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0,  j = 0, k = 0, nRead = 0, nRow = 0, nRead1 = 0, nRead2 = 0, nRead3 = 0;
            long nTot = 0;
            int nCNT = 0;
            string strMon = "";
            string strBDate = "";
            string strEdate = "";

            FstrPano = "";
            FstrSName = "";
            FstrJepDate = "";
            for (i = 3; i < SS1_Sheet1.RowCount  ; i++)
            {
                for (j = 0; j < SS1_Sheet1.ColumnCount  ; j++)
                {
                    SS1_Sheet1.Cells[i, j].Text = "";
                }

            }

            //cSpd.Spread_Clear_Simple(SS1, 3);
            strMon = VB.Right(ComboYYMM.Text.Trim(),3);
            SS1_Sheet1.Cells[0, 0].Text = VB.TR(SS1_Sheet1.Cells[0, 0].Text, "01월", strMon);
            strBDate = VB.Replace(VB.Replace(VB.Replace(ComboYYMM.Text.Trim(), " ", ""), "년", "-"), "월", "-") + "01";
            strEdate = CF.READ_LASTDAY(clsDB.DbCon, strBDate);

            try
            {


                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,PANO  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL ";
                SQL += ComNum.VBLF + "  WHERE BDATE >=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND BDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='')  ";
                SQL += ComNum.VBLF + "  AND GUBUN <> '00000'  ";
                SQL += ComNum.VBLF + "  GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD'),PANO ";

               
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                nRead = Dt.Rows.Count;
                SS1_Sheet1.Rows.Count = nRead * 5 ;
                nRow = 0;
                nCNT = 0;

                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nCNT = nCNT + 1;
                        nRow = nRow + 1;
                        SS1_Sheet1.Cells[nRow + 2, 0].Text = nCNT.ToString();
                        SS1_Sheet1.Cells[nRow + 2, 1].Text = "";
                        SS1_Sheet1.Cells[nRow + 2, 2].Text = Dt.Rows[i]["BDate"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow + 2, 3].Text = "포항성모병원";

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT SABUN,JUMIN1,JUMIN2,JUMIN3,SNAME,TO_CHAR(CDATE,'YYYY-MM-DD') JDATE,Pano  ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
                        SQL += ComNum.VBLF + "  WHERE PANO ='" + Dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "  AND ( ";
                        SQL += ComNum.VBLF + "    (EXAMRES1 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES1 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES2 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES2 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES3 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES3 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES4 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES4 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES6 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES6 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES7 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES7 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES8 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES8 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES9 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES9 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES10 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES10 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES11 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES11 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES12 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES12 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES13 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES13 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES14 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES14 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES15 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES15 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";
                        SQL += ComNum.VBLF + " OR (EXAMRES16 >= TO_DATE('" + strBDate + "','YYYY-MM-DD') AND EXAMRES16 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, strEdate, +1) + "','YYYY-MM-DD') ) ";

                        SQL += ComNum.VBLF + "  )";
                        SQL += ComNum.VBLF + "  ORDER BY JDATE DESC ";
                        SqlErr = clsDB.GetDataTable(ref Dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        nRead1 = Dt1.Rows.Count;
                        if (nRead1 > 0)
                        {
                            SS1_Sheet1.Cells[nRow + 2, 1].Text = Dt1.Rows[0]["JDate"].ToString().Trim();
                            SS1_Sheet1.Cells[nRow + 2, 4].Text = Dt1.Rows[0]["Sabun"].ToString().Trim();
                            if (Dt1.Rows[0]["Jumin3"].ToString().Trim() != "")
                            {
                                SS1_Sheet1.Cells[nRow + 2, 5].Text = Dt1.Rows[0]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(Dt1.Rows[0]["Jumin3"].ToString().Trim());
                            }
                            else
                            {
                                SS1_Sheet1.Cells[nRow + 2, 5].Text = Dt1.Rows[0]["Jumin1"].ToString().Trim() + "-" + Dt1.Rows[0]["Jumin2"].ToString().Trim();
                            }
                            SS1_Sheet1.Cells[nRow + 2, 6].Text = Dt1.Rows[0]["SName"].ToString().Trim();
                            SS1_Sheet1.Cells[nRow + 2, 19].Text = Dt1.Rows[0]["Pano"].ToString().Trim();



                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT a.WRTNO,a.Gubun,b.code  ";
                            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL A, " + ComNum.DB_PMPA + "etc_bcode B";
                            SQL += ComNum.VBLF + "  WHERE a.Gubun = b.Gubun  ";
                            SQL += ComNum.VBLF + "  AND a.Code=b.Code   ";
                            SQL += ComNum.VBLF + "  AND a.Gubun <> '00001' ";
                            SQL += ComNum.VBLF + "  AND a.Pano = '" + Dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "  AND a.BDATE >=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "  AND a.BDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "  AND  (b.DELDATE IS NULL OR b.DELDATE ='')  ";
                            SQL += ComNum.VBLF + "  AND  (a.DELDATE IS NULL OR a.DELDATE ='')   ";
                            SQL += ComNum.VBLF + "  GROUP BY a.WRTNO,a.Gubun,b.code ";


                            SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            nRead2 = Dt2.Rows.Count;
                            if (nRead2 > 0)
                            {
                                for (j = 0; j < nRead2; j++)
                                {
                                    if (j != 0)
                                    {
                                        nRow = nRow + 1;
                                    }
                                    SS1_Sheet1.Cells[nRow + 2, 2].Text = Dt.Rows[i]["BDate"].ToString().Trim();
                                    SS1_Sheet1.Cells[nRow + 2, 3].Text = "포항성모병원";
                                    SS1_Sheet1.Cells[nRow + 2, 1].Text = Dt1.Rows[0]["JDate"].ToString().Trim();
                                    SS1_Sheet1.Cells[nRow + 2, 4].Text = Dt1.Rows[0]["Sabun"].ToString().Trim();

                                    if (Dt1.Rows[0]["Jumin3"].ToString().Trim() != "")
                                    {
                                        SS1_Sheet1.Cells[nRow + 2, 5].Text = Dt1.Rows[0]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(Dt1.Rows[0]["Jumin3"].ToString().Trim());
                                    }
                                    else
                                    {
                                        SS1_Sheet1.Cells[nRow + 2, 5].Text = Dt1.Rows[0]["Jumin1"].ToString().Trim() + "-" + Dt1.Rows[0]["Jumin2"].ToString().Trim();
                                    }
                                    SS1_Sheet1.Cells[nRow + 2, 6].Text = Dt1.Rows[0]["SName"].ToString().Trim();
                                    SS1_Sheet1.Cells[nRow + 2, 19].Text = Dt1.Rows[0]["Pano"].ToString().Trim();

                                    SS1_Sheet1.Cells[nRow + 2, 8].Text = READ_Posco_Exam_Name("01", clsPublic.GstrSysDate, Dt2.Rows[j]["Gubun"].ToString().Trim(), Dt2.Rows[j]["Code"].ToString().Trim());
                                    nTot = 0;

                                    SQL = "";
                                    SQL += ComNum.VBLF + " SELECT a.Gubun,a.Code,b.Name2,b.Amt,b.sCol  ";
                                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL A, " + ComNum.DB_PMPA + "etc_bcode B";
                                    SQL += ComNum.VBLF + "  WHERE a.Gubun = b.Gubun  ";
                                    SQL += ComNum.VBLF + "  AND a.Code=b.Code   ";
                                    SQL += ComNum.VBLF + "  AND a.WRTNO = '" + Dt2.Rows[j]["WRTNO"].ToString().Trim() + "' ";
                                    SQL += ComNum.VBLF + "  AND a.Gubun = '" + Dt2.Rows[j]["Gubun"].ToString().Trim() + "' ";
                                    SQL += ComNum.VBLF + "  AND a.Pano = '" + Dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                                    SQL += ComNum.VBLF + "  AND a.BDATE >=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                    SQL += ComNum.VBLF + "  AND a.BDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                                    SQL += ComNum.VBLF + "  AND  (b.DELDATE IS NULL OR b.DELDATE ='')  ";
                                    SQL += ComNum.VBLF + "  AND  (a.DELDATE IS NULL OR a.DELDATE ='')   ";
                                    SQL += ComNum.VBLF + " union all   ";
                                    SQL += ComNum.VBLF + " SELECT a.Gubun,a.Code,b.Name2,b.Amt,b.sCol  ";
                                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL A, " + ComNum.DB_PMPA + "etc_bcode B";
                                    SQL += ComNum.VBLF + "  WHERE a.Gubun = b.Gubun  ";
                                    SQL += ComNum.VBLF + "  AND a.Code=b.Code   ";
                                    SQL += ComNum.VBLF + "  AND a.Gubun = '00001' ";
                                    SQL += ComNum.VBLF + "  AND a.WRTNO = '" + Dt2.Rows[j]["WRTNO"].ToString().Trim() + "' ";
                                    SQL += ComNum.VBLF + "  AND a.Pano = '" + Dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                                    SQL += ComNum.VBLF + "  AND a.BDATE >=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                    SQL += ComNum.VBLF + "  AND a.BDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                                    SQL += ComNum.VBLF + "  AND  (b.DELDATE IS NULL OR b.DELDATE ='')  ";
                                    SQL += ComNum.VBLF + "  AND  (a.DELDATE IS NULL OR a.DELDATE ='')   ";


                                    SqlErr = clsDB.GetDataTable(ref Dt3, SQL, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return;
                                    }

                                    nRead3 = Dt3.Rows.Count;
                                    if (nRead3 > 0)
                                    {
                                        for (k = 0; k < nRead3; k++)
                                        {
                                            if(Dt3.Rows[k]["Gubun"].ToString().Trim() =="00001")
                                            {
                                                SS1_Sheet1.Cells[nRow + 2, 8].Text =VB.Val(Dt3.Rows[k]["AMT"].ToString().Trim()).ToString("###,###,###");
                                                nTot = nTot + (long)VB.Val(Dt3.Rows[k]["AMT"].ToString().Trim());
                                            }
                                            else
                                            {
                                                switch (Convert.ToInt32(VB.Val((Dt3.Rows[k]["sCol"].ToString().Trim()))))
                                                {
                                                    case 10:
                                                    case 11:
                                                    case 12:
                                                    case 13:
                                                        SS1_Sheet1.Cells[nRow + 2, Convert.ToInt32(VB.Val((Dt3.Rows[k]["sCol"].ToString().Trim()))) - 1].Text = VB.Val(Dt3.Rows[k]["AMT"].ToString().Trim()).ToString("###,###,###");
                                                        nTot = nTot + (long)VB.Val(Dt3.Rows[k]["AMT"].ToString().Trim());
                                                        break;
                                         
                                                }
                                            }
                                      
                                        }
                                        SS1_Sheet1.Cells[nRow + 2, 13].Text = VB.Val(nTot.ToString().Trim()).ToString("###,###,###");
                                        Dt3.Dispose();
                                        Dt3 = null;

                                    }
                                }

                            }
                            else
                            {
                                SS1_Sheet1.Cells[nRow + 2, 7].Text = "";
                            }
                            Dt2.Dispose();
                            Dt2 = null;
                        }
                        Dt1.Dispose();
                        Dt1 = null;
                    }
                }

                Dt.Dispose();
                Dt = null;


            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }
        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            long nWRTNO = 0;

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }
            FstrJepDate = SS1_Sheet1.Cells[e.Row, 2].Text;
            FstrSName = SS1_Sheet1.Cells[e.Row, 6].Text;
            FstrPano = SS1_Sheet1.Cells[e.Row, 19].Text;
            nWRTNO = 0;

            READ_Posco_Exam("", FstrPano, FstrJepDate, 0, chkSel.Checked);
            lbl_Pos_Sts.Text = FstrPano + "  " + FstrJepDate + "  고유번호: " + nWRTNO.ToString();


           
        }

       

        private void CmdSave_Click(object sender, EventArgs e)
        {
            string strYear = VB.Left(ComboYYMM.Text, 4);
            string strMon = VB.Right(ComboYYMM.Text, 2);
            string strName = strYear + "." + strMon + " 청구서및내역서(POSCO)";

            bool x = false;

            if (ComFunc.MsgBoxQ("파일로 만드시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                return;

            x = SS1.SaveExcel("C:\\"+ strName + ".xlsx", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
            {
                if (x == true)
                    ComFunc.MsgBox("엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }
        }

        private void chkSel_Click(object sender, EventArgs e)
        {

            READ_Posco_Exam("", FstrPano, FstrJepDate, 0, chkSel.Checked);
            lbl_Pos_Sts.Text = FstrPano + "  " + FstrJepDate;
        }
    }
}
