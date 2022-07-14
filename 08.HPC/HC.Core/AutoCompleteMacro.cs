using ComBase.Controls;
using ComBase.Mvc.Enums;
using HC.Core.Dto;
using HC.Core.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HC_Core
{
    public partial class AutoCompleteMacro : UserControl
    {
        public Point Position = Point.Empty;
        public Control ParentControl = null;
        List<TextBox> textBoxList = null;
        private MacrowordService macrowordService;
        private string formName;
        private string controlId;
        private TextBox currentTextBox;

        /// <summary>
        /// 
        /// </summary>
        private bool IsManual { get; set; }
        public AutoCompleteMacro()
        {
            InitializeComponent();
            this.formName = Name;
            textBoxList = new List<TextBox>();
            macrowordService = new MacrowordService();
            IsManual = true;
        }
        public AutoCompleteMacro(string formName)
        {
            InitializeComponent();
            this.formName = formName;
            textBoxList = new List<TextBox>();
            macrowordService = new MacrowordService();
        }
        public void Add(TextBox textBox)
        {
            
            textBox.Leave += TextBox_Leave;
            textBox.DoubleClick += TextBox_Click1;
            //textBox.Click += TextBox_Click1;
            //     textBox.TextChanged += TextBox_TextChanged;
            textBoxList.Add(textBox);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            List<MacrowordDto> list = macrowordService.FindAll(formName, controlId, tb.Text);
            SSMacro.ActiveSheet.RowCount = list.Count;
            for (int i=0; i<list.Count; i++)
            {
                SSMacro.ActiveSheet.Cells[i, 0].Value = list[i].TITLE;
            }
            //SSMacro.SetDataSource(list);
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            //HideMacro();
        }
        private void HideMacro()
        {
            if(currentTextBox.Focused == false && SSMacro.Focused == false && BtnMacro.Focused == false)
            {
                this.Hide();
            }
        }
        private void TextBox_Click1(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            currentTextBox = tb;


         //   this.Parent = tb.TopLevelControl;  

            //if (tb.Parent.Parent != null)
            //{
            //    this.Parent = tb.Parent.Parent;
            //    if (tb.Parent.Parent.Parent != null)
            //    {
            //        this.Parent = tb.Parent.Parent.Parent;
            //    }

            //}
            
          
            if (IsManual == false)
            {
                Point pts = tb.TopLevelControl.PointToScreen(tb.Location);
                this.Location = new Point(pts.X - (TopLevelControl.Location.X - 210), tb.Location.Y - (TopLevelControl.Location.Y - 105));
            }
                //if (TopLevelControl == null)
                //{
                //    this.Parent = tb.Parent.Parent;
                //    this.Location = pts;
                //   //this.Location = new Point(pts.X - (tmp.Location.X - 210), tb.Location.Y - (tmp.Location.Y - 105));
                //}
                //else
                //{
                //    this.Location = new Point(pts.X - (TopLevelControl.Location.X - 210), tb.Location.Y - (TopLevelControl.Location.Y - 105));
                //}
             
            // this.Location = new Point(tb.Location.X, tb.Location.Y + tb.Height);
            //if (tb.Parent is Panel || tb.Parent is GroupBox)
            //{
            //    int height = tb.Parent.Height;
            //    if (tb.Location.Y > height / 2)
            //    {
            //        this.Location = new Point(tb.Location.X, tb.Location.Y - (tb.Height +this.Height));
            //    }
            //}

            this.Show();
            this.BringToFront();
            this.controlId = tb.Name;
            Search();
        }


        private void AutoCompleteMacro_Load(object sender, EventArgs e)
        {
            SSMacro.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight=20 });
            SSMacro.AddColumnText("상용구", nameof(MacrowordDto.TITLE), 228, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

        }
     
        private void Search()
        {
            List<MacrowordDto>  list = macrowordService.FindAll(formName, controlId);
            SSMacro.SetDataSource(list);
        }
     

        private void SSMacro_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            MacrowordDto dto = SSMacro.GetRowData(e.Row) as MacrowordDto;
            currentTextBox.Text = dto.CONTENT;
            this.Hide();
            currentTextBox.Focus();
        }

        private void BtnMacro_Click(object sender, EventArgs e)
        {
            MacrowordForm form = new MacrowordForm(this.formName, this.controlId);
            form.ShowDialog();

            Search();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentTextBox != null)
            {
                currentTextBox.Focus();
            }
            
            this.Hide();
          

        }
    }
}
