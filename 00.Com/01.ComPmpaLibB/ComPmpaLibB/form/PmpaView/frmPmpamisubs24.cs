using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComDbB;
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " >> frmSupLbExSTS15.cs 폼이름 재정의" />


    public partial class frmPmpamisubs24 : Form
    {

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        double[,] FnAmt = new double[91, 8];
        string strYYMM_1 = "";
        int nHang = 0;
        double nAmt = 0;
        string strFDate = "";
        string strTdate = "";
        string strYYMM = "";

        public frmPmpamisubs24()
        {
            InitializeComponent();
        }

        private void frmPmpamisubs24_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 36, "", "2");
            //clsVbfunc.SetCboDate
            cboYYMM.SelectedIndex = 0;
            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            string SQL = "";

            btnPrint.Enabled = false;

            SS1_Sheet1.Cells[0, 2, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Text = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            strYYMM_1 = CPF.DATE_YYMM_ADD(strYYMM, -1);//, -1)

            for (i = 1; i <= 90; i++)
            {
                for (j = 1; j <= 7; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }

            try
            {
                CmdView_IwolJewonMisu(clsDB.DbCon,ref j);//'이월 재원미수액
                CmdView_SuipTotAmt(clsDB.DbCon, ref j);//'당월 수입총진료비
                CmdView_JungMirAmt(clsDB.DbCon, ref j);//'당월 중간청구액 계산
                CmdView_JanJewonMisu(clsDB.DbCon, ref j);//'월말 재원미수액

                for (i = 1; i <= 6; i++)
                {
                    //'당월소계
                    FnAmt[9, i] = FnAmt[6, i] - FnAmt[7, i] - FnAmt[8, i];
                    //'재원미수금 차액
                    FnAmt[24, i] = FnAmt[19, i] - (FnAmt[1, i] + FnAmt[6, i] - FnAmt[10, i]);
                    //'중간납입액 차액
                    if (i == 6)
                        FnAmt[25, i] = FnAmt[23, i] - (FnAmt[5, i] + FnAmt[7, i] - FnAmt[15, i]);
                    //'중간청구액 차액
                    FnAmt[26, i] = FnAmt[22, i] - (FnAmt[4, i] + FnAmt[8, i] - FnAmt[11, i]);
                }

                //'금액을 Display
                for (i = 1; i <= 26; i++)
                {
                    for (j = 1; j <= 6; j++)
                    {
                        SS1_Sheet1.Cells[i - 1, j + 1].Text = FnAmt[i, j].ToString("###,###,###,##0");
                    }
                }
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void CmdView_IwolJewonMisu(PsmhDb pDbCon, ref int j)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {


                SQL = "";
                SQL = "SELECT SuBi,SUM(TotAmt) TotAmt,SUM(JohapMisu) JohapMisu,";
                SQL = SQL + ComNum.VBLF + " SUM(BoninMisu) BoninMisu,SUM(JungAmt) JungAmt,";
                SQL = SQL + ComNum.VBLF + " SUM(IpgumAmt) IpgumAmt ";
                SQL = SQL + ComNum.VBLF + " FROM MISU_BALJEWON ";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM='" + strYYMM_1 + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY SuBi ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SuBi ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(dt.Rows[i]["SuBi"].ToString().Trim());
                        //'이월 총진료비
                        nHang = 1;
                        nAmt = Convert.ToDouble(dt.Rows[i]["TotAmt"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + nAmt;
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + nAmt;

                        //'이월 조합미수금
                        nHang = 2;
                        nAmt = Convert.ToDouble(dt.Rows[i]["JohapMisu"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + nAmt;
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + nAmt;
                        //'이월 개인미수금
                        nHang = 3;
                        nAmt = Convert.ToDouble(dt.Rows[i]["BoninMisu"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + nAmt;
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + nAmt;
                        //'이월 중간청구액
                        nHang = 4;
                        nAmt = Convert.ToDouble(dt.Rows[i]["JungAmt"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + nAmt;
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + nAmt;
                        //'이월 중간납입액
                        nHang = 5;
                        nAmt = Convert.ToDouble(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + nAmt;
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + nAmt;
                    }

                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// '당월 총수입금,퇴원내역
        /// </summary>
        private void CmdView_SuipTotAmt(PsmhDb pDbCon, ref int j)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int K = 0;
            try
            {

                SQL = "";
                SQL = "SELECT * FROM MISU_BALDAILY ";
                SQL = SQL + ComNum.VBLF + "WHERE ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND IpdOpd = 'I' ";// '입원

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(dt.Rows[i]["BiGbn"].ToString().Trim());

                        //'당월총수입금액
                        nHang = 6;
                        FnAmt[nHang, j] = FnAmt[nHang, j] + Convert.ToDouble(dt.Rows[i]["Amt26"].ToString().Trim());
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + Convert.ToDouble(dt.Rows[i]["Amt26"].ToString().Trim());
                        //'당월 중간납+보증금
                        nHang = 7;
                        FnAmt[nHang, j] = FnAmt[nHang, j] + Convert.ToDouble(dt.Rows[i]["Amt29"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + Convert.ToDouble(dt.Rows[i]["Amt30"].ToString().Trim());
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + Convert.ToDouble(dt.Rows[i]["Amt29"].ToString().Trim());
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + Convert.ToDouble(dt.Rows[i]["Amt30"].ToString().Trim());
                        //'퇴원총진료비 ~ 원단위절사액

                        for (K = 31; K <= 39; K++)
                        {
                            nHang = K - 21;
                            FnAmt[nHang, j] = FnAmt[nHang, j] + Convert.ToDouble(dt.Rows[i]["Amt" + K.ToString("00")].ToString().Trim());
                            FnAmt[nHang, 6] = FnAmt[nHang, 6] + Convert.ToDouble(dt.Rows[i]["Amt" + K.ToString("00")].ToString().Trim());
                        }
                    }

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// '당월 중간청구액 계산
        /// </summary>
        private void CmdView_JungMirAmt(PsmhDb pDbCon,ref int j)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int K = 0;
            try
            {

                SQL = "";
                SQL = "SELECT Bi,BuildJAmt FROM MIR_IPDID ";
                SQL = SQL + ComNum.VBLF + "WHERE BuildDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BuildDate<=TO_DATE('" + strTdate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "  AND YYMM >= '200112' ";
                SQL = SQL + ComNum.VBLF + "  AND Flag='1' ";// '중간청구를 Build한것만

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        j = CPM.READ_Bi_SuipTong(dt.Rows[i]["Bi"].ToString().Trim(), strFDate);

                        //'당월중간청구액
                        nHang = 8;
                        FnAmt[nHang, j] = FnAmt[nHang, j] + Convert.ToDouble(dt.Rows[i]["BuildJAmt"].ToString().Trim());
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + Convert.ToDouble(dt.Rows[i]["BuildJAmt"].ToString().Trim());
                    }

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }
        }

        private void CmdView_JanJewonMisu(PsmhDb pDbCon,ref int j)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {


                SQL = "";
                SQL = "SELECT SuBi,SUM(TotAmt) TotAmt,SUM(JohapMisu) JohapMisu,";
                SQL = SQL + ComNum.VBLF + " SUM(BoninMisu) BoninMisu,SUM(JungAmt) JungAmt,";
                SQL = SQL + ComNum.VBLF + " SUM(IpgumAmt) IpgumAmt ";
                SQL = SQL + ComNum.VBLF + " FROM MISU_BALJEWON ";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM='" + strYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY SuBi ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SuBi ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(dt.Rows[i]["SuBi"].ToString().Trim());
                        //'월말 총진료비
                        nHang = 19;
                        nAmt = Convert.ToDouble(dt.Rows[i]["TotAmt"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + nAmt;
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + nAmt;
                        //'월말 조합미수금
                        nHang = 20;
                        nAmt = Convert.ToDouble(dt.Rows[i]["JohapMisu"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + nAmt;
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + nAmt;
                        //'월말 개인미수금
                        nHang = 21;
                        nAmt = Convert.ToDouble(dt.Rows[i]["BoninMisu"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + nAmt;
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + nAmt;
                        //'월말 중간청구액
                        nHang = 22;
                        nAmt = Convert.ToDouble(dt.Rows[i]["JungAmt"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + nAmt;
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + nAmt;
                        //'이월 중간납입액
                        nHang = 23;
                        nAmt = Convert.ToDouble(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                        FnAmt[nHang, j] = FnAmt[nHang, j] + nAmt;
                        FnAmt[nHang, 6] = FnAmt[nHang, 6] + nAmt;
                    }



                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "월말현재 재원미수금 집계표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 70, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
