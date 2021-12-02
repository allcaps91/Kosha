using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComLibB;

namespace ComNurLibB
{
    public partial class frmERTransCheckList : Form, MainFormMessage
    {
        #region //MainFormMessage

        string fstrINTIME = "";
        string fstrPTNO = "";
        string fstrWard = "";

        

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {
        }

        public void MsgUnloadForm(Form frm)
        {
        }

        public void MsgFormClear()
        {
        }

        public void MsgSendPara(string strPara)
        {
        }

        #endregion //MainFormMessage

        public frmERTransCheckList(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmERTransCheckList()
        {
            InitializeComponent();
            //fstrINTIME = "2019-06-19 04:38";
            //fstrPTNO = "04430000";
            //fstrWard = "ER";
            setEvent();
        }

        public frmERTransCheckList(string argInTime, string argPTNO, string argWard)
        {
            InitializeComponent();
            fstrINTIME = argInTime;
            fstrPTNO = argPTNO;
            fstrWard = argWard;
            setEvent();
        }

        //기본값 세팅
        private void setCtrlData()
        {
        }

        private void setCtrlInit()
        {
        }

        private void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData(fstrINTIME, fstrPTNO);
            }
            else if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSetData(fstrINTIME, fstrPTNO);
            }
            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return;  //권한확인
                }
                ePrintData();
            }

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                lbMemo.Visible = false;
                if (fstrWard =="ER")
                {
                    lbMemo.Visible = true;
                }
                eGetData(fstrINTIME, fstrPTNO);
                
            }
        }

        private void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        private void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }


        private void eSetData(string strInDate, string strPano)
        {

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            //DataTable dt = null;

            string strFRDATE = "";
            //string strTODATE = "";

            string strSEQNO = "";
            string strCHECKNAME = "";
            string strQTY = "";
            string strUNIT = "";
            string strCHECK_ER = "";
            string strCHECK_WARD1 = "";
            string strCHECK_WARD2 = "";

            string strINDATE = lbInDate.Text.Trim();

            ComFunc.ReadSysDate(clsDB.DbCon);

            //2019-08-06 응급실 에서 출발일시 공란일 경우에도 저장 되도록, 출발일시가 들어가면 저장 확정으로 병동에서 확인이 가능함.
            //if (SS1.ActiveSheet.Cells[3, 5].Text.Trim() == "" || SS1.ActiveSheet.Cells[3, 7].Text.Trim() =="")
            //{
            //    MessageBox.Show("응급실 출발 일시가 공란입니다. 확인하시기 바랍니다.", "응급실 출발일시 확인");
            //    return;
            //}

            strFRDATE = SS1.ActiveSheet.Cells[3, 5].Text.Trim() + " " + SS1.ActiveSheet.Cells[3, 7].Text.Trim();
            strFRDATE = strFRDATE.Trim();

            if (strFRDATE != "")
            {
                if (Convert.ToDateTime(strINDATE) > Convert.ToDateTime(strFRDATE))
                {
                    ComFunc.MsgBox("ER 출발일시가 응급실 내원일시보다 빠릅니다. 확인하여 주십시요.");
                    return;
                }

                ComFunc cf = new ComFunc();

                if (Convert.ToDateTime(strINDATE) < Convert.ToDateTime(cf.DATE_ADD(clsDB.DbCon, SS1.ActiveSheet.Cells[3, 5].Text.Trim(), -3) + " " + SS1.ActiveSheet.Cells[3, 7].Text.Trim()))
                {
                    ComFunc.MsgBox("ER 출발일시가 응급실 내원일시에서 3일을 초과하였습니다.");
                    return;
                }

            }

            //if (SS1.ActiveSheet.Cells[3, 5].Text.Trim() != "" && (clsPublic.GstrSysDate != SS1.ActiveSheet.Cells[3, 5].Text.Trim()))
            //{
            //    if (ComFunc.MsgBoxQ("응급실 출발 일자가 오늘(" + clsPublic.GstrSysDate + ")이 아닙니다. " + ComNum.VBLF + "계속 하시겠습니까?", "응급실 출발일 확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
            //    {
            //        return;
            //    }
            //}
            //{
            //    MessageBox.Show("응급실 출발 일시가 공란입니다. 확인하시기 바랍니다.", "응급실 출발일시 확인");
            //    return;
            //}

            //if (fstrWard != "ER" && fstrWard != "")
            //{
            //    if (SS1.ActiveSheet.Cells[4, 5].Text.Trim() == "" || SS1.ActiveSheet.Cells[4, 7].Text.Trim() == "")
            //    {
            //        MessageBox.Show("병동 도착 일시가 공란입니다. 확인하시기 바랍니다.", "병동 도착일시 확인");
            //        return;
            //    }
            //}

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (fstrWard != "ER" && fstrWard != "" && fstrWard != "OP")
                {
                    strFRDATE = SS1.ActiveSheet.Cells[3, 5].Text.Trim() + " " + SS1.ActiveSheet.Cells[3, 7].Text.Trim();
                    strFRDATE = strFRDATE.Trim();

                    //strTODATE = SS1.ActiveSheet.Cells[4, 5].Text.Trim() + " " + SS1.ActiveSheet.Cells[4, 7].Text.Trim();
                    SQL = " UPDATE KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS SET ";
                    //SQL += ComNum.VBLF + " TODATE = TO_DATE('" + strTODATE + "','YYYY-MM-DD HH24:MI'), "; 
                    //2020-06-10 병동에서도 출발일시 수정할 수 있도록 요청
                    SQL += ComNum.VBLF + " FRDATE = TO_DATE('" + strFRDATE + "','YYYY-MM-DD HH24:MI'), ";
                    SQL += ComNum.VBLF + " TOWARD = '" + fstrWard + "',";
                    SQL += ComNum.VBLF + " CHECK_WARD1_SABUN = '" + clsType.User.Sabun + "',";
                    SQL += ComNum.VBLF + " CHECK_WARD1_DATE = SYSDATE ";
                    SQL += ComNum.VBLF + "  WHERE INTIME = TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "    AND PANO = '" + strPano + "'";
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
                else if (fstrWard != "ER" && fstrWard != "" && fstrWard == "OP")
                {
                    SQL = " UPDATE KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS SET ";
                    SQL += ComNum.VBLF + " CHECK_WARD2_SABUN = '" + clsType.User.Sabun + "',";
                    SQL += ComNum.VBLF + " CHECK_WARD2_DATE = SYSDATE ";
                    SQL += ComNum.VBLF + "  WHERE INTIME = TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "    AND PANO = '" + strPano + "'";
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
                else
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS_HIS ";
                    SQL += ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS ";
                    SQL += ComNum.VBLF + "  WHERE INTIME = TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "    AND PANO = '" + strPano + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS_SUB_HIS ";
                    SQL += ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS_SUB ";
                    SQL += ComNum.VBLF + "  WHERE INTIME = TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "    AND PANO = '" + strPano + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = " DELETE KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS ";
                    SQL += ComNum.VBLF + "  WHERE INTIME = TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "    AND PANO = '" + strPano + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = " DELETE KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS_SUB ";
                    SQL += ComNum.VBLF + "  WHERE INTIME = TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "    AND PANO = '" + strPano + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    strFRDATE = SS1.ActiveSheet.Cells[3, 5].Text.Trim() + " " + SS1.ActiveSheet.Cells[3, 7].Text.Trim();
                    strFRDATE = strFRDATE.Trim();

                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS ( ";
                    SQL += ComNum.VBLF + " INTIME, PANO, FRDATE, CHECK_ER_SABUN, ";
                    SQL += ComNum.VBLF + " CHECK_ER_DATE ";
                    SQL += ComNum.VBLF + " ) VALUES ( ";
                    SQL += ComNum.VBLF + " TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI'), '" + strPano + "', TO_DATE('" + strFRDATE + "','YYYY-MM-DD HH24:MI'), '" + clsType.User.Sabun + "',";
                    SQL += ComNum.VBLF + " SYSDATE ";
                    SQL += ComNum.VBLF + " )";
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

                for (i = 8; i <= 29; i++ )
                {
                    strSEQNO = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                    strCHECKNAME = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                    strQTY = SS1.ActiveSheet.Cells[i, 4].Text.Trim();
                    strUNIT = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                    strCHECK_ER = SS1.ActiveSheet.Cells[i, 6].Text.Trim();
                    strCHECK_WARD1 = SS1.ActiveSheet.Cells[i, 7].Text.Trim();
                    strCHECK_WARD2 = SS1.ActiveSheet.Cells[i, 8].Text.Trim();

                    if (strSEQNO != "" && strCHECKNAME != "")
                    {
                        if (fstrWard != "ER" && fstrWard != "" && fstrWard != "OP")
                        {
                            SQL = " UPDATE KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS_SUB SET ";
                            SQL += ComNum.VBLF + " CHECK_WARD1 = '" + strCHECK_WARD1 + "' ";
                            SQL += ComNum.VBLF + " WHERE INTIME = TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI') ";
                            SQL += ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                            SQL += ComNum.VBLF + "   AND CHECKNAME = '" + strCHECKNAME + "' ";
                            SQL += ComNum.VBLF + "   AND SEQNO = '" + strSEQNO + "' ";
                        }
                        else if (fstrWard != "ER" && fstrWard != "" && fstrWard == "OP")
                        {
                            SQL = " UPDATE KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS_SUB SET ";
                            SQL += ComNum.VBLF + " CHECK_WARD2 = '" + strCHECK_WARD2 + "' ";
                            SQL += ComNum.VBLF + " WHERE INTIME = TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI') ";
                            SQL += ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                            SQL += ComNum.VBLF + "   AND CHECKNAME = '" + strCHECKNAME + "' ";
                            SQL += ComNum.VBLF + "   AND SEQNO = '" + strSEQNO + "' ";
                        }
                        else
                        {
                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS_SUB ( ";
                            SQL += ComNum.VBLF + " INTIME, PANO, SEQNO, CHECKNAME, ";
                            SQL += ComNum.VBLF + " QTY, UNIT, CHECK_ER ) VALUES ( ";
                            SQL += ComNum.VBLF + " TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI'), '" + strPano + "','" + strSEQNO + "','" + strCHECKNAME + "',";
                            SQL += ComNum.VBLF + "'" + strQTY + "','" + strUNIT + "','" + strCHECK_ER + "') ";
                        }
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
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                MessageBox.Show("저장되었습니다.", "저장완료");
                eGetData(strInDate, strPano);
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private string eGetData(string strInDate, string strPano)
        {
            string SQL = "";
            string SqlErr = "";

            string strTemp = "";
            DataTable dt = null;

            int i = 0;
            int nRead = 0;
            int nRowCnt = 0;

            //10987909
            //노재균(M / 60)
            //581126 - 1 * *****

            SS1.ActiveSheet.Cells[3, 1].Text = "";
            SQL = " SELECT A.PANO || '  ' || B.SNAME || '(' || A.SEX || '/' || A.AGE || ')' SNAME, B.JUMIN1 || '-' || B.JUMIN2 JUMIN ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_ER_PATIENT A, KOSMOS_PMPA.BAS_PATIENT B ";
            SQL += ComNum.VBLF + "  WHERE A.PANO = '" + strPano + "' ";
            SQL += ComNum.VBLF + "    AND INTIME = TO_DATE('" + strInDate + "', 'YYYY-MM-DD HH24:MI') ";
            SQL += ComNum.VBLF + "    AND A.PANO = B.PANO ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "NO";
            }

            if (dt.Rows.Count > 0)
            {
                SS1.ActiveSheet.Cells[3, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim() + "             " + dt.Rows[i]["JUMIN"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;


            lbInDate.Text = strInDate;
            lbName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, strPano);
            lbPtno.Text = "(" + strPano + ")";

            SQL = "    SELECT TO_CHAR(INTIME,'YYYY-MM-DD HH24:MI') INDATE, PANO, TO_CHAR(FRDATE,'YYYY-MM-DD HH24:MI') FRDATE, ";
            SQL += ComNum.VBLF + " TO_CHAR(TODATE,'YYYY-MM-DD HH24:MI') TODATE, CHECK_ER_SABUN, CHECK_WARD1_SABUN, CHECK_WARD2_SABUN ";
            SQL += ComNum.VBLF + "      FROM KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS ";
            SQL += ComNum.VBLF + "     WHERE INTIME = TO_DATE('" + strInDate + "', 'YYYY-MM-DD HH24:MI') ";
            SQL += ComNum.VBLF + "       AND PANO = '" + strPano + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "NO";
            }

            if (dt.Rows.Count > 0)
            {
                strTemp = dt.Rows[i]["FRDATE"].ToString().Trim();
                SS1.ActiveSheet.Cells[3, 5].Text = VB.Left(strTemp, 10);
                SS1.ActiveSheet.Cells[3, 7].Text = VB.Right(strTemp, 5);
                //strTemp = dt.Rows[i]["TODATE"].ToString().Trim();
                //SS1.ActiveSheet.Cells[4, 5].Text = VB.Left(strTemp, 10);
                //SS1.ActiveSheet.Cells[4, 7].Text = VB.Right(strTemp, 5);

                SS1.ActiveSheet.Cells[30, 6].Text = clsErNr.READ_INSA_NAME(clsDB.DbCon, dt.Rows[i]["CHECK_ER_SABUN"].ToString().Trim());
                SS1.ActiveSheet.Cells[30, 7].Text = clsErNr.READ_INSA_NAME(clsDB.DbCon, dt.Rows[i]["CHECK_WARD1_SABUN"].ToString().Trim());
                SS1.ActiveSheet.Cells[30, 8].Text = clsErNr.READ_INSA_NAME(clsDB.DbCon, dt.Rows[i]["CHECK_WARD2_SABUN"].ToString().Trim());
                if (SS1.ActiveSheet.Cells[30, 7].Text.Trim() != "")
                {
                    if (fstrWard != "ER" && fstrWard != "")
                    {
                     //병동의 경우 동일 사번 이외엔 수정 안되도록 필요시 적용
                    }
                    else
                    {
                        btnSave.Enabled = false;    
                    }
                        
                }
            }
            else
            {
                //MessageBox.Show("신규작성입니다.", "신규작성");

                SCREEN_CLEAR();
            }

            dt.Dispose();
            dt = null;
            

            SQL = "    SELECT SEQNO, CHECKNAME, QTY, UNIT, CHECK_ER, CHECK_WARD1, CHECK_WARD2 ";
            SQL += ComNum.VBLF + "     FROM KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS_SUB ";
            SQL += ComNum.VBLF + "    WHERE INTIME = TO_DATE('" + strInDate + "', 'YYYY-MM-DD HH24:MI') ";
            SQL += ComNum.VBLF + "      AND PANO = '" + strPano + "' ";
            SQL += ComNum.VBLF + "    ORDER BY SEQNO ASC ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "NO";
            }

            if (dt.Rows.Count > 0)
            {
                nRowCnt = 0;
                nRead = dt.Rows.Count;
                if (nRead > 22)
                {
                    nRead = 22;
                }
                nRowCnt = 8;
                for (i = 0; i < nRead; i++)
                {
                    SS1.ActiveSheet.Cells[nRowCnt, 1].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                    SS1.ActiveSheet.Cells[nRowCnt, 2].Text = dt.Rows[i]["CHECKNAME"].ToString().Trim();
                    SS1.ActiveSheet.Cells[nRowCnt, 4].Text = dt.Rows[i]["QTY"].ToString().Trim();
                    SS1.ActiveSheet.Cells[nRowCnt, 5].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                    SS1.ActiveSheet.Cells[nRowCnt, 6].Text = dt.Rows[i]["CHECK_ER"].ToString().Trim();
                    SS1.ActiveSheet.Cells[nRowCnt, 7].Text = dt.Rows[i]["CHECK_WARD1"].ToString().Trim();
                    SS1.ActiveSheet.Cells[nRowCnt, 8].Text = dt.Rows[i]["CHECK_WARD2"].ToString().Trim();

                    nRowCnt = nRowCnt + 1;
                }
            }

            dt.Dispose();
            dt = null;
            return "OK";
        }

        private void SCREEN_CLEAR()
        {
            int i = 0;

            SS1.ActiveSheet.Cells[3, 5].Text = "";
            SS1.ActiveSheet.Cells[4, 5].Text = "";

            SS1.ActiveSheet.Cells[3, 7].Text = "";
            SS1.ActiveSheet.Cells[4, 7].Text = "";

            SS1.ActiveSheet.Cells[30, 6].Text = "";
            SS1.ActiveSheet.Cells[30, 7].Text = "";
            SS1.ActiveSheet.Cells[30, 8].Text = "";

            for (i = 8; i <= 27; i++)
            {
                SS1.ActiveSheet.Cells[i, 4].Text = "";      //수량
                SS1.ActiveSheet.Cells[i, 6].Text = "";      //체크리스트
                SS1.ActiveSheet.Cells[i, 7].Text = "";
                SS1.ActiveSheet.Cells[i, 8].Text = "";

            }
            for (i = 28; i <= 29; i++)
            {
                SS1.ActiveSheet.Cells[i, 2].Text = "";      //수량
                SS1.ActiveSheet.Cells[i, 4].Text = "";      //체크리스트
                SS1.ActiveSheet.Cells[i, 5].Text = "";
                SS1.ActiveSheet.Cells[i, 6].Text = "";
                SS1.ActiveSheet.Cells[i, 7].Text = "";
                SS1.ActiveSheet.Cells[i, 8].Text = "";
            }
        }

        void ePrintData()
        {

            SS1.PrintSheet(SS1.ActiveSheetIndex);

        }

        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.Column == 5 && (e.Row == 3 || e.Row == 4))
            {
                clsPublic.GstrCalDate = "";

                frmCalendar2 frmCalendar2X = new frmCalendar2();
                frmCalendar2X.StartPosition = FormStartPosition.CenterParent;
                frmCalendar2X.ShowDialog();
                frmCalendar2X.Dispose();
                frmCalendar2X = null;

                SS1_Sheet1.Cells[e.Row, e.Column].Text = clsPublic.GstrCalDate;

                clsPublic.GstrCalDate = "";
            }
            else if((e.Column == 6 || e.Column == 7 || e.Column == 8) && (e.Row >= 8 && e.Row <= 29))
            {
                if (SS1.ActiveSheet.Cells[e.Row, e.Column].Text.Trim() == "")
                {
                    SS1.ActiveSheet.Cells[e.Row, e.Column].Text = "Y";
                }
                else
                {
                    SS1.ActiveSheet.Cells[e.Row, e.Column].Text = "";
                }
            }
        }

    }
}