using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.BattleReplay;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Battlefield.Implementation;
using SnakeBattleNet.Core.Battlemanager;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.WinForms
{
    public partial class Form1 : Form
    {
        IBattleField battleField = new BattleField();
        private BattleManager battleManager;

        public Form1()
        {
            InitializeComponent();
            InitAll();
        }

        private void InitAll()
        {
            boardGrid.ColumnCount = 27;
            boardGrid.RowCount = 27;

            var id = Guid.NewGuid().ToString();
            var brainChips = new List<IBrainModule>();
            var snakes = new List<ISnake>();
            var replayRecorder = new ReplayRecorder();
            snakes.Add(new Snake(id, "Snake number 1"));
            snakes.Add(new Snake(id, "Snake number 2"));
            snakes.Add(new Snake(id, "Snake number 3"));
            snakes.Add(new Snake(id, "Snake number 4"));
            battleManager = new BattleManager(battleField, snakes, replayRecorder);
            battleManager.InitializeField();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape") Application.Exit();
            //if (e.Alt == true && e.KeyCode.ToString() == "Escape") Application.Exit();
        }

        private void BoardGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var color = Color.Gray;
            switch (battleField[e.ColumnIndex, e.RowIndex].FieldRowContent)
            {
                case FieldRowContent.Empty:
                    color = Color.White;
                    break;
                case FieldRowContent.Wall:
                    color = Color.Orange;
                    break;
                case FieldRowContent.Head:
                    color = Color.Blue;
                    break;
                case FieldRowContent.Body:
                    color = Color.Red;
                    break;
                case FieldRowContent.Tail:
                    color = Color.Green;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var cellStyle = DataGridViewCellStyle(color);
            (sender as DataGridView)[e.ColumnIndex, e.RowIndex].Style = cellStyle;
        }

        private DataGridViewCellStyle DataGridViewCellStyle(Color color)
        {
            var cellStyle = new DataGridViewCellStyle();
            cellStyle.BackColor = color;
            cellStyle.ForeColor = color;
            cellStyle.SelectionBackColor = color;
            cellStyle.SelectionForeColor = color;
            return cellStyle;
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            battleManager.Act();
            boardGrid.Invalidate();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            timer1.Interval = 250;
            if (timer1.Enabled)
                timer1.Stop();
            else
                timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            battleManager.Act();
            boardGrid.Invalidate();
        }
    }
}
