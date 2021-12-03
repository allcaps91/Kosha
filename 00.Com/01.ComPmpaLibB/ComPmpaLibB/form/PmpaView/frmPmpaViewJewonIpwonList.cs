using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

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
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\IPD\ilrepb\Frm재원입원퇴원명부" >> frmPmpaViewJewonIpwonList.cs 폼이름 재정의" />

    public partial class frmPmpaViewJewonIpwonList : Form
    {

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();

        int FnGubun = 0;

        string strPANO = "";
        string strSname = "";
        string strJumin1 = "";
        string strJumin2 = "";
        string strGwa = "";
        string strRoom = "";
        string strilsu = "";
        string StrIDate = "";
        string StrODate = "";
        string StrDrCode = "";
        string StrTel = "";
        string StrGamek = "";
        string strDate = "";
        string strcop = "";
        string StrCarNo = "";
        string StrPname = "";
        string strGwange = "";
        string strKiho = "";
        string strBiName = "";
        string StrDrName = "";
        string strBasBi = "";

        public frmPmpaViewJewonIpwonList()
        {
            InitializeComponent();
        }

        private void frmPmpaViewJewonIpwonList_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}
            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpFdate2.Value = Convert.ToDateTime(strDTP);
            dtpTdate2.Value = Convert.ToDateTime(strDTP);
            dtpFdate3.Value = Convert.ToDateTime(strDTP);
            dtpTdate3.Value = Convert.ToDateTime(strDTP);

            tabPage1.Select();

            cboGubun.Items.Clear();
            cboGubun.Items.Add("전체                    00");//       'ALL
            cboGubun.Items.Add("보험(전체)              01  ");// '11-15, 41-45
            cboGubun.Items.Add("보험(공단)              11");// '11
            cboGubun.Items.Add("보험(직장)              12");// '12
            cboGubun.Items.Add("보험(지역)              13");// '13
            cboGubun.Items.Add("보호                    21");// '21-25
            cboGubun.Items.Add("산재                    31");//       '31
            cboGubun.Items.Add("공상                    32");//       '32
            cboGubun.Items.Add("산재공상                33");//   '33
            cboGubun.Items.Add("보험계약                45");//   '45
            cboGubun.Items.Add("일반                    51");//       '51,54
            cboGubun.Items.Add("TA보험                  52");//     '52
            cboGubun.Items.Add("일반계약                53");//   '53
            cboGubun.Items.Add("TA일반                  55");//     '55
            cboGubun.SelectedIndex = 0;


            cboGubun2.Items.Clear();
            cboGubun2.Items.Add("전체                    00");//       'ALL
            cboGubun2.Items.Add("보험(전체)              01  ");// '11-15, 41-45
            cboGubun2.Items.Add("보험(공단)              11");// '11
            cboGubun2.Items.Add("보험(직장)              12");// '12
            cboGubun2.Items.Add("보험(지역)              13");// '13
            cboGubun2.Items.Add("보호                    21");// '21-25
            cboGubun2.Items.Add("산재                    31");//       '31
            cboGubun2.Items.Add("공상                    32");//       '32
            cboGubun2.Items.Add("산재공상                33");//   '33
            cboGubun2.Items.Add("보험계약                45");//   '45
            cboGubun2.Items.Add("일반                    51");//       '51,54
            cboGubun2.Items.Add("TA보험                  52");//     '52
            cboGubun2.Items.Add("일반계약                53");//   '53
            cboGubun2.Items.Add("TA일반                  55");//     '55
            cboGubun2.SelectedIndex = 0;


            cboGubun3.Items.Clear();
            cboGubun3.Items.Add("전체                    00");//       'ALL
            cboGubun3.Items.Add("보험(전체)              01  ");// '11-15, 41-45
            cboGubun3.Items.Add("보험(공단)              11");// '11
            cboGubun3.Items.Add("보험(직장)              12");// '12
            cboGubun3.Items.Add("보험(지역)              13");// '13
            cboGubun3.Items.Add("보호                    21");// '21-25
            cboGubun3.Items.Add("산재                    31");//       '31
            cboGubun3.Items.Add("공상                    32");//       '32
            cboGubun3.Items.Add("산재공상                33");//   '33
            cboGubun3.Items.Add("보험계약                45");//   '45
            cboGubun3.Items.Add("일반                    51");//       '51,54
            cboGubun3.Items.Add("TA보험                  52");//     '52
            cboGubun3.Items.Add("일반계약                53");//   '53
            cboGubun3.Items.Add("TA일반                  55");//     '55
            cboGubun3.SelectedIndex = 0;
        }

        private int PatientGubun()
        {

            if (Tab.SelectedIndex == 0)
                switch (VB.Left(cboGubun.Text, 10))
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



            else if (Tab.SelectedIndex == 1)
            {
                switch (VB.Left(cboGubun2.Text, 10))
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

            else if (Tab.SelectedIndex == 2)
                switch (VB.Left(cboGubun3.Text, 10))
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
            return FnGubun;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            PatientGubun();

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            switch (FnGubun)
            {
                case 1:
                    strTitle = "재  원  자  명  부 (보험-전체) ";
                    break;
                case 2:
                    strTitle = "재  원  자  명  부 (보험-공단) ";
                    break;
                case 3:
                    strTitle = "재  원  자  명  부 (보험-직장) ";
                    break;
                case 4:
                    strTitle = "재  원  자  명  부 (보험-지역) ";
                    break;
                case 5:
                    strTitle = "재  원  자  명  부 (보호) ";
                    break;
                case 6:
                    strTitle = "재  원  자  명  부 (산재) ";
                    break;
                case 7:
                    strTitle = "재  원  자  명  부 (공상) ";
                    break;
                case 8:
                    strTitle = "재  원  자  명  부 (산재공상) ";
                    break;
                case 9:
                    strTitle = "재  원  자  명  부 (보험계약) ";
                    break;
                case 10:
                    strTitle = "재  원  자  명  부 (일반) ";
                    break;
                case 11:
                    strTitle = "재  원  자  명  부 (TA보험) ";
                    break;
                case 12:
                    strTitle = "재  원  자  명  부 (일반계약) ";
                    break;
                case 13:
                    strTitle = "재  원  자  명  부 (TA일반) ";
                    break;
                default:
                    strTitle = "재  원  자  명  부 (전체) ";
                    break;
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            PatientGubun();

            switch (FnGubun)
            {
                case 1:
                    strTitle = "일일 입원환자 대장 (보험-전체) ";
                    break;
                case 2:
                    strTitle = "일일 입원환자 대장 (보험-공단) ";
                    break;
                case 3:
                    strTitle = "일일 입원환자 대장 (보험-직장) ";
                    break;
                case 4:
                    strTitle = "일일 입원환자 대장 (보험-지역) ";
                    break;
                case 5:
                    strTitle = "일일 입원환자 대장 (보호) ";
                    break;
                case 6:
                    strTitle = "일일 입원환자 대장 (산재) ";
                    break;
                case 7:
                    strTitle = "일일 입원환자 대장 (공상) ";
                    break;
                case 8:
                    strTitle = "일일 입원환자 대장 (산재공상) ";
                    break;
                case 9:
                    strTitle = "일일 입원환자 대장 (보험계약) ";
                    break;
                case 10:
                    strTitle = "일일 입원환자 대장 (일반) ";
                    break;
                case 11:
                    strTitle = "일일 입원환자 대장 (TA보험) ";
                    break;
                case 12:
                    strTitle = "일일 입원환자 대장 (일반계약) ";
                    break;
                case 13:
                    strTitle = "일일 입원환자 대장 (TA일반) ";
                    break;
                default:
                    strTitle = "일일 입원환자 대장 (전체) ";
                    break;
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnPrint3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            PatientGubun();

            switch (FnGubun)
            {

                case 1:
                    strTitle = "퇴  원  자  명  부 (보험-전체) ";
                    break;
                case 2:
                    strTitle = "퇴  원  자  명  부 (보험-공단) ";
                    break;
                case 3:
                    strTitle = "퇴  원  자  명  부 (보험-직장) ";
                    break;
                case 4:
                    strTitle = "퇴  원  자  명  부 (보험-지역) ";
                    break;
                case 5:
                    strTitle = "퇴  원  자  명  부 (보호) ";
                    break;
                case 6:
                    strTitle = "퇴  원  자  명  부 (산재) ";
                    break;
                case 7:
                    strTitle = "퇴  원  자  명  부 (공상) ";
                    break;
                case 8:
                    strTitle = "퇴  원  자  명  부 (산재공상) ";
                    break;
                case 9:
                    strTitle = "퇴  원  자  명  부 (보험계약) ";
                    break;
                case 10:
                    strTitle = "퇴  원  자  명  부 (일반) ";
                    break;
                case 11:
                    strTitle = "퇴  원  자  명  부 (TA보험) ";
                    break;
                case 12:
                    strTitle = "퇴  원  자  명  부 (일반계약) ";
                    break;
                case 13:
                    strTitle = "퇴  원  자  명  부 (TA일반) ";
                    break;
                default:
                    strTitle = "퇴  원  자  명  부 (전체) ";
                    break;
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void SS1_Display()
        {
            switch (VB.Right(cboGubun.Text, 2))
            {
                case "31":
                case "55":
                    SS1_Sheet1.ColumnHeader.Cells[0, 10].Text = "진단만료일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 13].Text = "사고일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 16].Text = "회사명";
                    break;

                case "52":
                    SS1_Sheet1.ColumnHeader.Cells[0, 10].Text = "진단만료일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 13].Text = "차량번호";
                    SS1_Sheet1.ColumnHeader.Cells[0, 16].Text = "계약처";
                    break;

                default:
                    SS1_Sheet1.ColumnHeader.Cells[0, 10].Text = "퇴원일";
                    SS1_Sheet1.ColumnHeader.Cells[0, 13].Text = "전화번호";
                    SS1_Sheet1.ColumnHeader.Cells[0, 16].Text = "비    고";
                    break;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            int j = 0;
            string StrRemark = "";
            string strBi = "";
            string strErChk = "";
            string strRev = "";
            string SQL = "";
            DataTable dt = null;
            DataTable dtRs = null;
            string SqlErr = "";

            strBi = VB.Right(cboGubun.Text, 2);

            if (strBi == "01")
            {
                strBi = "11','12','13','40','41','42','43";
            }
            else if (strBi == "21")
            {
                strBi = "21','22','23','24";
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.Pano,A.Bi,A.Sname,a.Pname,b.Gwange,Jumin1,Jumin2,        ";
                SQL = SQL + ComNum.VBLF + "        A.DeptCode, A.Sex, A.Age,A.RoomCode,                       ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(Indate,'YYYY-MM-DD HH24:MI') IDate,                ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(Outdate,'YYYY-MM-DD') ODate,                       ";
                SQL = SQL + ComNum.VBLF + "        A.DrCode,Tel,A.GbGamek,Kiho,B.BI BASBI,A.Ilsu,             ";
                SQL = SQL + ComNum.VBLF + "        A.AmSet3,A.AmSet1,A.Remark,A.AMSET7                        ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "IPD_NEW_MASTER A    ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE IS NULL                                           ";
                SQL = SQL + ComNum.VBLF + "   AND a.Pano <> '81000004'                                        "; //'전산실연습 제외1
                SQL = SQL + ComNum.VBLF + "   AND a.GbSTS IN  ('0')                                           ";
                if (strBi != "00")
                    SQL = SQL + ComNum.VBLF + "   AND a.BI IN ('" + strBi + "')             ";
                SQL = SQL + ComNum.VBLF + "   AND AmSet6 <> '*'                                               "; //'환자구분변경
                SQL = SQL + ComNum.VBLF + "   AND AmSet4 <> '3'                                               "; //'정상애기
                SQL = SQL + ComNum.VBLF + "   AND A.Pano = B.Pano                                             ";

                if (rdoOptSortSelect0.Checked == true)
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.Sname, A.Pano                                      ";
                else if (rdoOptSortSelect1.Checked == true)
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.WardCode, A.RoomCode, A.Pano                       ";
                else
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.DeptCode, A.DrCode, A.Pano                         ";

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

                SS1_Sheet1.RowCount = 0;
                SS1_Sheet1.RowCount = 1;

                SS1_Sheet1.Rows.Count = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                SS1_Display();

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

                    switch (dt.Rows[i]["AMSET7"].ToString().Trim())
                    {
                        case "3":
                        case "4":
                        case "5":
                            strErChk = "⊙";
                            break;
                        default:
                            strErChk = "";
                            break;
                    }

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
                    if (dt.Rows.Count > 0)
                    {
                        StrDrName = dtRs.Rows[0]["DrName"].ToString().Trim();
                    }
                    StrTel = dt.Rows[i]["Tel"].ToString().Trim();
                    StrGamek = dt.Rows[i]["GbGamek"].ToString().Trim();

                    dtRs.Dispose();
                    dtRs = null;

                    switch (VB.Right(cboGubun.Text, 1))
                    {
                        case "1":
                            strKiho = dt.Rows[i]["Kiho"].ToString().Trim();
                            break;
                        default:
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

                            break;
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
                        SS1_Sheet1.Cells[i, 10].Text = StrTel.Trim();
                        SS1_Sheet1.Cells[i, 13].Text = strDate.Trim();
                        SS1_Sheet1.Cells[i, 16].Text = StrRemark.Trim();
                    }
                    // 'ElseIf strBi = "52" Or strBi = "55" Then '2009-10-07 윤조연 함종현주임 요청 주석 55 별도 표시
                    else if (strBi == "52")
                    {
                        SS1_Sheet1.Cells[i, 10].Text = StrTel.Trim();
                        SS1_Sheet1.Cells[i, 13].Text = StrCarNo.Trim();
                        SS1_Sheet1.Cells[i, 16].Text = strKiho.Trim();
                    }
                    else if (strBi == "55")
                    {
                        SS1_Sheet1.Cells[i, 10].Text = StrTel.Trim();
                        SS1_Sheet1.Cells[i, 13].Text = StrCarNo.Trim();
                        SS1_Sheet1.Cells[i, 16].Text = strKiho.Trim() + "-" + StrRemark.Trim();
                    }
                    else
                    {
                        SS1_Sheet1.Cells[i, 5].Text = strKiho.Trim();
                        SS1_Sheet1.Cells[i, 10].Text = StrODate.Trim();
                        SS1_Sheet1.Cells[i, 13].Text = StrTel.Trim();
                        SS1_Sheet1.Cells[i, 16].Text = StrRemark.Trim();
                    }
                    SS1_Sheet1.Cells[i, 11].Text = strilsu.Trim();
                    SS1_Sheet1.Cells[i, 12].Text = StrDrName.Trim();
                    SS1_Sheet1.Cells[i, 14].Text = strErChk;
                    SS1_Sheet1.Cells[i, 15].Text = strRev;

                    strKiho = "";
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

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            int j = 0;
            string StrFDay = "";
            string StrTDay = "";
            string strDD = "";
            string strBi = "";
            string strBiName = "";
            string strErChk = "";
            string strRev = "";
            string SQL = "";
            DataTable dt = null;
            DataTable dtRs = null;
            string SqlErr = "";

            StrFDay = dtpFdate2.Value.ToString("yyyy-MM-dd");
            StrTDay = dtpFdate2.Value.AddDays(-1).ToString("yyyy-MM-dd");

            strBi = VB.Right(cboGubun2.Text, 2);

            if (strBi == "01")
            {
                strBi = "11','12','13','40','41','42','43";
            }
            else if (strBi == "21")
            {
                strBi = "21','22','23','24";
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.Pano, A.Sname, A.Bi, A.DeptCode, A.Drcode,       "; //
                SQL = SQL + ComNum.VBLF + "        A.RoomCode,A.Remark,                               "; //
                SQL = SQL + ComNum.VBLF + "        A.Sex, A.Age, A.ReliGion, B.JuMin1, B.JuMin2,      "; //
                SQL = SQL + ComNum.VBLF + "        B.JuSo, B.Tel,                                     "; //
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(Outdate,'YYYY-MM-DD') ODate,                 "; //
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(InDate,'YYYY-MM-DD HH24:MI') InDate,b.ZIPCODE1,      "; //
                SQL = SQL + ComNum.VBLF + "        b.ZIPCODE2,AMSET7                                  "; //
                SQL = SQL + ComNum.VBLF + "   FROM BAS_PATIENT B, IPD_NEW_MASTER A                    "; //
                SQL = SQL + ComNum.VBLF + "  WHERE Trunc(a.InDate) >= TO_DATE('" + dtpFdate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') "; //  '입원
                SQL = SQL + ComNum.VBLF + "    AND Trunc(a.InDate) <= TO_DATE('" + dtpTdate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') "; //    '입원
                SQL = SQL + ComNum.VBLF + "    AND a.Pano <> '81000004'                                "; // '전산실연습 제외
                SQL = SQL + ComNum.VBLF + "   AND a.GbSTS NOT IN  ('9')                                   "; //
                SQL = SQL + ComNum.VBLF + "   AND a.PANO = B.PANO                                     "; //

                if (strBi != "00")
                {
                    SQL = SQL + ComNum.VBLF + "  AND a.BI IN ('" + strBi + "')      ";
                }
                if (rdoOptSort0.Checked == true)
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.Sname, A.Pano                              ";
                else if (rdoOptSort1.Checked == true)
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.WardCode, A.RoomCode, A.Pano               ";
                else if (rdoOptSort3.Checked == true)
                    SQL = SQL + ComNum.VBLF + " ORDER BY InDate,  A.Pano               ";
                else
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.DeptCode, A.DrCode, A.Pano                 ";

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

                SS2_Sheet1.RowCount = 0;

                SS2_Sheet1.Rows.Count = dt.Rows.Count;
                SS2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS2_Sheet1.Cells[i, 0].Text = i + 1.ToString();
                    SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();

                    switch (dt.Rows[i]["Bi"].ToString().Trim())
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

                    SS2_Sheet1.Cells[i, 3].Text = strBiName;
                    SS2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ReMark"].ToString().Trim();

                    switch (dt.Rows[i]["AMSET7"].ToString().Trim())
                    {
                        case "3":
                        case "4":
                        case "5":
                            strErChk = "⊙";
                            break;
                        default:
                            strErChk = "";
                            break;
                    }

                    if (dt.Rows[i]["PANO"].ToString().Trim() == "03921540")
                    {
                        i = i;
                    }

                    SQL = " SELECT PANO FROM IPD_RESERVED ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND CDATE = TO_DATE('" + VB.Left(dt.Rows[0]["InDate"].ToString().Trim(), 10) + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND (GBCHK IS NULL OR GBCHK = '0') ";

                    SqlErr = clsDB.GetDataTable(ref dtRs, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strRev = "⊙";
                    }
                    else
                    {
                        strRev = "";
                    }
                    dtRs.Dispose();
                    dtRs = null;

                    SS2_Sheet1.Cells[i, 9].Text = strErChk;
                    SS2_Sheet1.Cells[i, 10].Text = strRev;

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

        private void btnSearch3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            string StrRemark = "";
            string strBi = "";
            string strRoutDate = "";
            string strBun = "";
            string strErChk = "";
            string strRev = "";
            string SQL = "";
            DataTable dt = null;
            DataTable dtRs = null;
            string SqlErr = "";

            strBi = VB.Right(cboGubun3.Text, 2);

            if (strBi == "01")
            {
                strBi = "11','12','13','40','41','42','43";
            }
            else if (strBi == "21")
            {
                strBi = "21','22','23','24";
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.Pano, A.Bi, A.Sname, a.Pname, b.Gwange, Jumin1, Jumin2,                      ";
                SQL = SQL + ComNum.VBLF + "        A.DeptCode, A.Sex, A.Age,A.RoomCode,                                           ";//
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.Indate,'YYYY-MM-DD') IDate, TO_CHAR(a.Outdate,'YYYY-MM-DD') ODate,                       ";//
                SQL = SQL + ComNum.VBLF + "        A.DrCode, Tel, A.GbGamek, b.Kiho, B.BI BASBI,A.Ilsu, A.AmSet3,       ";
                SQL = SQL + ComNum.VBLF + "        A.AmSet1, A.Remark, C.AMSET5,A.AMSET7,                                         ";//
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(C.ROUTDATE,'YYYY-MM-DD HH24:MI') ROUTDATE                              ";
                SQL = SQL + ComNum.VBLF + "   FROM BAS_PATIENT B, IPD_NEW_MASTER A, IPD_TRANS C                                   ";//
                SQL = SQL + ComNum.VBLF + " WHERE A.OUTDATE >= TO_DATE('" + dtpFdate3.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                       ";
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE <= TO_DATE('" + dtpTdate3.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                       ";
                SQL = SQL + ComNum.VBLF + "   AND a.Pano <> '81000004'                                                            ";// //'전산실연습 제외1
                SQL = SQL + ComNum.VBLF + "   AND a.GbSTS IN  ('7')                                                               ";//
                SQL = SQL + ComNum.VBLF + "   AND a.LASTTRS = c.TRSNO                                                             ";//
                if (strBi != "00")
                    SQL = SQL + ComNum.VBLF + "   AND a.BI IN ('" + strBi + "')                                 ";//
                SQL = SQL + ComNum.VBLF + "   AND a.AmSet6 <> '*'                                                                 ";// '환자구분변경
                SQL = SQL + ComNum.VBLF + "   AND a.AmSet4 <> '3'                                                                 ";// '정상애기
                SQL = SQL + ComNum.VBLF + "   AND A.Pano = B.Pano                                                                 ";//

                if (rdoOptSortSelect0.Checked == true)
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.Sname, A.Pano                                                          ";

                else if (rdoOptSortSelect1.Checked == true)
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.WardCode, A.RoomCode, A.Pano                                           ";

                else if (rdoOptSortSelect2.Checked == true)
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.DeptCode, A.DrCode, A.Pano                                             ";

                else
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.ACTDATE,A.DeptCode, A.DrCode, A.Pano                                    ";

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

                SS3_Sheet1.RowCount = 0;
                SS3_Sheet1.Rows.Count = dt.Rows.Count;
                SS3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

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
                    strRoutDate = dt.Rows[i]["ROUTDATE"].ToString().Trim();
                    StrDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                    StrRemark = dt.Rows[i]["Remark"].ToString().Trim();

                    switch (dt.Rows[i]["AMSET5"].ToString().Trim())
                    {
                        case "1":
                            strBun = "완쾌";
                            break;
                        case "2":
                            strBun = "자의";
                            break;
                        case "3":
                            strBun = "사망";
                            break;
                        case "4":
                            strBun = "전원";
                            break;
                        default:
                            strBun = "";
                            break;
                    }

                    switch (dt.Rows[i]["AMSET7"].ToString().Trim())
                    {
                        case "3":
                        case "4":
                        case "5":
                            strErChk = "⊙";
                            break;
                        default:
                            strErChk = "";
                            break;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT PANO                         ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_RESERVED                     ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND CDATE = TO_DATE('" + StrIDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND GBCHK IS NULL ";

                    SqlErr = clsDB.GetDataTable(ref dtRs, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dtRs.Rows.Count > 0)
                    {
                        strRev = "⊙";
                    }
                    else
                    {
                        strRev = "";
                    }

                    dtRs.Dispose();
                    dtRs = null;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT DrName                         ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
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

                    switch (VB.Right(cboGubun.Text, 1))
                    {
                        case "1":
                            strKiho = dt.Rows[i]["Kiho"].ToString().Trim();
                            break;
                        default:
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

                            break;
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
                        SQL = SQL + ComNum.VBLF + " SELECT To_Char(Date1,'YYYY-MM-DD') SDate1,CoprName,CoprNo FROM " + ComNum.DB_PMPA + "BAS_SANID  ";
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
                        if (dtRs.Rows.Count > 0)
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
                        if (dtRs.Rows.Count > 0)
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
                        else
                        {
                            StrTel = "";
                        }
                        dtRs.Dispose();
                        dtRs = null;
                    }

                    SS3_Sheet1.Cells[i, 0].Text = strPANO.Trim();
                    SS3_Sheet1.Cells[i, 1].Text = strBiName.Trim();
                    SS3_Sheet1.Cells[i, 2].Text = StrPname.Trim();
                    SS3_Sheet1.Cells[i, 3].Text = strGwange.Trim();
                    SS3_Sheet1.Cells[i, 4].Text = strSname.Trim();

                    SS3_Sheet1.Cells[i, 5].Text = strKiho.Trim();
                    SS3_Sheet1.Cells[i, 6].Text = strJumin1.Trim() + "-" + strJumin2.Trim();
                    SS3_Sheet1.Cells[i, 7].Text = strGwa.Trim();
                    SS3_Sheet1.Cells[i, 8].Text = strRoom.Trim();
                    SS3_Sheet1.Cells[i, 9].Text = StrIDate.Trim();


                    SS3_Sheet1.Cells[i, 10].Text = StrODate.Trim();
                    SS3_Sheet1.Cells[i, 11].Text = strRoutDate.Trim();
                    SS3_Sheet1.Cells[i, 12].Text = strilsu.Trim();
                    SS3_Sheet1.Cells[i, 13].Text = StrDrName.Trim();
                    SS3_Sheet1.Cells[i, 14].Text = StrTel.Trim();
                    SS3_Sheet1.Cells[i, 15].Text = strBun.Trim();
                    SS3_Sheet1.Cells[i, 16].Text = strErChk;
                    SS3_Sheet1.Cells[i, 17].Text = strRev;
                    SS3_Sheet1.Cells[i, 18].Text = StrRemark.Trim();

                    strKiho = "";
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SS3_CellClick(object sender, CellClickEventArgs e)
        {
            SS1_Sheet1.Cells[0, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            SS1_Sheet1.Cells[e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }
    }
}
