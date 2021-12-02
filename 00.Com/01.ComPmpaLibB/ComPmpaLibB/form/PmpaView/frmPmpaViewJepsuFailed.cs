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
    /// File Name       : frmPmpaViewJepsuFailed.cs
    /// Description     : 접수부도자 일자별 List
    /// Author          : 안정수
    /// Create Date     : 2017-08-28
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\jepres\jepres13.frm(FrmJepsuFailedView) => frmPmpaViewJepsuFailed.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jepres\jepres13.frm(FrmJepsuFailedView)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewJepsuFailed : Form, MainFormMessage
    {
        ComFunc CF = new ComFunc();
        string mstrPassId = "";
        string FstrRowID = string.Empty;
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;
        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        #endregion

        public frmPmpaViewJepsuFailed(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewJepsuFailed()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewJepsuFailed(string FstrPassId)
        {
            InitializeComponent();
            setEvent();
            mstrPassId = FstrPassId;
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
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등     

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            optGB0.Checked = true;        
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                //                
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                //        
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
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

            if (optGB0.Checked == true)
            {
                strTitle = "( " + dtpDate.Text + " ) 예약부도자 LIST" + "/n";
            }
            else if (optGB1.Checked == true)
            {
                strTitle = "( " + dtpDate.Text + " ) 예약환불자 LIST" + "/n";
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 80, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);            
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int nREAD = 0;

            string strSDate = "";            
            
            int nAmt = 0;
            int nTAmt = 0;

            string strPart = "";
            string strPart2 = "";
            int nRow = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            strSDate = dtpDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,                                                                      ";
            SQL += ComNum.VBLF + "  PANO, DEPTCODE, DRCODE, CHOJAE, BI, SNAME, SEX, AGE, GBGAMEK, GBSPC, JIN , BOHUN,                           ";
            SQL += ComNum.VBLF + "  TO_CHAR(YDATE1,'YYYY-MM-DD HH24:MI') YDATE1,                                                                ";
            SQL += ComNum.VBLF + "  TO_CHAR(YDATE2,'YYYY-MM-DD HH24:MI') YDATE2,                                                                ";
            SQL += ComNum.VBLF + "  TO_CHAR(YDATE3,'YYYY-MM-DD HH24:MI') YDATE3,                                                                ";
            SQL += ComNum.VBLF + "  AMT1 , AMT2, AMT3, AMT4, AMT5, AMT6, AMT7,                                                                  ";
            SQL += ComNum.VBLF + "  GELCODE, CAMT, CSABUN, CPART,                                                                               ";
            SQL += ComNum.VBLF + "  TO_CHAR(RDATE,'YYYY-MM-DD HH24:MI') RDATE, RAMT, RSABUN, RPART, REMARK,ROWID                                     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND                                                                         ";
            SQL += ComNum.VBLF + "WHERE 1=1";

            if (optGB0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + strSDate + "','YYYY-MM-DD')                                                  ";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND RDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')                                                   ";
                SQL += ComNum.VBLF + "  AND RDATE < TO_DATE('" + Convert.ToDateTime(strSDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
            }

            SQL += ComNum.VBLF + "      AND GUBUN = '01'                                                                                        ";

            if (optGB0.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY DRCODE, PANO                                                                                     ";
            }

            else if (optGB1.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY RPART DESC, DRCODE, PANO                                                                         ";
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
                    ComFunc.MsgBox("해당하는 예약부도자가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = nREAD;

                    if (optGB0.Checked == true)
                    {
                        strPart = dt.Rows[0]["CPART"].ToString().Trim();
                    }
                    else if (optGB1.Checked == true)
                    {
                        strPart = dt.Rows[0]["RPART"].ToString().Trim();
                    }

                    nTAmt = 0;
                    nAmt = 0;
                    nRow = 0;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //display
                        if (optGB0.Checked == true)
                        {
                            strPart2 = dt.Rows[i]["CPART"].ToString().Trim();
                        }
                        else if (optGB1.Checked == true)
                        {
                            strPart2 = dt.Rows[i]["RPART"].ToString().Trim();
                        }

                        if (strPart != strPart2)
                        {
                            nRow += 1;
                            ssList_Sheet1.Rows.Count = nRow;
                            ssList_Sheet1.Rows[nRow - 1].BackColor = Color.Beige;

                            ssList_Sheet1.Cells[nRow - 1, 0].Text = "소  계";
                            ssList_Sheet1.Cells[nRow - 1, 5].Text = " 작업조: [" + VB.Left(strPart + VB.Space(5), 5) + "]";
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", nAmt);
                            nAmt = 0;
                            strPart = strPart2;
                        }
                        nRow += 1;
                        ssList_Sheet1.Rows.Count = nRow;

                        ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Bi"].ToString().Trim();

                        if (optGB0.Checked == true)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", dt.Rows[i]["CAMT"].ToString().Trim());
                            nAmt += Convert.ToInt32(VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()));
                            nTAmt += Convert.ToInt32(VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()));
                        }
                        else if (optGB1.Checked == true)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", dt.Rows[i]["RAMT"].ToString().Trim());
                            nAmt += Convert.ToInt32(VB.Val(dt.Rows[i]["RAMT"].ToString().Trim()));
                            nTAmt += Convert.ToInt32(VB.Val(dt.Rows[i]["RAMT"].ToString().Trim()));
                        }
                        ssList_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();

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

            nRow += 1;
            ssList_Sheet1.Rows.Count = nRow;
            ssList_Sheet1.Rows[nRow - 1].BackColor = Color.Beige;
            ssList_Sheet1.Cells[nRow - 1, 0].Text = "소  계";
            ssList_Sheet1.Cells[nRow - 1, 5].Text = " 작업사번: [" + VB.Left(strPart + VB.Space(5), 5) + "]";
            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", nAmt);

            nRow += 1;
            ssList_Sheet1.Rows.Count = nRow;
            ssList_Sheet1.Rows[nRow - 1].BackColor = Color.LightPink;
            ssList_Sheet1.Cells[nRow - 1, 0].Text = "전체 합계";
            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", nTAmt);
        }

        private void ssList_CellClick(object sender, CellClickEventArgs e)
        {

            txtRemark.Text = "";
            txtSname.Text = "";
            txtSname.Text = ssList_Sheet1.Cells[e.Row, 1].Text + " " + ssList_Sheet1.Cells[e.Row, 2].Text;    //예약일자
            FstrRowID = ssList_Sheet1.Cells[e.Row, 8].Text;    //부도일자
            txtRemark.Text = ssList_Sheet1.Cells[e.Row, 7].Text;    //예약금액

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Seve_Process(clsDB.DbCon);
        }
        private void Seve_Process(PsmhDb pDbCon)
        {
         
           

         

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_REFUND ";
                SQL += ComNum.VBLF + "    SET REMARK   = '" + txtRemark.Text + "' ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND ROWID = '" + FstrRowID + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
            txtRemark.Text = "";
            txtSname.Text = "";
            FstrRowID = "";
            txtRemark.Text = "";


            eGetData();
        }
    }
}
