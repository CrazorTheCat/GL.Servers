namespace GL.Servers.HD.Files.CSV_Logic.Data
{
	using GL.Servers.Files.CSV_Reader;
	using GL.Servers.HD.Files.CSV_Helpers;

    internal class PredefinedOrderData : Data
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="PredefinedOrderData"/> class.
        /// </summary>
        /// <param name="Row">The row.</param>
        /// <param name="DataTable">The data table.</param>
        public PredefinedOrderData(Row Row, DataTable DataTable) : base(Row, DataTable)
        {
            Data.Load(this, this.GetType(), Row);
        }

        public string Goods
        {
            get; set;
        }

        public int GoodAmounts
        {
            get; set;
        }

        public string OrderReceiver
        {
            get; set;
        }
    }
}
