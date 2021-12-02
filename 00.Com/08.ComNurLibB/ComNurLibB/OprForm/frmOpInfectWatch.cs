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

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOrder
    /// File Name       : frmOpInfectWatch.cs
    /// Description     : 수술부위 감염 감시 기록지 서식
    /// Author          : 김욱동
    /// Create Date     : 2021-10-12
    /// <history> 
    /// 결핵환자 교육삼당료1 서식
    /// </summary>
    public partial class frmOpInfectWatch : Form
    {
        private string GstrPANO = "";
        private string GstrSname = "";
        private string GstrOptime = "";
        private string GstrAsaAdd= "";
        private string GstrOpreGubun = "";
        private string GstrErGubun = "";
        private string GstrDrcode = "";
        private string GstrDrSabun = "";
        private string GstrOpdate = "";
        private string GstrIllcode = "";
        private string GstrWrtno = "";
        private string GstrROWID = "";
        private double GdblIpdNO_OCS = 0;

        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();
        
        public delegate void SendText(string strText);
        public event SendText rSendText;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmOpInfectWatch()
        {
            InitializeComponent();
        }

        public frmOpInfectWatch(string strPANO, string strOpdate, string strOptime, string strDrcode, string strAsaAdd, string strErGubun, string strOpreGubun, string strWrtno)
        {
            InitializeComponent();
            GstrWrtno = strWrtno;
            GstrOptime = strOptime;
            
            if (strAsaAdd == "1") GstrAsaAdd = "0";
            if (strAsaAdd == "2") GstrAsaAdd = "1";
            if (strAsaAdd == "3") GstrAsaAdd = "2";
            if (strAsaAdd == "4") GstrAsaAdd = "3";
            if (strAsaAdd == "5") GstrAsaAdd = "4";
            
            if (strErGubun == "*") GstrErGubun = "0";
            if (strErGubun != "*") GstrErGubun = "1";

            if (strOpreGubun == "0") GstrOpreGubun = "1";
            if (strOpreGubun == "1") GstrOpreGubun = "0";

            GstrDrcode = strDrcode;
            GstrOpdate = strOpdate;
            GstrPANO = strPANO;
            
            
        }

        private void frmOpInfectWatch_Load(object sender, EventArgs e)
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

            //if (GstrSaveGu != "") { btnSave.Enabled = false; }

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

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DRCODE, DRNAME, DEPTCODE, DRBUNHO, SABUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "     WHERE DRCODE = '" + GstrDrcode + "' ";

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
                    ssView2_Sheet1.Cells[6, 3].Text = dt.Rows[0]["DRNAME"].ToString().Trim();

                    switch (dt.Rows[0]["DRNAME"].ToString().Trim())
                    {
                        case "하동엽":
                            ssView2_Sheet1.Cells[6, 7].Text = "1";
                            break;
                        case "손동녕":
                            ssView2_Sheet1.Cells[6, 7].Text = "2";
                            break;
                        case "서수한":
                            ssView2_Sheet1.Cells[6, 7].Text = "3";
                            break;
                        case "이효준":
                            ssView2_Sheet1.Cells[6, 7].Text = "5";
                            break;
                    }

                    GstrDrSabun = dt.Rows[0]["SABUN"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SNAME, PNAME, JUMIN1, JUMIN2 , JIKUP, SEX, TEL, HPHONE, GBFOREIGNER, A.BIRTH,";
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
                    GstrSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    //환자번호
                    ssView2_Sheet1.Cells[3, 4].Text = txtPano.Text;
                    //성별
                    ssView2_Sheet1.Cells[3, 10].Value = dt.Rows[0]["SEX"].ToString().Trim() == "M" ? "0" : "1";
                    //생년월일
                    ssView2_Sheet1.Cells[4, 10].Text = dt.Rows[0]["BIRTH"].ToString().Trim();
                    //수술일자
                    ssView2_Sheet1.Cells[4, 5].Text = GstrOpdate;
                    //AsaC
                    ssView2_Sheet1.Cells[14, 5].Value = GstrAsaAdd;
                    //수술소요시간
                    ssView2_Sheet1.Cells[15, 5].Text = GstrOptime;
                    //응급수술여부
                    ssView2_Sheet1.Cells[16, 7].Value = GstrErGubun;
                    //재수술여부
                    ssView2_Sheet1.Cells[17, 11].Value = GstrOpreGubun;

                }

                dt.Dispose();
                dt = null;

                SQL = "";
                
                SQL += " SELECT                                                                     \r\n";
                SQL += "   TO_CHAR(TO_DATE(MEDFRDATE, 'YYYYMMDD'), 'YYYY-MM-DD') MEDFRDATE                                                                  \r\n";
                SQL += "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST                                   \r\n";
                SQL += "   WHERE 1 = 1                                                              \r\n";
                SQL += "    AND Ptno = '" + txtPano.Text + "'                                        \r\n";
                SQL += "    AND FORMNO IN ('2618')                                           \r\n";
                SQL += "    AND TO_CHAR(TO_DATE(Chartdate,'YYYY-MM-DD'),'YYYY-MM-DD') = '" + ssView2_Sheet1.Cells[4, 5].Text.Trim() + "'                                      \r\n";
                SQL += "    ORDER BY Chartdate DESC                                             \r\n";

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
                    ssView2_Sheet1.Cells[5, 5].Text = dt.Rows[0]["MEDFRDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";

                SQL += " SELECT                                                                     \r\n";
                SQL += "   OUTDATE                                                                \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                   \r\n";
                SQL += "   WHERE 1 = 1                                                              \r\n";
                SQL += "    AND PANO = '" + txtPano.Text + "'                                        \r\n";
                SQL += "    AND INDATE = '" + ssView2_Sheet1.Cells[5, 5].Text.Trim() + "'                                        \r\n";
                SQL += "    ORDER BY INDATE DESC                                             \r\n";

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
                    ssView2_Sheet1.Cells[5, 10].Text = dt.Rows[0]["OUTDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                GetOptimes(GstrWrtno);

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
            ssView2_Sheet1.Cells[7, 3].Value = false;
            ssView2_Sheet1.Cells[7, 5].Value = false;
            ssView2_Sheet1.Cells[7, 8].Value = false;
            ssView2_Sheet1.Cells[7, 10].Value = false;

            ssView2_Sheet1.Cells[8, 3].Value = false;
            ssView2_Sheet1.Cells[8, 5].Value = null;
            ssView2_Sheet1.Cells[8, 8].Value = false;
            ssView2_Sheet1.Cells[8, 10].Value = null;

            ssView2_Sheet1.Cells[9, 3].Value = false;
            ssView2_Sheet1.Cells[9, 5].Value = false;
            ssView2_Sheet1.Cells[9, 8].Value = false;
            ssView2_Sheet1.Cells[9, 10].Value = false;

            ssView2_Sheet1.Cells[10, 3].Value = false;
            ssView2_Sheet1.Cells[10, 5].Value = false;
            ssView2_Sheet1.Cells[10, 8].Value = false;
            ssView2_Sheet1.Cells[10, 10].Value = false;

            ssView2_Sheet1.Cells[11, 3].Value = false;
            ssView2_Sheet1.Cells[11, 5].Value = false;
            ssView2_Sheet1.Cells[11, 8].Value = false;
            ssView2_Sheet1.Cells[11, 10].Value = false;

            ssView2_Sheet1.Cells[12, 3].Value = false;
            ssView2_Sheet1.Cells[12, 5].Value = null;
            ssView2_Sheet1.Cells[12, 10].Value = false;

            ssView2_Sheet1.Cells[13, 5].Value = null;

            //ssView2_Sheet1.Cells[16, 3].Value = null;
            //ssView2_Sheet1.Cells[16, 7].Value = null;
            ssView2_Sheet1.Cells[16, 11].Value = null;
            ssView2_Sheet1.Cells[17, 3].Value = null;
            //ssView2_Sheet1.Cells[17, 7].Value = null;
            //ssView2_Sheet1.Cells[17, 11].Value = null;
            ssView2_Sheet1.Cells[18, 3].Value = null;
            ssView2_Sheet1.Cells[18, 9].Value = null;
            ssView2_Sheet1.Cells[19, 3].Value = null;

            ssView2_Sheet1.Cells[19, 10].Value = null;
            ssView2_Sheet1.Cells[21, 3].Value = null;
            ssView2_Sheet1.Cells[22, 3].Value = null;
            ssView2_Sheet1.Cells[23, 6].Value = null;

            ssView2_Sheet1.Cells[25, 2].Value = false;
            ssView2_Sheet1.Cells[26, 2].Value = false;
            ssView2_Sheet1.Cells[27, 2].Value = false;
            ssView2_Sheet1.Cells[30, 2].Value = false;

            ssView2_Sheet1.Cells[31, 2].Value = false;
            ssView2_Sheet1.Cells[32, 2].Value = false;
            ssView2_Sheet1.Cells[35, 2].Value = false;

            ssView2_Sheet1.Cells[36, 2].Value = false;
            ssView2_Sheet1.Cells[37, 2].Value = false;
            ssView2_Sheet1.Cells[38, 2].Value = false;

            ssView2_Sheet1.Cells[39, 4].Value = null;


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
                if (ComFunc.MsgBoxQ("정말로 삭제하시겠습니까? 삭제한 기록지는 복구 불가능합니다.", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                try
                {


                    clsDB.setBeginTran(clsDB.DbCon);

                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_PMPA + "NUR_STD_INFECT13  WHERE ROWID = '" + GstrROWID + "'";

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
                ComFunc.MsgBox("선택된 기록지가 없습니다. 더블클릭하여 삭제하고자 하는 기록지를 선택하여 주세요.");
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
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME,  TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') AS ENTDATE , A.ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT13 A ";
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

        private void GetOptimes(string strWrtno)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";

                SQL += " SELECT ANGBN, OPTITLE, OPSTIME, OPETIME, PANO, SNAME, AGE, SEX   \r\n";
                SQL += "  FROM KOSMOS_PMPA.ORAN_MASTER                                                \r\n";
                SQL += "   WHERE 1 = 1                                                             \r\n";
                SQL += "    AND WRTNO = '" + strWrtno + "'                                    \r\n";

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

                    //if(VB.Left(dt.Rows[0]["OPTITLE"].ToString().Trim(), 3) == "lap" || VB.Left(dt.Rows[0]["OPTITLE"].ToString().Trim(), 3) == "Lap")
                    ssView2_Sheet1.Cells[3, 0].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssView2_Sheet1.Cells[4, 0].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView2_Sheet1.Cells[5, 0].Text = dt.Rows[0]["AGE"].ToString().Trim() + "/" + dt.Rows[0]["SEX"].ToString().Trim();

                    ssView2_Sheet1.Cells[15, 7].Text = dt.Rows[0]["OPSTIME"].ToString().Trim() + " ~ " + dt.Rows[0]["OPETIME"].ToString().Trim();
                    ssView2_Sheet1.Cells[16, 3].Value = VB.Left(dt.Rows[0]["OPTITLE"].ToString().Trim(), 3) == "lap" || VB.Left(dt.Rows[0]["OPTITLE"].ToString().Trim(), 3) == "Lap" ? "0" : "1";
                    ssView2_Sheet1.Cells[17, 7].Value = dt.Rows[0]["ANGBN"].ToString().Trim() == "G" ? "0" : "1";

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
            string strActDate = "";
            string strOpDate = "";
            string strAdmDate = "";
            string strDisDate = "";

            string strDrSabun = "";
            string strOpType1 = "";
            string strOpType2 = "";
            string strOpType3 = "";
            string strOpType4 = "";
            string strOpType5 = "";
            string strOpType5_1 = "";
            string strOpType6 = "";
            string strOpType6_1 = "";
            string strOpType7 = "";
            string strOpType8 = "";
            string strOpType9 = "";
            string strOpType10 = "";
            string strOpType11= "";
            string strOpType11_1 = "";
            string strOpType12 = "";
            string strOpType13= "";
            string strOpType14 = "";
            string strOpType15 = "";
            string strOpType16 = "";
            string strOpType17 = "";
            string strOpType18 = "";
            string strOpType19 = "";
            string strOpType19_1 = "";
            string strOpType20 = "";
            string strWounDc = "";
            string strAsaC = "";
            string strOptime = "";
            string strEndoSur = "";
            string strErOp= "";
            string strTrauma = "";
            string strMultiPro = "";
            string strGeneralAnes = "";
            string strReOper = "";
            string strSsiTrack = "";
            string strSsiOccur = "";
            string strSsiDate = "";
            string strTracks = "";
            string strLtrackDate = "";
            string strSsiDegree = "";


            string strSsiCr1 = "";
            string strSsiCr2 = "";
            string strSsiCr3 = "";
            string strSsiCr4 = "";
            string strSsiCr5 = "";
            string strSsiCr6 = "";
            string strSsiCr7 = "";
            string strSsiCr8 = "";
            string strSsiCr9 = "";
            string strSsiCr10 = "";
            string strPaiDen = "";

            string strBigo1 = "";
            


            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록 환자의 등록번호를 조회 후 작업해주세요.");
                return rtnVal;
            }

            //등록일자
            strActDate = cpublic.strSysDate;
            //이름
            strSName = GstrSname;
            //수술일자
            strOpDate = ssView2_Sheet1.Cells[4, 5].Text.Trim();
            strAdmDate = ssView2_Sheet1.Cells[5, 5].Text.Trim();
            strDisDate = ssView2_Sheet1.Cells[5, 10].Text.Trim();

            //수술의사
            strDrSabun = GstrDrSabun;

            strOpType1 = ssView2_Sheet1.Cells[7, 3].Text.Trim() == "True" ? "1" : "0";
            strOpType2 = ssView2_Sheet1.Cells[7, 5].Text.Trim() == "True" ? "1" : "0";
            strOpType3 = ssView2_Sheet1.Cells[7, 8].Text.Trim() == "True" ? "1" : "0";
            strOpType4 = ssView2_Sheet1.Cells[7, 10].Text.Trim() == "True" ? "1" : "0";

            strOpType5 = ssView2_Sheet1.Cells[8, 3].Text.Trim() == "True" ? "1" : "0";
            //KPRO GB
            if (ssView2_Sheet1.Cells[8, 5].Value != null)
            {
                strOpType5_1 = ssView2_Sheet1.Cells[8, 5].Value.ToString();
            }
            else
            {
                strOpType5_1 = "";
            }
            strOpType6 = ssView2_Sheet1.Cells[8, 8].Text.Trim() == "True" ? "1" : "0";
            //HPRO GB
            if (ssView2_Sheet1.Cells[8, 10].Value != null)
            {
                strOpType6_1 = ssView2_Sheet1.Cells[8, 10].Value.ToString();
            }
            else
            {
                strOpType6_1 = "";
            }


            strOpType7 = ssView2_Sheet1.Cells[9, 3].Text.Trim() == "True" ? "1" : "0";
            strOpType8 = ssView2_Sheet1.Cells[9, 5].Text.Trim() == "True" ? "1" : "0";
            strOpType9 = ssView2_Sheet1.Cells[9, 8].Text.Trim() == "True" ? "1" : "0";
            strOpType10 = ssView2_Sheet1.Cells[9, 10].Text.Trim() == "True" ? "1" : "0";

            strOpType11 = ssView2_Sheet1.Cells[10, 3].Text.Trim() == "True" ? "1" : "0";
            //CBGB GB
            if (ssView2_Sheet1.Cells[23, 6].Value != null)
            {
                strOpType11_1 = ssView2_Sheet1.Cells[23, 6].Value.ToString();
            }
            else
            {
                strOpType11_1 = "";
            }

            strOpType12 = ssView2_Sheet1.Cells[10, 5].Text.Trim() == "True" ? "1" : "0";
            strOpType13 = ssView2_Sheet1.Cells[10, 8].Text.Trim() == "True" ? "1" : "0";
            strOpType14 = ssView2_Sheet1.Cells[10, 10].Text.Trim() == "True" ? "1" : "0";

            strOpType15 = ssView2_Sheet1.Cells[11, 3].Text.Trim() == "True" ? "1" : "0";
            strOpType16 = ssView2_Sheet1.Cells[11, 5].Text.Trim() == "True" ? "1" : "0";
            strOpType17 = ssView2_Sheet1.Cells[11, 8].Text.Trim() == "True" ? "1" : "0";
            strOpType18 = ssView2_Sheet1.Cells[11, 10].Text.Trim() == "True" ? "1" : "0";

            strOpType19 = ssView2_Sheet1.Cells[12, 3].Text.Trim() == "True" ? "1" : "0";
            //APPY GB
            if (ssView2_Sheet1.Cells[12, 5].Value != null)
            {
                strOpType19_1 = ssView2_Sheet1.Cells[12, 5].Value.ToString();
            }
            else
            {
                strOpType19_1 = "";
            }

            strOpType20 = ssView2_Sheet1.Cells[12, 10].Text.Trim() == "True" ? "1" : "0";

            //WoundC
            if (ssView2_Sheet1.Cells[13, 5].Value != null)
            {
                strWounDc = ssView2_Sheet1.Cells[13, 5].Value.ToString();
            }
            else
            {
                strWounDc = "";
            }

            //AsaC
            if (ssView2_Sheet1.Cells[14, 5].Value != null)
            {
                strAsaC = ssView2_Sheet1.Cells[14, 5].Value.ToString();
            }
            else
            {
                strAsaC = "";
            }

            //수술시간
            strOptime = ssView2_Sheet1.Cells[15, 5].Value.ToString();

            //EndoSur
            if (ssView2_Sheet1.Cells[16, 3].Value != null)
            {
                strEndoSur = ssView2_Sheet1.Cells[16, 3].Value.ToString();
            }
            else
            {
                strEndoSur = "";
            }

            //ErOp
            if (ssView2_Sheet1.Cells[16, 7].Value != null)
            {
                strErOp = ssView2_Sheet1.Cells[16, 7].Value.ToString();
            }
            else
            {
                strErOp = "";
            }

            //Trauma
            if (ssView2_Sheet1.Cells[16, 11].Value != null)
            {
                strTrauma = ssView2_Sheet1.Cells[16, 11].Value.ToString();
            }
            else
            {
                strTrauma = "";
            }

            //MultiPro
            if (ssView2_Sheet1.Cells[17, 3].Value != null)
            {
                strMultiPro = ssView2_Sheet1.Cells[17, 3].Value.ToString();
            }
            else
            {
                strMultiPro = "";
            }

            //GeneralAnes
            if (ssView2_Sheet1.Cells[17, 7].Value != null)
            {
                strGeneralAnes = ssView2_Sheet1.Cells[17, 7].Value.ToString();
            }
            else
            {
                strGeneralAnes = "";
            }

            //Reoper
            if (ssView2_Sheet1.Cells[17, 11].Value != null)
            {
                strReOper = ssView2_Sheet1.Cells[17, 11].Value.ToString();
            }
            else
            {
                strReOper = "";
            }

            //SsiTrack
            if (ssView2_Sheet1.Cells[18, 3].Value != null)
            {
                strSsiTrack = ssView2_Sheet1.Cells[18, 3].Value.ToString();
            }
            else
            {
                strSsiTrack = "";
            }

            //SsiOccur
            if (ssView2_Sheet1.Cells[18, 9].Value != null)
            {
                strSsiOccur = ssView2_Sheet1.Cells[18, 9].Value.ToString();
            }
            else
            {
                strSsiOccur = "";
            }

            //추적중단사유
            if (ssView2_Sheet1.Cells[19, 3].Value != null)
            {
                strTracks = ssView2_Sheet1.Cells[19, 3].Value.ToString();
            }
            else
            {
                strTracks = "";
            }

            //마지막추적일
            strLtrackDate = ssView2_Sheet1.Cells[19, 10].Text.Trim();
            //SSI발생일
            strSsiDate = ssView2_Sheet1.Cells[21, 3].Text.Trim();

            //SsiDegree
            if (ssView2_Sheet1.Cells[22, 3].Value != null)
            {
                strSsiDegree = ssView2_Sheet1.Cells[22, 3].Value.ToString();
            }
            else
            {
                strSsiDegree = "";
            }

            //SSI Criteria
            strSsiCr1 = ssView2_Sheet1.Cells[25, 2].Text.Trim() == "True" ? "1" : "0";
            strSsiCr2 = ssView2_Sheet1.Cells[26, 2].Text.Trim() == "True" ? "1" : "0";
            strSsiCr3 = ssView2_Sheet1.Cells[27, 2].Text.Trim() == "True" ? "1" : "0";
            strSsiCr4 = ssView2_Sheet1.Cells[30, 2].Text.Trim() == "True" ? "1" : "0";
            strSsiCr5 = ssView2_Sheet1.Cells[31, 2].Text.Trim() == "True" ? "1" : "0";
            strSsiCr6 = ssView2_Sheet1.Cells[32, 2].Text.Trim() == "True" ? "1" : "0";
            strSsiCr7 = ssView2_Sheet1.Cells[35, 2].Text.Trim() == "True" ? "1" : "0";
            strSsiCr8 = ssView2_Sheet1.Cells[36, 2].Text.Trim() == "True" ? "1" : "0";
            strSsiCr9 = ssView2_Sheet1.Cells[37, 2].Text.Trim() == "True" ? "1" : "0";
            strSsiCr10 = ssView2_Sheet1.Cells[38, 2].Text.Trim() == "True" ? "1" : "0";

            //Paiden
            if (ssView2_Sheet1.Cells[39, 4].Value != null)
            {
                strPaiDen = ssView2_Sheet1.Cells[39, 4].Value.ToString();
            }
            else
            {
                strPaiDen = "";
            }

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
                    SQL = "UPDATE " + ComNum.DB_PMPA + "NUR_STD_INFECT13";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         ACTDATE = TO_DATE('" + strActDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         OPDATE = TO_DATE('" + strOpDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         ADMDATE = TO_DATE('" + strAdmDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         DISDATE = TO_DATE('" + strDisDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         DRSABUN = '" + strDrSabun + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE1 = '" + strOpType1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE2 = '" + strOpType2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE3 = '" + strOpType3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE4 = '" + strOpType4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE5 = '" + strOpType5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE5_1 = '" + strOpType5_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE6 = '" + strOpType6 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE6_1 = '" + strOpType6_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE7 = '" + strOpType7 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE8 = '" + strOpType8 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE9 = '" + strOpType9 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE10 = '" + strOpType10 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE11 = '" + strOpType11 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE11_1 = '" + strOpType11_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE12 = '" + strOpType12 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE13 = '" + strOpType13 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE14 = '" + strOpType14 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE15 = '" + strOpType15 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE16 = '" + strOpType16 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE17 = '" + strOpType17 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE18 = '" + strOpType18 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE19 = '" + strOpType19 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE19_1 = '" + strOpType19_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTYPE20 = '" + strOpType20 + "', ";
                    SQL = SQL + ComNum.VBLF + "         WOUNDC = '" + strWounDc + "', ";
                    SQL = SQL + ComNum.VBLF + "         ASAC = '" + strAsaC + "', ";
                    SQL = SQL + ComNum.VBLF + "         OPTIME = '" + strOptime + "', ";
                    SQL = SQL + ComNum.VBLF + "         ENDOSUR = '" + strEndoSur + "', ";
                    SQL = SQL + ComNum.VBLF + "         EROP = '" + strErOp + "', ";
                    SQL = SQL + ComNum.VBLF + "         TRAUMA = '" + strTrauma + "', ";
                    SQL = SQL + ComNum.VBLF + "         MULTIPRO = '" + strMultiPro + "', ";
                    SQL = SQL + ComNum.VBLF + "         GENERALANES = '" + strGeneralAnes + "', ";
                    SQL = SQL + ComNum.VBLF + "         REOPER = '" + strReOper + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSITRACK = '" + strSsiTrack + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSIOCCUR = '" + strSsiOccur + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSIDATE = TO_DATE('" + strSsiDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         TRACKS = '" + strTracks + "', ";
                    SQL = SQL + ComNum.VBLF + "         LTRACKDATE = TO_DATE('" + strLtrackDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         SSIDEGREE = '" + strSsiDegree + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSICR1 = '" + strSsiCr1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSICR2 = '" + strSsiCr2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSICR3 = '" + strSsiCr3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSICR4 = '" + strSsiCr4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSICR5 = '" + strSsiCr5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSICR6 = '" + strSsiCr6 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSICR7 = '" + strSsiCr7 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSICR8 = '" + strSsiCr8 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSICR9 = '" + strSsiCr9 + "', ";
                    SQL = SQL + ComNum.VBLF + "         SSICR10 = '" + strSsiCr10 + "', ";
                    SQL = SQL + ComNum.VBLF + "         PAIDEN = '" + strPaiDen + "', ";
                    SQL = SQL + ComNum.VBLF + "         ENTSABUN = '" + clsType.User.Sabun + "', ";
                    SQL = SQL + ComNum.VBLF + "         ENTDATE = SYSDATE";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "' ";
                }
                else
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_STD_INFECT13";
                    SQL = SQL + ComNum.VBLF + " (ACTDATE, PANO, SNAME, OPDATE,  ADMDATE, DISDATE, DRSABUN, OPTYPE1, OPTYPE2, OPTYPE3, OPTYPE4, OPTYPE5, OPTYPE6, OPTYPE7, OPTYPE8, ";
                    SQL = SQL + ComNum.VBLF + "  OPTYPE9, OPTYPE10, OPTYPE11, OPTYPE12, OPTYPE13, OPTYPE14, OPTYPE15, OPTYPE16, OPTYPE17, OPTYPE18, OPTYPE19, OPTYPE20, ";
                    SQL = SQL + ComNum.VBLF + " OPTYPE5_1, OPTYPE6_1, OPTYPE11_1, OPTYPE19_1, WOUNDC, ASAC, OPTIME, ENDOSUR, EROP, TRAUMA, MULTIPRO, GENERALANES, REOPER, ";
                    SQL = SQL + ComNum.VBLF + " SSITRACK, SSIOCCUR, SSIDATE, TRACKS, LTRACKDATE, SSIDEGREE, SSICR1,SSICR2,SSICR3,SSICR4,SSICR5,SSICR6,SSICR7,SSICR8,SSICR9,SSICR10, ";
                    SQL = SQL + ComNum.VBLF + " PAIDEN, BIGO1, ENTDATE, ENTSABUN) ";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strActDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtPano.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSName + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strOpDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strAdmDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDisDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + strDrSabun + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType6 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType7 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType8 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType9 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType10 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType11 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType12 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType13+ "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType14 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType15 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType16 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType17 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType18 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType19 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType20 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType5_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType6_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType11_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOpType19_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strWounDc + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strAsaC + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strOptime + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEndoSur + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strErOp + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strTrauma + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMultiPro + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGeneralAnes + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strReOper + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiTrack + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiOccur + "', ";
                    SQL = SQL + ComNum.VBLF + "          TO_DATE('" + strSsiDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + strTracks + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strLtrackDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiDegree + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiCr1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiCr2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiCr3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiCr4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiCr5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiCr6 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiCr7 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiCr8 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiCr9 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSsiCr10 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPaiDen + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrWrtno + "', ";
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
            //ssView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Both;
            ssView2_Sheet1.PrintInfo.ShowBorder = false;
            ssView2_Sheet1.PrintInfo.ShowGrid = false;
            ssView2_Sheet1.PrintInfo.ShowShadows = false;
            ssView2_Sheet1.PrintInfo.UseMax = true;
            ssView2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView2_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView2_Sheet1.PrintInfo.ZoomFactor = 0.9f;
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

            //if (GstrSaveGu != "") { btnDelete.Enabled = false; }
            
            GstrROWID = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();
            GdblIpdNO_OCS = VB.Val(ssList_Sheet1.Cells[e.Row, 4].Text.Trim());

            SCREEN_CLEAR();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.ACTDATE, A.PANO, A.SNAME, A.OPDATE,  A.ADMDATE, A.DISDATE, A.DRSABUN, A.OPTYPE1,A.OPTYPE2,A.OPTYPE3,A.OPTYPE4,A.OPTYPE5,A.OPTYPE6,A.OPTYPE7,A.OPTYPE8,A.OPTYPE9,";
                SQL = SQL + ComNum.VBLF + "     A.OPTYPE10,A.OPTYPE11,A.OPTYPE12,A.OPTYPE13,A.OPTYPE14,A.OPTYPE15,A.OPTYPE16,A.OPTYPE17,A.OPTYPE18,A.OPTYPE19,A.OPTYPE20,A.OPTYPE5_1,A.OPTYPE6_1,A.OPTYPE11_1,A.OPTYPE19_1,";
                SQL = SQL + ComNum.VBLF + "     A.WOUNDC, A.ASAC, A.OPTIME, A.ENDOSUR, A.EROP, A.TRAUMA, A.MULTIPRO, A.GENERALANES, A.REOPER, A.SSITRACK, A.SSIOCCUR, A.SSIDATE, A.TRACKS, ";
                SQL = SQL + ComNum.VBLF + "     A.LTRACKDATE, A.SSIDEGREE, A.SSICR1, A.SSICR2, A.SSICR3, A.SSICR4, A.SSICR5, A.SSICR6, A.SSICR7, A.SSICR8, A.SSICR9, A.SSICR10,";
                SQL = SQL + ComNum.VBLF + "     A.PAIDEN, A.BIGO1";
                //SQL = SQL + ComNum.VBLF + "     (SELECT ILLNAMEE FROM KOSMOS_PMPA.BAS_ILLS WHERE ILLCODE = A.ILLCODE) AS ILLNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT13 A  ";
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
                    ssView2_Sheet1.Cells[3, 4].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssView2_Sheet1.Cells[4, 5].Text = dt.Rows[0]["OPDATE"].ToString().Trim();
                    ssView2_Sheet1.Cells[5, 5].Text = dt.Rows[0]["ADMDATE"].ToString().Trim();
                    ssView2_Sheet1.Cells[5, 10].Text = dt.Rows[0]["DISDATE"].ToString().Trim();

                    ssView2_Sheet1.Cells[6, 3].Text = clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());
                    //ssView2_Sheet1.Cells[5, 8].Text = clsAES.GstrAesJumin1 + "-" + clsAES.GstrAesJumin2;
                    switch (ssView2_Sheet1.Cells[6, 3].Text.Trim())
                    {
                        case "하동엽":
                            ssView2_Sheet1.Cells[6, 7].Text = "1";
                            break;
                        case "손동녕":
                            ssView2_Sheet1.Cells[6, 7].Text = "2";
                            break;
                        case "서수한":
                            ssView2_Sheet1.Cells[6, 7].Text = "3";
                            break;
                        case "이효준":
                            ssView2_Sheet1.Cells[6, 7].Text = "5";
                            break;
                    }
                    ssView2_Sheet1.Cells[7, 3].Value = dt.Rows[0]["OPTYPE1"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[7, 5].Value = dt.Rows[0]["OPTYPE2"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[7, 8].Value = dt.Rows[0]["OPTYPE3"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[7, 10].Value = dt.Rows[0]["OPTYPE4"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[8, 3].Value = dt.Rows[0]["OPTYPE5"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[8, 5].Value = dt.Rows[0]["OPTYPE5_1"].ToString().Trim() == "" ? null : dt.Rows[0]["OPTYPE5_1"].ToString().Trim();
                    ssView2_Sheet1.Cells[8, 8].Value = dt.Rows[0]["OPTYPE6"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[8, 10].Value = dt.Rows[0]["OPTYPE6_1"].ToString().Trim() == "" ? null : dt.Rows[0]["OPTYPE6_1"].ToString().Trim();

                    ssView2_Sheet1.Cells[9, 3].Value = dt.Rows[0]["OPTYPE7"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[9, 5].Value = dt.Rows[0]["OPTYPE8"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[9, 8].Value = dt.Rows[0]["OPTYPE9"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[9, 10].Value = dt.Rows[0]["OPTYPE10"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[10, 3].Value = dt.Rows[0]["OPTYPE11"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[10, 5].Value = dt.Rows[0]["OPTYPE12"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[10, 8].Value = dt.Rows[0]["OPTYPE13"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[10, 10].Value = dt.Rows[0]["OPTYPE14"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[11, 3].Value = dt.Rows[0]["OPTYPE15"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[11, 5].Value = dt.Rows[0]["OPTYPE16"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[11, 8].Value = dt.Rows[0]["OPTYPE17"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[11, 10].Value = dt.Rows[0]["OPTYPE18"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[12, 3].Value = dt.Rows[0]["OPTYPE19"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[12, 5].Value = dt.Rows[0]["OPTYPE19_1"].ToString().Trim() == "" ? null : dt.Rows[0]["OPTYPE19_1"].ToString().Trim();
                    ssView2_Sheet1.Cells[12, 10].Value = dt.Rows[0]["OPTYPE20"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[13, 5].Value = dt.Rows[0]["WOUNDC"].ToString().Trim() == "" ? null : dt.Rows[0]["WOUNDC"].ToString().Trim();
                    ssView2_Sheet1.Cells[14, 5].Value = dt.Rows[0]["ASAC"].ToString().Trim() == "" ? null : dt.Rows[0]["ASAC"].ToString().Trim();

                    ssView2_Sheet1.Cells[15, 5].Value = dt.Rows[0]["OPTIME"].ToString().Trim();

                    GetOptimes(dt.Rows[0]["BIGO1"].ToString().Trim());

                    ssView2_Sheet1.Cells[16, 3].Value = dt.Rows[0]["ENDOSUR"].ToString().Trim() == "" ? null : dt.Rows[0]["ENDOSUR"].ToString().Trim();
                    ssView2_Sheet1.Cells[16, 7].Value = dt.Rows[0]["EROP"].ToString().Trim() == "" ? null : dt.Rows[0]["EROP"].ToString().Trim();
                    ssView2_Sheet1.Cells[16, 11].Value = dt.Rows[0]["TRAUMA"].ToString().Trim() == "" ? null : dt.Rows[0]["TRAUMA"].ToString().Trim();
                    ssView2_Sheet1.Cells[17, 3].Value = dt.Rows[0]["MULTIPRO"].ToString().Trim() == "" ? null : dt.Rows[0]["MULTIPRO"].ToString().Trim();
                    ssView2_Sheet1.Cells[17, 7].Value = dt.Rows[0]["GENERALANES"].ToString().Trim() == "" ? null : dt.Rows[0]["GENERALANES"].ToString().Trim();
                    ssView2_Sheet1.Cells[17, 11].Value = dt.Rows[0]["REOPER"].ToString().Trim() == "" ? null : dt.Rows[0]["REOPER"].ToString().Trim();
                    ssView2_Sheet1.Cells[18, 3].Value = dt.Rows[0]["SSITRACK"].ToString().Trim() == "" ? null : dt.Rows[0]["SSITRACK"].ToString().Trim();
                    ssView2_Sheet1.Cells[18, 9].Value = dt.Rows[0]["SSIOCCUR"].ToString().Trim() == "" ? null : dt.Rows[0]["SSIOCCUR"].ToString().Trim();
                    ssView2_Sheet1.Cells[19, 3].Value = dt.Rows[0]["TRACKS"].ToString().Trim() == "" ? null : dt.Rows[0]["TRACKS"].ToString().Trim();

                    ssView2_Sheet1.Cells[19, 10].Value = dt.Rows[0]["LTRACKDATE"].ToString().Trim();
                    ssView2_Sheet1.Cells[21, 3].Value = dt.Rows[0]["SSIDATE"].ToString().Trim();
                    ssView2_Sheet1.Cells[22, 3].Value = dt.Rows[0]["SSIDEGREE"].ToString().Trim() == "" ? null : dt.Rows[0]["SSIDEGREE"].ToString().Trim();
                    ssView2_Sheet1.Cells[23, 6].Value = dt.Rows[0]["OPTYPE11_1"].ToString().Trim() == "" ? null : dt.Rows[0]["OPTYPE11_1"].ToString().Trim();

                    ssView2_Sheet1.Cells[25, 2].Value = dt.Rows[0]["SSICR1"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[26, 2].Value = dt.Rows[0]["SSICR2"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[27, 2].Value = dt.Rows[0]["SSICR3"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[30, 2].Value = dt.Rows[0]["SSICR4"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[31, 2].Value = dt.Rows[0]["SSICR5"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[32, 2].Value = dt.Rows[0]["SSICR6"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[35, 2].Value = dt.Rows[0]["SSICR7"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[36, 2].Value = dt.Rows[0]["SSICR8"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[37, 2].Value = dt.Rows[0]["SSICR9"].ToString().Trim() == "1" ? true : false;
                    ssView2_Sheet1.Cells[38, 2].Value = dt.Rows[0]["SSICR10"].ToString().Trim() == "1" ? true : false;

                    ssView2_Sheet1.Cells[39, 4].Value = dt.Rows[0]["PAIDEN"].ToString().Trim() == "" ? null : dt.Rows[0]["PAIDEN"].ToString().Trim();
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

