namespace SimCity_Model.Model
{
    public class Industrial : Building
    {
        #region Fields
        private int _capacity;
        private int _currentWorkerNumber;
        #endregion

        #region Properties
        public int Capacity { get { return _capacity; } set { _capacity = value; } }
        public int CurrentWorkerNumber { get { return _currentWorkerNumber; } set { _currentWorkerNumber = value; } }

        #endregion

        #region Constructor
        public Industrial(int capacity)
        {
            _capacity = capacity;
            _currentWorkerNumber = 0;
        }
        #endregion

        #region Methods
        public void addWorker(int num)
        {
            _currentWorkerNumber += num;
        }
        public void removeWorker(int num)
        {
            _currentWorkerNumber -= num;
        }

        #endregion
    }
}