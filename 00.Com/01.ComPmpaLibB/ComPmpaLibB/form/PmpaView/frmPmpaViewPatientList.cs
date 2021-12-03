using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{

    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-11-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// frmPmpaViewJewonIpwonList 대체 하는 폼, 디버깅 중 중단 
    /// </history>
    /// <seealso cref= d:\psmh\IPD\ilrepb\Frm환자명부.FRM" >> frmPmpaViewPatientList.cs 폼이름 재정의" />

    public partial class frmPmpaViewPatientList : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");


        int FnGubun = 0;
        int FnSELECT = 0;
        int FnSortSELECT = 0;
        int FnSELECT1 = 0;
        int FnSELECT2 = 0;
        int TabSel = 0;
        string strPANO = "";
        string strBi = "";
        string strSname = "";
        string strJumin1 = "";
        string strJumin2 = "";
        string strGwa = "";
        string StrDeptName = "";
        string strRoom = "";
        string strilsu = "";
        string StrIDate = "";
        string StrODate = "";
        string StrDrCode = "";
        string StrTel = "";
        string StrGamek = "";
        string strGamekName = "";
        string StrGel = "";
        string StrGelName = "";
        string strSex = "";
        string StrAge = "";
        string StrReliGion = "";
        string StrReliGionName = "";
        string StrJuso = "";
        string strSet = "";
        string strSet1 = "";
        string StrSetName = "";
        string StrSetName1 = "";
        string strZip1 = "";
        string strZip2 = "";
        string strDate = "";
        string strcop = "";
        string StrCarNo = "";
        string StrPname = "";
        string strGwange = "";
        string strKiho = "";
        string StrTBed = "";
        string StrNal = "";
        string strBiName = "";
        string StrDrName = "";
        string StrSum = "";
        string strBasBi = "";

        int nSel = 0;
        int nSel1 = 0;
        int nCount = 0;
        int RowChk = 0;
        int nCount1 = 0;

        public frmPmpaViewPatientList()
        {
            InitializeComponent();
        }

        private void frmPmpaViewPatientList_Load(object sender, EventArgs e)
        {
            dtpFdate2.Value = Convert.ToDateTime(strDTP);

            cboGubun2.Items.Clear();
            cboGubun2.Items.Add("전체");// 'ALL
            cboGubun2.Items.Add("보험(전체)");// '11-15, 41-45
            cboGubun2.Items.Add("보험(공단)");// '11
            cboGubun2.Items.Add("보험(직장)");// '12
            cboGubun2.Items.Add("보험(지역)");// '13
            cboGubun2.Items.Add("보호");//       '21-25
            cboGubun2.Items.Add("산재");//       '31
            cboGubun2.Items.Add("공상");//       '32
            cboGubun2.Items.Add("산재공상");//   '33
            cboGubun2.Items.Add("보험계약");//   '45
            cboGubun2.Items.Add("일반");//       '51,54
            cboGubun2.Items.Add("TA보험");//     '52
            cboGubun2.Items.Add("일반계약");//   '53
            cboGubun2.Items.Add("TA일반");//     '55
            cboGubun2.SelectedIndex = 0;

            FnGubun = 1;
            FnSortSELECT = 1;

            TabSel = 0;
        }

        private void PatientGubun()
        {
            ssView_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 0;
            SS2_Sheet1.RowCount = 0;
            SS3_Sheet1.RowCount = 0;

            FnGubun = 0;
            switch (cboGubun2.Text)
            {
                case "전체":
                    FnGubun = 99;
                    break;
                case "보험(전체)":
                    FnGubun = 1;
                    break;
                case "보험(공단)":
                    FnGubun = 2;
                    break;
                case "보험(직장)":
                    FnGubun = 3;
                    break;
                case "보험(지역)":
                    FnGubun = 4;
                    break;
                case "보호":
                    FnGubun = 5;
                    break;
                case "산재":
                    FnGubun = 6;
                    break;
                case "공상":
                    FnGubun = 7;
                    break;
                case "산재공상":
                    FnGubun = 8;
                    break;
                case "보험계약":
                    FnGubun = 9;
                    break;
                case "일반":
                    FnGubun = 10;
                    break;
                case "TA보험":
                    FnGubun = 11;
                    break;
                case "일반계약":
                    FnGubun = 12;
                    break;
                case "TA일반":
                    FnGubun = 13;
                    break;
                default:
                    FnGubun = 99;
                    break;
            }
        }

        private void btnSearch()
        {
            string StrFDay = "";
            string StrTDay = "";

            StrFDay = dtpFdate2.Value.ToString("yyyy-MM-dd");
            StrTDay = dtpFdate2.Value.AddDays(-1).ToString("yyyy-MM-dd");

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 1;
            SS2_Sheet1.RowCount = 0;
            SS2_Sheet1.RowCount = 1;
            SS3_Sheet1.RowCount = 0;
            SS3_Sheet1.RowCount = 1;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            //try
            //{

            if (rdoOptSelect0.Checked == true)
            {
                SS1BuildSubMaster();//      '재원자
            }
            else if (rdoOptSelect1.Checked == true)
            {
                SS1BuildSubMaster1();//     '입원자
            }
            else
            {
                ss1BuildSubMaster2();     //'퇴원자

                if (FnGubun == 6)
                {
                    SS1BuildSubMaster1();//     '입원자

                    SS1BuildMove();       //'퇴원자지만 재원자 화면사용
                }
                else
                {
                    SS3_Sheet1.Visible = true;

                    //Call SS1BuildMove2      '퇴원자
                }
                ss1BuildSubMaster2(); //     '퇴원자

            }

            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //}
        }

        private void btnSearch3_Click(object sender, EventArgs e)
        {
            btnSearch();
        }


        private void SS1_Display()
        {
            switch (FnGubun)
            {
                case 6:
                case 13:
                    SS1_Sheet1.ColumnHeader.Cells[0, 10].Text = "사고일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 13].Text = "진단만료일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 14].Text = "회사명";
                    break;
                case 11:
                    SS1_Sheet1.ColumnHeader.Cells[0, 10].Text = "진단만료일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 13].Text = "차량번호";
                    SS1_Sheet1.ColumnHeader.Cells[0, 14].Text = "계약처";
                    break;
                default:
                    SS1_Sheet1.ColumnHeader.Cells[0, 10].Text = "퇴원일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 13].Text = "전화번호";
                    SS1_Sheet1.ColumnHeader.Cells[0, 14].Text = "비    고";
                    break;
            }
        }

        private void BiDefineProcess()
        {
            strBiName = "";
            switch (strBi)
            {

                case "11":
                    strBiName = "공    단";
                    break;
                case "12":
                    strBiName = "연 합 회";
                    break;
                case "13":
                    strBiName = "지    역";
                    break;
                case "21":
                    strBiName = "보 호  1";
                    break;
                case "22":
                    strBiName = "보 호  2";
                    break;
                case "23":
                    strBiName = "의료부조";
                    break;
                case "24":
                    strBiName = "행    려";
                    break;
                case "31":
                    strBiName = "산    재";
                    break;
                case "32":
                    strBiName = "공    상";
                    break;
                case "33":
                    strBiName = "산재공상";
                    break;
                case "41":
                    strBiName = "공단 180";
                    break;
                case "42":
                    strBiName = "직장 180";
                    break;
                case "43":
                    strBiName = "지역 180";
                    break;
                case "44":
                    strBiName = "가족계획";
                    break;
                case "45":
                    strBiName = "보험계약";
                    break;
                case "51":
                    strBiName = "일    반";
                    break;
                case "52":
                    strBiName = "TA 보 험";
                    break;
                case "53":
                    strBiName = "계    약";
                    break;
                case "54":
                    strBiName = "미 확 인";
                    break;
                case "55":
                    strBiName = "TA 일 반";
                    break;
                default:
                    strBiName = "";
                    break;
            }
        }

        private void SS1BuildMoveSub()
        {
            int i = 0;

            BiDefineProcess();

            SS1_Sheet1.RowCount = SS1_Sheet1.RowCount + 1;
            SS1_Sheet1.SetRowHeight(SS1_Sheet1.RowCount - 1, ComNum.SPDROWHT);

            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = strPANO.Trim();
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = strBiName.Trim();
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = strSname.Trim();
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = strJumin1 + "-" + strJumin2;
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 4].Text = StrDeptName.Trim();
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 5].Text = StrTBed.Trim();
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 6].Text = strRoom.Trim();
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 7].Text = StrIDate.Trim();
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 8].Text = StrODate.Trim();
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 9].Text = StrNal.Trim();
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 10].Text = StrDrName.Trim();
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 11].Text = StrTel.Trim();

            switch (FnGubun)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 10:
                case 13:
                case 99:
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 12].Text = (strGamekName).Trim();
                    break;
                case 7:
                case 8:
                case 9:
                case 12:
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 12].Text = (StrGelName).Trim();
                    break;
                case 6:
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 12].Text = strcop.Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 8].Text = (strDate).Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 9].Text = (strilsu).Trim();
                    break;
                case 11:
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 8].Text = (strDate).Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 9].Text = (StrCarNo).Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 12].Text = (StrGelName).Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 5].Text = (strilsu).Trim();
                    break;
            }

        }

        private string MihGelNameSearch(string strPANO, string strBi)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strVal = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT Kiho                       ";
            SQL = SQL + ComNum.VBLF + "   FROM Bas_mih                    ";
            SQL = SQL + ComNum.VBLF + "  WHERE pano = '" + strPANO + "'   ";
            SQL = SQL + ComNum.VBLF + "    AND Bi = '" + strBi + "'       ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return strVal = "";
            }
            if (dt.Rows.Count > 0)
            {
                strVal = dt.Rows[0]["Kiho"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            return strVal;
        }

        private void SS1BuildSubMaster()
        {
            int i = 0;
            string StrFDay = "";
            string StrTDay = "";
            string StrRemark = "";
            string SQL = "";
            DataTable dt = null;
            DataTable dtRs = null;
            string SqlErr = "";

            StrFDay = dtpFdate2.Value.ToString("yyyy-MM-dd");
            StrTDay = dtpFdate2.Value.AddDays(1).ToString("yyyy-MM-dd");

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT A.Pano, A.Bi, A.Sname, a.Pname, b.Gwange, Jumin1, Jumin2, A.DeptCode, A.Sex, A.Age,A.RoomCode,  ";//
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(Indate,'YYYY-MM-DD') IDate, TO_CHAR(Outdate,'YYYY-MM-DD') ODate,                       ";//
            SQL = SQL + ComNum.VBLF + "        A.DrCode, Tel, A.GbGamek, Kiho, B.BI BASBI,A.Ilsu, A.AmSet3, A.AmSet1, A.Remark                ";//
            SQL = SQL + ComNum.VBLF + "   FROM BAS_PATIENT B, IPD_NEW_MASTER A                                                                ";//
            SQL = SQL + ComNum.VBLF + "  WHERE (a.ActDate IS NULL OR a.ActDate>TO_DATE('" + StrTDay + "','YYYY-MM-DD'))    ";
            SQL = SQL + ComNum.VBLF + "   AND a.InDate <= TO_DATE('" + StrFDay + " 23:59','YYYY-MM-DD HH24:MI')                               ";//  '퇴원
            SQL = SQL + ComNum.VBLF + "   AND a.Pano <  '90000000' ";//'지병환자 제외
            SQL = SQL + ComNum.VBLF + "   AND a.Pano <> '81000004' "; //'전산실연습 제외
            SQL = SQL + ComNum.VBLF + "   AND a.GbSTS NOT IN  ('7','9')    ";

            switch (FnGubun)
            {
                case 1://'보험전체
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '10' AND A.Bi < '16'                                                    ";//
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '40' AND A.Bi < '46'                                                    ";//
                    break;
                case 2://'보험(공단)
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '11'                                                                    ";//
                    break;
                case 3:// '보험(직장)
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '12'                                                                    ";//
                    break;
                case 4:// '보험(지역)
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '13'                                                                    ";//
                    break;
                case 5://'보호
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '20' AND A.Bi < '26'                                                    ";//
                    break;
                case 6://'산재
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '31'                                                                    ";//
                    break;
                case 7://'공상
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '32'                                                                    ";//
                    break;
                case 8://'산재공상
                    SQL = SQL + ComNum.VBLF + "     AND A.Bi = '33'                                                                   ";//
                    break;
                case 9://'보험계약
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '45'                                                                    ";//
                    break;
                case 10://'일반 + 일반계약 + 미확인
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '50' AND A.Bi < '55'                                                    ";//
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi != '52'                                                                   ";//
                    break;
                case 11://'TA보험
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '52'                                                                    ";//
                    break;
                case 12://'일반계약
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '53'                                                                    ";//
                    break;
                case 13://'TA일반
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '55'                                                                    ";//
                    break;
            }
            SQL = SQL + ComNum.VBLF + "    AND AmSet6 <> '*'                                                                          ";// '환자구분변경
            SQL = SQL + ComNum.VBLF + "    AND AmSet4 <> '3'                                                                          ";// '정상애기
            SQL = SQL + ComNum.VBLF + "    AND A.Pano = B.Pano                                                                        ";//

            switch (FnSortSELECT)
            {
                case 1:
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.Sname, A.Pano                                                         ";
                    break;
                case 2:
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.WardCode, A.RoomCode, A.Pano                                                    ";
                    break;
                case 3:
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.DeptCode, A.DrCode, A.Pano                                                      ";
                    break;
                case 4:
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Kiho, A.Pano                                                                    ";
                    break;
            }

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

            SS1_Display();

            SS1_Sheet1.Rows.Count = dt.Rows.Count;
            SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strPANO = dt.Rows[i]["Pano"].ToString().Trim();
                strBi = dt.Rows[i]["Bi"].ToString().Trim();

                switch (strBi)
                {
                    case "11":
                        strBiName = "공    단";
                        break;
                    case "12":
                        strBiName = "연 합 회";
                        break;
                    case "13":
                        strBiName = "지    역";
                        break;
                    case "21":
                        strBiName = "보 호  1";
                        break;
                    case "22":
                        strBiName = "보 호  2";
                        break;
                    case "23":
                        strBiName = "의료부조";
                        break;
                    case "24":
                        strBiName = "행    려";
                        break;
                    case "31":
                        strBiName = "산    재";
                        break;
                    case "32":
                        strBiName = "공    상";
                        break;
                    case "33":
                        strBiName = "산재공상";
                        break;
                    case "41":
                        strBiName = "공단 180";
                        break;
                    case "42":
                        strBiName = "직장 180";
                        break;
                    case "43":
                        strBiName = "지역 180";
                        break;
                    case "44":
                        strBiName = "가족계획";
                        break;
                    case "45":
                        strBiName = "보험계약";
                        break;
                    case "51":
                        strBiName = "일    반";
                        break;
                    case "52":
                        strBiName = "TA 보 험";
                        break;
                    case "53":
                        strBiName = "계    약";
                        break;
                    case "54":
                        strBiName = "미 확 인";
                        break;
                    case "55":
                        strBiName = "TA 일 반";
                        break;
                }

                strSname = dt.Rows[i]["SName"].ToString().Trim();
                strJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();
                strJumin2 = dt.Rows[i]["Jumin2"].ToString().Trim();
                strGwa = dt.Rows[i]["DeptCode"].ToString().Trim();
                strilsu = dt.Rows[i]["Ilsu"].ToString().Trim();
                strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                StrIDate = dt.Rows[i]["IDate"].ToString().Trim();
                StrODate = dt.Rows[i]["ODate"].ToString().Trim();
                StrDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                StrRemark = dt.Rows[i]["Remark"].ToString().Trim();

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DrName                         ";
                SQL = SQL + ComNum.VBLF + "   FROM BAS_DOCTOR                     ";
                SQL = SQL + ComNum.VBLF + "  WHERE DrCode = '" + StrDrCode + "'   ";

                SqlErr = clsDB.GetDataTable(ref dtRs, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dtRs.Rows.Count > 0)
                {
                    StrDrName = dtRs.Rows[0]["DrName"].ToString().Trim();
                }
                StrTel = dt.Rows[i]["Tel"].ToString().Trim();
                StrGamek = dt.Rows[i]["GbGamek"].ToString().Trim();

                dtRs.Dispose();
                dtRs = null;

                if (FnGubun == 1 || FnGubun == 2 || FnGubun == 3 || FnGubun == 4)
                {
                    strKiho = dt.Rows[i]["Kiho"].ToString().Trim();
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT MiaName                                                        ";
                    SQL = SQL + ComNum.VBLF + "   FROM BAS_MIA                                                        ";
                    SQL = SQL + ComNum.VBLF + "  WHERE MIACODE  = '" + dt.Rows[i]["Kiho"].ToString().Trim() + "'    ";

                    SqlErr = clsDB.GetDataTable(ref dtRs, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dtRs.Rows.Count > 0)
                    {
                        strKiho = dtRs.Rows[0]["MiaName"].ToString().Trim();
                    }

                    dtRs.Dispose();
                    dtRs = null;
                }

                strBasBi = dt.Rows[i]["BASBI"].ToString().Trim();
                StrPname = dt.Rows[i]["Pname"].ToString().Trim();

                switch (dt.Rows[i]["GwAnge"].ToString().Trim())
                {
                    case "1":
                        strGwange = "본인";
                        break;
                    case "2":
                        strGwange = "부모";
                        break;
                    case "3":
                        strGwange = "자녀";
                        break;
                    case "4":
                        strGwange = "배우자";
                        break;
                    case "5":
                        strGwange = "기타";
                        break;
                }

                if (strBi == "31" || strBi == "52" || strBi == "55")
                {

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT To_Char(Date1,'YYYY-MM-DD') SDate1,CoprName,CoprNo FROM BAS_SANID  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPANO + "'                                           ";
                    SQL = SQL + ComNum.VBLF + "    AND Bi = '" + strBi + "'                                               ";

                    SqlErr = clsDB.GetDataTable(ref dtRs, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dtRs.Rows.Count > 0)
                    {
                        strDate = dtRs.Rows[0]["SDate1"].ToString().Trim();         //'재해발생일자 및 사고일자
                        strcop = dtRs.Rows[0]["CoprName"].ToString().Trim();      //'산재:사업장명 교통:보험회사코드+가해자
                        StrCarNo = dtRs.Rows[0]["CoprNo"].ToString().Trim();        //'산재:성립번호 교통:차량번호
                    }
                    else
                    {
                        strDate = "";
                        strcop = "";
                        StrCarNo = "";
                    }
                    dtRs.Dispose();
                    dtRs = null;
                }

                if (strBi == "31")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT MAX(IPDTODATE) TDate1      ";
                    SQL = SQL + ComNum.VBLF + "   FROM BAS_SANDTL                 ";
                    SQL = SQL + ComNum.VBLF + "   WHERE PANO = '" + strPANO + "'  ";

                    SqlErr = clsDB.GetDataTable(ref dtRs, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        StrTel = VB.Left(dtRs.Rows[0]["TDate1"].ToString().Trim(), 4) + "-" + VB.Mid(dtRs.Rows[0]["TDate1"].ToString().Trim(), 5, 2) + "-" + VB.Right(dtRs.Rows[0]["TDate1"].ToString().Trim(), 2);
                    }
                    else
                    {
                        StrTel = "";
                    }
                    dtRs.Dispose();
                    dtRs = null;
                }
                else if (strBi == "52" || strBi == "55")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT MAX(TODATE) TODATE      ";
                    SQL = SQL + ComNum.VBLF + "   FROM BAS_SANJIN                 ";
                    SQL = SQL + ComNum.VBLF + "   WHERE PANO = '" + strPANO + "'  ";

                    SqlErr = clsDB.GetDataTable(ref dtRs, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        StrTel = VB.Left(dtRs.Rows[0]["TODATE"].ToString().Trim(), 4) + "-" + VB.Mid(dtRs.Rows[0]["TODATE"].ToString().Trim(), 5, 2) + "-" + VB.Right(dtRs.Rows[0]["TODATE"].ToString().Trim(), 2);
                    }
                    else
                    {
                        StrTel = "";
                    }
                    dtRs.Dispose();
                    dtRs = null;
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT To_Char(Date3,'YYYY-MM-DD') Date1 FROM BAS_SANID   ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPANO + "'                           ";
                    SQL = SQL + ComNum.VBLF + "    AND Bi = '" + strBi + "'                               ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY DATE1 DESC                                      ";

                    SqlErr = clsDB.GetDataTable(ref dtRs, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dtRs.Rows.Count > 0)
                    {
                        StrTel = dtRs.Rows[0]["Date1"].ToString().Trim();
                    }

                    dtRs.Dispose();
                    dtRs = null;
                }

                SS1_Sheet1.Cells[i, 0].Text = strPANO.Trim();
                SS1_Sheet1.Cells[i, 1].Text = strBiName.Trim();
                SS1_Sheet1.Cells[i, 2].Text = StrPname.Trim();
                SS1_Sheet1.Cells[i, 3].Text = strGwange.Trim();
                SS1_Sheet1.Cells[i, 4].Text = strSname.Trim();

                SS1_Sheet1.Cells[i, 6].Text = strJumin1.Trim() + "-" + strJumin2.Trim();
                SS1_Sheet1.Cells[i, 7].Text = strGwa.Trim();
                SS1_Sheet1.Cells[i, 8].Text = strRoom.Trim();
                SS1_Sheet1.Cells[i, 9].Text = StrIDate.Trim();

                if (strBi == "31")
                {
                    SS1_Sheet1.Cells[i, 5].Text = strKiho.Trim();
                    SS1_Sheet1.Cells[i, 10].Text = strDate.Trim();
                    SS1_Sheet1.Cells[i, 13].Text = StrTel.Trim();
                    SS1_Sheet1.Cells[i, 14].Text = StrRemark.Trim();
                }
                // 'ElseIf strBi = "52" Or strBi = "55" Then '2009-10-07 윤조연 함종현주임 요청 주석 55 별도 표시
                else if (strBi == "52" || strBi == "55")
                {
                    SS1_Sheet1.Cells[i, 10].Text = StrTel.Trim();
                    SS1_Sheet1.Cells[i, 13].Text = StrCarNo.Trim();
                    SS1_Sheet1.Cells[i, 14].Text = strKiho.Trim();
                }
                else
                {
                    SS1_Sheet1.Cells[i, 5].Text = strKiho.Trim();
                    SS1_Sheet1.Cells[i, 10].Text = StrODate.Trim();
                    SS1_Sheet1.Cells[i, 13].Text = StrTel.Trim();
                    SS1_Sheet1.Cells[i, 14].Text = StrRemark.Trim();
                }
                SS1_Sheet1.Cells[i, 11].Text = strilsu.Trim();
                SS1_Sheet1.Cells[i, 12].Text = StrDrName.Trim();

                strKiho = "";

            }
            dt.Dispose();
            dt = null;
        }

        private void SS1BuildSubMaster1()
        {
            int i = 0;
            int j = 0;
            string StrFDay = "";
            string StrTDay = "";
            string strDD = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            StrFDay = dtpFdate2.Value.ToString("yyyy-MM-dd");
            StrTDay = dtpFdate2.Value.AddDays(1).ToString("yyyy-MM-dd");

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT A.Pano, A.Sname, A.Bi, A.DeptCode, A.Drcode, A.RoomCode,A.Remark,  ";
            SQL = SQL + ComNum.VBLF + "        A.Sex, A.Age, A.ReliGion, B.JuMin1, B.JuMin2, B.JuSo, B.Tel,                     ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(Outdate,'YY-MM-DD') ODate, TO_CHAR(InDate,'YY-MM-DD') InDate,b.ZIPCODE1, b.ZIPCODE2                  ";
            SQL = SQL + ComNum.VBLF + "   FROM BAS_PATIENT B, IPD_NEW_MASTER A                                                                ";
            SQL = SQL + ComNum.VBLF + "  WHERE (a.ActDate IS NULL OR a.ActDate<TO_DATE('" + StrTDay + "','YYYY-MM-DD'))    ";
            SQL = SQL + ComNum.VBLF + "   AND a.InDate <= TO_DATE('" + StrFDay + " 23:59','YYYY-MM-DD HH24:MI')                               ";  //'퇴원
            SQL = SQL + ComNum.VBLF + "    AND a.IpwonTime >=TO_DATE('" + StrFDay + "','YYYY-MM-DD')                                         ";
            SQL = SQL + ComNum.VBLF + "    AND a.IpwonTime <=TO_DATE('" + StrFDay + " 23:59','YYYY-MM-DD HH24:MI')                           ";
            SQL = SQL + ComNum.VBLF + "   AND a.Pano <  '90000000' ";// '지병환자 제외
            SQL = SQL + ComNum.VBLF + "   AND a.Pano <> '81000004' ";// '전산실연습 제외
            SQL = SQL + ComNum.VBLF + "   AND a.GbSTS NOT IN  ('7','9')    ";

            switch (FnGubun)
            {

                case 1:// '보험전체
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '10' AND A.Bi < '16'                            ";
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '40' AND A.Bi < '46'                            ";
                    break;
                case 2://'보험(공단)
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '11'                                            ";
                    break;
                case 3://'보험(직장)
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '12'                                            ";
                    break;
                case 4://'보험(지역)
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '13'                                            ";
                    break;
                case 5://'보호
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '20' AND A.Bi < '26'                            ";
                    break;
                case 6://'산재
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '31'                                            ";
                    break;
                case 7://'공상
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '32'                                            ";
                    break;
                case 8://'산재공상
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '33'                                            ";
                    break;
                case 9://'보험계약
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '45'                                            ";
                    break;
                case 10://'일반 + 일반계약 + 미확인
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '50' AND A.Bi < '55'                            ";

                    SQL = SQL + ComNum.VBLF + "    AND A.Bi != '52'                                           ";
                    break;
                case 11://'TA보험
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '52'                                            ";
                    break;
                case 12://'일반계약
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '53'                                            ";
                    break;
                case 13://'TA일반
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '55'                                            ";
                    break;
            }
            SQL = SQL + ComNum.VBLF + "    AND A.Pano = B.Pano                                                ";
            SQL = SQL + ComNum.VBLF + "   AND a.Pano <  '90000000' ";
            SQL = SQL + ComNum.VBLF + "   AND a.Pano <> '81000004' ";

            switch (FnSortSELECT)
            {
                case 1:
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.Sname, A.Pano                                 ";
                    break;
                case 2:
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.WardCode, A.RoomCode, A.Pano                  ";
                    break;
                case 3:
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.DeptCode, A.DrCode, A.Pano                    ";
                    break;
            }

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
                RowChk = Convert.ToInt32(false);
                return;
            }

            SS3_Sheet1.RowCount = 0;
            SS3_Sheet1.RowCount = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                SS3_Sheet1.Cells[i, 0].Text = i + 1.ToString();
                SS3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                SS3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                SS3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Bi"].ToString().Trim();
                SS3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                SS3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["InDate"].ToString().Trim();
                SS3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Sex"].ToString().Trim();
                SS3_Sheet1.Cells[i, 7].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                SS3_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                SS3_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ReMark"].ToString().Trim();

                StrODate = dt.Rows[i]["ODate"].ToString().Trim();
                strZip1 = dt.Rows[i]["ZIPCODE1"].ToString().Trim();
                strZip2 = dt.Rows[i]["ZIPCODE2"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        private void ss1BuildSubMaster2()
        {
            int i = 0;
            int j = 0;
            string StrFDay = "";
            string StrTDay = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            StrFDay = dtpFdate2.Value.ToString("yyyy-MM-dd");
            StrTDay = dtpFdate2.Value.AddDays(1).ToString("yyyy-MM-dd");

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT A.Pano, A.Bi, A.Sname, Jumin1, Jumin2, A.DeptCode, A.Sex, A.Age,A.RoomCode,    ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(Indate,'YY-MM-DD') IDate, TO_CHAR(Outdate,'YY-MM-DD') ODate,           ";
            SQL = SQL + ComNum.VBLF + "        A.DrCode, Tel, A.GbGamek, Kiho, B.BI BASBI,A.Ilsu, A.AmSet3, A.AmSet1          ";
            SQL = SQL + ComNum.VBLF + "   FROM BAS_PATIENT B, IPD_NEW_MASTER A                                                ";
            SQL = SQL + ComNum.VBLF + "  WHERE A.OUTDate = TO_DATE('" + dtpFdate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                     ";

            switch (FnGubun)
            {

                case 1://'보험전체
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '10' AND A.Bi < '16'                                            ";
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '40' AND A.Bi < '46'                                            ";
                    break;
                case 2://'보험전체'보험(공단)
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '11'                                                            ";
                    break;
                case 3://'보험전체'보험(직장)
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '12'                                                            ";
                    break;
                case 4://'보험전체'보험(지역)
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '13'                                                            ";
                    break;
                case 5://'보험전체 '보호
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '20' AND A.Bi < '26'                                            ";
                    break;
                case 6://'보험전체'산재
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '31'                                                            ";
                    break;
                case 7://'보험전체 '공상
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '32'                                                            ";
                    break;
                case 8://'보험전체'산재공상
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '33'                                                            ";
                    break;
                case 9://'보험전체'보험계약
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '45'                                                            ";
                    break;
                case 10://'보험전체'일반 + 일반계약 + 미확인
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi > '50' AND A.Bi < '55'                                            ";
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi != '52'                                                           ";
                    break;
                case 11://'보험전체'TA보험
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '52'                                                            ";
                    break;
                case 12://'보험전체'일반계약
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '53'                                                            ";
                    break;
                case 13://'보험전체 'TA일반
                    SQL = SQL + ComNum.VBLF + "    AND A.Bi = '55'                                                            ";
                    break;
            }

            SQL = SQL + ComNum.VBLF + "    AND A.AmSet6 <> '*'                                                                "; //'환자구분변경
            SQL = SQL + ComNum.VBLF + "    AND A.AmSet4 <> '3'                                                                "; //'정상애기
            SQL = SQL + ComNum.VBLF + "    AND A.AmSet1 <> '0'                                                                ";
            SQL = SQL + ComNum.VBLF + "    AND A.Pano = B.Pano                                                                ";
            SQL = SQL + ComNum.VBLF + "    AND a.GbSTS <> '9' ";
            SQL = SQL + ComNum.VBLF + "    AND (A.RDate IS NULL OR a.ActDate = TO_DATE('" + StrFDay + "','YYYY-MM-DD')) ";
            SQL = SQL + ComNum.VBLF + "    AND A.Pano <  '90000000' ";
            SQL = SQL + ComNum.VBLF + "    AND A.Pano <> '81000004' ";
            SQL = SQL + ComNum.VBLF + "    AND A.OUTDATE IS NOT NULL ";

            switch (FnSortSELECT)
            {
                case 1:
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.Sname, A.Pano                                                 ";
                    break;
                case 2:
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.WardCode, A.RoomCode, A.Pano                                  ";
                    break;
                case 3:
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.DeptCode, A.DrCode, A.Pano                                    ";
                    break;
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
                SS1_Sheet1.RowCount = 0;

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    if (FnGubun == 6)
                    {

                        strPANO = dt.Rows[i]["Pano"].ToString().Trim();
                        strBi = dt.Rows[i]["Bi"].ToString().Trim();
                        strSname = dt.Rows[i]["SName"].ToString().Trim();
                        strJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();
                        strJumin2 = dt.Rows[i]["Jumin2"].ToString().Trim();
                        strGwa = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strilsu = dt.Rows[i]["Ilsu"].ToString().Trim();
                        strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                        StrIDate = dt.Rows[i]["IDate"].ToString().Trim();
                        StrODate = dt.Rows[i]["ODate"].ToString().Trim();
                        StrDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                        StrTel = dt.Rows[i]["Tel"].ToString().Trim();
                        StrGamek = dt.Rows[i]["GbGamek"].ToString().Trim();
                        StrGel = dt.Rows[i]["Kiho"].ToString().Trim();
                        strBasBi = dt.Rows[i]["BASBI"].ToString().Trim();

                        ssView_Sheet1.Cells[i, 0].Text = strPANO + strBi + strSname + strJumin1 + strJumin2;
                        ssView_Sheet1.Cells[i, 0].Text = ssView_Sheet1.Cells[i, 0].Text + StrSum + strGwa + strRoom + StrIDate + StrODate;
                        ssView_Sheet1.Cells[i, 0].Text = ssView_Sheet1.Cells[i, 0].Text + StrDrCode + StrTel + StrGamek + StrGel + strBasBi + strilsu;
                    }
                    else
                    {
                        strPANO = dt.Rows[i]["Pano"].ToString().Trim();
                        strSname = dt.Rows[i]["SName"].ToString().Trim();
                        strBi = dt.Rows[i]["Bi"].ToString().Trim();
                        strGwa = dt.Rows[i]["DeptCode"].ToString().Trim();
                        StrIDate = dt.Rows[i]["IDate"].ToString().Trim();
                        strSex = dt.Rows[i]["SEX"].ToString().Trim();
                        StrAge = dt.Rows[i]["AGE"].ToString().Trim();
                        strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                        strSet = dt.Rows[i]["Amset3"].ToString().Trim();
                        strSet1 = dt.Rows[i]["Amset1"].ToString().Trim();

                        ssView_Sheet1.Cells[i, 0].Text = strPANO + strSname + strBi + strGwa + StrIDate + strSex + StrAge + strRoom + strSet + strSet1;
                    }

                }

            }

            dt.Dispose();
            dt = null;
        }

        private void SS1BuildMove()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            StrNal = "";
            StrDrName = "";
            StrTBed = "";

            switch (FnGubun)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 10:
                case 13:
                case 99:
                    SS1_Sheet1.ColumnHeader.Cells[0, 11].Text = "퇴원일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 13 - 1].Text = "일수";
                    SS1_Sheet1.ColumnHeader.Cells[0, 16 - 1].Text = "비   고";
                    break;
                case 7:
                case 8:
                case 9:
                case 12:
                    SS1_Sheet1.ColumnHeader.Cells[0, 12 - 1].Text = "퇴원일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 13 - 1].Text = "일수";
                    SS1_Sheet1.ColumnHeader.Cells[0, 16 - 1].Text = "감액구분";
                    break;
                case 6:
                    SS1_Sheet1.ColumnHeader.Cells[0, 12 - 1].Text = "사고일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 13 - 1].Text = "일수";
                    SS1_Sheet1.ColumnHeader.Cells[0, 16 - 1].Text = "회사명";
                    break;
                case 11:
                    SS1_Sheet1.ColumnHeader.Cells[0, 1 - 1].Text = "병록번호";
                    SS1_Sheet1.ColumnHeader.Cells[0, 2 - 1].Text = "환자구분";
                    SS1_Sheet1.ColumnHeader.Cells[0, 3 - 1].Text = "환자명";
                    SS1_Sheet1.ColumnHeader.Cells[0, 4 - 1].Text = "주민번호";
                    SS1_Sheet1.ColumnHeader.Cells[0, 5 - 1].Text = "진료과";
                    SS1_Sheet1.ColumnHeader.Cells[0, 6 - 1].Text = "재원일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 7 - 1].Text = "병실";
                    SS1_Sheet1.ColumnHeader.Cells[0, 8 - 1].Text = "입원일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 9 - 1].Text = "사고일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 10 - 1].Text = "차량No";
                    SS1_Sheet1.ColumnHeader.Cells[0, 11 - 1].Text = "의사명";
                    SS1_Sheet1.ColumnHeader.Cells[0, 12 - 1].Text = "전화번호";
                    SS1_Sheet1.ColumnHeader.Cells[0, 13 - 1].Text = "계약처";
                    break;
            }

            switch (FnGubun)
            {
                case 6:
                case 8:
                case 11:
                case 13:
                    SS1_Sheet1.ColumnHeader.Cells[0, 15 - 1].Text = "진단만료일";
                    break;
                default:
                    SS1_Sheet1.ColumnHeader.Cells[0, 15 - 1].Text = "전화번호";
                    break;
            }
            SS1_Sheet1.RowCount = 1;

            if (FnSELECT == 1)
            {
                for (i = 0; i < ssView_Sheet1.Rows.Count; i++)
                {
                    strPANO = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 1, 8);
                    strBi = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 9, 2);
                    strSname = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 11, 10);
                    strJumin1 = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 21, 6);
                    strJumin2 = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 27, 7);
                    strGwa = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 34, 2);
                    strRoom = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 36, 4);
                    StrIDate = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 40, 8);
                    StrODate = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 48, 8);
                    StrDrCode = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 56, 4);

                    if (FnGubun == 11 || FnGubun == 6 || FnGubun == 8 || FnGubun == 13)
                    {
                        StrTel = CPF.READ_BAS_SANID(clsDB.DbCon, strPANO);
                    }
                    else
                    {
                        StrTel = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 60, 14);
                    }
                    StrGamek = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 74, 1);
                    StrGel = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 75, 4);
                    strBasBi = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 79, 2);
                    strilsu = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 81, 2);

                    SQL = "";
                    SQL = " SELECT TBed FROM BAS_ROOM WHERE RoomCode = " + VB.Val(strRoom) + " ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        StrTBed = dt.Rows[0]["TBed"].ToString().Trim();
                        dt.Dispose();
                        dt = null;
                    }

                    SQL = "";
                    SQL = " SELECT Nal FROM IPD_BCASH WHERE Pano = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + " AND ActDate = TO_DATE('" + dtpFdate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        StrTBed = dt.Rows[0]["Nal"].ToString().Trim();
                        dt.Dispose();
                        dt = null;
                    }

                    SQL = "";
                    SQL = " SELECT DrName FROM BAS_DOCTOR WHERE DrCode = '" + StrDrCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        StrTBed = dt.Rows[0]["DrName"].ToString().Trim();
                        dt.Dispose();
                        dt = null;
                    }

                    SQL = "";
                    SQL = " SELECT DeptNameK FROM BAS_ClinicDept WHERE DeptCode = '" + strGwa + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        StrTBed = dt.Rows[0]["DeptNameK"].ToString().Trim();
                        dt.Dispose();
                        dt = null;
                    }

                    if (strBi == "31" || strBi == "52")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT To_Char(Date1,'YY-MM-DD') Date1,CoprName,CoprNo    ";
                        SQL = SQL + ComNum.VBLF + "   FROM BAS_SANID                                          ";
                        SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPANO + "'                           ";
                        SQL = SQL + ComNum.VBLF + "    AND Bi = '" + strBi + "'                               ";


                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            StrTBed = dt.Rows[0]["Date1"].ToString().Trim();
                            StrTBed = dt.Rows[0]["CoprName"].ToString().Trim();
                            StrTBed = dt.Rows[0]["CoprNo"].ToString().Trim();
                        }
                        else
                        {
                            strDate = "";
                            strcop = "";
                            StrCarNo = "";
                        }
                        dt.Dispose();
                        dt = null;

                        if (strBi == "31")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT MAX(IPDTODATE) Date1       ";
                            SQL = SQL + ComNum.VBLF + "   FROM BAS_SANDTL                 ";
                            SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPANO + "'   ";

                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt.Rows.Count > 0)
                            {
                                StrTBed = dt.Rows[0]["Date1"].ToString().Trim();
                            }
                            else
                            {
                                strDate = "";
                            }

                            dt.Dispose();
                            dt = null;
                        }

                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT To_Char(Date3,'YY-MM-DD') Date1    ";
                            SQL = SQL + ComNum.VBLF + "   FROM BAS_SANID                          ";
                            SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPANO + "'           ";
                            SQL = SQL + ComNum.VBLF + "    AND Bi = '" + strBi + "'               ";
                            SQL = SQL + ComNum.VBLF + "   ORDER BY DATE1 DESC                     ";

                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt.Rows.Count > 0)
                            {
                                StrTBed = dt.Rows[0]["Date1"].ToString().Trim();
                            }
                            else
                            {
                                strDate = "";
                            }

                            dt.Dispose();
                            dt = null;
                        }
                    }
                    strGamekName = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "2", "BAS_감액코드명", StrGamek);

                    if (strBasBi == strBi)
                    {
                        CPF.GelNameGubun(clsDB.DbCon, StrGel);
                    }
                    else
                    {
                        MihGelNameSearch(strPANO, strBi);
                    }

                    SS1BuildMoveSub();
                }
            }
            else if (FnSELECT1 == 2)
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strPANO = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 1, 8);
                    strBi = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 9, 2);
                    strSname = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 11, 10);
                    strJumin1 = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 21, 6);
                    strJumin2 = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 27, 7);
                    strGwa = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 34, 2);
                    strRoom = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 36, 4);
                    StrIDate = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 40, 8);
                    StrODate = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 48, 8);
                    StrDrCode = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 56, 4);

                    if (FnGubun == 11 || FnGubun == 6 || FnGubun == 8 || FnGubun == 13)
                    {
                        StrTel = CPF.READ_BAS_SANID(clsDB.DbCon, strPANO);
                    }
                    else
                    {
                        StrTel = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 60, 14);
                    }

                    StrGamek = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 74, 1);
                    StrGel = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 75, 4);
                    strBasBi = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 79, 2);
                    strilsu = VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 81, 3);

                    SQL = "";
                    SQL = " SELECT TBed FROM BAS_ROOM WHERE RoomCode = " + VB.Val(strRoom) + " ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        StrTBed = dt.Rows[0]["TBed"].ToString().Trim();
                    }

                    SQL = "";
                    SQL = " SELECT Nal FROM IPD_CASH WHERE Pano = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + " AND ActDate = TO_DATE('" + dtpFdate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        StrTBed = dt.Rows[0]["Nal"].ToString().Trim();
                    }

                    SQL = "";
                    SQL = " SELECT DrName FROM BAS_DOCTOR WHERE DrCode = '" + StrDrCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        StrTBed = dt.Rows[0]["DrName"].ToString().Trim();
                    }

                    SQL = " SELECT DeptNameK FROM BAS_ClinicDept WHERE DeptCode = '" + strGwa + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        StrTBed = dt.Rows[0]["DeptNameK"].ToString().Trim();
                    }

                    if (strBi == "31" || strBi == "52")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT To_Char(Date1,'YY-MM-DD') Date1,CoprName,CoprNo    ";
                        SQL = SQL + ComNum.VBLF + "   FROM BAS_SANID                                          ";
                        SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPANO + "'                           ";
                        SQL = SQL + ComNum.VBLF + "    AND Bi = '" + strBi + "'                               ";


                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            StrTBed = dt.Rows[0]["Date1"].ToString().Trim();
                            StrTBed = dt.Rows[0]["CoprName"].ToString().Trim();
                            StrTBed = dt.Rows[0]["CoprNo"].ToString().Trim();
                        }
                        else
                        {
                            strDate = "";
                            strcop = "";
                            StrCarNo = "";
                        }
                        dt.Dispose();
                        dt = null;

                        if (strBi == "31")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT MAX(IPDTODATE) Date1       ";
                            SQL = SQL + ComNum.VBLF + "   FROM BAS_SANDTL                 ";
                            SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPANO + "'   ";

                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt.Rows.Count > 0)
                            {
                                StrTel = VB.Left(dt.Rows[0]["Date1"].ToString().Trim(), 4) + "-" + VB.Mid(dt.Rows[0]["Date1"].ToString().Trim(), 5, 2) + "-" + VB.Right(dt.Rows[0]["Date1"].ToString().Trim(), 2);
                            }
                            else
                            {
                                StrTel = "";
                            }
                            dt.Dispose();
                            dt = null;
                        }
                    }

                    strGamekName = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "2", "BAS_감액코드명", StrGamek);

                    if (strBasBi == strBi)
                        CPF.GelNameGubun(clsDB.DbCon, StrGel);
                    else

                        MihGelNameSearch(strPANO, strBi);

                    SS1BuildMoveSub();
                }
            }

        }

        private void rdoOptSelect_CheckedChanged(object sender, EventArgs e)
        {
            SS1_Sheet1.RowCount = 0;
            SS2_Sheet1.RowCount = 0;
            SS3_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;

            if (rdoOptSelect0.Checked == true)
            {
                FnSELECT = 1;
            }
            else if (rdoOptSelect1.Checked == true)
            {
                FnSELECT = 2;
            }
            else if (rdoOptSelect2.Checked == true)
            {
                FnSELECT = 3;
            }

            if (rdoOptSelect0.Checked == true)
            {
                SS1_Sheet1.Visible = true;
                SS1.Dock = DockStyle.Fill;
                SS2_Sheet1.Visible = false;
                SS3_Sheet1.Visible = false;
            }
            else if (rdoOptSelect1.Checked == true)
            {
                SS3_Sheet1.Visible = true;
                SS3.Dock = DockStyle.Fill;
                SS1_Sheet1.Visible = false;
                SS2_Sheet1.Visible = false;
            }
            else
            {
                if (FnGubun == 6)
                {
                    SS1_Sheet1.Visible = true;
                    SS1.Dock = DockStyle.Fill;
                    SS2_Sheet1.Visible = false;
                    SS3_Sheet1.Visible = false;
                }
                else
                {
                    SS3_Sheet1.Visible = true;
                    SS3.Dock = DockStyle.Fill;
                    SS1_Sheet1.Visible = false;
                    SS2_Sheet1.Visible = false;
                }
            }
        }



        private void rdoOptSortSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOptSortSelect0.Checked == true)
            {
                FnSortSELECT = 1;
            }
            else if (rdoOptSortSelect1.Checked == true)
            {
                FnSortSELECT = 2;
            }
            else if (rdoOptSortSelect2.Checked == true)
            {
                FnSortSELECT = 3;
            }
        }

        private void cboGubun2_SelectedIndexChanged(object sender, EventArgs e)
        {
            PatientGubun();
        }
    }
}


