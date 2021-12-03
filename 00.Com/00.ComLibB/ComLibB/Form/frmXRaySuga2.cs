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

namespace ComLibB
{
    public partial class frmXRaySuga2 : Form
    {
        ComFunc CF = new ComFunc();
        private int FnSS2Row;

        string strSuCode;
        string strSuNext;
        string strBCode;

        int nNewIAmt;      //'일반수가

        int nBAmt;
        int nTAmt;
        int nIAmt;
        string strSuDate;
        int nOldBAmt;
        int nOldTAmt;
        int nOldIAmt;
        string strSuDate3;
        int nBAmt3;
        int nTAmt3;
        int nIAmt3;
        string strSuDate4;
        int nBAmt4;
        int nTAmt4;
        int nIAmt4;
        string strSuDate5;
        int nBAmt5;
        int nTAmt5;
        int nIAmt5;

        public frmXRaySuga2()
        {
            InitializeComponent();
        }

        private void frmXRaySuga2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpSuga.Text = CF.DATE_ADD(clsDB.DbCon, CF.READ_LASTDAY(clsDB.DbCon, clsPublic.GstrSysDate), 1);

        }

        #region //Suga_Update_Main
        private string Suga_Update_Main(string ArgBCode, int ArgMesu, int ArgPrice)
        {
            string rtnVal = "NO";
            int nREAD;

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return rtnVal; //권한 확인

                //'방사선 필름 일반수가를 SET (보험수가 200%,10원이하 절사)
                nNewIAmt = Gesan_IlbanAmt(ArgPrice, "65", "1", "0");

                //'해당 자료를 SELECT

                SQL = "SELECT SUCODE,SUNEXT,BAMT,TAMT,IAMT,TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE,";
                SQL = SQL + ComNum.VBLF + " OLDBAMT,OLDTAMT,OLDIAMT,SUHAM,SUNAMEK,BCODE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3,BAMT3,TAMT3,IAMT3,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4,BAMT4,TAMT4,IAMT4,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5,BAMT5,TAMT5,IAMT5 ";
                SQL = SQL + ComNum.VBLF + " FROM VIEW_SUGA_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE BUN >= '65' AND BUN <= '66' ";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND GBN   = 'H' ";
                SQL = SQL + ComNum.VBLF + "  AND SUGBE = '1' "; //'행위료만 처리함;
                SQL = SQL + ComNum.VBLF + "  AND BAMT  <> 0  "; //'현재수가가 0이 아닌것(산정제외가 아닌것);
                switch (ArgBCode)
                {
                    case "G00 - G60":
                        SQL = SQL + ComNum.VBLF + " AND (BCODE >= 'G00' AND BCODE <= 'G20999' ";
                        SQL = SQL + ComNum.VBLF + "  OR  BCODE >= 'G25' AND BCODE <= 'G60999') ";
                        break;
                    case "G21 - G24":
                        SQL = SQL + ComNum.VBLF + " AND BCODE >= 'G21' AND BCODE <= 'G24999' ";
                        break;
                    case "G61 - G80":
                        SQL = SQL + ComNum.VBLF + " AND BCODE >= 'G61' AND BCODE <= 'G80999' ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + " AND BCODE >= 'G81' AND BCODE <= 'G90999' ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "  AND SUBSTR(BCODE,4,2)='" + VB.Format(ArgMesu, "00") + "' "; //'촬영매수;
                SQL = SQL + ComNum.VBLF + "  AND (SUDATE IS NULL OR SUDATE <= TO_DATE('" + dtpSuga.Text + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "  AND BAMT <> " + ArgPrice + " ";  //'보험단가가 틀린 경우만;
                SQL = SQL + ComNum.VBLF + "ORDER BY SUCODE,SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    ss2_Sheet1.RowCount = 0;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSuCode = dt.Rows[i]["SuCode"].ToString().Trim();
                        strSuNext = dt.Rows[i]["SuNext"].ToString().Trim();
                        strBCode = dt.Rows[i]["BCode"].ToString().Trim();
                        nBAmt = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()));
                        nTAmt = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt"].ToString().Trim()));
                        nIAmt = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()));
                        strSuDate = dt.Rows[i]["SuDate"].ToString().Trim();
                        nOldBAmt = Convert.ToInt32(VB.Val(dt.Rows[i]["OldBAmt"].ToString().Trim()));
                        nOldTAmt = Convert.ToInt32(VB.Val(dt.Rows[i]["OldTAmt"].ToString().Trim()));
                        nOldIAmt = Convert.ToInt32(VB.Val(dt.Rows[i]["OldIAmt"].ToString().Trim()));
                        strSuDate3 = dt.Rows[i]["SuDate3"].ToString().Trim();
                        nBAmt3 = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt3"].ToString().Trim()));
                        nTAmt3 = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt3"].ToString().Trim()));
                        nIAmt3 = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt3"].ToString().Trim()));
                        strSuDate4 = dt.Rows[i]["SuDate4"].ToString().Trim();
                        nBAmt4 = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt4"].ToString().Trim()));
                        nTAmt4 = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt4"].ToString().Trim()));
                        nIAmt4 = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt4"].ToString().Trim()));
                        strSuDate5 = dt.Rows[i]["SuDate5"].ToString().Trim();
                        nBAmt5 = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt5"].ToString().Trim()));
                        nTAmt5 = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt5"].ToString().Trim()));
                        nIAmt5 = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt5"].ToString().Trim()));


                        //'수가를 변경전의 내용을 Display
                        ss2_Sheet1.RowCount = ss2_Sheet1.RowCount + 1;
                        ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                        ss2_Sheet1.Cells[i, 0].Text = strBCode;
                        ss2_Sheet1.Cells[i, 1].Text = strSuCode;
                        ss2_Sheet1.Cells[i, 2].Text = strSuNext;
                        ss2_Sheet1.Cells[i, 3].Text = VB.Format(nBAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 4].Text = VB.Format(nTAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 5].Text = VB.Format(nIAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 6].Text = strSuDate;
                        ss2_Sheet1.Cells[i, 7].Text = VB.Format(nOldBAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 8].Text = VB.Format(nOldTAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 9].Text = VB.Format(nOldIAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["SuNameK"].ToString().Trim();


                        if (Suga_Update_Change(ArgPrice) == false)  //'수가를 변경함
                        {
                            return "NO";
                        }


                        //'수가를 변경후 내용을 Display
                        ss2_Sheet1.RowCount = ss2_Sheet1.RowCount + 1;
                        ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                        ss2_Sheet1.Cells[i, 3].Text = VB.Format(nBAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 4].Text = VB.Format(nTAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 5].Text = VB.Format(nIAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 6].Text = strSuDate;
                        ss2_Sheet1.Cells[i, 7].Text = VB.Format(nOldBAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 8].Text = VB.Format(nOldTAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 9].Text = VB.Format(nOldIAmt, "#######0.0");
                        ss2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["SuNameK"].ToString().Trim();

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

            return "OK";
        }
        #endregion

        #region //Gesan_IlbanAmt
        private int Gesan_IlbanAmt(int ArgBAmt, string argBun, string ArgSugbE, string ArgSugbF, string ArgSugbJ = "")
        {
            int nIAmt = 0;

            //'진찰료,입원료는 보험가,일반가 동일하게 처리함
            if (Convert.ToInt32(argBun) <= Convert.ToInt32("10"))
            {
                return ArgBAmt;
            }


            //'비급여수가(식대(74)-종합건진(84)는 보험가,일반가 동일하게 처리함
            if (Convert.ToInt32(argBun) >= Convert.ToInt32("74"))
            {
                return ArgBAmt;
            }


            //'내복약,외용약품의 일반가 계산
            if ((argBun == "11" || argBun == "12") && ArgSugbE == "0")
            {
                nIAmt = Gesan_IlbanAmt_YAK(ArgBAmt, ArgSugbF);
            }
            //'주사약 일반가 계산
            else if (argBun == "20" && ArgSugbE == "0")
            {
                nIAmt = Gesan_IlbanAmt_JUSA(ArgBAmt, ArgSugbF);
            }
            //'기타 일반수가를 계산
            else
            {
                nIAmt = Gesan_IlbanAmt_ETC(ArgBAmt, ArgSugbE, ArgSugbJ);
            }

            return nIAmt;
        }
        #endregion

        #region //Gesan_IlbanAmt_YAK
        private int Gesan_IlbanAmt_YAK(int ArgBAmt, string ArgSugbF)   //'내복약,외용약품의 일반가 계산
        {
            int nIAmt = 0;

            //'비급여수가 100,000원이상은 보험가,일반가 동일하게 처리함
            if (ArgSugbF == "1" && ArgBAmt >= 100000)
            {
                nIAmt = ArgBAmt;
                return nIAmt;
            }

            if (ArgBAmt < 11)
            {
                nIAmt = ArgBAmt * 5;
            }
            else if (ArgBAmt < 51)
            {
                nIAmt = ArgBAmt * 4;
            }
            else if (ArgBAmt < 101)
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 3.5);
            }
            else if (ArgBAmt < 500)
            {
                nIAmt = ArgBAmt * 3;
            }
            else if (ArgBAmt < 1001)
            {
                nIAmt = ArgBAmt * 2;
            }
            else
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 1.5);
            }
            return nIAmt;
        }
        #endregion

        #region //Gesan_IlbanAmt_JUSA
        private int Gesan_IlbanAmt_JUSA(int ArgBAmt, string ArgSugbF)   //'주사약제 일반수가 계산
        {
            int nIAmt = 0;

            //'비급여수가 100,000원이상은 보험가,일반가 동일하게 처리함
            if (ArgSugbF == "1" && ArgBAmt >= 100000)
            {
                nIAmt = ArgBAmt;
                return nIAmt;
            }

            if (ArgBAmt < 501)
            {
                nIAmt = ArgBAmt * 5;
            }
            else if (ArgBAmt < 1001)
            {
                nIAmt = ArgBAmt * 4;
            }
            else if (ArgBAmt < 3001)
            {
                nIAmt = ArgBAmt * 3;
            }
            else if (ArgBAmt < 5001)
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 2.5);
            }
            else if (ArgBAmt < 10001)
            {
                nIAmt = ArgBAmt * 2;
            }
            else
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 1.5);
            }
            return nIAmt;
        }
        #endregion

        #region //Gesan_IlbanAmt_ETC
        private int Gesan_IlbanAmt_ETC(int ArgBAmt, string ArgSugbE, string ArgSugbJ)    //'기타수가 일반가 계산
        {
            int nIAmt = 0;

            //'행위료이면 보험수가 * 보험병원가산율 * 2
            if (ArgSugbE == "1") nIAmt = Convert.ToInt32((ArgBAmt * 1.25) * 2);
            //'재료대이면 보험수가의 2배
            if (ArgSugbE != "1") nIAmt = ArgBAmt * 2;
            //'10원보다 크면 10원미만 절사
            //'외부의뢰검사 는 절사 않함
            if (ArgSugbJ != "9" && ArgSugbJ != "8")
            {
                if (nIAmt > 10)
                {
                    nIAmt = nIAmt / 10;
                    nIAmt = nIAmt * 10;
                }
            }
            return nIAmt;
        }
        #endregion

        #region //Suga_Update_Change
        private bool Suga_Update_Change(int ArgPrice)   //'수가를 변경
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                //'재작업이면 Old수가는 처리 안함
                if (strSuDate == dtpSuga.Text)
                {
                    nBAmt = ArgPrice;
                    nTAmt = ArgPrice;
                    nIAmt = nNewIAmt;

                    SQL = "UPDATE BAS_SUH SET BAMT=" + ArgPrice + ",";
                    SQL = SQL + ComNum.VBLF + " TAMT=" + ArgPrice + ",IAMT=" + nNewIAmt + " ";
                    SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + strSuCode + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUNEXT = '" + strSuNext + "' ";
                }
                else
                {
                    //'수가변경4 => 수가변경5로
                    strSuDate5 = strSuDate4;
                    nBAmt5 = nBAmt4;
                    nTAmt5 = nTAmt4;
                    nIAmt5 = nIAmt4;
                    //'수가변경3 => 수가변경4로
                    strSuDate4 = strSuDate3;
                    nBAmt4 = nBAmt3;
                    nTAmt4 = nTAmt3;
                    nIAmt4 = nIAmt3;
                    //'수가Old => 수가변경3로
                    strSuDate3 = strSuDate;
                    nBAmt3 = nOldBAmt;
                    nTAmt3 = nOldTAmt;
                    nIAmt3 = nOldIAmt;
                    //'현재수가를 => Old수가변경
                    strSuDate = dtpSuga.Text;
                    nOldBAmt = nBAmt;
                    nOldTAmt = nTAmt;
                    nOldIAmt = nIAmt;
                    //'변경할수가를 현재수가로
                    nBAmt = ArgPrice;
                    nTAmt = ArgPrice;
                    nIAmt = nNewIAmt;

                    SQL = "UPDATE BAS_SUH SET BAMT=" + nBAmt + ",TAMT=" + nTAmt + ",IAMT=" + nIAmt + ",";
                    SQL = SQL + ComNum.VBLF + " SUDATE=TO_DATE('" + dtpSuga.Text + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " OLDBAMT=" + nOldBAmt + ",OLDTAMT=" + nOldTAmt + ",OLDIAMT=" + nOldIAmt + ",";
                    SQL = SQL + ComNum.VBLF + " SUDATE3=TO_DATE('" + strSuDate3 + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " BAMT3=" + nBAmt3 + ",TAMT3=" + nTAmt3 + ",IAMT3=" + nIAmt3 + ",";
                    SQL = SQL + ComNum.VBLF + " SUDATE4=TO_DATE('" + strSuDate4 + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " BAMT4=" + nBAmt4 + ",TAMT4=" + nTAmt4 + ",IAMT4=" + nIAmt4 + ",";
                    SQL = SQL + ComNum.VBLF + " SUDATE5=TO_DATE('" + strSuDate5 + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " BAMT5=" + nBAmt5 + ",TAMT5=" + nTAmt5 + ",IAMT5=" + nIAmt5 + " ";
                    SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + strSuCode + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUNEXT = '" + strSuNext + "' ";
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

                if (intRowAffected != 0)
                {
                    clsPublic.GstrMsgList = strSuCode + "의 " + strSuNext + "수가코드 UPDATE" + ComNum.VBLF;
                    clsPublic.GstrMsgList = clsPublic.GstrMsgList + "시 오류가 발생함";
                    return rtVal;
                }

                //'그룹코드의 합계금액을 UPDATE
                if (Group_SUGA_UPDATE(strSuCode, VB.Trim(dtpSuga.Text)) != "OK")
                {
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
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
        #endregion

        #region //Group_SUGA_UPDATE
        private string Group_SUGA_UPDATE(string ArgSuCode, string ArgSuDate)
        {
            string rtVal = "NO";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //' 묶음코드의 보험,자보,일반수가를 하위코드를 기준으로 다시 계산함
            //' ArgSuCode$:수가코드  ArgSuDate$:적용일자

            int nTotBAmt;
            int nTotTAmt;
            int nTotIAmt;

            string[] strSuDate = new string[4];
            int[] nBAmt = new int[5];
            int[] nTAmt = new int[5];
            int[] nIAmt = new int[5];

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return rtVal; //권한 확인

                //'보험총액,자보총액,일반수가총액을 계산함.

                SQL = "SELECT SUM(DECODE(SUGBE,'1',BAMT*SUQTY*1.25,BAMT*SUQTY)) CBAMT,";
                SQL = SQL + ComNum.VBLF + " SUM(DECODE(SUGBE,'1',TAMT*SUQTY*1.37,TAMT*SUQTY)) CTAMT,";
                SQL = SQL + ComNum.VBLF + " SUM(IAMT*SUQTY) CIAMT ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_SUH ";
                SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + ArgSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SUGBSS IN ('0','1') "; //'일반수가 계산시 성인기준으로

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                    clsPublic.GstrMsgList = ArgSuCode + " 수가코드는 그룹(묶음)코드가 아니거나," + ComNum.VBLF;
                    clsPublic.GstrMsgList = clsPublic.GstrMsgList + "수가코드가 등록되지 않았습니다." + ComNum.VBLF + ComNum.VBLF;
                    clsPublic.GstrMsgList = clsPublic.GstrMsgList + "그룹(묶음)코드의 보험,자보,일반수가" + ComNum.VBLF;
                    clsPublic.GstrMsgList = clsPublic.GstrMsgList + "변경하지 못 하였습니다.";

                    ComFunc.MsgBox(clsPublic.GstrMsgList, "오류발생");
                    return rtVal;
                }

                nTotBAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["cBAmt"].ToString().Trim()));
                nTotTAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["cTAmt"].ToString().Trim()));

                if (Convert.ToInt32(VB.Val(dt.Rows[0]["cIAmt"].ToString().Trim())) >= 1000)
                {
                    nTotIAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["cIAmt"].ToString().Trim())) / 100;
                    nTotIAmt = nTotIAmt * 100;
                }
                else
                {
                    nTotIAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["cIAmt"].ToString().Trim())) / 10;
                    nTotIAmt = nTotIAmt * 10;
                }

                dt.Dispose();
                dt = null;

                //'UPDATE할 BAS_SUT를 READ
                SQL = "SELECT BAMT,TAMT,IAMT,TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE,";
                SQL = SQL + ComNum.VBLF + " OLDBAMT,OLDTAMT,OLDIAMT,";
                SQL = SQL + ComNum.VBLF + " BAMT3,TAMT3,IAMT3,TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3,";
                SQL = SQL + ComNum.VBLF + " BAMT4,TAMT4,IAMT4,TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4,";
                SQL = SQL + ComNum.VBLF + " BAMT5,TAMT5,IAMT5,TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5 ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_SUT ";
                SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + VB.Trim(ArgSuCode) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                    clsPublic.GstrMsgList = ArgSuCode + "가 수가코드(BAS_SUT)에 등록되지" + ComNum.VBLF;
                    clsPublic.GstrMsgList = clsPublic.GstrMsgList + "않았습니다. 확인 하세요.";
                    ComFunc.MsgBox(clsPublic.GstrMsgList, "오류발생");
                    return rtVal;
                }


                nBAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[0]["BAmt"].ToString().Trim()));
                nTAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[0]["TAmt"].ToString().Trim()));
                nIAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[0]["IAmt"].ToString().Trim()));
                strSuDate[0] = dt.Rows[0]["SuDate"].ToString().Trim();
                nBAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[0]["OldBAmt"].ToString().Trim()));
                nTAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[0]["OldTAmt"].ToString().Trim()));
                nIAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[0]["OldIAmt"].ToString().Trim()));
                strSuDate[1] = dt.Rows[0]["SuDate3"].ToString().Trim();
                nBAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[0]["BAmt3"].ToString().Trim()));
                nTAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[0]["TAmt3"].ToString().Trim()));
                nIAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[0]["IAmt3"].ToString().Trim()));
                strSuDate[2] = dt.Rows[0]["SuDate4"].ToString().Trim();
                nBAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[0]["BAmt4"].ToString().Trim()));
                nTAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[0]["TAmt4"].ToString().Trim()));
                nIAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[0]["IAmt4"].ToString().Trim()));
                strSuDate[3] = dt.Rows[0]["SuDate5"].ToString().Trim();
                nBAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[0]["BAmt5"].ToString().Trim()));
                nTAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[0]["TAmt5"].ToString().Trim()));
                nIAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[0]["IAmt5"].ToString().Trim()));

                dt.Dispose();
                dt = null;




                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                if (strSuDate[0] == ArgSuDate)
                {
                    SQL = "UPDATE BAS_SUT SET BAMT=" + nTotBAmt + ",";
                    SQL = SQL + ComNum.VBLF + " TAMT=" + nTotTAmt + ",IAMT=" + nTotIAmt + " ";
                    SQL = SQL + ComNum.VBLF + "WHERE SUCODE='" + VB.Trim(ArgSuCode) + "' ";
                }
                else
                {
                    SQL = "UPDATE BAS_SUT SET ";
                    SQL = SQL + ComNum.VBLF + " BAMT=" + nTotBAmt + ",TAMT=" + nTotTAmt + ",IAMT=" + nTotIAmt + ",";
                    SQL = SQL + ComNum.VBLF + " OLDBAMT=" + nBAmt[0] + ",OLDTAMT=" + nTAmt[0] + ",OLDIAMT=" + nIAmt[0] + ",";
                    SQL = SQL + ComNum.VBLF + " BAMT3=" + nBAmt[1] + ",TAMT3=" + nTAmt[1] + ",IAMT3=" + nIAmt[1] + ",";
                    SQL = SQL + ComNum.VBLF + " BAMT4=" + nBAmt[2] + ",TAMT4=" + nTAmt[2] + ",IAMT4=" + nIAmt[2] + ",";
                    SQL = SQL + ComNum.VBLF + " BAMT5=" + nBAmt[3] + ",TAMT5=" + nTAmt[3] + ",IAMT5=" + nIAmt[3] + ",";
                    SQL = SQL + ComNum.VBLF + " SUDATE=TO_DATE('" + ArgSuDate + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " SUDATE3=TO_DATE('" + VB.Trim(strSuDate[0]) + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " SUDATE4=TO_DATE('" + VB.Trim(strSuDate[1]) + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " SUDATE5=TO_DATE('" + VB.Trim(strSuDate[2]) + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "WHERE SUCODE='" + VB.Trim(ArgSuCode) + "' ";
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

                if (intRowAffected != 0)
                {
                    clsPublic.GstrMsgList = ArgSuCode + "BAS_SUT에 그룹(묶음) 수가를" + ComNum.VBLF;
                    clsPublic.GstrMsgList = clsPublic.GstrMsgList + "UPDATE 도중에 오류가 발생함";
                    ComFunc.MsgBox(clsPublic.GstrMsgList, "오류발생");
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                return "OK";
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
        #endregion

        private void SS_Clear(FarPoint.Win.Spread.SheetView Spd)
        {
            for (int i = 0; i < Spd.RowCount; i++)
            {
                for (int j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.Cells[0, 2, 2, 6].Text = "";
            ss2_Sheet1.RowCount = 50;

            SS_Clear(ss2_Sheet1);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            int i;
            int j;
            int nREAD;
            string strBCode;
            int nMesu;
            int nPrice;
            string strOK;
            int nIlsu;


            ComFunc.ReadSysDate(clsDB.DbCon);
            if (dtpSuga.Value < Convert.ToDateTime(clsPublic.GstrSysDate))
            {
                nIlsu = CF.DATE_ILSU(clsDB.DbCon, clsPublic.GstrSysDate, VB.Trim(dtpSuga.Text));
            }
            else
            {
                nIlsu = CF.DATE_ILSU(clsDB.DbCon, VB.Trim(dtpSuga.Text), clsPublic.GstrSysDate);
            }


            clsPublic.GstrMsgList = "변경할 수가 적용일자가 정말로 " + dtpSuga.Text + "일이" + ComNum.VBLF;
            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "맞습니까? 만일 오류날짜를 지정하면" + ComNum.VBLF;
            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "다시 복구가 불가능 합니다. ";

            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "날짜확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No) return;

            btnChange.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = false;


            strOK = "OK";
            FnSS2Row = 0;
            ss2_Sheet1.RowCount = 50;
            SS_Clear(ss2_Sheet1);



            for (i = 0; i < 3; i++)
            {
                strBCode = ss1_Sheet1.Cells[i, 1].Text;
                for (j = 2; j < 7; j++)
                {
                    nMesu = j - 1;
                    nPrice = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, j].Text));
                    if (nPrice != 0)
                    {
                        strOK = Suga_Update_Main(strBCode, nMesu, nPrice);
                        if (strOK != "OK") break;
                    }
                }
            }

            if (strOK == "OK")
            {
                btnChange.Enabled = true;
                btnCancel.Enabled = true;
                btnExit.Enabled = true;

                ComFunc.MsgBox("저장 하였습니다.");
            }

        }


    }
}
