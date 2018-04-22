using System.Windows.Forms;

namespace shipapp.Connections.HelperClasses
{
    /// <summary>
    /// Helper class to add columns to selected datagridview
    /// </summary>
    class DataGridViewColumnHelper
    {
        public DataGridViewColumnHelper() { }
        /// <summary>
        /// Add Text column to datagrid view
        /// </summary>
        /// <param name="view">Data grid View to attach to</param>
        /// <param name="colHeaderText">The texts that is displayed to the user</param>
        /// <param name="colName">The internal name of the column</param>
        /// <param name="cellText">The text value of the cell</param>
        /// <param name="colDisplayIndex">The position that the column is displayed to the user</param>
        public void AddCustomColumn(DataGridView view, string colHeaderText, string colName, string cellText, int colDisplayIndex)
        {
            DataGridViewTextBoxColumn dgvText = new DataGridViewTextBoxColumn();
            dgvText.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvText.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvText.DisplayIndex = colDisplayIndex;
            dgvText.HeaderText = colHeaderText;
            dgvText.Name = colName;
            dgvText.ToolTipText = cellText;
            dgvText.CellTemplate.Value = cellText;
            view.Columns.Add(dgvText);
            view.Update();
        }
        /// <summary>
        /// Adds drop down list column
        /// </summary>
        /// <param name="view">Data grid View to attach to</param>
        /// <param name="colHeaderText">The texts that is displayed to the user</param>
        /// <param name="colName">The internal name of the column</param>
        /// <param name="cellList">List of phone numbers to be used by the column</param>
        /// <param name="colDisplayIndex">The position that the column is displayed to the user</param>
        public void AddCustomColumn(DataGridView view, string colHeaderText, string colName, int colDisplayIndex)
        {
            DataGridViewComboBoxColumn boxColumn = new DataGridViewComboBoxColumn
            {
                AutoComplete = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = colDisplayIndex,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox,
                HeaderText = colHeaderText,
                Name = colName,
                Sorted = true,
                SortMode = DataGridViewColumnSortMode.Automatic,
                ToolTipText = colHeaderText
            };
            view.Columns.Add(boxColumn);
            view.Update();
        }
        /// <summary>
        /// Adds a button column
        /// </summary>
        /// <param name="view">Data grid View to attach to</param>
        /// <param name="colHeaderText">The texts that is displayed to the user</param>
        /// <param name="colName">The internal name of the column</param>
        /// <param name="colDisplayIndex">The position that the column is displayed to the user</param>
        /// <param name="buttonText"></param>
        public void AddCustomColumn(DataGridView view, string colHeaderText, string colName, int colDisplayIndex, string buttonText)
        {
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            buttonColumn.DisplayIndex = colDisplayIndex;
            buttonColumn.HeaderText = colHeaderText;
            buttonColumn.Name = colName;
            buttonColumn.Text = buttonText;
            buttonColumn.ToolTipText = buttonText;
            view.Columns.Add(buttonColumn);
            view.Update();
        }
    }
}
