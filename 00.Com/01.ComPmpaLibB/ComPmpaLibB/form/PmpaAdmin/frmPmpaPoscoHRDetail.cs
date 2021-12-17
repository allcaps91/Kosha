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
    public partial class frmPmpaPoscoHRDetail : Form
    {
        clsSpread cSpd = new clsSpread();
        clsComPmpaSpd cPmpaSpd = new clsComPmpaSpd();
        ComFunc CF = new ComFunc();
        clsPmpaFunc cPF = new clsPmpaFunc();
        clsPmpaMisu cPM = new clsPmpaMisu();
        Card CARD = new Card();
        clsPmpaPb cPb = new clsPmpaPb();

        clsPmpaType.AcctReqData RSD = new clsPmpaType.AcctReqData();
        clsPmpaType.AcctResData RD = new clsPmpaType.AcctResData();

        long FnWRTNO = 0;     //팝업시 포스코 내역 Display 여부
        string FstrPano = string.Empty;    //팝업시 포스코 내역 Display 여부
        string FstrJepDate = string.Empty;    //팝업시 포스코 내역 Display 여부
        string FstrROWID = string.Empty;    //팝업시 포스코 내역 Display 여부

        public frmPmpaPoscoHRDetail()
        {
            InitializeComponent();
            SetEvent();
        }
        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.CmdView.Click += new EventHandler(eBtnClick);
            this.btnSave2.Click += new EventHandler(eBtnClick);
            this.TxtSDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);

        }
        public void eDtpFormatSet(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            DateTime date = new DateTime(1900, 1, 1, 00, 00, 00);

            if (dtp.Value == date)
            {
                return;
            }

            dtp.Format = DateTimePickerFormat.Short;

        }
        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)             //닫기
            {
                this.Close();
            }
            else if (sender == this.CmdView)       //조회
            {
                Screen_Display();
            }
            else if (sender == this.btnSave2)      //포스코항목 저장
            {
                Posco_Save();
            }

        }

        private void Posco_Save()
        {

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (FnWRTNO > 0)
                {
                    if (Posco_Exam_Detail_Set(clsDB.DbCon, FnWRTNO, TxtPano.Text.Trim(), TxtSDate.Text) == false)
                        if (Posco_Exam_Detail_Set(clsDB.DbCon, FnWRTNO, TxtPano.Text.Trim(), TxtSDate.Text) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                    if (SET_HIC_JEPSU_POSCO_WRTNO(clsDB.DbCon, FnWRTNO, FstrROWID) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    ComFunc.MsgBox("저장완료");

                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                Screen_Display();
                Screen_Clear();

                return;
            }




            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }
        private bool SET_HIC_JEPSU_POSCO_WRTNO(PsmhDb pDbCon, long ArgWRTNO, string ArgRowid)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;


            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "HIC_JEPSU ";
                SQL += ComNum.VBLF + "  P_WRTNO = " +  ArgWRTNO  + "  ";
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + ArgRowid + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool Posco_Exam_Detail_Set(PsmhDb pDbCon, long ArgWRTNO, string ArgPano, string ArgBDate)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int i = 0;
            string strChk = string.Empty;
            string strCode1 = string.Empty;
            string strCode2 = string.Empty;
            //string strChange = string.Empty;
            string strROWID = string.Empty;

            bool rtnVal = false;

            try
            {
                if (ArgWRTNO > 0)
                {
                    for (i = 0; i < SS1_Sheet1.Rows.Count; i++)
                    {
                        strChk = SS1_Sheet1.Cells[i, 0].Text;
                        strCode1 = SS1_Sheet1.Cells[i, 4].Text;
                        strCode2 = SS1_Sheet1.Cells[i, 5].Text;
                        //strChange = SS1_Sheet1.Cells[i, 6].Text;
                        strROWID = SS1_Sheet1.Cells[i, 7].Text;

                        if (strChk == "True" && strROWID == "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL ";
                            SQL += ComNum.VBLF + "        (PANO,WRTNO,GUBUN,CODE,BDATE,CNT,ENTSABUN,ENTDATE ) ";
                            SQL += ComNum.VBLF + " VALUES (";
                            SQL += ComNum.VBLF + "        '" + ArgPano + "', " + ArgWRTNO + ",'" + strCode1 + "','" + strCode2 + "',  ";
                            SQL += ComNum.VBLF + "        TO_DATE('" + ArgBDate + "','YYYY-MM-DD'), 1 ," + clsType.User.IdNumber + ",SYSDATE ) ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        }
                        else if (strChk == "False" && strROWID != "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL ";
                            SQL += ComNum.VBLF + "    SET DELDATE =SYSDATE, ";
                            SQL += ComNum.VBLF + "        ENTSABUN =" + clsType.User.IdNumber + ", ";
                            SQL += ComNum.VBLF + "        ENTDATE =SYSDATE ";
                            SQL += ComNum.VBLF + "  WHERE ROWID ='" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        }
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                    }
                }
                //Screen_Display();

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }
        private void Screen_Display()
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0, nRead2 = 0, nRow = 0;

            lblAmt.Text = "";
            SS1_Sheet1.Rows.Count = 0;
            try
            {
                cSpd.Spread_All_Clear(SS1);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT b.Ptno,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName, ";
                SQL += ComNum.VBLF + "   a.Sex,a.Age,a.GjJong,a.WRTNO,nvl(a.P_WRTNO,0) P_WRTNO,a.SExams,a.ROWID";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "HIC_JEPSU A, " + ComNum.DB_PMPA + "HIC_PATIENT B ";
                SQL += ComNum.VBLF + "  WHERE a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "    AND a.GjJong IN ('31','35')  ";  //'암검진만
                SQL += ComNum.VBLF + "    AND a.GbSTS <> 'D'  ";
                SQL += ComNum.VBLF + "    AND (a.DelDate IS NULL OR a.DelDate='') ";
                if (chk_misu.Checked == true)
                {
                    SQL += ComNum.VBLF + "    AND a.JepDate >=TO_DATE('" + VB.Left(TxtSDate.Text, 8) + "01" + "','YYYY-MM-DD')   ";
                    SQL += ComNum.VBLF + "    AND a.JepDate <=TO_DATE('" + CF.READ_LASTDAY(clsDB.DbCon, TxtSDate.Text) + "','YYYY-MM-DD')   ";
                    SQL += ComNum.VBLF + "    AND b.LTDCODE ='0223' ";
                    if (ChkUse.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND  a.P_WRTNO >  0 ";
                    }
                }
                else
                {
                    if (ChkUse.Checked == true)
                    {
                        SQL += ComNum.VBLF + "    AND a.JepDate >=TO_DATE('" + VB.Left(TxtSDate.Text, 8) + "01" + "','YYYY-MM-DD')   ";
                        SQL += ComNum.VBLF + "    AND a.JepDate <=TO_DATE('" + CF.READ_LASTDAY(clsDB.DbCon, TxtSDate.Text) + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "    AND a.P_WRTNO  >  0   ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "    AND a.JepDate =TO_DATE('" + TxtSDate.Text + "','YYYY-MM-DD')   ";
                    }
                }
                SQL += ComNum.VBLF + "  ORDER BY a.JepDate,a.WRTNO ";
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
                        if (SS1_Sheet1.Rows.Count < nRow)
                        {
                            SS1_Sheet1.Rows.Count = nRow;
                        }

                        SS1_Sheet1.Cells[nRow - 1, 0].Text = Dt.Rows[i]["Ptno"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, 1].Text = Dt.Rows[i]["JepDate"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, 2].Text = Dt.Rows[i]["SName"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, 3].Text = Dt.Rows[i]["Sex"].ToString().Trim() + "/" + Dt.Rows[i]["Age"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, 4].Text = Dt.Rows[i]["GjJong"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, 5].Text = Dt.Rows[i]["P_WRTNO"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, 6].Text = Dt.Rows[i]["ROWID"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT SUM(LTDAMT) LTDAMT";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "HIC_SUNAP ";
                        SQL += ComNum.VBLF + "  WHERE WRTNO =" + Dt.Rows[i]["WRTNO"].ToString().Trim() + " ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        nRead2 = Dt2.Rows.Count;

                        if (nRead2 > 0)
                        {
                            SS1_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(Dt2.Rows[0]["LTDAMT"].ToString().Trim()).ToString("###,###,###");
                        }

                        Dt2.Dispose();
                        Dt2 = null;
                        SS1_Sheet1.Cells[nRow - 1, 8].Text = SExam_Names_Disp(Dt.Rows[i]["SExams"].ToString().Trim());

                    }

                }

                Dt.Dispose();
                Dt = null;
                Screen_Clear();
                READ_Posco_Exam("2", "", TxtSDate.Text, 0, chkSel.Checked);
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
        private string SExam_Names_Disp(string ArgCode)
        {
            string SQL = "";
            string SqlErr = "";

            DataTable DtDisp = new DataTable();
            string rtnVal = string.Empty;
            string strSql = string.Empty;

            if (ArgCode == "") { return rtnVal; }

            for (int i = 1; i <= VB.I(ArgCode, ","); i++)
            {
                if (VB.Pstr(ArgCode, ",", i).Trim() != "")
                    strSql += "'" + VB.Pstr(ArgCode, ",", i).Trim() + "',";
            }

            if (strSql == "") { return rtnVal; }

            strSql = VB.Left(strSql, strSql.Length - 1);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Code, Name, YName ";
            SQL += ComNum.VBLF + "   FROM HIC_GROUPCODE ";
            SQL += ComNum.VBLF + "  WHERE Code IN (" + strSql + ") ";
            SQL += ComNum.VBLF + "    AND SWLICENSE='" + clsType.HosInfo.SwLicense + "' ";
            SQL += ComNum.VBLF + "ORDER BY Code ";
            SqlErr = clsDB.GetDataTable(ref DtDisp, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            for (int i = 0; i < DtDisp.Rows.Count; i++)
            {
                if (DtDisp.Rows[i]["YName"].ToString().Trim() != "")
                    rtnVal += DtDisp.Rows[i]["YName"].ToString().Trim() + ",";
                else
                    rtnVal += DtDisp.Rows[i]["Name"].ToString().Trim() + ",";
            }

            DtDisp.Dispose();
            DtDisp = null;

            return rtnVal;
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

                        if (ArgWRTNO > 0)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL ";
                            SQL += ComNum.VBLF + "  WHERE GUBUN ='" + Dt.Rows[i]["Gubun"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "    AND CODE ='" + Dt.Rows[i]["Code"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "    AND BDATE =TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='') ";
                            SQL += ComNum.VBLF + "    AND WRTNO =" + ArgWRTNO + "   ";
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
            ComFunc.ReadSysDate(clsDB.DbCon);

            Screen_Clear();

            TxtSDate.Text = clsPmpaPb.GstrActDate;

        }
        private void Screen_Clear()
        {
            int i = 0;



            TxtPano.Text = "";
            lbl_Pos_Sts.Text = "";
            FstrJepDate = "";
            FstrPano = "";
            FstrROWID = "";
            FnWRTNO = 0;
            btnSave2.Enabled = false;
            lblAmt.Text = "";

            //for (i = 0; i < 12; i++)
            //{
            //    SS2_Sheet1.Cells[i, 1].Text = "";
            //}


        }

        private void Sheet_Color_Clear()
        {

        }

        private void SS_Posco_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            int k = 0;
            long nSum = 0;

            lblAmt.Text = "";

            if (e.Column != 0)
            {
                return;
            }

            if (SS_Posco_Sheet1.Cells[e.Row, 0].Text == "True")
            {
                SS_Posco_Sheet1.Cells[e.Row, 0, e.Row, SS_Posco_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(203, 248, 192);
            }
            else
            {
                SS_Posco_Sheet1.Cells[e.Row, 0, e.Row, SS_Posco_Sheet1.ColumnCount - 1].BackColor = Color.White;
            }

            for (k = 0; k < SS_Posco_Sheet1.Rows.Count; k++)
            {
                if (SS_Posco_Sheet1.Cells[k, 0].Text == "True")
                {
                    nSum += Convert.ToInt64(VB.Replace(SS_Posco_Sheet1.Cells[k, 3].Text, ",", ""));
                }

            }

            lblAmt.Text = VB.Format(nSum, "###,###,###,##0");
        }

        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strExams = "";


            string strSName = string.Empty;
            Screen_Clear();

            //Sheet_Color_Clear();
            SS1_Sheet1.Cells[0, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].BackColor = Color.White;

            if (e.Row > 0 || e.Column > 0)
            {
                FstrPano = SS1_Sheet1.Cells[e.Row, 0].Text;
                FstrJepDate = SS1_Sheet1.Cells[e.Row, 1].Text;
                strSName = SS1_Sheet1.Cells[e.Row, 2].Text;
                FnWRTNO = Convert.ToInt64(SS1_Sheet1.Cells[e.Row, 5].Text);
                FstrROWID = SS1_Sheet1.Cells[e.Row, 6].Text;
                strExams = SS1_Sheet1.Cells[e.Row, 8].Text;
            }


            //포스코 청구 상세내역
            READ_Posco_Exam("2", FstrPano, FstrJepDate, FnWRTNO, chkSel.Checked);
            lbl_Pos_Sts.Text = strSName + " " + FstrPano + " " + FstrJepDate + "  고유번호: " + FnWRTNO.ToString() + ComNum.VBLF + ComNum.VBLF + strExams;
            SS1_Sheet1.Cells[e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1].BackColor = Color.Red;
            btnSave2.Enabled = true;

        }

        private void SS1_CellDoubleClick_1(object sender, CellClickEventArgs e)
        {
            string strSName = string.Empty;
            string strExams = string.Empty;
            Screen_Clear();

            //Sheet_Color_Clear();
            SS1_Sheet1.Cells[0, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].BackColor = Color.White;

            if (e.Row > 0 || e.Column > 0)
            {
                FstrPano = SS1_Sheet1.Cells[e.Row, 0].Text;
                FstrJepDate = SS1_Sheet1.Cells[e.Row, 1].Text;
                strSName = SS1_Sheet1.Cells[e.Row, 2].Text;
                FnWRTNO = Convert.ToInt64(SS1_Sheet1.Cells[e.Row, 5].Text);
                FstrROWID = SS1_Sheet1.Cells[e.Row, 6].Text;
                strExams = SS1_Sheet1.Cells[e.Row, 8].Text;
            }


            //포스코 청구 상세내역
            READ_Posco_Exam("2", FstrPano, FstrJepDate, FnWRTNO, chkSel.Checked);
            lbl_Pos_Sts.Text = strSName + " " + FstrPano + " " + FstrJepDate + "  고유번호: " + FnWRTNO.ToString() + strExams;
            SS1_Sheet1.Cells[e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1].BackColor = Color.Red;
            btnSave2.Enabled = true;
        }
    }
}
