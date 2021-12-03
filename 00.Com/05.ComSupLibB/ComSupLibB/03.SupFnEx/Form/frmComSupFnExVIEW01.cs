using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupFnEx
    /// File Name       : frmComSupFnExVIEW01.cs
    /// Description     : 기능검사 등록번호로 컨설트 내용
    /// Author          : 윤조연
    /// Create Date     : 2017-06-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// frmComSupFnExViewConsult.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\Ocs\ekg\FrmConsultList.frm(FrmConsultList) >> frmComSupFnExVIEW01.cs 폼이름 재정의" />
    public partial class frmComSupFnExVIEW01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();
        clsComSupFnExSpd fnexSpd = new clsComSupFnExSpd();      
        clsComSupFnExSQL fnExSql = new clsComSupFnExSQL();

        clsComSup.cBasPatient cBasPatient = null;
        
        string gstrPano = "";
        string gstrInDate = "";
        string gstrIO = "";
        string gstrDept = "";
        string gManual = "";



        #endregion

        public frmComSupFnExVIEW01(PsmhDb pDbCon, string argIO, string argPano, string argIDate, string argDept,string argManul)
        {
            InitializeComponent();

            setPanoInfo(pDbCon, argIO, argPano, argIDate, argDept, argManul);                       

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
            //this.btnSearch1.Click += new EventHandler(eBtnEvent);

            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);            

            this.txtPano.KeyDown += new KeyEventHandler(eTxtKeyDown);


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
                fnexSpd.sSpd_Consultview(ssList, fnexSpd.sSpdConsultview, fnexSpd.nSpdConsultview, 1, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();

                screen_clear();

                //
                screen_display();
            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtResult.Text = "";
                    read_pano_info( clsDB.DbCon, ComFunc.SetAutoZero(txtPano.Text.Trim(), ComNum.LENPTNO));
                }
            }
                  
        }

        void setPanoInfo(PsmhDb pDbCon, string argIO, string argPano, string argIDate, string argDept, string argManul)
        {

            gstrPano = argPano;
            gstrInDate = argIDate;
            gstrIO = argIO;
            gstrDept = argDept;

            if (argManul =="Y")
            {
                gManual = "Y";
            }

            
            DataTable dt = fnExSql.sel_ipdNewMaster_indate(pDbCon, argPano, argIDate);
            if (dt != null && dt.Rows.Count > 0)
            {
                gstrInDate = dt.Rows[0]["InDate"].ToString().Trim();
            }
            dt = null;

            if (gManual=="Y")
            {
                lblinfo.Text = "환자명/등록번호 : " + gstrPano + "/" + clsVbfunc.GetPatientName(pDbCon, gstrPano) + "   전체기간";
            }
            else
            {
                lblinfo.Text = "환자명/등록번호 : " + gstrPano + "/" + clsVbfunc.GetPatientName(pDbCon, gstrPano) + "   내원(입원)일자 : " + gstrInDate;
            }
            
        }

        void read_pano_info(PsmhDb pDbCon, string argPano)
        {
            string strDate = "";
            cBasPatient = sup.sel_Bas_Patient_cls(pDbCon, argPano);

            txtPano.Text = argPano;
            txtSName.Text = cBasPatient.SName;
            txtJumin.Text = cBasPatient.JuminFull;
            if (gstrInDate =="")
            {
                gstrInDate = cpublic.strSysDate;
            }
            strDate = gstrInDate;
            gstrInDate =   Convert.ToDateTime(strDate).AddYears(-1).ToShortDateString();

            gstrIO = "";
            gManual = "Y";
            setPanoInfo(pDbCon, gstrIO, argPano, gstrInDate, "", gManual);

            screen_display();

        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            string strConsult = "";

            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }
            
            for (int i = 0; i < o.ActiveSheet.RowCount; i++)
            {
                o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
            }
            
            o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGray;
           
            txtResult.Text = "";

            strConsult = "=============================================================== \r\n";

            if (ssList.ActiveSheet.Cells[e.Row,(int)clsComSupFnExSpd.enmConsultview.ErSMS].Text.Trim()== "응급")
            {
                strConsult += " \r\n";
                strConsult += "     ■ 응급        □ 비응급     \r\n";                
            }
            else
            {
                strConsult += " \r\n";
                strConsult += "     □ 응급        ■ 비응급     \r\n";                
            }
            if (ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.WorkSTS].Text.Trim() == "불가")
            {                
                strConsult += "     □ 거동        ■ 거동불가     \r\n";
                strConsult += " \r\n";
            }
            else
            {                
                strConsult += "     ■ 거동        □ 거동불가     \r\n";
                strConsult += " \r\n";                
            }

            strConsult += "▶ 처 방 일 : " + ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.BDate].Text.Trim() +" \r\n";
            strConsult += "▶ 의뢰일시 : " + ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.sDate].Text.Trim() + " \r\n";
            strConsult += "▶ 의 뢰 과 : " + ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.FrDept].Text.Trim() +" ( " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.FrDept].Text.Trim()) + " ) " + " \r\n";
            strConsult += "▶ 의 뢰 의 : " + ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.cDrName].Text.Trim() + " \r\n";
            strConsult += "=============================================================== \r\n";
            strConsult += "▶ 의뢰내용 : \r\n";
            strConsult += "=============================================================== \r\n";
            strConsult +=  ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.FrRemark].Text.Trim() + " \r\n\r\n\r\n";
            strConsult += "=============================================================== \r\n";
            strConsult += "▶ 결과 일시 : " + ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.eDate].Text.Trim() + " \r\n";
            strConsult += "▶ Consult과 : " + ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.ToDept].Text.Trim() + " ( " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.ToDept].Text.Trim()) + " ) " + " \r\n";
            strConsult += "▶ Consult의 : " + ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.dDrName].Text.Trim() + " \r\n";
            strConsult += "=============================================================== \r\n";
            strConsult += "▶ 결과 내용 : \r\n";
            strConsult += "=============================================================== \r\n";
            strConsult += ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmConsultview.ToRemark].Text.Trim() + " \r\n";
            strConsult += "=============================================================== \r\n";


            txtResult.Text = strConsult;

            

                     
        }
        
        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            

        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList, gstrIO, gstrPano, gstrInDate, gstrDept);
        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

          
        }

        void GetData(PsmhDb pDbCon,FarPoint.Win.Spread.FpSpread Spd, string GbIO, string argPano, string argInDate,string argDept)
        {

            int i = 0;            
            DataTable dt = null;

            
            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            //쿼리실행      
            dt = fnExSql.sel_consultView( pDbCon, GbIO, argPano, argInDate,argDept,gManual);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {                          
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.FrDept].Text = dt.Rows[i]["FRDEPTCODE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.cDrName].Text = dt.Rows[i]["CDRNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.InpDate].Text = dt.Rows[i]["INPDATE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.ToDept].Text = dt.Rows[i]["TODEPTCODE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.dDrName].Text = dt.Rows[i]["DDRNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.FrDrCode].Text = dt.Rows[i]["FRDRCODE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.ToDrCode].Text = dt.Rows[i]["TODRCODE"].ToString().Trim();                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.FrRemark].Text = dt.Rows[i]["FRREMARK"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.sDate].Text = dt.Rows[i]["sDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.eDate].Text = dt.Rows[i]["edate"].ToString().Trim();                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.ToRemark].Text = dt.Rows[i]["TOREMARK"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.EntDate].Text = dt.Rows[i]["EntDate"].ToString().Trim();                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.ErSMS].Text = dt.Rows[i]["GbEMSMS"].ToString().Trim();
                    if (dt.Rows[i]["GbSTS"].ToString().Trim()=="1")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.WorkSTS].Text = "불가";
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.WorkSTS].Text = "가능";
                    }

                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmConsultview.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    

                }
            }
            
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            //자료표시
            
            #endregion


        }
                
        
        
    }
}
