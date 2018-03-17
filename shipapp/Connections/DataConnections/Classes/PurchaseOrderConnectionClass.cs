using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;
using shipapp.Models.ModelData;

namespace shipapp.Connections.DataConnections.Classes
{
    class PurchaseOrderConnectionClass:DatabaseConnection
    {
        public PurchaseOrderConnectionClass() : base()
        {
            //
        }
        /// <summary>
        /// Gets a specific Purchase Order.
        /// </summary>
        /// <param name="id">Purchase Order Id to get</param>
        /// <returns>Purchase Order object</returns>
        public PurchaseOrder GetPurchaseOrder(long id)
        {
            PurchaseOrder po = Get_PurchaseOrder(id);
            po.CreatedBy = DataConnectionClass.EmployeeConn.GetFaculty(po.CreatedBy.Id);
            po.ApprovedBy = DataConnectionClass.EmployeeConn.GetFaculty(po.ApprovedBy.Id);
            return po;
        }
        /// <summary>
        /// Adds a PO to the database
        /// </summary>
        /// <param name="value">PO combined</param>
        public void AddPurchaseOrder(PurchaseOrder value)
        {
            Write_PurchaseOrder_ToDatabase(value);
        }
        /// <summary>
        /// Update an existing PO
        /// </summary>
        /// <param name="value">Modified PO to be updated</param>
        public void UpdatePurchaseOrder(PurchaseOrder value)
        {
            Update_PurchaseOrder(value);
        }
        public void DeletePO(PurchaseOrder p)
        {
            throw new NotImplementedException();
        }
    }
}
