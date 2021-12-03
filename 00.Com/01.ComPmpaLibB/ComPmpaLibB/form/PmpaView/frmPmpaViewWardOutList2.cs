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
    /// Create Date     : 2017-10-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref=  d:\psmh\IPD\iviewa\iviewa.vbp\Frm퇴원예고자.frm" >> frmPmpaViewWardOutList.cs 폼이름 재정의" />

    public partial class frmPmpaViewWardOutList2 : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string strInDate = "";
        string strPtno = "";
        string strPrtGb = "";
        string prtSubTitle = "";
        int i = 0;
        int Response = 0;
        int nIndex = 0;


        public frmPmpaViewWardOutList2()
        {
            InitializeComponent();
        }

        private void frmPmpaViewWardOutList2_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "     AND WARDCODE NOT IN ('IU','NP','2W','NR','IQ') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

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

                cboWard.Items.Clear();
                cboWard.Items.Add("전체");

                SS1_Sheet1.Rows.Count = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                }
                cboWard.Items.Add("SICU");
                cboWard.Items.Add("MICU");

                cboWard.SelectedIndex = 0;

                dtpFdate.Value = Convert.ToDateTime(strDTP);
                dtpTDate.Value = Convert.ToDateTime(strDTP).AddDays(-1);

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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int nRow = 0;

            SS1_Sheet1.Cells[0, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Text = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";


                SQL = SQL + ComNum.VBLF + "   TO_CHAR(M.ROutDate,'YYYY-MM-DD HH24:MI') MROUTDATE,";
                SQL = SQL + ComNum.VBLF + "   TO_CHAR(M.SimsaTime,'YYYY-MM-DD HH24:MI') SimsaTime,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(E.ROutDate,'YYYY-MM-DD') ROutDate, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(E.ROutENTTIME,'YYYY-MM-DD HH24:MI') ROutENTTIME, ";



                // 'SunapTime


                SQL = SQL + ComNum.VBLF + "  E.InRoom, E.InDept, ";
                SQL = SQL + ComNum.VBLF + " M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet6,M.AmSet7 ";
                SQL = SQL + ComNum.VBLF + " FROM   ADMIN.IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_DOCTOR  D, ";
                SQL = SQL + ComNum.VBLF + "        ADMIN.NUR_MASTER  E ";

                switch (cboWard.Text)
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='233' ";
                        break;
                    case "ND":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ') ";        //'윤조연추가 2005-03-29break;
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode='" + cboWard.Text + "' ";
                        break;
                }
                if (clsPublic.GnJobSabun != 4349)
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.Pano<>'81000004' ";
                }
                SQL = SQL + ComNum.VBLF + " AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + dtpFdate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000' ";
                SQL = SQL + ComNum.VBLF + " AND M.Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                SQL = SQL + ComNum.VBLF + "  AND M.Pano=P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "  AND M.Ipdno=E.Ipdno(+) ";
                SQL = SQL + ComNum.VBLF + "  AND M.DrCode=D.DrCode(+) ";
                //'nur_,master
                SQL = SQL + ComNum.VBLF + "  AND trunc(E.ROutDate) =TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (rdoOptgb3.Checked == true)//'17: 30 이전등록
                {
                    SQL = SQL + ComNum.VBLF + "  AND E.ROutEntTime < TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + " 17:31','YYYY-MM-DD HH24:MI') ";
                }
                else if (rdoOptgb2.Checked == true) // '18시이전등록
                {
                    SQL = SQL + ComNum.VBLF + "  AND E.ROutEntTime < TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + " 18:01','YYYY-MM-DD HH24:MI') ";
                }
                else if (rdoOptgb0.Checked == true) // '17시이전등록
                {
                    SQL = SQL + ComNum.VBLF + "  AND E.ROutEntTime < TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + " 17:01','YYYY-MM-DD HH24:MI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TRUNC(E.ROutEntTime) <= TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }

                if (cboWard.Text != "전체")
                {
                    if (cboWard.Text == "MICU")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND E.InWard ='IU'";

                        SQL = SQL + ComNum.VBLF + "   AND E.InRoom ='234'";
                    }
                    else if (cboWard.Text == "SICU")
                    {

                        SQL = SQL + ComNum.VBLF + "   AND E.InWard ='IU'";
                        SQL = SQL + ComNum.VBLF + "   AND E.InRoom ='233'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND E.InWard ='" + cboWard.Text + "' ";
                    }
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY E.ROUTENTTIME ASC ";

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

                SS1_Sheet1.Rows.Count = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;
                    SS1_Sheet1.RowCount = nRow;

                    SS1_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["ROutDate"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["ROUTENTTIME"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 4].Text = Get_Bi(dt.Rows[i]["Bi"].ToString().Trim());
                    SS1_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["INROOM"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["INDEPT"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                    SS1_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["INDATE"].ToString().Trim();

                    //if (VB.Format(dt.Rows[i]["ROUTENTTIME"].ToString().Trim(), "YYYY-MM-DD") == dtpFdate.Value.ToString("yyyy-MM-dd"))
                    //{
                    //    SS1.Row = nRow: SS1.Col = -1: SS1.BackColor = RGB(255, 200, 200)
                    //}
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

        private string Get_Bi(string Bi)
        {
            string strVal = "";

            switch (Bi)
            {
                case "11":
                case "12":
                case "13":
                case "32":
                case "41":
                case "42":
                case "43":
                case "44":
                    strVal = "건강보험";
                    break;
                case "52":
                case "55":
                    strVal = "자보";
                    break;
                case "31":
                case "33":
                    strVal = "산재";
                    break;
                case "51":
                case "54":
                    strVal = "일반";
                    break;
                case "21":
                case "22":
                case "23":
                case "24":
                    strVal = "의료급여";
                    break;
                default:
                    strVal = "기타";
                    break;
            }
            return strVal;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }
            if (strPrtGb == "NO")
            {
                return;
            }

            if (ComFunc.MsgBoxQ("검색된 자료를 출력하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                strTitle = "퇴원예고자 명단";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("조회일자 : " + dtpFdate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
