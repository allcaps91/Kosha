using ComBase;
using ComDbB;
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
    /// File Name       : frmComSupXraySET20.cs
    /// Description     : 조영제 바코드 출력
    /// Author          : 안정수
    /// Create Date     : 2019-09-24
    /// Update History  : 
    /// </summary>    
    /// <seealso cref= " >> frmComSupXraySET20.cs 폼이름 재정의" />
    public partial class frmComSupXraySET20 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();  
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxrayspd = new clsComSupXraySpd();
        clsComSupXraySQL cxraysql = new clsComSupXraySQL();
        clsComSupXraySpd.cComSupPRT_XRAYCont cXrayCon = null;

        bool show = true;
        string gROWID = "";
        string gTab = "";
        string gJob = "";


        #endregion

        public frmComSupXraySET20(bool bShow = true)
        {
            InitializeComponent();
            show = bShow;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            //this.btnExit.Click += new EventHandler(eBtnClick);

            //this.btnSearch1.Click += new EventHandler(eBtnSearch);    

            this.btnPrint1.Click += new EventHandler(eBtnPrint);
            this.btnPrint2.Click += new EventHandler(eBtnPrint);
            this.btnPrint3.Click += new EventHandler(eBtnPrint);
            this.btnPrint4.Click += new EventHandler(eBtnPrint);
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
                cxrayspd.sSpd_enmSupXraySET20(ssCT, cxrayspd.sSpdenmSupenmXraySET20, cxrayspd.nSpdenmSupenmXraySET20, 1, 0);
                cxrayspd.sSpd_enmSupXraySET20(ssMRI, cxrayspd.sSpdenmSupenmXraySET20, cxrayspd.nSpdenmSupenmXraySET20, 1, 0);
                cxrayspd.sSpd_enmSupXraySET20(ssSP, cxrayspd.sSpdenmSupenmXraySET20, cxrayspd.nSpdenmSupenmXraySET20, 1, 0);
                cxrayspd.sSpd_enmSupXraySET20(ssHyang, cxrayspd.sSpdenmSupenmXraySET20, cxrayspd.nSpdenmSupenmXraySET20, 1, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                panBtop.Visible = false;
                line1.Visible = false;
                btnPrint1.Enabled = true;
                btnPrint2.Enabled = true;
                btnPrint3.Enabled = true;

                //Tab.ControlBox.Visible = false; //컨트롤 박스 단축키 없기
                //툴팁

                screen_clear();

                //screen_display();

                //
                //setSpd(ssList, sSpdTelview, nSpdTelview, 1, 0);
            }
        }

        void screen_clear()
        {
            read_sysdate();

            ssList.ActiveSheet.Rows.Count = 0;
            ssCT.ActiveSheet.RowCount = 0;
            ssMRI.ActiveSheet.RowCount = 0;
            ssSP.ActiveSheet.RowCount = 0;
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }
         
        void eBtnClick(object sender, EventArgs e)
        {
            //if (sender == this.btnExit)
            //{
            //    this.Close();
            //    return;
            //}        
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint1)
            {
                ePrint_Bar_Cont(clsDB.DbCon, ssCT);
            }
            else if (sender == this.btnPrint2)
            {
                ePrint_Bar_Cont(clsDB.DbCon, ssMRI);
            }
            else if (sender == this.btnPrint3)
            {
                ePrint_Bar_Cont(clsDB.DbCon, ssSP);
            }
            else if (sender == this.btnPrint4)
            {
                ePrint_Bar_Cont(clsDB.DbCon, ssHyang);
            }
        }
        void ePrint_Bar_Cont(PsmhDb pDbCon, FpSpread Spd, string argJob = "")
        {
            if (Spd.ActiveSheet.Rows.Count > 0)
            {
                cXrayCon = new clsComSupXraySpd.cComSupPRT_XRAYCont();
                for (int i = 0; i < Spd.ActiveSheet.Rows.Count; i++)
                {
                    if (Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CHK].Text.Trim() == "True")
                    {
                        cXrayCon.strPano = ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.Pano].Text.Trim();
                        cXrayCon.strSName = ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.SName].Text.Trim();
                        cXrayCon.strSexAge = ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.Sex].Text.Trim() + "/" + ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.Age].Text.Trim();
                        cXrayCon.strSuCode = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text.Trim();
                        cXrayCon.strSuName = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text.Trim();

                        frmComSupPRT01 f = new frmComSupPRT01(clsComSupSpd.enmPrtType.ENDO_BAR, "혈액환자정보", cXrayCon);

                        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CHK].Text = "False";
                    }
                }

                //Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa02.CHK, Spd.ActiveSheet.Rows.Count - 1, (int)clsComSupEndsSpd.enmSupEndsJusa02.CHK].Text = "False";
            }
        }

        public void Read_Patient_info(PsmhDb pDbCon, string argRowid)
        {
            screen_clear();

            GetData(pDbCon, ssList, argRowid);

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argRowid)
        {
            DataTable dt = null;

            dt = cxraysql.sel_Read_Detail(pDbCon, argRowid);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                ssList.ActiveSheet.Rows.Count = dt.Rows.Count;
                ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.Pano].Text = ComFunc.SetAutoZero(dt.Rows[0]["PANO"].ToString().Trim(), 8);
                ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.SName].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.Sex].Text = dt.Rows[0]["SEX"].ToString().Trim();
                ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.Age].Text = dt.Rows[0]["AGE"].ToString().Trim();
                ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.BDATE].Text = VB.Left(dt.Rows[0]["BDATE"].ToString().Trim(), 10);
                ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.XCODE].Text = dt.Rows[0]["XCODE"].ToString().Trim();
                ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.XJONG].Text = dt.Rows[0]["XJONG"].ToString().Trim();
                ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.IPDOPD].Text = dt.Rows[0]["IPDOPD"].ToString().Trim();
                ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.SEEKDATE].Text = VB.Left(dt.Rows[0]["SEEKDATE"].ToString().Trim(), 10);
                ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.ROWID].Text = dt.Rows[0]["ROWID"].ToString().Trim();

                if (ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.XJONG].Text == "2"         //특수
                    || ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.XJONG].Text == "4"      //CT
                    || ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.XJONG].Text == "5")     //MRI
                {
                    screen_display();
                }
            }            
        }

        void screen_display()
        {
            DataTable dt = null;
            int i = 0;

            string argCode1 = "";
            string argCode2 = "";
            string argCode3 = "";
            string argCode4 = "";

            argCode1 = "'X-HX3150', 'X-IB130 ', 'X-IOM400', 'X-OPT320', 'X-P30130', 'X-X300BT'";    //CT
            argCode2 = "'X-GAD10', 'X-GADO', 'X-PR10', 'X-UNRI15'";                                 //MRI
            argCode3 = "'X-BAL4', 'X-BARI',  'X-GAST',   'X-MAC',   'X-OM10',  'X-P30030'";         //특수
            argCode4 = " 'A-BASCAM', 'A-DZP10', 'A-POL12', 'A-ATI2MG', 'A-POCSY','A-KET5'";         //마약향정

            ssSP.ActiveSheet.Rows.Count = 0;
            ssCT.ActiveSheet.Rows.Count = 0;
            ssMRI.ActiveSheet.Rows.Count = 0;            
            ssHyang.ActiveSheet.Rows.Count = 0;

            dt = cxraysql.sel_Xray_BasUse(clsDB.DbCon, ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.XCODE].Text, ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.Age].Text);
                        
            if (ComFunc.isDataTableNull(dt) == false)
            {
                if (ssList.ActiveSheet.Rows.Count != 0)
                {
                    if (ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.XJONG].Text == "4")
                    {
                        ssCT.ActiveSheet.Rows.Count = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssCT.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["MCODE"].ToString().Trim();
                            ssCT.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["MNAME"].ToString().Trim();
                        }
                    }
                    else if (ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.XJONG].Text == "5")
                    {
                        ssMRI.ActiveSheet.Rows.Count = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssMRI.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["MCODE"].ToString().Trim();
                            ssMRI.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["MNAME"].ToString().Trim();
                        }
                    }
                    else if (ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.XJONG].Text == "2")
                    {
                        ssSP.ActiveSheet.Rows.Count = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssSP.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["MCODE"].ToString().Trim();
                            ssSP.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["MNAME"].ToString().Trim();
                        }
                    }                    
                }
            }
            dt.Dispose();
            dt = null;

            if (ssList.ActiveSheet.Rows.Count != 0)
            {
                if(ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.IPDOPD].Text == "I")
                {
                    dt = cxraysql.sel_Read_MayakHyang_I(clsDB.DbCon, ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.Pano].Text
                        , ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.BDATE].Text
                        , ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.SEEKDATE].Text);

                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        ssHyang.ActiveSheet.Rows.Count = dt.Rows.Count; 

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssHyang.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                            ssHyang.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["FC_NAME"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt = cxraysql.sel_Read_MayakHyang_O(clsDB.DbCon, ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.Pano].Text
                        , ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.BDATE].Text
                        , ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.SEEKDATE].Text);
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        ssHyang.ActiveSheet.Rows.Count = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssHyang.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                            ssHyang.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["FC_NAME"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null; 
                }
            }


            //dt = cxraysql.sel_Read_MCode(clsDB.DbCon, argCode1);
            //if (ComFunc.isDataTableNull(dt) == false)
            //{
            //    ssCT.ActiveSheet.Rows.Count = dt.Rows.Count;
            //    for(i = 0; i < dt.Rows.Count; i++)
            //    {
            //        ssCT.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["MCODE"].ToString().Trim();
            //        ssCT.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["MNAME"].ToString().Trim();
            //    }
            //}
            //dt.Dispose();
            //dt = null;

                //dt = cxraysql.sel_Read_MCode(clsDB.DbCon, argCode2);
                //if (ComFunc.isDataTableNull(dt) == false)
                //{
                //    ssMRI.ActiveSheet.Rows.Count = dt.Rows.Count;
                //    for (i = 0; i < dt.Rows.Count; i++)
                //    {
                //        ssMRI.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["MCODE"].ToString().Trim();
                //        ssMRI.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["MNAME"].ToString().Trim();
                //    }
                //}
                //dt.Dispose();
                //dt = null;

                //dt = cxraysql.sel_Read_MCode(clsDB.DbCon, argCode3);
                //if (ComFunc.isDataTableNull(dt) == false)
                //{
                //    ssSP.ActiveSheet.Rows.Count = dt.Rows.Count;
                //    for (i = 0; i < dt.Rows.Count; i++)
                //    {
                //        ssSP.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["MCODE"].ToString().Trim();
                //        ssSP.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["MNAME"].ToString().Trim();
                //    }
                //}
                //dt.Dispose();
                //dt = null;

                //dt = cxraysql.sel_Read_MCode(clsDB.DbCon, argCode3);
                //if (ComFunc.isDataTableNull(dt) == false)
                //{
                //    ssSP.ActiveSheet.Rows.Count = dt.Rows.Count;
                //    for (i = 0; i < dt.Rows.Count; i++)
                //    {
                //        ssSP.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["MCODE"].ToString().Trim();
                //        ssSP.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["MNAME"].ToString().Trim();
                //    }
                //}
                //dt.Dispose();
                //dt = null;

                //dt = cxraysql.sel_ORDER_XRAY(clsDB.DbCon,
                //                             ComFunc.SetAutoZero(ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.Pano].Text, 8),
                //                             ssList.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmXraySET20PI.BDATE].Text);
                //if (ComFunc.isDataTableNull(dt) == false)
                //{
                //    ssHyang.ActiveSheet.Rows.Count = dt.Rows.Count;
                //    for (i = 0; i < dt.Rows.Count; i++)
                //    {
                //        ssHyang.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                //        ssHyang.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                //    }
                //}
                //dt.Dispose();
                //dt = null; 

                //if(ssHyang.ActiveSheet.Rows.Count == 0)
                //{
                //    dt = cxraysql.sel_Read_ctYAK(clsDB.DbCon, argCode4);
                //    if (ComFunc.isDataTableNull(dt) == false)
                //    {
                //        ssHyang.ActiveSheet.Rows.Count = dt.Rows.Count;
                //        for (i = 0; i < dt.Rows.Count; i++)
                //        {
                //            ssHyang.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.CODE].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                //            ssHyang.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET20.NAME].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                //        }
                //    }
                //    dt.Dispose();
                //    dt = null;
                //}

        }
    }
}
