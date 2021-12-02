using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;


namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewDeptView.cs
    /// Description     : 당일과별수진자조회
    /// Author          : 안정수
    /// Create Date     : 2017-07-06
    /// Update History  : 2017-11-03 
    /// <history>       
    /// d:\psmh\OPD\oumsad\OUMSAD17.FRM(FrmDeptView) => frmPmpaViewDeptView.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\OUMSAD17.FRM(FrmDeptView)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewDeptView : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");        
        string CureentTime = DateTime.Now.ToString("HH:mm:ss");

        string mstrFlagGam = "";
        string mstrGamEnd = "";
        string mstrGamMsg = "";
        string mstrGamGubun = "";

        string mstrJobPart = "";

        int FnRow = 0;
        int FnCol = 0;
        int nRow = 0;

        public frmPmpaViewDeptView()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewDeptView(string GstrJobPart)
        {
            InitializeComponent();
            setEvent();
            mstrJobPart = GstrJobPart;
        }

        public frmPmpaViewDeptView(string GstrFlagGam, string GstrGamEnd, string GstrGamMsg)
        {
            InitializeComponent();
            setEvent();
            mstrFlagGam = GstrFlagGam;
            mstrGamEnd = GstrGamEnd;
            mstrGamMsg = GstrGamMsg;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등 

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Load_DeptCode();
            
            if (mstrJobPart != "" || mstrJobPart != null)
            {
                txtPart.Text = mstrJobPart;
            }

            else
            {
                txtPart.Focus();
            }
        }

        void Load_DeptCode()
        {
            int i = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            for (i = 0; i < 50; i++)
            {
                clsPmpaPb.GstrSetDeptCodes[i] = "";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                ";
            SQL += ComNum.VBLF + " * FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT          ";
            SQL += ComNum.VBLF + "WHERE GBJUPSU   = '1' ";
            SQL += ComNum.VBLF + "Order By Printranking                                 ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        clsPmpaPb.GstrSetDeptCodes[i] = dt.Rows[i]["DeptCode"].ToString().Trim();
                        clsPmpaPb.GstrSetDepts[i] = dt.Rows[i]["DeptNameK"].ToString().Trim();
                        cboDept.Items.Add(clsPmpaPb.GstrSetDeptCodes[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
            cboDept.SelectedIndex = 0;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }
        }

        void Clear_SS()
        {
            ssList1_Sheet1.Cells[0, 1, ssList1_Sheet1.Rows.Count - 1, 2].Text = "";
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            string strDept = "";
            string strDate = "";

            string strSname = "";
            string strPano = "";
            string strChoJae = "";
            string strDrname = "";
            string strAmt77 = "";
            string strAmt7 = "";
            string strField = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList2_Sheet1.Rows.Count = 0;

            Clear_SS();

            FnRow = 1;

            clsPublic.GstrActDate = CurrentDate;

            strDate = clsPublic.GstrActDate;
            cboDept.Text = cboDept.Text.ToUpper();
            strDept = cboDept.Text.ToString();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                    ";
            SQL += ComNum.VBLF + "  Sname,Pano,Chojae,DrName,Amt7                                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER M," + ComNum.DB_PMPA + "BAS_DOCTOR D";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate  = TO_DATE('" + strDate + "','YYYY-MM-DD')              ";
            SQL += ComNum.VBLF + "      AND DeptCode = '" + strDept + "'                                    ";
            if (txtPart.Text != "")
            {
                SQL += ComNum.VBLF + "      AND Part     = '" + txtPart.Text + "'                               ";
            }
            SQL += ComNum.VBLF + "      AND M.DrCode = D.DrCode(+)                                          ";
            SQL += ComNum.VBLF + "ORDER BY Sname                                                            ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList2_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPano = dt.Rows[i]["Pano"].ToString().Trim();
                        strAmt7 = dt.Rows[i]["Amt7"].ToString().Trim();
                        strSname = dt.Rows[i]["Sname"].ToString().Trim();
                        strDrname = dt.Rows[i]["DrName"].ToString().Trim();
                        strChoJae = clsPmpaPb.GstrSetChoJaes[Convert.ToInt32(VB.Val(dt.Rows[i]["Chojae"].ToString().Trim()))];

                        strAmt77 = VB.Space(5 - VB.Len(strAmt7)) + strAmt7 + "  ";

                        ssList2_Sheet1.Cells[i, 0].Text = strSname;
                        ssList2_Sheet1.Cells[i, 1].Text = strPano;
                        ssList2_Sheet1.Cells[i, 2].Text = strChoJae;
                        ssList2_Sheet1.Cells[i, 3].Text = strDrname;
                        ssList2_Sheet1.Cells[i, 4].Text = String.Format("{0:#,###,###,###0}", VB.Val(strAmt77)) + " ";

                        ssList2.ActiveSheet.Rows[i].Height = 22;
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void ssList2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            FnRow = e.Row;
        }

        void ssList2_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            FnRow = e.Row;
            FnCol = e.Column;
        }

        void ssList2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = "";
            string strDr = "";

            strPano = ssList2_Sheet1.Cells[e.Row, 1].Text;
            strDr = ssList2_Sheet1.Cells[e.Row, 3].Text;

            Clear_SS();
            Read_OM(strPano, strDr);
        }

        void ssList2_KeyDown(object sender, KeyEventArgs e)
        {
            // ↑ 키값 72
            if (e.KeyValue == 72)
            {
                FnRow -= 1;
            }
            // ↓ 키값 80
            else if (e.KeyValue == 80)
            {
                FnRow += 1;
            }

            if (FnRow < 1)
            {
                FnRow = 1;
            }
            if (FnRow > ssList2_Sheet1.Rows.Count)
            {
                FnRow = ssList2_Sheet1.Rows.Count;
            }
        }

        void ssList2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strPano = "";
            string strDr = "";

            if (e.KeyChar == 13)
            {
                strPano = ssList2_Sheet1.Cells[FnRow - 1, 1].Text;
                strDr = ssList2_Sheet1.Cells[FnRow - 1, 3].Text;
                if (strPano == "")
                {
                    return;
                }
                Clear_SS();
                Read_OM(strPano, strDr);
            }
        }

        void txtPart_Enter(object sender, EventArgs e)
        {
            TextBox tP = sender as TextBox;
            tP.SelectionStart = 0;
            tP.SelectionLength = tP.Text.Length;
        }

        void txtPart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    txtPart.SelectAll();
                }
            }
        }

        void txtPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtPart_Leave(object sender, EventArgs e)
        {
            txtPart.Text = txtPart.Text.ToUpper();
        }

        void Read_OM(string ArgPano, string ArgDr)
        {
            string strDept = "";
            string strDate = "";
            string strReserved = "";
            string strRep = "";
            string strSex = "";
            string strSingu = "";
            string strJumin1 = "";
            string strJumin2 = "";

            int nAge = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            clsPublic.GstrActDate = CurrentDate;

            strDept = clsPmpaPb.GstrSetDeptCodes[cboDept.SelectedIndex];
            strDate = clsPublic.GstrActDate;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                        ";
            SQL += ComNum.VBLF + "  M.Sname,M.Sex,M.Age,M.DrCode,M.Bi,M.JiCode,JiName,                                                          ";
            SQL += ComNum.VBLF + "  Reserved,Chojae,M.GbGamek,M.GbSpc,Jin,Singu,Rep,                                                            ";
            SQL += ComNum.VBLF + "  P.Jumin1, P.Jumin2,P.Jumin3,                                                                                ";
            SQL += ComNum.VBLF + "  TO_CHAR(Jtime,'yyyy-mm-dd hh24:mi') Jtime,                                                                  ";
            SQL += ComNum.VBLF + "  TO_CHAR(Stime,'yyyy-mm-dd hh24:mi') Stime, Amt7                                                             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER M, " + ComNum.DB_PMPA + "BAS_AREA A, " + ComNum.DB_PMPA + "BAS_PATIENT P";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
            SQL += ComNum.VBLF + "      AND ActDate    = TO_DATE('" + strDate + "','YYYY-MM-DD')                                                ";
            SQL += ComNum.VBLF + "      AND M.Pano     = '" + ArgPano + "'                                                                       ";
            SQL += ComNum.VBLF + "      AND M.DeptCode = '" + strDept + "'                                                                      ";
            SQL += ComNum.VBLF + "      AND M.JiCode   =  A.JiCode(+)                                                                           ";
            SQL += ComNum.VBLF + "      AND M.Pano     =  P.Pano                                                                                ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strRep = dt.Rows[0]["Rep"].ToString().Trim();
                    strSex = dt.Rows[0]["Sex"].ToString().Trim();
                    strJumin1 = dt.Rows[0]["Jumin1"].ToString().Trim();

                    //주민암호화
                    if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[0]["Jumin2"].ToString().Trim();
                    }

                    strSingu = dt.Rows[0]["Singu"].ToString().Trim();
                    nAge = Convert.ToInt32(VB.Val(dt.Rows[0]["Age"].ToString().Trim()));
                    strReserved = "당일접수";

                    if (dt.Rows[0]["Reserved"].ToString().Trim() == "1")
                    {
                        strReserved = "예약접수";
                    }

                    ssList1_Sheet1.Cells[0, 1].Text = dt.Rows[0]["Sname"].ToString().Trim();
                    ssList1_Sheet1.Cells[1, 1].Text = ArgPano;
                    ssList1_Sheet1.Cells[2, 1].Text = strDept;
                    ssList1_Sheet1.Cells[3, 1].Text = dt.Rows[0]["DrCode"].ToString().Trim();
                    ssList1_Sheet1.Cells[4, 1].Text = dt.Rows[0]["Bi"].ToString().Trim();
                    ssList1_Sheet1.Cells[5, 1].Text = dt.Rows[0]["JiCode"].ToString().Trim();
                    ssList1_Sheet1.Cells[6, 1].Text = strReserved;
                    ssList1_Sheet1.Cells[7, 1].Text = dt.Rows[0]["Chojae"].ToString().Trim();
                    ssList1_Sheet1.Cells[8, 1].Text = dt.Rows[0]["GbGamek"].ToString().Trim();
                    ssList1_Sheet1.Cells[9, 1].Text = dt.Rows[0]["GbSpc"].ToString().Trim();
                    ssList1_Sheet1.Cells[10, 1].Text = dt.Rows[0]["Jin"].ToString().Trim();
                    ssList1_Sheet1.Cells[11, 1].Text = strSingu;
                    ssList1_Sheet1.Cells[12, 1].Text = strRep;
                    ssList1_Sheet1.Cells[13, 1].Text = VB.Mid(dt.Rows[0]["JTime"].ToString().Trim(), 1, 10);
                    ssList1_Sheet1.Cells[14, 1].Text = VB.Mid(dt.Rows[0]["STime"].ToString().Trim(), 1, 10);
                    ssList1_Sheet1.Cells[15, 1].Text = dt.Rows[0]["Amt7"].ToString().Trim();

                    //감액 대상자 확인 여부
                    READ_BAS_GAMF(strJumin1 + strJumin2);
                    if (mstrFlagGam == "OK")
                    {
                        if (mstrGamEnd != "")
                        {
                            if (String.Compare(CurrentDate, mstrGamEnd) > 1)
                            {
                                mstrGamMsg = "감액적용해제";
                            }
                        }
                    }
                    else
                    {
                        mstrGamMsg = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_감액코드명", dt.Rows[0]["GbGamek"].ToString().Trim());
                    }

                    ssList1_Sheet1.Cells[0, 2].Text = strSex + " / " + nAge;
                    ssList1_Sheet1.Cells[2, 2].Text = cboDept.SelectedItem.ToString();
                    ssList1_Sheet1.Cells[3, 2].Text = ArgDr;
                    ssList1_Sheet1.Cells[4, 2].Text = clsPmpaPb.GstrSetBis[Convert.ToInt32(VB.Val(dt.Rows[0]["Bi"].ToString().Trim()))];
                    ssList1_Sheet1.Cells[5, 2].Text = dt.Rows[0]["JiName"].ToString().Trim();
                    ssList1_Sheet1.Cells[6, 2].Text = dt.Rows[0]["Reserved"].ToString().Trim();
                    ssList1_Sheet1.Cells[7, 2].Text = clsPmpaPb.GstrSetChoJaes[Convert.ToInt32(VB.Val(dt.Rows[0]["Chojae"].ToString().Trim()))];
                    ssList1_Sheet1.Cells[8, 2].Text = mstrGamMsg;
                    ssList1_Sheet1.Cells[9, 2].Text = clsPmpaPb.GstrSetSpcs[Convert.ToInt32(VB.Val(dt.Rows[0]["GbSpc"].ToString().Trim()))];
                    ssList1_Sheet1.Cells[10, 2].Text = CF.READ_JIN(clsDB.DbCon, dt.Rows[0]["Jin"].ToString().Trim());
                    ssList1_Sheet1.Cells[11, 2].Text = "";
                    if (strSingu == "1")
                    {
                        ssList1_Sheet1.Cells[11, 2].Text = "신환";
                    }
                    ssList1_Sheet1.Cells[12, 2].Text = VB.Right(dt.Rows[0]["JTime"].ToString().Trim(), 5);
                    ssList1_Sheet1.Cells[13, 2].Text = VB.Right(dt.Rows[0]["STime"].ToString().Trim(), 5);
                    ssList1_Sheet1.Cells[14, 2].Text = "";
                    if (strRep == "+")
                    {
                        ssList1_Sheet1.Cells[14, 2].Text = "발행";
                    }

                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        public void READ_BAS_GAMF(string strJumin)
        {
            //'==================================================================================================
            //    '2013-11-27 감액구분 부담율 변경사항
            //    '감액코드 25 한얼시용자 -> 병원시용자로 같이 사용
            //    '감액코드 26 한얼직원-> 병원직원과 동일한 감액율 적용
            //    '감액코드 27 한얼가족-> 병원직원 직계존비속과 동일한 감액율 적용
            //    '공통사항 : 입원/외래 동일한 기준이며, 입원자는 2013-12-01일부 입원자에 한함.
            //    '치과 감액은 별도의 적용이 없음.

            string strVal = "";
            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strSabun = "";

            int nSabun = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            mstrFlagGam = "NO";

            //2013-05-21
            mstrGamGubun = "";


            strVal = "00";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                ";
            SQL += ComNum.VBLF + "  GAMCODE,  GAMMESSAGE, TO_CHAR(GAMEND, 'YYYY-MM-DD') GAMEND, TO_CHAR(GamEnter, 'YYYY-MM-DD') GamENTER";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_GAMF                                                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                             ";
            SQL += ComNum.VBLF + "      AND GAMJUMIN3  = '" + clsAES.AES(strJumin) + "'                                                 ";
            SQL += ComNum.VBLF + "      AND (GAMEND >= TO_DATE('" + strDate + "','YYYY-MM-DD') OR GAMEND IS NULL)                       ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GAMENTER"].ToString().Trim() != "")
                    {
                        if (String.Compare(dt.Rows[0]["GAMENTER"].ToString().Trim(), strDate) > 1)
                        {
                            mstrFlagGam = "NO";
                            mstrGamGubun = "";
                            mstrGamMsg = "";
                        }
                        else
                        {
                            mstrGamEnd = dt.Rows[0]["GamEnd"].ToString().Trim();
                            mstrFlagGam = "OK";
                            mstrGamGubun = dt.Rows[0]["GAMCODE"].ToString().Trim();
                            mstrGamMsg = dt.Rows[0]["GamMessage"].ToString().Trim();
                        }
                    }
                    else
                    {
                        mstrGamEnd = dt.Rows[0]["GamEnd"].ToString().Trim();

                        mstrFlagGam = "OK";

                        mstrGamGubun = dt.Rows[0]["GAMCODE"].ToString().Trim();
                        mstrGamMsg = dt.Rows[0]["GamMessage"].ToString().Trim();
                    }
                }

                //퇴사자인지 확인
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                ";
                    SQL += ComNum.VBLF + "  IPSADAY,KORNAME, TO_CHAR(TOIDAY, 'YYYY-MM-DD') TOIDAY                               ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST                                                    ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
                    SQL += ComNum.VBLF + "      AND (Jumin  = '" + strJumin + "' OR Jumin3  = '" + clsAES.AES(strJumin) + "' )  ";   //'2013-02-20;                    
                    SQL += ComNum.VBLF + "      AND TOIDAY < TRUNC(SYSDATE)                                                     ";
                    SQL += ComNum.VBLF + "      AND SABUN <'60000'                                                              ";
                    SQL += ComNum.VBLF + " UNION ALL                                                                            ";
                    SQL += ComNum.VBLF + "SELECT                                                                                ";
                    SQL += ComNum.VBLF + "  JDATE,NAME KORNAME,TO_CHAR(JDATE, 'YYYY-MM-DD') TOIDAY                              ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE                                                  ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
                    SQL += ComNum.VBLF + "      AND  GUBUN ='원무강제퇴사자감액'                                                ";
                    SQL += ComNum.VBLF + "      AND  TRIM(CODE) = '" + strJumin + "'                                            ";
                    SQL += ComNum.VBLF + " ORDER BY 1  DESC                                                                     ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        mstrGamEnd = "";
                        mstrFlagGam = "OK";
                        mstrGamGubun = "42";
                        mstrGamMsg = "<" + dt1.Rows[0]["KORNAME"].ToString().Trim() + ">" + "퇴사자";
                    }

                    //천주교 신자 감액(자원봉사, 퇴직자 신자 감액에서 제외 원무과 요청)
                    else
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                    ";
                        SQL += ComNum.VBLF + "  GAMPANO                                                 ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_GAMFSINGA                  ";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND GAMJUMIN_new  = '" + clsAES.AES(strJumin) + "'  ";   //2014-05-27

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            mstrGamEnd = "";
                            mstrFlagGam = "OK";
                            mstrGamGubun = "51";
                            mstrGamMsg = "<신자 감액>";
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;

                //    '감액퇴사 점검 2013-05-14 ------------------
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                            ";
                SQL += ComNum.VBLF + "  GAMSABUN                                                                        ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_GAMF                                               ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
                SQL += ComNum.VBLF + "      AND GAMJUMIN3  = '" + clsAES.AES(strJumin) + "'                             ";
                SQL += ComNum.VBLF + "      AND (GAMEND >= TO_DATE('" + strDate + "','YYYY-MM-DD') OR GAMEND IS NULL)   ";
                SQL += ComNum.VBLF + "      AND GAMSABUN IS NOT NULL                                                    ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                //감액 대상인것만
                if (dt.Rows.Count > 0)
                {
                    nSabun = Convert.ToInt32(VB.Val(dt.Rows[0]["GAMSABUN"].ToString().Trim()));

                    //        'bas_gamf 사번길이 치환
                    if (nSabun >= 600000)
                    {
                        strSabun = nSabun.ToString("000000");
                    }
                    else
                    {
                        strSabun = nSabun.ToString("00000");
                    }



                    SQL = " ";
                    SQL += ComNum.VBLF + "SELECT                                                                            ";
                    SQL += ComNum.VBLF + "  SABUN                                                                           ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST                                                ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
                    SQL += ComNum.VBLF + "      AND SABUN ='" + strSabun + "'                                               ";
                    SQL += ComNum.VBLF + "      AND ( TOIDAY>= TO_DATE('" + strDate + "','YYYY-MM-DD') OR TOIDAY IS NULL)   ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count == 0)
                    {
                        ComFunc.MsgBox("감액등록을 확인바랍니다..!! \r\n\r\n" + "사번: " + strSabun + "은 퇴사처리되었습니다.");
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;
                //    '감액퇴사 점검 2013-05-14 ----------------
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
            //return strVal;
        }

        void cboDept_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btnView.Focus();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "과별 월별 외래 인원 집계표";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strHeader = CS.setSpdPrint_String(strTitle, new Font("맑은 고딕", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회일자: " + clsPublic.GstrSysDate, new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, false);
            //strHeader += CS.setSpdPrint_String("인쇄일자: " + Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("yyyy-MM-dd HH:mm"), new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, false);
            if (txtPart.Text.Trim() != "")
            {
                strHeader += CS.setSpdPrint_String("     입력조: " + txtPart.Text.Trim(), new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, false);
            }
            strHeader += CS.setSpdPrint_String("     진료과: " + cboDept.Text.Trim(), new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, false);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 200, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, false, true, true, false, false);

            CS.setSpdPrint(ssList2, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);

            //btnPrint.Enabled = false;
        }
    }
}
