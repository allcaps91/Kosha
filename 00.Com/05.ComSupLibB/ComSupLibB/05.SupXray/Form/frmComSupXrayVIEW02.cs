using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayVIEW02.cs
    /// Description     : 영상의학과 일보 조회
    /// Author          : 윤조연
    /// Create Date     : 2017-07-04
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xumain\xumain16.frm(FrmIlbo) >> frmComSupXrayVIEW02.cs 폼이름 재정의" />
    public partial class frmComSupXrayVIEW02 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsComSup sup = new clsComSup();
        clsComSupXray xray = new clsComSupXray();        
        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();
        clsComSupXrayRead cRead = new clsComSupXrayRead();


        //통계용
        string[] strDeptCode = null;
        string[] strDeptName = null;
        int[,] nCNT = null;
        string[] strCode = null;
        string[] strName = null;

        #endregion

        public frmComSupXrayVIEW02()
        {
            InitializeComponent();

            setEvent();

        }

        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            dtpFDate.Text = cpublic.strSysDate;

            setArray(pDbCon);

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
                xraySpd.sSpd_XrayIlbo(ssList, xraySpd.sSpdXrayIlbo, xraySpd.nSpdXrayIlbo, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                screen_clear();

                setCtrlData(clsDB.DbCon);
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
                GetData(clsDB.DbCon, ssList, dtpFDate.Text.Trim());
            }            
            else if (sender == this.btnPrint)
            {
                //출력
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

            strTitle = "영상의학 일보 " + "(" + dtpFDate.Text + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        void screen_clear()
        {
            


        }

        void setArray(PsmhDb pDbCon)
        {
            strDeptCode = new string[0];
            strDeptName = new string[0];
            nCNT = new int[0,0];
            strCode = new string[0];
            strName = new string[0];

            
            DataTable dt = sup.sel_Bas_ClinicDept(pDbCon);
            if (dt != null && dt.Rows.Count > 0)
            {
                //배열 초기화
                strDeptCode = new string[dt.Rows.Count+1];
                strDeptName = new string[dt.Rows.Count+1];

                nCNT = new int[dt.Rows.Count+1, 5];
                strCode = new string[dt.Rows.Count+1];
                strName = new string[dt.Rows.Count+1];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strDeptCode[i] = dt.Rows[i]["DeptCode"].ToString().Trim();
                    strDeptName[i] = dt.Rows[i]["DeptNameK"].ToString().Trim();
                }

            }

        }

        void setArray(int arrylen )
        {            
            nCNT = new int[arrylen, 5];
            strCode = new string[arrylen];
            strName = new string[arrylen];
            
        }           
        
        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argDate)
        {
            int i = 0;            
            DataTable dt = null;
            DataTable dt2 = null;
            string strIO = "";
            string strData ="";
            string strSwData = "";
            string strPano = "";
            int nRow = 0;

            ssList.Enabled = false;

            if (optO.Checked == true)
            {
                strIO = "O";
            }
            else if (optI.Checked == true)
            {
                strIO = "I";
            }

            Cursor.Current = Cursors.WaitCursor;

            //저장된 배열값 초기화
            setArray(strDeptCode.Length+1);

            Spd.ActiveSheet.RowCount = 0;
            

            //진료과별 통계 쿼리실행                  
            dt = xraySql.sel_Xray_Detail_Tong(pDbCon, "과별",strIO, argDate);                       

            #region //데이터셋 읽어 자료 표시
            
            if ( dt !=null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount =strDeptName.Length;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strData = dt.Rows[i]["DeptCode"].ToString().Trim();
                    if (strData != strSwData)
                    {
                        nRow = sup.Get2_Conv(strDeptCode, strData);
                        strCode[nRow] = strData;
                        strSwData = strData;
                    }
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();

                    if (strPano != "00000100" && strPano != "00000110" && strPano != "00000200" && strPano != "00000300")
                    {
                        nCNT[nRow, 1] += 1;
                        nCNT[strDeptName.Length-1, 1] += 1;
                    }

                    nCNT[nRow, 2] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());
                    nCNT[nRow, 3] += Convert.ToInt32(dt.Rows[i]["Cnt2"].ToString());

                    nCNT[strDeptName.Length-1, 2] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());
                    nCNT[strDeptName.Length-1, 3] += Convert.ToInt32(dt.Rows[i]["Cnt2"].ToString());


                }

                //값 표시
            for ( i = 0; i < strDeptName.Length; i++)
            {
                if (i == strDeptName.Length-1 )
                {
                    ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayIlbo.DeptName].Text = "합계";
                }
                else
                {
                    ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayIlbo.DeptName].Text = strDeptName[i];
                }

                ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayIlbo.DeptCnt1].Text = nCNT[i, 1].ToString();
                ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayIlbo.DeptCnt2].Text = nCNT[i, 2].ToString();
                ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayIlbo.DeptCnt3].Text = nCNT[i, 3].ToString();


            }
            }

            dt.Dispose();
            dt = null;
                   

            #endregion

            //재료코드 소모 쿼리
            dt = xraySql.sel_Xray_Use_Jaje_Tong(pDbCon, argDate);

            strData = "";
            strSwData = "";
            nRow = -1;

            //저장된 배열값 초기화
            setArray(dt.Rows.Count+1);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {   

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strData = dt.Rows[i]["MCode"].ToString().Trim();
                    if (strData != strSwData)
                    {
                        nRow++;

                        strCode[nRow] = strData;
                        strName[nRow] = dt.Rows[i]["mName"].ToString().Trim();
                        strSwData = strData;
                        
                    }
                    
                    if (Convert.ToInt16(dt.Rows[i]["GbUse"].ToString().Trim()) ==0)
                    {
                        nCNT[nRow, 1] += Convert.ToInt32(dt.Rows[i]["Qty1"].ToString());
                        nCNT[strCode.Length-1, 1] += Convert.ToInt32(dt.Rows[i]["Qty1"].ToString());
                    }
                    else
                    {
                        nCNT[nRow, 2] += Convert.ToInt32(dt.Rows[i]["Qty1"].ToString());
                        nCNT[strCode.Length - 1, 2] += Convert.ToInt32(dt.Rows[i]["Qty1"].ToString());
                    }
                                        
                    nCNT[nRow, 3] += Convert.ToInt32(dt.Rows[i]["Qty1"].ToString());
                    nCNT[strCode.Length - 1, 3] += Convert.ToInt32(dt.Rows[i]["Qty1"].ToString());
                                                           


                }

                nRow = 0;
                //값 표시
                for (i = 0; i < strCode.Length; i++)
                {
                    if (i == strCode.Length - 1)
                    {
                        ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.mCode].Text = "";
                        ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.mName].Text = "합계";
                    }
                    else
                    {
                        ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.mCode].Text = strCode[i];
                        ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.mName].Text = strName[i];
                    }

                    ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.mSomo].Text = nCNT[i, 1].ToString();
                    ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.mRpt].Text = nCNT[i, 2].ToString();

                    ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.mJego].Text = "";
                    ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.mSum].Text = nCNT[i, 3].ToString();

                    nRow++;
                }

            dt.Dispose();
            dt = null;
            

            }

            #endregion

            //촬영기사만 체크
            dt = xraySql.sel_Xray_Detail_Tong(pDbCon, "기사별2", strIO, argDate);

            //저장된 배열값 초기화
            setArray(dt.Rows.Count + 1);

            //촬영기사 전체 
            dt = xraySql.sel_Xray_Detail_Tong(pDbCon, "기사별",strIO,argDate);

            strData = "";
            strSwData = "";
            nRow = -1;           

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {            

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strData = dt.Rows[i]["ExID"].ToString().Trim();
                    if (strData != strSwData)
                    {
                        nRow++;

                        strCode[nRow] = strData;                        
                        strSwData = strData;

                    }

                    strPano = dt.Rows[i]["Pano"].ToString().Trim();

                    if (strPano != "00000100" && strPano != "00000110" && strPano != "00000200" && strPano != "00000300")
                    {
                        nCNT[nRow, 1] += 1;
                        nCNT[strCode.Length - 1, 1] += 1;
                    }

                    nCNT[nRow, 2] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());
                    nCNT[nRow, 3] += Convert.ToInt32(dt.Rows[i]["Cnt2"].ToString());

                    nCNT[strCode.Length - 1, 2] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());
                    nCNT[strCode.Length - 1, 3] += Convert.ToInt32(dt.Rows[i]["Cnt2"].ToString());


                }

                //종합건진,회사신검,단체신검은 인원을 계산
                dt = xraySql.sel_Xray_Use(pDbCon, dtpFDate.Text.Trim());

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        dt2 = cRead.sel_XrayDetail(pDbCon, "T1", "", "", 0, dtpFDate.Text.Trim(), Convert.ToInt32(dt.Rows[i]["ExID"].ToString()), strIO);
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            strData = dt2.Rows[0]["ExID"].ToString().Trim();
                            nRow = sup.Get2_Conv(strCode, strData);
                            strCode[nRow] = strData;
                            nCNT[nRow, 1] += Convert.ToInt32(dt.Rows[i]["cQty"].ToString());
                            nCNT[strCode.Length - 1, 1] += Convert.ToInt32(dt.Rows[i]["cQty"].ToString());
                        }
                    }

                }

                nRow = 0;
                //값 표시
                for (i = 0; i < strCode.Length; i++)
                {
                    if (i == strCode.Length - 1)
                    {
                        ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.sGisa].Text = "합계";
                    }
                    else
                    {
                        ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.sGisa].Text = clsVbfunc.GetInSaName(pDbCon, strCode[i]);
                    }

                    ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.sCnt1].Text = nCNT[i, 1].ToString();
                    ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.sCnt2].Text = nCNT[i, 2].ToString();
                    ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.sCnt3].Text = nCNT[i, 3].ToString();

                    nRow++;
                }

            dt.Dispose();
            dt = null;

            

            }

            #endregion
                        

            //기사별로 촬영통계            
            dt = xraySql.sel_Xray_Use_Gisa_Tong(pDbCon, strIO, argDate);

            //저장된 배열값 초기화
            setArray(dt.Rows.Count + 1);

            strData = "";
            strSwData = "";
            nRow = -1;

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strData = dt.Rows[i]["ExID"].ToString().Trim();
                    if (strData != strSwData)
                    {
                        nRow++;

                        strCode[nRow] = strData;
                        strSwData = strData;

                    }
                    
                    nCNT[nRow, 1] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());
                    nCNT[strCode.Length - 1, 1] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());                  

                }

                nRow = 0;
                //값 표시
                for (i = 0; i < strCode.Length; i++)
                {
                    if (i == strCode.Length - 1)
                    {
                        ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.sGisa2].Text = "합계";
                    }
                    else
                    {
                        ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.sGisa2].Text = clsVbfunc.GetInSaName(pDbCon, strCode[i]);
                    }

                    ssList.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayIlbo.sRepert].Text = nCNT[i, 1].ToString();


                    nRow++;

                }
            }

            dt.Dispose();
            dt = null;

            //출력때문 사용
            ssList.ActiveSheet.ColumnCount ++;
            ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount-1, ssList.ActiveSheet.ColumnCount-1].Text = "1";
            ssList.ActiveSheet.Columns[ssList.ActiveSheet.ColumnCount - 1].Visible = false;


            #endregion



            Cursor.Current = Cursors.Default;
            ssList.Enabled = true;

        }
    }
}
