namespace MinimumBasketImplementation
{
    public class Item
    {
        #region Fields
        private int _ID;
        private string? _Name;
        private decimal _Price;
        private int _Quantity;
        #endregion

        #region Properties
        public int ID { get { return _ID; } set { _ID = value; } }
        public string? Name { get { return _Name; } set { _Name = value; } }
        public decimal Price { get { return _Price; } set { _Price = value; } }
        public int Quantity { get { return _Quantity; } set { _Quantity = value; } }
        #endregion

        #region Constructors
        public Item(int id, string name, decimal price, int quantity)
        {
            ID = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public Item()
        {
        }
        #endregion
    }
}
