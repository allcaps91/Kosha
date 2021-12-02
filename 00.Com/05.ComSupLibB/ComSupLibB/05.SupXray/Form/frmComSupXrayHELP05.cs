using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayHELP05.cs
    /// Description     : 영상의학과 환자성명 검색
    /// Author          : 윤조연
    /// Create Date     : 2018-04-12    
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xumain\xumain28.frm(FrmPanoSearch) >> frmComSupXrayHELP05.cs 폼이름 재정의" />
    public partial class frmComSupXrayHELP05 : Form
    {
        #region 클래스 선언 및 etc....

        clsComSup sup = new clsComSup();
        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();
               

        #endregion

        #region //델리게이트관련

        //델리게이트
        public delegate void SendMsg(clsComSupXray.cXray_Patient argCls);
        public event SendMsg rSendMsg;

        clsComSupXray.cXray_Patient cXray_Patient = null;

        #endregion

        public frmComSupXrayHELP05()
        {
            InitializeComponent();
            setEvent();
        }

        void setCtrlData()
        {
            cXray_Patient = new clsComSupXray.cXray_Patient();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.txtSName.KeyDown += new KeyEventHandler(eTxtKeyDown);

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
                xraySpd.sSpd_enmXrayHELP05(ssList, xraySpd.sSpdenmXrayHELP05, xraySpd.nSpdenmXrayHELP05, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();
                //
                //screen_display();
                screen_clear();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                cXray_Patient.STS = "";
                this.Close();
                return;
            }  
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eBtnPrint(object sender, EventArgs e)
        {

        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtSName)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    screen_display();
                }
            }

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            FpSpread o = (FpSpread)sender;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column);
                    return;
                }
                else
                {
                    cXray_Patient.STS = "OK";
                    cXray_Patient.Pano = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayHELP05.Pano].Text.Trim();
                    cXray_Patient.GbIO ="외래";
                    if (optGubun_Ipd.Checked == true)
                    {
                        cXray_Patient.GbIO = "입원";
                    }

                    rSendMsg(cXray_Patient);

                    this.Close();
                }
            }

                

        }

        void screen_clear()
        {
            txtSName.Text = "";
            txtSName.Select();
        }
        
        void screen_display()
        {
            string s = txtSName.Text.Trim();

            if (s == "")
            {
                ComFunc.MsgBox("성명을 넣고 조회하십시오!!");
                txtSName.Select();
                return;
            }

            string strJob = "";

            if (optGubun_Opd.Checked == true)
            {
                strJob = "SUB+OPD";
            }
            else if (optGubun_Ipd.Checked == true)
            {
                strJob = "SUB+IPD";
            }
            else
            {
                strJob = "03"; //이름검색
            }                       

            GetData(clsDB.DbCon, ssList,strJob,s);
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,string argJob,string argSearch)
        {
            int i = 0;

            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            ssList.Enabled = false;

            dt = sup.sel_Xray_Patient(pDbCon, argJob, argSearch);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayHELP05.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayHELP05.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayHELP05.Jumin].Text = dt.Rows[i]["JuminFULL"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayHELP05.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayHELP05.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();                 
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayHELP05.DrName].Text = dt.Rows[i]["FC_DrName"].ToString().Trim();                   
                }
            }


            #endregion

            Cursor.Current = Cursors.Default;
            ssList.Enabled = true;

        }

    }

}
