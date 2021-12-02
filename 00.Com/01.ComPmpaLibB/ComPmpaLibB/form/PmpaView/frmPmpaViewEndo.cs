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
    /// File Name       : frmPmpaViewEndo.cs
    /// Description     : 내시경 환자 관리
    /// Author          : 안정수
    /// Create Date     : 2017-09-01
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\oiguide\FrmENDO.frm(FrmEndo) => frmPmpaViewEndo.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oiguide\FrmENDO.frm(FrmEndo)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewEndo : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        string mstrPassId = "";
        public frmPmpaViewEndo()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewEndo(string FstrPassId)
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
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등 

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            CF.COMBO_DEPT_SET(clsDB.DbCon, cboDept);

            cboIO.Items.Clear();
            cboIO.Items.Add("*.전체");
            cboIO.Items.Add("I.입원");
            cboIO.Items.Add("O.외래");

            cboIO.SelectedIndex = 2;

            cboDrCode.Items.Clear();
            cboDrCode.Items.Add("****.전체");
            cboDrCode.SelectedIndex = 0;

            ssList_Sheet1.Columns[ssList_Sheet1.Columns.Count - 1].Visible = false;
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
                
                // 출력시, 가장 끝 칼럼이 빈값일 경우 짤리는 현상 방지를 위함
                ssList_Sheet1.Columns[ssList_Sheet1.Columns.Count - 1].Visible = true;

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

            ssList_Sheet1.Columns[ssList_Sheet1.Columns.Count - 1].Visible = false;
            
            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            //ssList.ActiveSheet.Cells[0, 9].Text = "zzz";
            //ssList.ActiveSheet.Columns[9].Visible = false;


            #endregion

            strTitle = "내시경 예약 환자 LIST";
            strSubTitle = "◆예약일: " + dtpFdate.Text + "부터 " + dtpTDate.Text + "까지";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 80, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, true, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;
            int nRow = 0;
            string strOK = "";
            string strROWID = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.Rows.Count = 0;
            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                             ";
            SQL += ComNum.VBLF + "  A.RDATE,  A.PTNO,  A.SNAME , A.DEPTCODE, A.DRCODE,  B.DRNAME, RESULTDATE, GBIO, C.ORDERNAME,                     ";
            SQL += ComNum.VBLF + "  A.REMARK, A.GBSUNAP, A.ROWID , D.TEL, D.HPHONE                                                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "ENDO_JUPMST A, " + ComNum.DB_PMPA + "BAS_DOCTOR B,                                       ";
            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_ORDERCODE C, " + ComNum.DB_PMPA + "BAS_PATIENT D                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                          ";
            SQL += ComNum.VBLF + "      AND A.DRCODE =B.DRCODE (+)                                                                                   ";
            SQL += ComNum.VBLF + "      AND A.ORDERCODE =C.ORDERCODE(+)                                                                              ";
            SQL += ComNum.VBLF + "      AND A.PTNO  = D.PANO(+)                                                                                      ";
            SQL += ComNum.VBLF + "      AND RDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')                                                   ";
            SQL += ComNum.VBLF + "      AND RDATE <  TO_DATE('" + Convert.ToDateTime(dtpTDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";

            if (VB.Left(cboDept.SelectedItem.ToString().Trim(), 2) != "**")
            {
                SQL += ComNum.VBLF + "  AND A.DEPTCODE = '" + VB.Left(cboDept.SelectedItem.ToString().Trim(), 2) + "'                                ";
            }
            if (VB.Left(cboDrCode.SelectedItem.ToString().Trim(), 2) != "**")
            {
                SQL += ComNum.VBLF + "  AND A.DRCODE = '" + VB.Left(cboDrCode.SelectedItem.ToString().Trim(), 4) + "'                                ";
            }
            if (VB.Left(cboIO.SelectedItem.ToString().Trim(), 1) != "*")
            {
                SQL += ComNum.VBLF + "  AND A.GBIO = '" + VB.Left(cboIO.SelectedItem.ToString().Trim(), 1) + "'                                      ";
            }

            SQL += ComNum.VBLF + "ORDER BY A.RDATE, A.GBIO                                                                                           ";

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
                    nRead = dt.Rows.Count;

                    ssList_Sheet1.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = VB.Left(dt.Rows[i]["RDATE"].ToString().Trim(), 10);
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PtNo"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GBIO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();

                        //접수
                        if (dt.Rows[i]["GbSunap"].ToString().Trim() == "1")
                        {
                            ssList_Sheet1.Cells[i, 7].Text = "접수";
                        }

                        //미접수
                        else if (dt.Rows[i]["GbSunap"].ToString().Trim() == "2")
                        {

                        }

                        //취소
                        else if (dt.Rows[i]["GbSunap"].ToString().Trim() == "*")
                        {
                            ssList_Sheet1.Cells[i, 7].Text = "취소";
                            ssList_Sheet1.Rows[i].ForeColor = Color.Red;
                        }

                        if (dt.Rows[i]["ResultDate"].ToString().Trim() != "")
                        {
                            ssList_Sheet1.Cells[i, 7].Text = "결과";
                        }

                        ssList_Sheet1.Cells[i, 8].Text = "T:" + dt.Rows[i]["Tel"].ToString().Trim() + " H:" + dt.Rows[i]["Hphone"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
            Cursor.Current = Cursors.Default;


        }

        void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VB.Left(cboDept.SelectedItem.ToString(), 2) != "**")
            {
                CF.ComboDr_Set(clsDB.DbCon, cboDrCode, VB.Left(cboDept.SelectedItem.ToString(), 2), "1");
            }
        }

        void cboDept_Leave(object sender, EventArgs e)
        {
            CF.ComboDr_Set(clsDB.DbCon, cboDrCode, VB.Left(cboDept.SelectedItem.ToString(), 2), "1");
        }

        void ssList_EditModeOff(object sender, EventArgs e)
        {

            int a = ssList_Sheet1.ActiveRowIndex;
            string strROWID = "";
            string strRemark = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            if (ssList.EditMode == true)
            {
                return;
            }


            strRemark = ssList_Sheet1.Cells[a, 9].Text.Replace("'", "`");
            strROWID = ssList_Sheet1.Cells[a, 10].Text;

            if (strROWID == "")
            {
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_MED + "ENDO_JUPMST";
            SQL += ComNum.VBLF + "  SET                                  ";
            SQL += ComNum.VBLF + "REMARK = '" + strRemark + "'           ";
            SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'       ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            try
            {

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }




        }
    }
}
