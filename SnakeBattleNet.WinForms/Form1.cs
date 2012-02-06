using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EatMySnake.Core.Battlefield;
using EatMySnake.Core.Battlefield.Implementation;
using EatMySnake.Core.Battlemanager;
using EatMySnake.Core.Common;
using EatMySnake.Core.Snake;
using EatMySnake.Core.Snake.Implementation;

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

            var id = Guid.NewGuid();
            var brainChips = new List<IBrainChip>();
            var snakes = new List<ISnake>();
            snakes.Add(new Snake(id, "Snake number 1", id, brainChips));
            snakes.Add(new Snake(id, "Snake number 2", id, brainChips));
            snakes.Add(new Snake(id, "Snake number 3", id, brainChips));
            snakes.Add(new Snake(id, "Snake number 4", id, brainChips));
            battleManager = new BattleManager(battleField, snakes);
            battleManager.SetupHandlers();
            battleManager.InitializeField();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape") Application.Exit();
            //if (e.Alt == true && e.KeyCode.ToString() == "Escape") Application.Exit();
        }

        private void BoardGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            switch (battleField[e.ColumnIndex, e.RowIndex].Content)
            {
                case Content.Empty:
                    {
                        e.CellStyle.BackColor = Color.White;
                        e.CellStyle.ForeColor = Color.White;
                        e.CellStyle.SelectionBackColor = Color.White;
                        e.CellStyle.SelectionForeColor = Color.White;
                    }
                    break;
                case Content.Wall:
                    {
                        e.CellStyle.BackColor = Color.Orange;
                        e.CellStyle.ForeColor = Color.Orange;
                        e.CellStyle.SelectionBackColor = Color.Orange;
                        e.CellStyle.SelectionForeColor = Color.Orange;
                    }
                    break;
                case Content.Head:
                    {
                        e.CellStyle.BackColor = Color.Blue;
                        e.CellStyle.ForeColor = Color.Blue;
                        e.CellStyle.SelectionBackColor = Color.Blue;
                        e.CellStyle.SelectionForeColor = Color.Blue;
                    }
                    break;
                case Content.Body:
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.SelectionBackColor = Color.Red;
                        e.CellStyle.SelectionForeColor = Color.Red;
                    }
                    break;
                case Content.Tail:
                    {
                        e.CellStyle.BackColor = Color.Green;
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.SelectionBackColor = Color.Green;
                        e.CellStyle.SelectionForeColor = Color.Green;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
