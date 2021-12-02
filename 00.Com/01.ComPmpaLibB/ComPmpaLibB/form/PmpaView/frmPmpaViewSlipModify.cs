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
    /// File Name       : frmPmpaViewSlipModify.cs
    /// Description     : 외래 부분취소,환불자 명단 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-27
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\oviewa\OVIEWA11.FRM(FrmSlipModify) => frmPmpaViewSlipModify.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA11.FRM(FrmSlipModify)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewSlipModify : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        string mstrJobMan = "";

        public frmPmpaViewSlipModify()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewSlipModify(string GstrJobMan)
        {
            InitializeComponent();
            setEvent();
            mstrJobMan = GstrJobMan;
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

            optJob0.Checked = true;

            ssList_Sheet1.Columns[ssList_Sheet1.Columns.Count - 1].Visible = false;
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
              //  if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
              //  {
              //      return; //권한 확인
              //  }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
             //   if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
             //   {
             //       return; //권한 확인
             //   }
                ePrint();
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


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            ssList.ActiveSheet.Cells[0, ssList_Sheet1.Columns.Count - 1].Text = "zzz";
            //ssList.ActiveSheet.Columns[9].Visible = false;


            #endregion

            strTitle = "전표 수정,삭제 현황";
            strSubTitle = "작업일자 : " + dtpDate.Text;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("바탕체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("바탕체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "\r\n" + "출력자:  " + mstrJobMan, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 130, 70);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            string strPtNO = "";

            CS.Spread_All_Clear(ssList);

            btnView.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                        ";
            SQL += ComNum.VBLF + "  a.Pano,b.Sname,a.Bi,SeqNo,TO_CHAR(Bdate,'yy-mm-dd') Bilja,Part,a.DeptCode,                  ";
            SQL += ComNum.VBLF + "  SUM(DECODE(BUN,'91',0,'92',0,'96',0,'98',0,'99',0,Amt1)) TAmt,                              ";
            SQL += ComNum.VBLF + "  SUM(DECODE(BUN,'98',Amt1,0)) JAmt,                                                          ";
            SQL += ComNum.VBLF + "  SUM(DECODE(BUN,'92',Amt1,'96',Amt1,0)) MAmt,                                                ";
            SQL += ComNum.VBLF + "  SUM(DECODE(BUN,'99',Amt1,0)) YAmt                                                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP a, " + ComNum.DB_PMPA + "Bas_Patient b                    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "      AND a.ActDate = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                            ";
            SQL += ComNum.VBLF + "      AND (a.Pano, ACTDATE) IN (SELECT Pano, ACTDATE FROM OPD_Slip                            ";
            SQL += ComNum.VBLF + "                                WHERE ActDate = TO_DATE('" + dtpDate.Text + "','yyyy-mm-dd')  ";
            SQL += ComNum.VBLF + "                                  AND BUN = '99'                                              ";
            SQL += ComNum.VBLF + "                                  AND Amt1 < 0                                                ";
            if (optJob0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND Bdate <> ActDate                                                                    ";
            }
            SQL += ComNum.VBLF + "                                GROUP BY Pano, ACTDATE )                                      ";
            SQL += ComNum.VBLF + "      AND a.Pano = b.Pano                                                                     ";
            SQL += ComNum.VBLF + "GROUP BY a.Pano,b.Sname,a.Bi,a.SeqNo,Bdate,a.Part,a.DeptCode                                  ";
            SQL += ComNum.VBLF + "ORDER BY a.Pano,a.SeqNo                                                                       ";

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
                    btnView.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    dtpDate.Focus();
                    return;
                }

                strPtNO = "";

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Rows.Count = i + 1;

                        if (i == 0)
                        {
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        }
                        else if (i > 0)
                        {
                            if (strPtNO != dt.Rows[i]["Pano"].ToString().Trim())
                            {
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                            }
                        }

                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = dt.Rows[i]["SeqNO"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = dt.Rows[i]["Bilja"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["TAmt"].ToString().Trim()));
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 7].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["JAmt"].ToString().Trim()));
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 8].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()));
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 9].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["YAmt"].ToString().Trim()));
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 10].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[i]["Part"].ToString().Trim());

                        strPtNO = dt.Rows[i]["Pano"].ToString().Trim();
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

            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;
        }



    }
}
