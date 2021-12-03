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
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewNewPBuseTong.cs
    /// Description     : 신환추천부서별현황
    /// Author          : 안정수
    /// Create Date     : 2017-09-27
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\jepres\jepres08.frm(FrmBuseTong) => frmPmpaViewNewPBuseTong.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jepres\jepres08.frm(FrmBuseTong)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewNewPBuseTong : Form
    {
        public frmPmpaViewNewPBuseTong()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

            this.optGubun0.CheckedChanged += new EventHandler(eControl_Checked);
            this.optGubun1.CheckedChanged += new EventHandler(eControl_Checked);

            this.cboBuse1.SelectedIndexChanged += new EventHandler(eControl_IndexChange);
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

            optGubun0.Checked = true;

            set_Combo();
        }

        void eBtnEvent(object sender, EventArgs e)
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
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnCancel)
            {
                ssList_Sheet1.Rows.Count = 0;
                ssList_Sheet1.Rows.Count = 50;
            }
        }

        void eControl_IndexChange(object sender, EventArgs e)
        {
            cboBuse2.SelectedIndex = cboBuse1.SelectedIndex;
        }

        void eControl_Checked(object sender, EventArgs e)
        {
            if (sender == this.optGubun0)
            {
                List_View();
                cboBuse2.Enabled = true;
                ssList_Sheet1.ColumnHeader.Cells[0, 0].Text = "부  서";
                ssList_Sheet1.ColumnHeader.Cells[0, 1].Text = "목표인원";
                ssList_Sheet1.Rows.Count = 0;
            }

            else if (sender == this.optGubun1)
            {
                List_View();
                cboBuse2.Enabled = true;
                ssList_Sheet1.ColumnHeader.Cells[0, 0].Text = "성  명";
                ssList_Sheet1.ColumnHeader.Cells[0, 1].Text = "부서명";
                ssList_Sheet1.Rows.Count = 0;
            }
        }

        void set_Combo()
        {
            int i = 0;
            string strYear = "";

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            strYear = VB.Left(CurrentDate, 4);

            for (i = 1; i <= 5; i++)
            {
                cboYear.Items.Add(strYear + "년");
                strYear = (VB.Val(strYear) - i).ToString();
            }

            cboYear.SelectedIndex = 0;
            List_View();
        }

        void List_View()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            cboBuse1.Items.Clear();
            cboBuse2.Items.Clear();

            if (optGubun0.Checked == true)
            {
                cboBuse1.Items.Add("전체                  000000");
                cboBuse2.Items.Add("전체                  000000");
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  NAME, BUCODE                                                                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUSE                                                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND BUCODE >= '033100'                                                              ";
            SQL += ComNum.VBLF + "      AND SUBSTR(BUCODE, 5,2) = '00'                                                      ";
            SQL += ComNum.VBLF + "      AND BUCODE NOT IN  ('055400','077800','099400','099200','099300','066108','066200') ";
            SQL += ComNum.VBLF + "      AND (DELDATE IS NULL OR DELDATE <> '' )                                             ";
            SQL += ComNum.VBLF + "ORDER BY BUCODE                                                                           ";

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
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cboBuse1.Items.Add(dt.Rows[i]["NAME"].ToString().Trim() + "               " + dt.Rows[i]["BUCODE"].ToString().Trim());
                        cboBuse2.Items.Add(dt.Rows[i]["NAME"].ToString().Trim() + "               " + dt.Rows[i]["BUCODE"].ToString().Trim());
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

            cboBuse1.SelectedIndex = 0;
            cboBuse2.SelectedIndex = 0;
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strName = "";
            string strBuse = "";
            bool PrePrint = true;

            btnPrint.Enabled = false;

            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            ssList.ActiveSheet.Cells[0, 16].Text = "zzz";
            ssList.ActiveSheet.Columns[16].Visible = false;


            #endregion

            if (optGubun0.Checked == true)
            {
                strName = optGubun0.Text;
            }
            else
            {
                strName = optGubun1.Text;
                strBuse = VB.Left(cboBuse1.SelectedItem.ToString(), 12);
            }

            strTitle = cboYear.SelectedItem.ToString() + " " + strName + " 신환추천 달성 현황";
            if (optGubun0.Checked == true)
            {
                if (strBuse != "")
                {
                    strSubTitle = "부서 : " + strBuse;
                }
                else
                {
                    strSubTitle = "부서 : 전체";
                }
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            if (optGubun0.Checked == true)
            {
                strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
            }

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 120, 100);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion

            btnPrint.Enabled = true;
        }

        void eGetData()
        {
            string strFBuse = "";
            string strTBuse = "";
            string strBuName = "";
            string strBuCode = "";
            string strYear = "";
            string YYMM = "";
            string strName = "";
            string strSabun = "";

            int nREAD = 0;
            int nREAD1 = 0;
            int i = 0; int j = 0; int k = 0;
            int MM = 0;
            int nTotal = 0;
            int nCnt = 0;
            int nSinCnt = 0;
            int nSumMog = 0;
            int nSumChu = 0;

            int[] nSum = new int[13];

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            strFBuse = VB.Right(cboBuse1.SelectedItem.ToString(), 6);
            strTBuse = VB.Right(cboBuse2.SelectedItem.ToString(), 6);
            strYear = VB.Left(cboYear.SelectedItem.ToString(), 4);

            nTotal = 0;
            nSumMog = 0;
            nSumChu = 0;
            ssList_Sheet1.Rows.Count = 0;

            for (j = 0; j < nSum.Length; j++)
            {
                nSum[j] = 0;
            }

            ssList_Sheet1.Rows.Count = 1;


            if (optGubun0.Checked == true)
            {
                if (strFBuse == "000000")
                {
                    foreach (string a in cboBuse1.Items)
                    {
                        strTBuse = VB.Right(a, 6);
                        strBuName = VB.Left(a, 10);

                        #region Select_Buse_View(GoSub)

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                        ";
                        SQL += ComNum.VBLF + "  (COUNT(SABUN) * 3) CNT                                      ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST                            ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                        SQL += ComNum.VBLF + "      AND (TOIDAY IS NULL OR TOIDAY = '')                     ";
                        SQL += ComNum.VBLF + "      AND SUBSTR(BUSE,1,4) = '" + VB.Left(strTBuse, 4) + "'   ";

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
                                nSinCnt = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT"].ToString().Trim()));
                                nSumMog += nSinCnt;

                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = strBuName;
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = nSinCnt + " ";
                            }
                        }
                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        dt.Dispose();
                        dt = null;

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                                                ";
                        for (MM = 1; MM <= 11; MM++)
                        {
                            YYMM = strYear + ComFunc.SetAutoZero(MM.ToString(), 2);
                            SQL += ComNum.VBLF + "COUNT(CASE WHEN TO_CHAR(BDATE,'YYYYMM') = '" + YYMM + "'  THEN A.SABUN END) CNT"
                                            + ComFunc.SetAutoZero(MM.ToString(), 2) + ",                                            ";
                        }
                        YYMM = strYear + "12";

                        SQL += ComNum.VBLF + "COUNT(CASE WHEN TO_CHAR(BDATE,'YYYYMM') = '" + YYMM + "'  THEN A.SABUN END) CNT"
                                            + ComFunc.SetAutoZero(MM.ToString(), 2) + "                                             ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SINHOAN A, " + ComNum.DB_ERP + "INSA_MST B             ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
                        SQL += ComNum.VBLF + "      AND A.SABUN = B.SABUN                                                           ";
                        SQL += ComNum.VBLF + "      AND (TOIDAY IS NULL OR TOIDAY = '')                                             ";
                        SQL += ComNum.VBLF + "      AND B.BUSE NOT IN ('033115')                                                    ";
                        SQL += ComNum.VBLF + "      AND SUBSTR(B.BUSE,1,4) = '" + VB.Left(strTBuse, 4) + "'                         ";
                        SQL += ComNum.VBLF + "      AND A.CANDATE IS NULL                                                           ";
                        SQL += ComNum.VBLF + "      AND A.SIGN1SABUN IS NOT NULL                                                    ";  //부서장 결제된 것

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
                                for (j = 1; j <= 12; j++)
                                {
                                    nCnt = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT" + ComFunc.SetAutoZero(j.ToString(), 2)].ToString().Trim()));
                                    nSum[j] += nCnt;
                                    nTotal += nCnt;
                                    nSumChu += nCnt;

                                    if (nCnt > 0)
                                    {
                                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, j + 3].Text = nCnt + " ";
                                    }
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

                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = nTotal + " ";
                        if (nSinCnt != 0)
                        {
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:##0}", (nTotal / nSinCnt) * 100) + "%" + " ";
                        }
                        nCnt = 0;
                        nTotal = 0;

                        #endregion Select_Buse_View(GoSub) End

                        ssList_Sheet1.Rows.Count += 1;
                    }
                }
                else
                {
                    strTBuse = VB.Right(cboBuse1.SelectedItem.ToString(), 6);
                    strBuName = VB.Left(cboBuse1.SelectedItem.ToString(), 10);

                    #region Select_Buse_View(GoSub)

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                        ";
                    SQL += ComNum.VBLF + "  (COUNT(SABUN) * 3) CNT                                      ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST                            ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                    SQL += ComNum.VBLF + "      AND (TOIDAY IS NULL OR TOIDAY = '')                     ";
                    SQL += ComNum.VBLF + "      AND SUBSTR(BUSE,1,4) = '" + VB.Left(strTBuse, 4) + "'   ";

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
                            nSinCnt = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT"].ToString().Trim()));
                            nSumMog += nSinCnt;

                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = strBuName;
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = nSinCnt + " ";
                        }
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                ";
                    for (MM = 1; MM <= 11; MM++)
                    {
                        YYMM = strYear + ComFunc.SetAutoZero(MM.ToString(), 2);
                        SQL += ComNum.VBLF + "COUNT(CASE WHEN TO_CHAR(BDATE,'YYYYMM') = '" + YYMM + "'  THEN A.SABUN END) CNT"
                                        + ComFunc.SetAutoZero(MM.ToString(), 2) + ",                                            ";
                    }
                    YYMM = strYear + "12";

                    SQL += ComNum.VBLF + "COUNT(CASE WHEN TO_CHAR(BDATE,'YYYYMM') = '" + YYMM + "'  THEN A.SABUN END) CNT"
                                        + ComFunc.SetAutoZero(MM.ToString(), 2) + " ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SINHOAN A, " + ComNum.DB_ERP + "INSA_MST B             ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
                    SQL += ComNum.VBLF + "      AND A.SABUN = B.SABUN                                                           ";
                    SQL += ComNum.VBLF + "      AND (TOIDAY IS NULL OR TOIDAY = '')                                             ";
                    SQL += ComNum.VBLF + "      AND B.BUSE NOT IN ('033115')                                                    ";
                    SQL += ComNum.VBLF + "      AND SUBSTR(B.BUSE,1,4) = '" + VB.Left(strTBuse, 4) + "'                         ";
                    SQL += ComNum.VBLF + "      AND A.CANDATE IS NULL                                                           ";
                    SQL += ComNum.VBLF + "      AND A.SIGN1SABUN IS NOT NULL                                                    ";  //부서장 결제된 것

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
                            for (j = 1; j <= 12; j++)
                            {
                                nCnt = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT" + ComFunc.SetAutoZero(j.ToString(), 2)].ToString().Trim()));
                                nSum[j] += nCnt;
                                nTotal += nCnt;
                                nSumChu += nCnt;

                                if (nCnt > 0)
                                {
                                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, j + 3].Text = nCnt + " ";
                                }
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

                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = nTotal + " ";
                    if (nSinCnt != 0)
                    {
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:##0}", (nTotal / nSinCnt) * 100) + "%" + " ";
                    }
                    nCnt = 0;
                    nTotal = 0;

                    #endregion Select_Buse_View(GoSub) End

                    ssList_Sheet1.Rows.Count += 1;
                }
            }
            else
            {
                strTBuse = VB.Right(cboBuse1.SelectedItem.ToString(), 6);

                #region Select_Sabun_View(GoSub)

                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                    ";
                SQL += ComNum.VBLF + "  (COUNT(a.SABUN) * 3) CNT, A.SABUN, A.KORNAME,  B.NAME, A.BUSE           ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_PMPA + "BAS_BUSE B    ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
                SQL += ComNum.VBLF + "      AND (TOIDAY IS NULL OR TOIDAY = '')                                 ";
                SQL += ComNum.VBLF + "      AND SUBSTR(BUSE,1,4) = '" + VB.Left(strTBuse, 4) + "'               ";
                SQL += ComNum.VBLF + "      AND (B.DELDATE IS NULL OR B.DELDATE <> '')                          ";
                SQL += ComNum.VBLF + "      AND A.BUSE = B.BUCODE                                               ";
                SQL += ComNum.VBLF + "GROUP BY A.BUSE, B.NAME, A.SABUN, A.KORNAME                               ";

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
                        nREAD1 = dt.Rows.Count;

                        for (k = 0; k < nREAD1; k++)
                        {
                            strSabun = dt.Rows[k]["SABUN"].ToString().Trim();
                            if (strSabun == "02220")
                            {
                                strSabun = dt.Rows[k]["SABUN"].ToString().Trim();
                            }
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT                                                                                    ";
                            SQL += ComNum.VBLF + "  A.SABUN,                                                                                ";
                            for (MM = 1; MM <= 11; MM++)
                            {
                                YYMM = strYear + ComFunc.SetAutoZero(MM.ToString(), 2);
                                SQL += ComNum.VBLF + "COUNT(CASE WHEN TO_CHAR(BDATE,'YYYYMM') = '" + YYMM + "'  THEN A.SABUN END) CNT"
                                                + ComFunc.SetAutoZero(MM.ToString(), 2) + ",                                                ";
                            }
                            YYMM = strYear + "12";
                            SQL += ComNum.VBLF + "COUNT(CASE WHEN TO_CHAR(BDATE,'YYYYMM') = '" + YYMM + "'  THEN A.SABUN END) CNT"
                                                + ComFunc.SetAutoZero(MM.ToString(), 2) + "                                                 ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SINHOAN A, " + ComNum.DB_ERP + "INSA_MST B                 ";
                            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                            SQL += ComNum.VBLF + "      AND A.SABUN = B.SABUN                                                               ";
                            SQL += ComNum.VBLF + "      AND A.SABUN  = '" + strSabun + "'                                                   ";
                            SQL += ComNum.VBLF + "      AND A.CANDATE IS NULL                                                               ";
                            SQL += ComNum.VBLF + "      AND A.SIGN1SABUN IS NOT NULL                                                        ";  //부서장 결제된것
                            SQL += ComNum.VBLF + "      AND SUBSTR(B.BUSE,1,4) = '" + VB.Left(strTBuse, 4) + "'                             ";
                            SQL += ComNum.VBLF + "GROUP BY A.SABUN                                                                          ";
                            SQL += ComNum.VBLF + "HAVING (                                                                                  ";
                            for (MM = 1; MM <= 11; MM++)
                            {
                                YYMM = strYear + ComFunc.SetAutoZero(MM.ToString(), 2);
                                SQL += ComNum.VBLF + "COUNT(CASE WHEN TO_CHAR(BDATE,'YYYYMM') = '" + YYMM + "'  THEN A.SABUN END) > 0 OR    ";
                            }
                            YYMM = strYear + "12";

                            SQL += ComNum.VBLF + "COUNT(CASE WHEN TO_CHAR(BDATE,'YYYYMM') = '" + YYMM + "'  THEN A.SABUN END) > 0)          ";
                            SQL += ComNum.VBLF + "ORDER BY SABUN                                                                            ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (dt1.Rows.Count > 0)
                            {
                                nSinCnt = Convert.ToInt32(VB.Val(dt.Rows[k]["CNT"].ToString().Trim()));
                                nSumMog += nSinCnt;

                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = dt.Rows[k]["KORNAME"].ToString().Trim();
                                if (optGubun0.Checked == true)
                                {
                                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = nSinCnt + " ";
                                }
                                else
                                {
                                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = dt.Rows[k]["NAME"].ToString().Trim();
                                }
                                for (j = 1; j <= 12; j++)
                                {
                                    nCnt = Convert.ToInt32(dt1.Rows[0]["CNT" + ComFunc.SetAutoZero(j.ToString(), 2)].ToString().Trim());
                                    nSum[j] += nCnt;
                                    nTotal += nCnt;
                                    nSumChu += nCnt;
                                    if (nCnt > 0)
                                    {
                                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = nCnt + " ";
                                    }
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = nTotal + " ";
                            if (nTotal == 0)
                            {
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = "0%";
                            }
                            else
                            {
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:##0}", (nTotal / nSinCnt) * 100) + "%" + " ";
                            }
                            nCnt = 0;
                            nTotal = 0;
                            ssList_Sheet1.Rows.Count += 1;
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



                #endregion Select_Sabun_View(GoSub) End
            }

            if (nSumMog > 0)
            {
                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "합  계";

                if (optGubun0.Checked == true)
                {
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = String.Format("{0:###,###}", nSumMog);
                }
                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = String.Format("{0:###,###}", nSumChu);
                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:###}", (nSumChu / nSumMog) * 100) + "%" + " ";
                for (j = 1; j <= 12; j++)
                {
                    if (nSum[j] > 0)
                    {
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, j + 3].Text = nSum[j] + " ";
                    }
                }
            }
        }

    }
}
