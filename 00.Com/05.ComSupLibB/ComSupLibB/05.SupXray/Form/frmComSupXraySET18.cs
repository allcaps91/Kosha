using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET18.cs
    /// Description     : 영상의학과 판독프로그램에 사용되는 정렬선택폼
    /// Author          : 윤조연
    /// Create Date     : 2018-05-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "신규폼 frmComSupXraySET18.cs 폼이름 재정의" />
    public partial class frmComSupXraySET18 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSup sup = new clsComSup();        
        clsComSupSpd supSpd = new clsComSupSpd();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();
        clsComSupXray cxray = new clsComSupXray();
        
        clsComSupXray.enm_XraySort XSort = clsComSupXray.enm_XraySort.NULL;       

        #endregion

        public frmComSupXraySET18()
        {
            InitializeComponent();
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //
            XSort = cxray.read_xrayList_Sort(clsDB.DbCon, clsType.User.IdNumber);

            setSort();
        }

        //권한체크
        void setAuth()
        {

        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);            
            //this.btnCancel.Click += new EventHandler(eBtnClick);

            //this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnSet.Click += new EventHandler(eBtnSave);            
            //this.btnDelete.Click += new EventHandler(eBtnSave);

            //this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            //this.txtSearch.KeyDown += new KeyEventHandler(eTxtEvent);
            //this.txtResult.KeyUp += new KeyEventHandler(eTxtKeyUp);
            //this.txtJepCode.KeyDown += new KeyEventHandler(eTxtEvent);

            //this.cboJong.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            

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
                
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                screen_clear();

                setCtrlData();

                setAuth();

                //
                screen_display();
                //search_data(ssList,"");
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }   
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            //if (sender == this.btnSearch)
            //{
            //    screen_display();
            //    screen_clear("VIEW");
            //}
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSet)
            {
                //
                eSave(clsDB.DbCon, "설정");
            }            

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            //if (sender == this.btnPrint)
            //{
            //    ePrint();
            //}

        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            //gROWID = "";

            //if (e.Row < 0 || e.ColumnHeader == true) return;
            //if (e.Button != MouseButtons.Left) return;

            //if (sender == this.ssList)
            //{
            //    for (int i = 0; i < o.ActiveSheet.RowCount; i++)
            //    {
            //        o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
            //    }

            //    o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGray;

            //    txtSetName.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayReadSet.SetName].Text.Trim();
            //    txtXName.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayReadSet.ExName].Text.Trim();
            //    txtResult.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayReadSet.Result].Text.Trim() + o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayReadSet.Result1].Text.Trim();
            //    gROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayReadSet.ROWID].Text.Trim();

            //    screen_clear("CLICK");

            //}

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;


            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
            //if (sender == this.ssList)
            //{
            //    if (e.ColumnHeader == true)
            //    {
            //        clsSpread.gSpdSortRow(o, e.Column);
            //        return;
            //    }
            //    if (e.RowHeader == true)
            //    {
            //        return;
            //    }

            //}


        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            //if (sender == this.txtSearch)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        screen_display();
            //    }
            //}

        }

        void eSpliterClick(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ExpandableSplitter ex = (DevComponents.DotNetBar.ExpandableSplitter)sender;

            //if (ex.Expanded == true)
            //{
            //    panel3.Size = new System.Drawing.Size(882, 20);
            //}
            //else
            //{
            //    panel3.Size = new System.Drawing.Size(882, 280);
            //}
        }

        void eTxtKeyUp(object sender, KeyEventArgs e)
        {
            
        }

        void eSave(PsmhDb pDbCon, string Job)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            

            #region 저장 및 갱신에 사용될 배열초기화 및 변수세팅

            clsComSup.cBasBCode cBasBCode = new clsComSup.cBasBCode();
            cBasBCode.ROWID = "";
            cBasBCode.Gubun = "C#_Xray_판독명단정렬";
            cBasBCode.Code = clsType.User.IdNumber;
            cBasBCode.Name = read_xSort();
            cBasBCode.EntSabun = Convert.ToInt32(clsType.User.IdNumber);

            dt = sup.sel_BasBCode(pDbCon, " 'C#_Xray_판독명단정렬' ", clsType.User.IdNumber);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                cBasBCode.ROWID = dt.Rows[0]["ROWID"].ToString().Trim();                                
            }            

            #endregion
                  

            try
            {
                clsDB.setBeginTran(pDbCon);
                
                if (Job == "설정")
                {
                    if (cBasBCode.ROWID != "")
                    {
                        SqlErr =sup.up_Bas_BCode(pDbCon, cBasBCode, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = sup.ins_Bas_BCode(pDbCon, cBasBCode, ref intRowAffected);
                    }
                }
                
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }
                else
                {
                    clsDB.setCommitTran(pDbCon);

                    screen_clear();
                    this.Close();
                    return;

                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장    
            }

        }

        void eCboSelChanged(object sender, EventArgs e)
        {
            //ComboBox o = (ComboBox)sender;
            ////조회
            //try
            //{
            //    if (o.SelectedItem.ToString() != null)
            //    {
            //        gXJong = setJong(o.Text.Trim());
            //        if (gXJong == "*")
            //        {
            //            gXJong = "";
            //        }
            //        screen_display();
            //    }

            //}
            //catch
            //{

            //}

        }

        string setJong(string argXJong)
        {
            string s = string.Empty;

            s = VB.Left(argXJong, 1);
            if (s == "*")
            {
                s = "";
            }

            return s;
        }

        void setCombo()
        {
            ////검사종류
            //method.setCombo_View(this.cboJong, Query.Get_BasBcode(clsDB.DbCon, "C#_XRAY_접수종류", "", " Code|| '.' || Name Names ", " AND CODE IN ('1','2','3','4','5','6','7','8','9') "), clsParam.enmComParamComboType.ALL);

            //if (gXJong != "")
            //{
            //    DataTable dt = Query.Get_BasBcode(clsDB.DbCon, "C#_XRAY_접수종류", gXJong, " Code|| '.' || Name Names ");
            //    if (ComFunc.isDataTableNull(dt) == false)
            //    {
            //        cboJong.Text = dt.Rows[0]["Names"].ToString().Trim();
            //    }

            //}
        }               

        void setSort()
        {
            if (XSort == clsComSupXray.enm_XraySort.PANO)
            {
                optPano.Checked = true;
            }
            else if (XSort == clsComSupXray.enm_XraySort.SNAME)
            {
                optSName.Checked = true;
            }
            else if (XSort == clsComSupXray.enm_XraySort.DEPT)
            {
                optDept.Checked = true;
            }
            else if (XSort == clsComSupXray.enm_XraySort.XNAME)
            {
                optXName.Checked = true;
            }
            else if (XSort == clsComSupXray.enm_XraySort.XCODE)
            {
                optXCode.Checked = true;
            }
            else if (XSort == clsComSupXray.enm_XraySort.SEEK)
            {
                optSeek.Checked = true;
            }
            else
            {
                
            }
        }

        string read_xSort()
        {
            if (optPano.Checked == true)
            {
                return "PANO";
            }
            else if (optSName.Checked == true)
            {
                return "SNAME";
            }
            else if (optDept.Checked == true)
            {
                return "DEPT";
            }
            else if (optXName.Checked == true)
            {
                return "XNAME";
            }
            else if (optXCode.Checked == true)
            {
                return "XCODE";
            }
            else if (optSeek.Checked == true)
            {
                return "SEEK";
            }
            else
            {
                return "";
            }
            
        }

        void screen_clear(string argJob = "")
        {
            //
            read_sysdate();

            //콘트롤 값 clear
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {

                if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).Checked = false;
                }

            }

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display()
        {            
            //GetData(clsDB.DbCon,argSabun);
        }

        void GetData(PsmhDb pDbCon, string argSabun)
        {
            
            //DataTable dt = null;
            
            //Cursor.Current = Cursors.WaitCursor;

            //dt = sup.sel_BasBCode(pDbCon, " 'C#_Xray_판독명단정렬' ", argSabun);

            //#region //데이터셋 읽어 자료 표시


            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    XSort = cXray.read_xrayList_Sort(dt.Rows[0]["Name"].ToString().Trim());
            //}
            //else
            //{
            //    XSort = clsComSupXray.enm_XraySort.NULL;
            //}

            //#endregion

            //Cursor.Current = Cursors.Default;

        }

    }
}
