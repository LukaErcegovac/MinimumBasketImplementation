namespace OrderMicroservice
{
    public class Order
    {
        #region Fields
        private int _ID;
        private int _UserID;
        private DateTime _OrderDate;
        private List<OrderItem>? _OrderItems;
        private decimal _TotalAmount;
        #endregion

        #region Properties
        public int ID { get { return _ID; } set { _ID = value; } }
        public int UserID { get { return _UserID; } set { _UserID = value; } }
        public DateTime OrderDate { get { return _OrderDate; } set { _OrderDate = DateTime.Now; } }
        public List<OrderItem>? OrderItems { get { return _OrderItems; } set { _OrderItems = value; CalculateTotalAmount(); } }
        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                _TotalAmount = value;
            }
        }
        #endregion

        #region Constructors
        public Order() { }
        public Order(int id, int userId, DateTime orderDate, OrderItem? orderItem, decimal totalAmount)
        {
            ID = id;
            UserID = userId;
            OrderDate = orderDate;
            OrderItems = orderItem != null ? new List<OrderItem> { orderItem } : null;
            TotalAmount = totalAmount;
            CalculateTotalAmount();
        }
        #endregion

        #region Methods
        private void CalculateTotalAmount()
        {
            TotalAmount = OrderItems?.Sum(oi => oi.UnitPrice * oi.Quantity) ?? 0;
        }
        #endregion
    }
}
