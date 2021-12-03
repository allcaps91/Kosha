using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSlipView.cs
    /// Description     : 처방전 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : 생성자 부분 변경 및 환자정보표시 부분 추가
    /// <history>       
    /// D:\타병원\PSMHH\exam\exveri\exveri07.frm(FrmSlipView) => frmSlipView.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\exam\exveri\exveri07.frm(FrmSlipView)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\exam\exveri.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmSlipView : Form, MainFormMessage
    {
        string mstrPanoSearch = "";

        string FstrPano = "";
        string FstrSname = "";
        string FstrBi = "";
        string FstrSex = "";

        int FnAge = 0;

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

        public frmSlipView(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();       
        }
        public frmSlipView()
        {
            InitializeComponent();
            setEvent();
        }
        
        //gsPanoSearch, FstrPano를 받아들인다.
        public frmSlipView(string gsPanoSearch)
        {
            InitializeComponent();
            setEvent();

            mstrPanoSearch = gsPanoSearch;            
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);            
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.SetAllControlClear(panel2);                
                GetData();
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }   
        }  

        void GetData()
        {
            ssSlipView_Sheet1.RowCount = 0;

            string strOldData = "";
            string strNewData = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            int nRow = 0;


            //재원중인 환자종류를 READ
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT";
            SQL = SQL + ComNum.VBLF + "    Pano, Sname, Bi, Age, Sex";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
            SQL = SQL + ComNum.VBLF + " AND Pano = '" + mstrPanoSearch + "' ";
            SQL = SQL + ComNum.VBLF + " AND AmSet1 = '0' ";
            SQL = SQL + ComNum.VBLF + " AND AmSet6 = ' ' ";
            SQL = SQL + ComNum.VBLF + " AND GBSTS IN ('0', '2', '3', '4') ";
           
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
                FstrPano = dt.Rows[0]["Pano"].ToString().Trim();
                FstrSname = dt.Rows[0]["Sname"].ToString().Trim();
                FstrBi = dt.Rows[0]["Bi"].ToString().Trim();
                FnAge = Convert.ToInt32(dt.Rows[0]["Age"].ToString().Trim());
                FstrSex = dt.Rows[0]["Sex"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            txtPano.Text = FstrPano;
            txtSName.Text = FstrSname;
            txtAge.Text =  FstrSex + "/" + FnAge;                        
                  
            //재원SLIP을 READ
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, a.Bun, a.SuCode, a.SuNext,";
            SQL = SQL + ComNum.VBLF + "    a.Qty,b.SuNameK,SUM(a.Nal) Nal";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, " + ComNum.DB_PMPA + "BAS_SUN b";                
            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
            SQL = SQL + ComNum.VBLF + " AND a.Pano = '" + FstrPano + "' ";
            SQL = SQL + ComNum.VBLF + " AND a.Bi = '" + FstrBi + "' ";
            SQL = SQL + ComNum.VBLF + " AND a.SuNext = b.SuNext(+) ";
            SQL = SQL + ComNum.VBLF + "GROUP BY a.BDate,a.Bun,a.SuCode,a.SuNext,a.Qty,b.SuNameK";
            SQL = SQL + ComNum.VBLF + "ORDER BY a.BDate DESC,a.Bun,a.SuCode,a.SuNext ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nRow = 0;
                strOldData = "";                
                ssSlipView_Sheet1.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dt.Rows[i]["Nal"].ToString().Trim()) > 0)
                    {
                        nRow += 1;

                        if(nRow > ssSlipView.ActiveSheet.Rows.Count)
                        {
                            ssSlipView.ActiveSheet.Rows.Count = nRow;
                        }

                        strNewData = dt.Rows[i]["BDate"].ToString().Trim();

                        if (strNewData != strOldData)
                        {
                            ssSlipView_Sheet1.Cells[nRow - 1, 0].Text = strNewData;
                            strOldData = strNewData;
                        }

                        ssSlipView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                        ssSlipView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssSlipView_Sheet1.Cells[nRow - 1, 3].Text = String.Format("{0:##0.00}", VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                        ssSlipView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Nal"].ToString().Trim();
                        ssSlipView_Sheet1.Cells[nRow - 1, 5].Text = " " + dt.Rows[i]["SuNameK"].ToString().Trim();
                    }
                }
            }

            ssSlipView.ActiveSheet.Rows.Count = nRow;
           
            dt.Dispose();
            dt = null;
        }
    }
}
