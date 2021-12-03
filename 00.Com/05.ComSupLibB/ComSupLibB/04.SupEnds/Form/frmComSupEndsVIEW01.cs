using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsVIEW01.cs
    /// Description     : 내시경관리 전화통보 리스트 조회
    /// Author          : 윤조연
    /// Create Date     : 2017-07-01
    /// Update History  :  
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\ekg\endres08.frm(FrmTel) >> frmComSupEndsVIEW01.cs 폼이름 재정의" />
    public partial class frmComSupEndsVIEW01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupEndsSpd endsSpd = new clsComSupEndsSpd();
        clsComSupEndsSQL endsSql = new clsComSupEndsSQL();
                

        #endregion

        public frmComSupEndsVIEW01()
        {
            InitializeComponent();

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;
            

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch1.Click += new EventHandler(eBtnSearch);
            this.btnPrint.Click += new EventHandler(eBtnPrint);

            //this.ssList.CellDoubleClick += ssList_CellDoubleClick;


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
                //
                endsSpd.sSpd_enmTelview(ssList, endsSpd.sSpdTelview, endsSpd.nSpdTelview, 1, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();

                screen_clear();
            }
               
        }
                
        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch1)
            {
                //조회
                GetData(clsDB.DbCon,ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                ePrint();
            }
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

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

            strTitle = "전화통보 LIST " + "(" + dtpFDate.Text.Replace("-","") + " - " + dtpTDate.Text.Replace("-", "") + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            dtpFDate.Text = cpublic.strSysDate;
            dtpTDate.Text = cpublic.strSysDate;

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate)
        {

            int i = 0;  
            DataTable dt = null;
           
            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            //쿼리실행      
            dt = endsSql.sel_TelView(pDbCon, argSDate, argTDate);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTelview.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTelview.Pano].Text = dt.Rows[i]["Ptno"].ToString().Trim() ;

                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTelview.Age].Text = "";//TODO 윤조연 Age_Gesan(Date_Format(Format(AdoGetString(Rs, "BirthDate", i), "YYYY-MM-DD")))
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTelview.Sex].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTelview.ExName].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTelview.Tel].Text = dt.Rows[i]["TEL"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTelview.RDate].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTelview.TDate].Text = dt.Rows[i]["TDATE"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTelview.Tongbo].Text = clsVbfunc.GetInSaName(pDbCon, dt.Rows[i]["Sabun"].ToString().Trim());


                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }
        
    }

}
