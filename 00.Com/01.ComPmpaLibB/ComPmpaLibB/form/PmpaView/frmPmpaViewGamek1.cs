using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System.Drawing;
using FarPoint.Win;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewGamek1.cs
    /// Description     : 감액명세서 조회폼
    /// Author          : 안정수
    /// Create Date     : 2017-08-30
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm감액조회1.FRM(Frm감액조회1) => frmPmpaViewGamek1.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm감액조회1.FRM(Frm감액조회1)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewGamek1 : Form
    {
        //ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        //clsPmpaFunc CPF = new clsPmpaFunc();

        string mstrJobMan = "";
        int mnJobSabun = 0;
        string mstrJobPart = "";

        string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

        string strDate = "";
        string strDate_2 = "";
  

        //************************************

        int[] nCho1 = new int[5];
        int[] nJe1 = new int[5];
        int[] ninTot1 = new int[5];

        int[] nCho2 = new int[5];
        int[] nJe2 = new int[5];
        int[] ninTot2 = new int[5];

        double[] nAmt1 = new double[5];
        double[] nAmt2 = new double[5];
        double[] nAmt3 = new double[5];
        double[] nAmt4 = new double[5];
        double[] nAmt5 = new double[5];
        double[] nAmt6 = new double[5];
        double[] nAmt7 = new double[5];

        double[] nJinChal = new double[5];
        double[] nToYak = new double[5];
        double[] nJusa = new double[5];
        double[] nMaChi = new double[5];
        double[] nMulri = new double[5];
        double[] nSin = new double[5];
        double[] nCher = new double[5];
        double[] nIngong = new double[5];
        double[] nSusul = new double[5];
        double[] nGiv = new double[5];
        double[] nTSG = new double[5];
        double[] nGTG = new double[5];
        double[] nXray = new double[5];
        double[] nBang = new double[5];

        double[] nTot1 = new double[5];

        double[] nCT = new double[5];
        double[] nJY = new double[5];
        double[] nCar = new double[5];
        double[] nTJ = new double[5];
        double[] nGita = new double[5];
        double[] nTot2 = new double[5];

        double[] nSuTot = new double[5];

        double[] nChomi = new double[5];
        double[] nGemi = new double[5];
        double[] nGam = new double[5];
        double[] nTot3 = new double[5];

        double[] nHpay = new double[5];

        double[] nYpay = new double[5];

        double[] nMisuAmt = new double[5];
        double[] nEtcAmt = new double[5];
        double[] nYTot = new double[5];

        //************************************

        int nCount = 0;
        string strSelect = "";
        string strGubun = "";

        string[] strGamDate = new string[300];
        string[] StrGamSel = new string[300];
        string[] StrGamPano = new string[300];
        string[] StrGamName = new string[300];
        string[] strGamGwa = new string[300];
        string[] strGamDr = new string[300];

        double[] nJubsuAmt = new double[300];
        double[] nJinRuAmt = new double[300];
        double[] nGamTot = new double[300];

        string[] strGamPart = new string[300];
        string[] strGamBigo = new string[300];
        string[] strSuName = new string[300];
        string[] strWardCode = new string[300];

        double nJubSuTot = 0;
        double nJinRuTot = 0;
        double nTotTotAmt = 0;

        //************************************

        string[] StrMiSel = new string[3500];
        string[] strMiPano = new string[3500];
        string[] strMIName = new string[3500];
        string[] strMIGwa = new string[3500];
        string[] strMiDr = new string[3500];
        string[] StrMiBohun = new string[3500];
        string[] StrMiGelCode = new string[3500];

        double[] nJubsuMi = new double[3500];
        double[] nJinruMi = new double[3500];
        double[] nMiTot = new double[3500];

        public frmPmpaViewGamek1()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewGamek1(string GstrJobName, int GnJobSabun)
        {
            InitializeComponent();
            setEvent();
            mstrJobMan = GstrJobName;
            mnJobSabun = GnJobSabun;
        }

        public frmPmpaViewGamek1(string GstrJobPart)
        {
            InitializeComponent();
            setEvent();
            mstrJobPart = GstrJobPart;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

            this.optGubun0.CheckedChanged += new EventHandler(eBtnEvent);
            this.optGubun1.CheckedChanged += new EventHandler(eBtnEvent);
            this.optGubun2.CheckedChanged += new EventHandler(eBtnEvent);

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

            optGubun2.Checked = true;

            if (mstrJobPart != " ")
            {
                txtPart.Text = mstrJobPart;
            }

            if (mnJobSabun != 0)
            {
                txtPart.Text = mnJobSabun.ToString();
            }

            btnPrint.Enabled = false;
            label5.Text = "2012년 5월 효도감액(부,모,시부,시모,장인,장모+배우자,조부,조모)";

           
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                ePrint();
            }

            else if (sender == this.optGubun0 || sender == this.optGubun1 || sender == this.optGubun2)
            {
                if (sender == this.optGubun0)
                {
                    label5.Text = "2012년 5월 효도감액(부,모,시부,시모,장인,장모)";
                }

                else if (sender == this.optGubun1)
                {
                    label5.Text = "2012년 5월 효도감액(부,모,시부,시모,장인,장모+배우자)";
                }

                else
                {
                    label5.Text = "2012년 5월 효도감액(부,모,시부,시모,장인,장모+배우자,조부,조모)";
                }
            }

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
            bool PrePrint = true;

            string JobDate = dtpFDate.Text;
            string JobMan = mstrJobMan;

            if (ssList_Sheet1.Rows.Count == 0)
            {
                return;
            }

            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            //ssList.ActiveSheet.Cells[0, 9].Text = "zzz";
            //ssList.ActiveSheet.Columns[9].Visible = false;


            #endregion

            strTitle = "감  액  명  세  서";

            if(JobMan != "")
            {
                strSubTitle = "작성자 : " + JobMan + "\r\n" + "작업일자 : " + JobDate;
            }

            else
            {
                strSubTitle = "작업일자 : " + JobDate;
            }
            

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 45, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, true, false, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            btnPrint.Enabled = true;
            Cursor.Current = Cursors.WaitCursor;
            ClearProcess();
            ssListPrintBuild();
            ssListPrintMove();
            Cursor.Current = Cursors.Default;            
        }

        void ClearProcess()
        {
            int i = 0;

            for (i = 0; i < StrGamSel.Length; i++)
            {
                StrGamSel[i] = "";
                StrGamPano[i] = "";
                StrGamName[i] = "";
                strGamGwa[i] = "";
                strGamDr[i] = "";
                nJubsuAmt[i] = 0;
                nJinRuAmt[i] = 0;
                nGamTot[i] = 0;
                strGamBigo[i] = "";
                strGamDate[i] = "";
            }

            nJubSuTot = 0;
            nJinRuTot = 0;
            nTotTotAmt = 0;
            nCount = 0;

            ssList_Sheet1.Rows.Count = 0;
        }

        void ssListPrintBuild()
        {
            int i = 0;

            string strOK = "";
            string strKwan = "";
            string strSabun = "";

            DataTable dt = null;
            DataTable dt2 = null;

            string SQL = "";
            string SqlErr = "";

            strDate = dtpFDate.Text;
            strDate_2 = dtpTDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(O.ActDate,'YYYY-MM-DD') ACTDATE, O.Pano, B.Sname, O.DeptCode,                       ";
            SQL += ComNum.VBLF + "  O.Drcode, O.Part,B.Jumin1,B.Jumin2,                                                         ";
            SQL += ComNum.VBLF + "  SUM(DECODE(WARDCODE,'99',Amt1+Amt2,0)) JUBGAM,                                              ";
            SQL += ComNum.VBLF + "  SUM(DECODE(WARDCODE,'99',0,Amt1+Amt2)) JINGAM , SUNEXT, O.WARDCODE                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_Patient B, " + ComNum.DB_PMPA + "Opd_Slip O                    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "      AND ActDate >= TO_DATE('" + strDate + "','YYYY-MM-DD')                                  ";
            SQL += ComNum.VBLF + "      AND ActDate <= TO_DATE('" + strDate_2 + "','YYYY-MM-DD')                                ";
            SQL += ComNum.VBLF + "      And Bun = '92'                                                                          ";
            SQL += ComNum.VBLF + "      And O.Pano = B.Pano(+)                                                                  ";
            if (txtPart.Text != "" && !VB.IsNull(txtPart.Text))
            {
                SQL += ComNum.VBLF + "  And O.Part = '" + txtPart.Text + "'                                                     ";
            }

            SQL += ComNum.VBLF + "      AND O.PANO <> '81000004'                                                                ";
            SQL += ComNum.VBLF + "Group By  TO_CHAR(O.ActDate,'YYYY-MM-DD') ,O.Pano,B.SName,O.Deptcode,O.DrCode,O.Part,         ";
            SQL += ComNum.VBLF + "          B.Jumin1,B.Jumin2, SUNEXT, WARDCODE                                                 ";
            SQL += ComNum.VBLF + "Order By  TO_CHAR(O.ActDate,'YYYY-MM-DD') ,sunext,  O.Pano, B.SName, O.Deptcode, O.DrCode     ";

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

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (dt.Rows.Count > 0)
            {
                btnPrint.Focus();

                nCount = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "";
                    strKwan = "";
                    strSabun = "";

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                                        ";
                    SQL += ComNum.VBLF + "  a.Pano,c.kwan,c.Sabun                                                                                       ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER a, " + ComNum.DB_PMPA + "BAS_PATIENT b, " + ComNum.DB_ERP + "INSA_MSTB c";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
                    SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                                    ";
                    SQL += ComNum.VBLF + "      AND b.Jumin1||b.Jumin2 =c.Jumin(+)                                                                      ";
                    SQL += ComNum.VBLF + "      AND a.ACTDATE =TO_DATE('" + dt.Rows[i]["ActDate"].ToString().Trim() + "','YYYY-MM-DD')                  ";
                    SQL += ComNum.VBLF + "      AND a.Pano ='" + dt.Rows[i]["Pano"].ToString().Trim() + "'                                              ";
                    SQL += ComNum.VBLF + "      AND a.DeptCode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'                                      ";
                    SQL += ComNum.VBLF + "      AND a.GBGAMEK IN ('22','23','24')                                                                       ";
                    if (optGubun0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND c.Kwan IN ('2','3','10','11','12','13')                                                             ";
                    }
                    else if (optGubun1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND c.Kwan IN ( '0','2','3','10','11','12','13')                                                        ";
                    }
                    else if (optGubun2.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND c.Kwan IN ( '0','2','3','6','7','10','11','12','13')";  //조부모 추가 2012-04-30
                    }
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    //if (dt2.Rows.Count == 0)
                    //{
                    //    dt2.Dispose();
                    //    dt2 = null;
                    //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    //    return;
                    //}

                    if (dt2.Rows.Count > 0)
                    {
                        strKwan = dt2.Rows[0]["Kwan"].ToString().Trim();
                        strSabun = dt2.Rows[0]["Sabun"].ToString().Trim();
                        if (strKwan != "")
                        {
                            strOK = "OK";
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;

                    if (strOK == "OK")
                    {
                        if (dt.Rows[i]["JUBGAM"].ToString().Trim() != "0" || dt.Rows[i]["JINGAM"].ToString().Trim() != "0")
                        {
                            nCount += 1;

                            strGamDate[nCount] = dt.Rows[i]["ActDate"].ToString().Trim();
                            StrGamPano[nCount] = dt.Rows[i]["Pano"].ToString().Trim();
                            StrGamName[nCount] = dt.Rows[i]["Sname"].ToString().Trim();
                            strGamGwa[nCount] = dt.Rows[i]["DeptCode"].ToString().Trim();
                            strGamDr[nCount] = dt.Rows[i]["DrCode"].ToString().Trim();
                            nJubsuAmt[nCount] = VB.Val(dt.Rows[i]["JUBGAM"].ToString().Trim());
                            nJinRuAmt[nCount] = VB.Val(dt.Rows[i]["JINGAM"].ToString().Trim());
                            nGamTot[nCount] = VB.Val(dt.Rows[i]["JUBGAM"].ToString().Trim()) + VB.Val(dt.Rows[i]["JINGAM"].ToString().Trim());
                            strGamPart[nCount] = dt.Rows[i]["Part"].ToString().Trim();
                            strSuName[nCount] = dt.Rows[i]["SUNEXT"].ToString().Trim();
                            strWardCode[nCount] = dt.Rows[i]["WARDCODE"].ToString().Trim();

                            strGamBigo[nCount] = "";
                            if (strSuName[nCount] == "Y92-01")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + "SELECT                                                      ";
                                SQL += ComNum.VBLF + "  REMARK                                                    ";
                                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_GAMF_REMARK                  ";
                                SQL += ComNum.VBLF + "WHERE 1=1                                                   ";
                                SQL += ComNum.VBLF + "      AND PANO  = '" + StrGamPano[nCount] + "'              ";
                                SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if (dt2.Rows.Count == 0)
                                {
                                    dt2.Dispose();
                                    dt2 = null;
                                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                                    return;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    strGamBigo[nCount] = dt2.Rows[0]["REMARK"].ToString().Trim();
                                }
                                else
                                {
                                    strGamBigo[nCount] = clsVbfunc.READ_SugaName(clsDB.DbCon, strSuName[nCount]);
                                }
                                dt2.Dispose();
                                dt2 = null;
                            }
                            else
                            {
                                switch (strKwan)
                                {
                                    case "0":
                                        strKwan = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun) + "[배우자]";
                                        break;

                                    case "2":
                                        strKwan = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun) + "[부]";
                                        break;
                                    case "3":
                                        strKwan = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun) + "[모]";
                                        break;

                                    case "6":
                                        strKwan = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun) + "[조부]";
                                        break;
                                    case "7":
                                        strKwan = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun) + "[조모]";
                                        break;

                                    case "10":
                                        strKwan = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun) + "[시부]";
                                        break;
                                    case "11":
                                        strKwan = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun) + "[시모]";
                                        break;
                                    case "12":
                                        strKwan = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun) + "[장인]";
                                        break;
                                    case "13":
                                        strKwan = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun) + "[장모]";
                                        break;
                                }

                                strGamBigo[nCount] = strKwan;

                                strKwan = "";
                            }
                        }
                    }

                }
            }

            dt.Dispose();
            dt = null;

            btnPrint.Focus();
        }

        void ssListPrintMove()
        {
            int i = 0;
            int num = 0;

            string SSel = "";
            string SaveSel = "";
            string SPano = "";
            string SGwa = "";
            string SGwaName = "";
            string SDrCode = "";
            string SDrName = "";
            string strPartName = "";

            double nJubSo = 0;
            double nJinSo = 0;
            double nGamSo = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            //nFalg -> false

            if (nCount == 0)
            {
                return;
            }

            nJubSo = 0;
            nJinSo = 0;
            nGamSo = 0;
            num = 0;
            SaveSel = "";

            for (i = 0; i < nCount + 1; i++)
            {
                if (nJubsuAmt[i] != 0 || nJinRuAmt[i] != 0)
                {
                    num += 1;

                    SSel = strGamDate[i].Trim();

                    SPano = StrGamPano[i].Trim();
                    SGwa = strGamGwa[i].Trim();
                    SDrCode = strGamDr[i].Trim();

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                    ";
                    SQL += ComNum.VBLF + "  DeptNamek                               ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_ClinicDept ";
                    SQL += ComNum.VBLF + "WHERE DeptCode = '" + SGwa + "'           ";
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
                        SGwaName = dt.Rows[0]["DeptNameK"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;


                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                    ";
                    SQL += ComNum.VBLF + "  DrName                                  ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_Doctor     ";
                    SQL += ComNum.VBLF + "WHERE DrCode = '" + SDrCode + "'          ";
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
                        SDrName = dt.Rows[0]["DrName"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    //if (StrGamPano[i].Trim() != "#")
                    if (strGamPart[i].Trim() != "#")
                    {
                        if (String.Compare(dtpFDate.Text, "2006-01-05") >= 1)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  Name";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_Pass";
                            SQL += ComNum.VBLF + "WHERE IDNUMBER = '" + ComFunc.SetAutoZero(strGamPart[i], 5) + "'";                              
                        }
                        else
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  Name";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_Pass";
                            SQL += ComNum.VBLF + "WHERE PART = '" + strGamPart[i] + "'";                           
                        }

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strPartName = dt.Rows[0]["Name"].ToString().Trim();
                        }

                        else if (strGamPart[i] == "#")
                        {
                            strPartName = "입원";
                        }

                        dt.Dispose();
                        dt = null;
                    }

                    else if (strGamPart[i] == "#")
                    {
                        strPartName = "입원";
                    }

                    ssList_Sheet1.Rows.Count = num;

                    if (SaveSel == SSel)
                    {
                        GamReportGubun();

                        ssList_Sheet1.Cells[num - 1, 0].Text = strGamDate[i];
                        ssList_Sheet1.Cells[num - 1, 1].Text = SPano;
                        ssList_Sheet1.Cells[num - 1, 2].Text = StrGamName[i];
                        ssList_Sheet1.Cells[num - 1, 3].Text = SGwa;
                        ssList_Sheet1.Cells[num - 1, 4].Text = SDrName;
                        ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubsuAmt[i]);
                        ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinRuAmt[i]);
                        ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nGamTot[i]);
                        ssList_Sheet1.Cells[num - 1, 8].Text = strPartName;
                        ssList_Sheet1.Cells[num - 1, 9].Text = strGamBigo[i];

                        nJubSo += nJubsuAmt[i];
                        nJubSuTot += nJubsuAmt[i];
                        nJinSo += nJinRuAmt[i];
                        nJinRuTot += nJinRuAmt[i];
                        nGamSo += nJubsuAmt[i] + nJinRuAmt[i];
                        nTotTotAmt += (nJubsuAmt[i] + nJinRuAmt[i]);
                    }
                    else if (i == 1)
                    {
                        SaveSel = SSel;
                        strSelect = SSel;
                        GamReportGubun();

                        clsVbfunc.READ_SugaName(clsDB.DbCon, strSuName[i].Trim());

                        ssList_Sheet1.Cells[num - 1, 0].Text = strGamDate[i];
                        ssList_Sheet1.Cells[num - 1, 1].Text = SPano;
                        ssList_Sheet1.Cells[num - 1, 2].Text = StrGamName[i];
                        ssList_Sheet1.Cells[num - 1, 3].Text = SGwa;
                        ssList_Sheet1.Cells[num - 1, 4].Text = SDrName;
                        ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubsuAmt[i]);
                        ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinRuAmt[i]);
                        ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nGamTot[i]);
                        ssList_Sheet1.Cells[num - 1, 8].Text = strPartName;
                        ssList_Sheet1.Cells[num - 1, 9].Text = strGamBigo[i];

                        nJubSo += nJubsuAmt[i];
                        nJubSuTot += nJubsuAmt[i];
                        nJinSo += nJinRuAmt[i];
                        nJinRuTot += nJinRuAmt[i];
                        nGamSo += nJubsuAmt[i] + nJinRuAmt[i];
                        nTotTotAmt += (nJubsuAmt[i] + nJinRuAmt[i]);
                    }

                    else if (num == 1)
                    {
                        SaveSel = SSel;
                        strSelect = SSel;
                        GamReportGubun();

                        ssList_Sheet1.Cells[num - 1, 0].Text = strGamDate[i];
                        ssList_Sheet1.Cells[num - 1, 1].Text = SPano;
                        ssList_Sheet1.Cells[num - 1, 2].Text = StrGamName[i];
                        ssList_Sheet1.Cells[num - 1, 3].Text = SGwa;
                        ssList_Sheet1.Cells[num - 1, 4].Text = SDrName;
                        ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubsuAmt[i]);
                        ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinRuAmt[i]);
                        ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nGamTot[i]);
                        ssList_Sheet1.Cells[num - 1, 8].Text = strPartName;
                        ssList_Sheet1.Cells[num - 1, 9].Text = strGamBigo[i];

                        nJubSo += nJubsuAmt[i];
                        nJubSuTot += nJubsuAmt[i];
                        nJinSo += nJinRuAmt[i];
                        nJinRuTot += nJinRuAmt[i];
                        nGamSo += nJubsuAmt[i] + nJinRuAmt[i];
                        nTotTotAmt += (nJubsuAmt[i] + nJinRuAmt[i]);
                    }

                    else if (SaveSel != SSel && i != 0 && num > 1)
                    {
                        SaveSel = SSel;
                        strSelect = SSel;

                        ssList_Sheet1.Cells[num - 1, 0].Text = "소　　계";
                        ssList_Sheet1.Cells[num - 1, 1].Text = "";
                        ssList_Sheet1.Cells[num - 1, 2].Text = "";
                        ssList_Sheet1.Cells[num - 1, 3].Text = "";
                        ssList_Sheet1.Cells[num - 1, 4].Text = "";
                        ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubSo);
                        ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinSo);
                        ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nGamSo);
                        ssList_Sheet1.Cells[num - 1, 0, num - 1, ssList_Sheet1.ColumnCount - 1].Border = new LineBorder(Color.Black, 1, false, false, false, true);

                        //SS1.Col = 1: SS1.Row = num
                        //SS1.Col2 = -1: SS1.Row2 = num
                        //SS1.BlockMode = True
                        //SS1.CellBorderType = 8
                        //SS1.CellBorderStyle = SS_BORDER_STYLE_SOLID
                        //SS1.CellBorderColor = RGB(0, 0, 0)
                        //SS1.Action = SS_ACTION_SET_CELL_BORDER
                        //SS1.BlockMode = False

                        nJubSo = 0;
                        nJinSo = 0;
                        nGamSo = 0;

                        num += 1;

                        ssList_Sheet1.Rows.Count = num;
                        GamReportGubun();

                        ssList_Sheet1.Cells[num - 1, 0].Text = strGamDate[i];
                        ssList_Sheet1.Cells[num - 1, 1].Text = SPano;
                        ssList_Sheet1.Cells[num - 1, 2].Text = StrGamName[i];
                        ssList_Sheet1.Cells[num - 1, 3].Text = SGwa;
                        ssList_Sheet1.Cells[num - 1, 4].Text = SDrName;
                        ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubsuAmt[i]);
                        ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinRuAmt[i]);
                        ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nGamTot[i]);
                        ssList_Sheet1.Cells[num - 1, 8].Text = strPartName;
                        ssList_Sheet1.Cells[num - 1, 9].Text = strGamBigo[i];

                        nJubSo += nJubsuAmt[i];
                        nJubSuTot += nJubsuAmt[i];
                        nJinSo += nJinRuAmt[i];
                        nJinRuTot += nJinRuAmt[i];
                        nGamSo += nJubsuAmt[i] + nJinRuAmt[i];
                        nTotTotAmt += (nJubsuAmt[i] + nJinRuAmt[i]);
                    }
                }
            }

            if (num > 0)
            {
                num += 1;
                ssList_Sheet1.Rows.Count = num;

                ssList_Sheet1.Cells[num - 1, 0].Text = "소　　계";
                ssList_Sheet1.Cells[num - 1, 1].Text = "";
                ssList_Sheet1.Cells[num - 1, 2].Text = "";
                ssList_Sheet1.Cells[num - 1, 3].Text = "";
                ssList_Sheet1.Cells[num - 1, 4].Text = "";
                ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubSo);
                ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinSo);
                ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nGamSo);
                ssList_Sheet1.Cells[num - 1, 0, num - 1, ssList_Sheet1.ColumnCount - 1].Border = new LineBorder(Color.Black, 1, false, false, false, true);

                nJubSo = 0;
                nJinSo = 0;
                nGamSo = 0;

                num += 1;
                ssList_Sheet1.Rows.Count = num;

                ssList_Sheet1.Cells[num - 1, 0].Text = "총　　계";
                ssList_Sheet1.Cells[num - 1, 1].Text = "";
                ssList_Sheet1.Cells[num - 1, 2].Text = "";
                ssList_Sheet1.Cells[num - 1, 3].Text = "";
                ssList_Sheet1.Cells[num - 1, 4].Text = "";
                ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubSuTot);
                ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinRuTot);
                ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nTotTotAmt);
                ssList_Sheet1.Cells[num - 1, 0, num - 1, ssList_Sheet1.ColumnCount - 1].Border = new LineBorder(Color.Black, 1, false, false, false, true);

                nJubSuTot = 0;
                nJinRuTot = 0;
                nTotTotAmt = 0;

                //SS1.Col = 1: SS1.Row = num - 1
                //SS1.Col2 = -1: SS1.Row2 = num
                //SS1.BlockMode = True
                //SS1.CellBorderType = 8
                //SS1.CellBorderStyle = SS_BORDER_STYLE_SOLID
                //SS1.CellBorderColor = RGB(0, 0, 0)
                //SS1.Action = SS_ACTION_SET_CELL_BORDER
                //SS1.BlockMode = False
            }
        }

        void GamReportGubun()
        {
            strGubun = clsVbfunc.READ_SugaName(clsDB.DbCon, strSelect.Trim());
        }

        void txtPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
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

        void dtpFDate_ValueChanged(object sender, EventArgs e)
        {
            if(dtpFDate.Text != clsPmpaPb.GstrSysDate)
            {
                dtpTDate.Text = dtpFDate.Text;
            }            
        }
    }
}
