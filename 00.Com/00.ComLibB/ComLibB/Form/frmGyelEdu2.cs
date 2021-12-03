using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;
using ComBase;

using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using FarPoint.Win.Spread;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : MedOrder
    /// File Name       : frmGyelEdu.cs
    /// Description     : 결핵환자 교육삼당료1 서식
    /// Author          : 김욱동
    /// Create Date     : 2021-09-13
    /// <history> 
    /// 결핵환자 교육삼당료1 서식
    /// </summary>
    public partial class frmGyelEdu2 : Form
    {
        private string GstrPANO = "";
        private string GstrIllcode = "";
        private string GstrSaveGu = "";
        private string GstrROWID = "";
        private double GdblIpdNO_OCS = 0;
  
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();

        public delegate void SendText(string strText);
        public event SendText rSendText;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmGyelEdu2()
        {
            InitializeComponent();
        }

        public frmGyelEdu2(string strPANO, string strIllcode, string strSaveGu = "")
        {
            InitializeComponent();
            GstrSaveGu = strSaveGu;
            GstrIllcode = strIllcode;
            GstrPANO = strPANO;
     
        }

        private void frmGyelEdu2_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            //폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            txtPano.Text = GstrPANO;

            if (GstrPANO != "") { lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, GstrPANO); }

            if (GstrSaveGu != "") { btnSave.Enabled = false; }

            read_sysdate();

            Screen_display();

            SCREEN_CLEAR();

            GetData();


        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
        }



        private void Screen_display()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strAge = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DRCODE, DRNAME, DEPTCODE, DRBUNHO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "     WHERE SABUN = '" + clsType.User.Sabun + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    //의사성명
                    ssView2_Sheet1.Cells[17, 2].Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                    ssView2_Sheet1.Cells[17, 4].Text = dt.Rows[0]["DRBUNHO"].ToString().Trim();
                    ssView2_Sheet1.Cells[17, 6].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT KORNAME , BUSE FROM ADMIN.INSA_MST ";
                    SQL = SQL + ComNum.VBLF + "   WHERE SABUN = '" + clsType.User.Sabun + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ( TOIDAY IS NULL OR TOIDAY < TRUNC(SYSDATE) )";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssView2_Sheet1.Cells[17, 2].Text = dt.Rows[0]["KORNAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SNAME, PNAME, JUMIN1, JUMIN2 , JIKUP, SEX, TEL, HPHONE, GBFOREIGNER,";
                SQL = SQL + ComNum.VBLF + "     A.ZIPCODE1, A.ZIPCODE2, B.ZIPNAME1 || ' ' || B.ZIPNAME2 ||' ' || B.ZIPNAME3 AS ZIPNAME, JUSO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "BAS_ZIPSNEW B ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.ZIPCODE1 = B.ZIPCODE1(+) ";
                SQL = SQL + ComNum.VBLF + "         AND A.ZIPCODE2 = B.ZIPCODE2(+) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {

                    //환자명
                    ssView2_Sheet1.Cells[5, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();

                    clsAES.Read_Jumin_AES(clsDB.DbCon, txtPano.Text);

                    //주민등록번호
                    ssView2_Sheet1.Cells[5, 8].Text = clsAES.GstrAesJumin1 + "-" + clsAES.GstrAesJumin2;

                    ////연령
                    //strAge = ComFunc.AgeCalc(clsDB.DbCon, clsAES.GstrAesJumin1 + clsAES.GstrAesJumin2).ToString();

                    //if (VB.Val(strAge) < 19)
                    //{
                    //    ssView2_Sheet1.Cells[8, 1].Text = "보호자성명 ( " + dt.Rows[0]["PNAME"].ToString().Trim() + " )";
                    //}
                    //else
                    //{
                    //    ssView2_Sheet1.Cells[8, 1].Text = "보호자성명 (         )";
                    //}

                    //GstrAge = strAge;

                    ////우편번호
                    //ssView2_Sheet1.Cells[11, 3].Text = dt.Rows[0]["ZIPCODE1"].ToString().Trim() + "-" + dt.Rows[0]["ZIPCODE2"].ToString().Trim();

                    ////전화번호
                    //ssView2_Sheet1.Cells[9, 2].Text = dt.Rows[0]["HPHONE"].ToString().Trim() + "/" + dt.Rows[0]["TEL"].ToString().Trim();

                    ////주소
                    //ssView2_Sheet1.Cells[10, 2].Text = dt.Rows[0]["ZIPNAME"].ToString().Trim() + " " + dt.Rows[0]["JUSO"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;



                SQL = "";

                SQL += " SELECT                                                                     \r\n";
                SQL += "   CHARTTIME                                                                \r\n";
                SQL += "   ,(SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000000418') AS it10 \r\n";
                SQL += "   ,(SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000000002') AS it11 \r\n";
                //SQL += "   ,(SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000002018') AS it02 \r\n";
                //SQL += "   ,(SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000001765') AS it03 \r\n";
                //SQL += "   ,(SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000014815') AS it04 \r\n";
                SQL += "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A                                  \r\n";
                SQL += "    LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R                     \r\n";
                SQL += "      ON A.EMRNO = R.EMRNO                                                  \r\n";
                SQL += "     AND A.EMRNOHIS = R.EMRNOHIS                                            \r\n";
                SQL += "     AND R.ITEMCD IN('I0000000418','I0000000002')  \r\n";
                SQL += "   WHERE 1 = 1                                                              \r\n";
                SQL += "    AND Ptno = '" + txtPano.Text + "'                                        \r\n";
                //SQL += "    AND FORMNO IN(1562, 3150, 2431)                                           \r\n";
                SQL += "    AND R.ITEMVALUE IS NOT NULL                                         \r\n";
                SQL += "    ORDER BY R.INPDATE DESC ,R.INPTIME DESC                                             \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    string strTemp = "";
                    string strTemp2 = "";

                    strTemp = Regex.Replace(dt.Rows[0]["IT11"].ToString().Trim(), @"[^.,0-9]", "");
                    strTemp2 = Regex.Replace(dt.Rows[0]["IT10"].ToString().Trim(), @"[^.,0-9]", "");

                    ssView2_Sheet1.Cells[19, 5].Text = strTemp;
                    ssView2_Sheet1.Cells[19, 8].Text = strTemp2;

                    if (dt.Rows[0]["IT10"].ToString().Trim() != "" && dt.Rows[0]["IT11"].ToString().Trim() != "")
                    {
                        ssView2_Sheet1.Cells[19, 11].Value = Math.Round((double.Parse(strTemp2) * 10000) / (double.Parse(strTemp) * double.Parse(strTemp)), 1);
                    }

                }

                dt.Dispose();
                dt = null;


                SQL = "";

                SQL += " SELECT ILLNAMEK                                                           \r\n";
                SQL += "  FROM ADMIN.BAS_ILLS                                                \r\n";
                SQL += "   WHERE 1 = 1                                                             \r\n";
                SQL += "    AND ILLCODE = '" + GstrIllcode + "'                                    \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.Cells[6, 2].Text = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                    ssView2_Sheet1.Cells[6, 8].Text = GstrIllcode;

                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                ssView2_Sheet1.Cells[8, 2].Text = cpublic.strSysDate;
                ssView2_Sheet1.Cells[30, 2].Text = cpublic.strSysDate;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void SCREEN_CLEAR()
        {

            ssView2_Sheet1.Cells[10, 2].Value = null;
            ssView2_Sheet1.Cells[11, 2].Value = null;
            ssView2_Sheet1.Cells[11, 6].Value = null;
            ssView2_Sheet1.Cells[11, 10].Value = null;
            ssView2_Sheet1.Cells[12, 2].Value = null;
            ssView2_Sheet1.Cells[13, 2].Value = null;

            ssView2_Sheet1.Cells[20, 4].Value = null;
            ssView2_Sheet1.Cells[21, 4].Value = null;
            ssView2_Sheet1.Cells[22, 2].Value = null;
            ssView2_Sheet1.Cells[23, 2].Value = null;

            ssView2_Sheet1.Cells[24, 2].Value = false;
            ssView2_Sheet1.Cells[24, 5].Value = false;
            ssView2_Sheet1.Cells[24, 8].Value = false;

            ssView2_Sheet1.Cells[25, 2].Value = false;
            ssView2_Sheet1.Cells[25, 5].Value = false;
            ssView2_Sheet1.Cells[25, 8].Value = false;

            ssView2_Sheet1.Cells[26, 2].Value = false;
            ssView2_Sheet1.Cells[26, 5].Value = false;
            ssView2_Sheet1.Cells[26, 8].Value = false;

            ssView2_Sheet1.Cells[27, 2].Value = false;
            ssView2_Sheet1.Cells[27, 5].Value = false;

            ssView2_Sheet1.Cells[27, 8].Value = false;
            ssView2_Sheet1.Cells[28, 9].Value = null;

            ssView2_Sheet1.Cells[28, 2].Value = false;
            ssView2_Sheet1.Cells[28, 5].Value = false;

            //ssView2_Sheet1.Cells[30, 2].Value = null;
            ssView2_Sheet1.Cells[31, 2].Value = null;
            ssView2_Sheet1.Cells[32, 2].Value = null;

            ssView2_Sheet1.Cells[33, 2].Value = null;
            ssView2_Sheet1.Cells[33, 6].Value = null;
            ssView2_Sheet1.Cells[33, 8].Value = null;
            ssView2_Sheet1.Cells[33, 10].Value = null;

            ssView2_Sheet1.Cells[34, 2].Value = false;
            ssView2_Sheet1.Cells[34, 7].Value = false;

            ssView2_Sheet1.Cells[35, 2].Value = false;
            ssView2_Sheet1.Cells[35, 7].Value = false;

            ssView2_Sheet1.Cells[36, 2].Value = false;
            ssView2_Sheet1.Cells[36, 7].Value = false;

            ssView2_Sheet1.Cells[37, 2].Value = false;
            ssView2_Sheet1.Cells[37, 7].Value = false;

            ssView2_Sheet1.Cells[38, 2].Value = false;

            ssView2_Sheet1.Cells[39, 2].Value = false;
            ssView2_Sheet1.Cells[39, 7].Value = false;


            ssView2_Sheet1.Cells[40, 7].Value = false;
            ssView2_Sheet1.Cells[40, 8].Value = null;


            ssView2_Sheet1.Cells[41, 2].Value = null;
            ssView2_Sheet1.Cells[42, 2].Value = null;
            ssView2_Sheet1.Cells[43, 2].Value = null;

        }


        private void btnSearch_Click(object sender, EventArgs e)
        {

            SCREEN_CLEAR();
            GetData();
            btnDelete.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (string.IsNullOrEmpty(GstrROWID) == false)
            {
                if (ComFunc.MsgBoxQ("정말로 삭제하시겠습니까? 삭제한 보고서는 복구 불가능합니다.", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                try
                {


                    clsDB.setBeginTran(clsDB.DbCon);

                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_PMPA + "NUR_STD_INFECT10  WHERE ROWID = '" + GstrROWID + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        lbAutogu.Text = "";
                        SCREEN_CLEAR();
                        GetData();
                        btnDelete.Enabled = false;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("삭제하였습니다.");
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    lbAutogu.Text = "";
                    SCREEN_CLEAR();
                    GetData();
                    btnDelete.Enabled = false;
                    return;
                }


            }
            else
            {
                ComFunc.MsgBox("선택된 보고서가 없습니다. 더블클릭하여 삭제하고자 하는 보고서를 선택하여 주세요.");
                return;
            }
            GetData();
        }
        private void ssView2_CellChanged(object sender, FarPoint.Win.Spread.SheetViewEventArgs e)
        {
            //ssView2_Sheet1.Cells[18, 11].Text = (double.Parse(ssView2_Sheet1.Cells[18, 8].Text.Trim()) * 10000) / (double.Parse(ssView2_Sheet1.Cells[18, 5].Text.Trim()) * double.Parse(ssView2_Sheet1.Cells[18, 5].Text.Trim())); 
        }

        private void GetData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') AS ENTDATE, A.ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT10 A ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();

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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }





        private void btnSave_Click(object sender, EventArgs e)
        {


            SaveData();
            GetData();
            btnDelete.Enabled = false;


        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int k = 0;

            string strSName = "";
            string strSeffect = "";
            string strSeffect01 = "";
            string strSeffect02 = "";
            string strSeffect03 = "";
            string strSeffect04 = "";
            string strSeffect05 = "";
            string strSeffect06 = "";
            string strSeffect07 = "";
            string strSeffect08 = "";
            string strSeffect09 = "";
            string strSeffect10 = "";
            string strSeffect11 = "";
            string strSeffect12 = "";
            string strSeffect13 = "";
            string strSeffect14 = "";
            string strActDate = "";
            string strEduDate = "";
            string strEduCount = "";
            string strEduTime = "";
            string strEducator = "";
            string strAlcoholGu = "";
            string strHeight = "";
            string strWeight = "";
            string strBmi = "";
            string strJumin = "";
            string strillcode = "";
            string strGungu = "";
            string strSmokeGu = "";
            string strGunno = "";
            string strJinInfo1 = "";
            string strJinInfo2 = "";
            string strJinInfo3 = "";
            string strJinInfo4 = "";
            string strJinInfo5 = "";
            string strIpdOpd = "";
            string strDegreecom = "";

            string strSuJect = "";
            string strSuJect1 = "";
            string strSuJect2 = "";
            string strSuJect3 = "";

            string strEduContents01 = "";
            string strEduContents02 = "";
            string strEduContents03 = "";
            string strEduContents04 = "";
            string strEduContents05 = "";
            string strEduContents06 = "";
            string strEduContents07 = "";
            string strEduContents08 = "";
            string strEduContents09 = "";
            string strEduContents10 = "";
            string strEduContents11 = "";
            string strEduContents12 = "";

            string strEduMatPro = "";
            string strEduEvende = "";
            string strRemark = "";




            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록 환자의 등록번호를 조회 후 작업해주세요.");
                return rtnVal;
            }

            //등록일자
            strActDate = ssView2_Sheet1.Cells[8, 2].Text.Trim();
            //이름
            strSName = ssView2_Sheet1.Cells[5, 2].Text.Trim();
            //주민번호
            strJumin = ssView2_Sheet1.Cells[5, 8].Text.Trim();
            //상병코드
            strillcode = ssView2_Sheet1.Cells[6, 8].Text.Trim();
            //산정특례구분
            if (ssView2_Sheet1.Cells[7, 2].Value != null)
            {
                strGungu = ssView2_Sheet1.Cells[7, 2].Value.ToString();
            }
            else
            {
                strGungu = "";
            }
            //산정특례번호 
            strGunno = ssView2_Sheet1.Cells[7, 5].Text.Trim();

            //진단정보
            if (ssView2_Sheet1.Cells[10, 2].Value != null)
            {
                strJinInfo1 = ssView2_Sheet1.Cells[10, 2].Value.ToString();
            }
            else
            {
                strJinInfo1 = "";
            }
            if (ssView2_Sheet1.Cells[11, 2].Value != null)
            {
                strJinInfo2 = ssView2_Sheet1.Cells[11, 2].Value.ToString();
            }
            else
            {
                strJinInfo2 = "";
            }
            if (ssView2_Sheet1.Cells[12, 2].Value != null)
            {
                strJinInfo3 = ssView2_Sheet1.Cells[12, 2].Value.ToString();
            }
            else
            {
                strJinInfo3 = "";
            }
            strJinInfo4 = ssView2_Sheet1.Cells[11, 6].Text.Trim();
            strJinInfo5 = ssView2_Sheet1.Cells[11, 10].Text.Trim();

            if (ssView2_Sheet1.Cells[13, 2].Value != null)
            {
                strIpdOpd = ssView2_Sheet1.Cells[13, 2].Value.ToString();
            }
            else
            {
                strIpdOpd = "";
            }
            

            //교육자
            strEducator = clsType.User.IdNumber;

            //키몸무게비엠아이
            strHeight = ssView2_Sheet1.Cells[19, 5].Text.Trim();
            strWeight = ssView2_Sheet1.Cells[19, 8].Text.Trim();
            strBmi = ssView2_Sheet1.Cells[19, 11].Text.Trim();

            //음주흡연여부
            if (ssView2_Sheet1.Cells[20, 4].Value != null)
            {
                strSmokeGu = ssView2_Sheet1.Cells[20, 4].Value.ToString();
            }
            else
            {
                strSmokeGu = "";
            }
            if (ssView2_Sheet1.Cells[21, 4].Value != null)
            {
                strAlcoholGu = ssView2_Sheet1.Cells[21, 4].Value.ToString();
            }
            else
            {
                strAlcoholGu = "";
            }
            //복양순응도
            if (ssView2_Sheet1.Cells[22, 2].Value != null)
            {
                strDegreecom = ssView2_Sheet1.Cells[22, 2].Value.ToString();
            }
            else
            {
                strDegreecom = "";
            }



            //약제부작용여부
            if (ssView2_Sheet1.Cells[23, 2].Value != null)
            {
                strSeffect = ssView2_Sheet1.Cells[23, 2].Value.ToString();
            }
            else
            {
                strSeffect = "";
            }


            strSeffect01 = ssView2_Sheet1.Cells[24, 2].Text.Trim() == "True" ? "1" : "0";
            strSeffect02 = ssView2_Sheet1.Cells[25, 2].Text.Trim() == "True" ? "1" : "0";
            strSeffect03 = ssView2_Sheet1.Cells[26, 2].Text.Trim() == "True" ? "1" : "0";
            strSeffect04 = ssView2_Sheet1.Cells[27, 2].Text.Trim() == "True" ? "1" : "0";
            strSeffect05 = ssView2_Sheet1.Cells[28, 2].Text.Trim() == "True" ? "1" : "0";

            strSeffect06 = ssView2_Sheet1.Cells[24, 5].Text.Trim() == "True" ? "1" : "0";
            strSeffect07 = ssView2_Sheet1.Cells[25, 5].Text.Trim() == "True" ? "1" : "0";
            strSeffect08 = ssView2_Sheet1.Cells[26, 5].Text.Trim() == "True" ? "1" : "0";
            strSeffect09 = ssView2_Sheet1.Cells[27, 5].Text.Trim() == "True" ? "1" : "0";
            strSeffect10 = ssView2_Sheet1.Cells[28, 5].Text.Trim() == "True" ? "1" : "0";

            strSeffect11 = ssView2_Sheet1.Cells[24, 8].Text.Trim() == "True" ? "1" : "0";
            strSeffect12 = ssView2_Sheet1.Cells[25, 8].Text.Trim() == "True" ? "1" : "0";
            strSeffect13 = ssView2_Sheet1.Cells[26, 8].Text.Trim() == "True" ? "1" : "0";
            strSeffect14 = ssView2_Sheet1.Cells[28, 9].Text.Trim();

            //교육일자
            strEduDate = ssView2_Sheet1.Cells[30, 2].Text.Trim();
            //교육회차
            if (ssView2_Sheet1.Cells[31, 2].Value != null)
            {
                strEduCount = ssView2_Sheet1.Cells[31, 2].Value.ToString();
            }
            else
            {
                strEduCount = "";
            }

            //교육소요시간
            strEduTime = ssView2_Sheet1.Cells[32, 2].Text.Trim();

            //교육제공대상
            if (ssView2_Sheet1.Cells[33, 2].Value != null)
            {
                strSuJect = ssView2_Sheet1.Cells[33, 2].Value.ToString();
            }
            else
            {
                strSuJect = "";
            }
            if (ssView2_Sheet1.Cells[33, 8].Value != null)
            {
                strSuJect1 = ssView2_Sheet1.Cells[33, 8].Value.ToString();
            }
            else
            {
                strSuJect1 = "";
            }

            strSuJect2 = ssView2_Sheet1.Cells[33, 6].Text.Trim();
            strSuJect3 = ssView2_Sheet1.Cells[33, 10].Text.Trim();

            //교육상담내용
            strEduContents01 = ssView2_Sheet1.Cells[34, 2].Text.Trim() == "True" ? "1" : "0";
            strEduContents02 = ssView2_Sheet1.Cells[35, 2].Text.Trim() == "True" ? "1" : "0";
            strEduContents03 = ssView2_Sheet1.Cells[36, 2].Text.Trim() == "True" ? "1" : "0";
            strEduContents04 = ssView2_Sheet1.Cells[37, 2].Text.Trim() == "True" ? "1" : "0";
            strEduContents05 = ssView2_Sheet1.Cells[38, 2].Text.Trim() == "True" ? "1" : "0";
            strEduContents06 = ssView2_Sheet1.Cells[39, 2].Text.Trim() == "True" ? "1" : "0";

            strEduContents07 = ssView2_Sheet1.Cells[34, 7].Text.Trim() == "True" ? "1" : "0";
            strEduContents08 = ssView2_Sheet1.Cells[35, 7].Text.Trim() == "True" ? "1" : "0";
            strEduContents09 = ssView2_Sheet1.Cells[36, 7].Text.Trim() == "True" ? "1" : "0";
            strEduContents10 = ssView2_Sheet1.Cells[37, 7].Text.Trim() == "True" ? "1" : "0";
            strEduContents11 = ssView2_Sheet1.Cells[39, 7].Text.Trim() == "True" ? "1" : "0";
            strEduContents12 = ssView2_Sheet1.Cells[40, 8].Text.Trim();

            //교육자료제공여부
            if (ssView2_Sheet1.Cells[41, 2].Value != null)
            {
                strEduMatPro = ssView2_Sheet1.Cells[41, 2].Value.ToString();
            }
            else
            {
                strEduMatPro = "";
            }

            //교육내용이해정도
            if (ssView2_Sheet1.Cells[42, 2].Value != null)
            {
                strEduEvende = ssView2_Sheet1.Cells[42, 2].Value.ToString();
            }
            else
            {
                strEduEvende = "";
            }

            //기타기재사항
            strRemark = ssView2_Sheet1.Cells[43, 2].Text.Trim();




            ////필수 입력 내용 점검
            //if (strGungu == "")
            //{
            //    ComFunc.MsgBox("산정특례구분을 반드시 체크해주십시오.");
            //    return rtnVal;
            //}
            //if (strJinInfo1 == "" && strJinInfo2 == "" && strJinInfo3 == "")
            //{
            //    ComFunc.MsgBox("진단정보를 반드시 체크해주십시오.");
            //    return rtnVal;
            //}
            //if (strIpdOpd == "")
            //{
            //    ComFunc.MsgBox("입원/외래 여부를 반드시 체크 요망합니다.");
            //    return rtnVal;
            //}
            //if (strSmokeGu == "")
            //{
            //    ComFunc.MsgBox("흡연 여부를 반드시 체크 요망합니다.");
            //    return rtnVal;
            //}
            //if (strAlcoholGu == "")
            //{
            //    ComFunc.MsgBox("음주 여부를 반드시 체크 요망합니다.");
            //    return rtnVal;
            //}
        
            //if (strDegreecom == "")
            //{
            //    ComFunc.MsgBox("복약순응도를 반드시 체크 요망합니다.");
            //    return rtnVal;
            //}
            //if (strSeffect == "")
            //{
            //    ComFunc.MsgBox("약제부작용 유/무를 반드시 체크 요망합니다.");
            //    return rtnVal;
            //}

            //if (strEduDate == "")
            //{
            //    ComFunc.MsgBox("교육일자를 반드시 입력 해주십시오.");
            //    return rtnVal;
            //}
            //if (strEduCount == "")
            //{
            //    ComFunc.MsgBox("교육회차를 반드시 입력 해주십시오.");
            //    return rtnVal;
            //}
            //if (strEduTime == "")
            //{
            //    ComFunc.MsgBox("교육소요시간을 반드시 입력 해주십시오.");
            //    return rtnVal;
            //}
            //if (strSuJect == "" && strSuJect1 == "")
            //{
            //    ComFunc.MsgBox("교육제공대상을 반드시 체크해주십시오.");
            //    return rtnVal;
            //}
            //if (strEduMatPro == "")
            //{
            //    ComFunc.MsgBox("교육자료제공여부를 반드시 체크 요망합니다.");
            //    return rtnVal;
            //}
            //if (strEduEvende == "")
            //{
            //    ComFunc.MsgBox("교육내용이해정도를 반드시 체크 요망합니다.");
            //    return rtnVal;
            //}

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (GstrROWID != "")
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "NUR_STD_INFECT10";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         ILLCODE = '" + strillcode + "', ";
                    SQL = SQL + ComNum.VBLF + "         M2_DISREG9 = '" + strGunno + "', ";
                    SQL = SQL + ComNum.VBLF + "         PATGU1 = '" + strJinInfo1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         PATGU2 = '" + strJinInfo2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         PATGU3 = '" + strJinInfo3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         PATGU4 = '" + strJinInfo4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         PATGU5 = '" + strJinInfo5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         IPDOPD = '" + strIpdOpd + "', ";
                    SQL = SQL + ComNum.VBLF + "         HEIGHT = '" + strHeight + "',  ";
                    SQL = SQL + ComNum.VBLF + "         WEIGHT = '" + strWeight + "', ";
                    SQL = SQL + ComNum.VBLF + "         BMI = '" + strBmi + "', ";
                    SQL = SQL + ComNum.VBLF + "         SMOKEGU = '" + strSmokeGu + "',  ";
                    SQL = SQL + ComNum.VBLF + "         ALCOHOLGU = '" + strAlcoholGu + "',  ";
                    SQL = SQL + ComNum.VBLF + "         DEGREECOM = '" + strDegreecom + "',  ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT = '" + strSeffect + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT01 = '" + strSeffect01 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT02 = '" + strSeffect02 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT03 = '" + strSeffect03 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT04 = '" + strSeffect04 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT05 = '" + strSeffect05 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT06 = '" + strSeffect06 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT07 = '" + strSeffect07 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT08 = '" + strSeffect08 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT09 = '" + strSeffect09 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT10 = '" + strSeffect10 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT11 = '" + strSeffect11 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT12 = '" + strSeffect12 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT13 = '" + strSeffect13 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEFFECT14 = '" + strSeffect14 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUDATE = TO_DATE('" + strEduDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         EDUCOUNT = '" + strEduCount + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUTIME = '" + strEduTime + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUSUJECT = '" + strSuJect + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUSUJECT1 = '" + strSuJect1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUSUJECT2= '" + strSuJect2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUSUJECT3 = '" + strSuJect3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS01 = '" + strEduContents01 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS02 = '" + strEduContents02 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS03 = '" + strEduContents03 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS04 = '" + strEduContents04 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS05 = '" + strEduContents05 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS06 = '" + strEduContents06 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS07 = '" + strEduContents07 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS08 = '" + strEduContents08 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS09 = '" + strEduContents09 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS10 = '" + strEduContents10 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS11 = '" + strEduContents11 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUCONTENTS12 = '" + strEduContents12 + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUMATPRO = '" + strEduMatPro + "', ";
                    SQL = SQL + ComNum.VBLF + "         EDUEVENDE = '" + strEduEvende + "', ";
                    SQL = SQL + ComNum.VBLF + "         BIGO1 = '" + strRemark + "', ";
                    SQL = SQL + ComNum.VBLF + "         ENTSABUN = '" + clsType.User.Sabun + "', ";
                    SQL = SQL + ComNum.VBLF + "         ENTDATE = SYSDATE";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "' ";
                }
                else
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_STD_INFECT10";
                    SQL = SQL + ComNum.VBLF + " (ACTDATE, PANO, SNAME, ILLCODE,  GUNGU, M2_DISREG9, PATGU1, PATGU2, PATGU3, PATGU4, PATGU5, IPDOPD, EDUCATOR, HEIGHT, WEIGHT, BMI, ";
                    SQL = SQL + ComNum.VBLF + "  SMOKEGU, ALCOHOLGU, DEGREECOM, SEFFECT, SEFFECT01, SEFFECT02, SEFFECT03, SEFFECT04, SEFFECT05, SEFFECT06, SEFFECT07, SEFFECT08, ";
                    SQL = SQL + ComNum.VBLF + " SEFFECT09, SEFFECT10, SEFFECT11, SEFFECT12, SEFFECT13, SEFFECT14, EDUDATE, EDUCOUNT, EDUTIME, EDUSUJECT, EDUSUJECT1, EDUSUJECT2, EDUSUJECT3, ";
                    SQL = SQL + ComNum.VBLF + " EDUCONTENTS01, EDUCONTENTS02, EDUCONTENTS03, EDUCONTENTS04, EDUCONTENTS05, EDUCONTENTS06, EDUCONTENTS07, EDUCONTENTS08, ";
                    SQL = SQL + ComNum.VBLF + " EDUCONTENTS09, EDUCONTENTS10, EDUCONTENTS11, EDUCONTENTS12, EDUMATPRO, EDUEVENDE, BIGO1, ENTDATE, ENTSABUN) ";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strActDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtPano.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSName + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strillcode + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGungu + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGunno + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strJinInfo1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strJinInfo2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strJinInfo3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strJinInfo4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strJinInfo5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strIpdOpd + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEducator + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHeight + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strWeight + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strBmi + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSmokeGu + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strAlcoholGu + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strDegreecom + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect01 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect02 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect03 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect04 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect05 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect06 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect07 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect08 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect09 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect10 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect11 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect12 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect13 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSeffect14 + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strEduDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduCount + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduTime + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSuJect + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSuJect1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSuJect2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSuJect3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents01 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents02 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents03 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents04 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents05 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents06 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents07 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents08 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents09 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents10 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents11 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduContents12 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduMatPro + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEduEvende + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strRemark + "', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "'";

                    SQL = SQL + ComNum.VBLF + "     )";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                ComFunc.MsgBox("저장 완료 하였습니다");

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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ssView2_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView2_Sheet1.PrintInfo.Margin.Top = 20;
            ssView2_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView2_Sheet1.PrintInfo.Margin.Header = 10;
            ssView2_Sheet1.PrintInfo.ShowColor = false;
            ssView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView2_Sheet1.PrintInfo.ShowBorder = false;
            ssView2_Sheet1.PrintInfo.ShowGrid = false;
            ssView2_Sheet1.PrintInfo.ShowShadows = false;
            ssView2_Sheet1.PrintInfo.UseMax = true;
            ssView2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView2_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView2_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView2_Sheet1.PrintInfo.Preview = false;
            ssView2.PrintSheet(0);
            btnDelete.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //ChkAutoSin.Checked = true;
                btnDelete.Enabled = false;
                if (txtPano.Text.Trim() == "") { return; }

                txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");
                lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPano.Text);

                if (lblSName.Text != "")
                {
                    GetData();
                    SCREEN_CLEAR();
                    Screen_display();
                }

            }
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //ChkAutoSin.Checked = false;

            if (e.ColumnHeader == true || e.RowHeader == true) { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            btnDelete.Enabled = true;

            if (GstrSaveGu != "") { btnDelete.Enabled = false; }

            GstrROWID = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();
            GdblIpdNO_OCS = VB.Val(ssList_Sheet1.Cells[e.Row, 4].Text.Trim());

            SCREEN_CLEAR();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.ACTDATE, A.PANO, A.SNAME, A.ILLCODE,  A.GUNGU, A.M2_DISREG9, A.PATGU1, A.PATGU2, A.PATGU3, A.PATGU4, A.PATGU5, A.IPDOPD, A.EDUCATOR, A.HEIGHT, A.WEIGHT, A.BMI, ";
                SQL = SQL + ComNum.VBLF + "     A.SMOKEGU, A.ALCOHOLGU, A.DEGREECOM, A.SEFFECT, A.SEFFECT01, A.SEFFECT02, A.SEFFECT03, A.SEFFECT04, A.SEFFECT05, A.SEFFECT06, A.SEFFECT07, A.SEFFECT08, ";
                SQL = SQL + ComNum.VBLF + "     A.SEFFECT09, A.SEFFECT10, A.SEFFECT11, A.SEFFECT12, A.SEFFECT13, A.SEFFECT14, A.EDUDATE, A.EDUCOUNT, A.EDUTIME, A.EDUSUJECT, A.EDUSUJECT1, A.EDUSUJECT2, A.EDUSUJECT3, ";
                SQL = SQL + ComNum.VBLF + "     A.EDUCONTENTS01, A.EDUCONTENTS02, A.EDUCONTENTS03, A.EDUCONTENTS04, A.EDUCONTENTS05, A.EDUCONTENTS06, A.EDUCONTENTS07, A.EDUCONTENTS08, ";
                SQL = SQL + ComNum.VBLF + "     A.EDUCONTENTS09, A.EDUCONTENTS10, A.EDUCONTENTS11, A.EDUCONTENTS12, A.EDUMATPRO, A.EDUEVENDE, A.BIGO1, ";
                SQL = SQL + ComNum.VBLF + "     (SELECT ILLNAMEE FROM ADMIN.BAS_ILLS WHERE ILLCODE = A.ILLCODE) AS ILLNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT10 A  ";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + GstrROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.Cells[5, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView2_Sheet1.Cells[5, 8].Text = clsAES.GstrAesJumin1 + "-" + clsAES.GstrAesJumin2;
                    ssView2_Sheet1.Cells[6, 2].Text = dt.Rows[0]["ILLNAME"].ToString().Trim();
                    ssView2_Sheet1.Cells[6, 8].Text = dt.Rows[0]["ILLCODE"].ToString().Trim();


                    ssView2_Sheet1.Cells[7, 2].Text = dt.Rows[0]["GUNGU"].ToString().Trim() == "" ? null : dt.Rows[0]["GUNGU"].ToString().Trim();
                    ssView2_Sheet1.Cells[7, 5].Text = dt.Rows[0]["M2_DISREG9"].ToString().Trim();

                    ssView2_Sheet1.Cells[8, 2].Text = dt.Rows[0]["ACTDATE"].ToString().Trim();

                    ssView2_Sheet1.Cells[10, 2].Value = dt.Rows[0]["PATGU1"].ToString().Trim() == "" ? null : dt.Rows[0]["PATGU1"].ToString().Trim();
                    ssView2_Sheet1.Cells[11, 2].Value = dt.Rows[0]["PATGU2"].ToString().Trim() == "" ? null : dt.Rows[0]["PATGU2"].ToString().Trim();
                    ssView2_Sheet1.Cells[11, 6].Value = dt.Rows[0]["PATGU4"].ToString().Trim() == "" ? null : dt.Rows[0]["PATGU4"].ToString().Trim();
                    ssView2_Sheet1.Cells[11, 10].Value = dt.Rows[0]["PATGU5"].ToString().Trim() == "" ? null : dt.Rows[0]["PATGU5"].ToString().Trim();
                    ssView2_Sheet1.Cells[12, 2].Value = dt.Rows[0]["PATGU3"].ToString().Trim() == "" ? null : dt.Rows[0]["PATGU3"].ToString().Trim();
                    ssView2_Sheet1.Cells[13, 2].Value = dt.Rows[0]["IPDOPD"].ToString().Trim() == "" ? null : dt.Rows[0]["IPDOPD"].ToString().Trim();

                    //ssView2_Sheet1.Cells[17, 2].Text = clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, dt.Rows[0]["EDUCATOR"].ToString().Trim());
                    //ssView2_Sheet1.Cells[17, 4].Text = clsVbfunc.GetOCSDoctorDRBUNHO(clsDB.DbCon, dt.Rows[0]["EDUCATOR"].ToString().Trim());
                    if (string.IsNullOrEmpty(clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, dt.Rows[0]["EDUCATOR"].ToString().Trim())) == true )
                    {
                        ssView2_Sheet1.Cells[17, 2].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["EDUCATOR"].ToString().Trim());
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[17, 2].Text = clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, dt.Rows[0]["EDUCATOR"].ToString().Trim());
                        ssView2_Sheet1.Cells[17, 4].Text = clsVbfunc.GetOCSDoctorDRBUNHO(clsDB.DbCon, dt.Rows[0]["EDUCATOR"].ToString().Trim());
                        ssView2_Sheet1.Cells[17, 6].Text = clsVbfunc.GetOCSDrDeptcodeSabun(clsDB.DbCon, dt.Rows[0]["EDUCATOR"].ToString().Trim());
                    }
                    
                    //
                    //
                    ssView2_Sheet1.Cells[19, 5].Value = dt.Rows[0]["HEIGHT"].ToString().Trim();
                    ssView2_Sheet1.Cells[19, 8].Value = dt.Rows[0]["WEIGHT"].ToString().Trim();
                    ssView2_Sheet1.Cells[19, 11].Value = dt.Rows[0]["BMI"].ToString().Trim();

                    ssView2_Sheet1.Cells[20, 4].Value = dt.Rows[0]["SMOKEGU"].ToString().Trim() == "" ? null : dt.Rows[0]["SMOKEGU"].ToString().Trim();
                    ssView2_Sheet1.Cells[21, 4].Value = dt.Rows[0]["ALCOHOLGU"].ToString().Trim() == "" ? null : dt.Rows[0]["ALCOHOLGU"].ToString().Trim();
                    ssView2_Sheet1.Cells[22, 2].Value = dt.Rows[0]["DEGREECOM"].ToString().Trim() == "" ? null : dt.Rows[0]["DEGREECOM"].ToString().Trim();
                    ssView2_Sheet1.Cells[23, 2].Value = dt.Rows[0]["SEFFECT"].ToString().Trim() == "" ? null : dt.Rows[0]["SEFFECT"].ToString().Trim();

                    ssView2_Sheet1.Cells[24, 2].Value = dt.Rows[0]["SEFFECT01"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[24, 5].Value = dt.Rows[0]["SEFFECT06"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[24, 8].Value = dt.Rows[0]["SEFFECT11"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[25, 2].Value = dt.Rows[0]["SEFFECT02"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[25, 5].Value = dt.Rows[0]["SEFFECT07"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[25, 8].Value = dt.Rows[0]["SEFFECT12"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[26, 2].Value = dt.Rows[0]["SEFFECT03"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[26, 5].Value = dt.Rows[0]["SEFFECT08"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[26, 8].Value = dt.Rows[0]["SEFFECT13"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[27, 2].Value = dt.Rows[0]["SEFFECT04"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[27, 5].Value = dt.Rows[0]["SEFFECT09"].ToString().Trim() == "1" ? true : false;
                    if (string.IsNullOrEmpty(dt.Rows[0]["SEFFECT14"].ToString().Trim()) == false)
                    {
                        ssView2_Sheet1.Cells[27, 8].Value = true;
                        ssView2_Sheet1.Cells[28, 9].Value = dt.Rows[0]["SEFFECT14"].ToString().Trim();
                    }

                    ssView2_Sheet1.Cells[28, 2].Value = dt.Rows[0]["SEFFECT05"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[28, 5].Value = dt.Rows[0]["SEFFECT10"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[30, 2].Value = dt.Rows[0]["EDUDATE"].ToString().Trim();
                    ssView2_Sheet1.Cells[31, 2].Value = dt.Rows[0]["EDUCOUNT"].ToString().Trim() == "" ? null : dt.Rows[0]["EDUCOUNT"].ToString().Trim();
                    ssView2_Sheet1.Cells[32, 2].Value = dt.Rows[0]["EDUTIME"].ToString().Trim();

                    ssView2_Sheet1.Cells[33, 2].Value = dt.Rows[0]["EDUSUJECT"].ToString().Trim() == "" ? null : dt.Rows[0]["EDUSUJECT"].ToString().Trim();
                    ssView2_Sheet1.Cells[33, 6].Value = dt.Rows[0]["EDUSUJECT2"].ToString().Trim();
                    ssView2_Sheet1.Cells[33, 8].Value = dt.Rows[0]["EDUSUJECT1"].ToString().Trim() == "" ? null : dt.Rows[0]["EDUSUJECT1"].ToString().Trim();
                    ssView2_Sheet1.Cells[33, 10].Value = dt.Rows[0]["EDUSUJECT3"].ToString().Trim();

                    ssView2_Sheet1.Cells[34, 2].Value = dt.Rows[0]["EDUCONTENTS01"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[34, 7].Value = dt.Rows[0]["EDUCONTENTS07"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[35, 2].Value = dt.Rows[0]["EDUCONTENTS02"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[35, 7].Value = dt.Rows[0]["EDUCONTENTS08"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[36, 2].Value = dt.Rows[0]["EDUCONTENTS03"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[36, 7].Value = dt.Rows[0]["EDUCONTENTS09"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[37, 2].Value = dt.Rows[0]["EDUCONTENTS04"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[37, 7].Value = dt.Rows[0]["EDUCONTENTS10"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[38, 2].Value = dt.Rows[0]["EDUCONTENTS05"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[39, 2].Value = dt.Rows[0]["EDUCONTENTS06"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[39, 7].Value = dt.Rows[0]["EDUCONTENTS11"].ToString().Trim() == "1" ? true : false;

                    if (string.IsNullOrEmpty(dt.Rows[0]["EDUCONTENTS12"].ToString().Trim()) == false)
                    {
                        ssView2_Sheet1.Cells[40, 7].Value = true;
                        ssView2_Sheet1.Cells[40, 8].Value = dt.Rows[0]["EDUCONTENTS12"].ToString().Trim();
                    }

                    ssView2_Sheet1.Cells[41, 2].Value = dt.Rows[0]["EDUMATPRO"].ToString().Trim() == "" ? null : dt.Rows[0]["EDUMATPRO"].ToString().Trim();
                    ssView2_Sheet1.Cells[42, 2].Value = dt.Rows[0]["EDUEVENDE"].ToString().Trim() == "" ? null : dt.Rows[0]["EDUEVENDE"].ToString().Trim();
                    ssView2_Sheet1.Cells[43, 2].Value = dt.Rows[0]["BIGO1"].ToString().Trim();


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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }



    }
}
