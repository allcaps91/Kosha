using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\nurse\nrtong\nrtong.vbp\nrtong13.frm >> frmOpdiljiPrint.cs 폼이름 재정의" />

    public partial class frmOpdiljiPrint : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        int nSSRowCount = 0;
        //int ERow = 0;
        //int ECol = 0;

        public frmOpdiljiPrint()
        {
            InitializeComponent();
        }

        private void Clear()
        {
            SS1_Sheet1.Cells[2, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = true;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;

            Clear();
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
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            //int j = 0;
            int nCount = 0;
            int nSinInwon = 0;
            int nGuInwon = 0;
            int nIlban = 0;
            int nIpwon = 0;
            string strOldDept = "";
            string strNowDate = "";
            string strAnName = "";
            string strAnName1 = "";
            string strAnName2 = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strdept = "";

            int intCount = 0;

            btnSearch.Enabled = false;

            strNowDate = dtpDate.Text;

            FarPoint.Win.ComplexBorder borderWhite = new FarPoint.Win.ComplexBorder(	//ㄱ
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border1 = new FarPoint.Win.ComplexBorder(	//ㄱ
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border2 = new FarPoint.Win.ComplexBorder(	//ㄱ
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border3 = new FarPoint.Win.ComplexBorder(	//ㄱ
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder border4 = new FarPoint.Win.ComplexBorder(	//ㄱ
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),  //BOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            Cursor.Current = Cursors.WaitCursor;

            //try
            //{
            SQL = "";
            SQL = "SELECT A.DEPT, B.DRNAME, A.ANSABUN1, A.ANSABUN2, A.SININWON, A.GUINWON,";
            SQL = SQL + ComNum.VBLF + " A.ILBAN, A.IPWON, A.ROWID , A.SININWON+A.GUINWON+A.ILBAN  SUMINWON";
            SQL = SQL + ComNum.VBLF + " FROM NUR_OPDILJI A, BAS_DOCTOR B ,BAS_CLINICDEPT C";
            SQL = SQL + ComNum.VBLF + " WHERE ACTDATE =TO_DATE('" + strNowDate + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = B.DRCODE(+)";
            SQL = SQL + ComNum.VBLF + "   AND B.DRDEPT1 = C.DEPTCODE(+)";
            SQL = SQL + ComNum.VBLF + " ORDER BY C.PRINTRANKING,B.PRINTRANKING";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return;
            }

            //SS1_Sheet1.RowCount =  

            strOldDept = "m";
            nCount = dt.Rows.Count;
            SS1_Sheet1.RowCount = nCount + 4;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strOldDept != dt.Rows[i]["DEPT"].ToString().Trim())
                {
                    //Row = i +3 , Col = 1

                    switch (dt.Rows[i]["Dept"].ToString().Trim())
                    {
                        case "MD":
                            strdept = "내과";
                            break;
                        case "GS":
                            strdept = "외과";
                            break;
                        case "OG":
                            strdept = "산부인과";//     '~j
                            break;
                        case "PD":
                            strdept = "소아과";
                            break;
                        case "OS":
                            strdept = "정형외과";
                            break;
                        case "NS":
                            strdept = "신경외과";
                            break;
                        case "CS":
                            strdept = "흉부외과";
                            break;
                        case "NE":
                            strdept = "신경과";
                            break;
                        case "NP":
                            strdept = "정신과";
                            break;
                        case "EN":
                            strdept = "이비인후과";
                            break;
                        case "OT":
                            strdept = "안과";
                            break;
                        case "UR":
                            strdept = "비뇨기과";
                            break;
                        case "DM":
                            strdept = "피부과";
                            break;
                        case "DT":
                            strdept = "치과";//         '~j
                            break;
                        case "PC":
                            strdept = "통증치료과";//   '~j
                            break;
                        case "JU":
                            strdept = "주사실";
                            break;
                        case "SI":
                            strdept = "심전도실";
                            break;
                        case "ED":
                            strdept = "내시경";
                            break;
                        case "RM":
                            strdept = "재활의학과";
                            break;
                    }

                    strOldDept = dt.Rows[i]["DEPT"].ToString().Trim();
                    SS1_Sheet1.Cells[i + 2, 0].Text = strdept;

                    if (i != 0)
                    {
                        SS1_Sheet1.Cells[(i + 2) - intCount, 0].RowSpan = intCount;
                        intCount = 0;
                    }
                }

                intCount++;

                SS1_Sheet1.Cells[i + 2, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();   //의사성명
                SS1_Sheet1.Cells[i + 2, 3].Text = dt.Rows[i]["SinInwon"].ToString().Trim();   //신환자수
                SS1_Sheet1.Cells[i + 2, 4].Text = dt.Rows[i]["Guinwon"].ToString().Trim();   //구환자수
                SS1_Sheet1.Cells[i + 2, 5].Text = dt.Rows[i]["Suminwon"].ToString().Trim();   //일반환자수
                SS1_Sheet1.Cells[i + 2, 6].Text = dt.Rows[i]["Ipwon"].ToString().Trim();   //합계
                SS1_Sheet1.Cells[i + 2, 7].Text = dt.Rows[i]["Ipwon"].ToString().Trim();   //입원환자수

                nSinInwon = nSinInwon + (int)VB.Val(dt.Rows[i]["SinInwon"].ToString().Trim());
                nGuInwon = nGuInwon + (int)VB.Val(dt.Rows[i]["Guinwon"].ToString().Trim());
                nIlban = nIlban + (int)VB.Val(dt.Rows[i]["ilban"].ToString().Trim());
                nIpwon = nIpwon + (int)VB.Val(dt.Rows[i]["Ipwon"].ToString().Trim());

                //'strAnName = Trim(str(RS!Ansabun1))
                strAnName = dt.Rows[i]["Ansabun1"].ToString().Trim();

                #region Nurse_name_display
                strAnName = Nurse_name_display(strAnName);


                #endregion

                strAnName1 = strAnName;
                strAnName = "";

                strAnName = dt.Rows[i]["Ansabun2"].ToString().Trim();

                #region Nurse_name_display
                strAnName = Nurse_name_display(strAnName);
                #endregion

                strAnName2 = strAnName;
                strAnName = "";

                SS1_Sheet1.Cells[i + 2, 2].Text = strAnName1.Trim() + " " + strAnName2.Trim();  //'간호사

                strAnName = "";
                strAnName1 = "";
                strAnName2 = "";


            }

            SS1_Sheet1.Cells[(i + 2) - intCount, 0].RowSpan = intCount;

            SS1_Sheet1.Cells[nCount + 3, 0].Text = "합 계";
            SS1_Sheet1.Cells[nCount + 3, 3].Text = nSinInwon.ToString();
            SS1_Sheet1.Cells[nCount + 3, 4].Text = nGuInwon.ToString();
            SS1_Sheet1.Cells[nCount + 3, 5].Text = nIlban.ToString();
            SS1_Sheet1.Cells[nCount + 3, 6].Text = (nSinInwon + nGuInwon + nIlban).ToString();
            SS1_Sheet1.Cells[nCount + 3, 7].Text = nIpwon.ToString();

            nSSRowCount = nCount + 4;

            //SS1_Sheet1.Cells[nCount + 3, 0].BackColor = System.Drawing.Color.FromArgb(0, 0, 0);
            //SS1_Sheet1.Cells[2, 0, nCount + 3, 0].BackColor = System.Drawing.Color.FromArgb(0, 0, 0);

            SS1_Sheet1.Cells[0, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = borderWhite;
            SS1_Sheet1.Cells[0, 0, SS1_Sheet1.RowCount - 2, SS1_Sheet1.ColumnCount - 2].Border = border1;
            SS1_Sheet1.Cells[0, SS1_Sheet1.ColumnCount - 1, SS1_Sheet1.RowCount - 2, SS1_Sheet1.ColumnCount - 1].Border = border2;
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 2].Border = border3;
            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = border4;


            btnCancel.Enabled = true;
            btnPrint.Enabled = true;

            dt.Dispose();
            dt = null;
            //}
            //catch (Exception ex)
            //{
            //    if (dt != null)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        Cursor.Current = Cursors.Default;
            //    }

            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //}

        }

        private string Nurse_name_display(string strAnName)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            string strVar = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (VB.Len(strAnName) != 0)
                {
                    SQL = "";
                    SQL = "SELECT KORNAME FROM KOSMOS_ADM.INSA_MST";
                    SQL = SQL + ComNum.VBLF + " WHERE SABUN IN ('" + strAnName.PadLeft(5, '0') + "')";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return strVar;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "SELECT NAME FROM NUR_CODE";
                        SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '8'";
                        SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strAnName + "'";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return strVar;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strVar = dt1.Rows[0]["Name"].ToString().Trim();
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                    else
                    {
                        strVar = dt.Rows[0]["Korname"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                    return strVar;
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
                return strVar;
            }
            return strVar;
        }

        private void frmOpdiljiPrint_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpDate.Value = Convert.ToDateTime(strDTP);
            btnPrint.Enabled = false;
            Clear();
        }
    }
}
