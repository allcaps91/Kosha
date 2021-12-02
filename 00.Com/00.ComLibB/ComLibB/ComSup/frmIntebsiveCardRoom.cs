using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
//using MedIpdNr;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupInfc
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : 폼 호출
    /// </history>
    /// <seealso cref= D:\psmh\nurse\nrinfo\FrmICU" >> frmIntebsiveCardRoom.cs 폼이름 재정의" />
    /// 
    public partial class frmIntebsiveCardRoom : Form
    {
        public frmIntebsiveCardRoom()
        {
            InitializeComponent();
        }

        private void frmIntebsiveCardRoom_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComboYYMM.Items.Clear();
            clsVbfunc.SetCboDate(clsDB.DbCon, ComboYYMM, 24, "", "0");

            ComboYYMM.SelectedIndex = 0;

            ComboYear.Items.Clear();
            clsVbfunc.SetCboDateYY(clsDB.DbCon, ComboYear, 10, "1");

            ComboYear.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (OptMicu.Checked == true)
            {
                strTitle = "(" + ComboYYMM.Text + ") 집중치료실(MICU) 일자별 현황";
            }
            if (OptSicu.Checked == true)
            {
                strTitle = "(" + ComboYYMM.Text + ") 집중치료실(SICU) 일자별 현황";
            }


            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            strTitle = "ICU 연도별 사망자 통계";



            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("◈ 조회년도 : " + ComboYear.Text + "년도", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("◈ 출력일자 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ss2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int j = 0;
            int nCol = 0;
            string strSDate = "";
            string strEdate = "";
            int[,] nData = new int[14, 33];
            string strACTDATE = "";
            string strActDate_90 = "";
            string strPano = "";
            int nRead = 0;
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            ComFunc CF = new ComFunc();

            strSDate = VB.Left(ComboYYMM.Text, 4) + "-" + VB.Right(ComboYYMM.Text, 2) + "-01";
            strEdate = CF.READ_LASTDAY(clsDB.DbCon, strSDate);

            //clear
            for (i = 1; i <= 13; i++)
            {
                for (j = 1; j <= 32; j++)
                {
                    nData[i, j] = 0;
                }
            }

            ss1_Sheet1.RowCount = 0;
            ss1_Sheet1.RowCount = 13;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(JOBDATE,'DD') DAY, COUNT(PANO) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_BM ";
                SQL = SQL + ComNum.VBLF + " WHERE JobDate >=to_date('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND JobDate <= TO_DATE('" + strEdate + "','YYYY-MM-DD')";
                if (OptTotal.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE IN ('IU','32','33','35')";
                }
                else if (Opt32.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='32' ";
                }
                else if (Opt33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='33' ";
                }
                else if (Opt35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='35' ";
                }
                else if (OptMicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND ROOMCODE ='234' ";
                }
                else if (OptSicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND ROOMCODE ='233' ";
                }
                SQL = SQL + ComNum.VBLF + "   AND GbBackup='J' ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY JOBDATE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nCol = (int)VB.Val(dt.Rows[i]["DAY"].ToString().Trim());
                        nData[1, nCol] = nData[1, nCol] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        nData[1, 32] = nData[1, 32] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(A.ACTDATE,'DD') DAY ,";
                SQL = SQL + ComNum.VBLF + "       SUM(DECODE(B.BI, '12', 1, '11', 1, '13', 1, 0))  CNT1,";
                SQL = SQL + ComNum.VBLF + "       SUM(DECODE(B.BI, '12', 0 ,'11', 0, '13', 0, 1))  CNT2";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_JINDAN A, " + ComNum.DB_PMPA + "IPD_BM B";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >=TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.GUBUN IN ('I','T')";
                if (OptTotal.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE IN ('IU','32','33','35')";
                }
                else if (Opt32.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='32' ";
                }
                else if (Opt33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='33' ";
                }
                else if (Opt35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='35' ";
                }
                else if (OptMicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND B.ROOMCODE ='234' ";
                }
                else if (OptSicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND B.ROOMCODE ='233' ";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE = B.JOBDATE(+)";
                SQL = SQL + ComNum.VBLF + "   AND B.GBBACKUP ='J' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.ACTDATE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nCol = (int)VB.Val(dt.Rows[i]["DAY"].ToString().Trim());
                        nData[2, nCol] = nData[2, nCol] + (int)VB.Val(dt.Rows[i]["CNT1"].ToString().Trim());
                        nData[3, nCol] = nData[3, nCol] + (int)VB.Val(dt.Rows[i]["CNT2"].ToString().Trim());
                        nData[2, 32] = nData[2, 32] + (int)VB.Val(dt.Rows[i]["CNT1"].ToString().Trim());
                        nData[3, 32] = nData[3, 32] + (int)VB.Val(dt.Rows[i]["CNT2"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(JOBDATE,'DD') DAY ,";
                SQL = SQL + ComNum.VBLF + "       SUM(DECODE(BI, '12', 1, '11', 1, '13', 1, 0))  CNT1,";
                SQL = SQL + ComNum.VBLF + "       SUM(DECODE(BI, '12', 0 ,'11', 0, '13', 0, 1))  CNT2";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_BM ";
                SQL = SQL + ComNum.VBLF + " WHERE JOBDATE >=TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND JOBDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')";

                if (OptTotal.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE IN ('IU','32','33','35')";
                }
                else if (Opt32.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='32' ";
                }
                else if (Opt33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='33' ";
                }
                else if (Opt35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='35' ";
                }
                else if (OptMicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND ROOMCODE ='234' ";
                }
                else if (OptSicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND ROOMCODE ='233' ";
                }

                SQL = SQL + ComNum.VBLF + "   AND GBBACKUP ='J' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY JOBDATE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nCol = (int)VB.Val(dt.Rows[i]["DAY"].ToString().Trim());
                        nData[4, nCol] = nData[4, nCol] + (int)VB.Val(dt.Rows[i]["CNT1"].ToString().Trim());
                        nData[5, nCol] = nData[5, nCol] + (int)VB.Val(dt.Rows[i]["CNT2"].ToString().Trim());
                        nData[4, 32] = nData[4, 32] + (int)VB.Val(dt.Rows[i]["CNT1"].ToString().Trim());
                        nData[5, 32] = nData[5, 32] + (int)VB.Val(dt.Rows[i]["CNT2"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(A.ACTDATE,'DD') DAY ,";
                SQL = SQL + ComNum.VBLF + "       SUM(DECODE(B.BI, '12', 1, '11', 1, '13', 1, 0))  CNT1,";
                SQL = SQL + ComNum.VBLF + "       SUM(DECODE(B.BI, '12', 0 ,'11', 0, '13', 0, 1))  CNT2";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_JINDAN A, " + ComNum.DB_PMPA + "IPD_BM B";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >=TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.GUBUN IN ('I','T')";

                if (OptTotal.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE IN ('IU','32','33','35')";
                }
                else if (Opt32.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='32' ";
                }
                else if (Opt33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='33' ";
                }
                else if (Opt35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='35' ";
                }
                else if (OptMicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND B.ROOMCODE ='234' ";
                }
                else if (OptSicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND B.ROOMCODE ='233' ";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE = B.JOBDATE(+)";
                SQL = SQL + ComNum.VBLF + "   AND B.GBBACKUP='J' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.ACTDATE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nCol = (int)VB.Val(dt.Rows[i]["DAY"].ToString().Trim());
                        nData[6, nCol] = nData[6, nCol] + (int)VB.Val(dt.Rows[i]["CNT1"].ToString().Trim());
                        nData[7, nCol] = nData[7, nCol] + (int)VB.Val(dt.Rows[i]["CNT2"].ToString().Trim());
                        nData[6, 32] = nData[6, 32] + (int)VB.Val(dt.Rows[i]["CNT1"].ToString().Trim());
                        nData[7, 32] = nData[7, 32] + (int)VB.Val(dt.Rows[i]["CNT2"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.ACTDATE,'DD') DAY, ";
                SQL = SQL + ComNum.VBLF + "       SUM(DECODE(B.BI, '12', 1, '11', 1, '13', 1, 0))  CNT1, ";
                SQL = SQL + ComNum.VBLF + "       SUM(DECODE(B.BI, '12', 0 ,'11', 0, '13', 0, 1))  CNT2  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_JINDAN A, " + ComNum.DB_PMPA + "IPD_BM B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >=TO_DATE('" + strSDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD') ";

                if (OptTotal.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN ='O'  ";
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE IN ('IU','32','33','35')";
                }
                else if (Opt32.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN ='O'  ";
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='32' ";
                }
                else if (Opt33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN ='O'  ";
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='33' ";
                }
                else if (Opt35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN ='O'  ";
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='35' ";
                }
                else if (OptMicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND ( GUBUN ='O'  OR  ( GUBUN='T' AND ROOM NOT IN ('234'))) ";
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND B.ROOMCODE ='234' ";
                }
                else if (OptSicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND ( GUBUN ='O'  OR  ( GUBUN='T' AND ROOM NOT IN ('233'))) ";
                    SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND B.ROOMCODE ='233' ";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE = B.JOBDATE(+) ";
                SQL = SQL + ComNum.VBLF + "   AND B.GBBACKUP='J' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.ACTDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nCol = (int)VB.Val(dt.Rows[i]["DAY"].ToString().Trim());
                        nData[8, nCol] = nData[8, nCol] + (int)VB.Val(dt.Rows[i]["CNT1"].ToString().Trim());
                        nData[9, nCol] = nData[9, nCol] + (int)VB.Val(dt.Rows[i]["CNT2"].ToString().Trim());
                        nData[8, 32] = nData[8, 32] + (int)VB.Val(dt.Rows[i]["CNT1"].ToString().Trim());
                        nData[9, 32] = nData[9, 32] + (int)VB.Val(dt.Rows[i]["CNT2"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                //사망환자수(사망/hopelees/ama)
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(A.ACTDATE,'DD') DAY, COUNT(A.PANO) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_SPECIAL A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >=TO_DATE('" + strSDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.GUBUN='4' ";
                if (OptTotal.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE IN ('IU','32','33','35')";
                }
                else if (Opt32.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='32' ";
                }
                else if (Opt33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='33' ";
                }
                else if (Opt35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='35' ";
                }
                else if (OptMicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ROOMCODE ='234' ";
                }
                else if (OptSicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ROOMCODE ='233' ";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.ACTDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nCol = (int)VB.Val(dt.Rows[i]["DAY"].ToString().Trim());
                        nData[10, nCol] = nData[10, nCol] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        nData[10, 32] = nData[10, 32] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());

                    }
                }

                dt.Dispose();
                dt = null;

                //icu 재입원(48시간)
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(A.ACTDATE,'DD') DAY , ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, A.PANO  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_JINDAN A  ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')";
                ;
                SQL = SQL + ComNum.VBLF + "   AND A.H48 ='1' ";
                if (OptTotal.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE IN ('IU','32','33','35')";
                }
                else if (Opt32.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='32' ";
                }
                else if (Opt33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='33' ";
                }
                else if (Opt35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='35' ";
                }
                else if (OptMicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ROOM ='234' ";
                }
                else if (OptSicu.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='IU' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ROOM ='233' ";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY 1 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nCol = (int)VB.Val(dt.Rows[0]["DAY"].ToString().Trim());
                        nData[13, nCol] = nData[13, nCol] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim()) + 1;
                        nData[13, 32] = nData[13, 32] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim()) + 1;

                    }
                }

                dt.Dispose();
                dt = null;

                //display
                for (i = 1; i <= 13; i++)
                {
                    switch (i)
                    {
                        case 1:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "실 가동병상수";
                            break;
                        case 2:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "실환자(보험)";
                            break;
                        case 3:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "실환자(기타)";
                            break;
                        case 4:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "연환자(보험)";
                            break;
                        case 5:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "연환자(기타)";
                            break;
                        case 6:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "입실환자(보험)";
                            break;
                        case 7:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "입실환자(기타)";
                            break;
                        case 8:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "퇴실환자(보험)";
                            break;
                        case 9:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "퇴실환자(기타)";
                            break;
                        case 10:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "사망환자(사망)";
                            break;
                        case 13:
                            ss1_Sheet1.Cells[i - 1, 0].Text = "48시간 재입원환자수";
                            break;
                    }

                    for (j = 1; j <= 32; j++)
                    {
                        ss1_Sheet1.Cells[i - 1, j].Text = nData[i, j].ToString();

                        if (nData[i, j] != 0)
                        {
                            ss1_Sheet1.Cells[i - 1, j].BackColor = Color.FromArgb(200, 200, 255);
                        }
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

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string cYEAR = "";
            string cSDate = "";
            string cEDATE = "";
            int j = 0;
            int nTemp1 = 0;
            int nTemp2 = 0;
            int i = 0;
            int nRow = 0;
            int nCol = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;


            for (i = 2; i <= ss2_Sheet1.GetLastNonEmptyColumn(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                for (j = 1; j <= ss2_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); j++)
                {
                    ss2_Sheet1.Cells[j, i].Text = "";
                }
            }

            cYEAR = VB.Left(ComboYear.Text, 4);

            cSDate = cYEAR + "-01-01";
            cEDATE = cYEAR + "-12-31";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.WARDCODE, TO_CHAR(A.ACTDATE,'MM') DAY, COUNT(A.PANO) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_SPECIAL A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >=TO_DATE('" + cSDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + cEDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.GUBUN='4' ";
                SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE IN ('MICU', 'SICU', '32', '33', '35') ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.WARDCODE, A.ACTDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["WARDCODE"].ToString().Trim())
                        {
                            case "MICU":
                            case "35":
                                nRow = 1;
                                break;
                            case "SICU":
                            case "33":
                            case "32":
                                nRow = 4;
                                break;
                        }
                        nCol = (int)VB.Val(dt.Rows[i]["DAY"].ToString().Trim()) + 1;
                        ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text = (VB.Val(ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text) + VB.Val(dt.Rows[i]["CNT"].ToString().Trim())).ToString();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT WARDCODE, TO_CHAR (JOBDATE, 'MM') DAY, COUNT (PANO) CNT";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "IPD_BM";
                SQL = SQL + ComNum.VBLF + "    WHERE     JOBDATE >= TO_DATE ('" + cSDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND JOBDATE <= TO_DATE ('" + cEDATE + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND WARDCODE IN ('32','33','35', 'IU')";
                SQL = SQL + ComNum.VBLF + "          AND GBBACKUP = 'J'";
                SQL = SQL + ComNum.VBLF + " GROUP BY WARDCODE, JOBDATE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["WARDCODE"].ToString().Trim())
                        {
                            case "234":
                            case "35":
                                nRow = 2;
                                break;
                            case "233":
                            case "33":
                            case "32":
                                nRow = 5;
                                break;
                        }
                        nCol = (int)VB.Val(dt.Rows[i]["DAY"].ToString().Trim()) + 1;
                        ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text = (VB.Val(ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text) + VB.Val(dt.Rows[i]["CNT"].ToString().Trim())).ToString();
                    }
                }

                dt.Dispose();
                dt = null;

                for (i = 1; i <= ss2_Sheet1.RowCount; i++)
                {
                    nTemp1 = 0;
                    for (j = 2; j < ss2_Sheet1.ColumnCount; j++)
                    {
                        nTemp1 = nTemp1 + (int)VB.Val(ss2_Sheet1.Cells[i - 1, j - 1].Text);
                    }

                    ss2_Sheet1.Cells[i - 1, ss2_Sheet1.ColumnCount - 1].Text = nTemp1.ToString();
                }

                for (i = 1; i <= 2; i++)
                {
                    for (j = 2; j <= ss2_Sheet1.ColumnCount; j++)
                    {
                        if (i == 1)
                        {
                            nTemp1 = (int)VB.Val(ss2_Sheet1.Cells[0, j - 1].Text);
                            nTemp2 = (int)VB.Val(ss2_Sheet1.Cells[1, j - 1].Text);

                            if (nTemp2 > 0)
                            {
                                ss2_Sheet1.Cells[2, j - 1].Text = (nTemp1 / (double)nTemp2 * 100).ToString("#0.0");
                            }
                        }
                        else if (i == 2)
                        {
                            nTemp1 = (int)VB.Val(ss2_Sheet1.Cells[3, j - 1].Text);
                            nTemp2 = (int)VB.Val(ss2_Sheet1.Cells[4, j - 1].Text);

                            if (nTemp2 > 0)
                            {
                                ss2_Sheet1.Cells[5, j - 1].Text = (nTemp1 / (double)nTemp2 * 100).ToString("#0.0");
                            }
                        }
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

        private void ss1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string Jewon = "";
            string dead = "";
            string strDate = "";
            string strICU = "";

            if (e.Column == 32 || e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }

            if (ss1_Sheet1.Cells[e.Row, e.Column].Text == "0")
            {
                return;
            }
            strDate = VB.Left(ComboYYMM.Text, 4) + "-" + VB.Right(ComboYYMM.Text, 2) + "-" + (e.Column - 1).ToString("00");

            if (OptMicu.Checked == true)
            {
                strICU = "MICU";
            }
            if (OptSicu.Checked == true)
            {
                strICU = "SICU";
            }
            switch (e.Row)
            {
                // TODO : 폼 호출
                case 12:
                    //frmICUdtl f = new frmICUdtl("13", strDate, "MICU");
                    //f.ShowDialog();
                    break;
                case 9:
                    //frmICUdtl f1 = new frmICUdtl("10", strDate, "SICU");
                    //f1.ShowDialog();
                    break;
            }
        }
    }
}
