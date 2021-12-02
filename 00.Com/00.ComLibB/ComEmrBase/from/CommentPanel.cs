using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class CommentPanel : Panel
    {
		private const int WS_EX_TRANSPARENT = 0x20;


		private int opacity = 30;
		[DefaultValue(30)]
		public int Opacity
		{
			get { return opacity; }
			set
			{
				if (value < 0 || value > 100)
				{
					throw new ArgumentException("dddddd");
				}

				this.opacity = value;

			}
		}


		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cpar = base.CreateParams;
				cpar.ExStyle = cpar.ExStyle | WS_EX_TRANSPARENT;

				return cpar;
			}
		}

		public CommentPanel()
		{
			InitializeComponent();

			SetStyle(ControlStyles.Opaque, true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			using (var brush = new SolidBrush(Color.FromArgb(this.opacity * 255 / 100, this.BackColor)))
			{
				e.Graphics.FillRectangle(brush, this.ClientRectangle);
			}

			base.OnPaint(e);
		}
	}
}
