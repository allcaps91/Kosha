using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaCytology.cs
/// Description     : 종합검진 Cytology 조회 및 등록
/// Author          : 이상훈
/// Create Date     : 2019-09-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm종검Cytology.frm(Frm종검Cytology)" />

namespace HC_Pan
{
    public partial class frmHaCytology : Form
    {
        ExamAnatmstService examAnatmstService = null;
        ExamSpecmstResultAnatmstService examSpecmstResultAnatmstService = null;
        OcsOrdercodeBasSutService ocsOrdercodeBasSutService = null;
        OcsOrdercodeService ocsOrdercodeService = null;
        ExamMasterService examMasterService = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();
        ComFunc cf = new ComFunc();

        string sPtno = "";
        string sSpecNo = "";
        string sSname = "";
        string sBDate = "";
        string sAnatNo = "";
        long sOrderNo = 0;
        string sGbIO = "";
        string sDept = "";
        string sDr = "";
        string FstrPANO = "";
        string FstrROWID = "";
        string FstrDept = "";
        string FstrDrCode = "";
        string fstrORDERCODE = "";

        public frmHaCytology()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            examAnatmstService = new ExamAnatmstService();
            examSpecmstResultAnatmstService = new ExamSpecmstResultAnatmstService();
            ocsOrdercodeBasSutService = new OcsOrdercodeBasSutService();
            ocsOrdercodeService = new OcsOrdercodeService();
            examMasterService = new ExamMasterService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtPtNo.LostFocus += new EventHandler(etxtLostFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpDate.Text = clsPublic.GstrSysDate;
            txtPtNo.Text = "";

            sp.Spread_All_Clear(SS2);
            fn_SS2_Display();
            SS2.Visible = false;
        }

        void fn_SS2_Display()
        {
            List<OCS_ORDERCODE_BAS_SUT> list = ocsOrdercodeBasSutService.GetItemCode();

            SS2.DataSource = list;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nRow = 0;
                //string strMasterGrop = "";
                List<string> strMasterGrop = new List<string>();
                string strOK = "";
                int nRowCnt = 0;

                string strFrDate = "";
                string strToDate = "";
                string strGubun = "";

                int nCnt = 0;

                strFrDate = dtpDate.Text + " 00:00";
                strToDate = dtpDate.Text + " 23:59";

                if (dtpDate.Text == "") return;

                if (rdoGubun1.Checked == true)
                {
                    strGubun = "OK";
                }
                else
                {
                    strGubun = "";
                }

                txtPtNo.Text = "";
                sp.Spread_All_Clear(SS1);

                txtRemark1.Text = "";
                txtRemark2.Text = "";
                txtRemark3.Text = "";
                txtRemark4.Text = "";
                FstrROWID = "";
                strOK = "";

                strMasterGrop = fn_Code_Read("A");  //전체

                List<EXAM_SPECMST_RESULT_ANATMST> list = examSpecmstResultAnatmstService.GetItembyMasterGroup(strFrDate, strToDate, strMasterGrop, strGubun);

                nRow = list.Count;
                nRowCnt = 0;
                SS1.ActiveSheet.RowCount = nRow;

                for (int i = 0; i < nRow; i++)
                {
                    strOK = "";

                    EXAM_ANATMST list2 = examAnatmstService.GetCountbyPaNo(list[i].RECEIVEDATE, list[i].PANO);

                    if (rdoGubun1.Checked == true)
                    {
                        if (list2 == null)
                        {
                            strOK = "OK";
                        }
                    }
                    else
                    {
                        if (list2 != null)
                        {
                            strOK = "OK";
                        }
                    }

                    if (strOK == "OK")
                    {
                        nRowCnt += 1;
                        SS1.ActiveSheet.Cells[nRowCnt - 1, 0].Text = list[i].PANO.Trim();
                        SS1.ActiveSheet.Cells[nRowCnt - 1, 1].Text = list[i].SNAME.Trim();
                        SS1.ActiveSheet.Cells[nRowCnt - 1, 2].Text = list[i].DEPTCODE.Trim();
                        SS1.ActiveSheet.Cells[nRowCnt - 1, 3].Text = cf.READ_DrName(clsDB.DbCon, list[i].DRCODE);
                        SS1.ActiveSheet.Cells[nRowCnt - 1, 4].Text = fn_Read_MasterName(list[i].MASTERCODE.Trim());
                        SS1.ActiveSheet.Cells[nRowCnt - 1, 5].Text = list[i].SPECNO;
                        SS1.ActiveSheet.Cells[nRowCnt - 1, 6].Text = list[i].DRCODE;
                        SS1.ActiveSheet.Cells[nRowCnt - 1, 7].Text = list[i].MASTERCODE;
                        if (list2 != null)
                        {
                            SS1.ActiveSheet.Cells[nRowCnt - 1, 8].Text = list2.ROWID.Trim();
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRowCnt - 1, 8].Text = "";
                        }
                    }
                }

                SS1.ActiveSheet.RowCount = nRowCnt;
            }
            else if (sender == btnSave)
            {
                int nMsg = 0;
                int result = 0;

                if (txtRemark1.Text.Trim().Length < 3)
                {
                    MessageBox.Show("Nature_Source of Specimen을 3글자이상 입력하십시오.", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                txtRemark1.Text = txtRemark1.Text.Replace("'", "`");
                txtRemark2.Text = txtRemark2.Text.Replace("'", "`");
                txtRemark3.Text = txtRemark3.Text.Replace("'", "`");
                txtRemark4.Text = txtRemark4.Text.Replace("'", "`");

                EXAM_ANATMST item = new EXAM_ANATMST();

                item.PTNO = sPtno;
                item.BDATE = sBDate;
                item.ORDERCODE = fstrORDERCODE;
                item.GBIO = "O";
                item.REMARK1 = txtRemark1.Text.Trim();
                item.REMARK2 = txtRemark2.Text.Trim();
                item.REMARK3 = txtRemark3.Text.Trim();
                item.REMARK4 = txtRemark4.Text.Trim();
                item.DEPTCODE = FstrDept.Trim();
                item.DRCODE = FstrDrCode.Trim();
                item.ROWID = FstrROWID;

                if (FstrROWID != "")
                {
                    result = examAnatmstService.Update(item);
                }
                else
                {
                    result = examAnatmstService.Insert(item);
                }

                if (result < 0)
                {
                    MessageBox.Show("Cytology 의뢰소견 등록시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FstrROWID = "";
                    return;
                }
                else
                {
                    MessageBox.Show("자료가 저장되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrROWID = "";
                    return;
                }
            }
        }

        private string fn_Read_MasterName(string argCode)
        {
            string rtnVal = "";

            if (argCode == "")
            {
                rtnVal = "";
                return rtnVal;
            }

            //BAS_SUN에서 해당 자료를 READ
            rtnVal = examMasterService.GetExamNamebyMasterCode(argCode);

            return rtnVal;
        }

        private List<string> fn_Code_Read(string argCode)
        {
            List<string> rtnVal = null;
            List<string> sMaster = new List<string>();
            //string sMaster = "";
            string sWsCode1Fr = "";
            string sWsCode1To = "";

            //해부병리 코드를 Read
            switch (argCode)
            {
                case "A":   //전체
                    sWsCode1Fr = "599";
                    sWsCode1To = "800";
                    break;
                case "S":   //조직 700~799
                    sWsCode1Fr = "699";
                    sWsCode1To = "800";
                    break;
                case "C":   //세포/신검 600~699
                    sWsCode1Fr = "599";
                    sWsCode1To = "700";
                    break;
                default:
                    break;
            }

            List<EXAM_MASTER> list = examMasterService.GetMasterCodebyWsCode1(sWsCode1Fr, sWsCode1To);

            for (int i = 0; i < list.Count; i++)
            {
                sMaster.Add(list[i].MASTERCODE.Trim());
            }

            rtnVal = sMaster;

            return rtnVal;
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader == true)
                {
                    sp.setSpdSort(SS1, e.Column, true);
                    return;
                }
                else
                {
                    SS2.Visible = false;
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strSName = "";

                FstrROWID = "";

                if (e.ColumnHeader == true)
                {
                    sp.setSpdSort(SS1, e.Column, true);
                    return;
                }

                SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0FFC0"));

                strSName = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                FstrDept = SS1.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                FstrDrCode = SS1.ActiveSheet.Cells[e.Row, 6].Text.Trim();
                fstrORDERCODE = SS1.ActiveSheet.Cells[e.Row, 7].Text.Trim();
                FstrROWID = SS1.ActiveSheet.Cells[e.Row, 8].Text.Trim();
                txtPtNo.Text = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                FstrPANO = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                txtRemark1.Text = "";
                txtRemark2.Text = "";
                txtRemark3.Text = "";
                txtRemark4.Text = "";

                grpBox.Text = strSName + " / " + SS1.ActiveSheet.Cells[e.Row, 4].Text.Trim();

                //종합건진,메뉴얼접수 이면
                fstrORDERCODE = ocsOrdercodeService.GetOrderCodebyItemCd(fstrORDERCODE);

                fn_Remark_Display(strSName, FstrROWID);

                SS2.Visible = true;
            }
            else if (sender == SS2)
            {
                SS2.Visible = false;
                txtRemark1.Text = SS2.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            }
        }

        private void fn_Remark_Display(string strSName, string fstrROWID)
        {
            sPtno = "";
            sSpecNo = "";
            sSname = "";
            sBDate = "";
            sGbIO = "";
            sDept = "";
            sDr = "";
            sOrderNo = 0;

            EXAM_ANATMST list = examAnatmstService.GetItembyRowId(fstrROWID);

            if (list != null)
            {
                sPtno = list.PTNO.To<string>("").Trim();
                sSpecNo = list.SPECNO.To<string>("").Trim();
                sSname = strSName.To<string>("").Trim();
                sBDate = list.BDATE.To<string>("");
                sAnatNo = list.ANATNO.To<string>("");
                sGbIO = list.GBIO.To<string>("");
                sDept = list.DEPTCODE.To<string>("").Trim();
                sDr = list.DRCODE.To<string>("").Trim();
                sOrderNo = list.ORDERNO;
                txtRemark1.Text = list.REMARK1.To<string>("").Trim();
                txtRemark2.Text = list.REMARK2.To<string>("").Trim();
                txtRemark3.Text = list.REMARK3.To<string>("").Trim();
                txtRemark4.Text = list.REMARK4.To<string>("").Trim();
            }
            strSName = "";
        }

        void etxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtPtNo)
            {
                txtPtNo.Text = string.Format("{0:00000000}", txtPtNo.Text);
                if (!VB.IsNumeric(txtPtNo.Text.Trim()))
                {
                    txtPtNo.Focus();
                    return;
                }
            }  
        }
    }
}
