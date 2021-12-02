using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewWardListByGrade.cs
    /// Description     : 등급별 병실현황 및 병실차액
    /// Author          : 박창욱
    /// Create Date     : 2017-11-03
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\frm등급별병실현황.frm(frm등급별병실현황.frm) >> frmPmpaViewWardListByGrade.cs 폼이름 재정의" />
    public partial class frmPmpaViewWardListByGrade : Form
    {
        string[] strHoSil = new string[8];

        public frmPmpaViewWardListByGrade()
        {
            InitializeComponent();
        }

        private void chkJob_CheckedChanged(object sender, EventArgs e)
        {
            if (chkJob.Checked == true)
            {
                cboYear.Enabled = true;
            }
            else
            {
                cboYear.Enabled = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "병 실  등 급 별  현 황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("기준일자 : " + dtpDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Read_Bas_Room();

        }

        void Read_Bas_Room()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nHoNo = 0;
            int nIlsu = 0;
            double nWardAmt = 0;
            double nOverAmt = 0;
            double nSwWardAmt = 0;
            double nSwOverAmt = 0;
            string strSwCode = "";
            string strSwClass = "";
            string strRoomCode = "";
            string strRoomClass = "";
            string strText = "";
            string strFDate = "";
            string strTDate = "";
            string strRoom = "";

            btnSearch.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 30;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.RoomCode, a.RoomClass, a.IlbanAmt,";
                SQL = SQL + ComNum.VBLF + "       a.OverAmt";
                SQL = SQL + ComNum.VBLF + "  FROM VIEW_BAS_ROOM A, ( ";
                SQL = SQL + ComNum.VBLF + "                         SELECT ROOMCODE, MAX(TDATE) MDATE";
                SQL = SQL + ComNum.VBLF + "                         FROM VIEW_BAS_ROOM ";
                SQL = SQL + ComNum.VBLF + "                         WHERE TDATE <=TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','yyyy-mm-dd') ";
                SQL = SQL + ComNum.VBLF + "                         GROUP BY ROOMCODE";
                SQL = SQL + ComNum.VBLF + "                        ) B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ROOMCODE  = B.ROOMCODE ";
                SQL = SQL + ComNum.VBLF + "   AND A.TDATE = B.MDATE ";
                SQL = SQL + ComNum.VBLF + "   AND A.Tbed <> 0";
                SQL = SQL + ComNum.VBLF + " ORDER BY  A.RoomClass, A.ILBANAmt desc, A.RoomCode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    double nSSOverAmt = 0;
                    strRoomCode = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strRoomClass = dt.Rows[i]["RoomClass"].ToString().Trim();
                    nWardAmt = VB.Val(dt.Rows[i]["ILBANAmt"].ToString().Trim());
                    if ((strRoomClass == "A" || strRoomClass == "B" || strRoomClass == "C") && string.Compare(dtpDate.Text, "2019-07-01") >= 0)
                    {
                        nSSOverAmt = 60000;
                    }
                   // nOverAmt = VB.Val(dt.Rows[i]["OverAmt"]+ nSSOverAmt.ToString().Trim()) ;
                    nOverAmt = VB.Val(dt.Rows[i]["OverAmt"].ToString().Trim());
                    nOverAmt = nOverAmt + nSSOverAmt;
                    if (strRoomClass != strSwClass || nWardAmt != nSwWardAmt || nOverAmt != nSwOverAmt)
                    {
                        if (nHoNo != 0)
                        {
                            strText = strHoSil[0] + strHoSil[1] + strHoSil[2] + strHoSil[3];
                            strText += strHoSil[4] + strHoSil[5] + strHoSil[6];
                            strText += strHoSil[7];

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text += " " + VB.Mid(strText, 1, strText.Trim().Length - 1);
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ssView_Sheet1.GetRowHeight(ssView_Sheet1.RowCount - 1) + 15);
                            for (k = 0; k < 8; k++)
                            {
                                strHoSil[k] = "";
                            }
                            nHoNo = 0;
                            strHoSil[0] = strRoomCode + ",";
                        }

                        strSwCode = strRoomCode;
                        strSwClass = strRoomClass;
                        nSwWardAmt = nWardAmt;
                        nSwOverAmt = nOverAmt;

                        ssView_Sheet1.RowCount += 1;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strSwClass;

                        switch (strSwClass)
                        {
                            case "A":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "VIP(A)";
                                break;
                            case "B":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "1인실(A)";
                                break;
                            case "C":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "1인실(B)";
                                break;
                            case "D":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "특B(B)";
                                break;
                            case "E":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "특C(A)";
                                break;
                            case "F":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "특C(B)";
                                break;
                            case "G":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "2인실(A)";
                                break;
                            case "H":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "2인실(B)";
                                break;
                            case "I":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "3인실(A)";
                                break;
                            case "J":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "3인실(B)";
                                break;
                            case "K":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "4인실(A)";
                                break;
                            case "L":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "4인실(B)";
                                break;
                            case "M":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "5인실";
                                break;
                            case "N":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "5인실(B)";
                                break;
                            case "O":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "6인실";
                                break;
                            case "P":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "7인실";
                                break;
                            case "Q":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "8인실";
                                break;
                            case "R":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "9인실";
                                break;
                            case "T":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "격리실";
                                break;
                            case "U":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "집중치료실";
                                break;
                            case "V":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "분만실";
                                break;
                            case "W":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "신생아실";
                                break;
                            case "X":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "인큐베타";
                                break;
                        }

                        switch (strRoomCode)
                        {
                            case "641":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "신생아집중치료실";
                                break;
                            case "233":
                            case "234":
                            case "320":
                            case "321":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "집중치료실";
                                break;
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = nSwWardAmt.ToString("###,##0 ");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nSwOverAmt.ToString("###,##0 ");
                        nHoNo = 1;
                        strHoSil[0] = strRoomCode + ",";
                    }
                    else
                    {
                        nHoNo += 1;
                        if (nHoNo == 8)
                        {
                            strHoSil[nHoNo - 1] = strRoomCode;
                            strText = strHoSil[0] + strHoSil[1] + strHoSil[2] + strHoSil[3];
                            strText += strHoSil[4] + strHoSil[5] + strHoSil[6];
                            strText += strHoSil[7];

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text += " " + strText.Trim() + ",";
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ssView_Sheet1.GetRowHeight(ssView_Sheet1.RowCount - 1) + 18);
                            for (k = 0; k < 8; k++)
                            {
                                strHoSil[k] = "";
                            }
                            nHoNo = 0;
                        }
                        else
                        {
                            strHoSil[nHoNo - 1] = strRoomCode + ",";
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                if (nHoNo != 0)
                {
                    strText = strHoSil[0] + strHoSil[1] + strHoSil[2] + strHoSil[3];
                    strText += strHoSil[4] + strHoSil[5] + strHoSil[6];
                    strText += strHoSil[7];

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = " " + VB.Mid(strText, 1, strText.Trim().Length - 1);
                    for (k = 0; k < 8; k++)
                    {
                        strHoSil[k] = "";
                    }
                    nHoNo = 0;
                }

                if (chkJob.Checked == true)
                {
                    strFDate = VB.Left(cboYear.Text, 4) + "-01-01";
                    strTDate = VB.Left(cboYear.Text, 4) + "-12-31";

                    nIlsu = (int)VB.DateDiff("D", Convert.ToDateTime(strFDate), Convert.ToDateTime(strTDate)) + 1;

                    for (i = 0; i < ssView_Sheet1.RowCount; i++)
                    {
                        strRoom = ssView_Sheet1.Cells[i, 2].Text.Trim();
                        if (VB.Right(strRoom.Trim(), 1) == ",")
                        {
                            strRoom = VB.Left(strRoom, strRoom.Length - 1);
                        }
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT COUNT(PANO) CNT FROM " + ComNum.DB_PMPA + "TONG_PATIENT ";
                        SQL = SQL + ComNum.VBLF + " WHERE JOBDATE >= TO_DATE('" + strFDate + "', 'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND JOBDATE <= TO_DATE('" + strTDate + "', 'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND ROOMCODE IN (" + strRoom + ") ";
                        SQL = SQL + ComNum.VBLF + "   AND PACLASS  = '3' ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        ssView_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[0]["CNT"].ToString().Trim()).ToString("###,##0 ");
                        ssView_Sheet1.Cells[i, 6].Text = (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) / nIlsu).ToString("#,##0.## ");
                        dt.Dispose();
                        dt = null;
                    }
                }

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void frmPmpaViewWardListByGrade_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDateYY(clsDB.DbCon, cboYear, 5, "2");
            cboYear.SelectedIndex = 0;

            Read_Bas_Room();
        }
    }
}
