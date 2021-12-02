using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ComBase.Controls
{
    public partial class MedieineCodeSearchText : UserControl
    {
        /// <summary>
        /// 그리드 데이터 선택 여부 
        /// Text Changed 이벤트 발생 관리
        /// </summary>
        private bool IsSelected = false;
        /// <summary>
        /// 자동완성 그리드
        /// </summary>
        public object DataList = null;
        /// <summary>
        /// 코드값
        /// </summary>
        public string ValueMember;
        /// <summary>
        /// 코드명 값
        /// </summary>
        public string DisplayMember;
        /// <summary>
        /// 조회옵션 
        /// SearchMode.Value = Where Code = :Code
        /// SearchMode.Display = Where CodeName = :CodeName
        /// SearchMode.All = Where Code = :Code OR CodeName = :CodeName
        /// </summary>
        public SearchMode SearchType = SearchMode.Value;
        /// <summary>
        /// 매핑 코드 컬럼
        /// </summary>
        public string ValueDataField;
        /// <summary>
        /// 매핑 코드명 컬럼
        /// </summary>
        public string DisplayDataField;
        /// <summary>
        /// 엔터시 실행 버튼
        /// </summary>
        public Button ExecuteButton;

        /// <summary>
        /// 코드 체인지 이벤트
        /// </summary>
        public event EventHandler TextChanged;
        /// <summary>
        /// Location 
        /// 기본으로 최상의 객체에서 부터 CodeSearch 부모 객체까지 Y 좌표를 구함
        /// 좌표값이 틀릴 경우 강제 세팅시 사용함
        /// </summary>
        public Point Position = Point.Empty;
        /// <summary>
        /// 스프레드 부모 객체
        /// 기본으로 CodeSearch.Parent.Parent 객체를 부모로 잡음
        /// 다를경우 강제 세팅으로 사용함
        /// </summary>
        public Control ParentControl = null;
        /// <summary>
        /// 데이터 바인딩 완료
        /// </summary>
        public delegate void DataSearchCompletedEventHandler(object sender, object data);
        public event DataSearchCompletedEventHandler DataSearchCompleted;

        public Control NextFocusControl = null;
        public MedieineCodeSearchText()
        {
            InitializeComponent();
        }

        private void GelSearchText_Load(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Start();
        }

        private void FindParent(ref List<Control> list, Control control)
        {
            list.Add(control.Parent);
            if (!(control.Parent is Form))
            { 
                FindParent(ref list, control.Parent);
            }
        }

        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                if(!timer1.Enabled)
                {
                    timer1.Stop();
                    timer1.Start();
                }
                SSSearch.Visible = true;
                SSSearch.Focus();
                SSSearch_Sheet1.SetActiveCell(0, 0);
            }
            else if(e.KeyCode == Keys.Enter)
            {
                SSSearch.Visible = true;

                TxtCode.Text = TxtCode.Text.Trim();
                TxtName.Text = string.Empty;
                if (!string.IsNullOrEmpty(TxtCode.Text))
                {
                    if (!timer1.Enabled)
                    {
                        timer1.Stop();
                        timer1.Start();
                    }

                    Control parent = this.TopLevelControl;
                    if (Position != Point.Empty)
                    {
                        SSSearch.Parent = ParentControl == null ? parent : ParentControl;
                        SSSearch.Location = Position;
                    }
                    else
                    {
                        List<Control> list = new List<Control>();
                        FindParent(ref list, this.Parent);
                        if (this.Parent != null)
                        {
                            int y = list.Sum(r => r.Location.Y) + this.Parent.Location.Y + this.Location.Y + this.TxtCode.Height;
                            int x = list.Sum(r => r.Location.X) + this.Parent.Location.X + this.Location.X;
                            SSSearch.Parent = list[list.Count - 1];
                            SSSearch.Location = new Point(x, y);
                        }
                    }
                }
                else
                {

                }

                var result = ((IList)DataList).Cast<object>();
                List<object> temp = null;
                if (this.SearchType == SearchMode.Value)
                {
                    temp = result.ToList().Where(r => r.GetPropertieValue(ValueMember).ToString().ToUpper().Trim().IndexOf(TxtCode.Text.ToUpper().Trim()) > -1).ToList();
                }
                else if (this.SearchType == SearchMode.Display)
                {
                    temp = result.ToList().Where(r => r.GetPropertieValue(DisplayMember).ToString().ToUpper().Trim().IndexOf(TxtName.Text.ToUpper().Trim()) > -1).ToList();
                }
                else
                {
                    temp = result.ToList().Where(r => r.GetPropertieValue(DisplayMember).ToString().ToUpper().Trim().IndexOf(TxtCode.Text.ToUpper().Trim()) > -1 || 
                            r.GetPropertieValue(ValueMember).ToString().ToUpper().Trim().IndexOf(TxtCode.Text.ToUpper().Trim()) > -1
                    ).ToList();
                }

                SSSearch.DataSource = temp;
                SSSearch.BringToFront();

                if (temp.Count() == 0)
                {
                    SSSearch.Visible = false;
                    timer1.Stop();
                    TxtCode.Focus();
                }
                else
                {
                    SSSearch.Focus();
                    SSSearch_Sheet1.SetActiveCell(0, 0);
                }
            }
        }

        private void SSSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyData == Keys.Up)
            {
                if(SSSearch_Sheet1.ActiveRowIndex == 0)
                {
                    TxtCode.Focus();
                }
            }
            else if(e.KeyData == Keys.Enter)
            {
                SetDataBind(SSSearch.GetActiveRowIndex());
            }
        }

        private void SetDataBind(int row)
        {
            timer1.Stop();

            object data = SSSearch.GetRowData(row);

            TxtCode.Text = data.GetPropertieValue(ValueMember).To<string>("").Trim();
            TxtName.Text = data.GetPropertieValue(DisplayMember).To<string>("").Trim();
            TxtName.Focus();

            if (DataSearchCompleted != null)
            {
                DataSearchCompleted(this, SSSearch.GetCurrentRowData());
            }

            if(this.NextFocusControl != null)
            {
                Timer timer = new Timer();
                timer.Interval = 100;
                timer.Tick += (ctl, evt) =>
                {
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
                    TxtName.Select(0, 0);
                    this.NextFocusControl.Focus();
                };

                timer.Start();
            }

            SSSearch.Visible = false;
        }

        private void SSSearch_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SetDataBind(e.Row);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(!TxtCode.Focused && !SSSearch.Focused)
            {
                SSSearch.Visible = false;
                timer1.Stop();
            }
            else
            {
                SSSearch.Visible = !TxtCode.Empty();
            }
        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            if(ExecuteButton != null)
            {
                ExecuteButton.PerformClick();
            }
        }

        public void SetDataList(object data)
        {
            DataList = data;
            SSSearch.DataSource = DataList;
        }

        public void Initialize()
        {
            TxtCode.Text = string.Empty;
            TxtName.Text = string.Empty;
            SSSearch.Visible = false;
        }

        public string GetValue()
        {
            return TxtCode.Text.Trim();
        }

        public string GetDisplay()
        {
            return TxtName.Text.Trim();
        }
        public void SetValueData(object obj)
        {
            TxtCode.Text = obj.ToString();
        }
        public void SetDisplayData(object obj)
        {
            TxtName.Text = obj.ToString();
        }

        public T GetCurrentDataRow<T>()
        {
            return (T)SSSearch.GetCurrentRowData();
        }

        private void MedieineCodeSearchText_Enter(object sender, EventArgs e)
        {
            TxtCode.Focus();
        }
    }
}
