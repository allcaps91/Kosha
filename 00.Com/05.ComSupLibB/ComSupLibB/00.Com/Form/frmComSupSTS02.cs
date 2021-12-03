using ComBase; //기본 클래스
using ComDbB; //DB연결
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupSTS02.cs
    /// Description     : 진료지원 종합 상태 조회 폼
    /// Author          : 김홍록
    /// Create Date     : 2018-03-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    ///  frmComSupSTS01.cs 신규폼 생성
    /// </history>
    /// <seealso cref= " frmComSupSTS01.cs 폼이름 재정의" />
    public partial class frmComSupSTS02 : Form
    {
        string gStrPANO = "";
        string gStrSNAME = "";
        clsMethod method = new clsMethod();
        clsComSup sup = new clsComSup();
        clsComSQL clsSQL = new clsComSQL();

        frmComSupSTS01 frmComSupSTS01x = null; //통합정보폼

        Thread thread;

        public frmComSupSTS02(string strPANO, string strSNAME)
        {
            InitializeComponent();

            this.gStrPANO   = strPANO;
            this.gStrSNAME  = strSNAME;

            setEvent();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                setCtrl();
            }           
        }

        void setCtrl()
        {
            this.lbl_PTINFO.Text = "[" + this.gStrPANO + "/" + this.gStrSNAME + "]";

            frmComSupSTS01x = new frmComSupSTS01("", "", false, true, false);
            sup.setCtrlLoad(this.pan_frmComSupSTS01, frmComSupSTS01x);


            setCtrlSpread();
        }

        void setCtrlSpread()
        {
            thread = new Thread(() => threadSetCtrlSpread());
            thread.Start();
        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.ss_PTINFO.CellClick    += new CellClickEventHandler(eSpreadClick);
            this.ss_PTINFO.LeaveCell += new LeaveCellEventHandler(eSPreadLeaveCell);
        }

        void eSPreadLeaveCell(object sender, LeaveCellEventArgs e)
        {
            if (e.NewColumn != 0)
            {
                this.ss_PTINFO.ActiveSheet.Cells[1, 0, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, this.ss_PTINFO.ActiveSheet.ColumnCount - 1].BackColor = System.Drawing.Color.White;
                this.ss_PTINFO.ActiveSheet.Cells[1, e.NewColumn, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, e.NewColumn].BackColor = method.cSpdCellClick;

            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            if (e.Column != 0)
            {
                this.ss_PTINFO.ActiveSheet.Cells[1, 0, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, this.ss_PTINFO.ActiveSheet.ColumnCount - 1].BackColor = System.Drawing.Color.White;
                this.ss_PTINFO.ActiveSheet.Cells[1, e.Column, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, e.Column].BackColor = method.cSpdCellClick;
            }
        }

        void threadSetCtrlSpread()
        {
            this.Invoke(new delegateRunProcess(setCtrlCircular), new object[] { true });

            PsmhDb pDbCon = null;
            pDbCon = clsDB.DBConnect();

            try
            {
                DataTable dt = clsSQL.sel_PATIENT_SCHDUL(clsDB.DbCon, this.gStrPANO);

                this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { dt });
                this.Invoke(new delegateRunProcess(setCtrlCircular), new object[] { false });
             
                clsDB.DisDBConnect(pDbCon);
                pDbCon = null;
            }
            catch (Exception ex)
            {
                this.Invoke(new delegateRunProcess(setCtrlCircular), new object[] { false });
                clsDB.DisDBConnect(pDbCon);
                pDbCon = null;
                ComFunc.MsgBox(ex.ToString());
            }
        }

        delegate void delegateRunProcess(bool b);
        void setCtrlCircular(bool b)
        {
            this.barProgress.Visible = b;
            this.barProgress.IsRunning = b;
        }

        delegate void delegateSetSpdStyle(DataTable dt);
        void setSpdStyle(DataTable dt)
        {            

            if (ComFunc.isDataTableNull(dt) == false)
            {

                DateTime sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")); 


                this.ss_PTINFO.ActiveSheet.Columns.Count = dt.Columns.Count - 1;
                this.ss_PTINFO.ActiveSheet.Rows.Count = dt.Rows.Count;

                this.ss_PTINFO.ActiveSheet.Cells[0, 0, this.ss_PTINFO.ActiveSheet.Rows.Count-1, this.ss_PTINFO.ActiveSheet.Columns.Count-1].Text = "";

                this.ss_PTINFO.ActiveSheet.Cells[0, 0, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, this.ss_PTINFO.ActiveSheet.Columns.Count - 1].Locked = true;

                this.ss_PTINFO.ActiveSheet.Cells[0, 0, 0, this.ss_PTINFO.ActiveSheet.Columns.Count - 1].BackColor = System.Drawing.Color.FromArgb(230, 253, 253);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.ss_PTINFO.ActiveSheet.Cells[i, 0].Text = dt.Rows[i][0].ToString();
                }

                this.ss_PTINFO.ActiveSheet.Columns.Get(0).Width = 60;
                this.ss_PTINFO.ActiveSheet.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Center;
                this.ss_PTINFO.ActiveSheet.Columns[0].VerticalAlignment = CellVerticalAlignment.Center;

                for (int j = 2; j < dt.Columns.Count; j++)
                {
                    this.ss_PTINFO.ActiveSheet.Columns.Get(j-1).Width = 80;

                    this.ss_PTINFO.ActiveSheet.Columns[j - 1].HorizontalAlignment  = CellHorizontalAlignment.Center;
                    this.ss_PTINFO.ActiveSheet.Columns[j - 1].VerticalAlignment  = CellVerticalAlignment.Center;

                    for (int i = 0; i < this.ss_PTINFO.ActiveSheet.Rows.Count; i++)
                    {
                        this.ss_PTINFO.ActiveSheet.Cells[i, j-1].Text = dt.Rows[i][j].ToString();

                        if (i == 0)
                        {
                            if (Convert.ToDateTime(dt.Rows[i][j].ToString()) > sysdate)
                            {
                                this.ss_PTINFO.ActiveSheet.Cells[i, j - 1].BackColor = System.Drawing.Color.FromArgb(15,187,187);
                            }
                            else if (Convert.ToDateTime(dt.Rows[i][j].ToString()) == sysdate)
                            {
                                this.ss_PTINFO.ActiveSheet.Cells[i, j - 1].BackColor = System.Drawing.Color.FromArgb(147, 247, 247);
                            }
                        }
                    }

                }

                this.ss_PTINFO.ActiveSheet.Cells[1, this.ss_PTINFO.ActiveSheet.Columns.Count - 1, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, this.ss_PTINFO.ActiveSheet.Columns.Count - 1].BackColor = method.cSpdCellClick;

                this.ss_PTINFO.ActiveSheet.FrozenColumnCount = 1;
                this.ss_PTINFO.ActiveSheet.SetActiveCell(1, this.ss_PTINFO.ActiveSheet.ColumnCount - 1);
                this.ss_PTINFO.ShowCell(0, 0, 0, this.ss_PTINFO.ActiveSheet.ColumnCount - 1, VerticalPosition.Center, HorizontalPosition.Center);                    
            }            
        }
    }
}
