using MapDomain.Exceptions;

namespace MapDomain.ValueObjects
{
    public class Square5
    {
        private const int MaxRightOrLeftMovements = 4;

        private Map map;
        private int i;
        private int j;
        private int current;
        private int max;
        private int min;
        private int left;
        private int right;

        public Square5(Map map, int i, int j)
        {
            Initialize(map, i, j);
            Validate();
            InitializeI();
            Calculate();
            SetResultI();
            InitializeJ();
            Calculate();
            SetResultJ();
        }

        private void Initialize(Map map, int i, int j)
        {
            this.map = map;
            this.i = i;
            this.j = j;
        }

        private void Validate()
        {
            if (i < 0 || i >= map.SegmentsHeight || j < 0 || j >= map.SegmentsWidth)
            {
                throw new NotFoundException();
            }
        }

        private void InitializeI()
        {
            left = right = current = i;
            max = map.SegmentsHeight - 1;
            min = 0;
        }

        private void InitializeJ()
        {
            left = right = current = j;
            max = map.SegmentsWidth - 1;
            min = 0;
        }

        private void Calculate()
        {
            var isLeftBreak = left == min;
            var isRightBreak = right == max;

            if (isLeftBreak && isRightBreak)
            {
                return;
            }

            if(!isLeftBreak)
            {
                left--;
            }

            if(!isRightBreak)
            {
                right++;
            }

            var currentLeftMovements = current - left;
            var currentRightMovements = right - current;
            var currentMovements = currentLeftMovements + currentRightMovements;

            if (currentMovements == MaxRightOrLeftMovements)
            {
                return;
            }

            Calculate();
        }

        private void SetResultI()
        {
            UpI = left;
            DownI = right;
        }

        private void SetResultJ()
        {
            LeftJ = left;
            RightJ = right;
        }

        public int UpI { get; private set; }

        public int DownI { get; private set; }

        public int LeftJ { get; private set; }

        public int RightJ { get; private set; }
    }
}
