using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    public partial class frmSepisisUnit : Form
    {
        string mstrIpdNo = "";

        public frmSepisisUnit(string argPano, string argSName, string argWard, string argRoom, string argJumin, string argInDate, string argInTime, string argDiagnosis, string strIpdNo)
        {
            InitializeComponent();

            //환자번호 성명  병동 병실  주민번호 입실일시    시간 진단명

            ssView_Sheet1.Cells[2, 0].Text = argPano;
            ssView_Sheet1.Cells[2, 1].Text = argSName;
            ssView_Sheet1.Cells[2, 2].Text = argWard;
            ssView_Sheet1.Cells[2, 3].Text = argRoom;
            ssView_Sheet1.Cells[2, 4].Text = argJumin;
            ssView_Sheet1.Cells[2, 5].Text = argInDate;
            ssView_Sheet1.Cells[2, 6].Text = argInTime;
            ssView_Sheet1.Cells[2, 7].Text = argDiagnosis;
            mstrIpdNo = strIpdNo;
        }

        private void frmSepisisUnit_Load(object sender, EventArgs e)
        {
            DATALOAD();
            READ_PROGRESS(mstrIpdNo);
        }

        private void READ_PROGRESS(string ArgIPDNO)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            ssSOFA_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //'의뢰일시 , 접수일시, 회신일시, 입원 / 외래, 진료과, 병동, 병실, 담당과장, 등록번호, 성명, 의뢰자, 답변자, WRTNO
                SQL = "";
                SQL = " SELECT TOTAL ,GRADE,  TO_CHAR(ENTDATE, 'YYYY-MM-DD HH24:MI') ENTDATE, ROWID, EMRNO ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_SOFASCOER MST";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO + " ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ENTDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssSOFA_Sheet1.RowCount = dt.Rows.Count;
                    ssSOFA_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssSOFA_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssSOFA_Sheet1.Cells[i, 1].Text = dt.Rows[i]["TOTAL"].ToString().Trim() + "   " + dt.Rows[i]["GRADE"].ToString().Trim();
                        ssSOFA_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssSOFA_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void DATALOAD()
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT EMRNO, BUN1, BUN2, BUN3, BUN4, BUN5, BUN6, BUN7, BUN8, BUN_TEXT1, BUN_TEXT2  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SEPSISNOTE ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO  = " + mstrIpdNo + "";
                SQL = SQL + ComNum.VBLF + "    AND ENTDATE = (SELECT MAX(ENTDATE)  FROM KOSMOS_PMPA.NUR_SEPSISNOTE WHERE IPDNO  = " + mstrIpdNo + ")";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    //'◆ 기본정보
                    ssView_Sheet1.Cells[0, 9].Text = dt.Rows[0]["EMRNO"].ToString().Trim();

                    ssView_Sheet1.Cells[5, 2].Value = (dt.Rows[0]["BUN1"].ToString().Trim() == "1" ? true : false);
                    ssView_Sheet1.Cells[5, 5].Value = (dt.Rows[0]["BUN2"].ToString().Trim() == "1" ? true : false);

                    ssView_Sheet1.Cells[7, 1].Value = (dt.Rows[0]["BUN3"].ToString().Trim() == "1" ? true : false);

                    ssView_Sheet1.Cells[8, 3].Value = (dt.Rows[0]["BUN4"].ToString().Trim() == "1" ? true : false);

                    ssView_Sheet1.Cells[9, 2].Value = (dt.Rows[0]["BUN5"].ToString().Trim() == "1" ? true : false);

                    ssView_Sheet1.Cells[11, 1].Value = (dt.Rows[0]["BUN6"].ToString().Trim() == "1" ? true : false);

                    ssView_Sheet1.Cells[12, 3].Value = (dt.Rows[0]["BUN7"].ToString().Trim() == "1" ? true : false);

                    ssView_Sheet1.Cells[15, 1].Value = (dt.Rows[0]["BUN8"].ToString().Trim() == "1" ? true : false);

                    ssView_Sheet1.Cells[6, 1].Text = dt.Rows[0]["BUN_TEXT1"].ToString().Trim();
                    ssView_Sheet1.Cells[10, 1].Text = dt.Rows[0]["BUN_TEXT2"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " INSERT INTO NUR_SEPSISNOTE      ";
                SQL = SQL + ComNum.VBLF + "(";
                SQL = SQL + ComNum.VBLF + "  PANO ,  IPDNO  ,  BDATE    , BUN1  , BUN2  , BUN3   ,  BUN4   ,  BUN5 , BUN6   ,  BUN7  , BUN8  , BUN_TEXT1,BUN_TEXT2, ENTDATE    , ENTSABUN  ";
                SQL = SQL + ComNum.VBLF + ")    ";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "(";
                SQL = SQL + ComNum.VBLF + "'" + ssView_Sheet1.Cells[2, 0].Text.Trim() + "',"; //PANO
                SQL = SQL + ComNum.VBLF + "'" + mstrIpdNo + "',"; //IPDNO
                SQL = SQL + ComNum.VBLF + " TRUNC(SYSDATE), "; //BDATE
                SQL = SQL + ComNum.VBLF + "'" + (Convert.ToBoolean(ssView_Sheet1.Cells[5, 2].Value) == true ? "1" : "0") + "',";  //BUN1
                SQL = SQL + ComNum.VBLF + "'" + (Convert.ToBoolean(ssView_Sheet1.Cells[5, 5].Value) == true ? "1" : "0") + "',";  //BUN2
                SQL = SQL + ComNum.VBLF + "'" + (Convert.ToBoolean(ssView_Sheet1.Cells[7, 1].Value) == true ? "1" : "0") + "',";  //BUN3
                SQL = SQL + ComNum.VBLF + "'" + (Convert.ToBoolean(ssView_Sheet1.Cells[8, 3].Value) == true ? "1" : "0") + "',";  //BUN4
                SQL = SQL + ComNum.VBLF + "'" + (Convert.ToBoolean(ssView_Sheet1.Cells[9, 2].Value) == true ? "1" : "0") + "',";  //BUN5
                SQL = SQL + ComNum.VBLF + "'" + (Convert.ToBoolean(ssView_Sheet1.Cells[11, 1].Value) == true ? "1" : "0") + "',";  //BUN6
                SQL = SQL + ComNum.VBLF + "'" + (Convert.ToBoolean(ssView_Sheet1.Cells[12, 3].Value) == true ? "1" : "0") + "',";  //BUN7
                SQL = SQL + ComNum.VBLF + "'" + (Convert.ToBoolean(ssView_Sheet1.Cells[15, 1].Value) == true ? "1" : "0") + "',";  //BUN8
                SQL = SQL + ComNum.VBLF + "'" + ssView_Sheet1.Cells[6, 1].Text.Trim() + "',"; //BUN_TEXT1
                SQL = SQL + ComNum.VBLF + "'" + ssView_Sheet1.Cells[10, 1].Text.Trim() + "',"; // BUN_TEXT2
                SQL = SQL + ComNum.VBLF + "SYSDATE , ";
                SQL = SQL + ComNum.VBLF + "" + clsType.User.Sabun;
                SQL = SQL + ComNum.VBLF + ")";


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (SAVE_EMR(mstrIpdNo) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("SAVE_EMR 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);

                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

        }

        private bool SAVE_EMR(string ArgIPDNO)
        {
            int nEMRNO = 0;
            string strInDate = "";
            //string strInTime = "";
            string strPano = "";
            //string stInDate = "";
            string strOutDate = "";
            string strOutTime = "";
            string strDeptCode = "";
            string strDrCd = "";
            //string strWRITEDATE = "";
            //string strWriteTime = "";
            string strXML = "";
            string strChartX1 = "";
            string strChartX2 = "";

            string strHead = "";
            string strTagHead = "";
            string strTagVal = "";
            string strTagTail = "";
            string strXMLCert = "";

            string strIPDNO = "";
            //string strEMRNO = "";

            string strChartDate = "";
            string strChartTime = "";

            string strIK5 = "";
            string strIK6 = "";
            string strIT2 = "";
            string strIK3 = "";
            string strIK4 = "";
            string strIT3 = "";
            string strIK2 = "";
            string strIK1 = "";
            string strIK7 = "";
            string strIK8 = "";

            string strIT1 = "";
            string strDT1 = "";

            string strROWID = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            strChartDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            strChartTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            strDT1 = ComFunc.FormatStrToDate(strChartDate, "D");
            strIT1 = clsType.User.UserName;

            strIK5 = (Convert.ToBoolean(ssView_Sheet1.Cells[5, 2].Value) == true ? "true" : "false");
            strIK6 = (Convert.ToBoolean(ssView_Sheet1.Cells[5, 5].Value) == true ? "true" : "false");
            strIK3 = (Convert.ToBoolean(ssView_Sheet1.Cells[7, 1].Value) == true ? "true" : "false");
            strIK4 = (Convert.ToBoolean(ssView_Sheet1.Cells[8, 3].Value) == true ? "true" : "false");
            strIK2 = (Convert.ToBoolean(ssView_Sheet1.Cells[9, 2].Value) == true ? "true" : "false");
            strIK1 = (Convert.ToBoolean(ssView_Sheet1.Cells[11, 1].Value) == true ? "true" : "false");
            strIK7 = (Convert.ToBoolean(ssView_Sheet1.Cells[12, 3].Value) == true ? "true" : "false");
            strIK8 = (Convert.ToBoolean(ssView_Sheet1.Cells[15, 1].Value) == true ? "true" : "false");

            strIT2 = ssView_Sheet1.Cells[6, 1].Text.Trim();
            strIT3 = ssView_Sheet1.Cells[10, 1].Text.Trim();

            try
            {
                SQL = "";
                SQL = " SELECT PANO, SNAME, INDATE, OUTDATE, DRCODE, DEPTCODE, IPDNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    strPano = dt.Rows[0]["PANO"].ToString().Trim();
                    strInDate = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
                    strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                    strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                    strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    strIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("환자 정보가 없습니다. 저장에 실패 하였습니다.");
                    return false;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT A.ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SEPSISNOTE A";
                SQL = SQL + ComNum.VBLF + " WHERE ENTDATE = (SELECT MAX(ENTDATE)";
                SQL = SQL + ComNum.VBLF + "                  FROM KOSMOS_PMPA.NUR_SEPSISNOTE";
                SQL = SQL + ComNum.VBLF + "                  WHERE IPDNO = " + strIPDNO + " )";
                SQL = SQL + ComNum.VBLF + " AND A.IPDNO = " + strIPDNO;


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                strXML = "";

                strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
                strChartX1 = "<chart>";
                strChartX2 = "</chart>";

                strXML = strHead + strChartX1;

                strTagHead = "<ik5 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "NO1" + VB.Chr(34) + ">";
                strTagVal = strIK5;
                strTagTail = "</ik5>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<ik6 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "NO2" + VB.Chr(34) + ">";
                strTagVal = strIK6;
                strTagTail = "</ik6>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<ik3 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "YES1 R" + VB.Chr(34) + ">";
                strTagVal = strIK3;
                strTagTail = "</ik3>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<ik4 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "NO3" + VB.Chr(34) + ">";
                strTagVal = strIK4;
                strTagTail = "</ik4>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<ik2 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "YES2" + VB.Chr(34) + ">";
                strTagVal = strIK2;
                strTagTail = "</ik2>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<ik1 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "NO4" + VB.Chr(34) + ">";
                strTagVal = strIK1;
                strTagTail = "</ik1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<ik7 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "YES3" + VB.Chr(34) + ">";
                strTagVal = strIK7;
                strTagTail = "</ik7>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<ik8 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "YES3" + VB.Chr(34) + ">";
                strTagVal = strIK8;
                strTagTail = "</ik8>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<it2 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "memo1" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT2;
                strTagTail = "]]></it2>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<it3 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "memo2" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT3;
                strTagTail = "]]></it3>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<dt1 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "작성일자" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strDT1;
                strTagTail = "]]></dt1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it1 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "작성시간" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT1;
                strTagTail = "]]></it1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strXML = strXML + " <im1  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표1" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10510" + VB.Chr(34) + "></im1>";
                strXML = strXML + " <im2  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표2" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10511" + VB.Chr(34) + "></im2>";
                strXML = strXML + " <im3  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표3" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10512" + VB.Chr(34) + "></im3>";
                strXML = strXML + " <im4  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표4" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10513" + VB.Chr(34) + "></im4>";
                strXML = strXML + " <im5  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표5" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10513" + VB.Chr(34) + "></im5>";
                strXML = strXML + " <im6  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표6" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10513" + VB.Chr(34) + "></im6>";
                strXML = strXML + " <im7  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표7" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10513" + VB.Chr(34) + "></im7>";
                strXML = strXML + " <im8  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표8" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10513" + VB.Chr(34) + "></im8>";
                strXML = strXML + " <im9  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표9" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10513" + VB.Chr(34) + "></im9>";
                strXML = strXML + " <im10  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표10" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10513" + VB.Chr(34) + "></im10>";
                strXML = strXML + " <im11  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표11" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10510" + VB.Chr(34) + "></im11>";
                strXML = strXML + " <im12  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표12" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10510" + VB.Chr(34) + "></im12>";
                strXML = strXML + " <im13  type=" + VB.Chr(34) + "image" + VB.Chr(34) + " label=" + VB.Chr(34) + "화살표13" + VB.Chr(34) + " src=" + VB.Chr(34) + "getChartImage.mts?imgNo=10511" + VB.Chr(34) + "></im13>";

                strXMLCert = strXML;

                strXML = strXML + strChartX2;

                strXMLCert = strXML;

                SQL = "";
                SQL = SQL + "SELECT KOSMOS_EMR.GETEMRXMLNO() FUNSEQNO FROM DUAL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    nEMRNO = Convert.ToInt32(dt.Rows[0]["FUNSEQNO"].ToString().Trim());
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("FUNSEQNO 조회중 문제가 발생했습니다");
                    return false;
                }

                dt.Dispose();
                dt = null;

                if (clsNurse.CREATE_EMR_XMLINSRT3(nEMRNO, "2666", clsType.User.IdNumber, strChartDate, strChartTime, 0,
                                        strPano, "I", strInDate.Replace("-", ""), "120000", strOutDate.Replace("-", ""), strOutTime, strDeptCode, strDrCd, "0", 1, strXML) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("기록지 생성에 오류가 발생 하였습니다.", "확인");
                    return false;
                }

                SQL = "";
                SQL = " UPDATE KOSMOS_PMPA.NUR_SEPSISNOTE SET ";
                SQL = SQL + ComNum.VBLF + " EMRNO = " + nEMRNO;
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
