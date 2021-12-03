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
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewSinBusePrint.cs
    /// Description     : 신환추천부서별리스트
    /// Author          : 안정수
    /// Create Date     : 2017-09-01
    /// Update History  : 2017-11-02
    /// <history>       
    /// 2014년 1월 이후 데이터 없음 사용여부 확인 필요
    /// d:\psmh\OPD\jepres\jepres07.frm(FrmSinBusePrint) => frmPmpaViewSinBusePrint.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jepres\jepres07.frm(FrmSinBusePrint)
    /// </seealso>
    /// </summary> 
    public partial class frmPmpaViewSinBusePrint : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        public frmPmpaViewSinBusePrint()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
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

            optSort0.Checked = true;
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

            else if (sender == this.btnCancel)
            {
                ssList_Sheet1.Rows.Count = 0;
                ssList_Sheet1.Rows.Count = 50;

                btnView.Enabled = true;
                btnPrint.Enabled = false;
                btnCancel.Enabled = false;
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            string strLine1 = VB.Space(30) + "┌─┬────┬────┬────┬────┐ " + "\n";
            string strLine2 = VB.Space(30) + "│결│담　  당│계　  장│팀　  장│병 원 장│ " + "\n";
            string strLine3 = VB.Space(30) + "│　├────┼────┼────┼────┤ " + "\n";
            string strLine4 = VB.Space(30) + "│　│　　　　│　　　　│　　　　│　　　　│ " + "\n";
            string strLine5 = VB.Space(30) + "│재│　　　　│　　　　│　　　  │　　　  │ " + "\n";
            string strLine6 = VB.Space(30) + "└─┴────┴────┴────┴────┘ " + "\n";

            strTitle = "부서별 신환추천 현황";
            strSubTitle = "작업기간: " + dtpFdate.Text + " ~ " + dtpTDate.Text;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strHeader += SPR.setSpdPrint_String(strLine1, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine2, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine3, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine4, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine5, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine6, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 75, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, false, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);            
        }

        void eGetData()
        {
            int i = 0;
            int nREAD = 0;

            string strTDate = "";
            string strFDate = "";
            string strBuse_New = "";
            string strBuse_Old = "";

            int nSubSum = 0;
            int nTotal = 0;

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = "";

            btnCancel.Enabled = true;
            btnPrint.Enabled = true;
            btnView.Enabled = false;

            strTDate = dtpFdate.Text;
            strFDate = dtpTDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                     ";
            SQL += ComNum.VBLF + "  A.SABUN, A.SNAME, SUBSTR(C.BUSE,1,4) BUSE, COUNT(A.SABUN) CNT, c.BUSE BUSECODE                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SINHOAN A, " + ComNum.DB_PMPA + "BAS_BUSE B, " + ComNum.DB_ERP + "INSA_MST C";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                  ";
            SQL += ComNum.VBLF + "      AND A.BDATE >= TO_DATE('" + strTDate + "','YYYY-MM-DD')                                              ";
            SQL += ComNum.VBLF + "      AND A.BDATE <= TO_DATE('" + strFDate + "','YYYY-MM-DD')                                              ";
            SQL += ComNum.VBLF + "      AND A.SABUN = C.SABUN                                                                                ";
            SQL += ComNum.VBLF + "      AND C.BUSE = B.BUCODE                                                                                ";
            SQL += ComNum.VBLF + "      AND A.CANDATE IS NULL                                                                                ";
            SQL += ComNum.VBLF + "      AND A.SIGN1SABUN IS NOT NULL                                                                         ";  //부서장 결제된 것
            SQL += ComNum.VBLF + "GROUP BY A.SABUN, A.SNAME, C.BUSE                                                                          ";

            if (optSort0.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY c.BUSE, SABUN, SNAME                                                                          ";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY c.BUSE, SNAME, SABUN                                                                          ";
            }

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
                    nREAD = dt.Rows.Count;

                    strBuse_New = "";
                    strBuse_Old = "";
                    nSubSum = 0;
                    nTotal = 0;

                    ssList_Sheet1.Rows.Count = 1;
                    for (i = 0; i < nREAD; i++)
                    {
                        strBuse_New = dt.Rows[i]["BUSE"].ToString().Trim();

                        if (ssList_Sheet1.Rows.Count == 1)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT                                    ";
                            SQL += ComNum.VBLF + "  NAME                                    ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUSE       ";
                            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
                            SQL += ComNum.VBLF + "      AND BUCODE = '" + strBuse_New + "00'";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count == 0)
                            {
                                dt1.Dispose();
                                dt1 = null;
                                ComFunc.MsgBox("해당 DATA가 없습니다.");
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = dt1.Rows[0]["NAME"].ToString().Trim();
                            }

                            dt1.Dispose();
                            dt1 = null;

                            strBuse_Old = strBuse_New;
                        }

                        else if (strBuse_New != strBuse_Old)
                        {
                            ssList_Sheet1.Rows[ssList_Sheet1.Rows.Count - 1].ForeColor = Color.Black;
                            ssList_Sheet1.Rows[ssList_Sheet1.Rows.Count - 1].BackColor = Color.LightSkyBlue;

                            for (int k = 0; k < ssList_Sheet1.Rows.Count; k++)
                            {

                                FarPoint.Win.Spread.Model.CellRange cr = new FarPoint.Win.Spread.Model.CellRange(0, 0, k, 1);
                                ssList.Sheets[0].SetOutlineBorder(cr, new FarPoint.Win.LineBorder(Color.White));
                                ssList.Sheets[0].SetInsideBorder(cr, new FarPoint.Win.LineBorder(Color.White));
                            }


                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "소   계";
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = nSubSum.ToString();

                            nSubSum = 0;

                            ssList_Sheet1.Rows.Count += 1;

                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT                                    ";
                            SQL += ComNum.VBLF + "  NAME                                    ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUSE       ";
                            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
                            SQL += ComNum.VBLF + "      AND BUCODE = '" + strBuse_New + "00'";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            //if (dt1.Rows.Count == 0)
                            //{
                            //    dt1.Dispose();
                            //    dt1 = null;
                            //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                            //}

                            if (dt1.Rows.Count > 0)
                            {
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = dt1.Rows[0]["NAME"].ToString().Trim();
                            }

                            dt1.Dispose();
                            dt1 = null;

                            strBuse_Old = strBuse_New;
                        }

                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = dt.Rows[i]["CNT"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                                ";
                        SQL += ComNum.VBLF + "  NAME BUNAME                                                         ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUSE                                   ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
                        SQL += ComNum.VBLF + "      AND BUCODE = '" + dt.Rows[i]["BUSECODE"].ToString().Trim() + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            dt1.Dispose();
                            dt1 = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = dt1.Rows[0]["BUNAME"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;

                        nSubSum += Convert.ToInt32(VB.Val(dt.Rows[i]["CNT"].ToString().Trim()));
                        nTotal += Convert.ToInt32(VB.Val(dt.Rows[i]["CNT"].ToString().Trim()));
                        ssList_Sheet1.Rows.Count += 1;
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

            ssList_Sheet1.Rows[ssList_Sheet1.Rows.Count - 1].ForeColor = Color.Black;
            ssList_Sheet1.Rows[ssList_Sheet1.Rows.Count - 1].BackColor = Color.LightSkyBlue;
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "소   계";
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = nSubSum.ToString();

            ssList_Sheet1.Rows.Count += 1;


            ssList_Sheet1.Rows[ssList_Sheet1.Rows.Count - 1].ForeColor = Color.Black;
            ssList_Sheet1.Rows[ssList_Sheet1.Rows.Count - 1].BackColor = Color.LightPink;
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "총   계";
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = nTotal.ToString();
        }
    }
}
