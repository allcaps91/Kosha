using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using System.Drawing.Printing;
using Oracle.ManagedDataAccess.Client;
using static ComBase.clsEmrFunc;
using System.Collections.Generic;

namespace ComBase
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frnPatBun3.cs
    /// Description     : BRANDEN SCALE(욕창사정도구표)
    /// Author          : 박창욱
    /// Create Date     : 2018-05-07
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\mtsEmr\CarePlan\Frm욕창사정도구표201511.frm(FrmPatBun3.frm) >> frnPatBun3.cs 폼이름 재정의" />
    /// <seealso cref= "\mtsEmr\CarePlan\Frm소아욕창201511.frm(FrmPatBun3소아욕창.frm) >> frnPatBun3.cs 폼이름 재정의" />
    /// <seealso cref= "\mtsEmr\CarePlan\Frm.frm(FrmPatBun3신생아욕창.frm) >> frnPatBun3.cs 폼이름 재정의" />
    public partial class frnPatBun3 : Form
    {
        string FstrROWID = "";
        string FstrRowid2 = "";
        string FstrActdate = "";

        string GstrHelpCode = "";
        string gsWard = "";
        string GstrRetValue = "";

        string GstrFormGbn = "1";

        string FstrSname = "";
        string FstrPano = "";
        string FstrSex = "";
        string FstrAge = "";
        string FstrRoom = "";
        string FstrDeptCode = "";
        string FstrIpdno = "";

        public frnPatBun3()
        {
            InitializeComponent();
        }

        //public frnPatBun3(string strPano, string strIpdno, string strActDate, string strFromGbn)
        //{
        //    InitializeComponent();
        //    FstrPano = strPano;
        //    FstrIpdno = strIpdno;
        //    FstrActdate = strActDate;
        //    GstrFormGbn = strFromGbn;
        //}

        public frnPatBun3(string strSname, string strPano, string strSex, string strAge, string strRoom, string strDeptCode, string strIpdno, string sWard, string strFormGbn)
        {
            InitializeComponent();

            FstrSname = strSname;
            FstrPano = strPano;
            FstrSex = strSex;
            FstrAge = strAge;
            FstrRoom = strRoom;
            FstrDeptCode = strDeptCode;
            FstrIpdno = strIpdno;

            gsWard = sWard;
            GstrFormGbn = strFormGbn;
        }

        private void chkJ_CheckedChanged(object sender, EventArgs e)
        {
            if (chkJ.Checked == true)
            {
                dtpJDate.Enabled = true;
                dtpJTime.Enabled = true;
                lblJ.Visible = true;
            }
            else
            {
                dtpJDate.Enabled = false;
                dtpJTime.Enabled = false;
                lblJ.Visible = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            //GstrHelpCode = this.Tag.ToString();
            //VB - 주석
            //frmBIGO.Show 1
            //GstrHelpCode = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (rdoOpt0.Checked == true)
            {
                Save_Data();
            }
            else if (rdoOpt1.Checked == true)
            {
                Save_Data2();
            }
            else if (rdoOpt2.Checked == true)
            {
                Save_Data3();
            }
        }

        void Save_Data()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int[] nTOT = new int[8];
            double nIPDNO = 0;
            double nEMRNO = 0;
            double dblEmrHisNo = 0;

            string strPano = "";
            string strSName = "";
            string strDept = "";
            string strAge = "";
            string strSex = "";
            string strRoom = "";

            string[] strTOT = new string[8];

            string strACTDATE = "";
            string strActTime = "";

            string strWARNING = "";

            string strSysDate = "";
            string strSysTime = "";

            string strOK = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");


            for (i = 0; i < strTOT.Length; i++)
            {
                strTOT[i] = "";
            }

            //if (chkJ.Checked == true && ComFunc.MsgBoxQ("조사일시 : " + dtpJDate.Value.ToString("yyyy-MM-dd") + " " + dtpJTime.Value.ToString("HH:mm") + ComNum.VBLF + ComNum.VBLF + "저장하시겠습니까?") == DialogResult.No)
            //{
            //    return;
            //}

            if (clsVbfunc.GetCertiOk(clsDB.DbCon, clsType.User.Sabun) == false)
            {
                ComFunc.MsgBox("전자인증서가 없는 사번입니다." + ComNum.VBLF + ComNum.VBLF + " 욕창사정도구표를 작성하실 수 없습니다.");
                return;
            }

            if (gsWard != "ER" && gsWard != "HD")
            {
                if (FstrIpdno == "")
                {
                    ComFunc.MsgBox("저장실패, 환자를 선택해주세요.");
                    return;
                }
            }

            strSName = FstrSname;
            strPano = FstrPano;
            strSex = FstrSex;
            strAge = FstrAge;
            strRoom = FstrRoom;
            strDept = FstrDeptCode;
            nIPDNO = VB.Val(FstrIpdno);

            if (gsWard == "ER" || gsWard == "HD")
            {
                nIPDNO = 0;
            }

            strACTDATE = ssInfo_Sheet1.Cells[1, 5].Text.Trim();

            if (chkJ.Checked == true)
            {
                strACTDATE = dtpJDate.Value.ToString("yyyy-MM-dd");
                strActTime = dtpJTime.Value.ToString("HH:mm");
            }
            else
            {
                strActTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
            }

            strTOT[1] = ssTemp_Sheet1.Cells[0, 0].Text.Trim();
            strTOT[2] = ssTemp_Sheet1.Cells[0, 1].Text.Trim();
            strTOT[3] = ssTemp_Sheet1.Cells[0, 2].Text.Trim();
            strTOT[4] = ssTemp_Sheet1.Cells[0, 3].Text.Trim();
            strTOT[5] = ssTemp_Sheet1.Cells[0, 4].Text.Trim();
            strTOT[6] = ssTemp_Sheet1.Cells[0, 5].Text.Trim();

            for (i = 1; i < strTOT.Length - 1; i++)
            {
                if (strTOT[i] == "")
                {
                    ComFunc.MsgBox("점수에 공란이 있습니다. 확인하시기 바랍니다.");
                    return;
                }
            }

            for (i = 1; i < strTOT.Length - 1; i++)
            {
                strTOT[7] = (VB.Val(strTOT[7]) + VB.Val(strTOT[i])).ToString();
            }

            strOK = "OK";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                EmrForm pForm = SerEmrFormUpdateNo(clsDB.DbCon, "2479");
                if (nEMRNO > 0 && pForm.FmOLDGB == 1)
                {
                    //기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
                    //KOSMOS_EMR.EMRXMLHISTORY_HISTORYNO_SEQ
                    dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                    SQL = SQL + ComNum.VBLF + "      '" + strSysDate.Replace("-", "") + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + strSysTime.Replace(":", "") + "', '" + clsType.User.Sabun + "',CERTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                }

                strWARNING = Make_Warning();
                if (pForm.FmOLDGB == 1)
                {
                    nEMRNO = 0;
                }
                nEMRNO = EMRXML_Insert(nIPDNO, strTOT[1], strTOT[2], strTOT[3], strTOT[4], strTOT[5], strTOT[6], strTOT[7], strWARNING, strACTDATE, strActTime, nEMRNO);

                if (nEMRNO == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("EMR 전송에 실패하였습니다.");
                    return;
                }

                if (FstrROWID == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE ( Pano,Ipdno,ActDate,Jumsu1,Jumsu2,Jumsu3,Jumsu4,";
                    SQL = SQL + ComNum.VBLF + " Jumsu5,Jumsu6,Total,  SNAME, Sex, Age, DeptCode, RoomCode, EntSabun,EntDate, EMRNO , ACTTIME) VALUES";
                    SQL = SQL + ComNum.VBLF + " ('" + strPano + "'," + nIPDNO + ", ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strACTDATE + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + " " + NVL(strTOT[1]) + ", " + NVL(strTOT[2]) + "," + NVL(strTOT[3]) + "," + NVL(strTOT[4]) + "," + NVL(strTOT[5]) + "," + NVL(strTOT[6]) + "," + NVL(strTOT[7]) + ", ";
                    SQL = SQL + ComNum.VBLF + "  '" + strSName + "','" + strSex + "', " + VB.Val(strAge) + ",'" + strDept + "'," + VB.Val(strRoom) + ",";
                    SQL = SQL + ComNum.VBLF + " " + clsType.User.Sabun + ", TO_DATE('" + strSysDate + " " + strSysTime + "','YYYY-MM-DD HH24:MI')," + nEMRNO + ",'" + strActTime + "' ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE SET ";
                    SQL = SQL + ComNum.VBLF + " Jumsu1 =" + NVL(strTOT[1]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " Jumsu2 =" + NVL(strTOT[2]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " Jumsu3 =" + NVL(strTOT[3]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " Jumsu4 =" + NVL(strTOT[4]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " Jumsu5 =" + NVL(strTOT[5]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " Jumsu6 =" + NVL(strTOT[6]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " Total =" + NVL(strTOT[7]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " SName ='" + strSName + "',  ";
                    SQL = SQL + ComNum.VBLF + " Sex ='" + strSex + "',  ";
                    SQL = SQL + ComNum.VBLF + " Age =" + VB.Val(strAge) + ",  ";
                    SQL = SQL + ComNum.VBLF + " DeptCode ='" + strDept + "',  ";
                    SQL = SQL + ComNum.VBLF + " RoomCode =" + VB.Val(strRoom) + ",  ";
                    SQL = SQL + ComNum.VBLF + " EntSabun=" + clsType.User.Sabun + ",  ";
                    SQL = SQL + ComNum.VBLF + " EMRNO = " + nEMRNO + ", ";
                    SQL = SQL + ComNum.VBLF + " EntDate =TO_DATE('" + strSysDate + " " + strSysTime + "','YYYY-MM-DD HH24:MI')  ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (gsWard != "ER" && gsWard != "HD")
                {
                    SQL = "";
                    SQL = "SELECT ROWID";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_PRESSURE_SORE WHERE Ipdno=" + nIPDNO + " ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY Actdate ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        FstrRowid2 = dt.Rows[0]["ROWID"].ToString().Trim();

                        SQL = "";
                        SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_PRESSURE_SORE  SET ";
                        SQL = SQL + ComNum.VBLF + " Total= " + NVL(strTOT[7]) + " ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrRowid2 + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            if (dt != null)
                            {
                                dt.Dispose();
                                dt = null;
                            }
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                strOK = Save_Warning(nIPDNO.ToString(), strPano, strACTDATE, strActTime);

                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장 중 에러 발생");
                    return;
                }

                strOK = Save_Eval(nIPDNO.ToString(), strPano, strACTDATE, strActTime);

                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장 중 에러 발생");
                    return;
                }

                if (VB.Val(strTOT[7]) > 0)
                {
                    GstrRetValue = strTOT[7];
                }
                else
                {
                    GstrRetValue = "";
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            Data_Read();
        }

        void Save_Data2()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double nIPDNO = 0;
            double nEMRNO = 0;

            string strOK = "";
            string strPano = "";
            string strSName = "";
            string strDept = "";
            string strAge = "";
            string strSex = "";
            string strRoom = "";
            string strTOT = "";
            string strWARNING = "";
            string strACTDATE = "";
            string strActTime = "";
            string strROWID = "";
            string strRowid2 = "";
            string[] strJUMSU = new string[8];

            string strSysDate = "";
            string strSysTime = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");

            if (chkJ.Checked == true)
            {
                strACTDATE = dtpJDate.Value.ToString("yyyy-MM-dd");
                strActTime = dtpJTime.Value.ToString("HH:mm");
            }
            else
            {
                strACTDATE = strSysDate;
                strActTime = strSysTime;
            }

            //if (chkJ.Checked == true && ComFunc.MsgBoxQ("조사일시 : " + dtpJDate.Value.ToString("yyyy-MM-dd") + " " + dtpJTime.Value.ToString("HH:mm") + ComNum.VBLF + ComNum.VBLF + "저장하시겠습니까?") == DialogResult.No)
            //{
            //    return;
            //}

            if (clsVbfunc.GetCertiOk(clsDB.DbCon, clsType.User.Sabun) == false)
            {
                ComFunc.MsgBox("전자인증서가 없는 사번입니다." + ComNum.VBLF + ComNum.VBLF + " 욕창사정도구표를 작성하실 수 없습니다.");
                return;
            }

            if (gsWard != "ER" && gsWard != "HD")
            {
                if (FstrIpdno == "")
                {
                    ComFunc.MsgBox("저장실패, 환자를 선택해주세요.");
                    return;
                }
            }

            for (i = 0; i < strJUMSU.Length; i++)
            {
                strJUMSU[i] = "";
            }

            strTOT = "";

            strJUMSU[1] = ssView2_Sheet1.Cells[2, 4].Text.Trim();
            if (VB.Val(strJUMSU[1]) < 1)
            {
                ComFunc.MsgBox("기동력 점수가 공란입니다.");
                return;
            }

            strJUMSU[2] = ssView2_Sheet1.Cells[7, 4].Text.Trim();
            if (VB.Val(strJUMSU[2]) < 1)
            {
                ComFunc.MsgBox("신체활동정도 점수가 공란입니다.");
                return;
            }

            strJUMSU[3] = ssView2_Sheet1.Cells[12, 4].Text.Trim();
            if (VB.Val(strJUMSU[3]) < 1)
            {
                ComFunc.MsgBox("감각인지 점수가 공란입니다.");
                return;
            }

            strJUMSU[4] = ssView2_Sheet1.Cells[17, 4].Text.Trim();
            if (VB.Val(strJUMSU[4]) < 1)
            {
                ComFunc.MsgBox("피부가 습기에 노출된 정도 점수가 공란입니다.");
                return;
            }

            strJUMSU[5] = ssView2_Sheet1.Cells[22, 4].Text.Trim();
            if (VB.Val(strJUMSU[5]) < 1)
            {
                ComFunc.MsgBox("마찰력 전단력 점수가 공란입니다.");
                return;
            }

            strJUMSU[6] = ssView2_Sheet1.Cells[27, 4].Text.Trim();
            if (VB.Val(strJUMSU[6]) < 1)
            {
                ComFunc.MsgBox("영양상태 점수가 공란입니다.");
                return;
            }

            strJUMSU[7] = ssView2_Sheet1.Cells[32, 4].Text.Trim();
            if (VB.Val(strJUMSU[7]) < 1)
            {
                ComFunc.MsgBox("조직관류와 산화 점수가 공란입니다.");
                return;
            }

            strTOT = ssView2_Sheet1.Cells[36, 4].Text.Trim();


            strSName = FstrSname;
            strPano = FstrPano;
            strSex = FstrSex;
            strAge = FstrAge;
            strRoom = FstrRoom;
            strDept = FstrDeptCode;
            nIPDNO = VB.Val(FstrIpdno);

            //Data Check

            if (gsWard == "ER" || gsWard == "HD")
            {
                nIPDNO = 0;
            }

            nEMRNO = 0;
            strWARNING = Make_Warning();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                nEMRNO = EMRXML_Insert2(nIPDNO, strJUMSU[1], strJUMSU[2], strJUMSU[3], strJUMSU[4], strJUMSU[5], strJUMSU[6], strJUMSU[7], strTOT, strWARNING, nEMRNO);

                if (nEMRNO == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("EMR 전송에 실패하였습니다.");
                    return;
                }

                if (strROWID == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_CHILD (";
                    SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, JUMSU1, ";
                    SQL = SQL + ComNum.VBLF + " JUMSU2, JUMSU3, JUMSU4, JUMSU5, ";
                    SQL = SQL + ComNum.VBLF + " JUMSU6, JUMSU7, TOTAL, SNAME, ";
                    SQL = SQL + ComNum.VBLF + " SEX, AGE, DEPTCODE, ROOMCODE, ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN, ENTDATE, EMRNO, ACTTIME ) VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + strPano + "'," + nIPDNO + ", TO_DATE('" + strACTDATE + "','YYYY-MM-DD'), " + NVL(strJUMSU[1]) + ",";
                    SQL = SQL + ComNum.VBLF + NVL(strJUMSU[2]) + "," + NVL(strJUMSU[3]) + "," + NVL(strJUMSU[4]) + "," + NVL(strJUMSU[5]) + ",";
                    SQL = SQL + ComNum.VBLF + NVL(strJUMSU[6]) + "," + NVL(strJUMSU[7]) + ", " + NVL(strTOT) + ", '" + strSName + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strSex + "', " + VB.Val(strAge) + ",'" + strDept + "'," + VB.Val(strRoom) + ",";
                    SQL = SQL + ComNum.VBLF + clsType.User.Sabun + ", TO_DATE('" + strSysDate + " " + strSysTime + "','YYYY-MM-DD HH24:MI')," + nEMRNO + ",'" + strActTime + "' ) ";
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_CHILD SET ";
                    SQL = SQL + ComNum.VBLF + " JUMSU1 =" + NVL(strJUMSU[1]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU2 =" + NVL(strJUMSU[2]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU3 =" + NVL(strJUMSU[3]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU4 =" + NVL(strJUMSU[4]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU5 =" + NVL(strJUMSU[5]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU6 =" + NVL(strJUMSU[6]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU7 =" + NVL(strJUMSU[7]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " TOTAL =" + NVL(strTOT) + ",  ";
                    SQL = SQL + ComNum.VBLF + " SNAME ='" + strSName + "',  ";
                    SQL = SQL + ComNum.VBLF + " SEX ='" + strSex + "',  ";
                    SQL = SQL + ComNum.VBLF + " AGE =" + VB.Val(strAge) + ",  ";
                    SQL = SQL + ComNum.VBLF + " DEPTCODE ='" + strDept + "',  ";
                    SQL = SQL + ComNum.VBLF + " ROOMCODE =" + VB.Val(strRoom) + ",  ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN=" + clsType.User.Sabun + ",  ";
                    SQL = SQL + ComNum.VBLF + " EMRNO = " + nEMRNO + ", ";
                    SQL = SQL + ComNum.VBLF + " ENTDATE =TO_DATE('" + strSysDate + " " + strSysTime + "','YYYY-MM-DD HH24:MI')  ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (gsWard != "ER" && gsWard != "HD")
                {
                    SQL = "";
                    SQL = " SELECT ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_PRESSURE_SORE WHERE IPDNO=" + nIPDNO + " ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strRowid2 = dt.Rows[0]["ROWID"].ToString().Trim();

                        SQL = "";
                        SQL = " UPDATE NUR_PRESSURE_SORE  SET ";
                        SQL = SQL + ComNum.VBLF + " TOTAL = " + strTOT + " ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strRowid2 + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            if (dt != null)
                            {
                                dt.Dispose();
                                dt = null;
                            }
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                strOK = Save_Warning(nIPDNO.ToString(), strPano, strACTDATE, strActTime);

                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("저장 중 에러 발생");
                    return;
                }

                strOK = Save_Eval(nIPDNO.ToString(), strPano, strACTDATE, strActTime);

                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("저장 중 에러 발생");
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            Data_Read2();
        }

        void Save_Data3()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double nIPDNO = 0;
            double nEMRNO = 0;

            string strOK = "";
            string strPano = "";
            string strSName = "";
            string strDept = "";
            string strAge = "";
            string strSex = "";
            string strRoom = "";
            string strTOT = "";
            string strWARNING = "";
            string strACTDATE = "";
            string strActTime = "";
            string strROWID = "";
            string strRowid2 = "";
            string[] strJUMSU = new string[9];

            string strSysDate = "";
            string strSysTime = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");

            if (chkJ.Checked == true)
            {
                strACTDATE = dtpJDate.Value.ToString("yyyy-MM-dd");
                strActTime = dtpJTime.Value.ToString("HH:mm");
            }
            else
            {
                strACTDATE = strSysDate;
                strActTime = strSysTime;
            }

            //if (chkJ.Checked == true && ComFunc.MsgBoxQ("조사일시 : " + dtpJDate.Value.ToString("yyyy-MM-dd") + " " + dtpJTime.Value.ToString("HH:mm") + ComNum.VBLF + ComNum.VBLF + "저장하시겠습니까?") == DialogResult.No)
            //{
            //    return;
            //}

            if (clsVbfunc.GetCertiOk(clsDB.DbCon, clsType.User.Sabun) == false)
            {
                ComFunc.MsgBox("전자인증서가 없는 사번입니다." + ComNum.VBLF + ComNum.VBLF + " 욕창사정도구표를 작성하실 수 없습니다.");
                return;
            }

            if (gsWard != "ER" && gsWard != "HD")
            {
                if (FstrIpdno == "")
                {
                    ComFunc.MsgBox("저장실패, 환자를 선택해주세요.");
                    return;
                }
            }

            for (i = 0; i < strJUMSU.Length; i++)
            {
                strJUMSU[i] = "";
            }

            strTOT = "";

            strJUMSU[1] = ssView3_Sheet1.Cells[2, 4].Text.Trim();
            if (VB.Val(strJUMSU[1]) < 1)
            {
                ComFunc.MsgBox("전반적인신체상태 점수가 공란입니다.");
                return;
            }

            strJUMSU[2] = ssView3_Sheet1.Cells[7, 4].Text.Trim();
            if (VB.Val(strJUMSU[2]) < 1)
            {
                ComFunc.MsgBox("기동력 점수가 공란입니다.");
                return;
            }

            strJUMSU[3] = ssView3_Sheet1.Cells[12, 4].Text.Trim();
            if (VB.Val(strJUMSU[3]) < 1)
            {
                ComFunc.MsgBox("신체활동정도 점수가 공란입니다.");
                return;
            }

            strJUMSU[4] = ssView3_Sheet1.Cells[17, 4].Text.Trim();
            if (VB.Val(strJUMSU[4]) < 1)
            {
                ComFunc.MsgBox("감각인지 점수가 공란입니다.");
                return;
            }

            strJUMSU[5] = ssView3_Sheet1.Cells[22, 4].Text.Trim();
            if (VB.Val(strJUMSU[5]) < 1)
            {
                ComFunc.MsgBox("피부가 습기에 노출된 정도 점수가 공란입니다.");
                return;
            }

            strJUMSU[6] = ssView3_Sheet1.Cells[27, 4].Text.Trim();
            if (VB.Val(strJUMSU[6]) < 1)
            {
                ComFunc.MsgBox("마찰력 전단력 점수가 공란입니다.");
                return;
            }

            strJUMSU[7] = ssView3_Sheet1.Cells[32, 4].Text.Trim();
            if (VB.Val(strJUMSU[7]) < 1)
            {
                ComFunc.MsgBox("영양상태 점수가 공란입니다.");
                return;
            }

            strJUMSU[8] = ssView3_Sheet1.Cells[37, 4].Text.Trim();
            if (VB.Val(strJUMSU[8]) < 1)
            {
                ComFunc.MsgBox("조직관류와 산화 점수가 공란입니다.");
                return;
            }

            strTOT = ssView3_Sheet1.Cells[41, 4].Text.Trim();


            strSName = FstrSname;
            strPano = FstrPano;
            strSex = FstrSex;
            strAge = FstrAge;
            strRoom = FstrRoom;
            strDept = FstrDeptCode;
            nIPDNO = VB.Val(FstrIpdno);

            //Data Check

            if (gsWard == "ER" || gsWard == "HD")
            {
                nIPDNO = 0;
            }

            nEMRNO = 0;
            strWARNING = Make_Warning();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                nEMRNO = EMRXML_Insert3(nIPDNO, strJUMSU[1], strJUMSU[2], strJUMSU[3], strJUMSU[4], strJUMSU[5], strJUMSU[6], strJUMSU[7], strJUMSU[8], strTOT, strWARNING, nEMRNO);

                if (nEMRNO == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("EMR 전송에 실패하였습니다.");
                    return;
                }

                if (strROWID == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_BABY (";
                    SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, JUMSU1, ";
                    SQL = SQL + ComNum.VBLF + " JUMSU2, JUMSU3, JUMSU4, JUMSU5, ";
                    SQL = SQL + ComNum.VBLF + " JUMSU6, JUMSU7, JUMSU8, TOTAL, SNAME, ";
                    SQL = SQL + ComNum.VBLF + " SEX, AGE, DEPTCODE, ROOMCODE, ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN, ENTDATE, EMRNO, ACTTIME ) VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + strPano + "'," + nIPDNO + ", TO_DATE('" + strACTDATE + "','YYYY-MM-DD'), " + NVL(strJUMSU[1]) + ",";
                    SQL = SQL + ComNum.VBLF + NVL(strJUMSU[2]) + "," + NVL(strJUMSU[3]) + "," + NVL(strJUMSU[4]) + "," + NVL(strJUMSU[5]) + ",";
                    SQL = SQL + ComNum.VBLF + NVL(strJUMSU[6]) + "," + NVL(strJUMSU[7]) + "," + NVL(strJUMSU[8]) + ", " + NVL(strTOT) + ", '" + strSName + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strSex + "', " + VB.Val(strAge) + ",'" + strDept + "'," + VB.Val(strRoom) + ",";
                    SQL = SQL + ComNum.VBLF + clsType.User.Sabun + ", TO_DATE('" + strSysDate + " " + strSysTime + "','YYYY-MM-DD HH24:MI')," + nEMRNO + ",'" + strActTime + "' ) ";
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_BABY SET ";
                    SQL = SQL + ComNum.VBLF + " JUMSU1 =" + NVL(strJUMSU[1]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU2 =" + NVL(strJUMSU[2]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU3 =" + NVL(strJUMSU[3]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU4 =" + NVL(strJUMSU[4]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU5 =" + NVL(strJUMSU[5]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU6 =" + NVL(strJUMSU[6]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU7 =" + NVL(strJUMSU[7]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " JUMSU8 =" + NVL(strJUMSU[8]) + ",  ";
                    SQL = SQL + ComNum.VBLF + " TOTAL =" + NVL(strTOT) + ",  ";
                    SQL = SQL + ComNum.VBLF + " SNAME ='" + strSName + "',  ";
                    SQL = SQL + ComNum.VBLF + " SEX ='" + strSex + "',  ";
                    SQL = SQL + ComNum.VBLF + " AGE =" + VB.Val(strAge) + ",  ";
                    SQL = SQL + ComNum.VBLF + " DEPTCODE ='" + strDept + "',  ";
                    SQL = SQL + ComNum.VBLF + " ROOMCODE =" + VB.Val(strRoom) + ",  ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN=" + clsType.User.Sabun + ",  ";
                    SQL = SQL + ComNum.VBLF + " EMRNO = " + nEMRNO + ", ";
                    SQL = SQL + ComNum.VBLF + " ENTDATE =TO_DATE('" + strSysDate + " " + strSysTime + "','YYYY-MM-DD HH24:MI')  ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (gsWard != "ER" && gsWard != "HD")
                {
                    SQL = "";
                    SQL = " SELECT ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_PRESSURE_SORE WHERE IPDNO=" + nIPDNO + " ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strRowid2 = dt.Rows[0]["ROWID"].ToString().Trim();

                        SQL = "";
                        SQL = " UPDATE NUR_PRESSURE_SORE  SET ";
                        SQL = SQL + ComNum.VBLF + " TOTAL = " + strTOT + " ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strRowid2 + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            if (dt != null)
                            {
                                dt.Dispose();
                                dt = null;
                            }
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                strOK = Save_Warning(nIPDNO.ToString(), strPano, strACTDATE, strActTime);

                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("저장 중 에러 발생");
                    return;
                }

                strOK = Save_Eval(nIPDNO.ToString(), strPano, strACTDATE, strActTime);

                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("저장 중 에러 발생");
                    return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            Data_Read3();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            PrintDocument pd;
            pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = clsPrint.gGetDefaultPrinter();

            pd.PrintPage += new PrintPageEventHandler(eBarBARPrint);
            pd.Print();    //프린트

        }

        private void eBarBARPrint(object sender, PrintPageEventArgs ev)
        {
            Rectangle r1 = new Rectangle(20, 100, 850, 100);
            Rectangle r2 = new Rectangle(20, 160, 1000, 1500);

            ev.Graphics.DrawString("욕창 사정 도구표 (BRADEN SCALE)", new Font("맑은 고딕", 12f), Brushes.Black, 20, 50, new StringFormat());

            ssInfo.OwnerPrintDraw(ev.Graphics, r1, 0, 1);
            ssView.OwnerPrintDraw(ev.Graphics, r2, 0, 1);
        }

        private void btnPrint1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int j = 0;

            for (i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
            {
                for (j = 4; j < 7; j++)
                {
                    ssView_Sheet1.Cells[i, j].Text = "";
                }
            }

            PrintDocument pd;
            pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = clsPrint.gGetDefaultPrinter();

            pd.PrintPage += new PrintPageEventHandler(eBarBARPrint);
            pd.Print();    //프린트
        }

        void Data_Read()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            double nIPDNO = 0;
            double[] nJumsu = new double[8];

            string[] strJUMSU = new string[8];

            string strSysDate = "";

            ComFunc cf = new ComFunc();

            for (i = 0; i < strJUMSU.Length; i++)
            {
                strJUMSU[i] = "";
            }

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            Screen_Clear();

            FstrROWID = "";

            if ((FstrSname + FstrPano + FstrSex + FstrAge + FstrRoom + FstrDeptCode + FstrIpdno).Trim() == "")
            {
                btnSave.Enabled = false;
                return;
            }

            nIPDNO = VB.Val(FstrIpdno);

            for (i = 0; i < nJumsu.Length; i++)
            {
                nJumsu[i] = 0;
            }

            ssView_Sheet1.ColumnHeader.Cells[0, 3].Text = VB.Mid(strSysDate, 6, 2) + "/" + VB.Right(strSysDate, 2);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT JUMSU1, JUMSU2, JUMSU3, JUMSU4, JUMSU5, JUMSU6, TOTAL, ";
                SQL = SQL + ComNum.VBLF + " ENTSABUN, TO_CHAR(ACTDATE,'MM/DD') ACTDATE, ROWID, ACTTIME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE ";
                if (gsWard == "HD" || gsWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + FstrPano + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND IPDNO =  0";
                    if (FstrActdate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, FstrActdate, 1) + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + FstrActdate + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strSysDate, 1) + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = " + nIPDNO;
                }
                SQL = SQL + ComNum.VBLF + "      AND ENTDATE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE.ACTDATE DESC, ACTTIME DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i >= 3)
                        {
                            break;
                        }

                        if (i == 0)
                        {
                            ssInfo_Sheet1.Cells[0, 1].Text = FstrSname;
                            ssInfo_Sheet1.Cells[0, 3].Text = FstrPano;
                            ssInfo_Sheet1.Cells[0, 5].Text = FstrSex + "/" + FstrAge;
                            ssInfo_Sheet1.Cells[0, 7].Text = FstrRoom;
                            ssInfo_Sheet1.Cells[1, 1].Text = FstrDeptCode;
                            ssInfo_Sheet1.Cells[1, 3].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["EntSabun"].ToString().Trim());

                            if (FstrActdate != "")
                            {
                                ssInfo_Sheet1.Cells[1, 5].Text = FstrActdate;
                            }
                            else
                            {
                                ssInfo_Sheet1.Cells[1, 5].Text = strSysDate;
                            }
                        }

                        strJUMSU[1] = dt.Rows[i]["JUMSU1"].ToString().Trim();
                        strJUMSU[2] = dt.Rows[i]["JUMSU2"].ToString().Trim();
                        strJUMSU[3] = dt.Rows[i]["JUMSU3"].ToString().Trim();
                        strJUMSU[4] = dt.Rows[i]["JUMSU4"].ToString().Trim();
                        strJUMSU[5] = dt.Rows[i]["JUMSU5"].ToString().Trim();
                        strJUMSU[6] = dt.Rows[i]["JUMSU6"].ToString().Trim();
                        strJUMSU[7] = dt.Rows[i]["TOTAL"].ToString().Trim();

                        ssView_Sheet1.Cells[4 - Convert.ToInt32(VB.Val(strJUMSU[1])), i + 4].Text = strJUMSU[1];
                        ssView_Sheet1.Cells[8 - Convert.ToInt32(VB.Val(strJUMSU[2])), i + 4].Text = strJUMSU[2];
                        ssView_Sheet1.Cells[12 - Convert.ToInt32(VB.Val(strJUMSU[3])), i + 4].Text = strJUMSU[3];
                        ssView_Sheet1.Cells[16 - Convert.ToInt32(VB.Val(strJUMSU[4])), i + 4].Text = strJUMSU[4];
                        ssView_Sheet1.Cells[20 - Convert.ToInt32(VB.Val(strJUMSU[5])), i + 4].Text = strJUMSU[5];
                        ssView_Sheet1.Cells[23 - Convert.ToInt32(VB.Val(strJUMSU[6])), i + 4].Text = strJUMSU[6];

                        ssView_Sheet1.ColumnHeader.Cells[0, i + 4].Text = dt.Rows[i]["ACTDATE"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["ACTTIME"].ToString().Trim();

                        ssView_Sheet1.Cells[23, i + 4].Text = strJUMSU[7];

                        ssView_Sheet1.Cells[24, i + 4].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());

                        ssView_Sheet1.Cells[25, i + 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }
                else
                {
                    ssHistory_Sheet1.RowCount = 0;
                    ComFunc.MsgBox("신규자료입니다.");

                    if (FstrActdate != "")
                    {
                        ssView_Sheet1.ColumnHeader.Cells[0, 3].Text = Convert.ToDateTime(FstrActdate).ToString("MM-dd").Replace("-", "/");
                    }
                    else
                    {
                        ssView_Sheet1.ColumnHeader.Cells[0, 3].Text = Convert.ToDateTime(strSysDate).ToString("MM-dd").Replace("-", "/");
                    }

                    ssView_Sheet1.ColumnHeader.Cells[0, 4].Text = " ";
                    ssView_Sheet1.ColumnHeader.Cells[0, 5].Text = " ";
                    ssView_Sheet1.ColumnHeader.Cells[0, 6].Text = " ";

                    ssInfo_Sheet1.Cells[0, 1].Text = FstrSname;
                    ssInfo_Sheet1.Cells[0, 3].Text = FstrPano;
                    ssInfo_Sheet1.Cells[0, 5].Text = FstrSex + "/" + FstrAge;
                    ssInfo_Sheet1.Cells[0, 7].Text = FstrRoom;

                    ssInfo_Sheet1.Cells[1, 1].Text = FstrDeptCode;
                    ssInfo_Sheet1.Cells[1, 3].Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

                    if (FstrActdate != "")
                    {
                        ssInfo_Sheet1.Cells[1, 5].Text = FstrActdate;
                    }
                    else
                    {
                        ssInfo_Sheet1.Cells[1, 5].Text = strSysDate;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            Read_Warning("1", "", nIPDNO.ToString());
            Read_Eval("1", "", nIPDNO.ToString());
            Read_History_List(nIPDNO.ToString(), "");
        }

        void Screen_Clear()
        {
            int i = 0;
            int j = 0;

            if (rdoOpt0.Checked == true)
            {
                ssTemp_Sheet1.RowCount = 0;
                ssTemp_Sheet1.RowCount = 1;

                ssInfo_Sheet1.Cells[0, 1].Text = "";
                ssInfo_Sheet1.Cells[0, 3].Text = "";
                ssInfo_Sheet1.Cells[0, 5].Text = "";
                ssInfo_Sheet1.Cells[0, 7].Text = "";

                ssInfo_Sheet1.Cells[1, 1].Text = "";
                ssInfo_Sheet1.Cells[1, 3].Text = "";
                ssInfo_Sheet1.Cells[1, 5].Text = "";

                for (i = 0; i < 26; i++)
                {
                    for (j = 3; j < 7; j++)
                    {
                        ssView_Sheet1.Cells[i, j].Text = "";
                    }
                }

                ssView_Sheet1.ColumnHeader.Cells[0, 3, 0, 6].Text = "";

                Clear_Eval();
                Clear_Warning();

                ssHistory_Sheet1.RowCount = 0;
            }
            else if (rdoOpt1.Checked == true)
            {
                for (i = 4; i <= 8; i++)
                {
                    for (j = 0; j <= 38; j++)
                    {
                        ssView2_Sheet1.Cells[j, i].Text = "";
                    }
                }

                ssView2_Sheet1.Cells[0, 4].Text = "점 수";

                Clear_Eval();
                Clear_Warning();

                ssHistory_Sheet1.RowCount = 0;
            }
            else if (rdoOpt2.Checked == true)
            {
                for (i = 4; i <= 8; i++)
                {
                    for (j = 0; j <= 42; j++)
                    {
                        ssView3_Sheet1.Cells[j, i].Text = "";
                    }
                }

                ssView3_Sheet1.Cells[0, 4].Text = "점 수";
            }
        }

        private void frnPatBun3_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //If UCase(App.EXEName) = "CAREPLAN" Then
            //    mnuSAVE.Visible = False
            //    mnuPRT.Visible = False
            //    mnuPrePRT.Visible = False
            //    CmdOK.Visible = False
            //    CmdPrint.Visible = False
            //End If

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            pan0.Dock = DockStyle.Fill;
            pan1.Dock = DockStyle.Fill;
            pan2.Dock = DockStyle.Fill;

            switch (GstrFormGbn)
            {
                case "1":
                    rdoOpt0.Checked = true;
                    rdoOpt0.Enabled = true;
                    rdoOpt1.Enabled = false;
                    rdoOpt2.Enabled = false;
                    break;
                case "2":
                    rdoOpt1.Checked = true;
                    rdoOpt0.Enabled = false;
                    rdoOpt1.Enabled = true;
                    rdoOpt2.Enabled = false;
                    break;
                case "3":
                    rdoOpt2.Checked = true;
                    rdoOpt0.Enabled = false;
                    rdoOpt1.Enabled = false;
                    rdoOpt2.Enabled = true;
                    break;
            }

            chkJ.Checked = false;

            //FstrActdate = "";   //VB - GstrHelpName
            dtpJDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpJTime.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));
            chkJ.Checked = true;

            if (gsWard == "ER")
            {
                chkJ.Checked = true;

                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    SQL = "";
                    SQL = " SELECT PTMIINTM ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI";
                    SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + FstrPano + "'";
                    SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + FstrActdate.Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        chkJ.Enabled = true;
                        if (FstrActdate != "")
                        {
                            dtpJDate.Value = Convert.ToDateTime(FstrActdate);
                        }
                        else
                        {
                            dtpJDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                        }

                        dtpJTime.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(dt.Rows[0]["PTMIINTM"].ToString().Trim(), "M"));
                    }

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }

            if (rdoOpt0.Checked == true)
            {
                Data_Read();
            }
            else if (rdoOpt1.Checked == true)
            {
                Data_Read2();
            }
            else if (rdoOpt2.Checked == true)
            {
                Data_Read3();
            }
        }

        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            Tot_Sub();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int x = 0;
            int nTOT = 0;
            int p = 0;

            string strROWID = "";

            if (e.ColumnHeader == true)
            {
                if (e.Column < 3)
                {
                    return;
                }

                strROWID = ssView_Sheet1.Cells[25, e.Column].Text.Trim();

                if (strROWID != "")
                {
                    Delete_Bedsore(strROWID);
                }

                return;
            }

            if (e.Column == 1)
            {
                nTOT = 0;

                if (e.Row >= 0 && e.Row <= 3)
                {
                    x = 1;
                }
                else if (e.Row >= 4 && e.Row <= 7)
                {
                    x = 2;
                }
                else if (e.Row >= 8 && e.Row <= 11)
                {
                    x = 3;
                }
                else if (e.Row >= 12 && e.Row <= 15)
                {
                    x = 4;
                }
                else if (e.Row >= 16 && e.Row <= 19)
                {
                    x = 5;
                }
                else if (e.Row >= 20 && e.Row <= 22)
                {
                    x = 6;
                }

                nTOT = Convert.ToInt32(VB.Val(VB.Left(ssView_Sheet1.Cells[e.Row, 2].Text, 1)));

                if (nTOT == 0)
                {
                    return;
                }
                else
                {
                    switch (x)
                    {
                        case 1:
                            for (p = 0; p <= 3; p++)
                            {
                                ssView_Sheet1.Cells[p, 3].Text = "";
                            }
                            break;
                        case 2:
                            for (p = 4; p <= 7; p++)
                            {
                                ssView_Sheet1.Cells[p, 3].Text = "";
                            }
                            break;
                        case 3:
                            for (p = 8; p <= 11; p++)
                            {
                                ssView_Sheet1.Cells[p, 3].Text = "";
                            }
                            break;
                        case 4:
                            for (p = 12; p <= 15; p++)
                            {
                                ssView_Sheet1.Cells[p, 3].Text = "";
                            }
                            break;
                        case 5:
                            for (p = 16; p <= 19; p++)
                            {
                                ssView_Sheet1.Cells[p, 3].Text = "";
                            }
                            break;
                        case 6:
                            for (p = 20; p <= 22; p++)
                            {
                                ssView_Sheet1.Cells[p, 3].Text = "";
                            }
                            break;
                    }
                }

                ssView_Sheet1.Cells[e.Row, 3].Text = nTOT.ToString();
                ssTemp_Sheet1.Cells[0, x - 1].Text = nTOT.ToString();

                Tot_Sub();
            }
        }

        void Tot_Sub()
        {
            int k = 0;
            int nTOT = 0;

            //분류결과
            for (k = 0; k <= 5; k++)
            {
                nTOT = nTOT + Convert.ToInt32(VB.Val(ssTemp_Sheet1.Cells[0, k].Text));
            }

            ssView_Sheet1.Cells[23, 3].Text = nTOT.ToString();

            btnSave.Enabled = true;
        }

        double EMRXML_Insert(double argIPDNO, string nTOT1, string nTOT2, string nTOT3, string nTOT4, string nTOT5, string nTOT6, string nTOT7, string strWARNING, string strACTDATE = "", string strActTime = "", double newEmrNo = 0)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            double nEMRNO = 0;
            string strInDate = "";
            string strPano = "";
            string strOutDate = "";
            string strOutTime = "";
            string strDeptCode = "";
            string strDrCd = "";
            string strXML = "";
            string strChartX1 = "";
            string strChartX2 = "";

            string strHead = "";
            string strTagHead = "";
            string strTagVal = "";
            string strTagTail = "";
            string strXMLCert = "";

            string strSysDate = "";
            string strSysTime = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");

            if (gsWard == "HD" || gsWard == "ER")
            {
                SQL = "";
                SQL = " SELECT PANO, SNAME, ACTDATE INDATE, ACTDATE OUTDATE, DRCODE, DEPTCODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPano + "' ";
                if (chkJ.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + dtpJDate.Value.AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + dtpJDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "  AND  DEPTCODE = '" + gsWard + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC";
            }
            else
            {
                SQL = "";
                SQL = " SELECT PANO, SNAME, INDATE, OUTDATE, DRCODE, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return 0;
            }

            if (dt.Rows.Count > 0)
            {
                strPano = dt.Rows[0]["PANO"].ToString().Trim();
                strInDate = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
                strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
            }
            else
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("환자 정보가 없습니다. 전자챠트 전송에 실패하였습니다.");
                return 0;
            }

            dt.Dispose();
            dt = null;

            if (chkJ.Checked == false)
            {
                strACTDATE = strSysDate.Replace("-", "");
                strActTime = strSysTime.Replace(":", "");
            }

            EmrPatient pAcp = SetEmrPatInfoOcs(clsDB.DbCon, FstrPano, (gsWard == "ER" || gsWard == "HD") == true ? "O" : "I", strInDate.Replace("-", ""), strDeptCode);
            EmrForm pForm = SerEmrFormUpdateNo(clsDB.DbCon, "2479");

            if (pForm.FmOLDGB == 1)
            {
                #region xml
                strXML = "";

                strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
                strChartX1 = "<chart>";
                strChartX2 = "</chart>";

                strXML = strHead + strChartX1;

                strTagHead = "<it1 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "감각인지" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT1;
                strTagTail = "]]></it1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it2 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "습한정도" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT2;
                strTagTail = "]]></it2>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it3 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "신체활동정도" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT3;
                strTagTail = "]]></it3>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it4 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "기동력" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT4;
                strTagTail = "]]></it4>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it5 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "영양상태" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT5;
                strTagTail = "]]></it5>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it6 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "마찰력과 전단력" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT6;
                strTagTail = "]]></it6>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it7 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "총점" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT7;
                strTagTail = "]]></it7>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it30 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "재평가" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strWARNING;
                strTagTail = "]]></it30>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strXMLCert = strXML;

                strXML = strXML + strChartX2;

                strXMLCert = strXML;

                SQL = "";
                SQL = "SELECT " + ComNum.DB_EMR + "GetEmrXmlNo() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return 0;
                }

                if (dt.Rows.Count > 0)
                {
                    nEMRNO = VB.Val(dt.Rows[0]["FUNSEQNO"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;


                if (CREATE_EMR_XMLINSRT3(nEMRNO, "2479", clsType.User.IdNumber, strACTDATE.Replace("-", ""), strActTime.Replace(":", ""),
                    0, strPano, ((gsWard == "ER" || gsWard == "HD") == true ? "O" : "I"), strInDate.Replace("-", ""), "120000",
                    strOutDate.Replace("-", ""), strOutTime, strDeptCode, strDrCd, "0", 1, strXML) == false)
                {
                    nEMRNO = 0;
                }
                #endregion
            }
            else
            {
                Dictionary<string, string> strContent = new Dictionary<string, string>();
                strContent.Add("I0000037690", nTOT1);
                strContent.Add("I0000037691", nTOT2);
                strContent.Add("I0000037692", nTOT3);
                strContent.Add("I0000037693", nTOT4);
                strContent.Add("I0000000106", nTOT5);
                strContent.Add("I0000037694", nTOT6);
                strContent.Add("I0000000427", nTOT7);
                strContent.Add("I0000037396", strWARNING);
                nEMRNO =  SaveNurChartFlow(clsDB.DbCon, this, newEmrNo, pAcp, pForm, strACTDATE.Replace("-", ""), strActTime.Replace(":", ""), strContent);
            }

            return nEMRNO;
        }

        double EMRXML_Insert2(double argIPDNO, string nTOT1, string nTOT2, string nTOT3, string nTOT4, string nTOT5, string nTOT6, string nTOT7, string nTotal, string strWARNING, double newEmrNo = 0)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            double nEMRNO = 0;
            string strInDate = "";
            string strPano = "";
            string strOutDate = "";
            string strOutTime = "";
            string strDeptCode = "";
            string strDrCd = "";
            string strXML = "";
            string strChartX1 = "";
            string strChartX2 = "";

            string strHead = "";
            string strTagHead = "";
            string strTagVal = "";
            string strTagTail = "";
            string strXMLCert = "";

            string strActDate = "";
            string strActTime = "";

            string strSysDate = "";
            string strSysTime = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");

            try
            {
                if (gsWard == "HD" || gsWard == "ER")
                {
                    SQL = "";
                    SQL = " SELECT PANO, SNAME, ACTDATE INDATE, ACTDATE OUTDATE, DRCODE, DEPTCODE";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPano + "' ";
                    if (chkJ.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + dtpJDate.Value.AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + dtpJDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                    }
                    SQL = SQL + ComNum.VBLF + "  AND  DEPTCODE = '" + gsWard + "' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT PANO, SNAME, INDATE, OUTDATE, DRCODE, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return 0;
                }

                if (dt.Rows.Count > 0)
                {
                    strPano = dt.Rows[0]["PANO"].ToString().Trim();
                    strInDate = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
                    strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                    strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                    strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("환자 정보가 없습니다. 전자챠트 전송에 실패하였습니다.");
                    return 0;
                }

                dt.Dispose();
                dt = null;

                EmrPatient pAcp = SetEmrPatInfoOcs(clsDB.DbCon, FstrPano, (gsWard == "ER" || gsWard == "HD") == true ? "O" : "I", strInDate.Replace("-", ""), strDeptCode);
                EmrForm pForm = SerEmrFormUpdateNo(clsDB.DbCon, "2480");

                if (chkJ.Checked == true)
                {
                    strActDate = dtpJDate.Value.ToString("yyyyMMdd");
                    strActTime = dtpJTime.Value.ToString("HHmmss");
                }
                else
                {
                    strActDate = strSysDate.Replace("-", "");
                    strActTime = strSysTime.Replace(":", "");
                }

                if (pForm.FmOLDGB == 1)
                {
                    SQL = "";
                    SQL = "SELECT " + ComNum.DB_EMR + "GetEmrXmlNo() FunSeqNo FROM Dual";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return 0;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        nEMRNO = VB.Val(dt.Rows[0]["FUNSEQNO"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    strXML = "";

                    strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
                    strChartX1 = "<chart>";
                    strChartX2 = "</chart>";

                    strXML = strHead + strChartX1;

                    strTagHead = "<it2 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "기동력" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT1;
                    strTagTail = "]]></it2>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strTagHead = "<it3 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "신체활동정도" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT2;
                    strTagTail = "]]></it3>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strTagHead = "<it4 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "감각인지" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT3;
                    strTagTail = "]]></it4>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strTagHead = "<it5 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "피부가습기에노출된정도" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT4;
                    strTagTail = "]]></it5>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strTagHead = "<it6 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "마찰력전단력" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT5;
                    strTagTail = "]]></it6>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strTagHead = "<it7 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "영양상태" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT6;
                    strTagTail = "]]></it7>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strTagHead = "<it8 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "조직관류와산화" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT7;
                    strTagTail = "]]></it8>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strTagHead = "<it9 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "총점" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTotal;
                    strTagTail = "]]></it9>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strTagHead = "<it30 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "재평가" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = strWARNING;
                    strTagTail = "]]></it30>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strXMLCert = strXML;

                    strXML = strXML + strChartX2;
                    strXMLCert = strXML;

                    if (CREATE_EMR_XMLINSRT3(nEMRNO, "2480", clsType.User.IdNumber, strActDate, strActTime,
       0, strPano, ((gsWard == "ER" || gsWard == "HD") == true ? "O" : "I"), strInDate.Replace("-", ""), "120000",
       strOutDate.Replace("-", ""), strOutTime, strDeptCode, strDrCd, "0", 1, strXML) == false)
                    {
                        nEMRNO = 0;
                    }
                }
                else
                {
                    Dictionary<string, string> strContent = new Dictionary<string, string>();
                    strContent.Add("I0000037693", nTOT1);
                    strContent.Add("I0000037692", nTOT2);
                    strContent.Add("I0000037690", nTOT3);
                    strContent.Add("I0000037882", nTOT4);
                    strContent.Add("I0000037694", nTOT5);
                    strContent.Add("I0000000106", nTOT6);
                    strContent.Add("I0000037883", nTOT7);
                    strContent.Add("I0000000427", nTotal);
                    strContent.Add("I0000037396", strWARNING);
                    nEMRNO = SaveNurChartFlow(clsDB.DbCon, this, newEmrNo, pAcp, pForm, strActDate.Replace("-", ""), strActTime.Replace(":", ""), strContent);
                }
            
                return nEMRNO;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return 0;
            }
        }

        double EMRXML_Insert3(double argIPDNO, string nTOT1, string nTOT2, string nTOT3, string nTOT4, string nTOT5, string nTOT6, string nTOT7, string nTOT8, string nTotal, string strWARNING, double newEmrNo = 0)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            double nEMRNO = 0;
            string strInDate = "";
            string strPano = "";
            string strOutDate = "";
            string strOutTime = "";
            string strDeptCode = "";
            string strDrCd = "";
            string strXML = "";
            string strChartX1 = "";
            string strChartX2 = "";

            string strHead = "";
            string strTagHead = "";
            string strTagVal = "";
            string strTagTail = "";
            string strXMLCert = "";

            string strActDate = "";
            string strActTime = "";

            string strSysDate = "";
            string strSysTime = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");

            if (gsWard == "HD" || gsWard == "ER")
            {
                SQL = "";
                SQL = " SELECT PANO, SNAME, ACTDATE INDATE, ACTDATE OUTDATE, DRCODE, DEPTCODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPano + "' ";
                if (chkJ.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + dtpJDate.Value.AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + dtpJDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "  AND  DEPTCODE = '" + gsWard + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC";
            }
            else
            {
                SQL = "";
                SQL = " SELECT PANO, SNAME, INDATE, OUTDATE, DRCODE, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return 0;
            }

            if (dt.Rows.Count > 0)
            {
                strPano = dt.Rows[0]["PANO"].ToString().Trim();
                strInDate = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
                strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
            }
            else
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("환자 정보가 없습니다. 전자챠트 전송에 실패하였습니다.");
                return 0;
            }

            dt.Dispose();
            dt = null;

            if (chkJ.Checked == true)
            {
                strActDate = dtpJDate.Value.ToString("yyyyMMdd");
                strActTime = dtpJTime.Value.ToString("HHmmss");
            }
            else
            {
                strActDate = strSysDate.Replace("-", "");
                strActTime = strSysTime.Replace(":", "");
            }

            EmrPatient pAcp = SetEmrPatInfoOcs(clsDB.DbCon, FstrPano, (gsWard == "ER" || gsWard == "HD") == true ? "O" : "I", strInDate.Replace("-", ""), strDeptCode);
            EmrForm pForm = SerEmrFormUpdateNo(clsDB.DbCon, "2481");

            if (pForm.FmOLDGB == 1)
            {
                strXML = "";

                strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
                strChartX1 = "<chart>";
                strChartX2 = "</chart>";

                strXML = strHead + strChartX1;

                strTagHead = "<it1 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "전반적인신체상태" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT1;
                strTagTail = "]]></it1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it2 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "기동력" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT2;
                strTagTail = "]]></it2>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it3 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "신체활동정도" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT3;
                strTagTail = "]]></it3>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it4 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "감각인지" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT4;
                strTagTail = "]]></it4>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it5 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "피부가습기에노출된정도" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT5;
                strTagTail = "]]></it5>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it6 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "마찰력전단력" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT6;
                strTagTail = "]]></it6>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it7 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "영양상태" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT7;
                strTagTail = "]]></it7>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it8 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "조직관류와산화" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTOT8;
                strTagTail = "]]></it8>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it9 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "총점" + VB.Chr(34) + "><![CDATA[";
                strTagVal = nTotal;
                strTagTail = "]]></it9>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it30 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "재평가" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strWARNING;
                strTagTail = "]]></it30>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strXMLCert = strXML;

                strXML = strXML + strChartX2;

                strXMLCert = strXML;

                SQL = "";
                SQL = "SELECT " + ComNum.DB_EMR + "GetEmrXmlNo() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return 0;
                }

                if (dt.Rows.Count > 0)
                {
                    nEMRNO = VB.Val(dt.Rows[0]["FUNSEQNO"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;


                if (CREATE_EMR_XMLINSRT3(nEMRNO, "2481", clsType.User.IdNumber, strActDate, strActTime,
                    0, strPano, ((gsWard == "ER" || gsWard == "HD") == true ? "O" : "I"), strInDate.Replace("-", ""), "120000",
                    strOutDate.Replace("-", ""), strOutTime, strDeptCode, strDrCd, "0", 1, strXML) == false)
                {
                    nEMRNO = 0;
                }
            }
            else
            {
                Dictionary<string, string> strContent = new Dictionary<string, string>();
                strContent.Add("I0000037884", nTOT1);
                strContent.Add("I0000037693", nTOT2);
                strContent.Add("I0000037692", nTOT3);
                strContent.Add("I0000037690", nTOT4);
                strContent.Add("I0000037882", nTOT5);
                strContent.Add("I0000037694", nTOT6);
                strContent.Add("I0000000106", nTOT7);
                strContent.Add("I0000037883", nTOT8);
                strContent.Add("I0000000427", nTotal);
                strContent.Add("I0000037396", strWARNING);

                nEMRNO = SaveNurChartFlow(clsDB.DbCon, this, newEmrNo, pAcp, pForm, strActDate.Replace("-", ""), strActTime.Replace(":", ""), strContent);

            }

            return nEMRNO;
        }

        void Delete_Bedsore(string argROWID)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double nEMRNO = 0;
            string strACTDATE = "";
            string strActTime = "";
            string strPano = "";

            double dblEmrHisNo = 0;

            string strSysDate = "";
            string strSysTime = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");

            if (ComFunc.MsgBoxQ("해당 욕창기록을 삭제합니다.") == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT EMRNO, ACTDATE, ACTTIME, PANO ";
                if (rdoOpt0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE";
                }
                else if (rdoOpt1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_CHILD";
                }
                else if (rdoOpt2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_BABY";
                }
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nEMRNO = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
                    strACTDATE = VB.Left(dt.Rows[0]["ACTDATE"].ToString().Trim(), 10);
                    strActTime = dt.Rows[0]["ACTTIME"].ToString().Trim();
                    strPano = dt.Rows[0]["PANO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                if (rdoOpt0.Checked == true)
                {
                    SQL = " DELETE " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE ";
                }
                else if (rdoOpt1.Checked == true)
                {
                    SQL = " DELETE " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_CHILD ";
                }
                else if (rdoOpt2.Checked == true)
                {
                    SQL = " DELETE " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_BABY ";
                }
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (Delete_Warning(strPano, strACTDATE, strActTime) == "NO")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("차트 백업 중에 오류가 발생함");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (nEMRNO > 0)
                {
                    //기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
                    //KOSMOS_EMR.EMRXMLHISTORY_HISTORYNO_SEQ

                    dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "KOSMOS_EMR.EMRXMLHISNO");

                    #region 이전 XML 

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                    SQL = SQL + ComNum.VBLF + "      '" + strSysDate.Replace("-", "") + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + strSysTime.Replace(":", "") + "', '" + clsType.User.Sabun + "',CERTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    #endregion

                    #region 신규
                    if (SaveChartMastHis(clsDB.DbCon, nEMRNO.ToString(), dblEmrHisNo, strSysDate.Replace("-", ""), strSysTime.Replace(":", ""), "C", "") != "OK")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    #endregion
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (rdoOpt0.Checked == true)
            {
                Data_Read();
            }
            else if (rdoOpt1.Checked == true)
            {
                Data_Read2();
            }
            else if (rdoOpt2.Checked == true)
            {
                Data_Read3();
            }
        }

        void Clear_Warning()
        {
            //ssWarning_Sheet1.Cells[1, 3].Value = false;
            //ssWarning_Sheet1.Cells[1, 4].Value = false;
            //ssWarning_Sheet1.Cells[1, 5].Value = false;
            //ssWarning_Sheet1.Cells[1, 6].Value = false;
            //ssWarning_Sheet1.Cells[1, 8].Value = false;

            ssWarning_Sheet1.Cells[2, 3].Value = false;
            ssWarning_Sheet1.Cells[2, 5].Value = false;
            ssWarning_Sheet1.Cells[2, 7].Value = false;
            ssWarning_Sheet1.Cells[2, 8].Value = false;

            ssWarning_Sheet1.Cells[3, 3].Value = false;
            ssWarning_Sheet1.Cells[3, 6].Value = false;
            ssWarning_Sheet1.Cells[3, 7].Value = false;
        }

        void Read_Warning(string argGubun, string argPano, string argIPDNO, string argROWID = "")
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strACTDATE = "";

            ComFunc cf = new ComFunc();

            Clear_Warning();

            if (argPano == "")
            {
                argPano = ssInfo_Sheet1.Cells[0, 3].Text.Trim();
            }

            strACTDATE = ssInfo_Sheet1.Cells[1, 5].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "  WARD_ICU, BRADEN, BRADEN_IN, BRADEN_OUT,";
                SQL = SQL + ComNum.VBLF + "  BRADEN_OK, GRADE_HIGH, PARAL, COMA,";
                SQL = SQL + ComNum.VBLF + "  NOT_MOVE, DIET_FAIL, NEED_PROTEIN, EDEMA ";
                if (argGubun == "1")
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING_HISTORY ";
                }
                if (argROWID != "")
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + argROWID + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";

                    if (gsWard == "HD" || gsWard == "ER")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strACTDATE, 1) + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND IPDNO = 0";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + argIPDNO;
                    }
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    //ssWarning_Sheet1.Cells[1, 3].Value = dt.Rows[0]["WARD_ICU"].ToString().Trim() == "1" ? true : false;
                    //ssWarning_Sheet1.Cells[1, 4].Value = dt.Rows[0]["BRADEN"].ToString().Trim() == "1" ? true : false;
                    //ssWarning_Sheet1.Cells[1, 5].Value = dt.Rows[0]["BRADEN_IN"].ToString().Trim() == "1" ? true : false;
                    //ssWarning_Sheet1.Cells[1, 6].Value = dt.Rows[0]["BRADEN_OUT"].ToString().Trim() == "1" ? true : false;
                    //ssWarning_Sheet1.Cells[1, 8].Value = dt.Rows[0]["BRADEN_OK"].ToString().Trim() == "1" ? true : false;

                    ssWarning_Sheet1.Cells[2, 3].Value = dt.Rows[0]["GRADE_HIGH"].ToString().Trim() == "1" ? true : false;
                    ssWarning_Sheet1.Cells[2, 5].Value = dt.Rows[0]["PARAL"].ToString().Trim() == "1" ? true : false;
                    ssWarning_Sheet1.Cells[2, 7].Value = dt.Rows[0]["NOT_MOVE"].ToString().Trim() == "1" ? true : false;
                    ssWarning_Sheet1.Cells[2, 8].Value = dt.Rows[0]["EDEMA"].ToString().Trim() == "1" ? true : false;

                    ssWarning_Sheet1.Cells[3, 3].Value = dt.Rows[0]["COMA"].ToString().Trim() == "1" ? true : false;
                    ssWarning_Sheet1.Cells[3, 6].Value = dt.Rows[0]["DIET_FAIL"].ToString().Trim() == "1" ? true : false;
                    ssWarning_Sheet1.Cells[3, 8].Value = dt.Rows[0]["NEED_PROTEIN"].ToString().Trim() == "1" ? true : false;
                }

                dt.Dispose();
                dt = null;

                if (argGubun == "1")
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        string Save_Warning(string argIPDNO, string argPano, string argACTDATE, string argACTTIME)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPano = "";
            string strIPDNO = "";
            string strACTDATE = "";
            string strWARD_ICU = "";
            string strBraden = "";
            string strBRADEN_IN = "";
            string strBRADEN_OUT = "";
            string strBRADEN_OK = "";
            string strGRADE_HIGH = "";
            string strPARAL = "";
            string strCOMA = "";
            string strNOT_MOVE = "";
            string strDIET_FAIL = "";
            string strNEED_PROTEIN = "";
            string strEDEMA = "";

            string rtnVar = "";

            strPano = argPano;
            strIPDNO = argIPDNO;
            strACTDATE = argACTDATE;

            rtnVar = "NO";

            //strWARD_ICU = Convert.ToBoolean(ssWarning_Sheet1.Cells[1, 3].Value) == true ? "1" : "0";
            //strBraden = Convert.ToBoolean(ssWarning_Sheet1.Cells[1, 4].Value) == true ? "1" : "0";
            //strBRADEN_IN = Convert.ToBoolean(ssWarning_Sheet1.Cells[1, 5].Value) == true ? "1" : "0";
            //strBRADEN_OUT = Convert.ToBoolean(ssWarning_Sheet1.Cells[1, 6].Value) == true ? "1" : "0";
            //strBRADEN_OK = Convert.ToBoolean(ssWarning_Sheet1.Cells[1, 8].Value) == true ? "1" : "0";

            //저장할 당시 
            SQL = " SELECT PANO FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
            SQL += ComNum.VBLF + "   AND WARDCODE IN ('33','35') ";
            SQL += ComNum.VBLF + "   AND (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR OUTDATE = TRUNC(SYSDATE))";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }

            if (dt.Rows.Count > 0)
            {
                strWARD_ICU = "1";
            }
            else
            {
                strWARD_ICU = "0";
            }


            dt.Dispose();
            dt = null;

            strBraden =  "0";
            strBRADEN_IN = "0";
            strBRADEN_OUT =  "0";
            strBRADEN_OK = "0";

            strGRADE_HIGH = Convert.ToBoolean(ssWarning_Sheet1.Cells[2, 3].Value) == true ? "1" : "0";
            strPARAL = Convert.ToBoolean(ssWarning_Sheet1.Cells[2, 5].Value) == true ? "1" : "0";
            strNOT_MOVE = Convert.ToBoolean(ssWarning_Sheet1.Cells[2, 7].Value) == true ? "1" : "0";
            strEDEMA = Convert.ToBoolean(ssWarning_Sheet1.Cells[2, 8].Value) == true ? "1" : "0";

            strCOMA = Convert.ToBoolean(ssWarning_Sheet1.Cells[3, 3].Value) == true ? "1" : "0";
            strDIET_FAIL = Convert.ToBoolean(ssWarning_Sheet1.Cells[3, 6].Value) == true ? "1" : "0";
            strNEED_PROTEIN = Convert.ToBoolean(ssWarning_Sheet1.Cells[3, 8].Value) == true ? "1" : "0";



            SQL = "";
            SQL = " SELECT ROWID ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";
            if (gsWard == "HD" || gsWard == "ER")
            {
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = 0";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + argIPDNO;
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }

            if (dt.Rows.Count > 0)
            {
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING_HISTORY (";
                SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                SQL = SQL + ComNum.VBLF + " GUBUN, WARD_ICU, BRADEN, BRADEN_IN,";
                SQL = SQL + ComNum.VBLF + " BRADEN_OUT, BRADEN_OK, GRADE_HIGH, PARAL, COMA,";
                SQL = SQL + ComNum.VBLF + " NOT_MOVE, DIET_FAIL, NEED_PROTEIN, EDEMA, ";
                SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, ACTTIME )";
                SQL = SQL + ComNum.VBLF + " SELECT  ";
                SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                SQL = SQL + ComNum.VBLF + " GUBUN, WARD_ICU, BRADEN, BRADEN_IN,";
                SQL = SQL + ComNum.VBLF + " BRADEN_OUT, BRADEN_OK, GRADE_HIGH, PARAL, COMA,";
                SQL = SQL + ComNum.VBLF + " NOT_MOVE, DIET_FAIL, NEED_PROTEIN, EDEMA, ";
                SQL = SQL + ComNum.VBLF + " SYSDATE, " + clsType.User.Sabun + ", ACTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }
            }

            dt.Dispose();
            dt = null;


            SQL = "";
            SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING (";
            SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
            SQL = SQL + ComNum.VBLF + " GUBUN, WARD_ICU, BRADEN, BRADEN_IN,";
            SQL = SQL + ComNum.VBLF + " BRADEN_OUT, BRADEN_OK, GRADE_HIGH, PARAL, COMA,";
            SQL = SQL + ComNum.VBLF + " NOT_MOVE, DIET_FAIL, NEED_PROTEIN, EDEMA, ACTTIME) VALUES ( ";
            SQL = SQL + ComNum.VBLF + "'" + strPano + "', " + strIPDNO + ", TO_DATE('" + strACTDATE + "','YYYY-MM-DD'), " + clsType.User.Sabun + ", ";
            SQL = SQL + ComNum.VBLF + "'1','" + strWARD_ICU + "','" + strBraden + "','" + strBRADEN_IN + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strBRADEN_OUT + "','" + strBRADEN_OK + "','" + strGRADE_HIGH + "','" + strPARAL + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strCOMA + "','" + strNOT_MOVE + "','" + strDIET_FAIL + "','" + strNEED_PROTEIN + "','" + strEDEMA + "','" + argACTTIME + "')";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(SqlErr);
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }



            rtnVar = "OK";
            return rtnVar;

        }

        void Clear_Eval()
        {
            ssEval_Sheet1.Cells[1, 3].Value = false;
            ssEval_Sheet1.Cells[1, 4].Value = false;
            ssEval_Sheet1.Cells[1, 5].Value = false;

            ssEval_Sheet1.Cells[3, 3].Value = false;
            ssEval_Sheet1.Cells[3, 5].Value = false;
        }

        void Read_Eval(string argGubun, string argPano, string argIPDNO, string argROWID = "")
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strACTDATE = "";

            ComFunc cf = new ComFunc();

            Clear_Eval();

            if (argPano == "")
            {
                argPano = ssInfo_Sheet1.Cells[0, 3].Text.Trim();
            }

            strACTDATE = ssInfo_Sheet1.Cells[1, 5].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                SQL = SQL + ComNum.VBLF + " GUBUN, TRANSFOR, OP, PAT_CHANGE,";
                SQL = SQL + ComNum.VBLF + " YOK, PAT_CHANGE2 ";
                if (argGubun == "1")
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_EVAL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_EVAL_HISTORY ";
                }

                if (argROWID != "")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";
                    if (gsWard == "HD" || gsWard == "ER")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND IPDNO = 0";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strACTDATE, 1) + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + argIPDNO;
                    }
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssEval_Sheet1.Cells[1, 3].Value = dt.Rows[0]["TRANSFOR"].ToString().Trim() == "1" ? true : false;
                ssEval_Sheet1.Cells[1, 4].Value = dt.Rows[0]["OP"].ToString().Trim() == "1" ? true : false;
                ssEval_Sheet1.Cells[1, 5].Value = dt.Rows[0]["PAT_CHANGE"].ToString().Trim() == "1" ? true : false;

                ssEval_Sheet1.Cells[3, 3].Value = dt.Rows[0]["YOK"].ToString().Trim() == "1" ? true : false;
                ssEval_Sheet1.Cells[3, 5].Value = dt.Rows[0]["PAT_CHANGE2"].ToString().Trim() == "1" ? true : false;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        string Save_Eval(string argIPDNO, string argPano, string argACTDATE, string argACTTIME)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPano = "";
            string strIPDNO = "";
            string strACTDATE = "";
            string strTRANSFOR = "";
            string strOP = "";
            string strPAT_CHANGE = "";

            string strYOK = "";
            string strPAT_CHANGE2 = "";

            string rtnVar = "";

            strPano = argPano;
            strIPDNO = argIPDNO;
            strACTDATE = argACTDATE;

            strTRANSFOR = Convert.ToBoolean(ssEval_Sheet1.Cells[1, 3].Value) == true ? "1" : "0";
            strOP = Convert.ToBoolean(ssEval_Sheet1.Cells[1, 4].Value) == true ? "1" : "0";
            strPAT_CHANGE = Convert.ToBoolean(ssEval_Sheet1.Cells[1, 5].Value) == true ? "1" : "0";

            strYOK = Convert.ToBoolean(ssEval_Sheet1.Cells[3, 3].Value) == true ? "1" : "0";
            strPAT_CHANGE2 = Convert.ToBoolean(ssEval_Sheet1.Cells[3, 5].Value) == true ? "1" : "0";


            SQL = "";
            SQL = " SELECT ROWID ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_EVAL ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";
            if (gsWard == "HD" || gsWard == "ER")
            {
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = 0";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + argIPDNO;
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }

            if (dt.Rows.Count > 0)
            {
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_BRADEN_EVAL_HISTORY (";
                SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                SQL = SQL + ComNum.VBLF + " GUBUN, TRANSFOR, OP, PAT_CHANGE, ";
                SQL = SQL + ComNum.VBLF + " YOK, PAT_CHANGE2, DELDATE, DELSABUN, ACTTIME )";
                SQL = SQL + ComNum.VBLF + " SELECT  ";
                SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                SQL = SQL + ComNum.VBLF + " GUBUN, TRANSFOR, OP, PAT_CHANGE, ";
                SQL = SQL + ComNum.VBLF + " YOK, PAT_CHANGE2, SYSDATE, " + clsType.User.Sabun + ", ACTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_EVAL ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "NUR_BRADEN_EVAL ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }
            }

            dt.Dispose();
            dt = null;

            SQL = "";
            SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_BRADEN_EVAL (";
            SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
            SQL = SQL + ComNum.VBLF + " GUBUN, TRANSFOR, OP, PAT_CHANGE, ";
            SQL = SQL + ComNum.VBLF + " YOK, PAT_CHANGE2, ACTTIME) VALUES ( ";
            SQL = SQL + ComNum.VBLF + "'" + strPano + "', " + strIPDNO + ", TO_DATE('" + strACTDATE + "','YYYY-MM-DD'), " + clsType.User.Sabun + ", ";
            SQL = SQL + ComNum.VBLF + "'1','" + strTRANSFOR + "','" + strOP + "','" + strPAT_CHANGE + "',";
            SQL = SQL + ComNum.VBLF + "'" + strYOK + "','" + strPAT_CHANGE2 + "','" + argACTTIME + "')";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(SqlErr);
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }


            rtnVar = "OK";
            return rtnVar;

        }

        string Make_Warning()
        {
            string strTemp = "";

            if (Convert.ToBoolean(ssEval_Sheet1.Cells[1, 3].Value) == true)
            {
                strTemp = strTemp + "전동 ";
            }

            if (Convert.ToBoolean(ssEval_Sheet1.Cells[1, 4].Value) == true)
            {
                strTemp = strTemp + "수술/시술 ";
            }

            if (Convert.ToBoolean(ssEval_Sheet1.Cells[1, 5].Value) == true)
            {
                strTemp = strTemp + "진정치료(검사) ";
            }

            if (Convert.ToBoolean(ssEval_Sheet1.Cells[3, 3].Value) == true)
            {
                strTemp = strTemp + "욕창 발생 시 ";
            }

            if (Convert.ToBoolean(ssEval_Sheet1.Cells[3, 5].Value) == true)
            {
                strTemp = strTemp + "신체상태악화 ";
            }

            return strTemp;
        }

        string NVL(string arg)
        {
            if (arg == "")
            {
                return "NULL";
            }
            else
            {
                return arg;
            }
        }

        void Read_History_List(string argIPDNO, string argPano)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strACTDATE = "";

            ComFunc cf = new ComFunc();

            if (argPano == "")
            {
                argPano = ssInfo_Sheet1.Cells[0, 3].Text.Trim();
            }

            strACTDATE = ssInfo_Sheet1.Cells[1, 5].Text.Trim();

            ssHistory_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.ACTDATE, A.PANO, A.IPDNO, '1' GUBUN, A.ROWID ROWID1, B.ROWID ROWID2, A.ACTTIME  ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_BRADEN_WARNING A, KOSMOS_PMPA.NUR_BRADEN_EVAL B ";
                if (gsWard == "HD" || gsWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + argPano + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND A.ACTDATE <= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strACTDATE, 1) + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND A.ACTDATE >= TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND A.IPDNO = 0";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + argIPDNO;
                }
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = B.ACTDATE ";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTTIME = B.ACTTIME ";
                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT A.ACTDATE, A.PANO, A.IPDNO, '2' GUBUN, A.ROWID ROWID1, B.ROWID ROWID2, A.ACTTIME  ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_BRADEN_WARNING_HISTORY A, KOSMOS_PMPA.NUR_BRADEN_EVAL_HISTORY B ";
                if (gsWard == "HD" || gsWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + argPano + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND A.ACTDATE <= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strACTDATE, 1) + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND A.ACTDATE >= TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND A.IPDNO = 0";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + argIPDNO;
                }
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = B.ACTDATE ";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTTIME = B.ACTTIME ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC, ACTTIME DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssHistory_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssHistory_Sheet1.Cells[i, 0].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                    ssHistory_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                    ssHistory_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID1"].ToString().Trim();
                    ssHistory_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID2"].ToString().Trim();
                    ssHistory_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ACTTIME"].ToString().Trim();
                    ssHistory_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void ssHistory_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssHistory_Sheet1.RowCount == 0)
            {
                return;
            }

            string strGubun = "";
            string strPano = "";
            string strIPDNO = "";
            string strACTDATE = "";
            string strROWID = "";
            string strRowid2 = "";

            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            strACTDATE = ssHistory_Sheet1.Cells[e.Row, 0].Text.Trim();
            strPano = ssHistory_Sheet1.Cells[e.Row, 1].Text.Trim();
            strIPDNO = ssHistory_Sheet1.Cells[e.Row, 2].Text.Trim();
            strROWID = ssHistory_Sheet1.Cells[e.Row, 3].Text.Trim();
            strRowid2 = ssHistory_Sheet1.Cells[e.Row, 4].Text.Trim();
            strGubun = ssHistory_Sheet1.Cells[e.Row, 6].Text.Trim();

            Read_Warning(strGubun, strPano, strIPDNO, strROWID);
            Read_Eval(strGubun, strPano, strIPDNO, strRowid2);

            if (strGubun == "1")
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        string Delete_Warning(string argPano, string argACTDATE, string argACTTIME)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string rtnVar = "";

            SQL = "";
            SQL = " DELETE " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "   AND ACTTIME = '" + argACTTIME + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ACTTIME IS NOT NULL ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(SqlErr);
                return rtnVar;
            }

            SQL = "";
            SQL = " DELETE " + ComNum.DB_PMPA + "NUR_BRADEN_EVAL ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "   AND ACTTIME = '" + argACTTIME + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ACTTIME IS NOT NULL ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(SqlErr);
                return rtnVar;
            }

            SQL = "";
            SQL = " DELETE " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING_HISTORY ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "   AND ACTTIME = '" + argACTTIME + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ACTTIME IS NOT NULL ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(SqlErr);
                return rtnVar;
            }

            SQL = "";
            SQL = " DELETE " + ComNum.DB_PMPA + "NUR_BRADEN_EVAL_HISTORY ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "   AND ACTTIME = '" + argACTTIME + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ACTTIME IS NOT NULL ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(SqlErr);
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }

            rtnVar = "OK";
            return rtnVar;

        }

        private void rdoOpt_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOpt0.Checked == true)
            {
                btnSearch.Enabled = false;
                btnPrint1.Enabled = true;
                btnPrint.Enabled = true;
                pan0.Visible = true;
                pan1.Visible = false;
                pan2.Visible = false;
                lblTitle.Text = "BRANDEN SCALE(욕창사정도구표)";
            }
            else if (rdoOpt1.Checked == true)
            {
                btnSearch.Enabled = true;
                btnPrint1.Enabled = false;
                btnPrint.Enabled = false;
                pan0.Visible = false;
                pan1.Visible = true;
                pan2.Visible = false;
                lblTitle.Text = "소아 욕창 사정 도구표(Modified Braden Q Scale)";
            }
            else if (rdoOpt2.Checked == true)
            {
                btnSearch.Enabled = true;
                btnPrint1.Enabled = false;
                btnPrint.Enabled = false;
                pan0.Visible = false;
                pan1.Visible = false;
                pan2.Visible = true;
                lblTitle.Text = "신생아 욕창 사정 도구표(Neonatal/ infant Braden Q Scale)";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (rdoOpt0.Checked == true)
            {
                return;
            }
            else if (rdoOpt1.Checked == true)
            {
                Data_Read2();
            }
            else if (rdoOpt2.Checked == true)
            {
                Data_Read3();
            }
        }

        void Data_Read2()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            double nIPDNO = 0;

            string strSysDate = "";

            ComFunc cf = new ComFunc();

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            Screen_Clear();

            ssInfo_Sheet1.Cells[0, 1].Text = FstrSname;
            ssInfo_Sheet1.Cells[0, 3].Text = FstrPano;
            ssInfo_Sheet1.Cells[0, 5].Text = FstrSex + "/" + FstrAge;
            ssInfo_Sheet1.Cells[0, 7].Text = FstrRoom;

            ssInfo_Sheet1.Cells[1, 1].Text = FstrDeptCode;
            ssInfo_Sheet1.Cells[1, 3].Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);
            ssInfo_Sheet1.Cells[1, 5].Text = strSysDate;

            nIPDNO = VB.Val(FstrIpdno);

            if ((FstrSname + FstrPano + FstrSex + FstrAge + FstrRoom + FstrDeptCode + FstrIpdno).Trim() == "")
            {
                btnSave.Enabled = false;
                return;
            }

            ssView2_Sheet1.ColumnHeader.Cells[0, 3].Text = VB.Mid(strSysDate, 6, 2) + "/" + VB.Right(strSysDate, 2);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT JUMSU1, JUMSU2, JUMSU3, JUMSU4, ";
                SQL = SQL + ComNum.VBLF + " JUMSU5, JUMSU6, JUMSU7, TOTAL, ";
                SQL = SQL + ComNum.VBLF + " ENTSABUN, TO_CHAR(ACTDATE, 'MM/DD') ACTDATE, ROWID, ACTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_CHILD ";
                if (gsWard == "ER" || gsWard == "HD")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPano + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND IPDNO = 0 ";
                    if (chkJ.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE <= TO_DATE('" + dtpJDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE >= TO_DATE('" + dtpJDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE <= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strSysDate, 1) + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE >= TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + nIPDNO;
                }
                SQL = SQL + ComNum.VBLF + "     AND ENTDATE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC, ACTTIME DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i >= 3)
                        {
                            break;
                        }

                        ssView2_Sheet1.Cells[0, i + 5].Text = dt.Rows[i]["ACTDATE"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["ACTTIME"].ToString().Trim();
                        ssView2_Sheet1.Cells[2, i + 5].Text = dt.Rows[i]["JUMSU1"].ToString().Trim();
                        ssView2_Sheet1.Cells[7, i + 5].Text = dt.Rows[i]["JUMSU2"].ToString().Trim();
                        ssView2_Sheet1.Cells[12, i + 5].Text = dt.Rows[i]["JUMSU3"].ToString().Trim();
                        ssView2_Sheet1.Cells[17, i + 5].Text = dt.Rows[i]["JUMSU4"].ToString().Trim();
                        ssView2_Sheet1.Cells[22, i + 5].Text = dt.Rows[i]["JUMSU5"].ToString().Trim();
                        ssView2_Sheet1.Cells[27, i + 5].Text = dt.Rows[i]["JUMSU6"].ToString().Trim();
                        ssView2_Sheet1.Cells[32, i + 5].Text = dt.Rows[i]["JUMSU7"].ToString().Trim();
                        ssView2_Sheet1.Cells[36, i + 5].Text = dt.Rows[i]["TOTAL"].ToString().Trim();
                        ssView2_Sheet1.Cells[37, i + 5].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());
                        ssView2_Sheet1.Cells[38, i + 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            Read_Warning("1", "", nIPDNO.ToString());
            Read_Eval("1", "", nIPDNO.ToString());
            Read_History_List(nIPDNO.ToString(), "");
        }

        void Data_Read3()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            double nIPDNO = 0;

            string strSysDate = "";

            ComFunc cf = new ComFunc();

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            Screen_Clear();

            ssInfo_Sheet1.Cells[0, 1].Text = FstrSname;
            ssInfo_Sheet1.Cells[0, 3].Text = FstrPano;
            ssInfo_Sheet1.Cells[0, 5].Text = FstrSex + "/" + FstrAge;
            ssInfo_Sheet1.Cells[0, 7].Text = FstrRoom;

            ssInfo_Sheet1.Cells[1, 1].Text = FstrDeptCode;
            ssInfo_Sheet1.Cells[1, 3].Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);
            ssInfo_Sheet1.Cells[1, 5].Text = strSysDate;

            nIPDNO = VB.Val(FstrIpdno);

            if ((FstrSname + FstrPano + FstrSex + FstrAge + FstrRoom + FstrDeptCode + FstrIpdno).Trim() == "")
            {
                btnSave.Enabled = false;
                return;
            }

            ssView2_Sheet1.ColumnHeader.Cells[0, 3].Text = VB.Mid(strSysDate, 6, 2) + "/" + VB.Right(strSysDate, 2);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT JUMSU1, JUMSU2, JUMSU3, JUMSU4, ";
                SQL = SQL + ComNum.VBLF + " JUMSU5, JUMSU6, JUMSU7, JUMSU8, TOTAL, ";
                SQL = SQL + ComNum.VBLF + " ENTSABUN, TO_CHAR(ACTDATE, 'MM/DD') ACTDATE, ROWID, ACTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_BABY ";
                if (gsWard == "HD" || gsWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "   WHERE PANO = '" + FstrPano + "' ";
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = 0 ";

                    if (chkJ.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "       AND ACTDATE >= TO_DATE('" + dtpJDate.Value.ToString("yyyy-MM-dd") + "') ";
                        SQL = SQL + ComNum.VBLF + "       AND ACTDATE <= TO_DATE('" + dtpJDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "       AND ACTDATE >= TO_DATE('" + strSysDate + "') ";
                        SQL = SQL + ComNum.VBLF + "       AND ACTDATE <= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strSysDate, 1) + "') ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + nIPDNO;
                }
                SQL = SQL + ComNum.VBLF + "     AND ENTDATE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_BABY.ACTDATE DESC, ACTTIME DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i > 4)
                        {
                            break;
                        }

                        ssView3_Sheet1.Cells[0, i + 5].Text = dt.Rows[i]["ACTDATE"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["ACTTIME"].ToString().Trim();
                        ssView3_Sheet1.Cells[2, i + 5].Text = dt.Rows[i]["JUMSU1"].ToString().Trim();
                        ssView3_Sheet1.Cells[7, i + 5].Text = dt.Rows[i]["JUMSU2"].ToString().Trim();
                        ssView3_Sheet1.Cells[12, i + 5].Text = dt.Rows[i]["JUMSU3"].ToString().Trim();
                        ssView3_Sheet1.Cells[17, i + 5].Text = dt.Rows[i]["JUMSU4"].ToString().Trim();
                        ssView3_Sheet1.Cells[22, i + 5].Text = dt.Rows[i]["JUMSU5"].ToString().Trim();
                        ssView3_Sheet1.Cells[27, i + 5].Text = dt.Rows[i]["JUMSU6"].ToString().Trim();
                        ssView3_Sheet1.Cells[32, i + 5].Text = dt.Rows[i]["JUMSU7"].ToString().Trim();
                        ssView3_Sheet1.Cells[37, i + 5].Text = dt.Rows[i]["JUMSU8"].ToString().Trim();
                        ssView3_Sheet1.Cells[41, i + 5].Text = dt.Rows[i]["TOTAL"].ToString().Trim();
                        ssView3_Sheet1.Cells[42, i + 5].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());
                        ssView3_Sheet1.Cells[43, i + 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            Read_Warning("1", "", nIPDNO.ToString());
            Read_Eval("1", "", nIPDNO.ToString());
            Read_History_List(nIPDNO.ToString(), "");
        }

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            int nTOT = 0;
            int nValue = 0;

            string strROWID = "";

            if (e.Row == 0)
            {
                if (e.Column < 5)
                {
                    return;
                }

                strROWID = ssView2_Sheet1.Cells[38, e.Column].Text.Trim();

                if (strROWID != "")
                {
                    Delete_Bedsore(strROWID);
                }

                return;
            }

            if (e.Column == 2 || e.Column == 3)
            {
                nValue = Convert.ToInt32(VB.Val(ssView2_Sheet1.Cells[e.Row, 1].Text));

                if (e.Row >= 2 && e.Row <= 5)
                {
                    ssView2_Sheet1.Cells[2, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 7 && e.Row <= 10)
                {
                    ssView2_Sheet1.Cells[7, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 12 && e.Row <= 15)
                {
                    ssView2_Sheet1.Cells[12, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 17 && e.Row <= 20)
                {
                    ssView2_Sheet1.Cells[17, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 22 && e.Row <= 25)
                {
                    ssView2_Sheet1.Cells[22, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 27 && e.Row <= 30)
                {
                    ssView2_Sheet1.Cells[27, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 32 && e.Row <= 35)
                {
                    ssView2_Sheet1.Cells[32, 4].Text = nValue.ToString();
                }

                for (i = 1; i <= 35; i++)
                {
                    nTOT = nTOT + Convert.ToInt32(VB.Val(ssView2_Sheet1.Cells[i, 4].Text));
                }

                ssView2_Sheet1.Cells[36, 4].Text = nTOT.ToString();
            }
        }

        private void ssView3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            int nTOT = 0;
            int nValue = 0;

            string strROWID = "";

            if (e.Row == 0)
            {
                if (e.Column < 5)
                {
                    return;
                }

                strROWID = ssView3_Sheet1.Cells[43, e.Column].Text.Trim();

                if (strROWID != "")
                {
                    Delete_Bedsore(strROWID);
                }

                return;
            }

            if (e.Column == 2 || e.Column == 3)
            {
                nValue = Convert.ToInt32(VB.Val(ssView3_Sheet1.Cells[e.Row, 1].Text));

                if (e.Row >= 2 && e.Row <= 5)
                {
                    ssView3_Sheet1.Cells[2, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 7 && e.Row <= 10)
                {
                    ssView3_Sheet1.Cells[7, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 12 && e.Row <= 15)
                {
                    ssView3_Sheet1.Cells[12, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 17 && e.Row <= 20)
                {
                    ssView3_Sheet1.Cells[17, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 22 && e.Row <= 25)
                {
                    ssView3_Sheet1.Cells[22, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 27 && e.Row <= 30)
                {
                    ssView3_Sheet1.Cells[27, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 32 && e.Row <= 35)
                {
                    ssView3_Sheet1.Cells[32, 4].Text = nValue.ToString();
                }
                else if (e.Row >= 37 && e.Row <= 40)
                {
                    ssView3_Sheet1.Cells[37, 4].Text = nValue.ToString();
                }

                for (i = 1; i <= 40; i++)
                {
                    nTOT = nTOT + Convert.ToInt32(VB.Val(ssView3_Sheet1.Cells[i, 4].Text));
                }

                ssView3_Sheet1.Cells[41, 4].Text = nTOT.ToString();
            }
        }

        private bool CREATE_EMR_XMLINSRT3(double EmrNo, string FormNo, string strSabun, string strChartDate, string strChartTime, int iAcpNo,
            string strPtNo, string strInOutCls, string strMedFrDate, string strMedFrTime, string strMedEndDate, string strMedEndTime,
            string strMedDeptCd, string strMedDrCd, string strMibiCheck, int iUpdateNo, string strXML)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strWRITEDATE = "";
            string strWRITETIME = "";

            //차팅일자
            if (strChartDate != "" && strChartDate.IndexOf("-") != -1)
            {
                strChartDate = Convert.ToDateTime(strChartDate).ToString("yyyyMMdd");
            }
            if (strChartTime != "" && strChartTime.IndexOf(":") != -1)
            {
                strChartTime = Convert.ToDateTime(strChartTime).ToString("HHmmss");
            }
            //입실일자
            if (strMedFrDate != "" && strMedFrDate.IndexOf("-") != -1)
            {
                strMedFrDate = Convert.ToDateTime(strMedFrDate).ToString("yyyyMMdd");
            }
            if (strMedFrTime != "" && strMedFrTime.IndexOf(":") != -1)
            {
                strMedFrTime = Convert.ToDateTime(strMedFrTime).ToString("HHmmss");
            }
            //퇴실일자
            if (strMedEndDate != "" && strMedEndDate.IndexOf("-") != -1)
            {
                strMedEndDate = Convert.ToDateTime(strMedEndDate).ToString("yyyyMMdd");
            }
            if (strMedEndTime != "" && strMedEndTime.IndexOf(":") != -1)
            {
                strMedEndTime = Convert.ToDateTime(strMedEndTime).ToString("HHmmss");
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYYMMDD') AS CURRENTDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(SYSDATE,'HH24MISS') AS CURRENTTIME ";
                SQL = SQL + ComNum.VBLF + "    FROM DUAL ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strWRITEDATE = dt.Rows[0]["CURRENTDATE"].ToString().Trim();
                    strWRITETIME = dt.Rows[0]["CURRENTTIME"].ToString().Trim();
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

            int Result = 0;
            OracleCommand cmd = new OracleCommand();
            PsmhDb pDbCon = clsDB.DbCon;

            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_EMR.XMLINSRT3";
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("p_EMRNO", OracleDbType.Double, 0, EmrNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, VB.Val(FormNo), ParameterDirection.Input);
                cmd.Parameters.Add("p_USEID", OracleDbType.Varchar2, 8, strSabun, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTDATE", OracleDbType.Varchar2, 8, strChartDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTTIME", OracleDbType.Varchar2, 6, strChartTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_ACPNO", OracleDbType.Double, 0, iAcpNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_PTNO", OracleDbType.Varchar2, 9, strPtNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_INOUTCLS", OracleDbType.Varchar2, 1, strInOutCls, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRDATE", OracleDbType.Varchar2, 8, strMedFrDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRTIME", OracleDbType.Varchar2, 6, strMedFrTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDDATE", OracleDbType.Varchar2, 8, strMedEndDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDTIME", OracleDbType.Varchar2, 6, strMedEndTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDEPTCD", OracleDbType.Varchar2, 4, strMedDeptCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDRCD", OracleDbType.Varchar2, 6, strMedDrCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MIBICHECK", OracleDbType.Varchar2, 1, "0", ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITEDATE", OracleDbType.Varchar2, 8, strWRITEDATE, ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITETIME", OracleDbType.Varchar2, 6, strWRITETIME, ParameterDirection.Input);
                cmd.Parameters.Add("p_UPDATENO", OracleDbType.Int32, 0, iUpdateNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTXML", OracleDbType.Clob, VB.Len(strXML), strXML, ParameterDirection.Input);

                Result = cmd.ExecuteNonQuery();

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception e)
            {
                return rtnVal;
            }
        }
    }
}
