using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewGajeongTong.cs
    /// Description     : 가정간호 진료명단
    /// Author          : 안정수
    /// Create Date     : 2017-08-18
    /// Update History  : 2017-10-25
    /// <history>      
    /// 본 소스에서는 데이터가 존재하지 않음... 사용여부 및 테스트 필요
    /// d:\psmh\OPD\oviewa\OVIEWA17.FRM(FrmGajengTong) => frmPmpaViewGajeongTong.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA17.FRM(FrmGajengTong)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewGajeongTong : Form
    {
        public frmPmpaViewGajeongTong()
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

            //ssList_Sheet1.Rows.Count = 0;           

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            optBi0.Checked = true;
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            dtpFDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-10).ToShortDateString();
            dtpTDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-1).ToShortDateString();
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ssList_Sheet1.Rows.Count == 0)
            {
                return;
            }

            btnView.Enabled = false;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            strTitle = dtpFDate.Text + "부터 " + dtpTDate.Text + "까지 가정간호 진료명단";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 100, 20);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion

            btnView.Enabled = true;
        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;
            int nIlsu = 0;

            string strNewData = "";
            string strOldData = "";

            double nTamt = 0;
            double nJAmt = 0;
            double nBAmt = 0;
            double nTTAmt = 0;
            double nTJAmt = 0;
            double nTBAmt = 0;
            double nTotCnt = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            btnView.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            //누적할 배열을 Clear
            nTTAmt = 0;
            nTJAmt = 0;
            nTBAmt = 0;
            nTotCnt = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                                ";
            SQL += ComNum.VBLF + "  TO_CHAR(M.Actdate, 'YYYY-MM-DD') ActDate, M.Pano,                                                                   ";
            SQL += ComNum.VBLF + "  M.DeptCode, M.Bi, SNAME, DeptNameK,                                                                                 ";
            SQL += ComNum.VBLF + "  SUM(DECODE(O.Bun,'92', O.Amt1+ O.Amt2,'96', O.Amt1+O.Amt2,0)) TAmt,                                                 ";
            SQL += ComNum.VBLF + "  SUM(DECODE(O.Bun,'98',O.Amt1+O.Amt2,0)) JAmt,                                                                       ";
            SQL += ComNum.VBLF + "  SUM(DECODE(O.Bun,'99',O.Amt1+O.Amt2,0)) BAmt                                                                        ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_Master M, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C, " + ComNum.DB_PMPA + "OPD_SLIP O     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                             ";
            SQL += ComNum.VBLF + "      AND M.ActDate >= TO_DATE('" + dtpFDate.Text + "', 'yyyy-mm-dd')                                                 ";
            SQL += ComNum.VBLF + "      AND M.ActDate <= TO_DATE('" + dtpTDate.Text + "', 'yyyy-mm-dd')                                                 ";
            SQL += ComNum.VBLF + "      AND M.PANO     = O.PANO                                                                                         ";
            SQL += ComNum.VBLF + "      AND M.ACTDATE  = O.ACTDATE(+)                                                                                   ";
            SQL += ComNum.VBLF + "      AND M.DEPTCODE = C.DEPTCODE                                                                                     ";
            SQL += ComNum.VBLF + "      AND M.BI       = O.BI(+)                                                                                        ";
            SQL += ComNum.VBLF + "      AND M.DEPTCODE = O.DEPTCODE(+)                                                                                  ";
            //가정간호
            SQL += ComNum.VBLF + "      AND M.JIN      = '9'                                                                                            ";

            if (optBi1.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND M.BI IN ('21','22','23')                                                                                ";
            }
            else if (optBi2.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND M.BI IN ('11','12','13')                                                                                ";
            }
            SQL += ComNum.VBLF + "GROUP BY TO_CHAR(M.Actdate, 'YYYY-MM-DD'), M.Pano,                                                                    ";
            SQL += ComNum.VBLF + "         M.DeptCode, M.Bi, SNAME, DeptNameK                                                                           ";
            SQL += ComNum.VBLF + "ORDER BY TO_CHAR(M.Actdate, 'YYYY-MM-DD'), M.Pano, M.DeptCode, M.Bi                                                   ";

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
                    nRow = 0;
                    strOldData = "";
                    nTamt = 0;
                    nJAmt = 0;
                    nBAmt = 0;

                    nREAD = dt.Rows.Count;

                    for (i = 0; i < nREAD; i++)
                    {
                        nRow += 1;

                        if (nRow > ssList_Sheet1.Rows.Count)
                        {
                            ssList_Sheet1.Rows.Count = nRow;
                        }

                        strNewData = dt.Rows[i]["ActDate"].ToString().Trim();

                        if (strNewData != strOldData)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                            strOldData = strNewData;
                        }

                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();

                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Bi"].ToString().Trim();

                        ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptNameK"].ToString().Trim();

                        nJAmt = VB.Val(dt.Rows[i]["JAmt"].ToString().Trim());
                        nBAmt = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim());
                        nTamt = VB.Val(dt.Rows[i]["TAmt"].ToString().Trim());

                        ssList_Sheet1.Cells[nRow - 1, 5].Text = nJAmt.ToString("###,###,###,##0");
                        ssList_Sheet1.Cells[nRow - 1, 6].Text = nBAmt.ToString("###,###,###,##0");
                        ssList_Sheet1.Cells[nRow - 1, 7].Text = nJAmt.ToString("###,###,###,##0");

                        //합계에 누적
                        nTotCnt += 1;
                        nTTAmt += nTamt;
                        nTJAmt += nJAmt;
                        nTBAmt += nBAmt;
                    }

                    //합계를 Display
                    nRow += 1;
                    ssList_Sheet1.Rows.Count = nRow;

                    ssList_Sheet1.Cells[nRow - 1, 2].Text = "** 합계 **";
                    ssList_Sheet1.Cells[nRow - 1, 4].Text = String.Format("{0:#,###}", nTotCnt) + "(건)";
                    ssList_Sheet1.Cells[nRow - 1, 5].Text = String.Format("{0:#,###}", nTTAmt);
                    ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:#,###}", nTBAmt);
                    ssList_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:#,###}", nTJAmt);
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            ssList.Enabled = true;

            dt.Dispose();
            dt = null;

            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;
        }

        void dtpFDate_ValueChanged(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;
        }

        void dtpTDate_ValueChanged(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;
        }
    }
}
