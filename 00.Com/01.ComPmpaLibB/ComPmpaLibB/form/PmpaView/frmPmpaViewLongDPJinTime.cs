using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewLongDPJinTime.cs
    /// Description     : 원거리환자 진료소요시간
    /// Author          : 안정수
    /// Create Date     : 2017-09-20
    /// Update History  : 2017-11-06
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm원거리소요시간.frm(Frm원거리소요시간) => frmPmpaViewLongDPJinTime.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm원거리소요시간.frm(Frm원거리소요시간)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewLongDPJinTime : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        public frmPmpaViewLongDPJinTime()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnFileMake.Click += new EventHandler(eBtnEvent);
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

            optGubun0.Checked = true;
            btnPrint.Enabled = false;

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
               // if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
               // {
               //     return; //권한 확인
               // }
                ePrint();
            }

            else if (sender == this.btnCancel)
            {
                ssList_Sheet1.Rows.Count = 0;
                CS.Spread_All_Clear(ssList);
            }

            else if (sender == this.btnFileMake)
            {
                btnFileMake_Cilck(ssList);
            }
        }

        void ePrint()
        {
            ComFunc.MsgBox("구현되지 않음");
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            CS.Spread_All_Clear(ssList);
            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                    ";
            SQL += ComNum.VBLF + "  a.Pano,a.Sname,a.DeptCode,a.Jtime,a.Stime,                              ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.Jtime,'yyyy-mm-dd hh24:mi') Jepdate ,                         ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.Stime,'yyyy-mm-dd hh24:mi') SuDate ,                          ";
            SQL += ComNum.VBLF + "  c.JINAME,a.Sex,a.Age                                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER a, " +
                                           ComNum.DB_PMPA + "BAS_PATIENT b, " + ComNum.DB_PMPA + "BAS_AREA c";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "      AND a.Pano =b.Pano(+)                                               ";
            SQL += ComNum.VBLF + "      AND b.JiCode=c.JiCode                                               ";
            if (optGubun0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND (b.JiCode = '63' OR b.JICODE >= '77')                           ";
            }
            else if (optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND b.JiCode IN ('78','79','82')                                    ";
            }
            else if (optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND b.JiCode = '78'                                                 ";
            }
            else if (optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND b.JiCode = '79'                                                 ";
            }
            else if (optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND b.JiCode = '82'                                                 ";
            }
            SQL += ComNum.VBLF + "      AND a.ActDate >=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')       ";
            SQL += ComNum.VBLF + "      AND a.ActDate <=TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')       ";
            SQL += ComNum.VBLF + "      AND a.JTime < a.STime                                               ";
            SQL += ComNum.VBLF + "      AND a.STime IS NOT NULL                                             ";
            SQL += ComNum.VBLF + "ORDER BY a.DeptCode,a.Jtime,a.SName,C.JiName                              ";

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
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JepDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = CF.DATE_TIME(clsDB.DbCon, dt.Rows[i]["JepDate"].ToString().Trim(), dt.Rows[i]["SuDate"].ToString().Trim()).ToString();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["JiName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Age"].ToString().Trim();
                    }

                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            Cursor.Current = Cursors.Default;
            dt.Dispose();
            dt = null;

        }

        void btnFileMake_Cilck(FarPoint.Win.Spread.FpSpread Spd)
        {

            SaveFileDialog mDlg = new SaveFileDialog();
            mDlg.InitialDirectory = Application.StartupPath;
            mDlg.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            mDlg.FilterIndex = 1;

            if (mDlg.ShowDialog() == DialogResult.OK)
            {
                Spd.SaveExcel(mDlg.FileName, FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                ComFunc.MsgBox("저장이 완료 되었습니다.");
            }
        }

    }
}
