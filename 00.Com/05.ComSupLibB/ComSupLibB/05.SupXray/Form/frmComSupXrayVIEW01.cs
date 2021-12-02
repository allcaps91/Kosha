using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
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
    /// File Name       : frmComSupXrayVIEW01.cs
    /// Description     : 영상의학과 일보 조회
    /// Author          : 윤조연
    /// Create Date     : 2017-07-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\sumain\xumain18.frm(FrmBuwiIlbo) >> frmComSupXrayVIEW01.cs 폼이름 재정의" />
    public partial class frmComSupXrayVIEW01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsComSupXray xray = new clsComSupXray();
        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupSpd supspd = new clsComSupSpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();

        //통계용        
        int[,] nCNT = null;

        #endregion

        public frmComSupXrayVIEW01()
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

            dtpFDate.Text = cpublic.strSysDate;

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
                xraySpd.sSpd_XrayIlbo2(ssList, xraySpd.sSpdXrayIlbo2, xraySpd.nSpdXrayIlbo2, 10, 0);

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
                GetData(clsDB.DbCon,ssList, dtpFDate.Text.Trim());
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

            strTitle = "방사선 부위별 일보 " + "(" + dtpFDate.Text + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, true);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        void setArray()
        {
            nCNT = new int[101, 16];

        }

        void screen_clear()
        {



        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argDate)
        {
            int i = 0, j = 0, k = 0;
            string strIO = "";
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            int nRow = 0;

            if (optO.Checked == true)
            {
                strIO = "O";
            }
            else if (optI.Checked == true)
            {
                strIO = "I";
            }

            Cursor.Current = Cursors.WaitCursor;
            ssList.Enabled = false;


            dt = xraySql.sel_Xray_Detail_Tong(pDbCon, "촬영종류별1", strIO, argDate);

            setArray();

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    j = 0;
                    if (dt.Rows[i]["XJong"].ToString().Trim() == "")
                    {
                        j = 0;
                    }
                    else
                    {
                        if (dt.Rows[i]["XJong"].ToString().Trim().CompareTo("A") <0)
                        {
                            j = Convert.ToInt16(dt.Rows[i]["XJong"].ToString().Trim());
                        }
                        
                    }
                                       
                    nRow = Convert.ToInt16(dt.Rows[i]["XSubCode"].ToString().Trim());
                    if (nRow < 0 || nRow > 99 || j < 1 || j > 7)
                    {
                        MessageBox.Show("방사선구분 또는 세부분류 오류입니다");
                    }
                    else
                    {
                        k = j * 2-2;

                        nCNT[nRow, k] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());
                        nCNT[nRow, k + 1] += Convert.ToInt32(dt.Rows[i]["Cnt2"].ToString());

                        nCNT[100, k] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());
                        nCNT[100, k + 1] += Convert.ToInt32(dt.Rows[i]["Cnt2"].ToString());

                    }
                }


                dt = xraySql.sel_Xray_Detail_Tong(pDbCon, "촬영종류별2", strIO, argDate);


                if (dt != null && dt.Rows.Count > 0)
                {


                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        j = 0;
                        if (dt.Rows[i]["XJong"].ToString().Trim() == "")
                        {
                            j = 0;
                        }
                        else
                        {
                            if (dt.Rows[i]["XJong"].ToString().Trim().CompareTo("A") < 0)
                            {
                                j = Convert.ToInt16(dt.Rows[i]["XJong"].ToString().Trim());
                            }

                        }

                        nRow = Convert.ToInt16(dt.Rows[i]["XSubCode"].ToString().Trim());
                        if (nRow < 1 || nRow > 99 || j < 1 || j > 7)
                        {
                            MessageBox.Show("방사선구분 또는 세부분류 오류입니다");
                        }
                        else
                        { 
                            k = j * 2 - 2;

                            nCNT[nRow, k] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());
                            nCNT[nRow, k + 1] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());

                            nCNT[100, k] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());
                            nCNT[100, k + 1] += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString());

                        }
                    }



                }

                //값 표시               
                int nCol = 0;
                for (i = 1; i <= 7; i++)
                {
                    nRow = 0;
                    for ( j = 1; j <= 101; j++)
                    {
                        k = i * 2-2;
                        
                        if (nCNT[j-1,k] >0 || nCNT[j-1,k+1] >0)
                        {
                            nCol = i * 4 - 4;
                            if (j == 101)
                            {                                
                                ssList.ActiveSheet.Cells[nRow, nCol].Text = "합계";
                            }
                            else
                            {
                                dt = xraySql.sel_Xray_SubClass(pDbCon, i.ToString(), j.ToString());
                                if (dt!=null && dt.Rows.Count>0)
                                {
                                    ssList.ActiveSheet.Cells[nRow, nCol].Text = dt.Rows[0]["SubName"].ToString();
                                }
                                
                            }

                            Application.DoEvents();
                            nCol++;
                            ssList.ActiveSheet.Cells[nRow, nCol].Text = nCNT[j-1, k].ToString();
                            Application.DoEvents();                            
                            ssList.ActiveSheet.Cells[nRow, nCol + 1].Text = nCNT[j-1, k + 1].ToString();
                            Application.DoEvents();
                            nRow++;
                                                        
                        }

                        
                    }


                }


                #endregion

                Cursor.Current = Cursors.Default;
                ssList.Enabled = true;

            }
        }

    }

}
