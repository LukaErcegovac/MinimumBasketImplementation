using System.Text.Json.Serialization;

namespace OrderMicroservice
{
    public class OrderItem
    {
        #region Fields
        public int _ID;
        private int _ProductID;
        private int _Quantity;
        private decimal _UnitPrice;
        private int _OrderID;
        #endregion

        #region Properties
        public int ID { get { return _ID; } set { _ID = value; } }
        public int ProductID { get { return _ProductID; } set { _ProductID = value; } }
        public int Quantity { get { return _Quantity; } set { _Quantity = value; } }
        public decimal UnitPrice { get { return _UnitPrice; } set { _UnitPrice = value; } }
        public int OrderID { get { return _OrderID; } set { _OrderID = value; } }
        [JsonIgnore]
        public Order? Order { get; set; }
        #endregion

        #region Constructors
        public OrderItem() { }
        public OrderItem(int id, int productId, int quantity, decimal unitPrice, int orderID)
        {
            ID = id;
            ProductID = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            OrderID = orderID;
        }
        #endregion
    }
}
