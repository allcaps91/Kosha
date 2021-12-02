using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{

    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPlusJaewonPatient
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// Flag 1 = iupent03
    /// Flag 2 = oiguide01
    /// Flag 3 = oiguide01_mid
    /// Flag 4 = onmok02
    /// Flag 2, 3 동일
    /// </history>
    /// <seealso cref=D:\psmh\Ocs\ptocs\iupent.vbp\iupent03.frm"         >> frmPmpaViewPlusJaewonPatient.cs 폼이름 재정의" />
    /// <seealso cref=D:\psmh\OPD\oiguide\oiguide.vbp\oiguide01.frm"     >> frmPmpaViewPlusJaewonPatient.cs 폼이름 재정의" />
    /// <seealso cref=D:\psmh\mid\EmrList\emrlist.vbp\oiguide01_mid.frm" >> frmPmpaViewPlusJaewonPatient.cs 폼이름 재정의" />
    /// <seealso cref=D:\psmh\OPD\wonmok\wonmok.vbp\onmok02.frm"         >> frmPmpaViewPlusJaewonPatient.cs 폼이름 재정의" />

    public partial class frmPmpaViewPlusJaewonPatient : Form
    {
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string Flag = "";
        string[] strBis = new string[100];
        string strInDate = "";
        string strInDate1 = "";
        string strOptSql = "";
        string[] rdoArry = new string[14];
        string strName = "";


        public frmPmpaViewPlusJaewonPatient()
        {
            InitializeComponent();
        }

        private void frmPmpaOPDViewJaewonPatient_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            Flag = "4";
            Load_Bi_IDs();
        }

        #region 함수 모음

        /// <summary>
        /// BI SetTing
        /// </summary>
        private void Load_Bi_IDs()
        {
            //Flag "4" 일땐 종교
            switch (Flag)
            {
                case "1":
                    ssView_Sheet1.Columns[6].Visible = false;
                    ssView_Sheet1.Columns[5].Visible = true;
                    ssView_Sheet1.Columns[12].Visible = true;
                    ssView_Sheet1.Columns[13].Visible = true;
                    ssView_Sheet1.ColumnHeader.Cells[0, 12].Text = "상태";
                    ssView_Sheet1.SetColumnWidth(12, 50);
                    strBis[11] = "공단";
                    strBis[12] = "직장";
                    strBis[13] = "지역";
                    strBis[14] = "";
                    strBis[15] = "";

                    strBis[21] = "보호1";
                    strBis[22] = "보호2";
                    strBis[23] = "보호3";
                    strBis[24] = "행려";
                    strBis[25] = "";

                    strBis[31] = "산재";
                    strBis[32] = "공상";
                    strBis[33] = "산재공상";
                    strBis[34] = "";
                    strBis[35] = "";

                    strBis[41] = "공단180";
                    strBis[42] = "직장180";
                    strBis[43] = "지역180";
                    strBis[44] = "가족계획";
                    strBis[45] = "보험계약";


                    strBis[51] = "일반";
                    strBis[52] = "자보";
                    strBis[53] = "계약";
                    strBis[54] = "미확인";
                    strBis[55] = "자보일반";

                    break;
                case "2":
                case "3":
                    ssView_Sheet1.Columns[6].Visible = false;
                    ssView_Sheet1.Columns[5].Visible = true;
                    ssView_Sheet1.Columns[12].Visible = true;
                    ssView_Sheet1.Columns[13].Visible = true;
                    ssView_Sheet1.ColumnHeader.Cells[0, 12].Text = "주 소";
                    ssView_Sheet1.SetColumnWidth(12, 200);
                    strBis[11] = "공단";
                    strBis[12] = "직장";
                    strBis[13] = "지역";
                    strBis[14] = "";
                    strBis[15] = "";

                    strBis[21] = "보호1";
                    strBis[22] = "보호2";
                    strBis[23] = "보호3";
                    strBis[24] = "행려";
                    strBis[25] = "";

                    strBis[31] = "산재";
                    strBis[32] = "공상";
                    strBis[33] = "산재공상";
                    strBis[34] = "";
                    strBis[35] = "";

                    strBis[41] = "공단100%";
                    strBis[42] = "직장100%";
                    strBis[43] = "지역100%";
                    strBis[44] = "가족계획";
                    strBis[45] = "보험계약";


                    strBis[51] = "일반";
                    strBis[52] = "TA 보험";
                    strBis[53] = "계약";
                    strBis[54] = "미확인";
                    strBis[55] = "TA 일반";

                    //Patient_Sql();
                    break;
                case "4":
                    ssView_Sheet1.Columns[6].Visible = true;
                    ssView_Sheet1.Columns[5].Visible = false;
                    ssView_Sheet1.Columns[12].Visible = false;
                    ssView_Sheet1.Columns[13].Visible = false;
                    ssView_Sheet1.ColumnHeader.Cells[0, 12].Text = "상태";
                    ssView_Sheet1.SetColumnWidth(12, 50);

                    strBis[11] = "공단";
                    strBis[12] = "직장";
                    strBis[13] = "지역";
                    strBis[14] = "";
                    strBis[15] = "";

                    strBis[21] = "보호1";
                    strBis[22] = "보호2";
                    strBis[23] = "보호3";
                    strBis[24] = "행려";
                    strBis[25] = "";

                    strBis[31] = "산재";
                    strBis[32] = "공상";
                    strBis[33] = "산재공상";
                    strBis[34] = "";
                    strBis[35] = "";

                    strBis[41] = "공단100%";
                    strBis[42] = "직장100%";
                    strBis[43] = "지역100%";
                    strBis[44] = "가족계획";
                    strBis[45] = "보험계약";


                    strBis[51] = "일반";
                    strBis[52] = "TA 보험";
                    strBis[53] = "계약";
                    strBis[54] = "미확인";
                    strBis[55] = "TA 일반";
                    break;
            }
        }

        /// <summary>
        /// 조회 부분
        /// </summary>
        private void Patient_Sql()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            string strBi = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            SS_Setting();

            try
            {
                switch (Flag)
                {
                    #region Flag 1

                    case "1":
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT WardCode,RoomCode,Pano,Bi,Sname,Pname,Sex,Age,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate, 'yy-mm-dd') InDate,DeptCode,DrName, Amset1, AmSet6 ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER i," + ComNum.DB_PMPA + "BAS_DOCTOR k ";

                        switch (tabMenu.SelectedIndex)
                        {
                            case 0:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "         AND Sname LIKE '%" + txtSname.Text + "%'";
                                break;
                            case 1:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + strOptSql;
                                break;
                            case 2:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "         AND Pname LIKE '%" + txtPname.Text + "%'";
                                break;
                            case 3:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "         AND InDate >= to_date('" + strInDate + "','yyyy-mm-dd') ";
                                SQL = SQL + ComNum.VBLF + "         AND InDate <  to_date('" + strInDate1 + "','yyyy-mm-dd') ";
                                break;
                        }

                        SQL = SQL + ComNum.VBLF + " AND I.GBSTS IN ('0','2')";
                        SQL = SQL + ComNum.VBLF + " AND I.OUTDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + " AND i.DrCode = k.DrCode";
                        SQL = SQL + ComNum.VBLF + " ORDER BY Sname";

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
                            txtSname.Focus();
                            return;
                        }

                        ssView_Sheet1.Rows.Count = dt.Rows.Count;
                        ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strBi = dt.Rows[i]["Bi"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RoomCode"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = strBis[Convert.ToInt32((VB.Val(strBi).ToString()))];
                            ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sname"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Pname"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Sex"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Age"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["InDate"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DrName"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["AmSet1"].ToString().Trim();

                            switch (ssView_Sheet1.Cells[i, 12].Text)
                            {
                                case "0":
                                    ssView_Sheet1.Cells[i, 12].Text = "";
                                    break;
                                case "1":
                                    ssView_Sheet1.Cells[i, 12].Text = "퇴원완료";
                                    break;
                                case "2":
                                    ssView_Sheet1.Cells[i, 12].Text = "계산중";
                                    break;
                                case "3":
                                    ssView_Sheet1.Cells[i, 12].Text = "가퇴원";
                                    break;
                            }
                        }
                        #endregion
                        break;
                    #region Flag 2 ,3 


                    case "2":
                    case "3":
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT      i.WardCode   ,i.RoomCode     ,i.Pano ";
                        SQL = SQL + ComNum.VBLF + "             ,i.Bi       ,i.Sname     ,i.Pname        ,i.Sex";
                        SQL = SQL + ComNum.VBLF + "              ,i.Age      ,c.GbInfor2  ,i.GBSTS,";
                        SQL = SQL + ComNum.VBLF + "             TO_CHAR(i.InDate, 'yyyy-mm-dd') InDate,TO_CHAR(i.OutDate, 'yyyy-mm-dd') ";
                        SQL = SQL + ComNum.VBLF + "             OutDate     ,i.DeptCode  ,k.DrName       ,i.Religion       , i.FROMTRANS ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER i," + ComNum.DB_PMPA + "BAS_DOCTOR k , " + ComNum.DB_PMPA + "bas_patient c ";

                        switch (tabMenu.SelectedIndex)
                        {
                            case 0:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "         AND i.Sname LIKE '%" + txtSname.Text + "%'";
                                break;
                            case 1:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + strOptSql;
                                break;
                            case 2:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "         AND i.Pname LIKE '%" + txtPname.Text + "%'";
                                break;
                            case 3:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "         AND i.InDate >= to_date('" + strInDate + "','yyyy-mm-dd') ";
                                SQL = SQL + ComNum.VBLF + "         AND i.InDate <  to_date('" + strInDate1 + "','yyyy-mm-dd') ";
                                break;
                        }

                        SQL = SQL + ComNum.VBLF + "             AND i.DrCode = k.DrCode";
                        SQL = SQL + ComNum.VBLF + "             AND i.Pano = c.Pano(+) ";
                        SQL = SQL + ComNum.VBLF + "             AND (i.ACTDATE IS NULL OR i.ACTDATE =TRUNC(SYSDATE) )  ";
                        SQL = SQL + ComNum.VBLF + "             AND I.PANO <> '05719836' ";// '김문철 과장님 제외.
                        SQL = SQL + ComNum.VBLF + "             AND (I.SECRET IS NULL OR I.SECRET ='')  ";//  '2012-11-27 사생활보호요청
                        SQL = SQL + ComNum.VBLF + " ORDER BY Sname";

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
                            txtSname.Focus();
                            return;
                        }

                        ssView_Sheet1.Rows.Count = dt.Rows.Count;
                        ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strBi = dt.Rows[i]["Bi"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RoomCode"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = strBis[Convert.ToInt32((VB.Val(strBi).ToString()))];
                            ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sname"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Pname"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Sex"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Age"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["InDate"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DrName"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 12].Text = CPM.BAS_JUSO_Gaide(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim());

                            if (dt.Rows[i]["GbInfor2"].ToString().Trim() != "")
                            {
                                ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["GbInfor2"].ToString().Trim();
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i, 13].Text = "";
                            }

                            if (dt.Rows[i]["GBSTS"].ToString().Trim() != "")
                            {
                                if (dt.Rows[i]["OUTDATE"].ToString().Trim() == "0")
                                {
                                    ssView_Sheet1.Cells[i, 13].Text = ssView_Sheet1.Cells[i, 13].Text + "[퇴원중]";
                                }
                            }
                            else if (dt.Rows[i]["OUTDATE"].ToString().Trim() == "7")
                            {
                                ssView_Sheet1.Cells[i, 13].Text = ssView_Sheet1.Cells[i, 13].Text + "[당일퇴원]";
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i, 13].Text = ssView_Sheet1.Cells[i, 13].Text + "[퇴원중]";
                            }
                        }
                        break;
                    #endregion

                    #region Flag 4
                    case "4":
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT WardCode,RoomCode,Pano,Bi,Sname,Pname,Sex,Age,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate, 'yyyy-mm-dd') InDate,DeptCode,DrName,Religion ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER i," + ComNum.DB_PMPA + "BAS_DOCTOR k ";

                        switch (tabMenu.SelectedIndex)
                        {
                            case 0:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "         AND Sname LIKE '%" + txtSname.Text + "%'";
                                break;
                            case 1:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + strOptSql;
                                break;
                            case 2:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "         AND Pname LIKE '%" + txtPname.Text + "%'";
                                break;
                            case 3:
                                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "         AND InDate >= to_date('" + strInDate + "','yyyy-mm-dd') ";
                                SQL = SQL + ComNum.VBLF + "         AND InDate <  to_date('" + strInDate1 + "','yyyy-mm-dd') ";
                                break;
                        }

                        SQL = SQL + ComNum.VBLF + " AND i.OUTDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + " AND i.GBSTS  = '0' ";
                        SQL = SQL + ComNum.VBLF + " AND i.DrCode = k.DrCode";
                        SQL = SQL + ComNum.VBLF + " ORDER BY WardCode,RoomCode ";

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
                            txtSname.Focus();
                            return;
                        }

                        ssView_Sheet1.Rows.Count = dt.Rows.Count;
                        ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strBi = dt.Rows[i]["Bi"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RoomCode"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = strBis[Convert.ToInt32((VB.Val(strBi).ToString()))];
                            ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sname"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Pname"].ToString().Trim();

                            switch (dt.Rows[i]["Religion"].ToString().Trim())
                            {
                                case "1":
                                    ssView_Sheet1.Cells[i, 6].Text = "1.천주교";
                                    break;
                                case "2":
                                    ssView_Sheet1.Cells[i, 6].Text = "2.개신교";
                                    break;
                                case "3":
                                    ssView_Sheet1.Cells[i, 6].Text = "3.불  교";
                                    break;
                                case "4":
                                    ssView_Sheet1.Cells[i, 6].Text = "4.천도교";
                                    break;
                                case "5":
                                    ssView_Sheet1.Cells[i, 6].Text = "5.유  교";
                                    break;
                                case "6":
                                    ssView_Sheet1.Cells[i, 6].Text = "6.무  교";
                                    break;
                                default:
                                    ssView_Sheet1.Cells[i, 6].Text = "9.기  타";
                                    break;
                            }

                            ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Sex"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Age"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["InDate"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        }
                        break;
                        #endregion
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        /// <summary>
        /// 스프레드 클리어
        /// </summary>
        private void SS_Setting()
        {
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }

        private void rdoNameArray()
        {
            int i = 0;
            ;

            for (i = 0; i < 13; i++)
            {
                rdoArry[i] = null;
            }

            Control[] controls = ComFunc.GetAllControls(this);    //모든 control을 받아온다

            foreach (Control ctl in controls)   //foreach문을 사용하여 control을 하나씩 ctl에 넣는다.
            {
                if (ctl is RadioButton)     //한 control이 Radio버튼이라면 ?
                {
                    if (VB.Left(((RadioButton)ctl).Name, 3) == "rdo") //라디오버튼의 이름을 10글자 잘라서 rdoZipName인지 확인
                    {
                        if (((RadioButton)ctl).Checked == true)     //Check 여부 확인
                        {
                            strName = ((RadioButton)ctl).Text;
                            break;  //foreach문 나가기
                        }
                    }
                }
            }

            rdoArry[0] = "가";
            rdoArry[1] = "나";
            rdoArry[2] = "다";
            rdoArry[3] = "라";
            rdoArry[4] = "마";
            rdoArry[5] = "바";
            rdoArry[6] = "사";
            rdoArry[7] = "아";
            rdoArry[8] = "자";
            rdoArry[9] = "차";
            rdoArry[10] = "카";
            rdoArry[11] = "타";
            rdoArry[12] = "파";
            rdoArry[13] = "하";


        }

        #endregion

        private void rdo_CheckedChanged(object sender, EventArgs e)
        {
            int i = 0;

            //rdoNameArray();

            rdoArry[0] = "가";
            rdoArry[1] = "나";
            rdoArry[2] = "다";
            rdoArry[3] = "라";
            rdoArry[4] = "마";
            rdoArry[5] = "바";
            rdoArry[6] = "사";
            rdoArry[7] = "아";
            rdoArry[8] = "자";
            rdoArry[9] = "차";
            rdoArry[10] = "카";
            rdoArry[11] = "타";
            rdoArry[12] = "파";
            rdoArry[13] = "하";

            if (((RadioButton)sender).Checked == true)
            {
                strName = VB.Left(((RadioButton)sender).Text, 1);
            }
            strOptSql = "";
            strOptSql = strOptSql + "       AND i.Sname >= '" + strName + "'";

            for (i = 0; i < rdoArry.Length; i++)
            {
                if (strName == rdoArry[i])
                {
                    strOptSql = strOptSql + " AND   i.Sname < '";
                    strOptSql = strOptSql + rdoArry[i + 1] + "'";
                    break;
                }
            }
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (tabMenu.SelectedIndex == 0)
            {
                lblinfo.Text = "";

                lblinfo.Text = ssView_Sheet1.Cells[e.Row, e.Column].Text;
            }
        }

        private void tabMenu_Click(object sender, EventArgs e)
        {
            switch (tabMenu.SelectedIndex)
            {
                case 0:
                    txtSname.Focus();
                    break;

                case 1:
                    rdo00.Focus();
                    break;
                case 2:
                    txtPname.Focus();
                    break;
                case 3:
                    dtpinDate.Value = Convert.ToDateTime(strDTP);
                    break;
            }

        }

        private void dtpinDate_ValueChanged(object sender, EventArgs e)
        {
            //strInDate = VB.Format(dtpinDate.Text, "yyyy-MM-dd");
            //strInDate1 = (dtpinDate.Value.AddDays(1)).ToString("yyyy-MM-dd");
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            switch (tabMenu.SelectedIndex)
            {
                case 0:
                    if (txtSname.Text != "")
                    {
                        Patient_Sql();
                    }
                    break;
                case 1:
                    Patient_Sql();
                    break;
                case 2:
                    if (txtPname.Text != "")
                    {
                        Patient_Sql();
                    }
                    break;
                case 3:
                    strInDate = (dtpinDate.Value).ToString("yyyy-MM-dd");
                    strInDate1 = (dtpinDate.Value.AddDays(1)).ToString("yyyy-MM-dd");
                    Patient_Sql();
                    break;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnView.PerformClick();
            }
        }

        private void frmPmpaViewPlusJaewonPatient_Activated(object sender, EventArgs e)
        {
            if (tabMenu.SelectedIndex == 0)
            {
                txtSname.Focus();
                txtSname.SelectAll();
            }
            else if (tabMenu.SelectedIndex == 2)
            {
                txtPname.Focus();
                txtPname.SelectAll();
            }
        }

        private void frmPmpaViewPlusJaewonPatient_Enter(object sender, EventArgs e)
        {
            if (tabMenu.SelectedIndex == 0)
            {
                txtSname.Focus();
                txtSname.SelectAll();
            }
            else if (tabMenu.SelectedIndex == 2)
            {
                txtPname.Focus();
                txtPname.SelectAll();
            }
        }
    }
}
