using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB;
using ComSupLibB.SupFnEx;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupSTS01.cs
    /// Description     : 진료지원 종합 상태 조회 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-08-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    ///  frmComSupSTS01.cs 신규폼 생성
    /// </history>
    /// <seealso cref= " frmComSupSTS01.cs 폼이름 재정의" />
    public partial class frmComSupSTS01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        clsComSup csup = new clsComSup();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSup sup = new clsComSup();
        clsComSupXray cxray = new clsComSupXray();        
        clsComSupFnEx supfnex = new clsComSupFnEx();

        clsComSup.cBasPatient cBasPatient = null;
        //frmComSupXraySET20 frmComSupXraySET20x = null; // 조영제 바코드출력 폼         

        string gPano = "";
        string gDate = "";
        string gSelDate = "";
        string gPart = "";
        string gSelGubun = "";
        bool gViewTitle =false;
        bool gViewHead = false;
        bool gExit = false;

        

        Thread thread;
        FpSpread spd;



        #endregion

        public frmComSupSTS01(string argPano, string argDate, bool bViewTitle = true, bool bViewHead = true,bool bExit =true, string bPart = "")
        {
            InitializeComponent();

            gPano = argPano;
            gDate = argDate;
            gViewTitle = bViewTitle;
            gViewHead = bViewHead;
            gExit = bExit;
            gPart = bPart;

            setEvent();
        }
        public frmComSupSTS01(string argPano, string argDate, bool bViewTitle = true, bool bViewHead = true, bool bExit = true)
        {
            InitializeComponent();

            gPano = argPano;
            gDate = argDate;
            gViewTitle = bViewTitle;
            gViewHead = bViewHead;
            gExit = bExit;

            setEvent();
        }

        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            setCtrlUcSupComPt(0, 50);

            //frmComSupXraySET20x = new frmComSupXraySET20();

        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.Move += new EventHandler(eFormMove);            
            this.Resize += new EventHandler(eFormResize);

            this.btnExit.Click += new EventHandler(eBtnEvent);            
            this.btnSearch.Click += new EventHandler(eBtnEvent);

            this.ssList1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList1.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);

            //this.txtPano.KeyDown += new KeyEventHandler(eTxtKeyDown);

            this.chkYear.CheckedChanged += new EventHandler(eChkChanged);
            this.uc1.txtSearch_PtInfo.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.uc1.txtSearch_SName.TextChanged += new EventHandler(eTxtChange);
            this.uc1.txtSearch_PtInfo.TextChanged += new EventHandler(eTxtChange);

        }
        
        void setTxtTip()
        {
            //툴팁
            ssList1.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            ssList1.TextTipDelay = 500;
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
                supSpd.sSpd_TotResv(ssList1, supSpd.sSpdTotResv, supSpd.nSpdTotResv, 5, 0, gPart);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등           

                setAuth();

                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData();

                //tSearch.Text = gSearch;

                if (gPano != "")
                {
                    //txtPano.Text = gPano;
                    uc1.txtSearch_PtInfo.Text = gPano;
                    read_pano_info(clsDB.DbCon, gPano);
                }

                //
                screen_display();
            }           
        }

        void eFormMove(object sender, EventArgs e)
        {
            setCtrlUcSupComPt(0, 50);
        }

        void eFormResize(object sender, EventArgs e)
        {
            setCtrlUcSupComPt(0, 50);
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
                screen_display();
            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {       
            if (sender == this.uc1.txtSearch_PtInfo)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    screen_clear("A1");

                    string strSearch = uc1.txtSearch_PtInfo.Text.Trim();

                    if (VB.IsNumeric(strSearch))
                    {

                        cBasPatient = sup.sel_Bas_Patient_cls(clsDB.DbCon, strSearch);
                        txtJumin.Text = cBasPatient.JuminFull;
                        screen_display();
                    }

                }
            }

        }

        void eTxtChange(object sender,EventArgs e)
        {
            if (sender == this.uc1.txtSearch_PtInfo)
            {
                read_pano_info(clsDB.DbCon, uc1.txtSearch_PtInfo.Text.Trim());                
            }
            else if (sender == this.uc1.txtSearch_PtInfo)
            {
                read_pano_info(clsDB.DbCon, uc1.txtSearch_PtInfo.Text.Trim());
            }
        }

        void eChkChanged(object sender,EventArgs e)
        {
            screen_display();
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            string strSort = "";
            FpSpread s = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssList1)
            {
                strSort  = s.ActiveSheet.Cells[s.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmTotResv.Sort].Text.Trim();
                string strPano = s.ActiveSheet.Cells[s.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmTotResv.Pano].Text.Trim();
                string strUid = s.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmTotResv.Uid].Text.Trim();
                string strRWOID = s.ActiveSheet.Cells[s.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmTotResv.ROWID].Text.Trim();

                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(s, e.Column);
                    return;
                }

                if (e.Column == (int)clsComSupSpd.enmTotResv.STS)//이미지뷰어
                {
                    if (strSort == "5") //기능검사
                    {
                        #region //기능검사
                        string Jong =  clsComSup.setP(s.ActiveSheet.Cells[s.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmTotResv.Jong].Text.Trim(),".",1).Trim();
                        long WRTNO = Convert.ToInt32(s.ActiveSheet.Cells[s.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmTotResv.EMGWRTNO].Text.Trim());

                        if (s.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmTotResv.STS].Text == "▦")
                        {
                            if (Jong =="13" && WRTNO >0 )
                            {
                                one_img_display(clsDB.DbCon, strRWOID,WRTNO);
                            }
                            else
                            {
                                one_img_display(clsDB.DbCon, strRWOID);
                            }
                        }
                        #endregion
                    }
                    else if (strSort == "4") //내시경
                    {
                        #region //내시경
                        if (s.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmTotResv.STS].Text == "▦")
                        {
                            #region //PACS VIEW 연결
                                                        
                            sup.PACS_VIEW(strPano, strUid);

                            #endregion
                        }
                        #endregion
                    }
                    else if (strSort == "6") //영상검사
                    {
                        #region //영상검사
                        if (s.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmTotResv.STS].Text == "▦")
                        {
                            #region //PACS VIEW 연결

                            sup.PACS_VIEW(strPano, strUid);

                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        ComFunc.MsgBox(" 기능검사만 이미지 뷰어됩니다..");
                    }                    
                }

            }

        }

        void eTxtTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            string strPano = "";

            if (s.ActiveSheet.RowCount <= -1)
            {
                return;
            }

            if (e.RowHeader == true || e.Row < 0)
            {
                return;
            }

            
            try
            {

                strPano = s.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmTotResv.Pano].Text.Trim();

                if (e.Column == (int)clsComSupSpd.enmTotResv.STS)
                {

                }
                else if (e.Column == (int)clsComSupSpd.enmTotResv.SName)
                {
                    if(s.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmTotResv.Sort].Text.Trim() =="5") //기능검사면 참고사항
                    {
                        cBasPatient = sup.sel_Bas_Patient_cls(clsDB.DbCon, strPano);
                        e.TipText = "EKG메모 : " + cBasPatient.EkgMsg;
                        e.ShowTip = true;

                    }
                }
                else
                {
                    e.TipText = ssList1.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                    e.ShowTip = true;
                }
                
            }
            catch
            {

            }


        }

        void setAuth()
        {
            if (gViewTitle == false)
            {
                panTitleSub0.Visible = false;
            }
            if (gViewHead == false)
            {
                panPano.Enabled = false;
                panHead.Visible = false;
            }
            if (gExit == false)
            {
                btnExit.Visible = false;
            }
        }

        void setCtrlUcSupComPt(int addX=0,int addY =0)
        {
            Point p = new Point();

            p.X = this.Location.X +  this.uc1.Location.X + addX ;
            p.Y = this.Location.Y + this.panTitleSub0.Location.Y + this.uc1.Height * 7 + addY;

            this.uc1.pPSMH_LPoint = p;

        }

        void one_img_display(PsmhDb pDbCon, string argROWID,long argEMG =0)
        {
            string strGubun = "";
            string strRDate = "";
            string strImgGbn = "";

            DataTable dt = null;

            if (argEMG >0)
            {
                dt = sup.sel_Etc_Result(pDbCon, argEMG);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    supfnex.FnEx_display_EMG_file(dt,"1");                                                           
                }
            }
            else
            {
                dt = sup.sel_Etc_Jupmst(pDbCon, argROWID);

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (dt.Rows[0]["GbFTP"].ToString().Trim() == "Y")
                    {
                        strGubun = dt.Rows[0]["Gubun"].ToString().Trim();
                        strRDate = dt.Rows[0]["RDate"].ToString().Trim();
                        strImgGbn = dt.Rows[0]["IMAGE_GBN"].ToString().Trim();
                        if (strGubun == "6" || strGubun == "23")
                        {
                            strRDate = dt.Rows[0]["BDate"].ToString().Trim();
                        }
                        if (strGubun == "1" && strImgGbn != "09")
                        {
                            supfnex.FnEx_display_img_file("ECG", strRDate, dt.Rows[0]["FilePath"].ToString(), "1");
                        }
                        else if (strGubun == "18" || strGubun == "19" || strGubun == "20")
                        {
                            supfnex.FnEx_display_img_file("STRESS", strRDate, dt.Rows[0]["FilePath"].ToString(), "1");
                        }
                        else
                        {
                            supfnex.FnEx_display_img_file("JPG", strRDate, dt.Rows[0]["FilePath"].ToString(), "1");
                        }

                    }
                }

            }            
            
        }

        public void display_pano_view(string argPano, string argSelGubun="", string argSelDate ="")
        {            
            screen_clear();

            if (argPano != "")
            {
                read_pano_info(clsDB.DbCon, argPano);
                read_pano_info_uc();
                screen_display(argSelGubun,argSelDate);
            }
            
        }

        void read_pano_info(PsmhDb pDbCon, string argPano)
        {
            if (argPano =="")
            {
                return;
            }
            cBasPatient = sup.sel_Bas_Patient_cls(pDbCon, argPano);
                       
            //txtPano.Text = argPano;
            //txtSName.Text = cBasPatient.SName;
            txtJumin.Text = cBasPatient.JuminFull;  
        }

        void read_pano_info_uc()
        {
            uc1.txtSearch_PtInfo.Text = cBasPatient.Pano;
            uc1.txtSearch_SName.Text = cBasPatient.SName;

        }

        void F_frmComSupSTS01_Event()
        {
            screen_display();
        }

        void screen_clear(string Job ="")
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            if (Job =="ALL" || Job =="")
            {
                ssList1.ActiveSheet.RowCount = 0;

                //txtPano.Text = "";
                txtJumin.Text = "";
                //txtSName.Text = "";            
                uc1.txtSearch_PtInfo.Text = "";
                uc1.txtSearch_SName.Text = "";

            }
            else if (Job == "A1")
            {
                ssList1.ActiveSheet.RowCount = 0;
                txtJumin.Text = "";
            }            
        }

        void screen_display(string argSelGubun = "", string argSelDate = "")
        {
            gSelGubun = argSelGubun;
            gSelDate = argSelDate;
                            
            //GetData(ssList1,txtPano.Text.Trim());           
            GetData_th(ssList1);
                        
        }

        void GetData_th(FarPoint.Win.Spread.FpSpread Spd)
        {

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;
            
            spd = Spd;

            //스레드 시작
            thread = new Thread(tProcess);
            thread.Start();

            Cursor.Current = Cursors.Default;

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argPano)
        {

            int i = 0;
            DataTable dt = null;
            string strRDate = "";
            string strRDate2 = "";

            string strGubun = "";
            string strGubun4 = "";
            string strResult = "";

            Spd.ActiveSheet.RowCount = 0;           
                        
            Cursor.Current = Cursors.WaitCursor;

            dt = csup.sel_Tot_Resv(pDbCon, argPano,false, cpublic.strSysDate,"",chkYear.Checked);

            #region //데이터셋 읽어 자료 표시

            if (ComFunc.isDataTableNull(dt) == false)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    strRDate = dt.Rows[i]["RDATE"].ToString().Trim();
                    strRDate2 = Convert.ToDateTime(strRDate).ToShortDateString();
                    strGubun4 = dt.Rows[i]["GUBUN4"].ToString().Trim();
                    strGubun = dt.Rows[i]["Sort"].ToString().Trim();
                    strResult = dt.Rows[i]["Gbn5"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Sort].Text = strGubun;
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Part].Text = dt.Rows[i]["Gbn"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.STS].Text = "";


                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RDate].Text = strRDate;
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.DrCode].Text = clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Jong].Text = dt.Rows[i]["Gbn2"].ToString().Trim();
                                        
                   
                    if (dt.Rows[i]["Sort"].ToString().Trim() == "1" || dt.Rows[i]["Sort"].ToString().Trim() == "2" || dt.Rows[i]["Sort"].ToString().Trim() == "3")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Code].Text = dt.Rows[i]["Gbn3"].ToString().Trim();
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = dt.Rows[i]["Gbn3"].ToString().Trim();
                    }
                    else if (dt.Rows[i]["Sort"].ToString().Trim() == "6" )
                    {
                        if (dt.Rows[i]["Gbn3"].ToString().Trim() != "")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Code].Text = dt.Rows[i]["Gbn3"].ToString().Trim();
                            Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = cxray.OCS_XNAME_READ(pDbCon, dt.Rows[i]["Gbn3"].ToString().Trim(),false);
                        }
                        else
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Code].Text = dt.Rows[i]["Gbn4"].ToString().Trim();
                            Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = cxray.BAS_SUN_READ(pDbCon, dt.Rows[i]["Gbn4"].ToString().Trim());
                        }
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Code].Text = dt.Rows[i]["Gbn3"].ToString().Trim();
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = cxray.OCS_XNAME_READ(pDbCon, dt.Rows[i]["Gbn3"].ToString().Trim(),false);
                    }

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "";
                    if (cpublic.strSysDate.CompareTo(strRDate2) > 0 || strGubun4 == "2")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "부도";
                    }
                    else if (cpublic.strSysDate.CompareTo(strRDate2) == 0)
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "당일";
                    }

                    if (strGubun == "4") //내시경
                    {
                        if (strResult =="7")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "완료";
                            if (dt.Rows[i]["Gbn6"].ToString().Trim() != "")
                            {
                                Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.STS].Text = "▦";
                            }
                        }

                    }
                    else if (strGubun == "5") //기능
                    {
                        if (strResult == "3")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "완료";
                            if (dt.Rows[i]["Gbn6"].ToString().Trim()=="Y")
                            {
                                Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.STS].Text = "▦";
                            }
                            
                        }
                    }
                    if (strGubun == "6") //영상
                    {
                        if (strResult == "7")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "완료";
                            if (dt.Rows[i]["Gbn6"].ToString().Trim() != "")
                            {
                                Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.STS].Text = "▦";
                            }                                
                        
                        }
                    }


                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Remark].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
            }


            

            Cursor.Current = Cursors.Default;

            #endregion
                        
            

        }

        #region //데이타내용 스프레드 표시 스레드 적용

        void tProcess()
        {

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.ssList1.BeginInvoke(new System.Action(() => this.ssList1.Enabled = false));
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            DataTable dt = null;
            if(gPart == "0")
            {
                dt = csup.sel_Tot_Resv(clsDB.DbCon, uc1.txtSearch_PtInfo.Text.Trim(), false, cpublic.strSysDate, "", chkYear.Checked, "", "", gPart);
            }
            else
            {
                dt = csup.sel_Tot_Resv(clsDB.DbCon, uc1.txtSearch_PtInfo.Text.Trim(), false, cpublic.strSysDate, "", chkYear.Checked);
            }

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));
            this.ssList1.BeginInvoke(new System.Action(() => this.ssList1.Enabled = true));
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

        }

        delegate void threadSpdTypeDelegate(FarPoint.Win.Spread.FpSpread spd, DataTable dt);
        void tShowSpread(FarPoint.Win.Spread.FpSpread spd, DataTable dt)
        {
            int i = 0;            
            string strRDate = "";
            string strRDate2 = "";

            string strGubun = "";
            string strGubun4 = "";
            string strResult = "";

            spd.ActiveSheet.RowCount = 0;

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                
                spd.ActiveSheet.RowCount = dt.Rows.Count;
                spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    strRDate = dt.Rows[i]["RDATE"].ToString().Trim();
                    strRDate2 = Convert.ToDateTime(strRDate).ToShortDateString();
                    strGubun4 = dt.Rows[i]["GUBUN4"].ToString().Trim();
                    strGubun = dt.Rows[i]["Sort"].ToString().Trim();
                    strResult = dt.Rows[i]["Gbn5"].ToString().Trim();

                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Sort].Text = strGubun;
                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Part].Text = dt.Rows[i]["Gbn"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.STS].Text = "";


                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RDate].Text = strRDate;
                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    //spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.DrCode].Text = clsVbfunc.GetBASDoctorName(dt.Rows[i]["DrCode"].ToString().Trim());
                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.DrCode].Text = dt.Rows[i]["FC_DrName"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Jong].Text = dt.Rows[i]["Gbn2"].ToString().Trim();
                    if (dt.Rows[i]["Gbn2"].ToString().Trim()==".")
                    {
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Jong].Text = "";
                    }


                    if (dt.Rows[i]["Sort"].ToString().Trim() == "1" || dt.Rows[i]["Sort"].ToString().Trim() == "2" || dt.Rows[i]["Sort"].ToString().Trim() == "3")
                    {
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Code].Text = dt.Rows[i]["Gbn3"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = dt.Rows[i]["Gbn3"].ToString().Trim();
                    }
                    else if (dt.Rows[i]["Sort"].ToString().Trim() == "6")
                    {
                        if (dt.Rows[i]["Gbn3"].ToString().Trim() != "")
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Code].Text = dt.Rows[i]["Gbn3"].ToString().Trim();

                            //2019-06-07 안정수, 추가
                            if (dt.Rows[i]["GBN7"].ToString().Trim() != "")
                            {
                                spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = cxray.OCS_XNAME_READ(clsDB.DbCon, dt.Rows[i]["Gbn3"].ToString().Trim(), false) + " " + dt.Rows[i]["GBN7"].ToString().Trim();
                            }
                            else
                            {
                                spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = cxray.OCS_XNAME_READ(clsDB.DbCon, dt.Rows[i]["Gbn3"].ToString().Trim(), false);
                            }                            
                        }
                        else 
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Code].Text = dt.Rows[i]["Gbn4"].ToString().Trim();

                            //2019-06-07 안정수, 추가
                            if (dt.Rows[i]["GBN7"].ToString().Trim() != "")
                            {
                                spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = cxray.BAS_SUN_READ(clsDB.DbCon, dt.Rows[i]["Gbn4"].ToString().Trim()) + " " + dt.Rows[i]["GBN7"].ToString().Trim();
                            }
                            else
                            {
                                spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = cxray.BAS_SUN_READ(clsDB.DbCon, dt.Rows[i]["Gbn4"].ToString().Trim());
                            }
                            //spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = cxray.BAS_SUN_READ(clsDB.DbCon, dt.Rows[i]["Gbn4"].ToString().Trim());
                        }
                    }
                    else
                    {
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Code].Text = dt.Rows[i]["Gbn3"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Name].Text = cxray.OCS_XNAME_READ(clsDB.DbCon, dt.Rows[i]["Gbn3"].ToString().Trim(),false);
                    }

                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "";
                    if (cpublic.strSysDate.CompareTo(strRDate2) > 0 || strGubun4 == "2")
                    {
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "부도";
                    }
                    else if (cpublic.strSysDate.CompareTo(strRDate2) == 0)
                    {
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "당일";
                    }

                    if (strGubun == "4") //내시경
                    {
                        if (strResult == "7")
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "완료";
                            if (dt.Rows[i]["Gbn6"].ToString().Trim() != "" && dt.Rows[i]["Gbn7"].ToString().Trim() != "")
                            {
                                spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.STS].Text = "▦";
                                spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Uid].Text = dt.Rows[i]["Gbn6"].ToString().Trim();
                            }
                        }

                    }
                    else if (strGubun == "5") //기능
                    {
                        if (strResult == "3")
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "완료";
                            if (dt.Rows[i]["Gbn6"].ToString().Trim() == "Y" || (dt.Rows[i]["Gbn7"].ToString().Trim() != "0" && dt.Rows[i]["Gbn7"].ToString().Trim() != ""))
                            {
                                spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.STS].Text = "▦";
                            }

                        }
                    }
                    if (strGubun == "6") //영상
                    {
                        if (strResult == "7")
                        {
                            //2019-04-05 안정수, 당일촬영 구분자에 대해 접수로 표기하고, 실제 촬영이 완료된후에 완료표기되도록 보완 
                            //spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "완료";
                            spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "접수";
                            if (dt.Rows[i]["Gbn6"].ToString().Trim() != "")
                            {
                                spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.STS].Text = "▦";
                                spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Uid].Text = dt.Rows[i]["Gbn6"].ToString().Trim();
                                
                                //2019-04-05 안정수 추가
                                spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.RSTS].Text = "완료";
                            }
                        }

                        ////2019-10-16 안정수 추가                           
                        //frmComSupXraySET20x.Read_Patient_info(clsDB.DbCon, dt.Rows[i]["ROWID"].ToString().Trim());  //조영제출력
                    }

                    //if(gPart == "0")
                    //{
                        spd.ActiveSheet.Columns[(int)clsComSupSpd.enmTotResv.M].Visible = true;
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.M].Text = dt.Rows[i]["Gbn8"].ToString().Trim();
                    //}
  
                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.Remark].Text = "";
                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.EMGWRTNO].Text = dt.Rows[i]["Gbn7"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmTotResv.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();


                }

                // 화면상의 정렬표시 Clear
                spd.ActiveSheet.ColumnHeader.Cells[0,0,0,spd.ActiveSheet.ColumnCount-1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }

            dt.Dispose();
            dt = null;                        

        }

        delegate void threadProcessDelegate(bool b);
        void trunCircular(bool b)
        {
            //this.Progress.Visible = b;
            //this.Progress.IsRunning = b;
        }


        #endregion

    }
}
