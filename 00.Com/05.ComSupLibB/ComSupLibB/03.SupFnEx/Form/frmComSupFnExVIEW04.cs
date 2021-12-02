using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupFnEx
{
    public partial class frmComSupFnExVIEW04 : Form
    {
        /// <summary>
        /// Class Name      : ComSupLibB.SupFnEx
        /// File Name       : frmComSupFnExVIEW04.cs
        /// Description     : 포스코 명단 조회
        /// Author          : 윤조연
        /// Create Date     : 2017-08-10
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "\\frm포스코예약보기.frm(frm포스코예약보기) >> frmComSupFnExVIEW04.cs 폼이름 재정의" />
        /// 

        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupFnExSpd fnexSpd = new clsComSupFnExSpd();
        clsComSupFnExSQL fnExSql = new clsComSupFnExSQL();

        #endregion

        public frmComSupFnExVIEW04()
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


            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSearch.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

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
                fnexSpd.sSpd_Posco(ssList, fnexSpd.sSpdPosco, fnexSpd.nSpdPosco, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();

                screen_clear();

            }                        

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnSearch)
            {
                //조회
                GetData(clsDB.DbCon,ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
            }
            else if (sender == this.btnPrint)
            {
                //인쇄
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

            strTitle = "POSCO 검사 의뢰자 LIST " + "(" + dtpFDate.Text.Trim() +"~" + dtpTDate.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 30, 10);
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
            dt = fnExSql.sel_BAS_PATIENT_POSCO(pDbCon, argSDate, argTDate);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {


                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.JepDate].Text = dt.Rows[i]["JDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.Jumin].Text = dt.Rows[i]["Jumin"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.jobName].Text = dt.Rows[i]["JobName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.Sabun].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.SinGu].Text = dt.Rows[i]["Singu"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.USG].Text = dt.Rows[i]["EXAM1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.UGI].Text = dt.Rows[i]["EXAM8"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.GFS].Text = dt.Rows[i]["EXAM2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.GFS1].Text = dt.Rows[i]["EXAM3"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.CFS].Text = dt.Rows[i]["EXAM6"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.CT1].Text = dt.Rows[i]["EXAM7"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.CT2].Text = dt.Rows[i]["EXAM9"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.CT3].Text = dt.Rows[i]["EXAM10"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.CT4].Text = dt.Rows[i]["EXAM11"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.CT5].Text = dt.Rows[i]["EXAM12"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.Echo1].Text = dt.Rows[i]["EXAM13"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.Echo2].Text = dt.Rows[i]["EXAM14"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmPosco.Echo3].Text = dt.Rows[i]["EXAM15"].ToString().Trim();


                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

    }
}
