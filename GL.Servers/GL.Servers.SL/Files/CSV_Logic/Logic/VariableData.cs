namespace GL.Servers.SL.Files.CSV_Logic.Logic
{
	using GL.Servers.Files.CSV_Reader;
	using GL.Servers.SL.Files.CSV_Helpers;

    internal class VariableData : Data
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="VariableData"/> class.
        /// </summary>
        /// <param name="Row">The row.</param>
        /// <param name="DataTable">The data table.</param>
        public VariableData(Row Row, DataTable DataTable) : base(Row, DataTable)
        {
            Data.Load(this, this.GetType(), Row);
        }

        public int Value
        {
            get; set;
        }
    }
}
