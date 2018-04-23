namespace shipapp.Models.ModelData
{
    /// <summary>
    /// Note Helper model class
    /// </summary>
    class Note
    {
        /// <summary>
        /// Note Id is auto generated in the database do not modify
        /// </summary>
        public long Note_Id { get; set; }
        /// <summary>
        /// note contents as string
        /// </summary>
        public string Note_Value { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        public Note() { }
    }
}