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
    /// File Name       : frmPmpaViewCS.cs
    /// Description     : 고객상담조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-08
    /// Update History  : 2017-10-23
    /// 출력 및 조회 부분 수정
    /// <history>     
    /// 컨버전여부에는 Y로 되어있으나 VB에서는 사용안되는것으로 판단됨... 
    /// d:\psmh\Etc\csinfo\CsInfo61.frm(FrmCSView) => frmPmpaViewCS.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Etc\csinfo\CsInfo61.frm(FrmCSView)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewCS : Form
    {
        ComFunc CF = new ComFunc();
        string mstrPano = "";

        public delegate void SendPano(string strPano);
        public event SendPano rSendPano;
        public frmPmpaViewCS()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

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

            Set_Combo();

            groupBox5.Visible = false;
        }

        void Set_Combo()
        {
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            cboGu.Items.Add("0.전체");
            cboGu.Items.Add("1.재원");
            cboGu.Items.Add("3.퇴원");
            cboGu.Items.Add("4.외래");
            cboGu.Items.Add("5.기타");

            cboGu.SelectedIndex = 0;

            cboBun.Items.Add("1.상담일자별");
            cboBun.Items.Add("2.환자명별");
            cboBun.Items.Add("3.상담구분별");
            cboBun.Items.Add("4.상담자별");
            cboBun.Items.Add("5.진료과별");
            cboBun.Items.Add("6.진료과장별");
            cboBun.Items.Add("7.병원이유별");
            cboBun.SelectedIndex = 0;


            dtpFdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-30).ToShortDateString();
            dtpTdate.Text = CurrentDate;
            txtName.Text = "";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                //                
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                //                
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            string strLine1 = VB.Space(30) + "┌─┬────┬────┬────┐ " + "\n";
            string strLine2 = VB.Space(30) + "│결│담　  당│계　  장│팀　  장│ " + "\n";
            string strLine3 = VB.Space(30) + "│　├────┼────┼────┤ " + "\n";
            string strLine4 = VB.Space(30) + "│　│　　　　│　　　　│　　　　│ " + "\n";
            string strLine5 = VB.Space(30) + "│재│　　　　│　　　　│　　　  │ " + "\n";
            string strLine6 = VB.Space(30) + "└─┴────┴────┴────┘ " + "\n";



            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;



            #endregion

            strTitle = "고객정보 변경 내역";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strHeader += SPR.setSpdPrint_String(strLine1, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine2, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine3, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine4, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine5, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine6, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("작업기간: " + dtpFdate.Text + " ~ " + dtpTdate.Text + "\r\n" + "출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 55, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int nRead = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            string strPano = "";
            string strName = "";
            string strAge = "";
            string strSex = "";
            string strTel = "";
            string strJuso = "";
            string strGu = "";
            string strBun = "";
            string strSName = "";
            string strTdate = "";
            string strFdate = "";
            string strSub = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strSDate = "";
            string strZipCode = "";
            string strRemark = "";
            string strFeed = "";

            int nCNT = 0;
            int nCount = 0;

            ssList_Sheet1.Rows.Count = 0;

            nCount = 1;

            strGu = VB.Left(cboGu.SelectedItem.ToString(), 1);
            strBun = VB.Left(cboBun.SelectedItem.ToString(), 1);
            strTdate = dtpTdate.Text;
            strFdate = dtpFdate.Text;

            switch (strBun)
            {
                case "2":
                    strName = txtName.Text;
                    break;
                case "3":
                case "5":
                case "7":
                    strSub = VB.Left(cboSub.SelectedItem.ToString(), 2);
                    break;
                case "4":
                    strSub = VB.Mid(cboSub.SelectedItem.ToString(), 3, 10);
                    break;
                case "6":
                    strSub = VB.Left(cboSub.SelectedItem.ToString(), 4);
                    break;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                        ";
            SQL += ComNum.VBLF + "  A.PANO, TO_CHAR(A.SDATE,'YYYY-MM-DD') SDATE, B.ZIPCODE1, B.ZIPCODE2                         ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_SANGDAM A, " + ComNum.DB_PMPA + "BAS_PATIENT B          ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "      AND A.SDATE >= TO_DATE('" + strFdate + "','YYYY-MM-DD')                                 ";
            SQL += ComNum.VBLF + "      AND A.SDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD')                                 ";

            switch (strBun)
            {
                case "2":
                    SQL += ComNum.VBLF + "      AND A.SDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD')                         ";
                    break;
                case "3":
                    SQL += ComNum.VBLF + "      AND A.GUBUN = '" + strSub + "'                                                  ";
                    break;
                case "4":
                    SQL += ComNum.VBLF + "      AND A.ENTNAME = '" + strSub + "'                                                ";
                    break;
                case "5":
                    SQL += ComNum.VBLF + "      AND A.DEPTCODE  = '" + strSub + "'                                              ";
                    break;
                case "6":
                    SQL += ComNum.VBLF + "      AND A.DRCODE = '" + strSub + "'                                                 ";
                    break;
                case "7":
                    SQL += ComNum.VBLF + "      AND A.BUN" + Convert.ToInt32(strSub) + " = '1'                                  ";
                    break;
            }

            if (strGu != "0")
            {
                SQL += ComNum.VBLF + "      AND JOB = '" + strGu + "'                                                           ";
            }

            SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                                  ";
            SQL += ComNum.VBLF + "GROUP BY A.PANO, A.SDATE, B.ZIPCODE1, B.ZIPCODE2                                              ";
            SQL += ComNum.VBLF + "ORDER BY PANO                                                                                 ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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
                    nRead = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = 1;
                    nCNT = 1;

                    for (i = 0; i < nRead; i++)
                    {
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strZipCode = dt.Rows[i]["ZIPCODE1"].ToString().Trim() + dt.Rows[i]["ZIPCODE2"].ToString().Trim();
                        strSDate = dt.Rows[i]["SDATE"].ToString().Trim();

                        //SNAME은 없는 칼럼이라고 에러 발생하여 주석처리
                        //strSName = dt.Rows[i]["SNAME"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT";
                        SQL += ComNum.VBLF + "  A.SNAME, A.JUMIN1, A.JUMIN2, A.SEX, A.TEL, B.MAILJUSO, A.JUSO ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "BAS_MAILNEW B";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND A.PANO  = '" + strPano + "'";
                        SQL += ComNum.VBLF + "      AND B.MAILCODE  = '" + strZipCode + "' ";
                        SQL += ComNum.VBLF + "GROUP BY A.SNAME, A.JUMIN1, A.JUMIN2, A.SEX, A.TEL, B.MAILJUSO, A.JUSO";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        //if (dt1.Rows.Count == 0)
                        //{
                        //    dt.Dispose();
                        //    dt = null;
                        //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                        //    return;
                        //}

                        if (dt1.Rows.Count > 0)
                        {
                            strSName = ComFunc.LeftH(dt1.Rows[0]["SNAME"].ToString().Trim() + VB.Space(8), 8);
                            strJumin1 = dt1.Rows[0]["JUMIN1"].ToString().Trim();
                            strJumin2 = dt1.Rows[0]["JUMIN2"].ToString().Trim();
                            strSex = dt1.Rows[0]["SEX"].ToString().Trim();
                            strTel = dt1.Rows[0]["TEL"].ToString().Trim();
                            strJuso = dt1.Rows[0]["MAILJUSO"].ToString().Trim() + " " + dt1.Rows[0]["JUSO"].ToString().Trim();

                            if (strSex == "M")
                            {
                                strSex = "남";
                            }

                            else
                            {
                                strSex = "여";
                            }

                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = nCount.ToString();
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = strPano;
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = VB.Left("성명 : " + strSName + VB.Space(28), 28)
                                                                                   + VB.Left("나이 : " + ComFunc.AgeCalcEx(strJumin1 + strJumin2, strSDate) + VB.Space(14), 14);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text += VB.Left(" 성별 : " + strSex + VB.Space(14), 14) + VB.Left(" 전화번호 : " + strTel + VB.Space(20), 20);
                            nCount += 1;

                            ssList_Sheet1.Rows.Count += 1;
                        }

                        dt1.Dispose();
                        dt1 = null;

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                                                    ";
                        SQL += ComNum.VBLF + "  REMARK, FEED                                                                            ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_SANGDAM                                             ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                        SQL += ComNum.VBLF + "      AND SDATE = TO_DATE('" + strSDate + "','YYYY-MM-DD')                                ";
                        SQL += ComNum.VBLF + "      AND PANO  = '" + strPano + "'                                                       ";


                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strRemark = dt1.Rows[0]["REMARK"].ToString().Trim();
                            if (strRemark != "")
                            {
                                for (j = 1; j <= VB.Len(strRemark); j++)
                                {
                                    if (VB.Asc(VB.Mid(strRemark, j, 1)) == 13)
                                    {
                                        nCNT += 1;
                                    }
                                }

                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = "상담내용 : " + "\r\n";
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text += dt1.Rows[0]["REMARK"].ToString().Trim();

                                ssList_Sheet1.Rows.Count += 1;
                            }

                            strFeed = dt1.Rows[0]["FEED"].ToString().Trim();

                            if (strFeed != "")
                            {
                                for (j = 1; j <= VB.Len(strFeed); j++)
                                {
                                    if (VB.Asc(VB.Mid(strFeed, j, 1)) == 13)
                                    {
                                        nCNT += 1;
                                    }
                                }

                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = "Feed";
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = strFeed;

                                nCNT = 1;
                                ssList_Sheet1.Rows.Count += 1;
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }
            }

            catch (System.Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            ssList_Sheet1.Rows.Count -= 1;

            dt.Dispose();
            dt = null;
        }

        void cboBun_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nList = "";
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nList = VB.Left(cboBun.SelectedItem.ToString(), 1);
            cboSub.Items.Clear();

            switch (nList)
            {
                case "1":
                    groupBox3.Visible = true;
                    groupBox4.Visible = false;
                    groupBox5.Visible = false;
                    break;

                case "2":
                    groupBox4.Visible = true;
                    groupBox5.Visible = false;
                    break;

                case "3":

                    groupBox4.Visible = false;
                    groupBox5.Visible = true;
                    groupBox5.Text = "상담구분별";
                    cboSub.Items.Add("01.불만");
                    cboSub.Items.Add("02.건의");
                    cboSub.Items.Add("03.상담");
                    cboSub.Items.Add("04.칭찬");
                    cboSub.Items.Add("05.기타");
                    cboSub.SelectedIndex = 0;
                    break;

                case "4":
                    groupBox4.Visible = false;
                    groupBox5.Visible = true;
                    groupBox5.Text = "상담자별";

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                ";
                    SQL += ComNum.VBLF + "  ENTNAME                                                             ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_SANGDAM                         ";
                    SQL += ComNum.VBLF + "GROUP BY ENTNAME                                                      ";
                    SQL += ComNum.VBLF + "ORDER BY ENTNAME                                                      ";

                    try
                    {
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        }
                    }

                    catch (System.Exception ex)
                    {
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            cboSub.Items.Add(i + "." + dt.Rows[i]["ENTNAME"].ToString().Trim());
                        }
                    }
                    else
                    {
                        ComFunc.MsgBox("해당 DATA가 존재하지 않습니다.");
                    }

                    dt.Dispose();
                    dt = null;
                    cboSub.SelectedIndex = 0;
                    break;

                case "5":
                    groupBox4.Visible = false;
                    groupBox5.Visible = true;
                    groupBox5.Text = "진료과별";

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                ";
                    SQL += ComNum.VBLF + "  DEPTCODE, DEPTNAMEK                                                 ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT                             ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
                    SQL += ComNum.VBLF + "      AND DEPTCODE NOT IN ('II','TO','HR','PT','R6')                  ";
                    SQL += ComNum.VBLF + "ORDER BY PRINTRANKING                                                 ";

                    try
                    {
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                cboSub.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                            }
                        }
                    }

                    catch (System.Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }

                    dt.Dispose();
                    dt = null;

                    cboSub.SelectedIndex = 0;
                    break;

                case "6":
                    groupBox4.Visible = false;
                    groupBox5.Visible = true;
                    groupBox5.Text = "진료과장별";

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                ";
                    SQL += ComNum.VBLF + "  DRCODE, DRNAME                                                      ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR                                 ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
                    SQL += ComNum.VBLF + "      AND TOUR = 'N'                                                  ";
                    SQL += ComNum.VBLF + "      AND SUBSTR(DRCODE,3,2) <> '99'                                  ";
                    SQL += ComNum.VBLF + "      AND DRCODE NOT IN ('7101','7102','7601','8101')                 ";
                    SQL += ComNum.VBLF + "ORDER BY DRCODE, PRINTRANKING                                         ";

                    try
                    {
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                cboSub.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                            }
                        }
                    }

                    catch (System.Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }


                    dt.Dispose();
                    dt = null;

                    cboSub.SelectedIndex = 0;
                    break;

                case "7":
                    groupBox4.Visible = false;
                    groupBox5.Visible = true;
                    groupBox5.Text = "병원이유별";
                    cboSub.Items.Add("01.의료진 우수");
                    cboSub.Items.Add("02.직원친절");
                    cboSub.Items.Add("03.교통편리");
                    cboSub.Items.Add("04.주위권유(추천)");
                    cboSub.Items.Add("05.종합병원이라서");
                    cboSub.Items.Add("06.짧은진료시간");
                    cboSub.Items.Add("07.과거경험");
                    cboSub.Items.Add("08.시설/환경우수");
                    cboSub.Items.Add("09.청결하여");
                    cboSub.Items.Add("10.종교적이유");
                    cboSub.Items.Add("11.지정병원");
                    cboSub.Items.Add("12.기타");
                    cboSub.SelectedIndex = 0;
                    break;

            }
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            rSendPano(ssList_Sheet1.Cells[e.Row, 1].Text);

            if (Convert.ToInt32(VB.Left(ssList_Sheet1.Cells[e.Row, 1].Text, 1)) >= 0 && Convert.ToInt32(VB.Left(ssList_Sheet1.Cells[e.Row, 1].Text, 1)) <= 9)
            {
                this.Hide();
            }

        }
    }
}
