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
    /// File Name       : frmPmpaViewJanggi.cs
    /// Description     : 담당과장별 장기입원환자 통계
    /// Author          : 박창욱
    /// Create Date     : 2017-09-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iviewa\IVIEWAG.FRM(FrmJanggi.frm) >> frmPmpaViewJanggi.cs 폼이름 재정의" />
    public partial class frmPmpaViewJanggi : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        public frmPmpaViewJanggi()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
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

            strTitle = "담당과장별 입원환자 통계";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("재원일수: " + txtIlsu.Text + " 일 이상", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false, (float)0.85);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
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
            int nCount = 0;
            int nIlsu = 0;
            int nBohum = 0;
            int nJabo = 0;
            int nSanje = 0;
            int nBoho1 = 0;
            int nBoho2 = 0;
            int nIlban = 0;
            int nGita = 0;
            string strOldDrcode = "";
            string strNewDrcode = "";
            string strOldDept = "";
            string strNewDept = "";
            string strOldDrname = "";

            //1의보,2자보,3산재,4보호1,5보호2,6일반,7기타,8총원,9재원일수
            //1소계:2:총계
            int[,] nData = new int[3, 10];


            nIlsu = 0;
            nBohum = 0;
            nJabo = 0;
            nSanje = 0;
            nBoho1 = 0;
            nBoho2 = 0;
            nIlban = 0;
            nGita = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;

            nIlsu = (int)VB.Val(txtIlsu.Text.Trim());

            for (i = 0; i < 3; i++)
            {
                for (k = 0; k < 10; k++)
                {
                    nData[i, k] = 0;
                }
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.deptcode, a.drcode, b.drname,";
                SQL = SQL + ComNum.VBLF + "        a.bi, A.ilsu";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "bas_doctor b,";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "bas_clinicdept c  ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.GBSTS IN ('0','2')                             ";   //--재원
                SQL = SQL + ComNum.VBLF + "    AND a.OUTDATE IS NULL                                ";   //--구분병경제외
                if (nIlsu != 0)
                {
                    SQL = SQL + "    AND a.ilsu >= '" + nIlsu + "'   ";     //'100'   --입원일수
                }
                SQL = SQL + ComNum.VBLF + "    AND a.pano <> '81000004'                           ";
                SQL = SQL + ComNum.VBLF + "    AND a.drcode   = b.drcode(+)                       ";
                SQL = SQL + ComNum.VBLF + "    AND a.deptcode = c.deptcode(+)                     ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY c.printranking,b.printranking               ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                strOldDrcode = dt.Rows[0]["drcode"].ToString().Trim();
                strOldDept = dt.Rows[0]["deptcode"].ToString().Trim();
                strOldDrname = dt.Rows[0]["drname"].ToString().Trim();

                nCount = 1;
                nIlsu = 0;

                ssView_Sheet1.Cells[nCount - 1, 0].Text = dt.Rows[0]["deptcode"].ToString().Trim();

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    #region Display_Data



                    strNewDrcode = dt.Rows[i]["Drcode"].ToString().Trim();
                    if (strOldDrcode != strNewDrcode)
                    {
                        strOldDrcode = strNewDrcode;
                        ssView_Sheet1.Cells[nCount - 1, 1].Text = strOldDrname;
                        ssView_Sheet1.Cells[nCount - 1, 2].Text = nBohum.ToString();
                        ssView_Sheet1.Cells[nCount - 1, 3].Text = nJabo.ToString();
                        ssView_Sheet1.Cells[nCount - 1, 4].Text = nSanje.ToString();
                        ssView_Sheet1.Cells[nCount - 1, 5].Text = nBoho1.ToString();
                        ssView_Sheet1.Cells[nCount - 1, 6].Text = nBoho2.ToString();
                        ssView_Sheet1.Cells[nCount - 1, 7].Text = nIlban.ToString();
                        ssView_Sheet1.Cells[nCount - 1, 8].Text = nGita.ToString();
                        ssView_Sheet1.Cells[nCount - 1, 9].Text = (nBohum + nJabo + nSanje + nBoho1 + nBoho2 + nIlban + nGita).ToString();
                        if (nBohum + nJabo + nSanje + nBoho1 + nBoho2 + nIlban + nGita != 0)
                        {
                            ssView_Sheet1.Cells[nCount - 1, 12].Text = ((int)(nIlsu / (nBohum + nJabo + nSanje + nBoho1 + nBoho2 + nIlban + nGita))).ToString();
                        }

                        nIlsu = 0;
                        nBohum = 0;
                        nJabo = 0;
                        nSanje = 0;
                        nBoho1 = 0;
                        nBoho2 = 0;
                        nIlban = 0;
                        nGita = 0;
                        strOldDrname = dt.Rows[i]["Drname"].ToString().Trim();
                        strNewDept = dt.Rows[i]["deptcode"].ToString().Trim();

                        //소계
                        if (strOldDept != strNewDept)
                        {
                            nCount += 1;
                            strOldDept = strNewDept;
                            ssView_Sheet1.Cells[nCount - 1, 0].Text = "소계";
                            ssView_Sheet1.Cells[nCount - 1, 2].Text = nData[1, 1].ToString();
                            ssView_Sheet1.Cells[nCount - 1, 3].Text = nData[1, 2].ToString();
                            ssView_Sheet1.Cells[nCount - 1, 4].Text = nData[1, 3].ToString();
                            ssView_Sheet1.Cells[nCount - 1, 5].Text = nData[1, 4].ToString();
                            ssView_Sheet1.Cells[nCount - 1, 6].Text = nData[1, 5].ToString();
                            ssView_Sheet1.Cells[nCount - 1, 7].Text = nData[1, 6].ToString();
                            ssView_Sheet1.Cells[nCount - 1, 8].Text = nData[1, 7].ToString();
                            ssView_Sheet1.Cells[nCount - 1, 9].Text = nData[1, 8].ToString();

                            if (nData[1, 9] != 0)
                            {
                                ssView_Sheet1.Cells[nCount - 1, 12].Text = ((int)(nData[1, 9] / nData[1, 8])).ToString();
                            }

                            for (k = 0; k < 10; k++)
                            {
                                nData[1, k] = 0;
                            }

                            ssView_Sheet1.Cells[nCount, 0].Text = dt.Rows[i]["deptcode"].ToString().Trim();
                        }
                        nCount += 1;
                    }

                    nIlsu += (int)VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim());
                    nData[1, 9] += (int)VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim());
                    nData[2, 9] += (int)VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim());
                    switch (dt.Rows[i]["Bi"].ToString().Trim())
                    {
                        case "11":
                        case "12":
                        case "13":
                            nBohum += 1;
                            nData[1, 1] += 1;
                            nData[2, 1] += 1; //의보
                            break;
                        case "21":
                            nBoho1 += 1;
                            nData[1, 4] += 1; // "보호1종"
                            nData[2, 4] += 1;
                            break;
                        case "22":
                        case "23":
                            nBoho2 += 1;
                            nData[1, 5] += 1; // "보호2종"
                            nData[2, 5] += 1;
                            break;
                        case "31":
                        case "32":
                        case "33":
                            nSanje += 1;
                            nData[1, 3] += 1; //"산재"
                            nData[2, 3] += 1;
                            break;
                        case "51":
                            nIlban += 1;
                            nData[1, 6] += 1; //"일반"
                            nData[2, 6] += 1;
                            break;
                        case "52":
                            nJabo += 1;
                            nData[1, 2] += 1; //"자보"
                            nData[2, 2] += 1;
                            break;
                        default:
                            nGita += 1;
                            nData[1, 7] += 1;
                            nData[2, 7] += 1; //"기타"
                            break;
                    }



                    #endregion

                    nData[1, 8] += 1;   //총원
                    nData[2, 8] += 1;
                }

                strOldDrcode = strNewDrcode;
                ssView_Sheet1.Cells[nCount - 1, 1].Text = strOldDrname;
                ssView_Sheet1.Cells[nCount - 1, 2].Text = nBohum.ToString();
                ssView_Sheet1.Cells[nCount - 1, 3].Text = nJabo.ToString();
                ssView_Sheet1.Cells[nCount - 1, 4].Text = nSanje.ToString();
                ssView_Sheet1.Cells[nCount - 1, 5].Text = nBoho1.ToString();
                ssView_Sheet1.Cells[nCount - 1, 6].Text = nBoho2.ToString();
                ssView_Sheet1.Cells[nCount - 1, 7].Text = nIlban.ToString();
                ssView_Sheet1.Cells[nCount - 1, 8].Text = nGita.ToString();
                ssView_Sheet1.Cells[nCount - 1, 9].Text = (nBohum + nJabo + nSanje + nBoho1 + nBoho2 + nIlban + nGita).ToString();
                if (nBohum + nJabo + nSanje + nBoho1 + nBoho2 + nIlban + nGita != 0)
                {
                    ssView_Sheet1.Cells[nCount - 1, 12].Text = ((int)(nIlsu / (nBohum + nJabo + nSanje + nBoho1 + nBoho2 + nIlban + nGita))).ToString();
                }

                nIlsu = 0;
                nBohum = 0;
                nJabo = 0;
                nSanje = 0;
                nBoho1 = 0;
                nBoho2 = 0;
                nIlban = 0;
                nGita = 0;
                strOldDrname = "";
                strNewDept = "";

                //소계
                if (strOldDept != strNewDept)
                {
                    nCount += 1;
                    if (ssView_Sheet1.RowCount < nCount)
                    {
                        ssView_Sheet1.RowCount = nCount + 2;
                    }

                    strOldDept = strNewDept;
                    ssView_Sheet1.Cells[nCount - 1, 0].Text = "소계";
                    ssView_Sheet1.Cells[nCount - 1, 2].Text = nData[1, 1].ToString();
                    ssView_Sheet1.Cells[nCount - 1, 3].Text = nData[1, 2].ToString();
                    ssView_Sheet1.Cells[nCount - 1, 4].Text = nData[1, 3].ToString();
                    ssView_Sheet1.Cells[nCount - 1, 5].Text = nData[1, 4].ToString();
                    ssView_Sheet1.Cells[nCount - 1, 6].Text = nData[1, 5].ToString();
                    ssView_Sheet1.Cells[nCount - 1, 7].Text = nData[1, 6].ToString();
                    ssView_Sheet1.Cells[nCount - 1, 8].Text = nData[1, 7].ToString();
                    ssView_Sheet1.Cells[nCount - 1, 9].Text = nData[1, 8].ToString();

                    if (nData[1, 9] != 0)
                    {
                        ssView_Sheet1.Cells[nCount - 1, 12].Text = ((int)(nData[1, 9] / nData[1, 8])).ToString();
                    }

                    for (k = 0; k < 10; k++)
                    {
                        nData[1, k] = 0;
                    }

                    ssView_Sheet1.Cells[nCount, 0].Text = "";
                }


                //총계
                if (ssView_Sheet1.RowCount < nCount)
                {
                    ssView_Sheet1.RowCount = nCount + 2;
                }
                nCount += 1;

                ssView_Sheet1.Cells[nCount - 1, 0].Text = "총계";
                ssView_Sheet1.Cells[nCount - 1, 2].Text = nData[2, 1].ToString();
                ssView_Sheet1.Cells[nCount - 1, 3].Text = nData[2, 2].ToString();
                ssView_Sheet1.Cells[nCount - 1, 4].Text = nData[2, 3].ToString();
                ssView_Sheet1.Cells[nCount - 1, 5].Text = nData[2, 4].ToString();
                ssView_Sheet1.Cells[nCount - 1, 6].Text = nData[2, 5].ToString();
                ssView_Sheet1.Cells[nCount - 1, 7].Text = nData[2, 6].ToString();
                ssView_Sheet1.Cells[nCount - 1, 8].Text = nData[2, 7].ToString();
                ssView_Sheet1.Cells[nCount - 1, 9].Text = nData[2, 8].ToString();

                if (nData[2, 9] != 0)
                {
                    ssView_Sheet1.Cells[nCount - 1, 12].Text = ((int)(nData[2, 9] / nData[2, 8])).ToString();
                }

                ssView_Sheet1.RowCount = nCount;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewJanggi_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            txtIlsu.Text = "100";
        }

        private void txtIlsu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }
    }
}
