using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComBase
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-03-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\nurse\nrinfo\nrinfo.vbp\Frm욕창발생보고서" >> frmBedsoreReport.cs 폼이름 재정의" />

    public partial class frmBedsoreReport : Form
    {
        string FstrROWID = "";
        string FstrFlag = "";

        string gsWard = "";

        string mstrIpdno = "";

        CheckBox[] ChkP_1 = new CheckBox[11];
        CheckBox[] ChkP_2 = new CheckBox[15];
        CheckBox[] ChkP_3 = new CheckBox[10];
        TextBox[] Txt_Pstep = new TextBox[11];
        RadioButton[] Opt_P3 = new RadioButton[5];



        public frmBedsoreReport()
        {
            InitializeComponent();
        }

        public frmBedsoreReport(string strWard)
        {
            InitializeComponent();
            gsWard = strWard;
        }

        public frmBedsoreReport(string strWard ,string strIpdno)
        {
            InitializeComponent();
            gsWard = strWard;
            mstrIpdno = strIpdno;
        }


        private void frmBedsoreReport_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (clsType.User.JobGroup == "JOB013053")
            {
                chkWrite.Visible = true;
            }

            Txt_Pstep[0] = Txt_Pstep0;
            Txt_Pstep[1] = Txt_Pstep1;
            Txt_Pstep[2] = Txt_Pstep2;
            Txt_Pstep[3] = Txt_Pstep3;
            Txt_Pstep[4] = Txt_Pstep4;
            Txt_Pstep[5] = Txt_Pstep5;
            Txt_Pstep[6] = Txt_Pstep6;
            Txt_Pstep[7] = Txt_Pstep7;
            Txt_Pstep[8] = Txt_Pstep8;
            Txt_Pstep[9] = Txt_Pstep9;

            ChkP_1[0] = ChkP_10;
            ChkP_1[1] = ChkP_11;
            ChkP_1[2] = ChkP_12;
            ChkP_1[3] = ChkP_13;
            ChkP_1[4] = ChkP_14;
            ChkP_1[5] = ChkP_15;
            ChkP_1[6] = ChkP_16;
            ChkP_1[7] = ChkP_17;
            ChkP_1[8] = ChkP_18;
            ChkP_1[9] = ChkP_19;

            ChkP_2[0] = ChkP_20;
            ChkP_2[1] = ChkP_21;
            ChkP_2[2] = ChkP_22;
            ChkP_2[3] = ChkP_23;
            ChkP_2[4] = ChkP_24;
            ChkP_2[5] = ChkP_25;
            ChkP_2[6] = ChkP_26;
            ChkP_2[7] = ChkP_27;
            ChkP_2[8] = ChkP_28;
            ChkP_2[9] = ChkP_29;
            ChkP_2[10] = ChkP_210;
            ChkP_2[11] = ChkP_211;
            ChkP_2[12] = ChkP_212;
            ChkP_2[13] = ChkP_213;

            ChkP_3[0] = ChkP_30;
            ChkP_3[1] = ChkP_31;
            ChkP_3[2] = ChkP_32;
            ChkP_3[3] = ChkP_33;
            ChkP_3[4] = ChkP_34;
            ChkP_3[5] = ChkP_35;
            ChkP_3[6] = ChkP_36;
            ChkP_3[7] = ChkP_37;
            ChkP_3[8] = ChkP_38;

            Opt_P3[0] = Opt_P30;
            Opt_P3[1] = Opt_P31;
            Opt_P3[2] = Opt_P32;
            Opt_P3[3] = Opt_P33;

            SS1_Sheet1.Columns[9].Visible = false;
            SS1_Sheet1.Columns[10].Visible = false;

            if(gsWard == "")
            {
                gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");
            }

            ComboWard_SET();

            TxtDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            TxtEDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            cboErrGubun.Items.Clear();
            cboErrGubun.Items.Add("근접오류");
            cboErrGubun.Items.Add("위해사건");
            cboErrGubun.Items.Add("적신호사건");
            cboErrGubun.SelectedIndex = 0;

            cboErrGrade.Items.Clear();

            Search();

            if (mstrIpdno != "")
            {
                Info_Display(mstrIpdno);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgDate">"yyyy-MM-dd 형식으로"</param>
        /// <param name="argPTNO"></param>
        private void Info_Display(string ArgIpdNo, string ArgDate = "", string argPTNO = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int k = 0;
            string StrTemp = "";
            double nTOT1 = 0;

            ComFunc CF = new ComFunc();

            Cursor.Current = Cursors.WaitCursor; 

             nTOT1 = 0;

            try
            {
                if (ArgDate != "" && VB.IsDate(ArgDate) == false)
                {
                    ArgDate = "";
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Total FROM NUR_BRADEN_SCALE WHERE IPDNO= " + VB.Val(ArgIpdNo) + "  ORDER BY ActDate DESC  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {

                    nTOT1 = VB.Val(dt.Rows[0]["Total"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

              

                SCREEN_CLEAR();

                SQL = "";
                SQL = "  SELECT PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,TO_CHAR(InDate,'YYYY-MM-DD')InDate,";
                SQL = SQL + ComNum.VBLF + " SNAME,SEX,AGE,DIAGNOSYS,ROOMCODE,DEPTCODE,EntSabun, ";
                SQL = SQL + ComNum.VBLF + " GRADE,TOTAL,P_BALBUI,P_BALBUI_etc,P_STEP,P_HAPBUNG,P_PROGRESS,P_YOIN,P_PRE,REMARK,WardCode,ROWID, ENTDATE, PRTYN, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SEEKDATE,'YYYY-MM-DD HH24:MI') SEEKDATE, TO_CHAR(RETURNDATE,'YYYY-MM-DD HH24:MI') RETURNDATE, ERRGUBUN, ERRGRADE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_PRESSURE_SORE ";

                if (ComboWard.Text == "ER")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = 0";
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + ComFunc.SetAutoZero(argPTNO, 8) + "' ";

                    if (ArgDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND INDATE = TO_DATE('" + Convert.ToDateTime(ArgDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    }

                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = 'ER'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO =" + VB.Val(ArgIpdNo);
                }



                if (ArgDate != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND ENTDATE = TO_DATE('" + Convert.ToDateTime(ArgDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
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

                    //'MsgBox "욕창발생보고서 기존등록된 자료를 불러옵니다.", vbInformation, "확인"
                    FstrFlag = "Y";
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 0].Text = dt.Rows[0]["InDate"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 1].Text = dt.Rows[0]["ActDate"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 2].Text = CF.DATE_ILSU(clsDB.DbCon, dt.Rows[0]["ActDate"].ToString().Trim(), dt.Rows[0]["InDate"].ToString().Trim()).ToString();
                    SS1_Sheet1.Cells[0, 3].Text = dt.Rows[0]["Pano"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 4].Text = dt.Rows[0]["SName"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 5].Text = dt.Rows[0]["Sex"].ToString().Trim() + "/" + dt.Rows[0]["Age"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 6].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 7].Text = dt.Rows[0]["RoomCode"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 8].Text = dt.Rows[0]["Grade"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 9].Text = VB.Val(dt.Rows[0]["IpdNo"].ToString().Trim()).ToString();
                    SS1_Sheet1.Cells[0, 10].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 11].Text = Convert.ToDateTime(dt.Rows[0]["ENTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                    SS1_Sheet1.Cells[0, 12].Text = dt.Rows[0]["PRTYN"].ToString().Trim();

                    // '상세내역
                    TxtDiag.Text = dt.Rows[0]["DIAGNOSYS"].ToString().Trim();
                    TxtP_Jumsu.Text = nTOT1.ToString();//    '등록일의 최근값
                    Txt_Petc.Text = dt.Rows[0]["P_BALBUI_etc"].ToString().Trim();

                    txtSeekDate.Text = dt.Rows[0]["SEEKDATE"].ToString().Trim();
                    txtReturnDate.Text = dt.Rows[0]["RETURNDATE"].ToString().Trim();
                    cboErrGubun.Text = dt.Rows[0]["ERRGUBUN"].ToString().Trim();
                    cboErrGrade.Text = dt.Rows[0]["ERRGRADE"].ToString().Trim();

                    for (k = 0; k < VB.I(dt.Rows[0]["P_BALBUI"].ToString().Trim(), "^^"); k++)
                    {
                        if (VB.Pstr(dt.Rows[0]["P_BALBUI"].ToString().Trim(), "^^", k + 1) == "1")
                        {
                            ChkP_1[k].Checked = true;
                        }
                        if (string.Compare(VB.Pstr(dt.Rows[0]["P_STEP"].ToString().Trim(), "^^", k + 1), "0") > 0)
                        {
                            Txt_Pstep[k].Text = VB.Pstr(dt.Rows[0]["P_STEP"].ToString().Trim(), "^^", k + 1);
                        }
                    }

                    if (dt.Rows[0]["P_HAPBUNG"].ToString().Trim() == "Y")
                    {
                        Opt_P20.Checked = true;
                    }
                    else
                    {
                        Opt_P21.Checked = true;
                    }
                    if (dt.Rows[0]["P_PROGRESS"].ToString().Trim() == "1")
                    {
                        Opt_P30.Checked = true;
                    }
                    else if (dt.Rows[0]["P_PROGRESS"].ToString().Trim() == "2")
                    {
                        Opt_P31.Checked = true;
                    }
                    else if (dt.Rows[0]["P_PROGRESS"].ToString().Trim() == "3")
                    {
                        Opt_P32.Checked = true;
                    }
                    else if (dt.Rows[0]["P_PROGRESS"].ToString().Trim() == "4")
                    {
                        Opt_P33.Checked = true;
                    }

                    for (k = 0; k < VB.I(dt.Rows[0]["P_YOIN"].ToString().Trim(), "^^"); k++)
                    {
                        if (VB.Pstr(dt.Rows[0]["P_YOIN"].ToString().Trim(), "^^", k + 1) == "1")
                        {
                            ChkP_2[k].Checked = true;
                        }
                    }
                    for (k = 0; k < VB.I(dt.Rows[0]["P_PRE"].ToString().Trim(), "^^"); k++)
                    {
                        if (VB.Pstr(dt.Rows[0]["P_PRE"].ToString().Trim(), "^^", k + 1) == "1")
                        {
                            ChkP_3[k].Checked = true;
                        }
                    }

                    //'인쇄용 SS2

                    SS2_Sheet1.Cells[6, 2].Text = SS1_Sheet1.Cells[0, 0].Text;
                    SS2_Sheet1.Cells[6, 3].Text = SS1_Sheet1.Cells[0, 1].Text;
                    SS2_Sheet1.Cells[6, 4].Text = SS1_Sheet1.Cells[0, 2].Text;
                    SS2_Sheet1.Cells[6, 5].Text = SS1_Sheet1.Cells[0, 3].Text;
                    SS2_Sheet1.Cells[6, 6].Text = SS1_Sheet1.Cells[0, 4].Text;
                    SS2_Sheet1.Cells[6, 7].Text = SS1_Sheet1.Cells[0, 5].Text;
                    SS2_Sheet1.Cells[6, 8].Text = SS1_Sheet1.Cells[0, 6].Text;
                    SS2_Sheet1.Cells[6, 9].Text = SS1_Sheet1.Cells[0, 7].Text;
                    SS2_Sheet1.Cells[6, 10].Text = SS1_Sheet1.Cells[0, 8].Text;

                    SS2_Sheet1.Cells[3, 7].Text = SS1_Sheet1.Cells[0, 11].Text;

                    SS2_Sheet1.Cells[8, 2].Text = txtSeekDate.Text;
                    SS2_Sheet1.Cells[8, 3].Text = txtReturnDate.Text;
                    //SS2_Sheet1.Cells[8, 4].Text = clsVbfunc.GetInSaName(clsDB.DbCon, VB.Val(clsType.User.Sabun).ToString("00000"));
                    SS2_Sheet1.Cells[8, 4].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["ENTSABUN"].ToString());
                    

                    SS2_Sheet1.Cells[7, 6].Text = cboErrGubun.Text;
                    SS2_Sheet1.Cells[8, 5].Text = cboErrGrade.Text;


                    SS2_Sheet1.Cells[10+2, 4].Text = VB.Space(3) + TxtDiag.Text.Trim();

                    //  SS2.Row = 12:
                    //  SS2.Col = 5
                    if (Opt_P20.Checked == true)
                    {
                        SS2_Sheet1.Cells[11 + 2, 4].Text = "유";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[11 + 2, 4].Text = "무";
                    }

                    SS2_Sheet1.Cells[12 + 2, 4].Text = TxtP_Jumsu.Text.Trim();

                    // SS2.Row = 14:
                    // SS2.Col = 5:
                    if (Opt_P30.Checked == true)
                    {
                        SS2_Sheet1.Cells[13 + 2, 4].Text = "완쾌";
                    }
                    else if (Opt_P31.Checked == true)
                    {
                        SS2_Sheet1.Cells[13 + 2, 4].Text = "악화";
                    }
                    else if (Opt_P32.Checked == true)
                    {
                        SS2_Sheet1.Cells[13 + 2, 4].Text = "사망";
                    }
                    else if (Opt_P33.Checked == true)
                    {
                        SS2_Sheet1.Cells[13 + 2, 4].Text = "퇴원";
                    }
                    //  '발생부위, 단계
                    //SS2.Row = 18
                    //SS2.Col = 3: 
                    StrTemp = "";
                    for (k = 0; k <= 4; k++)
                    {
                        if (ChkP_1[k].Checked == true)
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_1[k].Text + "(√) 단계 [" + Txt_Pstep[k].Text + "], ";
                        }
                        else
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_1[k].Text + "(  ) 단계 [ ], ";
                        }
                    }
                    SS2_Sheet1.Cells[17 + 2, 2].Text = VB.Space(3) + StrTemp;
                    

                    //SS2.Row = 19
                    //SS2.Col = 3: 
                    StrTemp = "";
                    for (k = 5; k <= 8; k++)
                    {
                        if (ChkP_1[k].Checked == true)
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_1[k].Text + "(√) 단계 [" + Txt_Pstep[k].Text + "], ";
                        }
                        else
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_1[k].Text + "(  ) 단계 [ ], ";
                        }
                    }
                    SS2_Sheet1.Cells[18 + 2, 2].Text = VB.Space(3) + StrTemp;
                    //SS2.Row = 20
                    //SS2.Col = 3: 
                    StrTemp = "";

                    if (ChkP_19.Checked == true)
                    {
                        StrTemp = StrTemp + (k + 1) + "." + ChkP_1[k].Text + "{ " + Txt_Petc.Text + " }" + "(√) 단계 [" + Txt_Pstep[k].Text + "], ";
                    }

                    SS2_Sheet1.Cells[19 + 2, 2].Text = VB.Space(3) + StrTemp;

                    //'요인
                    //SS2.Row = 24
                    //SS2.Col = 3: 
                    StrTemp = "";
                    for (k = 0; k <= 5; k++)
                    {
                        if (ChkP_2[k].Checked == true)
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_2[k].Text + "(√), ";
                        }
                        else
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_2[k].Text + "( ), ";
                        }
                    }
                    SS2_Sheet1.Cells[23 + 2, 2].Text = VB.Space(3) + StrTemp;
                    //SS2.Row = 25
                    //SS2.Col = 3: 
                    StrTemp = "";
                    for (k = 6; k <= 11; k++)
                    {
                        if (ChkP_2[k].Checked == true)
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_2[k].Text + "(√), ";
                        }
                        else
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_2[k].Text + "( ), ";
                        }
                    }
                    SS2_Sheet1.Cells[24 + 2, 2].Text = VB.Space(3) + StrTemp;


                    //SS2.Row = 26
                    //SS2.Col = 3: 
                    StrTemp = "";
                    for (k = 12; k <= 13; k++)
                    {
                        if (ChkP_2[k].Checked == true)
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_2[k].Text + "(√), ";
                        }
                        else
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_2[k].Text + "( ), ";
                        }


                    }
                    SS2_Sheet1.Cells[25 + 2, 2].Text = VB.Space(3) + StrTemp;


                    //SS2.Row = 30
                    //SS2.Col = 3: 
                    StrTemp = "";
                    for (k = 0; k <= 3; k++)
                    {
                        if (ChkP_3[k].Checked == true)
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_3[k].Text + "(√), ";
                        }
                        else
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_3[k].Text + "( ), ";
                        }
                    }
                    SS2_Sheet1.Cells[29 + 2, 2].Text = VB.Space(3) + StrTemp;


                    //SS2.Row = 31
                    //SS2.Col = 3: 
                    StrTemp = "";
                    for (k = 4; k <= 8; k++)
                    {
                        if (ChkP_3[k].Checked == true)
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_3[k].Text + "(√), ";
                        }
                        else
                        {
                            StrTemp = StrTemp + (k + 1) + "." + ChkP_3[k].Text + "( ), ";
                        }
                    }
                    SS2_Sheet1.Cells[30 + 2, 2].Text = VB.Space(3) + StrTemp;


                    //SS2.Row = 37
                    //SS2.Col = 10: 
                    //2019-07-22 주석처리
                    //SS2_Sheet1.Cells[36 + 2, 9].Text = "조사자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["EntSabun"].ToString().Trim());


                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    MessageBox.Show("욕창발생보고서 신규등록입니다.", "확인");
                    //'신규자료일경우
                    FstrFlag = "";
                    FstrROWID = "";

                    if (ComboWard.Text == "ER")
                    {
                        SQL = "";
                        SQL = " SELECT PANO, SNAME, AGE, SEX, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE, ";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(SYSDATE, 'YYYY-MM-DD') ACTDATE, '' DIAGNOSIS, 0 IPDNO, 0 GRADE, ";
                        SQL = SQL + ComNum.VBLF + " DEPTCODE, 0 ROOMCODE, '' WARDCODE";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                        SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND PANO = '" + argPTNO + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND DEPTCODE = 'ER' ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = " SELECT  a.Pano, a.SName, a.Age,a.Sex, TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,";
                        SQL = SQL + " TO_CHAR(SYSDATE,'YYYY-MM-DD') ActDate, ";
                        SQL = SQL + " a.DIAGNOSIS , a.Ipdno, a.Grade, b.DeptCode, b.RoomCode,DECODE(b.RoomCode,233,'SICU',234,'MICU',b.WardCode) WardCode ";
                        SQL = SQL + " FROM " + ComNum.DB_PMPA + "NUR_MASTER a, " + ComNum.DB_PMPA + "IPD_NEW_MASTER b ";
                        SQL = SQL + " WHERE a.Ipdno=b.Ipdno(+) ";
                        SQL = SQL + "  AND a.IpdNo =" + VB.Val(ArgIpdNo) + " ";
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
                        SS1_Sheet1.Cells[0, 0].Text = dt.Rows[0]["Indate"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 1].Text = dt.Rows[0]["ActDate"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 2].Text = CF.DATE_ILSU(clsDB.DbCon, dt.Rows[0]["ActDate"].ToString().Trim(), dt.Rows[0]["InDate"].ToString().Trim()).ToString();
                        SS1_Sheet1.Cells[0, 3].Text = dt.Rows[0]["Pano"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 4].Text = dt.Rows[0]["SName"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 5].Text = dt.Rows[0]["Sex"].ToString().Trim() + "/" + dt.Rows[0]["Age"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 6].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 7].Text = dt.Rows[0]["RoomCode"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 8].Text = READ_GRADE(dt.Rows[0]["IPDNO"].ToString().Trim(), dt.Rows[0]["InDate"].ToString().Trim(), dt.Rows[0]["WARDCODE"].ToString().Trim());
                        SS1_Sheet1.Cells[0, 9].Text = dt.Rows[0]["IpdNo"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 10].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 11].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                        TxtDiag.Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                        TxtP_Jumsu.Text = nTOT1.ToString();    //'등록일의 최근값
                    }

                    dt.Dispose();
                    dt = null;
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }


        private void ChkP_All_CheckedChanged(object sender, EventArgs e)
        {
            string strchk = "";
            if (((CheckBox)sender).Checked == true)
            {
                if (((CheckBox)sender).Checked == true)
                {
                    strchk = VB.Right(((CheckBox)sender).Name, 1); //이름값 뒤의 라이트로 숫자를 인식

                    if (ChkP_1[Convert.ToInt32(strchk)].Checked == true)
                    {
                        if (FstrFlag == "")
                        {
                            Txt_Pstep[Convert.ToInt32(strchk)].Focus();
                            if (ChkP_19.Checked == true)
                            {
                                Txt_Petc.Focus();
                            }
                        }
                    }
                }
            }
        }

        private void ChkToiwon_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkToiwon.Checked == true)
            {
                TxtDate.Enabled = true;
                TxtEDate.Enabled = true;
            }
            else
            {
                TxtDate.Enabled = false;
                TxtEDate.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        private void SCREEN_CLEAR()
        {
            int i = 0;
            TxtDiag.Text = "";
            TxtP_Jumsu.Text = "";
            Txt_Petc.Text = "";

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 1;
            SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            txtSeekDate.Text = "";
            txtReturnDate.Text = "";

            cboErrGrade.SelectedIndex = -1;
            cboErrGubun.SelectedIndex = -1;

            for (i = 0; i <= 9; i++)
            {
                Txt_Pstep[i].Text = "";
            }
            Opt_P21.Checked = true;

            for (i = 0; i <= 3; i++)
            {
                Opt_P3[i].Checked = false;
            }

            for (i = 0; i <= 9; i++)
            {
                ChkP_1[i].Checked = false;
            }

            for (i = 0; i <= 13; i++)
            {
                ChkP_2[i].Checked = false;
            }

            for (i = 0; i <= 8; i++)
            {
                ChkP_3[i].Checked = false;
            }

            //인쇄용시트 SS2

            //SS2_Sheet1.Cells[6]
            for (i = 3; i <= 11; i++)
            {
                SS2_Sheet1.Cells[6, i - 1].Text = "";
            }

            for (i = 11; i <= 14; i++)
            {
                SS2_Sheet1.Cells[i - 1, 4].Text = "";
            }

            for (i = 19; i <= 21; i++)
            {
                SS2_Sheet1.Cells[i, 2].Text = "";
            }

            for (i = 25; i <= 27; i++)
            {
                SS2_Sheet1.Cells[i, 2].Text = "";
            }

            for (i = 31; i <= 36; i++)
            {
                SS2_Sheet1.Cells[i, 2].Text = "";
            }

            SS2_Sheet1.Cells[36, 9].Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (Delete() == false)
            {
                return;
            }
            Search();

        }

        private bool Delete()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            if (FstrROWID == "")
            {
                MessageBox.Show("삭제할 보고서를 선택하여 주십시요.", "확인");
                return rtnVal;
            }


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " INSERT INTO ADMIN.NUR_PRESSURE_SORE_HIS";
                SQL += ComNum.VBLF + " SELECT * FROM ADMIN.NUR_PRESSURE_SORE";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = " DELETE ADMIN.NUR_PRESSURE_SORE";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

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
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void CmdJumsu_Click(object sender, EventArgs e)
        {
            //TODO clsPublic.GstrRetValue
            clsPublic.GstrRetValue = "";

            string strSName = "";
            string strPano = "";
            string strSex = "";
            string strAge = "";
            string strRoom = "";
            string strDept = "";
            double nIPDNO = 0;

            strPano = SS1_Sheet1.Cells[0, 3].Text.Trim();
            strSName = SS1_Sheet1.Cells[0, 4].Text.Trim();
            strSex = VB.Pstr(SS1_Sheet1.Cells[0, 5].Text, "/", 1);
            strAge = VB.Pstr(SS1_Sheet1.Cells[0, 5].Text, "/", 2);
            strDept = SS1_Sheet1.Cells[0, 6].Text.Trim();
            strRoom = SS1_Sheet1.Cells[0, 7].Text.Trim();
            nIPDNO = VB.Val(SS1_Sheet1.Cells[0, 9].Text);

            //TODO
            clsPublic.GstrHelpCode = strSName + "^^" + strPano + "^^" + strSex + "/" + strAge + "^^" + strRoom + "^^" + strDept + "^^" + nIPDNO + "^^";


            //TODO clsPublic.GstrRetValue
            if (clsPublic.GstrRetValue != "")
            {
                TxtP_Jumsu.Text = clsPublic.GstrRetValue;
            }
        }

        private void btnPreView_Click(object sender, EventArgs e)
        {
            if (btnPreView.Text == "크게보기")
            {
                btnPreView.Text = "작게보기";
                panTop.Size = new System.Drawing.Size(760, 0);
                panbotom.Size = new System.Drawing.Size(760, 635);
            }
            else if (btnPreView.Text == "작게보기")
            {
                btnPreView.Text = "크게보기";
                panTop.Size = new System.Drawing.Size(760, 257);
                panbotom.Size = new System.Drawing.Size(760, 378);
                
            }

            string StrTemp = "";
            int k = 0;

            SS2_Sheet1.Cells[6, 2].Text = SS1_Sheet1.Cells[0, 0].Text;

            if (SS1_Sheet1.Cells[0, 0].Text == "")
            {
                return;
            }

            SS2_Sheet1.Cells[6, 3].Text = SS1_Sheet1.Cells[0, 1].Text;
            SS2_Sheet1.Cells[6, 4].Text = SS1_Sheet1.Cells[0, 2].Text;
            SS2_Sheet1.Cells[6, 5].Text = SS1_Sheet1.Cells[0, 3].Text;
            SS2_Sheet1.Cells[6, 6].Text = SS1_Sheet1.Cells[0, 4].Text;
            SS2_Sheet1.Cells[6, 7].Text = SS1_Sheet1.Cells[0, 5].Text;
            SS2_Sheet1.Cells[6, 8].Text = SS1_Sheet1.Cells[0, 6].Text;
            SS2_Sheet1.Cells[6, 9].Text = SS1_Sheet1.Cells[0, 7].Text;
            SS2_Sheet1.Cells[6, 10].Text = SS1_Sheet1.Cells[0, 8].Text;

            SS2_Sheet1.Cells[3, 7].Text = SS1_Sheet1.Cells[0, 11].Text;

            SS2_Sheet1.Cells[8, 2].Text = txtSeekDate.Text;
            SS2_Sheet1.Cells[8, 3].Text = txtReturnDate.Text;
            SS2_Sheet1.Cells[8, 4].Text = clsVbfunc.GetInSaName(clsDB.DbCon, VB.Val(clsType.User.Sabun).ToString("00000"));

            SS2_Sheet1.Cells[7, 6].Text = cboErrGubun.Text;
            SS2_Sheet1.Cells[8, 5].Text = cboErrGrade.Text;

            SS2_Sheet1.Cells[10 + 2, 4].Text = VB.Space(3) + TxtDiag.Text.Trim();

            if (Opt_P20.Checked == true)
            {
                SS2_Sheet1.Cells[11 + 2, 4].Text = "유";
            }
            else
            {
                SS2_Sheet1.Cells[11 + 2, 4].Text = "무";
            }

            SS2_Sheet1.Cells[12 + 2, 4].Text = TxtP_Jumsu.Text.Trim();

            if (Opt_P30.Checked == true)
            {
                SS2_Sheet1.Cells[13 + 2, 4].Text = "완쾌";
            }
            else if (Opt_P31.Checked == true)
            {
                SS2_Sheet1.Cells[13 + 2, 4].Text = "악화";
            }
            else if (Opt_P32.Checked == true)
            {
                SS2_Sheet1.Cells[13 + 2, 4].Text = "사망";
            }
            else if (Opt_P33.Checked == true)
            {
                SS2_Sheet1.Cells[13 + 2, 4].Text = "퇴원";
            }

            //발생부위, 단계
            StrTemp = "";
            for (k = 0; k <= 4; k++)
            {
                if (ChkP_1[k].Checked == true)
                {
                    StrTemp += (k + 1) + "." + ChkP_1[k].Text + "(√) 단계 [" + Txt_Pstep[k].Text + "], ";
                }
                else
                {
                    StrTemp += (k + 1) + "." + ChkP_1[k].Text + "(  ) 단계 [ ], ";
                }
            }

            SS2_Sheet1.Cells[17 + 2, 2].Text = VB.Space(3) + StrTemp;
            StrTemp = "";

            for (k = 5; k <= 8; k++)
            {
                if (ChkP_1[k].Checked == true)
                {
                    StrTemp += (k + 1) + "." + ChkP_1[k].Text + "(√) 단계 [" + Txt_Pstep[k].Text + "], ";
                }
                else
                {
                    StrTemp += (k + 1) + "." + ChkP_1[k].Text + "(  ) 단계 [ ], ";
                }
            }

            SS2_Sheet1.Cells[18 + 2, 2].Text = VB.Space(3) + StrTemp;
            StrTemp = "";

            if (ChkP_10.Checked == true)
            {
                StrTemp += (k + 1) + "." + ChkP_1[k].Text + "{ " + Txt_Petc.Text + " }" + "(√) 단계 [" + Txt_Pstep[k].Text + "], ";
            }
            SS2_Sheet1.Cells[19 + 2, 2].Text = VB.Space(3) + StrTemp;

            //요인
            //SS2.Row = 24
            //SS2.Col = 3: 
            StrTemp = "";

            for (k = 0; k <= 5; k++)
            {
                if (ChkP_2[k].Checked == true)
                {
                    StrTemp += (k + 1) + "." + ChkP_2[k].Text + "(√), ";
                }
                else
                {
                    StrTemp += (k + 1) + "." + ChkP_2[k].Text + "( ), ";
                }
            }

            SS2_Sheet1.Cells[23 + 2, 2].Text = VB.Space(3) + StrTemp;

            //SS2.Row = 25
            //SS2.Col = 3: 
            StrTemp = "";

            for (k = 6; k <= 11; k++)
            {
                if (ChkP_2[k].Checked == true)
                {
                    StrTemp += (k + 1) + "." + ChkP_2[k].Text + "(√), ";
                }
                else
                {
                    StrTemp += (k + 1) + "." + ChkP_2[k].Text + "( ), ";
                }
            }

            SS2_Sheet1.Cells[24 + 2, 2].Text = VB.Space(3) + StrTemp;

            //SS2.Row = 30
            //SS2.Col = 3: 
            StrTemp = "";
            for (k = 0; k <= 3; k++)
            {
                if (ChkP_3[k].Checked == true)
                {
                    StrTemp += (k + 1) + "." + ChkP_3[k].Text + "(√), ";
                }
                else
                {
                    StrTemp += (k + 1) + "." + ChkP_3[k].Text + "( ), ";
                }
            }

            SS2_Sheet1.Cells[29 + 2, 2].Text = VB.Space(3) + StrTemp;

            // SS2.Row = 31
            //SS2.Col = 3: 
            StrTemp = "";
            for (k = 4; k <= 8; k++)
            {
                if (ChkP_3[k].Checked == true)
                {
                    StrTemp += (k + 1) + "." + ChkP_3[k].Text + "(√), ";
                }
                else
                {
                    StrTemp += (k + 1) + "." + ChkP_3[k].Text + "( ), ";
                }
            }

            SS2_Sheet1.Cells[30 + 2, 2].Text = VB.Space(3) + StrTemp;

            SS2_Sheet1.Cells[36 + 2, 9].Text = "조사자" + clsVbfunc.GetInSaName(clsDB.DbCon, VB.Val(clsType.User.Sabun).ToString("00000"));
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (ComFunc.MsgBoxQ("보고서를 저장하지 않고 인쇄하는 버튼입니다." + ComNum.VBLF + ComNum.VBLF +
               "저장하지 않으신 경우 '저장및인쇄' 버튼을 사용하십시요. 계속하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            SS2_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            SS2_Sheet1.PrintInfo.ZoomFactor = 1.15f;
            SS2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            //SS2_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            SS2_Sheet1.PrintInfo.Margin.Top = 20;
            SS2_Sheet1.PrintInfo.Margin.Bottom = 20;
            SS2_Sheet1.PrintInfo.Margin.Header = 10;
            SS2_Sheet1.PrintInfo.ShowColor = false;
            SS2_Sheet1.PrintInfo.ShowBorder = true;
            SS2_Sheet1.PrintInfo.ShowGrid = true;
            SS2_Sheet1.PrintInfo.ShowShadows = false;
            SS2_Sheet1.PrintInfo.UseMax = false;
            SS2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            SS2_Sheet1.PrintInfo.UseSmartPrint = false;
            SS2_Sheet1.PrintInfo.ShowPrintDialog = false;
            SS2_Sheet1.PrintInfo.Preview = false;
            SS2_Sheet1.PrintInfo.Centering = Centering.Both;
            SS2.PrintSheet(0);

        }

        /// <summary>
        /// 저장 및 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (Save() == false)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("자료저장완료!! 저장한 자료를 인쇄하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Print();

            //TODO GstrHelpCode 
            clsPublic.GstrHelpCode = "";
        }

        private bool Save()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            string strPano = "";
            string strName = "";
            string strAge = "";
            string strSex = "";
            string strDiag = "";
            string strIPDate = "";
            string strACTDATE = "";
            string strRoom = "";
            string strGrade = "";
            double nIPDNO = 0;
            string strDeptCode = "";
            double nTOT = 0;
            string strP_Bui = "";
            string strP_Bui_etc = "";
            string strP_Step = "";
            string strP_hapbung = "";
            string strProress = "";
            string strP_Yoin = "";
            string strPre = "";
            string strWard = "";
            string StrTemp = "";
            string strDate = "";

            string strSeekDate = "";
            string strReturnDate = "";
            string strErrGubun = "";
            string strErrGrade = "";

            DataTable dt = null;

            strPano = "";
            strName = "";
            strAge = "";
            strSex = "";
            strDiag = "";
            strIPDate = "";
            strACTDATE = "";
            strRoom = "";
            strGrade = "";
            strDeptCode = "";
            nIPDNO = 0;
            nTOT = 0;
            strP_Bui = "";
            strP_Bui_etc = "";
            strP_Step = "";
            strP_hapbung = "";
            strProress = "";
            strP_Yoin = "";
            strPre = "";
            strWard = "";
            StrTemp = "";

            Cursor.Current = Cursors.WaitCursor;

            if (SS1_Sheet1.Cells[0, 9].Text == "")
            {
                MessageBox.Show("저장 실패, 해당 환자를 먼저 확인해주세요.", "확인");
                return rtnVal;
            }

            if (SS1_Sheet1.Cells[0, 12].Text == "Y")
            {
                if (ComFunc.MsgBoxQ("이미 인쇄한 보고서입니다. 수정하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return rtnVal;
                }
            }

            // '기본정보 세팅
            strIPDate = SS1_Sheet1.Cells[0, 0].Text.Trim();
            strACTDATE = SS1_Sheet1.Cells[0, 1].Text.Trim();//).ToString("yyyy-MM-dd");
            strPano = SS1_Sheet1.Cells[0, 3].Text.Trim();
            strName = SS1_Sheet1.Cells[0, 4].Text.Trim();
            strSex = VB.Pstr(SS1_Sheet1.Cells[0, 5].Text, "/", 1);
            strAge = VB.Pstr(SS1_Sheet1.Cells[0, 5].Text, "/", 2);

            strDeptCode = SS1_Sheet1.Cells[0, 6].Text.Trim();
            strRoom = SS1_Sheet1.Cells[0, 7].Text.Trim();
            strGrade = SS1_Sheet1.Cells[0, 8].Text.Trim();
            nIPDNO = VB.Val(SS1_Sheet1.Cells[0, 9].Text);
            strWard = SS1_Sheet1.Cells[0, 10].Text.Trim();
            strDate = SS1_Sheet1.Cells[0, 11].Text.Trim();

            strSeekDate = txtSeekDate.Text.Trim();
            strReturnDate = txtReturnDate.Text.Trim();

            strErrGubun = cboErrGubun.Text.Trim();
            strErrGrade = cboErrGrade.Text.Trim();

            strDiag = TxtDiag.Text.Trim();
            nTOT = 0;
            StrTemp = "";

            for (i = 0; i <= 9; i++)
            {
                if (ChkP_1[i].Checked == true)
                {
                    strP_Bui += "1^^";
                    StrTemp = "1";
                }
                else
                {
                    strP_Bui += "0^^";
                }

                if (Txt_Pstep[i].Text != "")
                {
                    strP_Step += Txt_Pstep[i].Text + "^^";
                }
                else
                {
                    strP_Step += "^^";
                }
            }

            if (StrTemp == "")
            {
                MessageBox.Show("욕창 발생부위를 선택하세요.", "확인");
                return rtnVal;
            }

            strP_Bui_etc = Txt_Petc.Text.Trim();

            if (Opt_P20.Checked == true)
            {
                strP_hapbung = "Y";
            }
            else
            {
                strP_hapbung = "N";
            }
            if (Opt_P30.Checked == true)
            {
                strProress = "1";
            }
            else if (Opt_P31.Checked == true)
            {
                strProress = "2";
            }
            else if (Opt_P32.Checked == true)
            {
                strProress = "3";
            }
            else if (Opt_P33.Checked == true)
            {
                strProress = "4";
            }
            else
            {
                strProress = "0";
            }
            StrTemp = "";

            for (i = 0; i <= 13; i++)
            {
                if (ChkP_2[i].Checked == true)
                {
                    strP_Yoin += "1^^";
                    StrTemp = "1";
                }
                else
                {
                    strP_Yoin += "0^^";
                }
            }

            if (StrTemp == "")
            {
                MessageBox.Show("욕창 발생요인을 선택하세요.", "확인");
                return rtnVal;
            }
            StrTemp = "";
            for (i = 0; i <= 8; i++)
            {
                if (ChkP_3[i].Checked == true)
                {
                    strPre += "1^^";
                    StrTemp = "1";
                }
                else
                {
                    strPre += "0^^";
                }
            }

            if (StrTemp == "")
            {
                MessageBox.Show("욕창예방활동을 선택하세요", "확인");
                return rtnVal;
            }
            if (strPano == "")
            {
                MessageBox.Show("등록번호가 공백입니다.", "확인");
                return rtnVal;
            }
            if (strName == "")
            {
                MessageBox.Show("성명이 공백입니다.", "확인");
                return rtnVal;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL += ComNum.VBLF + " FROM NUR_PRESSURE_SORE ";
                SQL += ComNum.VBLF + " WHERE IPDNO = " + nIPDNO;
                SQL += ComNum.VBLF + "   AND ENTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                else
                {
                    FstrROWID = "";
                }

                dt.Dispose();
                dt = null;

                if (FstrROWID != "")
                {
                    SQL = "";
                    SQL = "INSERT INTO ADMIN.NUR_PRESSURE_SORE_HIS  ";
                    SQL += ComNum.VBLF + " SELECT * FROM ADMIN.NUR_PRESSURE_SORE ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = "DELETE ADMIN.NUR_PRESSURE_SORE ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                }

                SQL = "";
                SQL = "INSERT INTO ADMIN.NUR_PRESSURE_SORE ( ";
                SQL += ComNum.VBLF + " PANO,SNAME,SEX,AGE,ACTDATE,INDATE,DIAGNOSYS,DeptCode,RoomCode,Grade,Total, ";
                SQL += ComNum.VBLF + " P_Balbui,P_BalBui_etc,P_Step,P_Hapbung,P_Progress,P_Yoin,P_Pre,Ipdno,WardCode, ";
                SQL += ComNum.VBLF + "  ENTDATE, ENTSABUN, ";
                SQL += ComNum.VBLF + "  SEEKDATE, RETURNDATE, ERRGUBUN, ERRGRADE  ";
                SQL += ComNum.VBLF + "  ) VALUES ('" + strPano + "','" + strName + "' , ";
                SQL += ComNum.VBLF + " '" + strSex + "' , '" + strAge + "', ";
                SQL += ComNum.VBLF + " TO_DATE('" + strACTDATE + "','YYYY-MM-DD'),TO_DATE('" + strIPDate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + " '" + strDiag + "' ,'" + strDeptCode + "', '" + strRoom + "','" + strGrade + "' , " + nTOT + ", ";
                SQL += ComNum.VBLF + " '" + strP_Bui + "','" + strP_Bui_etc + "', '" + strP_Step + "','" + strP_hapbung + "','" + strProress + "' ,";
                SQL += ComNum.VBLF + "  '" + strP_Yoin + "','" + strPre + "', " + nIPDNO + ",'" + strWard + "', ";
                SQL += ComNum.VBLF + " TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD'), " + clsType.User.Sabun + ", ";
                SQL += ComNum.VBLF + " TO_DATE('" + strSeekDate + "','YYYY-MM-DD HH24:MI'), TO_DATE('" + strReturnDate + "','YYYY-MM-DD HH24:MI'), '" + strErrGubun + "','" + strErrGrade + "'";
                SQL += ComNum.VBLF + " ) ";
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

        /// <summary>
        /// 저장 및 출력에 붙어야할 함수
        /// </summary>
        /// <returns></returns>
        private bool Print()
        {
            //'    MsgBox "저장버튼을 클릭하면 저장 후 인쇄가 됩니다."
            //'    Exit Sub

            string strInDate = "";
            string strPano = "";
            string strBDATE = "";
            string strIPDNO = "";

            //clsSpread.SpdPrint_Margin setMargin;
            //clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();
            //string strTitle = "";
            //string strHeader = "";
            //string strFooter = "";
            //bool PrePrint = true;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;



            if (SS1_Sheet1.Cells[0, 0].Text == "Y")
            {
                MessageBox.Show("환자를 먼저 선택하세요", "확인");
                return rtnVal;
            }

            if (SS1_Sheet1.Cells[0, 12].Text == "Y")
            {
                if (ComFunc.MsgBoxQ("이미 인쇄한 보고서입니다. 다시 인쇄하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return rtnVal;
                }
            }

            //strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            //strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            //CS.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both);

            SS2_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            SS2_Sheet1.PrintInfo.ZoomFactor = 1.15f;
            SS2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            SS2_Sheet1.PrintInfo.Margin.Top = 20;
            SS2_Sheet1.PrintInfo.Margin.Bottom = 20;
            SS2_Sheet1.PrintInfo.Margin.Header = 10;
            SS2_Sheet1.PrintInfo.ShowColor = false;
            SS2_Sheet1.PrintInfo.ShowBorder = true;
            SS2_Sheet1.PrintInfo.ShowGrid = true;
            SS2_Sheet1.PrintInfo.ShowShadows = false;
            SS2_Sheet1.PrintInfo.UseMax = false;
            SS2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            SS2_Sheet1.PrintInfo.UseSmartPrint = false;
            SS2_Sheet1.PrintInfo.ShowPrintDialog = false;
            SS2_Sheet1.PrintInfo.Preview = false;
            SS2_Sheet1.PrintInfo.Centering = Centering.Both;
            SS2.PrintSheet(0);


            strInDate = SS1_Sheet1.Cells[0, 0].Text;
            strBDATE = SS1_Sheet1.Cells[0, 1].Text;
            strPano = SS1_Sheet1.Cells[0, 3].Text;
            strIPDNO = SS1_Sheet1.Cells[0, 9].Text;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";

                if (ComboWard.Text == "ER")
                {
                    SQL = " UPDATE NUR_PRESSURE_SORE SET";
                    SQL += ComNum.VBLF + " PRTYN = 'Y' ";
                    SQL += ComNum.VBLF + " WHERE IPDNO = 0";
                    SQL += ComNum.VBLF + "   AND INDATE = TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  AND PANO = '" + strPano + "' ";
                    SQL += ComNum.VBLF + "  AND DEPTCODE = 'ER'";
                }
                else
                {
                    SQL = " UPDATE NUR_PRESSURE_SORE SET";
                    SQL += ComNum.VBLF + " PRTYN = 'Y' ";
                    SQL += ComNum.VBLF + " WHERE IPDNO = " + strIPDNO;
                    SQL += ComNum.VBLF + "   AND TRUNC(ACTDATE) = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
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

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

        }

        private void Search()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            string strWard = "";
            string strToDate = "";
            string strNextDate = "";
            string strRemark = "";
            string strOK = "";
            int nRow = 0;
            string strFlag = "";

            ComFunc CF = new ComFunc();

            Cursor.Current = Cursors.WaitCursor;

            SSList_Sheet1.Rows.Count = 0;
            SSHistory_Sheet1.Rows.Count = 0;

            strToDate = Convert.ToDateTime(TxtDate.Text).ToString("yyyy-MM-dd");
            strNextDate = CF.DATE_ADD(clsDB.DbCon, strToDate, 1);

            strWard = (ComboWard.Text).Trim();

            try
            {
                if (ComboWard.Text == "ER")
                {
                    ChkDaesang.Checked = false;
                    SQL = "";
                    SQL = " SELECT 0 IPDNO, PANO, SNAME, SEX, AGE, DEPTCODE, DRCODE, 1 ILSU, BI,  0 ReliGion, GBSPC, 0 WARDCODE, 0 ROOMCODE, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE,";
                    SQL += ComNum.VBLF + " BDATE ACTDATE, BDATE OUTDATE, '' JIYUK, 0";
                    SQL += ComNum.VBLF + "  FROM ADMIN.OPD_MASTER";
                    SQL += ComNum.VBLF + " WHERE BDate >= TO_DATE('" + TxtDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND BDate <= TO_DATE('" + TxtEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND DEPTCODE = 'ER'";
                    SQL += ComNum.VBLF + " ORDER BY SNAME ASC";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT a.IPDNO, a.Pano, a.SName, a.Sex, a.Age, a.DeptCode,a.DrCode,a.ILSU, ";
                    SQL += ComNum.VBLF + "  a.Bi, a.ReliGion, a.GbSpc, a.WardCode, a.RoomCode, ";
                    SQL += ComNum.VBLF + " TO_CHAR(A.INDATE,'YYYY-MM-DD')  INDATE , ";
                    SQL += ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , ";
                    SQL += ComNum.VBLF + "  TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE , ";
                    SQL += ComNum.VBLF + "  TO_CHAR(a.InDate,'YYYY-MM-DD') InDate, a.JiYuk, b.TBed ";
                    SQL += ComNum.VBLF + "  FROM IPD_NEW_MASTER a,Bas_Room b ";
                    if (ChkToiwon.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  WHERE a.IpwonTime >= TO_DATE('" + TxtDate.Value.ToString("yyyy-MM-dd") + " 00:01','YYYY-MM-DD HH24:MI') ";
                        SQL += ComNum.VBLF + "  AND a.IpwonTime <= TO_DATE('" + TxtEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "  WHERE ((A.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND a.OutDate IS NULL) OR a.OutDate>=TO_DATE('" + strNextDate + "','YYYY-MM-DD')) ";
                        SQL += ComNum.VBLF + "  AND a.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    }
                    SQL += ComNum.VBLF + "  AND a.Amset4 <> '3' ";
                    SQL += ComNum.VBLF + "  AND a.Pano < '90000000' ";
                    //SQL += ComNum.VBLF + "  AND a.Pano <> '81000004' ";
                    SQL += ComNum.VBLF + "  AND a.RoomCode = b.RoomCode(+) ";

                    if (chkWrite.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND A.IPDNO IN (SELECT IPDNO FROM NUR_PRESSURE_SORE ";
                        SQL += ComNum.VBLF + "                   WHERE ACTDATE >= TO_DATE('" + TxtDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                     AND ACTDATE <= TO_DATE('" + TxtEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                    }
                    else
                    {
                        switch (ComboWard.Text.Trim())
                        {
                            case "전체":
                                break;
                            case "SICU":
                                SQL += ComNum.VBLF + " AND a.RoomCode = '233' ";
                                break;
                            case "MICU":
                                SQL += ComNum.VBLF + " AND a.RoomCode = '234' ";
                                break;
                            case "ND":
                            case "NR":
                                SQL += ComNum.VBLF + " AND a.RoomCode IN('369','358','368','640','641','642')  ";
                                break;
                            default:
                                // SQL += ComNum.VBLF + " AND A.WardCode IN (" + ReadInWard(ComboWard.Text).Trim() + ") ";
                                SQL = SQL + ComNum.VBLF + " AND (( a.WardCode IN (" + ReadInWard(ComboWard.Text).Trim() + ") ) ";
                                SQL = SQL + ComNum.VBLF + " OR  ( a.IPDNO IN (SELECT IPDNO FROM NUR_PRESSURE_SORE ";
                                SQL = SQL + ComNum.VBLF + "                   WHERE ACTDATE >= TO_DATE('" + TxtDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "                     AND ACTDATE <= TO_DATE('" + TxtEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "                     AND WardCode IN (" + ReadInWard(ComboWard.Text).Trim() + ") ) ) ) ";
                                break;
                        }

                        SQL += ComNum.VBLF + "  ORDER BY a.RoomCode,a.SName,a.Indate DESC   ";
                    }
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
                    progressBar1.Value = 0;
                    progressBar1.Maximum = SSList_Sheet1.RowCount = dt.Rows.Count;


                    SSList_Sheet1.Rows.Count = dt.Rows.Count;
                    SSList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = (i + 1);
                        strOK = "";
                        strFlag = "";

                        SQL = "";
                        SQL = " SELECT Remark ";
                        SQL += ComNum.VBLF + " FROM NUR_MASTER WHERE Ipdno ='" + dt.Rows[i]["Ipdno"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strRemark = dt1.Rows[0]["Remark"].ToString().Trim();

                            if (strRemark != "")
                            {
                                if (VB.I(strRemark, "▶욕창,") > 1)
                                {
                                    strFlag = "OK";
                                }
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (ChkDaesang.Checked == true)
                        {
                            if (strFlag == "OK")
                            {
                                nRow += 1;
                                strOK = "OK";
                            }
                        }
                        else
                        {
                            nRow += 1;
                            strOK = "OK";
                        }

                        if ((strFlag == "OK" && ChkDaesang.Checked == true) || (strOK == "OK" && ChkDaesang.Checked == false))
                        {
                            SSList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                            SSList_Sheet1.Cells[nRow - 1, 1].Text = ComFunc.LPAD(dt.Rows[i]["Pano"].ToString().Trim(), 8, "0");
                            SSList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                            SSList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                            SSList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            SSList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Ipdno"].ToString().Trim();

                            if (strFlag == "OK")
                            {
                                SSList_Sheet1.Cells[nRow - 1, 6].Text = "OK";
                            }

                            if (ComboWard.Text == "ER")
                            {
                                SQL = "";
                                SQL = " SELECT ROWID ";
                                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_PRESSURE_SORE ";
                                SQL += ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                                SQL += ComNum.VBLF + "   AND IPDNO = 0 ";
                                SQL += ComNum.VBLF + "   AND INDATE = TO_DATE('" + dt.Rows[i]["Indate"].ToString().Trim() + "','YYYY-MM-DD') ";
                                SQL += ComNum.VBLF + "   AND DEPTCODE = 'ER'";
                            }
                            else
                            {
                                SQL = "";
                                SQL = " SELECT ROWID ";
                                SQL = SQL + " FROM " + ComNum.DB_PMPA + "NUR_PRESSURE_SORE WHERE IPDNO = " + dt.Rows[i]["Ipdno"].ToString().Trim() + " ";
                            }

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                SSList_Sheet1.Cells[nRow - 1, 0, nRow - 1, 6].ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                                SSList_Sheet1.Cells[nRow - 1, 0, nRow - 1, 6].BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                            }
                            dt1.Dispose();
                            dt1 = null;

                            strOK = "";
                            strFlag = "";
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                SSList_Sheet1.RowCount = nRow;

                SCREEN_CLEAR();
                
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }


        }

        /// <summary>
        /// 중증도 체크
        /// </summary>
        /// <returns></returns>
        private string READ_GRADE(string ArgIpdNo, string ArgInDate, string argWARD)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnStr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ReadICUSelect(argWARD) == true)
                {
                    SQL = "";
                    SQL = " SELECT GRADE FROM NUR_SERIOUS ";
                    SQL += ComNum.VBLF + " WHERE JobTime >=TO_DATE('" + ArgInDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  AND IPDNO =" + ArgIpdNo + " ";
                    SQL += ComNum.VBLF + "   AND GBICU = 'Y' ";
                    SQL += ComNum.VBLF + " ORDER BY JobTime Desc ";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT GRADE FROM NUR_SERIOUSK ";
                    SQL += ComNum.VBLF + " WHERE JobTime >=TO_DATE('" + ArgInDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  AND IPDNO =" + ArgIpdNo + " ";
                    SQL += ComNum.VBLF + " ORDER BY JobTime Desc ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnStr;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnStr = dt.Rows[0]["Grade"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnStr;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnStr;
            }

        }

        private bool ReadICUSelect(string argWARD, string argRoom = "")
        {
            bool rtnVal = false;

            switch (argWARD)
            {
                case "ICU":
                case "SICU":
                case "MICU":
                case "CCU":
                case "32":
                case "33":
                case "35":
                    rtnVal = true;
                    break;
            }
            return rtnVal;
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS1_Sheet1.Cells[e.Row, e.Column].Text == "")
            {
                return;
            }

            frmCalendar frmCalendarX = new frmCalendar();
            frmCalendarX.StartPosition = FormStartPosition.CenterParent;
            frmCalendarX.ShowDialog();

            SS1_Sheet1.Cells[e.Row, 1].Text = clsPublic.GstrCalDate;
        }

        private void SSHistory_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strIPDNO = "";
            string strDate = "";
            string strInDate = "";
            string strPano = "";

            strInDate = SSHistory_Sheet1.Cells[e.Row, 0].Text;
            strDate = SSHistory_Sheet1.Cells[e.Row, 1].Text;
            strPano = SSHistory_Sheet1.Cells[e.Row, 2].Text;
            strIPDNO = SSHistory_Sheet1.Cells[e.Row, 6].Text;


            if (ComboWard.Text == "ER")
            {
                Info_Display(strIPDNO, strDate, strPano);
            }
            else
            {
                Info_Display(strIPDNO, strDate);
            }
        }

        private void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strPano = "";
            string strIPDNO = "";
            string strInDate = "";

            strInDate = SSList_Sheet1.Cells[e.Row, 0].Text.Trim();
            strPano = SSList_Sheet1.Cells[e.Row, 1].Text.Trim();
            strIPDNO = SSList_Sheet1.Cells[e.Row, 5].Text.Trim();

            SSHistory_Sheet1.Rows.Count = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (SSList_Sheet1.Cells[e.Row, 6].Text == "OK" || ComboWard.Text == "ER")
                {

                    if (ComFunc.MsgBoxQ("신규로 작성하시려면 '예', 기존자료를 보실려면 '아니요' 를 선택하십시요", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        FstrROWID = "";

                        SQL = "";
                        SQL = "  SELECT PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD') AS ActDate,TO_CHAR(InDate,'YYYY-MM-DD') AS InDate,  ";
                        SQL += ComNum.VBLF + " SNAME, ROOMCODE,DEPTCODE,TO_CHAR(ENTDATE,'YYYY-MM-DD') AS ENTDATE,WardCode,ROWID, PRTYN ";
                        SQL += ComNum.VBLF + " FROM ADMIN.NUR_PRESSURE_SORE ";
                        SQL += ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            SSHistory_Sheet1.Rows.Count = dt.Rows.Count;

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                SSHistory_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Ipdno"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PRTYN"].ToString().Trim();
                            }
                            dt.Dispose();
                            dt = null;
                        }
                        else
                        {
                            dt.Dispose();
                            dt = null;

                            if (ComboWard.Text == "ER")
                            {
                                Info_Display(strIPDNO, strInDate, strPano);
                            }
                            else
                            {
                                Info_Display(strIPDNO);
                            }
                        }
                    }
                    else
                    {
                        if (ComboWard.Text == "ER")
                        {
                            Info_Display(strIPDNO, strInDate, strPano);
                        }
                        else
                        {
                            Info_Display(strIPDNO, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
                        }
                    }
                }
                else
                {
                    if (SSList_Sheet1.Cells[e.Row, 6].BackColor == Color.FromArgb(128, 255, 128))
                    {
                        FstrROWID = "";
                        SQL = "";
                        SQL = "  SELECT PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,TO_CHAR(InDate,'YYYY-MM-DD')InDate,  ";
                        SQL += ComNum.VBLF + " SNAME, ROOMCODE,DEPTCODE,ENTDATE,WardCode,ROWID, PRTYN ";
                        SQL += ComNum.VBLF + " FROM ADMIN.NUR_PRESSURE_SORE ";
                        SQL += ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            SSHistory_Sheet1.Rows.Count = dt.Rows.Count;

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                SSHistory_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Ipdno"].ToString().Trim();
                                SSHistory_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PRTYN"].ToString().Trim();
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                    else
                    {
                        SCREEN_CLEAR();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ComboWard_SET()
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            
            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  WardCode, WardName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND WARDCODE NOT IN ('IU','NP','2W','NR','IQ','ER') ";
            SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
            SQL += ComNum.VBLF + "ORDER BY WardCode ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            ComboWard.Items.Clear();
            ComboWard.Items.Add("전체");

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ComboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }
            }

            ComboWard.Items.Add("ER");

            dt.Dispose();
            dt = null;

            ComboWard.SelectedIndex = 0;

            if (gsWard != "")
            {
                for (i = 0; i < ComboWard.Items.Count; i++)
                {
                    if (ComboWard.Items[i].ToString() == gsWard)
                    {
                        ComboWard.SelectedIndex = i;
                        ComboWard.Enabled = false;
                        break;
                    }
                }
            }
            if (gsWard == "ER")
            {
                ComboWard.Enabled = true;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Search();
        }

        private string ReadInWard(string argWard)
        {
            //과거 병동 데이터 조회 되도록 프로그램
            //쿼리 사용시 IN으로 조회해야함.

            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            int i = 0;
            DataTable dt1 = null;


            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  CODE ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND GUBUN = 'NUR_과거병동조회'";
            SQL += ComNum.VBLF + "      AND NAME = '" + argWard + "'";
            SQL += ComNum.VBLF + "      AND DELDATE IS NULL";

            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt1.Rows.Count > 0)
            {
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    rtnVal = rtnVal + dt1.Rows[i]["CODE"].ToString().Trim() + "','";
                }

                rtnVal = "'" + rtnVal;
                rtnVal = VB.Mid(rtnVal, 1, (rtnVal.Length) - 2);
            }

            else
            {
                rtnVal = "'" + argWard + "'";
            }

            dt1.Dispose();
            dt1 = null;
            return rtnVal;
        }

        private void btnEMR_Click(object sender, EventArgs e)
        {
            if(SS1_Sheet1.Cells[0,3].Text.Trim().Length > 0)
            {
                clsVbEmr.EXECUTE_TextEmrViewEx(SS1_Sheet1.Cells[0, 3].Text, clsType.User.Sabun);
                return;
            }

            if (SSList_Sheet1.RowCount == 0) return;

            clsVbEmr.EXECUTE_TextEmrViewEx(SSList_Sheet1.Cells[SSList_Sheet1.ActiveRowIndex, 1].Text.Trim(), clsType.User.Sabun);
            return;
        }

        private void cboErrGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboErrGrade.Items.Clear();
            cboErrGrade.Text = "";

            if (cboErrGubun.Text.Trim() == "근접오류")
            {
                cboErrGrade.Items.Add("등급1. 오류가 발생할 위험이 있는 상황");
                cboErrGrade.Items.Add("등급2. 오류가 발생하였으나 환자에게 도달하지 않음");
            }
            else if (cboErrGubun.Text.Trim() == "위해사건")
            {
                cboErrGrade.Items.Add("등급3. 환자에게 투여/적용되었으나 해가 없음");
                cboErrGrade.Items.Add("등급4. 환자에게 투여/적용되었으며 추가적인 관찰이 필요함");
                cboErrGrade.Items.Add("등급5. 일시적 손상으로 중재가 필요함");
                cboErrGrade.Items.Add("등급6. 일시적 손상으로 입원기간이 연장됨");
                cboErrGrade.Items.Add("등급7. 생명을 유지하기 위해 필수적인 중재가 필요함");
            }
            else if (cboErrGubun.Text.Trim() == "위해사건")
            {
                cboErrGrade.Items.Add("등급8. 영구적 손상");
                cboErrGrade.Items.Add("등급9. 환자사망");
            }
            cboErrGrade.SelectedIndex = -1;

        }

        private void btnGrade1_Click(object sender, EventArgs e)
        {
            pic1.Image = Properties.Resources.욕창_1단계1;
            pic1.Visible = true;
            txtPic1.Visible = true;
        }

        private void btnGrade2_Click(object sender, EventArgs e)
        {
            pic1.Image = Properties.Resources.욕창_2단계1;
            pic1.Visible = true;
            txtPic1.Visible = true;
        }

        private void btnGrade3_Click(object sender, EventArgs e)
        {
            pic1.Image = Properties.Resources.욕창_3단계1;
            pic1.Visible = true;
            txtPic1.Visible = true;
        }

        private void btnGrade4_Click(object sender, EventArgs e)
        {
            pic1.Image = Properties.Resources.욕창_4단계1;
            pic1.Visible = true;
            txtPic1.Visible = true;
        }

        private void btnGrade5_Click(object sender, EventArgs e)
        {
            pic1.Image = Properties.Resources.욕창_미분류1;
            pic1.Visible = true;
            txtPic1.Visible = true;
        }

        private void btnGrade6_Click(object sender, EventArgs e)
        {
            pic1.Image = Properties.Resources.욕창_심부손상의심1;
            pic1.Visible = true;
            txtPic1.Visible = true;
        }

        private void pic1_DoubleClick(object sender, EventArgs e)
        {
            pic1.Visible = false;
            txtPic1.Visible = false;
        }
    }
}
