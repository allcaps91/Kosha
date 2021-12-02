using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirDrBunho.cs
    /// Description     : 면허번호 등록
    /// Author          : 박성완
    /// Create Date     : 2017-12-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\FrmDrBunho.frm
    public partial class frmComMirDrBunho : Form
    {
        //이벤트를 전달할 경우 
        public delegate void EventSendData(string strRetValue);
        public event EventSendData rEventSendData;

        //폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        private string FstrHelpCode = "";
        private string DeptCode1 = "";

        public frmComMirDrBunho(string GstrHelpCode, string TID_DeptCode1)
        {
            FstrHelpCode = GstrHelpCode;
            DeptCode1 = TID_DeptCode1;

            InitializeComponent();

            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += FrmComMirDrBunho_Load;

            this.btnSave.Click += BtnSave_Click;
            this.btnExit.Click += BtnExit_Click;

            this.ssDept.CellClick += SsDept_CellClick;
            this.ssDCT.CellDoubleClick += SsDCT_CellDoubleClick;
            this.ssDR.CellDoubleClick += SsDR_CellDoubleClick;
        }

        private void SsDR_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            ssDR.ActiveSheet.RemoveRows(e.Row, 1);
        }

        private void SsDept_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            FarPoint.Win.Spread.FpSpread spd = (FarPoint.Win.Spread.FpSpread)sender;

            SQL = "SELECT DRNAME, DRBUNHO FROM KOSMOS_OCS.OCS_DOCTOR" + ComNum.VBLF;
            SQL += " WHERE DEPTCODE = '" + spd.ActiveSheet.Cells[e.Row, 0].Text + "' " + ComNum.VBLF;
            SQL += "   AND GRADE IN ('1','2')  " + ComNum.VBLF;
            //2017-08-24 퇴사자를 밑으로
            SQL += " ORDER BY  GBOUT, GRADE ASC ,FDATE DESC " + ComNum.VBLF;

            grbDr.Text = spd.ActiveSheet.Cells[e.Row, 1].Text;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ssDCT.ActiveSheet.Rows.Count = 0;
            ssDCT.ActiveSheet.Rows.Count = dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssDCT.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["DrName"].ToString().Trim();
                ssDCT.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DrBUNHO"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
        }

        private void SsDCT_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            string strDrBunho = "";
            string strDrName = "";

            strDrName = ssDCT.ActiveSheet.Cells[e.Row, 0].Text;
            strDrBunho = ssDCT.ActiveSheet.Cells[e.Row, 1].Text;

            if (strDrBunho == "")
                return;

            ssDR.ActiveSheet.Rows.Count++;
            ssDR.ActiveSheet.Cells[ssDR.ActiveSheet.Rows.Count - 1, 0].Text = strDrName;
            ssDR.ActiveSheet.Cells[ssDR.ActiveSheet.Rows.Count - 1, 1].Text = strDrBunho;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FstrHelpCode = "";

            for (int i = 0; i < ssDR.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (FstrHelpCode == "")
                {
                    FstrHelpCode = ssDR.ActiveSheet.Cells[i, 1].Text;
                }
                else
                {
                    FstrHelpCode += "/" + ssDR.ActiveSheet.Cells[i, 1].Text;
                }
            }

            rEventSendData(FstrHelpCode);
            this.Close();
            //rEventClosed();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            FstrHelpCode = "";

            for (int i = 0; i < ssDR.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (FstrHelpCode == "")
                {
                    FstrHelpCode = ssDR.ActiveSheet.Cells[i, 1].Text;
                }
                else
                {
                    FstrHelpCode += "/" + ssDR.ActiveSheet.Cells[i, 1].Text;
                }
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            rEventSendData(FstrHelpCode);
            this.Close();
        }

        private void FrmComMirDrBunho_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SetSpread();
        }

        private void SetSpread()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인	

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string strTemp2 = "";
            string strDrBunho = "";
            int nRow = 0;

            try
            {
                SQL = " SELECT DEPTCODE, DEPTNAMEK " + ComNum.VBLF;
                SQL += "  FROM KOSMOS_PMPA.BAS_CLINICDEPT " + ComNum.VBLF;
                SQL += " ORDER BY PRINTRANKING  " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    dt.Dispose();
                    dt = null;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    dt.Dispose();
                    dt = null;
                    return;
                }

                ssDept.ActiveSheet.Rows.Count = 0;
                ssDept.ActiveSheet.Rows.Count = dt.Rows.Count;
                ssDept.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssDept.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssDept.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
                    if (DeptCode1 == dt.Rows[i]["DeptCode"].ToString().Trim())
                    {
                        nRow = i;
                        
                    }
                }

                
                FarPoint.Win.Spread.CellClickEventArgs EventData;                
                EventData = new FarPoint.Win.Spread.CellClickEventArgs(new FarPoint.Win.Spread.SpreadView(), nRow , 0, 0, 0, MouseButtons.Left, false, false);
                SsDept_CellClick(ssDept, EventData);

                dt.Dispose();
                dt = null;

                ssDR.ActiveSheet.Rows.Count = 0;

                if (FstrHelpCode != "")
                {
                    nRow = 0;

                    lblSuNext.Text = VB.Pstr(FstrHelpCode, ComNum.VBLF, 1);
                    strTemp2 = VB.Pstr(FstrHelpCode, ComNum.VBLF, 2);

                    string[] strTemp = strTemp2.Split('/');
                    

                    foreach (var word in strTemp)
                    {
                        strDrBunho = $"{word}";
                        if (strDrBunho != "")
                        {
                            //번호읽기
                            SQL = " SELECT DRNAME " + ComNum.VBLF;
                            SQL += "    FROM KOSMOS_OCS.OCS_DOCTOR " + ComNum.VBLF;
                            SQL += "   WHERE DRBUNHO = '" + strDrBunho.Trim() + "' " + ComNum.VBLF;
                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (dt.Rows.Count > 0)
                            {
                                nRow++;
                                if (ssDR.ActiveSheet.Rows.Count < nRow)
                                {
                                    ssDR.ActiveSheet.Rows.Count++;
                                }

                                ssDR.ActiveSheet.Cells[nRow - 1, 0].Text = dt.Rows[0]["DrName"].ToString().Trim();
                                ssDR.ActiveSheet.Cells[nRow - 1, 1].Text = strDrBunho.Trim();
                            }

                            dt.Dispose();
                            dt = null;
                        }
 
                    }


                    //for (int k = 0; k < strTemp.Length; k++)
                    //{
                    //    if (strTemp[0] != "")
                    //    {
                    //        strDrBunho = strTemp[k];

                    //        //번호읽기
                    //        SQL = " SELECT DRNAME " + ComNum.VBLF;
                    //        SQL += "    FROM KOSMOS_OCS.OCS_DOCTOR " + ComNum.VBLF;
                    //        SQL += "   WHERE DRBUNHO = '" + strDrBunho.Trim() + "' " + ComNum.VBLF;
                    //        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            nRow++;
                    //            if (ssDR.ActiveSheet.Rows.Count < nRow)
                    //            {
                    //                ssDR.ActiveSheet.Rows.Count++;
                    //            }

                    //            ssDR.ActiveSheet.Cells[nRow - 1, 0].Text = dt.Rows[k]["DrName"].ToString().Trim();
                    //            ssDR.ActiveSheet.Cells[nRow - 1, 1].Text = strDrBunho.Trim();
                    //        }

                    //        dt.Dispose();
                    //        dt = null;
                    //    }

                    //}
                }

                FstrHelpCode = "";

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

    }
}
