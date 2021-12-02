using ComBase;
using ComDbB;
using ComSupLibB.Com;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayVIEW04.cs
    /// Description     : 영상의학과 
    /// Author          : 윤조연
    /// Create Date     : 
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xucode\.frm() >> frmComSupXrayVIEW04.cs 폼이름 재정의" />
    public partial class frmComSupXrayVIEW04 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsComSup sup = new clsComSup();
        clsComSupXray xray = new clsComSupXray();
        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();
        clsQuery Query = new clsQuery();

        #endregion

        public frmComSupXrayVIEW04()
        {
            InitializeComponent();
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

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
            this.btnCode.Click += new EventHandler(eBtnEvent);
            //this.btnPrint.Click += new EventHandler(eBtnEvent);


            //this.ssList1.EditChange += ssList1_EditChange;


        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                //            
                //xraySpd.sSpd_XrayView03(ssList, xraySpd.sSpdXrayView03, xraySpd.nSpdXrayView03, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                screen_clear();

                setCtrlData();

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
                //GetData(clsDB.DbCon, ssList);
            }
            else if (sender == this.btnCode)
            {
                //
                frmComSupCODE01 f = new frmComSupCODE01("HIC");
                f.ShowDialog();
                sup.setClearMemory(f);
            }


        }

        void screen_clear()
        {



        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {
            //int i = 0;
            //DataTable dt = null;
            //DataTable dt2 = null;

           

            //Cursor.Current = Cursors.WaitCursor;


            //Spd.ActiveSheet.RowCount = 0;

            ////진료과별 통계 쿼리실행                  
            //dt = xraySql.sel_Xray_MCode(pDbCon, txtMCode.Text.ToUpper().Trim(), "", "", true);

            #region //데이터셋 읽어 자료 표시

            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    Spd.ActiveSheet.RowCount = dt.Rows.Count;
            //    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

            //    for (i = 0; i < dt.Rows.Count; i++)
            //    {
            //        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayView03.mCode].Text = dt.Rows[i]["MCode"].ToString().Trim();
            //        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayView03.mName].Text = dt.Rows[i]["MName"].ToString().Trim();

            //        dt2 = Query.Get_BasBcode(pDbCon, "XRAY_JCLASS", dt.Rows[i]["GbMCode"].ToString().Trim(), " Code || '.' || Name CodeName ");
            //        if (ComFunc.isDataTableNull(dt2) == false)
            //        {
            //            Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayView03.mGubun].Text = clsComSup.setP(dt2.Rows[0]["CodeName"].ToString().Trim(), ".", 2).Trim();
            //        }

            //        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayView03.Qty].Text = dt.Rows[i]["Qty"].ToString().Trim();
            //        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayView03.Unit].Text = dt.Rows[i]["Unit"].ToString().Trim();

            //    }


            //}

            //dt.Dispose();
            //dt = null;


            #endregion


            //Cursor.Current = Cursors.Default;
            //

        }

    }
}
