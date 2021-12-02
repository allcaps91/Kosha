using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
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
    /// File Name       : frmComSupXrayHELP01.cs
    /// Description     : 영상의학과 기초코드관리
    /// Author          : 윤조연
    /// Create Date     : 2017-07-07
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xucode\xucode07.frm(FrmBuseSearch) >> frmComSupXrayHELP01.cs 폼이름 재정의" />
    public partial class frmComSupXrayHELP01 : Form
    {
        #region 클래스 선언 및 etc....

        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();

        //기초코드의 부서코드 ,부서이름을 넘길때 사용
        public delegate void SendMsg(string strMsg);
        public event SendMsg rSendMsg;

        #endregion

        public frmComSupXrayHELP01()
        {
            InitializeComponent();

            setEvent();
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

            string strMsg = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCodeHelp.BuCode].Text.Trim() + "." + ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCodeHelp.Buse].Text.Trim();
            
            rSendMsg(strMsg);

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
                xraySpd.sSpd_XrayCodeHelp(ssList, xraySpd.sSpdXrayCodeHelp, xraySpd.nSpdXrayCodeHelp, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                
                //
                GetData(clsDB.DbCon, ssList);
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
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
            ssList.Enabled = false;
            
            dt = xraySql.sel_Bas_Buse(pDbCon, "",txtName.Text.Trim());

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for ( i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeHelp.BuCode].Text = dt.Rows[i]["BUCODE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeHelp.Buse].Text = dt.Rows[i]["NAME"].ToString().Trim();
                }
            }
            

            #endregion

            Cursor.Current = Cursors.Default;
            ssList.Enabled = true;

        }

    }
}
