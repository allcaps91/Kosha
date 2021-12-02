using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaViewJepsuFailRemark : Form
    {
        public frmPmpaViewJepsuFailRemark()
        {
            InitializeComponent();
            setEvent();
        }
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
        }
        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등        

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
              
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                
                ePrint();
            }
        }
        void eGetData()
        {
            int i = 0;
            int nRow = 0;

            double nRamt = 0;
            double nTRamt = 0;
            double nCAmt = 0;
            double nTCamt = 0;

            string strSDate = "";
            string strTDate = "";
            string strPart = "";
            string strSabun = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strSDate = dtpDate.Text;
            strTDate = dtpDateTo.Text;


            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  PANO,SNAME ,DEPTCODE,bi,TO_CHAR(BDATE,'YYYY-MM-DD')BDATE , REMARK                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER_DEL                                                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')                              ";
            SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                              ";
            SQL += ComNum.VBLF + "      AND REMARK IS NOT NULL                                                              ";
           
            SQL += ComNum.VBLF + "ORDER BY BDATE                                                                            ";


            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                   

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //   PANO,SNAME ,DEPTCODE,bi,TO_CHAR(BDATE, 'YYYY-MM-DD')BDATE , REMARK

                        nRow += 1;
                        ssList_Sheet1.Rows.Count = nRow;
                        ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["bi"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["REMARK"].ToString().Trim();


                    }



                }


               
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

        }
        private void ssList_PrintHeaderFooterArea(object sender, PrintHeaderFooterAreaEventArgs e)
        {
            Pen cPen = new Pen(Color.Black);
            cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            cPen.Width = 3;
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Far;

            if (e.IsHeader == true)
            {
                #region 칸 그리기
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 550, 90, 220, 70);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 550, 90, 30, 70);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 580, 115, 190, 45);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 640, 90, 65, 70);



                #endregion

                #region 칸안에 글
                e.Graphics.DrawString("결", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 575, 93, drawFormat);
                e.Graphics.DrawString("재", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 575, 135, drawFormat);
                e.Graphics.DrawString("담  당", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 637, 93, drawFormat);
                e.Graphics.DrawString("계  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 700, 93, drawFormat);
                e.Graphics.DrawString("팀  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 760, 93, drawFormat);
                #endregion


                #region 작성자 
                drawFormat.Alignment = StringAlignment.Far;
                e.Graphics.DrawString("작성자 : " + clsType.User.UserName, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 185, 103, drawFormat);
                e.Graphics.DrawString("출력시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 275, 123, drawFormat);
                e.Graphics.DrawString("Page : " + e.PageNumber, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 135, 143, drawFormat);
                #endregion
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "(" + dtpDate.Text + " ~ "+ dtpDateTo.Text  + ") 접수취소자 사유 집계표";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            btnPrint.Enabled = false;

            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;



            #endregion

            strHeader = SPR.setSpdPrint_String(strTitle + "\r\n\r\n\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(80, 10, 50, 10, 30, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion

            btnPrint.Enabled = true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
    }

}
