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
    /// File Name       : frmComSupEndsVIEW03.cs
    /// Description     : 내시경관리 물품 내역 조회 + 내시경 소독 약품 내역
    /// Author          : 윤조연
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\ocs\endo\endo_new\Frm내시경물품출고.frm(Frm내시경물품출고,Frm내시경소독약품출고) >> frmComSupEndsVIEW03.cs 폼이름 재정의" />
    public partial class frmComSupEndsVIEW03 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsQuery Query = new clsQuery();
        clsComSupEndsSpd endsSpd = new clsComSupEndsSpd();
        clsComSupEndsSQL endsSql = new clsComSupEndsSQL();
        

        string gList = "";
        string gViewType = "";
        string gTitle = "";
        

        #endregion

        public frmComSupEndsVIEW03(clsComSupEnds.enmEndoViewType EndoViewType)
        {
            InitializeComponent();

            if (EndoViewType == clsComSupEnds.enmEndoViewType.ENDO_GUME_VIEW)
            {
                gViewType = "Gume";
                gTitle = "내시경 물품 내역";
            }
            else if (EndoViewType == clsComSupEnds.enmEndoViewType.ENDO_DRUG_VIEW)
            {
                gViewType = "Drug";
                gTitle = "내시경 소독약품 내역";
            }

            lblcap.Text = gTitle;

            setEvent();

        }

        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {
            DataTable dt = null;
            
            gList = "";
            if (gViewType == "Gume")
            {
                //쿼리실행      
                dt = Query.Get_BasBcode(pDbCon, "ENDO_내시경부속물품", "","","", " Code ");
            }
            else if (gViewType == "Drug")
            {
                //쿼리실행      
                dt = Query.Get_BasBcode(pDbCon, "ENDO_내시경소독약품", "","","", " Code ");
            }

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    gList +=  "'" + dt.Rows[i]["Code"].ToString().Trim() + "'," ;
                }

                if (gList!="")
                {
                    gList = VB.Left(gList, VB.Len(gList) - 1);
                }

            }

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

        }        

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else if (gViewType  =="")
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {

                if (gViewType =="Gume")
                {                    
                    endsSpd.sSpd_enmGumeview(ssList, endsSpd.sSpdGumeview, endsSpd.nSpdGumeview, 1, 0);
                }
                else if (gViewType == "Drug")
                {
                    endsSpd.sSpd_enmDrugview(ssList, endsSpd.sSpdDrugview, endsSpd.nSpdDrugview, 1, 0);
                }

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData(clsDB.DbCon);

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
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                ePrint(gViewType);
            }
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void ePrint(string argViewType)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (argViewType == "Gume")
            {
                strTitle = gTitle;
            }
            else if (argViewType == "Drug")
            {
                strTitle = gTitle + " 작업기간: " + dtpFDate.Text.Trim() + "~" + dtpTDate.Text.Trim() ;
            }
            
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

        void screen_display()
        {
            //조회
            GetData(clsDB.DbCon, ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim(), gViewType);
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate,string viewType)
        {

            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            //쿼리실행   
            if (viewType=="Gume")
            {                   
                dt = endsSql.sel_GumeView(pDbCon, argSDate, argTDate, gList, "056104");                               
            }
            else if (viewType == "Drug")
            {                   
                dt = endsSql.sel_DrugView(pDbCon, argSDate, argTDate, gList, "056104");                
            }
            
            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmGumeview.CDate].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmGumeview.JepCode].Text = dt.Rows[i]["JepCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmGumeview.JepName].Text = dt.Rows[i]["JepName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmGumeview.Unit].Text = dt.Rows[i]["CovUnit"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmGumeview.Qty].Text = dt.Rows[i]["Qty"].ToString().Trim();

                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

    }
}
