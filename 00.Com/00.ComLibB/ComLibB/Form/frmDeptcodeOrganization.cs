using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using System.Drawing;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmDeptcodeOrganization
    /// File Name : frmDeptcodeOrganization.cs
    /// Title or Description : 부서코드별 조직도 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-01
    /// <history> 
    /// 2017-06-30 스프레드 출력, 인쇄버튼 기능 추가 및 수정
    /// </history>
    /// </summary>
    public partial class frmDeptcodeOrganization : Form
    {
        public frmDeptcodeOrganization()
        {
            InitializeComponent();
        }

        private void frmDeptcodeOrganization_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            Buse_Formation_Display(ss1_Sheet1);
        }

        private void Buse_Formation_Display(FarPoint.Win.Spread.SheetView ss1)
        {
            if(ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;

            string strOld = "";
            string strNew = "";
            string strBuName = "";
            string strSql = "";
            string SqlErr = "";
            DataTable dt = null;

            progressBar1.Value = 0;
            ss1.RowCount = 0;
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                strSql = "";
                strSql = strSql + " SELECT BuCode,Name FROM KOSMOS_PMPA.BAS_BUSE ";
                strSql = strSql + ComNum.VBLF + " WHERE DelDate IS NULL ";
                strSql = strSql + ComNum.VBLF + " ORDER BY BuCode,Name ";

                SqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    progressBar1.Value = (i + 1) / dt.Rows.Count * 100;

                    strNew = dt.Rows[i]["BuCode"].ToString().Trim();
                    strBuName = dt.Rows[i]["Name"].ToString().Trim();

                    if ( VB.Left(strOld, 2) != VB.Left(strNew, 2))
                    {
                        ss1_Sheet1.Cells[i, 0].Text = " " + Read_Bcode_Name("BAS_부서코드대분류", VB.Left(strNew, 2));
                    }

                    if ( VB.Left(strNew, 2) == "10")
                    {
                        ss1_Sheet1.Cells[i, 1].Text = " " + strBuName;
                        ss1_Sheet1.Cells[i, 3].Text = strNew;
                    }
                    else
                    {
                        if (VB.Left(strOld, 4) != VB.Left(strNew, 4))
                        {
                            if (strNew == "011101")
                            {
                                ss1_Sheet1.Cells[i, 1].Text = " 전문의";
                                ss1_Sheet1.Cells[i, 3].Text = strNew;
                            }
                            else
                            {
                                ss1_Sheet1.Cells[i, 1].Text = " " + strBuName;
                                ss1_Sheet1.Cells[i, 3].Text = strNew;
                            }
                        }
                        if (VB.Right(strNew, 2) != "00")
                        {
                            ss1_Sheet1.Cells[i, 2].Text = " " + strBuName;
                            ss1_Sheet1.Cells[i, 3].Text = strNew;
                        }
                    }
                    strOld = strNew;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, strSql, clsDB.DbCon); //에러로그 저장
            }
        }

        //TODO:READ_BCODE_Name 공통함수 임시구현
        string Read_Bcode_Name(string gubun, string code)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strVat = "";


            SQL = "";
            SQL = "SELECT Name FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + " WHERE Gubun='" + gubun + "' ";
            SQL = SQL + ComNum.VBLF + "   AND Code='" + code.Trim() + "' ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                strVat = dt.Rows[0]["Name"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            return strVat;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "각 병동 병상 현황";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}

