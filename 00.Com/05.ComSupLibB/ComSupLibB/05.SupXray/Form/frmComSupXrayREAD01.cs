using System;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Threading;
using System.Data;
using ComSupLibB.Com;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayREAD01.cs
    /// Description     : 영상의학과 촬영일자별 판독명단 공통
    /// Author          : 윤조연
    /// Create Date     : 2017-06-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 프로젝트별 판독 명단폼 frmComSupXrayREAD01.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\mir\miretc\miretc41.frm(FrmListView) >> frmComSupXrayREAD01.cs 폼이름 재정의" />
    public partial class frmComSupXrayREAD01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsComSup sup = new clsComSup();
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXray.SupXrayBase cxraybase = new clsComSupXray.SupXrayBase();
        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupSpd ccomspd = new clsComSupSpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();
        clsComSupXrayRead cRead = new clsComSupXrayRead();

        clsComSupXrayRead.cXrayViewList cXrayViewList = null;

        FarPoint.Win.Spread.FpSpread spd;
        Thread thread;

        #endregion

        public frmComSupXrayREAD01()
        {
            InitializeComponent();

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

          
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
            
        }

        //권한체크
        void setAuth()
        {
            
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


            this.optJob1.Click += new EventHandler(eOptEvent);
            this.optJob2.Click += new EventHandler(eOptEvent);

            this.cboDept.SelectedIndexChanged += new EventHandler(eCboEvent);
            
           
        }
        
        void eCboEvent(object sender, EventArgs e)
        {
            DataTable dt = sup.sel_Bas_Doctor_ComBo(clsDB.DbCon, VB.Left(this.cboDept.SelectedItem.ToString(),2));

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboDoct, dt, clsParam.enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }
        }

        void setCombo(PsmhDb pDbCon)
        {
            //XRAY_방사선종류
            DataTable dt = comSql.sel_BAS_BCODE_COMBO(pDbCon, "XRAY_방사선종류");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboJong, dt, clsParam.enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }

            dt = sup.sel_Bas_ClinicDept_ComBo(pDbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboDept, dt, clsParam.enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }

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
                xraySpd.sSpd_XrayViewList(ssList, xraySpd.sSpdXrayViewList, xraySpd.nSpdXrayViewList, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                screen_clear();

                setCtrlData();

                //
                setCombo(clsDB.DbCon);
                
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
                //
                GetData_th(ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
            }
            else if (sender == this.btnPrint)
            {
                //
                ePrint();
            }

        }

        void eOptEvent(object sender, EventArgs e)
        {
            
        }
        
        void ePrint()
        {
            string[] strhead = new string[2];
            string[] strfont = new string[2];

            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            strfont[0] = "/fn\"바탕체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strfont[1] = "/fn\"바탕체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strhead[0] = "/c/f1/n" + "촬영일자별 판독자 명단" + "/f1/n";
            strhead[1] = "/n/l/f2" + "조회기간 : " + dtpFDate.Text.Trim() + "~" + dtpTDate.Text.Trim() + " /l/f2" + "  출력시간 : " + cpublic.strSysDate + " " + cpublic.strSysTime + " /n";


            ccomspd.SPREAD_PRINT(ssList_Sheet1, ssList, strhead, strfont, 10, 10, 2, true);
        }       

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            //txtSearch.Text = "";   
            //dtpFDate.Text =cpublic.strSysDate;  

        }

        //스레드 데이타 조회
        void GetData_th(FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate)
        {
            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            #region //쿼리 변수세팅

            cXrayViewList = new clsComSupXrayRead.cXrayViewList();
            cXrayViewList.optJob1 = optJob1.Checked.ToString();
            cXrayViewList.SDate = argSDate;
            cXrayViewList.TDate = argTDate;
            cXrayViewList.XJong = VB.Left(cboJong.SelectedItem.ToString(), 1);

            cXrayViewList.Pano = "";
            cXrayViewList.DeptCode = VB.Left(cboDept.SelectedItem.ToString(), 2);
            cXrayViewList.DrCode = VB.Left(cboDoct.SelectedItem.ToString(), 4);
            cXrayViewList.SName = "";
            
            spd = Spd;

            #endregion

            //스레드 시작
            thread = new Thread(tProcess);
            thread.Start();

            Cursor.Current = Cursors.Default;

        }

        #region //데이타내용 스프레드 표시 스레드 적용

        void tProcess()
        {

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            DataTable dt = null;
            dt = cRead.sel_XrayDetailView(clsDB.DbCon, cXrayViewList);

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);
            
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });

        }

        delegate void threadSpdTypeDelegate(FarPoint.Win.Spread.FpSpread spd, DataTable dt);
        void tShowSpread(FarPoint.Win.Spread.FpSpread spd, DataTable dt)
        {
            string strOK = "";            
            int nRow = -1;            

            spd.ActiveSheet.RowCount = 0;
            
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {

                spd.ActiveSheet.RowCount = dt.Rows.Count;
                spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "OK";                                        

                    if (strOK == "OK")
                    {
                        nRow++;

                        if (spd.ActiveSheet.RowCount <= nRow) spd.ActiveSheet.RowCount = nRow + 1;

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.check01].Text = "";                                                

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.SeekDate].Text = dt.Rows[i]["SeekDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.DrName].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.GbIO].Text = dt.Rows[i]["IpdOpd"].ToString().Trim() == "I" ? "입원" : "외래";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.Ward].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.Room].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.ExCode].Text = dt.Rows[i]["XCode"].ToString().Trim();

                        if (cXrayViewList.optJob1=="True")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.ExName].Text = cxray.OCS_XNAME_READ(clsDB.DbCon, dt.Rows[i]["OrderCode"].ToString().Trim(),false, true) + " " + dt.Rows[i]["Remark"].ToString().Trim();
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.ExName].Text =  dt.Rows[i]["XName"].ToString().Trim();
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.result].Text = dt.Rows[i]["Result"].ToString().Trim();
                        }
                        
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.Exid].Text = dt.Rows[i]["Exid"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.Age].Text = dt.Rows[i]["Age"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayViewList.XJong].Text = dt.Rows[i]["XJong"].ToString().Trim();
                                                
                    }
                    
                }
            }

            dt.Dispose();
            dt = null;

            spd.ActiveSheet.RowCount = nRow + 1;

        }

        delegate void threadProcessDelegate(bool b);
        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }


        #endregion
    }
}
