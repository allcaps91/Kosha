using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmOrderDetailHistory
    /// File Name : frmOrderDetailHistory.cs
    /// Title or Description : 처방상세내역 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-02
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmOrderDetailHistory : Form
    {
        //부모폼으로 부터 받은 데이터를 할당하는 변수
        string GstrPANO = "";
        string GstrSName = "";
        string GstrActDate = "";
        string GstrIO = "";

        public frmOrderDetailHistory()
        {
            InitializeComponent();
        }

        public frmOrderDetailHistory(string PANO, string SName, string ActDate, string IO)
        {
            InitializeComponent();
            GstrPANO = PANO;
            GstrSName = SName;
            GstrActDate = ActDate;
            GstrIO = IO;
        }

        private void frmOrderDetailHistory_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            lblMsg.Text = "";
            if (Screen_Display() == false) return;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/c/f1" + "치과 처방 상세내역/n/n";
            strHead2 = "/ l / f2" + "등록번호: " + GstrPANO + "   환자명: " + GstrSName + "   진료일자: " + GstrActDate + " 구분:";

            if (GstrIO == "O")
            {
                strHead2 += "외래";
            }
            else
            {
                strHead2 += "입원";
            }
            strHead2 += VB.Space(65) + "인쇄:" + 
                ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + " " +
                ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            ss1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ss1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ss1_Sheet1.PrintInfo.Margin.Top = 10;
            ss1_Sheet1.PrintInfo.Margin.Bottom = 10;
            ss1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ss1_Sheet1.PrintInfo.ShowBorder = true;
            ss1_Sheet1.PrintInfo.ShowColor = true;
            ss1_Sheet1.PrintInfo.ShowGrid = true; 
            ss1_Sheet1.PrintInfo.ShowShadows = false;
            ss1_Sheet1.PrintInfo.UseMax = false;
            ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ss1_Sheet1.PrintInfo.Preview = true;
            ss1.PrintSheet(0);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool Screen_Display()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            int nX = 0;
            double nTotAmt1 = 0;
            double nTotAmt2 = 0;
            double nTotAmt3 = 0;
            string strFlag = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            lblMsg.Text = "등록번호: " + GstrPANO + "    환자명: " + GstrSName + "    진료일자: " + GstrActDate;
            if (GstrIO == "I") { lblMsg.Text += "   [입원]  "; }
            if (GstrIO == "O") { lblMsg.Text += "   [외래]  "; }

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, A.SUNEXT, A.QTY, ";
                SQL += ComNum.VBLF + "  A.NAL, A.GBSELF, A.BASEAMT, A.AMT1, A.BUN, B.SUNAMEK, ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE ";
                if (GstrIO == "O")
                {
                    SQL += ComNum.VBLF + "  FROM OPD_SLIP A, BAS_SUN B ";
                    SQL += ComNum.VBLF + " WHERE PANO = '" + GstrPANO + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + " FROM IPD_NEW_SLIP A, BAS_SUN B ";
                    SQL += ComNum.VBLF + " WHERE PANO = '" + GstrPANO + "' ";
                }
                SQL += ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + GstrActDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND DEPTCODE ='DT'";
                SQL += ComNum.VBLF + "   AND A.SUNEXT =B.SUNEXT";
                SQL += ComNum.VBLF + " ORDER BY ACTDATE DESC , SEQNO, BUN,NU, SUNEXT";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }
                ss1_Sheet1.Rows.Count = 0;
                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                strFlag = "";
                nTotAmt1 = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (strFlag != dt.Rows[i]["ACTDATE"].ToString())
                    {
                        strFlag = dt.Rows[i]["ACTDATE"].ToString();
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString();
                    }
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUNEXT"].ToString();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["QTY"].ToString();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["NAL"].ToString();
                    ss1_Sheet1.Cells[i, 5].Text = String.Format("{0:###,###,##0", dt.Rows[i]["BASEAMT"]);
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GBSELF"].ToString();
                    ss1_Sheet1.Cells[i, 7].Text = String.Format("{0:###,###,##0", dt.Rows[i]["AMT1"]);
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SUNAMEK"].ToString();
                    //nx = 1 로 만드는 BUN값
                    int[] BunValue1 =
                    {
                        1, 2, 4, 6, 7, 10, 16, 17, 18, 19, 24, 25, 28, 34, 22, 53, 54, 55, 56, 57, 58, 59, 60,
                        61, 62, 63, 41, 43, 44, 45, 46, 47, 48, 49, 50, 51, 38, 39, 65, 69, 75, 26, 27, 79, 72
                    };

                    //nx = 2 로 만드는 BUN값
                    int[] BunValue2 = { 3, 5, 7, 9, 77, 76, 78, 74, 11, 12, 13, 14, 15, 20, 21, 23, 37, 42, 70, 73, 71 };
                    //nx = 3 로 만드는 BUN값
                    int[] BunValue3 = { 40, 29, 36, 66, 82 };
                    foreach (int Val in BunValue1)
                    {
                        if (Int32.Parse(dt.Rows[i]["BUN"].ToString()) == BunValue1[Val])
                            nX = 1;
                    }
                    foreach (int Val in BunValue2)
                    {
                        if (Int32.Parse(dt.Rows[i]["BUN"].ToString()) == BunValue2[Val])
                            nX = 2;
                    }
                    foreach (int Val in BunValue3)
                    {
                        if (Int32.Parse(dt.Rows[i]["BUN"].ToString()) == BunValue3[Val])
                            nX = 3;
                    }

                    if (dt.Rows[i]["SUNAMEK"].ToString().Trim() == "C-DT")
                        nX = 3;

                    switch (nX)
                    {
                        case 1: ss1_Sheet1.Rows[i].BackColor = Color.FromArgb(200, 200, 255); break;
                        case 2: ss1_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 255, 230); break;
                        case 3: ss1_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 223, 223); break;
                    }

                    if (Int32.Parse(dt.Rows[i]["BUN"].ToString()) < 90)
                    {
                        if (nX == 1) nTotAmt1 += Convert.ToDouble(dt.Rows[i]["AMT1"]);
                        if (nX == 2) nTotAmt2 += Convert.ToDouble(dt.Rows[i]["AMT1"]);
                        if (nX == 3) nTotAmt3 += Convert.ToDouble(dt.Rows[i]["AMT1"]);
                    }
                    nX = 0;
                }
                dt.Dispose();
                dt = null;

                ss1_Sheet1.Rows.Count += 1;
                ss1_Sheet1.Cells[ss1_Sheet1.Rows.Count - 1, 1].Text = "배분수익금";
                ss1_Sheet1.Cells[ss1_Sheet1.Rows.Count - 1, 7].Text = string.Format("{0:##,###,###,##0 }", nTotAmt1);
                ss1_Sheet1.Rows[ss1_Sheet1.Rows.Count - 1].BackColor = Color.FromArgb(200, 200, 255);
                ss1_Sheet1.Rows.Count += 1;
                ss1_Sheet1.Cells[ss1_Sheet1.Rows.Count - 1, 1].Text = "병원수익금";
                ss1_Sheet1.Cells[ss1_Sheet1.Rows.Count - 1, 7].Text = string.Format("{0:##,###,###,##0 }", nTotAmt2);
                ss1_Sheet1.Rows[ss1_Sheet1.Rows.Count - 1].BackColor = Color.FromArgb(255, 255, 230);
                ss1_Sheet1.Rows.Count += 1;
                ss1_Sheet1.Cells[ss1_Sheet1.Rows.Count - 1, 1].Text = "치과수익금";
                ss1_Sheet1.Cells[ss1_Sheet1.Rows.Count - 1, 7].Text = string.Format("{0:##,###,###,##0 }", nTotAmt3);
                ss1_Sheet1.Rows[ss1_Sheet1.Rows.Count - 1].BackColor = Color.FromArgb(255, 223, 223);
                ss1_Sheet1.Rows.Count += 1;
                ss1_Sheet1.Cells[ss1_Sheet1.Rows.Count - 1, 1].Text = "합 계";
                ss1_Sheet1.Cells[ss1_Sheet1.Rows.Count - 1, 7].Text = string.Format("{0:##,###,###,##0 }", nTotAmt1 + nTotAmt2 + nTotAmt3);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }
    }
}
