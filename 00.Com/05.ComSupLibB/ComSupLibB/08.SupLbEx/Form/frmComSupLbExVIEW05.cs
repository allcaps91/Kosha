using ComLibB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupLbExLIST01.cs
    /// Description     : 병동 채혈 Work List
    /// Author          : 김홍록
    /// Create Date     : 2017-06-17
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrinfo\FrmExamList.frm" />
    public partial class frmComSupLbExVIEW05 : Form
    {
        DataSet gDs;

        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        DateTime sysdate;

        string strWard = string.Empty;
        string strDate = string.Empty;
      
        /// <summary>생성자</summary>
        public frmComSupLbExVIEW05(string strWard, string strDate)
        {
            InitializeComponent();

            this.strWard = strWard;
            this.strDate = strDate;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnClick);

            btnPrint.Click += new EventHandler(eBtnPrintClick);
        }
        
        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                setCtrl();
            }
        }

        void setCtrl()
        {
            setCtrlDate();
            setCtrlCombo();
            setCtrlSpread();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            setCtrlSpread();
        }

        void eBtnPrintClick(object sender, EventArgs e)
        {
            setCtrlSpreadPrint();       
        }

        void setCtrlDate()
        {
            if (string.IsNullOrEmpty(this.strDate) == false)
            {
                sysdate = Convert.ToDateTime(this.strDate);
            }
            else
            {
                sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            }

            this.dtpFDate.Value = sysdate;
        }

        void setCtrlCombo()
        {

            List<string> lstCbo = new List<string>();
            DataTable dt = comSql.sel_BAS_WARD_COMBO(clsDB.DbCon, true);            

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstCbo.Add(dt.Rows[i]["CODE_NAME"].ToString());
                }

                method.setCombo_View(this.cboWard, lstCbo, clsParam.enmComParamComboType.ALL);

                //TODO : 2017.06.08.김홍록 INI
                //this.cboWard.Text = GstrHelpCode;

                //this.cboWard.Enabled = false;

                dt.Dispose();
                dt = null;

                //2019-12-27 안정수, 서나영s 요청으로 병동 자동세팅되록 보완
                if (strWard == "")
                {
                    if (VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
                    {
                        strWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
                    }
                }

                if (strWard != "")
                {
                    for (int i = 0; i < cboWard.Items.Count; i++)
                    {
                        if (cboWard.Items[i].ToString().Trim().Contains(strWard))
                        {
                            cboWard.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }

            
            else
            {
                ComFunc.MsgBox("데이터가 존재하지 않습니다.");
            }

        }

        void setCtrlSpread()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            DataSet ds = null;
            DataTable dt = null;

            this.strWard = method.getGubunText(this.cboWard.Text, ".");

            strWard = strWard == "*" ? "" : strWard;

            #region DataSet방식
            //ds = lbExSQL.sel_EXAM_SPECMST_WardWorkList(clsDB.DbCon, strWard, this.dtpFDate.Value.ToString("yyyy-MM-dd"));

            //this.gDs = ds;

            //setSpdStyle(this.ssMain, ds, lbExSQL.sSel_EXAM_SPECMST_WardWorkList, lbExSQL.nSel_EXAM_SPECMST_WardWorkList);            
            #endregion

            #region DataTable방식

            dt = lbExSQL.sel_EXAM_SPECMST_WardWorkList_dt(clsDB.DbCon, strWard, this.dtpFDate.Value.ToString("yyyy-MM-dd"));

            if(dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("자료가 없습니다.");
                dt.Dispose();
                dt = null;
            }

            else if (dt.Rows.Count > 0)
            {
                int nRowCnt = 0;
                int nCol = 0;
                string strPano = "";
                string strPanoOld = "";

                spread.Spread_All_Clear(ssMain);

                nRowCnt = 0;
                ssMain.ActiveSheet.Rows.Count = nRowCnt + 1;

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();

                    if(i != 0 && strPano != strPanoOld)
                    {
                        nRowCnt += 1;
                    }

                    if(nRowCnt >= ssMain.ActiveSheet.Rows.Count)
                    {
                        ssMain.ActiveSheet.Rows.Count = nRowCnt + 1;
                    }

                    ssMain.ActiveSheet.Cells[nRowCnt, 0].Text = dt.Rows[i]["ROOM"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[nRowCnt, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[nRowCnt, 2].Text = strPano;
                    ssMain.ActiveSheet.Cells[nRowCnt, 3].Text = (dt.Rows[i]["SEX"].ToString().Trim() == "F" ? "여" : "남") + "/" + dt.Rows[i]["AGE"].ToString().Trim();

                    switch (dt.Rows[i]["TUBE"].ToString().Trim())
                    {
                        case "011":
                            nCol = 4;
                            break;

                        case "002":
                        case "002A":
                        case "002B":
                            nCol = 5;
                            break;

                        case "001":
                            nCol = 6;
                            break;
                    }

                    if(VB.Val(dt.Rows[i]["CNT2"].ToString().Trim()) > 0)
                    {
                        ssMain.ActiveSheet.Cells[nRowCnt, nCol].Text = "[" + dt.Rows[i]["CNT2"].ToString().Trim() + "]";
                    }

                    strPanoOld = strPano;
                }

                dt.Dispose();
                dt = null;
            }            

            #endregion

        }

        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {
            spd.ActiveSheet.Rows.Count = 0;

            spd.DataSource = ds;
            spd.ActiveSheet.ColumnCount = colName.Length;

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            //2. 헤더 사이즈
            //spread.setHeader(spd, colName, size);

            for (int i = 0; i < spd.ActiveSheet.ColumnCount; i++)
            {
                spd.ActiveSheet.Columns.Get(i).Width = size[i];
             
            }

            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            // 3. 컬럼 스타일 설정.
            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            // 4. 정렬
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);
        }

        void setCtrlSpreadPrint()
        {
            string strHeader = string.Empty;
            string strFoot = string.Empty;

            
            if (this.ssMain.ActiveSheet.Rows.Count > 0)
            {

                string s = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"),"A","-");

                strHeader = spread.setSpdPrint_String("병동 채혈 Work List",new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += spread.setSpdPrint_String("    ▣ 병동 :" + this.strWard, new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += spread.setSpdPrint_String("출력시간:" + s, new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

                clsSpread.SpdPrint_Margin setMargin = new clsSpread.SpdPrint_Margin(1, 1, 1, 1, 1, 1);
                clsSpread.SpdPrint_Option setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

                spread.setSpdPrint(this.ssMain, true, setMargin, setOption, strHeader, strFoot);

            }

        }

    }
}
