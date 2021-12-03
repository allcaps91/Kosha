using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Xml;
using ComLibB;

//using EnterpriseDT.Net.Ftp;

namespace ComEmrDg
{
    public partial class frmItemValueSpdReg : Form
    {
        //이미지를 삭제한 경우
        public delegate void SetItemValueSpd(Control frm, Control ct, string strTag, string strText);
        public event SetItemValueSpd rSetItemValueSpd;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        private Control frmX = null;
        private Control ctX = null;
        private string mstrTag = "";

        public frmItemValueSpdReg()
        {
            InitializeComponent();
        }

        public frmItemValueSpdReg(Control frm, Control ct, string strTag)
        {
            InitializeComponent();
            frmX = frm;
            ctX = ct;
            mstrTag = strTag;
        }

        private void frmItemValueSpdReg_Load(object sender, EventArgs e)
        {
            string strITEMSPDSEQ = VB.Split(mstrTag, ":")[1];


            GetItemInfo(strITEMSPDSEQ);
        }

        private void GetItemInfo(string strITEMSPDSEQ)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = " SELECT A.ITEMSPDSEQ, A.ITEMCD, A.ITEMNO, A.SPDNAME, I.ITEMNAME ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRITEMSPD A";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRITEM I";
            SQL = SQL + ComNum.VBLF + "        ON A.ITEMNO = I.ITEMNO";
            SQL = SQL + ComNum.VBLF + "    WHERE A.ITEMSPDSEQ = " + VB.Val(strITEMSPDSEQ);
            SqlErr = clsDB.GetDataTable(ref dt, SQL);

            if (dt == null)
            {
                MessageBox.Show(new Form() { TopMost = true }, "조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            lblTitle.Text = ((Control)ctX).Text;
            string strSPDNAME = dt.Rows[0]["SPDNAME"].ToString().Trim();

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            DownDocFile(strSPDNAME);
        }

        private void DownDocFile(string strSPDNAME)
        {
            if (strSPDNAME == "") return;
            //clsWinScp.ConWinScp("Ftp", clsType.gSvrInfo.strServerIp, clsType.gSvrInfo.strUser, clsType.gSvrInfo.strPasswd, clsType.gSvrInfo.strSvrHomePath, "");
            //clsWinScp.gTrsResult = clsWinScp.gWinScp.GetFiles(clsType.gSvrInfo.strServerPath + "/ItemValue/" + strSPDNAME, clsType.gSvrInfo.strClient + "\\ItemValue\\" + strSPDNAME, false, clsWinScp.gTrsOptions);

            //if (File.Exists(clsType.gSvrInfo.strClient + "\\ItemValue\\" + strSPDNAME) == true)
            //{
            //    ssView.Open(clsType.gSvrInfo.strClient + "\\ItemValue\\" + strSPDNAME);
            //}
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            SetPromToText();
        }

        private void SetPromToText()
        {
            string strText = "";
            string strTextRow = "";
            string strTotal = "";
            string strDel = "";
            string strItemCd = "";
            string strItemNm = "";
            string strTitle = "";
            string strTitleSub = "";
            string strTab = "  ";

            int intRow = 0;
            int intCol = 0;
            int mintItemCd = 1;
            int mintTitle = 3;
            int mintDate = 4;

            for (intRow = 0; intRow < ssView.Sheets[0].RowCount; intRow++)
            {
                if (strItemCd.Trim() != ssView.Sheets[0].Cells[intRow, 0].Text.Trim())
                {
                    if (strTextRow.Trim() != "")
                    {
                        if (strTitleSub == "")
                        {
                            strText = strText + strTextRow;
                        }
                        else
                        {
                            strText = strText + strTitleSub + " : " + strTextRow;
                        }

                    }
                    if (strText.Trim() != "")
                    {
                        strText = "\r\n" + strTitle + "\r\n" + strText;
                        //텍스트를 표시를 한다.
                        if (strTotal == "")
                        {
                            strTotal = strText;
                        }
                        else
                        {
                            strTotal = strTotal + "\r\n" + strText;
                        }

                        strText = "";
                    }

                    strItemCd = ssView.Sheets[0].Cells[intRow, 0].Text.Trim();
                    strItemNm = ssView.Sheets[0].Cells[intRow, 1].Text.Trim();
                    strTitle = "";
                    strTitle = ssView.Sheets[0].Cells[intRow, 2].Text.Trim();
                    strTitleSub = "";
                    strTitleSub = ssView.Sheets[0].Cells[intRow, 3].Text.Trim();

                    strText = "";
                    strTextRow = "";
                    //strTitleSub에 아래줄을 검색을 해서 Data가 있는지 확인한다.
                    //한줄 검색
                    for (intCol = 4; intCol < ssView.Sheets[0].ColumnCount; intCol++)
                    {
                        if (ssView.Sheets[0].Cells[intRow, intCol].CellType != null)// Text 뿌리기
                        {
                            if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "TextCellType")
                            {
                                strTextRow = strTextRow + ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() + VB.Space(2); ;
                            }

                            if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "MultiOptionCellType")
                            {
                                strTextRow = strTextRow + ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() + VB.Space(2); ;
                            }

                            if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "CheckBoxCellType")
                            {
                                if (ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() == "True")
                                {

                                    FarPoint.Win.Spread.CellType.CheckBoxCellType ck = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                                    ck = (FarPoint.Win.Spread.CellType.CheckBoxCellType)ssView.ActiveSheet.Cells.Get(intRow, intCol).CellType;

                                    strTextRow = strTextRow + ck.Caption + VB.Space(2);
                                }
                            }

                            if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "ComboBoxCellType")
                            {
                                strTextRow = strTextRow + ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() + VB.Space(2); ;
                            }
                        }
                    }
                }
                else
                {
                    if (strItemNm.Trim() != ssView.Sheets[0].Cells[intRow, 1].Text.Trim())
                    {
                        if (strTextRow.Trim() != "")
                        {
                            if (strTitleSub == "")
                            {
                                strText = strText + strTextRow;
                            }
                            else
                            {
                                strText = strText + VB.Space(2) + strTitleSub + " : " + strTextRow;
                            }
                        }
                        strTextRow = "";
                        strItemNm = ssView.Sheets[0].Cells[intRow, 1].Text.Trim();
                        strTitleSub = "";
                        strTitleSub = ssView.Sheets[0].Cells[intRow, 3].Text.Trim();

                        for (intCol = 4; intCol < ssView.Sheets[0].ColumnCount; intCol++)
                        {
                            if (ssView.Sheets[0].Cells[intRow, intCol].CellType != null)// Text 뿌리기
                            {
                                if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "TextCellType")
                                {
                                    strTextRow = strTextRow + ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() + VB.Space(2); ;
                                }

                                if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "MultiOptionCellType")
                                {
                                    strTextRow = strTextRow + ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() + VB.Space(2); ;
                                }

                                if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "CheckBoxCellType")
                                {
                                    if (ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() == "True")
                                    {
                                        FarPoint.Win.Spread.CellType.CheckBoxCellType ck = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                                        ck = (FarPoint.Win.Spread.CellType.CheckBoxCellType)ssView.ActiveSheet.Cells.Get(intRow, intCol).CellType;

                                        strTextRow = strTextRow + ck.Caption + VB.Space(2);
                                    }
                                }

                                if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "ComboBoxCellType")
                                {
                                    strTextRow = strTextRow + ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() + VB.Space(2); ;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (intCol = 4; intCol < ssView.Sheets[0].ColumnCount; intCol++)
                        {
                            if (ssView.Sheets[0].Cells[intRow, intCol].CellType != null)// Text 뿌리기
                            {
                                if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "TextCellType")
                                {
                                    strTextRow = strTextRow + ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() + VB.Space(2); ;
                                }

                                if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "MultiOptionCellType")
                                {
                                    strTextRow = strTextRow + ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() + VB.Space(2); ;
                                }

                                if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "CheckBoxCellType")
                                {
                                    if (ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() == "True")
                                    {
                                        FarPoint.Win.Spread.CellType.CheckBoxCellType ck = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                                        ck = (FarPoint.Win.Spread.CellType.CheckBoxCellType)ssView.ActiveSheet.Cells.Get(intRow, intCol).CellType;

                                        strTextRow = strTextRow + ck.Caption + VB.Space(2);
                                    }
                                }

                                if (ssView.Sheets[0].Cells[intRow, intCol].CellType.ToString().Trim() == "ComboBoxCellType")
                                {
                                    strTextRow = strTextRow + ssView.Sheets[0].Cells[intRow, intCol].Text.Trim() + VB.Space(2); ;
                                }
                            }
                        }
                    }
                }
            }

            if (strTextRow.Trim() != "")
            {
                if (strTitleSub == "")
                {
                    strText = strText + strTextRow;
                }
                else
                {
                    strText = strText + strTitleSub + " : " + strTextRow;
                }

            }

            if (strText.Trim() != "")
            {
                strText = "\r\n" + strTitle + "\r\n" + strText;
                //텍스트를 표시를 한다.
                if (strTotal == "")
                {
                    strTotal = strText;
                }
                else
                {
                    strTotal = strTotal + "\r\n" + strText;
                }
            }

            if (strTotal.Trim() != "")
            {
                strTotal = lblTitle.Text.Trim() + "\r\n" + strTotal;
            }
            rSetItemValueSpd( frmX,  ctX,  mstrTag, strTotal);
        }
    }
}
