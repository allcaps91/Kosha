using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComNurLibB;
using ComLibB;

namespace ComNurLibB
{
    public partial class frmNrCodeTongBuild : Form
    {
        string[,] nDATA = new string[2, 60];
        string[,] nDATA2 = new string[30, 6];
        string[,,] nDATA3 = new string[25, 42, 20];
        string[,,] nDATA4 = new string[3, 37, 40];

        const int MB_ICONQUESTION = 32;
        const int MB_YESNO = 4;
        const int MB_DEFBUTTON1 = 0;
        const int IDYES = 6;

        int nJusaCount = 0;
        int nGumuCount = 0;
        string GstrMsgTitle = "";
        string GstrMsgList = "";
        int GnMsgType = 0;
        int GnMsgReturn = 0;

        string strSdate = "";
        string strEdate = "";
        string strYYMM = "";

        ComFunc cfun = new ComFunc();


        public frmNrCodeTongBuild()
        {
            InitializeComponent();
        }

        void frmNrCodeTongBuild_Load(object sender, EventArgs e)
        {
            //FormInfo_History();
            int nYY = 0;
            int nMM = 0;
            string strYYMM = "";
            int i = 0;

            string strSql = string.Empty;
            DataTable dt = null;

            cfun.Read_SysDate();

            //Format : YYYY
            nYY = Convert.ToInt16(VB.Left((cfun.strSysDate), 4));
            //Format : M
            nMM = Convert.ToInt16(VB.Mid(cfun.strSysDate, 6, 2));

            //Format : YYYYMM
            strYYMM = VB.Left(cfun.strSysDate, 4) + VB.Mid(cfun.strSysDate, 6, 2);

            cboDate.Items.Clear();         
            for(i = 1; i < 12; i++)
            {
                cboDate.Items.Add(VB.Left(strYYMM, 4) + "년" + VB.Right(strYYMM, 2) + "월");
                strYYMM = clsNurse.DATE_YYMM_ADD(strYYMM, -1);                
            }
            cboDate.SelectedIndex = 1;
            strYYMM = "";

            // 주사코드 setting
            strSql = "";
            strSql = strSql + ComNum.VBLF + "SELECT";
            strSql = strSql + ComNum.VBLF + "    CODE ";
            strSql = strSql + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
            strSql = strSql + ComNum.VBLF + " WHERE Gubun= '3' ";
            dt = clsDB.GetDataTable(strSql);

            if(dt == null)
            {
                ComFunc.MsgBox("주사코드 setting error");
                return;
            }

            nJusaCount = 0;

            for(i = 0; i < dt.Rows.Count; i++)
            {
                //TODO
                //Do While Not Rs.EOF: nJusaCount = nJusaCount + 1: Rs.MoveNext: Loop
                //Rs.MoveFirst
                //i = 0
                //Do While Not Rs.EOF
                //nDATA(1, i + 1) = Trim(Rs!Code + "")
                //i = i + 1
                //Rs.MoveNext
                //Loop
                //Rs.Close: Set Rs = Nothing

                nJusaCount = nJusaCount + 1;

                nDATA[1, i + 1] = dt.Rows[i]["Code"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            // 근무형태 setting
            strSql = "";
            strSql = strSql + ComNum.VBLF + "SELECT";
            strSql = strSql + ComNum.VBLF + "    CODE ";
            strSql = strSql + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
            strSql = strSql + ComNum.VBLF + " WHERE Gubun= '4' ";
            strSql = strSql + ComNum.VBLF + " ORDER BY PRINTRANKING ";
            dt = clsDB.GetDataTable(strSql);

            if (dt == null)
            {
                ComFunc.MsgBox("근무형태 setting error");
                return;
            }

            nGumuCount = 0;

            for(i=0; i < dt.Rows.Count; i++)
            {
                nGumuCount = nGumuCount + 1;

                nDATA4[1, 1, i + 1] = dt.Rows[i]["Code"].ToString().Trim();
                //TODO
                //Rs.MoveFirst
                //i = 0
                //Do While Not Rs.EOF
                //nDATA4(1, 1, i + 1) = Trim(Rs!Code + "")
                //i = i + 1
                //Rs.MoveNext
                //Loop
                //Rs.Close: Set Rs = Nothing
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnStart_Click(object sender, EventArgs e)
        {
            int i, j, k;

            if(chkGun.Checked == false && chkInjection.Checked == false && chkPRN.Checked == false && chkW.Checked == false)
            {
                ComFunc.MsgBox("작업구분을 선택해주세요.");
            }
            txtMessage.Text = "BUILD 중입니다 ....";

            strYYMM = VB.Left(cboDate.SelectedItem.ToString().Trim(), 4) + VB.Mid(cboDate.SelectedItem.ToString().Trim(), 6, 2);
            strSdate = VB.Left(cboDate.SelectedItem.ToString().Trim(), 4) + "-" + VB.Mid(cboDate.SelectedItem.ToString().Trim(), 6, 2) + "-01";

            strEdate = clsNurse.READ_LASTDAY(strSdate);

            //clear
            for(i = 1; i < 27; i++)
            {
                for(j = 1; j < 7; j++)
                {
                    // nDATA2(i, j) = "";
                    nDATA2[i, j] = "";
                }
            }

            for(i = 1; i < 43; i++)
            {
                // nDATA(2, i) = "";
                nDATA2[2, i] = "";
            }

            for(i = 0; i < 26; i++)
            {
                for(j = 1; j < 43; j++)
                {
                    for(k = 1; k < 21; k++)
                    {
                        // nDATA3(i, j, k) = "";
                        nDATA3[i, j, k] = "";
                    }
                }
            }

            if(chkW.Checked == true)
            {
                txtMessage.Text = "간호부 통계 BUILD중....";
                Ganho_Tong_Build();
            }

            // If GnJobSabun = 4349 Then Exit Sub

            if (chkPRN.Checked == true)
            {
                txtMessage.Text = "외래 간호부 통계 BUILD중....";
                Opd_Tong_Build();
                chkPRN.ForeColor = Color.Blue;
            }

            if (chkInjection.Checked == true)
            {
                txtMessage.Text = "주사실 통계 BUILD중....";
                Jusa_Tong_Build();
                chkInjection.ForeColor = Color.Blue;
            }

            if (chkGun.Checked == true)
            {
                txtMessage.Text = "근무형태 통계 BUILD중....";
                Gunmu_Tong_Build();
                chkGun.ForeColor = Color.Blue;
            }

            txtMessage.Text = "작업완료....";
        }



        void Ganho_Tong_Build()
        {

            string[][] str = null; 

            int i, j, k;
            int nCount = 0;
            int nDept = 0;
            int nWard = 0;
            string strSabun = "";
            int nGubun = 0;
            string strDEPT = "";
            string strWard = "";
            string strJik = "";
            int nIlsu = 0;

            string cYYMM = "";
            string cWardCode = "";
            string cDeptCode = "";
            string cIlsu = "";
            string cTotbed = "";
            string cTotal = "";
            string cIpInwon = "";
            string cJewon = "";
            string cTewon = "";
            string cTrans = "";
            string cOp = "";
            string cDied = "";
            string cKeep = "";
            string cDelivery = "";
            string cGoOut = "";
            string cNomalBaByM = "";
            string cNomalBaByF = "";
            string cDelivery2 = "";
            string cDelivery3 = "";

            string strSql = string.Empty;
            DataTable dt = null;

            // 병동 통계 Build 여부 Check
            // TODO
            NUR_TONG1_BUILD_CHECK();

            //먼저 build 된 것을 삭제
            clsDB.setBeginTran();
            try
            {
                strSql = "";
                strSql = strSql + ComNum.VBLF + " DELETE FROM ";
                strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_TONG1 ";
                strSql = strSql + ComNum.VBLF + "WHERE YYMM = '" + strYYMM + "' ";
                clsDB.ExecuteNonQuery(strSql);

                clsDB.setCommitTran();
                ComFunc.MsgBox("삭제하였습니다.");
            }
            catch (Exception e)
            {
                clsDB.setRollbackTran();
                ComFunc.MsgBox("");
            }

            dt.Dispose();
            dt = null;

            // Debug.Rpint SQLRET
            // BED 수 통계 build

            try
            {
                strSql = "";
                strSql = strSql + ComNum.VBLF + " SELECT CODE, JIK ";
                strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";
                strSql = strSql + ComNum.VBLF + "WHERE GUBUN = '7' ";
                clsDB.ExecuteNonQuery(strSql);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회 중 에러 발생");
                }
            }
            catch (Exception e)
            {
                clsDB.setRollbackTran();
                ComFunc.MsgBox("해당월에는 build할 BED수 데이타가 없습니다.");
            }

            for(i = 0; i < dt.Rows.Count; i++)
            {
                strWard = dt.Rows[i]["Code"].ToString().Trim();
                // TODO
                Ward_Gubun();
                // TODO
                // nDATA3(0, nWard, 1) = Trim(Str(Val(nDATA3(0, nWard, 1)) + Val(Rs!Jik + "")))
                nDATA3[0, nWard, 1] = Convert.ToString(nDATA3[0, nWard, 1]).Trim() + VB.Val(dt.Rows[i]["JIK"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            try
            {
                strSql = "";
                strSql = strSql + ComNum.VBLF + " SELECT DEPTCODE, WARDCODE, CNT11+CNT12 CNT1, CNT21+CNT22 CNT2, CNT31+CNT32 CNT3, ";
                strSql = strSql + ComNum.VBLF + "        CNT51, CNT52, CNT51+CNT52 CNT5 ";
                strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_JEWON ";
                strSql = strSql + ComNum.VBLF + "WHERE ACTDATE >=TO_DATE('" + strSdate + "','YYYY-MM-DD')";
                strSql = strSql + ComNum.VBLF + "   AND ACTDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD')";
                strSql = strSql + ComNum.VBLF + "   AND DEPTCODE IN ('MD','GS','OG','GY','PD','OS','NS','CS','NP','EN','OT','UR',";
                strSql = strSql + ComNum.VBLF + "                    'DM','DT','NB','IQ','DB', 'NE',";
                strSql = strSql + ComNum.VBLF + "                    'MC','ME','MG','MN','MP','MR','MI')";
                strSql = strSql + ComNum.VBLF + "WHERE ACTDATE >=TO_DATE('" + strSdate + "','YYYY-MM-DD')";
                strSql = strSql + ComNum.VBLF + " AND ACTDATE >=TO_DATE('" + strSdate + "','YYYY-MM-DD')";
                strSql = strSql + ComNum.VBLF + " AND WARDCODE <> 'ER'";

                dt = clsDB.GetDataTable(strSql);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회 중 에러 발생");
                }
            }
            catch (Exception e)
            {
                clsDB.setRollbackTran();
                ComFunc.MsgBox("해당월에는 build할 병동 입/퇴/재원 데이타가 없습니다.");
            }

            for(i = 0; i < dt.Rows.Count; i++)
            {
                strDEPT = dt.Rows[i]["Deptcode"].ToString().Trim();
                //TODO
                Dept_Gubun();
                strWard = dt.Rows[i]["Wardcode"].ToString().Trim();

                string str1 = "200504";
                string str2 = "200912";

                string str3 = "200407";
                

                if (string.Compare(strYYMM, str1, true) == 0 && (string.Compare(strYYMM, str2, true) == 0))
                {
                    if (strWard == "NR") strWard = "ND";
                }              

                else if(string.Compare(strYYMM, str3, true) == 0 && (string.Compare(strYYMM, str1, true) == 0))
                {
                    if (strWard == "NR") strWard = "ND";
                    if (strWard == "DR") strWard = "55";
                }

                Ward_Gubun();

                if(strDEPT == "NB")
                {
                    //TODO
                    //nDATA3(nDept, nWard, 12) = Str(Val(nDATA3(nDept, nWard, 12)) + Val(Trim(Rs!Cnt51 + ""))) '정상아 남
                    //nDATA3(nDept, nWard, 13) = Str(Val(nDATA3(nDept, nWard, 13)) + Val(Trim(Rs!Cnt52 + ""))) '정상아 여
                    nDATA3[nDept, nWard, 12] = Convert.ToString(VB.Val(nDATA3[nDept, nWard, 12])) + VB.Val(dt.Rows[i]["Cnt51"].ToString().Trim());
                    nDATA3[nDept, nWard, 13] = Convert.ToString(VB.Val(nDATA3[nDept, nWard, 13])) + VB.Val(dt.Rows[i]["Cnt52"].ToString().Trim());


                }

                //TODO
                //nDATA3(nDept, nWard, 2) = Str(Val(nDATA3(nDept, nWard, 2)) + Val(Trim(Rs!Cnt1 + ""))) '입원
                //nDATA3(nDept, nWard, 3) = Str(Val(nDATA3(nDept, nWard, 3)) + Val(Trim(Rs!Cnt5 + ""))) '재원
                //nDATA3(nDept, nWard, 4) = Str(Val(nDATA3(nDept, nWard, 4)) + Val(Trim(Rs!Cnt2 + ""))) '퇴원
                //nDATA3(nDept, nWard, 5) = Str(Val(nDATA3(nDept, nWard, 5)) + Val(Trim(Rs!Cnt3 + "")))  '이실
                nDATA3[nDept, nWard, 2] = Convert.ToString(VB.Val(nDATA3[nDept, nWard, 2])) + VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim());
                nDATA3[nDept, nWard, 3] = Convert.ToString(VB.Val(nDATA3[nDept, nWard, 3])) + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim());
                nDATA3[nDept, nWard, 4] = Convert.ToString(VB.Val(nDATA3[nDept, nWard, 4])) + VB.Val(dt.Rows[i]["Cnt2"].ToString().Trim());
                nDATA3[nDept, nWard, 5] = Convert.ToString(VB.Val(nDATA3[nDept, nWard, 5])) + VB.Val(dt.Rows[i]["Cnt3"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            // 수술, 사망, KEEP, 정상분만, 이상분만, 제왕절개 통계 BUILD
            try
            {
                strSql = "";
                strSql = strSql + ComNum.VBLF + " SELECT WARDCODE, SUM(QTY1+QTY2+QTY3+QTY4) QTY , CODE ";
                strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_INOUT ";
                strSql = strSql + ComNum.VBLF + " WHERE CODE IN ('05','06','07','08','12','13') ";
                strSql = strSql + ComNum.VBLF + "  AND ACTDATE >=TO_DATE('" + strSdate + "','YYYY-MM-DD')";
                strSql = strSql + ComNum.VBLF + "  AND ACTDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                strSql = strSql + ComNum.VBLF + "  AND WARDCODE <> 'EM' ";
                strSql = strSql + ComNum.VBLF + " GROUP BY WARDCODE,CODE";
                dt = clsDB.GetDataTable(strSql);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회 중 에러 발생");
                }
            }
            catch (Exception e)
            {
                clsDB.setRollbackTran();
                ComFunc.MsgBox("해당월에는 build할 수술,사망, keep,delivery 데이타가 없습니다.");
            }

            for(i = 0; i < dt.Rows.Count; i++)
            {
                strWard = dt.Rows[i]["Wardcode"].ToString().Trim();
                Ward_Gubun();


                switch (dt.Rows[i]["Code"].ToString().Trim())
                {
                    //TODO
                    //Case "05": nDATA3(0, nWard, 6) = Str(Val(nDATA3(0, nWard, 6)) + Val(Trim(Rs!Qty + ""))) '수술
                    //Case "06": nDATA3(0, nWard, 7) = Str(Val(nDATA3(0, nWard, 7)) + Val(Trim(Rs!Qty + "")))  '사망
                    //Case "07": nDATA3(0, nWard, 8) = Str(Val(nDATA3(0, nWard, 8)) + Val(Trim(Rs!Qty + ""))) 'KEEP
                    //Case "08": nDATA3(0, nWard, 9) = Str(Val(nDATA3(0, nWard, 9)) + Val(Trim(Rs!Qty + ""))) '정상분만
                    //Case "12": nDATA3(0, nWard, 10) = Str(Val(nDATA3(0, nWard, 10)) + Val(Trim(Rs!Qty + ""))) '이상분만
                    //Case "13": nDATA3(0, nWard, 11) = Str(Val(nDATA3(0, nWard, 11)) + Val(Trim(Rs!Qty + ""))) '제왕절개
                    case "05":
                        nDATA3[0, nWard, 6] = Convert.ToString(VB.Val(nDATA3[0, nWard, 6])) + VB.Val(dt.Rows[i]["Qty" + ""].ToString().Trim());
                        break;
                    case "06":
                        nDATA3[0, nWard, 7] = Convert.ToString(VB.Val(nDATA3[0, nWard, 7])) + VB.Val(dt.Rows[i]["Qty" + ""].ToString().Trim());
                        break;
                    case "07":
                        nDATA3[0, nWard, 8] = Convert.ToString(VB.Val(nDATA3[0, nWard, 8])) + VB.Val(dt.Rows[i]["Qty" + ""].ToString().Trim());
                        break;
                    case "08":
                        nDATA3[0, nWard, 9] = Convert.ToString(VB.Val(nDATA3[0, nWard, 9])) + VB.Val(dt.Rows[i]["Qty" + ""].ToString().Trim());
                        break;
                    case "12":
                        nDATA3[0, nWard, 10] = Convert.ToString(VB.Val(nDATA3[0, nWard, 10])) + VB.Val(dt.Rows[i]["Qty" + ""].ToString().Trim());
                        break;
                    case "13":
                        nDATA3[0, nWard, 11] = Convert.ToString(VB.Val(nDATA3[0, nWard, 11])) + VB.Val(dt.Rows[i]["Qty" + ""].ToString().Trim());
                        break;

                }
            }

            for(i = 0; i < 25; i++)
            {
                switch (i)
                {
                    case 0:
                        cDeptCode = "";
                        break;

                    case 1:
                        cDeptCode = "MD";
                        break;

                    case 2:
                        cDeptCode = "GD";
                        break;

                    case 3:
                        cDeptCode = "OG";
                        break;

                    case 4:
                        cDeptCode = "PD";
                        break;

                    case 5:
                        cDeptCode = "OS";
                        break;

                    case 6:
                        cDeptCode = "NS";
                        break;

                    case 7:
                        cDeptCode = "CS";
                        break;

                    case 8:
                        cDeptCode = "NP";
                        break;

                    case 9:
                        cDeptCode = "EN";
                        break;

                    case 10:
                        cDeptCode = "OT";
                        break;

                    case 11:
                        cDeptCode = "UR";
                        break;

                    case 12:
                        cDeptCode = "DM";
                        break;

                    case 13:
                        cDeptCode = "DT";
                        break;

                    case 14:
                        cDeptCode = "IQ";
                        break;

                    case 15:
                        cDeptCode = "DB";
                        break;

                    case 16:
                        cDeptCode = "PC";
                        break;

                    case 17:
                        cDeptCode = "NE";
                        break;

                    case 18:
                        cDeptCode = "MC";
                        break;

                    case 19:
                        cDeptCode = "ME";
                        break;

                    case 20:
                        cDeptCode = "MG";
                        break;

                    case 21:
                        cDeptCode = "MN";
                        break;

                    case 22:
                        cDeptCode = "MP";
                        break;

                    case 23:
                        cDeptCode = "MR";
                        break;

                    case 24:
                        cDeptCode = "MI";
                        break;

                    default:
                        break;
                }

                if(cWardCode == "SICU")
                {
                   // j = j;
                }

                for(j = 1; j < 42; j++)
                {
                    cYYMM = strYYMM.Trim();

                    switch (j)
                    {
                        case 1:
                            cWardCode = "2W";
                            break;

                        case 2:
                            cWardCode = "3A";
                            break;

                        case 3:
                            cWardCode = "3B";
                            break;

                        case 4:
                            cWardCode = "4A";
                            break;

                        case 5:
                            cWardCode = "5W";
                            break;

                        case 6:
                            cWardCode = "6W";
                            break;

                        case 7:
                            cWardCode = "7W";
                            break;

                        case 8:
                            cWardCode = "8W";
                            break;

                        case 9:
                            cWardCode = "MICU";
                            break;

                        case 10:
                            cWardCode = "SICU";
                            break;

                        case 11:
                            cWardCode = "ER";
                            break;

                        case 12:
                            cWardCode = "DR";
                            break;

                        case 13:
                            cWardCode = "NR";
                            break;

                        case 14:
                            cWardCode = "HD";
                            break;

                        case 15:
                            cWardCode = "HU";
                            break;

                        case 16:
                            cWardCode = "ND";
                            break;

                        case 17:
                            cWardCode = "3C";
                            break;

                        case 18:
                            cWardCode = "3W";
                            break;

                        case 19:
                            cWardCode = "4W";
                            break;

                        case 20:
                            cWardCode = "6A";
                            break;

                        case 21:
                            cWardCode = "32";
                            break;

                        case 22:
                            cWardCode = "52";
                            break;

                        case 23:
                            cWardCode = "53";
                            break;

                        case 24:
                            cWardCode = "62";
                            break;

                        case 25:
                            cWardCode = "63";
                            break;

                        case 26:
                            cWardCode = "72";
                            break;

                        case 27:
                            cWardCode = "73";
                            break;

                        case 28:
                            cWardCode = "51";
                            break;

                        case 29:
                            cWardCode = "41";
                            break;

                        case 30:
                            cWardCode = "DS";
                            break;

                        case 31:
                            cWardCode = "71";
                            break;

                        case 32:
                            cWardCode = "81";
                            break;

                        case 33:
                            cWardCode = "50";
                            break;

                        case 34:
                            cWardCode = "60";
                            break;

                        case 35:
                            cWardCode = "70";
                            break;

                        case 36:
                            cWardCode = "80";
                            break;

                        case 37:
                            cWardCode = "55";
                            break;

                        case 38:
                            cWardCode = "65";
                            break;

                        case 39:
                            cWardCode = "75";
                            break;

                        case 40:
                            cWardCode = "33";
                            break;

                        case 41:
                            cWardCode = "35";
                            break;

                        default:
                            break;
                    }

                    nIlsu = Convert.ToInt16(clsNurse.DATE_ILSU(strEdate, strSdate));
                    cIlsu = Convert.ToString(nIlsu);


                    //TODO
                    //cIlsu = Val(Trim(nIlsu)):                 cTotbed = Val(Trim(nDATA3(0, j, 1)))
                    //cIpInwon = Val(Trim(nDATA3(i, j, 2))):    cJewon = Val(Trim(nDATA3(i, j, 3)))
                    //cTewon = Val(Trim(nDATA3(i, j, 4))):      cTrans = Trim(nDATA3(i, j, 5))

                    //cOp = Trim(nDATA3(i, j, 6)):         cDied = Trim(nDATA3(i, j, 7))
                    //cKeep = Trim(nDATA3(i, j, 8)):       cDelivery = Trim(nDATA3(i, j, 9))
                    //cDelivery2 = Trim(nDATA3(i, j, 10)): cDelivery3 = Trim(nDATA3(i, j, 11))
                    //cGoOut = "0":                        cNomalBaByM = Trim(nDATA3(i, j, 12))
                    //cNomalBaByF = Trim(nDATA3(i, j, 13))

                    cIlsu = Convert.ToString(nIlsu);
                    cTotbed = Convert.ToString(VB.Val(nDATA3[0, j, 1]));
                    cIpInwon = Convert.ToString(VB.Val(nDATA3[i, j, 4]));
                    cJewon = nDATA3[i, j, 5].Trim();
                    cTewon = Convert.ToString(VB.Val(nDATA3[i, j, 4]));
                    cTrans = nDATA3[i, j, 5].Trim();

                    cOp = nDATA3[i, j, 6].Trim();
                    cDied = nDATA3[i, j, 7].Trim();
                    cKeep = nDATA3[i, j, 8].Trim();
                    cDelivery = nDATA3[i, j, 9].Trim();
                    cDelivery2 = nDATA3[i, j, 10].Trim();
                    cDelivery3 = nDATA3[i, j, 11].Trim();
                    cGoOut = "0";
                    cNomalBaByM = nDATA3[i, j, 12].Trim();
                    cNomalBaByF = nDATA3[i, j, 13].Trim();


                    if(cWardCode == "SICU")                    
                    {
                        cWardCode = cWardCode;
                    }

                    clsDB.setBeginTran();
                    try
                    {
                        strSql = "";
                        strSql = strSql + ComNum.VBLF + "INSERT INTO ";
                        strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_TONG1 ";
                        strSql = strSql + ComNum.VBLF + " (YYMM, WARDCODE, DEPTCODE, ILSU, TOTBED, IPINWON, JEWON, TEWON, TRANS, ";
                        strSql = strSql + ComNum.VBLF + " OP, DIED, KEEP, DELIVERY, GOOUT, NOMALBABYM, NOMALBABYF,DELIVERY2,DELIVERY3) ";
                        strSql = strSql + ComNum.VBLF + " VALUES ('" + cYYMM + "','" + cWardCode + "','" + cDeptCode + "',";
                        strSql = strSql + ComNum.VBLF + "         '" + cIlsu + "','" + cTotbed + "','" + cIpInwon + "',";
                        strSql = strSql + ComNum.VBLF + "         '" + cJewon + "','" + cTewon + "', '" + cTrans + "',";
                        strSql = strSql + ComNum.VBLF + "         '" + cOp + "','" + cDied + "','" + cKeep + "','" + cDelivery + "',";
                        strSql = strSql + ComNum.VBLF + "         '" + cGoOut + "','" + cNomalBaByM + "','" + cNomalBaByF + "',";
                        strSql = strSql + ComNum.VBLF + "         '" + cDelivery2 + "','" + cDelivery3 + "')";          
                        clsDB.ExecuteNonQuery(strSql);

                        clsDB.setCommitTran();
                        ComFunc.MsgBox("등록하였습니다.");
                    }
                    catch (Exception e)
                    {
                        clsDB.setRollbackTran();
                        ComFunc.MsgBox(e.Message);
                    }
                     
                }
            }

            chkW.ForeColor = Color.Blue;
        }

        /// <summary>
        /// // 병동 통계 Build 여부 Check
        /// </summary>
        void NUR_TONG1_BUILD_CHECK()
        {
            int i = 0;
            string strSql = string.Empty;
            DataTable dt = null;

            strSql = "";
            strSql = strSql + ComNum.VBLF + "SELECT";
            strSql = strSql + ComNum.VBLF + " * FROM " + ComNum.DB_PMPA + "NUR_TONG1";
            strSql = strSql + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "' ";
            dt = clsDB.GetDataTable(strSql);

            if (dt == null)
            {
                ComFunc.MsgBox("조회 중 문제 발생");
                return;
            }

            GstrMsgTitle = "확 인";
            GstrMsgList = "이미 당월의 간호부 통계자료가 형성되어 있읍니다" + VB.Chr(13);
            GstrMsgList = Convert.ToString(MB_ICONQUESTION + MB_YESNO + MB_DEFBUTTON1);
            //GnMsgReturn = ComFunc.MsgBox(GstrMsgList, GnMsgType, GstrMsgTitle);

            if (GnMsgReturn != IDYES) return;




        }

        void Ward_Gubun()
        {

        }

        void Dept_Gubun()
        {

        }

        void Opd_Tong_Build()
        {

        }

        void Jusa_Tong_Build()
        {

        }

        void Gunmu_Tong_Build()
        {

        }

        private void cboDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                chkW.Focus();
            }
        }


        //TODO
        //Private Sub ComboYYMM_GotFocus()
        //Chk01.Value = False
        //Chk02.Value = False
        //Chk03.Value = False
        //Chk04.Value = False
        //LabMassage.Caption = ""
        //Chk01.ForeColor = RGB(0, 0, 0)
        //Chk02.ForeColor = RGB(0, 0, 0)
        //Chk03.ForeColor = RGB(0, 0, 0)
        //Chk04.ForeColor = RGB(0, 0, 0)
        //End Sub




    }
}
