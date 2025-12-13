namespace MinimumBasketImplementation
{
    public class Basket
    {
        #region Fields
        private Guid _ID;
        private int _UserID;
        private List<Item>? _Items;
        #endregion

        #region Properties
        public Guid ID { get { return _ID; } set { _ID = value; } }
        public List<Item>? Items { get { return _Items; } set { _Items = value; } }
        public int UserID { get { return _UserID; } set { _UserID = value; } }
        #endregion

        #region Constructors
        public Basket(Guid id, List<Item> items, int userId)
        {
            ID = id;
            Items = items;
            UserID = userId;
        }

        public Basket()
        {
        }
        #endregion
    }
}
