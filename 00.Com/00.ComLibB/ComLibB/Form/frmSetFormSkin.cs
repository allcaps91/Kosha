using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmSetFormSkin : Form
    {
        public frmSetFormSkin()
        {
            InitializeComponent();
        }

        private void frmSetFormSkin_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //Form 권한조회
            {
                this.Close();
                return;
            }       
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅
            GetUserSkinX();
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (SaveDataUser() == true)
            {

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetUserSkinX()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "     AND OPTIONGB = 'UserFormSkin' ";
            dt = ComQuery.Select_BAS_USEROPTION(clsDB.DbCon, SQL);

            if (dt == null)
            {
                optOption0.Checked = true;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                optOption0.Checked = true;
                return;
            }

            int intNVALUE1 = 0;
            intNVALUE1 = (int)VB.Val(dt.Rows[0]["NVALUE1"].ToString().Trim());
            dt.Dispose();
            dt = null;

            if (intNVALUE1 == 1)
            {
                optOption1.Checked = true;
            }
            else if (intNVALUE1 == 2)
            {
                optOption2.Checked = true;
            }
            else
            {
                optOption0.Checked = true;
            }
        }

        private bool SetUserSkinX()
        {
            SetDefaultSkin();

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "     AND OPTIONGB IN ('SetFormTitle', 'SetFormPanTitle', 'SetFormSubTitle', 'SetFormPanSubTitle', 'SetFormSpread', 'SetFormButton') ";
            dt = ComQuery.Select_BAS_USEROPTION(clsDB.DbCon, SQL);

            if (dt == null)
            {
                return false;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return false;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormTitle")
                {
                    cboTitle.SelectedIndex = (int)VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim());
                    if (cboTitle.SelectedIndex != 0)
                    {
                        if (VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim()) != 0)
                        {
                            ColorTitle.BackColor = ColorTranslator.FromWin32((int)VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim()));
                        }
                    }
                }
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormPanTitle")
                {
                    cboPanTitle.SelectedIndex = (int)VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim());
                    if (cboPanTitle.SelectedIndex != 0)
                    {
                        if (VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim()) != 0)
                        {
                            ColorPanTitle.BackColor = ColorTranslator.FromWin32((int)VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim()));
                        }
                    }
                }
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormSubTitle")
                {
                    cboSubTitle.SelectedIndex = (int)VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim());
                    if (cboSubTitle.SelectedIndex != 0)
                    {
                        if (VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim()) != 0)
                        {
                            ColorSubTitle.BackColor = ColorTranslator.FromWin32((int)VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim()));
                        }
                    }
                }
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormPanSubTitle")
                {
                    cboPanSubTitle.SelectedIndex = (int)VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim());
                    if (cboPanSubTitle.SelectedIndex != 0)
                    {
                        if (VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim()) != 0)
                        {
                            ColorPanSubTitle.BackColor = ColorTranslator.FromWin32((int)VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim()));
                        }
                    }
                }
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormSpread")
                {
                    cboSpread.SelectedIndex = (int)VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim());
                    if (cboSpread.SelectedIndex == 0)
                    {
                        if (VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim()) != 0)
                        {
                            ColorSpread.BackColor = ColorTranslator.FromWin32((int)VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim()));
                        }
                    }
                }
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormButton")
                {
                    cboButton.SelectedIndex = (int)VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim());
                }
            }
            dt.Dispose();
            dt = null;

            return true;
        }

        private void SetDefaultSkin()
        {
            cboTitle.SelectedIndex = 0;
            SetlblTitle.ForeColor = Color.Black;
            cboPanTitle.SelectedIndex = 0;
            SetpanTitle.BackColor = Color.White;
            cboSubTitle.SelectedIndex = 0;
            SetlblSubTitle.ForeColor = Color.White;
            cboPanSubTitle.SelectedIndex = 0;
            SetpanSubTitle.BackColor = Color.RoyalBlue;
            cboSpread.SelectedIndex = 0;
            SetSpreadSkinDefault(SetSpread);
            cboButton.SelectedIndex = 0;
            SetButton1.FlatStyle = FlatStyle.Flat;
            SetButton2.FlatStyle = FlatStyle.Flat;
        }

        private void cboTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTitle.SelectedIndex == 0)
            {
                ColorTitle.BackColor = Color.Black;
                ColorTitle.Visible = false;
                SetlblTitle.ForeColor = Color.Black; 
            }
            else
            {
                ColorTitle.BackColor = Color.Black;
                ColorTitle.Visible = true;
                SetlblTitle.ForeColor = Color.Black;
            }
        }

        private void cboPanTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPanTitle.SelectedIndex == 0)
            {
                ColorPanTitle.BackColor = Color.White;
                ColorPanTitle.Visible = false;
                SetpanTitle.BackColor = Color.White;
            }
            else
            {
                ColorPanTitle.BackColor = Color.White;
                ColorPanTitle.Visible = true;
                SetpanTitle.BackColor = Color.White;
            }
        }

        private void cboSubTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSubTitle.SelectedIndex == 0)
            {
                ColorSubTitle.BackColor = Color.White;
                ColorSubTitle.Visible = false;
                SetlblSubTitle.ForeColor = Color.White;
            }
            else
            {
                ColorSubTitle.BackColor = Color.White;
                ColorSubTitle.Visible = true;
                SetlblSubTitle.ForeColor = Color.White;
            }
        }

        private void cboPanSubTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPanSubTitle.SelectedIndex == 0)
            {
                ColorPanSubTitle.BackColor = Color.RoyalBlue;
                ColorPanSubTitle.Visible = false;
                SetpanSubTitle.BackColor = Color.RoyalBlue;
            }
            else
            {
                ColorPanSubTitle.BackColor = Color.RoyalBlue;
                ColorPanSubTitle.Visible = true;
                SetpanSubTitle.BackColor = Color.RoyalBlue;
            }
        }

        private void cboSpread_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSpread.SelectedIndex == 0)
            {
                ColorSpread.BackColor = Color.White; // SystemColors.Control;
                ColorSpread.Visible = true;
                SetSpreadSkinDefault(SetSpread);
            }
            else
            {
                ColorSpread.BackColor = Color.White; // SystemColors.Control;
                ColorSpread.Visible = false;
                SetSpreadSkinOffice(SetSpread);
                //SetSpread_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
                SetSpreadSheetSkinColor(SetSpread_Sheet1, System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247))))));
            }
        }

        private void cboButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboButton.SelectedIndex == 0)
            {
                SetButton1.FlatStyle = FlatStyle.Flat;
                SetButton2.FlatStyle = FlatStyle.Flat;
            }
            else
            {
                SetButton1.FlatStyle = FlatStyle.System;
                SetButton2.FlatStyle = FlatStyle.System;
            }
        }

        private void SetSpreadSkinDefault(FarPoint.Win.Spread.FpSpread Spread)
        {
            Spread.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Default;
            SetSpreadSheetSkinColor(SetSpread_Sheet1, Color.White);
            //FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            //// 
            //// SetSpread
            //// 
            ////Spread.AccessibleDescription = "SetSpread, Sheet1, Row 0, Column 0, ";
            ////Spread.Dock = System.Windows.Forms.DockStyle.Fill;
            ////Spread.Location = new System.Drawing.Point(0, 102);
            ////Spread.Name = "SetSpread";
            ////Spread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            ////Spread.Sheets[0]});
            ////Spread.Size = new System.Drawing.Size(520, 397);
            ////Spread.TabIndex = 111;
            //// 
            //// SetSpread_Sheet1
            //// 
            ////Spread.Sheets[0].Reset();
            ////Spread.Sheets[0].SheetName = "Sheet1";
            //// Formulas and custom names must be loaded with R1C1 reference style
            //Spread.Sheets[0].ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            //Spread.Sheets[0].FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
            //Spread.Sheets[0].FilterBarHeaderStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            //Spread.Sheets[0].FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //Spread.Sheets[0].FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            //enhancedRowHeaderRenderer1.BackColor = System.Drawing.SystemColors.Control;
            //enhancedRowHeaderRenderer1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //enhancedRowHeaderRenderer1.ForeColor = System.Drawing.SystemColors.ControlText;
            //enhancedRowHeaderRenderer1.Name = "";
            //enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            //enhancedRowHeaderRenderer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            //enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            //enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            //Spread.Sheets[0].FilterBarHeaderStyle.Renderer = enhancedRowHeaderRenderer1;
            //Spread.Sheets[0].FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //Spread.Sheets[0].FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            //Spread.Sheets[0].RowHeader.Columns.Default.Resizable = false;
            //Spread.Sheets[0].SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].SheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            //Spread.Sheets[0].SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
        }

        private void SetSpreadSkinOffice(FarPoint.Win.Spread.FpSpread Spread)
        {
            Spread.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            //FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer1 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            //FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            //FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            //FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            //// 
            //// SetSpread
            //// 
            ////Spread.AccessibleDescription = "SetSpread, Sheet1, Row 0, Column 0, ";
            ////Spread.Dock = System.Windows.Forms.DockStyle.Fill;
            //Spread.FocusRenderer = enhancedFocusIndicatorRenderer1;
            //Spread.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            //Spread.HorizontalScrollBar.Name = "";
            //Spread.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            //Spread.HorizontalScrollBar.TabIndex = 3;
            ////Spread.Location = new System.Drawing.Point(0, 102);
            ////Spread.Name = "SetSpread";
            ////Spread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            ////Spread.Sheets[0]});
            ////Spread.Size = new System.Drawing.Size(520, 397);
            //Spread.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            ////Spread.TabIndex = 111;
            //Spread.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            //Spread.VerticalScrollBar.Name = "";
            //Spread.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            //Spread.VerticalScrollBar.TabIndex = 4;
            //// 
            //// Spread.Sheet[0]
            //// 
            //Spread.Sheets[0].Reset();
            //Spread.Sheets[0].SheetName = "Sheet1";
            //// Formulas and custom names must be loaded with R1C1 reference style
            //Spread.Sheets[0].ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            //Spread.Sheets[0].ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            //Spread.Sheets[0].ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            //Spread.Sheets[0].ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            //Spread.Sheets[0].ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            //Spread.Sheets[0].FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
            //Spread.Sheets[0].FilterBarHeaderStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            //Spread.Sheets[0].FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //Spread.Sheets[0].FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //enhancedRowHeaderRenderer1.Name = "";
            //enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            //enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            //enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            //Spread.Sheets[0].FilterBarHeaderStyle.Renderer = enhancedRowHeaderRenderer1;
            //Spread.Sheets[0].FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //Spread.Sheets[0].FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            //Spread.Sheets[0].RowHeader.Columns.Default.Resizable = false;
            //Spread.Sheets[0].RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            //Spread.Sheets[0].RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
        }

        private void SetSpreadSheetSkinDefault(FarPoint.Win.Spread.SheetView SpdSheet)
        {
            //FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            SpdSheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Default;
        }

        private void SetSpreadSheetSkinOffice(FarPoint.Win.Spread.SheetView SpdSheet)
        {
            //FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            SpdSheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Default;
        }
        
        private void ColorTitle_BackColorChanged(object sender, EventArgs e)
        {
            if (cboTitle.SelectedIndex == 0)
            {
                return;
            }
            SetlblTitle.ForeColor = ColorTitle.BackColor;
        }

        private void ColorPanTitle_BackColorChanged(object sender, EventArgs e)
        {
            if (cboPanTitle.SelectedIndex == 0)
            {
                return;
            }
            SetpanTitle.BackColor = ColorPanTitle.BackColor;
        }

        private void ColorSubTitle_BackColorChanged(object sender, EventArgs e)
        {
            if (cboSubTitle.SelectedIndex == 0)
            {
                return;
            }
            SetlblSubTitle.ForeColor = ColorSubTitle.BackColor;
        }

        private void ColorPanSubTitle_BackColorChanged(object sender, EventArgs e)
        {
            if (cboPanSubTitle.SelectedIndex == 0)
            {
                return;
            }
            SetpanSubTitle.BackColor = ColorPanSubTitle.BackColor;
        }

        private void ColorSpread_BackColorChanged(object sender, EventArgs e)
        {
            if (cboSpread.SelectedIndex == 0)
            {
                SetSpreadSheetSkinColor(SetSpread_Sheet1, ColorSpread.BackColor);
            }
            else
            {
                SetSpreadSheetSkinColor(SetSpread_Sheet1, Color.White);
            }
        }

        private void SetSpreadSheetSkinColor(FarPoint.Win.Spread.SheetView SpdSheet, Color pColor)
        {
            for (int i = 0; i < SpdSheet.Columns.Count; i++)
            {
                for (int j = 0; j < SpdSheet.ColumnHeader.RowCount; j++)
                {
                    SpdSheet.ColumnHeader.Cells[j,i].BackColor = pColor;
                }
            }
            for (int i = 0; i < SpdSheet.Rows.Count; i++)
            {
                for (int j = 0; j < SpdSheet.RowHeader.ColumnCount; j++)
                {
                    SpdSheet.RowHeader.Cells[i, j].BackColor = pColor;
                }
            }
        }

        private void ColorTitle_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                colorDialog1.ShowDialog();
                ColorTitle.BackColor = colorDialog1.Color;
            }
            catch { }
        }

        private void ColorPanTitle_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                colorDialog1.ShowDialog();
                ColorPanTitle.BackColor = colorDialog1.Color;
            }
            catch { }
        }

        private void ColorSubTitle_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                colorDialog1.ShowDialog();
                ColorSubTitle.BackColor = colorDialog1.Color;
            }
            catch { }
        }

        private void ColorPanSubTitle_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                colorDialog1.ShowDialog();
                ColorPanSubTitle.BackColor = colorDialog1.Color;
            }
            catch { }
        }

        private void ColorSpread_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                colorDialog1.ShowDialog();
                ColorSpread.BackColor = colorDialog1.Color;
            }
            catch { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DeleteData() == true)
            {

            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            SetUserSkinX();
        }
        
        bool SaveDataUser()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                string strOPTIONGB = "";
                string strVVALUE1 = "";
                string strVVALUE2 = "";
                string strVVALUE3 = "";
                string strNVALUE1 = "0";
                string strNVALUE2 = "0";
                string strNVALUE3 = "0";

                strOPTIONGB = "UserFormSkin";
                if (optOption1.Checked == true)
                {
                    strNVALUE1 = "1";
                }
                else if (optOption2.Checked == true)
                {
                    strNVALUE1 = "2";
                }
                
                if (ComQuery.UpdateFormUserSet(clsDB.DbCon, strOPTIONGB, strVVALUE1, strVVALUE2, strVVALUE3, strNVALUE1, strNVALUE2, strNVALUE3) == false)
                {
                    return false;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        bool SaveData()
        {
            string SQL = "";    //Query문
            //string SqlErr = ""; //에러문 받는 변수
            //int intRowAffected = 0; //변경된 Row 받는 변수

            string strOPTIONGB = "";
            string strVVALUE1 = "";
            string strVVALUE2 = "";
            string strVVALUE3 = "";
            string strNVALUE1 = "0";
            string strNVALUE2 = "0";
            string strNVALUE3 = "0";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //SetFormTitle  cboTitle
                strOPTIONGB = "SetFormTitle";
                strNVALUE1 = cboTitle.SelectedIndex.ToString();
                if (cboTitle.SelectedIndex != 0)
                {
                    strNVALUE2 = (ColorTranslator.ToWin32(ColorTitle.BackColor)).ToString();
                }
                if(ComQuery.UpdateFormUserSet(clsDB.DbCon, strOPTIONGB, strVVALUE1, strVVALUE2, strVVALUE3, strNVALUE1, strNVALUE2, strNVALUE3) == false)
                {
                    return false;
                }

                //SetFormPanTitle  cboPanTitle
                strOPTIONGB = "SetFormPanTitle";
                strNVALUE1 = cboPanTitle.SelectedIndex.ToString();
                if (cboPanTitle.SelectedIndex != 0)
                {
                    strNVALUE2 = (ColorTranslator.ToWin32(ColorPanTitle.BackColor)).ToString();
                }
                if (ComQuery.UpdateFormUserSet(clsDB.DbCon, strOPTIONGB, strVVALUE1, strVVALUE2, strVVALUE3, strNVALUE1, strNVALUE2, strNVALUE3) == false)
                {
                    return false;
                }

                //SetFormSubTitle  cboSubTitle
                strOPTIONGB = "SetFormSubTitle";
                strNVALUE1 = cboSubTitle.SelectedIndex.ToString();
                if (cboSubTitle.SelectedIndex != 0)
                {
                    strNVALUE2 = (ColorTranslator.ToWin32(ColorSubTitle.BackColor)).ToString();
                }
                if (ComQuery.UpdateFormUserSet(clsDB.DbCon, strOPTIONGB, strVVALUE1, strVVALUE2, strVVALUE3, strNVALUE1, strNVALUE2, strNVALUE3) == false)
                {
                    return false;
                }

                //SetFormPanSubTitle  cboPanSubTitle
                strOPTIONGB = "SetFormPanSubTitle";
                strNVALUE1 = cboPanSubTitle.SelectedIndex.ToString();
                if (cboPanSubTitle.SelectedIndex != 0)
                {
                    strNVALUE2 = (ColorTranslator.ToWin32(ColorPanSubTitle.BackColor)).ToString();
                }
                if (ComQuery.UpdateFormUserSet(clsDB.DbCon, strOPTIONGB, strVVALUE1, strVVALUE2, strVVALUE3, strNVALUE1, strNVALUE2, strNVALUE3) == false)
                {
                    return false;
                }

                //SetFormSpread  cboSpread
                strOPTIONGB = "SetFormSpread";
                strNVALUE1 = cboSpread.SelectedIndex.ToString();
                if (cboSpread.SelectedIndex == 0)
                {
                    strNVALUE2 = (ColorTranslator.ToWin32(ColorSpread.BackColor)).ToString();
                }
                if (ComQuery.UpdateFormUserSet(clsDB.DbCon, strOPTIONGB, strVVALUE1, strVVALUE2, strVVALUE3, strNVALUE1, strNVALUE2, strNVALUE3) == false)
                {
                    return false;
                }

                //SetFormButton  cboButton
                strOPTIONGB = "SetFormButton";
                strNVALUE1 = cboButton.SelectedIndex.ToString();
                if (cboButton.SelectedIndex != 0)
                {
                    strNVALUE2 = (ColorTranslator.ToWin32(ColorButton.BackColor)).ToString();
                }
                if (ComQuery.UpdateFormUserSet(clsDB.DbCon, strOPTIONGB, strVVALUE1, strVVALUE2, strVVALUE3, strNVALUE1, strNVALUE2, strNVALUE3) == false)
                {
                    return false;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        bool DeleteData()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_PMPA + "BAS_USEROPTION ";
                SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "     AND OPTIONGB IN ('SetFormTitle', 'SetFormPanTitle', 'SetFormSubTitle', 'SetFormPanSubTitle', 'SetFormSpread', 'SetFormButton') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void optOption0_CheckedChanged(object sender, EventArgs e)
        {
            if (optOption0.Checked == true)
            {
                //panHide1.Top = panOption1.Top;
                //panHide1.Left = panOption1.Left;
                //panHide1.Width = panOption1.Width;
                //panHide1.Height = panOption1.Height;

                //panHide2.Top = panOption2.Top;
                //panHide2.Left = panOption2.Left;
                //panHide2.Width = panOption2.Width;
                //panHide2.Height = panOption2.Height;

                //panHide3.Top = panOption3.Top;
                //panHide3.Left = panOption3.Left;
                //panHide3.Width = panOption3.Width;
                //panHide3.Height = panOption3.Height;

                //panHide1.Visible = false;
                //panHide2.Visible = true;
                //panHide3.Visible = true;
                panOption1.Visible = true;
                panOption2.Visible = false;
                panOption3.Visible = false;
            }
        }

        private void optOption1_CheckedChanged(object sender, EventArgs e)
        {
            if (optOption1.Checked == true)
            {
                //panHide1.Top = panOption1.Top;
                //panHide1.Left = panOption1.Left;
                //panHide1.Width = panOption1.Width;
                //panHide1.Height = panOption1.Height;

                //panHide2.Top = panOption2.Top;
                //panHide2.Left = panOption2.Left;
                //panHide2.Width = panOption2.Width;
                //panHide2.Height = panOption2.Height;

                //panHide3.Top = panOption3.Top;
                //panHide3.Left = panOption3.Left;
                //panHide3.Width = panOption3.Width;
                //panHide3.Height = panOption3.Height;

                //panHide1.Visible = true;
                //panHide2.Visible = false;
                //panHide3.Visible = true;
                panOption1.Visible = false;
                panOption2.Visible = true;
                panOption3.Visible = false;
            }
        }

        private void optOption2_CheckedChanged(object sender, EventArgs e)
        {
            if (optOption2.Checked == true)
            {
                //panHide1.Top = panOption1.Top;
                //panHide1.Left = panOption1.Left;
                //panHide1.Width = panOption1.Width;
                //panHide1.Height = panOption1.Height;

                //panHide2.Top = panOption2.Top;
                //panHide2.Left = panOption2.Left;
                //panHide2.Width = panOption2.Width;
                //panHide2.Height = panOption2.Height;

                //panHide3.Top = panOption3.Top;
                //panHide3.Left = panOption3.Left;
                //panHide3.Width = panOption3.Width;
                //panHide3.Height = panOption3.Height;

                //panHide1.Visible = true;
                //panHide2.Visible = true;
                //panHide3.Visible = false;

                panOption1.Visible = false;
                panOption2.Visible = false;
                panOption3.Visible = true;

                SetUserSkinX();
            }
        }

        
    }
}
