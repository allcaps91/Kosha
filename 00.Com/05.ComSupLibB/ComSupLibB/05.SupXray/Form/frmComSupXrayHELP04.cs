using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayHELP04.cs
    /// Description     : 영상의학과 수동접수시 부위선택상세 폼
    /// Author          : 윤조연
    /// Create Date     : 2018-01-04
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xumain\xumain30.frm (FrmViewBoth) >> frmComSupXrayHELP04.cs 폼이름 재정의" />
    public partial class frmComSupXrayHELP04 : Form
    {        
        #region 클래스 선언 및 etc....

        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySQL = new clsComSupXraySQL();
        
        string gOrderCode = "";

        #endregion

        #region //델리게이트관련

        //델리게이트
        public delegate void SendMsg(clsComSupXray.cOCS_SubCode argCls);
        public event SendMsg rSendMsg;

        clsComSupXray.cOCS_SubCode cOCS_SubCode = null;

        #endregion

        public frmComSupXrayHELP04(string argOrderCode)
        {
            InitializeComponent();
            gOrderCode = argOrderCode;
            setEvent();
        }

        void setCtrlData()
        {            
            cOCS_SubCode = new clsComSupXray.cOCS_SubCode();            
        }


        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSearch.Click += new EventHandler(eBtnEvent);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {                        

            if (e.Row < 0 || e.Column < 0) return;

            cOCS_SubCode.STS = "OK";
            cOCS_SubCode.SuCode = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayOrdSubCode.SuCode].Text.Trim();
            cOCS_SubCode.SubName = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayOrdSubCode.SubName].Text.Trim();
            cOCS_SubCode.ROWID = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayOrdSubCode.ROWID].Text.Trim();

            rSendMsg(cOCS_SubCode);

            this.Close();

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
                cxraySpd.sSpd_XrayOrdSubCode(ssList, cxraySpd.sSpdXrayOrdSubCode, cxraySpd.nSpdXrayOrdSubCode, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();

                //
                GetData(clsDB.DbCon, ssList);

                
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                cOCS_SubCode.STS = "";
                this.Close();
                return;
            }
            else if (sender == this.btnSearch)
            {
                GetData(clsDB.DbCon, ssList);
            }

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {
            int i = 0;

            DataTable dt = null;
            

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            dt = cxraySQL.sel_OCS_SUBCODE(pDbCon, gOrderCode);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdSubCode.SuCode].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdSubCode.SubName].Text = dt.Rows[i]["SubName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdSubCode.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }
            }


            #endregion

            Cursor.Current = Cursors.Default;

        }

    }
}
