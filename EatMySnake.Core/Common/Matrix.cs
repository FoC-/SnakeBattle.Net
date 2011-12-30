namespace EatMySnake.Core.Common
{
    public class Matrix
    {
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        private Row[,] _rows;

        public Matrix(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            this._rows = new Row[sizeX, sizeY];
        }

        public Row this[int x, int y]
        {
            get
            {
                if (x > -1 && x < SizeX && y > -1 && y < SizeY)
                {
                    return _rows[x, y];
                }
                return null;
            }
            set
            {
                if (x > -1 && x < SizeX && y > -1 && y < SizeY)
                {
                    _rows[x, y] = value;
                }
            }
        }

        //todo:need to remove head from this
        public void InitilaizeFieldWithHead()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    _rows[x, y] = new Row();
                }
            }
            int xh = SizeX / 2;
            int yh = SizeY / 2;
            _rows[xh, yh] = new Row(Content.OwnHead);
        }
    }
}
