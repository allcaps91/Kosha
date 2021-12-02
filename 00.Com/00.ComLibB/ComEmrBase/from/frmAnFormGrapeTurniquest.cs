using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FarPoint.Win.Spread.CellType;

namespace ComEmrBase
{
    public partial class frmAnFormGrapeTurniquest : Form
    {
        //이벤트를 전달할 경우
        public delegate void GetInfo(string Info);
        public event GetInfo rGetInfo;

        //폼이 Close 될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        string strTag = string.Empty;

        public frmAnFormGrapeTurniquest()
        {
            InitializeComponent();
        }

        public frmAnFormGrapeTurniquest(string strTag)
        {
            InitializeComponent();
            this.strTag = strTag;
        }

        private void frmAnFormGrapeTurniquest_Load(object sender, EventArgs e)
        {
            Init();

            if (!string.IsNullOrEmpty(strTag))
            {
                SetData();
            }            
        }

        private void Init()
        {
            ssView.ActiveSheet.RowCount = 0;
            ssView.ActiveSheet.RowCount = 4;
            SetComboItem();
        }

        private void SetData()
        {
            int row = 0;
            string[] split = strTag.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string str in split)
            {
                string[] item = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                if (item.Length > 0)
                {
                    ssView.ActiveSheet.Cells[row, 0].Text = item[0];
                    ssView.ActiveSheet.Cells[row, 1].Text = item[1];
                    ssView.ActiveSheet.Cells[row, 2].Text = item[2];
                }

                row = row + 1;
            }            
        }

        private void SetComboItem()
        {            
            ListBox WoundList = new ListBox();
            WoundList.Items.Add("");
            WoundList.Items.Add("Arm");
            WoundList.Items.Add("Leg");

            ComboBoxCellType Type1 = new ComboBoxCellType();
            Type1.Clear();
            Type1.ListControl = WoundList;
            Type1.Editable = false;

            ListBox LRList = new ListBox();
            LRList.Items.Add("");
            LRList.Items.Add("Left");
            LRList.Items.Add("Right");

            ComboBoxCellType Type2 = new ComboBoxCellType();
            Type2.Clear();
            Type2.ListControl = LRList;
            Type2.Editable = false;

            ListBox UpDownList = new ListBox();
            UpDownList.Items.Add("");
            UpDownList.Items.Add("Up");
            UpDownList.Items.Add("Down");

            ComboBoxCellType Type3 = new ComboBoxCellType();
            Type3.Clear();
            Type3.ListControl = UpDownList;
            Type3.Editable = false;


            ssView.ActiveSheet.Columns[0].CellType = Type1;
            ssView.ActiveSheet.Columns[1].CellType = Type2;
            ssView.ActiveSheet.Columns[2].CellType = Type3;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strInfo = "";

            string strWound = "";
            string strLR = "";
            string strUpdown = "";

            for (int i = 0; i < ssView.ActiveSheet.RowCount; i++)
            {
                strWound = ssView.ActiveSheet.Cells[i, 0].Text;
                strLR = ssView.ActiveSheet.Cells[i, 1].Text;
                strUpdown = ssView.ActiveSheet.Cells[i, 2].Text;

                strInfo += string.Concat(strWound, ",", strLR, ",", strUpdown, "/");
            }
            
            rGetInfo(VB.Left(strInfo, strInfo.Length - 1));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }
    }
}
