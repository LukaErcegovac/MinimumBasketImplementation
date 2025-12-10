namespace UserMicroservice
{
    public class User
    {
        #region Fields
        private int _ID;
        private string _FirstName;
        private string _LastName;
        private string _Email;
        private string _Password;
        #endregion

        #region Properties
        public int ID { get { return _ID; } set { _ID = value; } }
        public string FirstName { get { return _FirstName; } set { _FirstName = value; } }
        public string LastName { get { return _LastName; } set { _LastName = value; } }
        public string Email { get { return _Email; } set { _Email = value; } }
        public string Password { get { return _Password; } set { _Password = value; } }
        #endregion

        #region Constructors
        public User(int id, string firstName, string lastName, string email, string password) 
        { 
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

        public User() { }
        #endregion
    }
}
