using ComBase;
using ComDbB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Description : 예약부도자 자료형성
/// Author : 박병규
/// Create Date : 2017.12.15
/// </summary>
/// <history>
/// </history>
/// <seealso cref="FrmReservedFailedBuild.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaBuildResFail : Form
    {
        clsSpread CS = null;
        clsPmpaQuery CPQ = null;

        DataTable Dt = new DataTable();
        DataTable DtSub = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        public frmPmpaBuildResFail()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
                Read_Data(clsDB.DbCon);
            else if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnExit)
                this.Close();

        }

        private void Save_Process(PsmhDb pDbCon)
        {
            string strCommit = "";
            string strTable = "";
            string strRowID_R = "";
            string strCardSeqno = "";
            string strJiwon = "";
            string strGubun = "";

            string[] Data = new string[28];

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            ComFunc.ReadSysDate(pDbCon);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strCommit = "OK";

                    Array.Clear(Data, 0, Data.Length);

                    if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) != true)
                    {
                        Data[0] = clsPublic.GstrSysDate; //예약부도 정리일자
                        Data[1] = ssList_Sheet1.Cells[i, 2].Text.ToString().Trim(); //등록번호
                        Data[2] = ssList_Sheet1.Cells[i, 3].Text.ToString().Trim(); //진료과
                        Data[3] = ssList_Sheet1.Cells[i, 4].Text.ToString().Trim(); //의사코드
                        Data[4] = ssList_Sheet1.Cells[i, 5].Text.ToString().Trim(); //초재구분
                        Data[5] = ssList_Sheet1.Cells[i, 6].Text.ToString().Trim(); //보험유형
                        Data[6] = ssList_Sheet1.Cells[i, 7].Text.ToString().Trim(); //환자성명
                        Data[7] = ssList_Sheet1.Cells[i, 8].Text.ToString().Trim(); //성별
                        Data[8] = ssList_Sheet1.Cells[i, 9].Text.ToString().Trim(); //나이
                        Data[9] = ssList_Sheet1.Cells[i, 10].Text.ToString().Trim(); //감액코드
                        Data[10] = ssList_Sheet1.Cells[i, 11].Text.ToString().Trim(); //특진여부
                        Data[11] = ssList_Sheet1.Cells[i, 12].Text.ToString().Trim(); //수납구분
                        Data[12] = ssList_Sheet1.Cells[i, 13].Text.ToString().Trim(); //보훈여부
                        Data[13] = ssList_Sheet1.Cells[i, 14].Text.ToString().Trim(); //예약접수일자
                        Data[14] = ssList_Sheet1.Cells[i, 15].Text.ToString().Trim(); //예약변경일자
                        Data[15] = ssList_Sheet1.Cells[i, 16].Text.ToString().Trim(); //예약최종일자
                        Data[16] = ssList_Sheet1.Cells[i, 17].Text.ToString().Trim(); //진찰료 발생금액
                        Data[17] = ssList_Sheet1.Cells[i, 18].Text.ToString().Trim(); //진찰료 특진료
                        Data[18] = ssList_Sheet1.Cells[i, 19].Text.ToString().Trim(); //진찰료 총액
                        Data[19] = ssList_Sheet1.Cells[i, 20].Text.ToString().Trim(); //진찰료 조합부담
                        Data[20] = ssList_Sheet1.Cells[i, 21].Text.ToString().Trim(); //진찰료 감액
                        Data[21] = ssList_Sheet1.Cells[i, 22].Text.ToString().Trim(); //진찰료 미수
                        Data[22] = ssList_Sheet1.Cells[i, 23].Text.ToString().Trim(); //진찰료 영수
                        Data[23] = ssList_Sheet1.Cells[i, 24].Text.ToString().Trim(); //계약처코드
                        Data[24] = ssList_Sheet1.Cells[i, 26].Text.ToString().Trim(); //rowid

                        strTable = ssList_Sheet1.Cells[i, 27].Text.ToString().Trim(); 
                        strRowID_R = ssList_Sheet1.Cells[i, 28].Text.ToString().Trim();
                        strCardSeqno = ssList_Sheet1.Cells[i, 29].Text.ToString().Trim();
                        strJiwon = ssList_Sheet1.Cells[i, 30].Text.ToString().Trim();
                        strGubun = ssList_Sheet1.Cells[i, 31].Text.ToString().Trim();//환불구분(00:예약비, 01:접수비)

                        Data[25] = Data[22]; //예약금보관 금액
                        Data[26] = clsType.User.IdNumber; //예약금보관 작업자사번

                        if (strGubun == "00")
                        {
                            Data[26] = "555"; //예약금보관 작업자 사번
                            Data[27] = "555"; //예약금보관 처리자 작업조
                        }
                        else
                        {
                            Data[26] = "666"; //접수비보관 작업자 사번
                            Data[27] = "666"; //접수비보관 처리자 작업조
                        }
                        
                        //이미 부도처리 되었는지 점검함
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT Pano, DeptCode ";
                        SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_REFUND ";
                        SQL += ComNum.VBLF + "  Where 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND ActDate   = TO_DATE('" + Data[0] + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND Pano      = '" + Data[1] + "' ";
                        SQL += ComNum.VBLF + "    AND DeptCode  = '" + Data[2].Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND Gubun     = '" + strGubun + "' ";
                        SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            Dt.Dispose();
                            Dt = null;
                            return;
                        }

                        if (Dt.Rows.Count > 0)
                        {
                            clsPublic.GstrMsgTitle = "부도자형성 점검";
                            clsPublic.GstrMsgList = "기 생성된 부도자료가 있음." + '\r';
                            clsPublic.GstrMsgList += "등록번호 : " + Dt.Rows[0]["PANO"].ToString().Trim() + '\r';
                            clsPublic.GstrMsgList += "진료과목 : " + Dt.Rows[0]["DEPTCODE"].ToString().Trim() + '\r';
                            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                            strCommit = "NO";
                        }

                        Dt.Dispose();
                        Dt = null;

                        //이미 예약변경으로 외래접수가 취소 되었는지 점검함 dtpDate.Text
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT ROWID From " + ComNum.DB_PMPA + "OPD_MASTER ";
                        SQL += ComNum.VBLF + "  Where 1         = 1 ";
                        //SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + Data[0] + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND Pano      = '" + Data[1] + "' ";
                        SQL += ComNum.VBLF + "    AND DeptCode  = '" + Data[2].Trim() + "' ";
                        SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            Dt.Dispose();
                            Dt = null;
                            return;
                        }

                        if (Dt.Rows.Count == 0)
                        {
                            clsPublic.GstrMsgTitle = "부도자형성 점검";
                            clsPublic.GstrMsgList = "예약변경으로 외래접수가 없음.." + '\r';
                            clsPublic.GstrMsgList += "외래에서 예약변경한 대상자" + '\r';
                            clsPublic.GstrMsgList += "등록번호 : " + Data[1] + '\r';
                            clsPublic.GstrMsgList += "진료과목 : " + Data[2] + '\r';
                            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                            strCommit = "NO";
                        }

                        Dt.Dispose();
                        Dt = null;


                        if (strCommit == "OK")
                        {
                            clsDB.setBeginTran(pDbCon);

                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_REFUND ";
                            SQL += ComNum.VBLF + "        (ACTDATE, PANO, DEPTCODE, ";
                            SQL += ComNum.VBLF + "         DRCODE, CHOJAE, BI, ";
                            SQL += ComNum.VBLF + "         SNAME, SEX, AGE, ";
                            SQL += ComNum.VBLF + "         GBGAMEK, GBSPC, JIN, ";
                            SQL += ComNum.VBLF + "         BOHUN, YDATE1, YDATE2, ";
                            SQL += ComNum.VBLF + "         YDATE3, AMT1, AMT2, ";
                            SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                            SQL += ComNum.VBLF + "         AMT6, AMT7, GELCODE, ";
                            SQL += ComNum.VBLF + "         CAMT, CSABUN, CPART, ";
                            SQL += ComNum.VBLF + "         CARDSEQNO, EntDate, JiWon, ";
                            SQL += ComNum.VBLF + "         Gubun) ";
                            SQL += ComNum.VBLF + " VALUES (TO_DATE('" + Data[0] + "','YYYY-MM-DD') , ";
                            SQL += ComNum.VBLF + "         '" + Data[1] + "' , "; //등록번호
                            SQL += ComNum.VBLF + "         '" + Data[2] + "', "; //진료과
                            SQL += ComNum.VBLF + "         '" + Data[3] + "', "; //의사코드
                            SQL += ComNum.VBLF + "         '" + Data[4] + "', "; //초재구분
                            SQL += ComNum.VBLF + "         '" + Data[5] + "', "; //보험종류
                            SQL += ComNum.VBLF + "         '" + Data[6] + "', "; //환자명
                            SQL += ComNum.VBLF + "         '" + Data[7] + "', "; //성별
                            SQL += ComNum.VBLF + "         '" + Data[8] + "', "; //나이
                            SQL += ComNum.VBLF + "         '" + Data[9] + "', "; //감액구분
                            SQL += ComNum.VBLF + "         '" + Data[10] + "', "; //특진구분
                            SQL += ComNum.VBLF + "         '" + Data[11] + "', "; //수납구분
                            SQL += ComNum.VBLF + "         '" + Data[12] + "',"; //보훈여부
                            SQL += ComNum.VBLF + "         TO_DATE('" + Data[13] + "','YYYY-MM-DD HH24:MI'), "; //예약접수일자
                            SQL += ComNum.VBLF + "         TO_DATE('" + Data[14] + "','YYYY-MM-DD HH24:MI'), "; //예약변경일자
                            SQL += ComNum.VBLF + "         TO_DATE('" + Data[15] + "','YYYY-MM-DD HH24:MI'), "; //예약최종일자
                            SQL += ComNum.VBLF + "         '" + Data[16] + "', "; //진찰료발생금액
                            SQL += ComNum.VBLF + "         '" + Data[17] + "', "; //진찰료특진료
                            SQL += ComNum.VBLF + "         '" + Data[18] + "', "; //진찰료총액
                            SQL += ComNum.VBLF + "         '" + Data[19] + "', "; //진찰료조합부담
                            SQL += ComNum.VBLF + "         '" + Data[20] + "', "; //진찰료감액
                            SQL += ComNum.VBLF + "         '" + Data[21] + "', "; //진찰료미수
                            SQL += ComNum.VBLF + "         '" + Data[22] + "', "; //진찰료영수
                            SQL += ComNum.VBLF + "         '" + Data[23] + "', "; //계약처코드
                            SQL += ComNum.VBLF + "         '" + Data[25] + "', "; //예약금보관금액
                            SQL += ComNum.VBLF + "         '" + Data[26] + "', "; //예약금/접수비 보관 작업자 사번
                            SQL += ComNum.VBLF + "         '" + Data[27] + "', "; //예약금/접수비 보관 처리자 작업조
                            SQL += ComNum.VBLF + "         '" + strCardSeqno.Trim() + "', ";
                            SQL += ComNum.VBLF + "         SYSDATE, ";
                            SQL += ComNum.VBLF + "         '" + strJiwon + "', ";
                            SQL += ComNum.VBLF + "         '" + strGubun + "') ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            if (strGubun == "00")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_RESV_DEL ";
                                SQL += ComNum.VBLF + "        (ACTDATE, PANO, DEPTCODE, ";
                                SQL += ComNum.VBLF + "         BI, SNAME, DRCODE, ";
                                SQL += ComNum.VBLF + "         DATE1, DATE2, DATE3, ";
                                SQL += ComNum.VBLF + "         CHOJAE, GBGAMEK, GBSPC, ";
                                SQL += ComNum.VBLF + "         JIN, AMT1, AMT2, ";
                                SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                                SQL += ComNum.VBLF + "         AMT6, AMT7, PART, ";
                                SQL += ComNum.VBLF + "         BOHUN, GELCODE, RETAMT, ";
                                SQL += ComNum.VBLF + "         CARDSEQNO, Jiwon) ";
                                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + Data[0] + "','YYYY-MM-DD'), ";
                                SQL += ComNum.VBLF + "         '" + Data[1] + "', "; //등록번호
                                SQL += ComNum.VBLF + "         '" + Data[2] + "', "; //진료과목
                                SQL += ComNum.VBLF + "         '" + Data[5] + "', "; //보험종류
                                SQL += ComNum.VBLF + "         '" + Data[6] + "', "; //환자명
                                SQL += ComNum.VBLF + "         '" + Data[3] + "', "; //의사코드
                                SQL += ComNum.VBLF + "         TO_DATE('" + Data[13] + "','YYYY-MM-DD HH24:MI'), "; //예약접수일자
                                SQL += ComNum.VBLF + "         TO_DATE('" + Data[14] + "','YYYY-MM-DD HH24:MI'), "; //예약변경일자
                                SQL += ComNum.VBLF + "         TO_DATE('" + Data[15] + "','YYYY-MM-DD HH24:MI'), "; //예약최종일자
                                SQL += ComNum.VBLF + "         '" + Data[4] + "', "; //초재구분
                                SQL += ComNum.VBLF + "         '" + Data[9] + "', "; //감액구분
                                SQL += ComNum.VBLF + "         '" + Data[10] + "', "; //특진구분
                                SQL += ComNum.VBLF + "         '" + Data[11] + "', "; //수납구분
                                SQL += ComNum.VBLF + "         '" + Data[16] + "', "; //진찰료발생금액
                                SQL += ComNum.VBLF + "         '" + Data[17] + "', "; //진찰료특진료
                                SQL += ComNum.VBLF + "         '" + Data[18] + "', "; //진찰료총액
                                SQL += ComNum.VBLF + "         '" + Data[19] + "', "; //진찰료조합부담
                                SQL += ComNum.VBLF + "         '" + Data[20] + "', "; //진찰료감액
                                SQL += ComNum.VBLF + "         '" + Data[21] + "', "; //진찰료미수
                                SQL += ComNum.VBLF + "         '" + Data[22] + "', "; //진찰료영수
                                SQL += ComNum.VBLF + "         '" + Data[27] + "', "; ////예약금/접수비 보관 처리자 작업조
                                SQL += ComNum.VBLF + "         '" + Data[12] + "', "; //보훈여부
                                SQL += ComNum.VBLF + "         '" + Data[23] + "', "; //계약처코드
                                SQL += ComNum.VBLF + "         '" + (Convert.ToInt64(VB.Val(Data[25])) * -1) + "', ";
                                SQL += ComNum.VBLF + "         '" + strCardSeqno.Trim() + "', ";
                                SQL += ComNum.VBLF + "         '" + strJiwon + "') ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr + "OPD_RESV_DEL 테이블에 자료 추가시 에러 발생함.");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                SQL = "";
                                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                                SQL += ComNum.VBLF + "    SET RETDATE   = SYSDATE , ";
                                SQL += ComNum.VBLF + "        RETAMT    = '" + (Convert.ToInt64(VB.Val(Data[25])) * -1) + "', ";
                                SQL += ComNum.VBLF + "        RETPART   = '" + Data[27] + "' ";
                                SQL += ComNum.VBLF + "  WHERE ROWID     = '" + strRowID_R + "'";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr + "OPD_RESERVED_NEW 테이블에 자료 수정시 에러 발생함.");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }


                            if (Data[24] != "")
                            {
                                SQL = "";
                                if (strTable == "M")
                                    SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "OPD_MASTER WHERE ROWID = '" + Data[24] + "'";
                                else
                                    SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "OPD_RESERVED WHERE ROWID = '" + Data[24] + "'";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }

                            clsDB.setCommitTran(pDbCon);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

           // CPQ.SMS_YEYAK_BUDO(pDbCon, clsPublic.GstrSysDate);
            ComFunc.MsgBox("자료형성이 완료 되었습니다.", "알림");
            Cursor.Current = Cursors.Default;

            btnSave.Enabled = false;
        }

        private void Read_Data(PsmhDb pDbCon)
        {
            string strDate = dtpDate.Text;
            string strSuCode = "$$11";

            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            ComFunc.ReadSysDate(pDbCon);


            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.PANO, A.SNAME, A.ACTDATE, ";
            SQL += ComNum.VBLF + "        A.DEPTCODE, COUNT(*) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_OORDER B ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND A.ACTDATE     = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.RESERVED    = '1' ";
            SQL += ComNum.VBLF + "    AND B.ORDERCODE   = '" + strSuCode + "' ";
            SQL += ComNum.VBLF + "    AND A.PANO        = B.PTNO ";
            SQL += ComNum.VBLF + "    AND A.ACTDATE     = B.BDATE ";
            SQL += ComNum.VBLF + "    AND A.DEPTCODE    = B.DEPTCODE ";
            SQL += ComNum.VBLF + "  GROUP BY A.PANO,A.SNAME,A.ACTDATE,A.DEPTCODE ";
            SQL += ComNum.VBLF + "  HAVING COUNT(*) > 1 ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                //clsPublic.GstrMsgList = strSuCode + " 처방이 2건이상 발생하였습니다." + '\r' + '\r';

                //for (int i = 0; i < Dt.Rows.Count; i++)
                //{
                //    clsPublic.GstrMsgList += Dt.Rows[i]["DEPTCODE"].ToString().Trim() + ". ";
                //    clsPublic.GstrMsgList += Dt.Rows[i]["PANO"].ToString().Trim() + " ";
                //    clsPublic.GstrMsgList += Dt.Rows[i]["SNAME"].ToString().Trim() + '\r';
                //}

                //clsPublic.GstrMsgTitle = "알림";
                //clsPublic.GMsgButtons = MessageBoxButtons.OK;
                //clsPublic.GMsgIcon = MessageBoxIcon.Warning;

                //MessageBox.Show(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, clsPublic.GMsgButtons, clsPublic.GMsgIcon);

                //Dt.Dispose();
                //Dt = null;
                //return;
            }

            Dt.Dispose();
            Dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DISTINCT A.PANO, A.DEPTCODE, A.DRCODE, ";
            SQL += ComNum.VBLF + "        A.CHOJAE, A.BI, A.SNAME, ";
            SQL += ComNum.VBLF + "        A.SEX, A.AGE, A.GBGAMEK, ";
            SQL += ComNum.VBLF + "        A.GBSPC, A.JIN, A.BOHUN, ";
            SQL += ComNum.VBLF + "        A.AMT1, A.AMT2, A.AMT3, ";
            SQL += ComNum.VBLF + "        A.AMT4, A.AMT5, A.AMT6, ";
            SQL += ComNum.VBLF + "        A.REP, A.AMT7, A.GELCODE, ";
            SQL += ComNum.VBLF + "        A.CARDSEQNO, A.ROWID, 'M' TAB, ";
            SQL += ComNum.VBLF + "        A.JIWON ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_OORDER B ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND A.ACTDATE     = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.RESERVED    = '1' "; //예약일때만
            SQL += ComNum.VBLF + "    AND A.REP  NOT IN ('#') "; //당일 입원자는 부도처리 않함
            SQL += ComNum.VBLF + "    AND A.PANO        = B.PTNO ";
            SQL += ComNum.VBLF + "    AND A.ACTDATE     = B.BDATE ";
            SQL += ComNum.VBLF + "    AND A.DEPTCODE    = B.DEPTCODE ";
            SQL += ComNum.VBLF + "    AND B.ORDERCODE   = '" + strSuCode + "' ";
            SQL += ComNum.VBLF + "  ORDER BY DEPTCODE, PANO ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("대상자가 없습니다.", "알림");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string strCheck = "0";
                string strStatus = "";
                string strYtime1 = "";
                string strYtime2 = "";
                string strYtime3 = "";
                string strRowID = "";

                //build 대상 상태 check
                SQL = "";
                SQL += ComNum.VBLF + " SELECT NVL(SUM(AMT1), 0) NAMT1 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND PANO      = '" + Dt.Rows[i]["PANO"].ToString() + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + Dt.Rows[i]["DEPTCODE"].ToString() + "' ";
                SQL += ComNum.VBLF + "    AND WARDCODE  <> 'II' ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    return;
                }

                if (DtSub.Rows.Count > 0)
                {
                    if (Convert.ToInt32(DtSub.Rows[0]["NAMT1"].ToString()) != 0)
                    {
                        strCheck = "1";
                        strStatus = "외래처방발생 확인";
                    }
                }

                DtSub.Dispose();
                DtSub = null;


                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(DATE1,'YYYY-MM-DD HH24:MI') DATE1, ";
                SQL += ComNum.VBLF + "        TO_CHAR(DATE2,'YYYY-MM-DD HH24:MI') DATE2, ";
                SQL += ComNum.VBLF + "        TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') DATE3, ";
                SQL += ComNum.VBLF + "        ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND DATE3     >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DATE3     <  TO_DATE('" + VB.DateAdd("D", 1, strDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + Dt.Rows[i]["PANO"].ToString() + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + Dt.Rows[i]["DEPTCODE"].ToString() + "' ";
                SQL += ComNum.VBLF + "    AND RETDATE IS NULL ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    return;
                }

                if (DtSub.Rows.Count == 0)
                {
                    if (strCheck == "0")
                    {
                        strStatus = "전산실 연락 요망(예약시간 확인 않됨)";
                        strYtime1 = clsPublic.GstrSysDate;
                        strYtime2 = clsPublic.GstrSysDate;
                        strYtime3 = clsPublic.GstrSysDate;
                    }
                }
                else
                {
                    strYtime1 = DtSub.Rows[0]["DATE1"].ToString();
                    strYtime2 = DtSub.Rows[0]["DATE2"].ToString();
                    strYtime3 = DtSub.Rows[0]["DATE3"].ToString();
                    strRowID = DtSub.Rows[0]["ROWID"].ToString();
                }

                DtSub.Dispose();
                DtSub = null;


                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_REFUND ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND PANO      = '" + Dt.Rows[i]["PANO"].ToString() + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + Dt.Rows[i]["DEPTCODE"].ToString() + "' ";
                SQL += ComNum.VBLF + "    AND GUBUN     = '00' ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    return;
                }

                if (DtSub.Rows.Count > 0)
                {
                    strCheck = "1";
                    strStatus = "이미 빌드됨.";
                }

                DtSub.Dispose();
                DtSub = null;

                if (Dt.Rows[i]["REP"].ToString() == "#") //당일 입원자는 부도처리 않함
                {
                    strCheck = "1";
                    strStatus = "당일 입원자임 외래 진료과에 확인 요망 !!";
                }

                if (strCheck == "1")
                {
                    ssList_Sheet1.Cells[i, 0].Locked = true;
                    ssList_Sheet1.Cells[i, 0].Value = true;
                }
                else
                    ssList_Sheet1.Cells[i, 0].Value = false;

                ssList_Sheet1.Cells[i, 1].Text = strStatus;
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DRCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["CHOJAE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["BI"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["SEX"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["AGE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["GBGAMEK"].ToString().Trim();
                ssList_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["GBSPC"].ToString().Trim();
                ssList_Sheet1.Cells[i, 12].Text = Dt.Rows[i]["JIN"].ToString().Trim();
                ssList_Sheet1.Cells[i, 13].Text = Dt.Rows[i]["BOHUN"].ToString().Trim();
                ssList_Sheet1.Cells[i, 14].Text = strYtime1;
                ssList_Sheet1.Cells[i, 15].Text = strYtime2;
                ssList_Sheet1.Cells[i, 16].Text = strYtime3;
                ssList_Sheet1.Cells[i, 17].Text = Dt.Rows[i]["AMT1"].ToString().Trim();
                ssList_Sheet1.Cells[i, 18].Text = Dt.Rows[i]["AMT2"].ToString().Trim();
                ssList_Sheet1.Cells[i, 19].Text = Dt.Rows[i]["AMT3"].ToString().Trim();
                ssList_Sheet1.Cells[i, 20].Text = Dt.Rows[i]["AMT4"].ToString().Trim();
                ssList_Sheet1.Cells[i, 21].Text = Dt.Rows[i]["AMT5"].ToString().Trim();
                ssList_Sheet1.Cells[i, 22].Text = Dt.Rows[i]["AMT6"].ToString().Trim();
                ssList_Sheet1.Cells[i, 23].Text = Dt.Rows[i]["AMT7"].ToString().Trim();
                ssList_Sheet1.Cells[i, 24].Text = Dt.Rows[i]["GELCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 26].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                ssList_Sheet1.Cells[i, 27].Text = Dt.Rows[i]["TAB"].ToString().Trim();
                ssList_Sheet1.Cells[i, 28].Text = strRowID;
                ssList_Sheet1.Cells[i, 29].Text = Dt.Rows[i]["CARDSEQNO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 30].Text = Dt.Rows[i]["JIWON"].ToString().Trim();
                ssList_Sheet1.Cells[i, 31].Text = "00";
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

            btnSave.Enabled = true;
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CS = new clsSpread();
            CPQ = new clsPmpaQuery();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);

            dtpDate.Text = clsPublic.GstrSysDate;

            btnSave.Enabled = false;

            ssList_Sheet1.Columns[5].Visible = false; //초재구분
            ssList_Sheet1.Columns[8].Visible = false; //성별
            ssList_Sheet1.Columns[9].Visible = false; //나이
            ssList_Sheet1.Columns[10].Visible = false; //감액코드
            ssList_Sheet1.Columns[11].Visible = false; //특진여부
            ssList_Sheet1.Columns[12].Visible = false; //수납구분
            ssList_Sheet1.Columns[13].Visible = false; //보훈여부
            ssList_Sheet1.Columns[15].Visible = false; //예약일자
            ssList_Sheet1.Columns[16].Visible = false; //최종예약일자
            ssList_Sheet1.Columns[17].Visible = false; //진찰료 발생금액
            ssList_Sheet1.Columns[18].Visible = false; //진찰료 특진료
            ssList_Sheet1.Columns[19].Visible = false; //진찰료 총액
            ssList_Sheet1.Columns[20].Visible = false; //진찰료 조합부담
            ssList_Sheet1.Columns[21].Visible = false; //진찰료 감액
            ssList_Sheet1.Columns[22].Visible = false; //진찰료 미수
            ssList_Sheet1.Columns[24].Visible = false; //계약처코드
            ssList_Sheet1.Columns[26].Visible = false; //ROWID(OPD_MASTER)
            ssList_Sheet1.Columns[27].Visible = false; //OPD_MASTER(M), OPD_RESERVED(R)

            //if (clsType.User.IdNumber == "25420")
            //    dtpDate.Enabled = true;

        }

    }
}
