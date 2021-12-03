using ComBase;
using FarPoint.Win;
using FarPoint.Win.Chart;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Chart;
using FarPoint.Win.Spread.Model;
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
    public partial class frmAnForm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnCount"></param>
        private void SetGrapeInit(int columnCount = 36)
        {
            ssView.ActiveSheet.Columns.Clear();
            ssView.ActiveSheet.Rows.Clear();

            //  스프레드 컬럼/로우 설정
            ssView.ActiveSheet.Columns.Count = columnCount;
            ssView.ActiveSheet.Rows.Count = 200;
            ssView.ActiveSheet.Rows[0].MergePolicy = MergePolicy.Always;

            //  스프레드 상단 시간설정 및 콤보박스 아이템 설정
            SetGrapeTime(2);
            //  콤보박스 첫번쨰로 설정
            cboTime.SelectedIndex = 0;

            ssView.ActiveSheet.Rows.Get(0).Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);
            ssView.ActiveSheet.Rows.Get(1).Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);

            //  컬럼고정
            ssView.ActiveSheet.FrozenColumnCount = 2;

            //  View Spread 항목설정
            List<MedcationInfo> list = Medcations.ToList().FindAll(r => r.IsView);
            ViewItemCount = list.Count;

            #region 그래프 설정

            //  그래프 설정
            XYPointSeries xyPointSeries = new XYPointSeries();
            SpChart = new SpreadChart(xyPointSeries, "SpreadChart1");
            ssView.ActiveSheet.DrawingContainer.ContainedObjects.Add(SpChart);

            SpChart.IgnoreUpdateShapeLocation = false;
            SpChart.IsGrayscale = false;

            //LegendArea legendArea1 = new LegendArea();
            //spreadChart.Model.LegendAreas.AddRange(new LegendArea[] { legendArea1 });

            YPlotArea = new YPlotArea
            {
                Location = new PointF(0.083F, 0.05F),
                Size = new SizeF(0.86F, 0.8F)
            };

            SpChart.Model.PlotAreas.AddRange(new PlotArea[] { YPlotArea });

            SBPSeries = new PointSeries
            {
                SeriesName = "SBP",
                LabelVisible = true,
                PointBorder = new NoLine(),
                PointFill = Properties.Resources.Chart_SBP
            };
            SBPSeries.Values.DataSource = new SeriesDataField(ssView, "SBP", "Sheet1!$C$101:$AL$101", SegmentDataType.AutoIndex, new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps });

            DBPSeries = new PointSeries
            {
                SeriesName = "DBP",
                LabelVisible = true,
                PointBorder = new NoLine(),
                PointFill = Properties.Resources.Chart_DBP
            };
            DBPSeries.Values.DataSource = new SeriesDataField(ssView, "DBP", "Sheet1!$C$102:$AL$102", SegmentDataType.AutoIndex, new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps });

            PulseSeries = new PointSeries
            {
                SeriesName = "맥박",
                LabelVisible = true,
                PointBorder = new NoLine(),
                PointFill = Properties.Resources.Chart_Pulse
            };
            PulseSeries.Values.DataSource = new SeriesDataField(ssView, "맥박", "Sheet1!$C$103:$AL$103", SegmentDataType.AutoIndex, new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps });

            BreathSeries = new PointSeries
            {
                SeriesName = "호흡",
                LabelVisible = true,
                PointBorder = new NoLine(),
                PointFill = Properties.Resources.Chart_Breath
            };
            BreathSeries.Values.DataSource = new SeriesDataField(ssView, "호흡", "Sheet1!$C$104:$AL$104", SegmentDataType.AutoIndex, new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps });

            YPlotArea.Series.AddRange(new Series[] {
                    SBPSeries,
                    DBPSeries,
                    PulseSeries,
                    BreathSeries,
                });

            IndexAxis indexAxis1 = new IndexAxis { LabelTextDirection = TextDirection.Rotate90Degree };
            ValueAxis valueAxis1 = new ValueAxis();
            YPlotArea.XAxis = indexAxis1;
            YPlotArea.YAxes.Clear();
            YPlotArea.YAxes.AddRange(new ValueAxis[] { valueAxis1 });

            int y = (ViewItemCount * 15) + 50;
            SpChart.Rectangle = new Rectangle(30, y, 1050, 600);
            SpChart.SheetName = "fpSpread1_Sheet1";

            YPlotLocation = YPlotArea.Location;
            YPlotSize = YPlotArea.Size;
            ChartRect = SpChart.Rectangle;

            #endregion
        }

        /// <summary>
        /// 스프레드 및 그래프 시간 설정
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hhmm"></param>
        private void SetGrapeTime(int index, string hhmm = "")
        {
            DateTime dtp;
            string prevHour = string.Empty;
            if (string.IsNullOrWhiteSpace(hhmm))
            {
                dtp = dtpNow.Value;
            }
            else
            {
                string dateTime = dtpNow.Value.ToString("yyyyMMdd") + hhmm;
                dtp = DateTime.ParseExact(dateTime, "yyyyMMddHHmm", null);
                prevHour = dtp.ToString("HH");

                dtp = dtp.AddMinutes(5);
            }

            ComplexBorder complexBorder = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);
            for (int i = index; i < ssView.ActiveSheet.Columns.Count; i++)
            {
                if (!prevHour.Equals(dtp.ToString("HH")))
                {
                    ssView.ActiveSheet.Columns.Get(i).Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);
                    ssView.ActiveSheet.Cells[0, i].Border = complexBorder;
                    ssView.ActiveSheet.Cells[1, i].Border = complexBorder;
                }

                prevHour = dtp.ToString("HH");

                Column c = ssView.ActiveSheet.Columns[i];
                c.CellType = new TextCellType();
                c.VerticalAlignment = CellVerticalAlignment.Center;
                c.HorizontalAlignment = CellHorizontalAlignment.Center;

                c.Width = 25;

                cboTime.Items.Add(dtp.ToString("HH:mm"));
                ssView.ActiveSheet.Cells[0, i].Text = dtp.ToString("HH");
                ssView.ActiveSheet.Cells[1, i].Text = dtp.ToString("mm");

                dtp = dtp.AddMinutes(5);
            }
        }

        /// <summary>
        /// 그래프 마커 설정
        /// </summary>
        /// <param name="index"></param>
        private void SetGrapePointMarkers(int index)
        {
            int max = ssView.ActiveSheet.Columns.Count - 2;
            int plus = 2;

            if (index > 0)
            {
                max = ssView.ActiveSheet.Columns.Count;
                plus = 0;
            }

            for (int i = index; i < max; i++)
            {
                string hhmm = string.Concat(ssView.ActiveSheet.Cells[0, i + plus].Text, ":", ssView.ActiveSheet.Cells[1, i + plus].Text);
                SBPSeries.CategoryNames.Add(hhmm);
                //  마크 사이즈 설정
                SBPSeries.PointMarkers.SetMarker(i, new BuiltinMarker(MarkerShape.Square, 10F));
                DBPSeries.PointMarkers.SetMarker(i, new BuiltinMarker(MarkerShape.Square, 10F));
                PulseSeries.PointMarkers.SetMarker(i, new BuiltinMarker(MarkerShape.Square, 10F));
                BreathSeries.PointMarkers.SetMarker(i, new BuiltinMarker(MarkerShape.Square, 10F));
            }
        }

        /// <summary>
        /// 그래프 데이터 저장
        /// </summary>
        /// <param name="dblEmrHisNo"></param>
        /// <param name="emrNo"></param>
        /// <returns></returns>
        private bool SetGrapeSave(double dblEmrHisNo, string emrNo)
        {
            string item = string.Empty;
            string unit = string.Empty;
            string itemCode = string.Empty;
            string hour = string.Empty;
            string minute = string.Empty;
            string value = string.Empty;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;


            try
            {
                int maxCount = Medcations.ToList().FindAll(r => r.IsView).Count + 2;

                for (int i = 2; i < maxCount; i++)
                {
                    if (i < 100)
                    {
                        item = ssView.ActiveSheet.Cells[i, 0].Text;
                    }
                    else
                    {
                        switch (i)
                        {
                            case 100: item = "SBP"; break;
                            case 101: item = "DBP"; break;
                            case 102: item = "맥박"; break;
                            case 103: item = "호흡"; break;
                        }
                    }

                    MedcationInfo medcation = Medcations.ToList().Find(r => r.Name.Equals(item));

                    if (medcation != null)
                    {
                        itemCode = medcation.ItemNo;
                        unit = medcation.Uint;
                    }

                    for (int j = 2; j < ssView.ActiveSheet.ColumnCount; j++)
                    {
                        SQL = "";
                        SqlErr = ""; //에러문 받는 변수
                        intRowAffected = 0;

                        hour = ssView.ActiveSheet.Cells[0, j].Text;
                        minute = ssView.ActiveSheet.Cells[1, j].Text;
                        value = ssView.ActiveSheet.Cells[i, j].Text;

                        SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRANCHARTGRAPE(";
                        SQL = SQL + ComNum.VBLF + "    PTNO";
                        SQL = SQL + ComNum.VBLF + "  , EMRNO";
                        SQL = SQL + ComNum.VBLF + "  , EMRNOHIS";
                        SQL = SQL + ComNum.VBLF + "  , FORMNO";
                        SQL = SQL + ComNum.VBLF + "  , ITEM";
                        SQL = SQL + ComNum.VBLF + "  , ITEMCODE";
                        SQL = SQL + ComNum.VBLF + "  , HOUR";
                        SQL = SQL + ComNum.VBLF + "  , MINUTE";
                        SQL = SQL + ComNum.VBLF + "  , VALUE";
                        SQL = SQL + ComNum.VBLF + "  , UNIT";
                        SQL = SQL + ComNum.VBLF + "  , COLINDEX";
                        SQL = SQL + ComNum.VBLF + "  , ROWINDEX";
                        SQL = SQL + ComNum.VBLF + ") VALUES (";
                        SQL = SQL + ComNum.VBLF + "    '" + AcpEmr.ptNo + "'    --  PTNO";
                        SQL = SQL + ComNum.VBLF + "  , '" + emrNo + "'          --  EMRNO";
                        SQL = SQL + ComNum.VBLF + "  , '" + dblEmrHisNo + "'    --  EMRNOHIS";
                        SQL = SQL + ComNum.VBLF + "  , '" + mstrFormNo + "'         --  FORMNO";
                        SQL = SQL + ComNum.VBLF + "  , '" + item + "'           --  ITEM";
                        SQL = SQL + ComNum.VBLF + "  , '" + itemCode + "'       --  ITEMCODE";
                        SQL = SQL + ComNum.VBLF + "  , '" + hour + "'           --  HOUR";
                        SQL = SQL + ComNum.VBLF + "  , '" + minute + "'         --  MINUTE";
                        SQL = SQL + ComNum.VBLF + "  , '" + value + "'          --  VALUE";
                        SQL = SQL + ComNum.VBLF + "  , '" + unit + "'           --  UNIT";
                        SQL = SQL + ComNum.VBLF + "  , '" + j + "'              --  COLINDEX";
                        SQL = SQL + ComNum.VBLF + "  , '" + i + "'              --  ROWINDEX";
                        SQL = SQL + ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return false;
                        }
                    }

                    //  그래프 값을 추출 하기 위해서
                    //  로우카운트 및 i 변경
                    //  그래프 값은 100로우부터 시작한다.
                    if (i < 100 && i == (maxCount - 1))
                    {
                        maxCount = 104;
                        i = 99;
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }

            return true;
        }
    }
}
