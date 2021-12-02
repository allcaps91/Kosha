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
    /// File Name       : frmComSupXrayHELP02.cs
    /// Description     : 영상의학과 오더판넬 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-07-11
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xumain\xumain29.frm ,FrmViewSlipSub_Code.frm (FrmViewSlipSub,FrmViewSlipSub_Code) >> frmComSupXrayHELP02.cs 폼이름 재정의" />
    public partial class frmComSupXrayHELP02 : Form
    {
        #region 클래스 선언 및 etc....

        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
               
        string gSlipNo = "";
        string gSubRate = "";

        #endregion

        #region //델리게이트관련

        //델리게이트
        public delegate void SendMsg(clsComSupXray.cOCS_OrderCode argCls);
        public event SendMsg rSendMsg;

        clsComSupXray.cOCS_OrderCode cOCS_OrderCode = null;

        #endregion

        public frmComSupXrayHELP02(string slipNO,string subRate)
        {
            InitializeComponent();

            gSlipNo = slipNO;
            gSubRate = subRate;

            setEvent();
        }

        void setCtrlData()
        {
            cOCS_OrderCode = new clsComSupXray.cOCS_OrderCode();
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

            cOCS_OrderCode.STS = "OK";
            cOCS_OrderCode.ORDERCODE = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayOrdCode.OrderCode].Text.Trim();
            cOCS_OrderCode.SUCODE = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayOrdCode.SuCode].Text.Trim();
            cOCS_OrderCode.ORDERNAME = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayOrdCode.OrderName].Text.Trim();
            cOCS_OrderCode.GBINFO = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayOrdCode.GbInfo].Text.Trim();
            cOCS_OrderCode.GBINPUT = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayOrdCode.GbInput].Text.Trim();
            cOCS_OrderCode.ROWID = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayOrdCode.ROWID].Text.Trim();

            rSendMsg(cOCS_OrderCode);

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
                cxraySpd.sSpd_XrayOrdCode(ssList, cxraySpd.sSpdXrayOrdCode, cxraySpd.nSpdXrayOrdCode, 5, 0);

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
                cOCS_OrderCode.STS = "";
                this.Close();
                return;
            }
            else if (sender == this.btnSearch)
            {
                GetData(clsDB.DbCon, ssList);
            }

        }
        
        void GetData(PsmhDb pDbCon,FarPoint.Win.Spread.FpSpread Spd)
        {
            int i = 0;

            DataTable dt = null;
            string strUnit = "";
            //int nDispSpace = 0;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
           
            dt = cxraySql.sel_OrderCode(pDbCon, gSlipNo,gSubRate);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strUnit = dt.Rows[i]["OrderName"].ToString().Trim();
                    //nDispSpace = Convert.ToInt32(dt.Rows[i]["DispSpace"].ToString().Trim());

                    if (dt.Rows[i]["GbInfo"].ToString().Trim()=="1")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.STS].Text = "▲" ;
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.STS].Text = "";
                    }

                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.GbInput].Text = dt.Rows[i]["GbInput"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.GbInfo].Text = dt.Rows[i]["GbInfo"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.GbBoth].Text = dt.Rows[i]["GbBoth"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.Bun].Text = dt.Rows[i]["Bun"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.NextCode].Text = dt.Rows[i]["NextCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.SuCode].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.GbDosage].Text = dt.Rows[i]["GbDosage"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.SpecCode].Text = dt.Rows[i]["SpecCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.Slipno].Text = dt.Rows[i]["Slipno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.GbImiv].Text = dt.Rows[i]["GbImiv"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayOrdCode.SubRate].Text = dt.Rows[i]["SubRate"].ToString().Trim();
                    

                }
            }


            #endregion

            Cursor.Current = Cursors.Default;
            
        }

    }
}
