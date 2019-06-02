using System;
using System.Collections.Generic;
namespace YawnMassage.Common.Domain.Contracts.DataTemplates
{
    /// <summary>
    /// Defines the <see cref="MappedFieldSet" />
    /// </summary>
    public class MappedFieldSet
    {
        /// <summary>
        /// Gets the DataRow
        /// </summary>
        private IDataRow DataRow { get; }

        /// <summary>
        /// Gets the FieldCells
        /// </summary>
        public Dictionary<string, IDataCell> FieldCells { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedFieldSet"/> class.
        /// </summary>
        /// <param name="dataRow">The dataRow<see cref="IDataRow"/></param>
        /// <param name="fieldCells">The fieldCells<see cref="Dictionary{string, IDataCell}"/></param>
        public MappedFieldSet(IDataRow dataRow, Dictionary<string, IDataCell> fieldCells)
        {
            DataRow = dataRow;
            FieldCells = fieldCells;
        }

        /// <summary>
        /// The StringValue
        /// </summary>
        /// <param name="fieldName">The fieldName<see cref="string"/></param>
        /// <param name="isRequired">The isRequired<see cref="bool"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string StringValue(string fieldName, bool isRequired = true)
        {
            var dataCell = FieldCells[fieldName.ToLower()];

            string value = dataCell.Value == null ? null : dataCell.Value.ToString();

            if (isRequired && string.IsNullOrWhiteSpace(value))
            {
                dataCell.MarkAsRequired();
                DataRow.MarkAsError();
            }

            return value;
        }

        /// <summary>
        /// The BooleanValue
        /// </summary>
        /// <param name="fieldName">The fieldName<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool BooleanValue(string fieldName)
        {
            var dataCell = FieldCells[fieldName.ToLower()];

            if (dataCell.Value != null && !(dataCell.Value is int))
            {
                dataCell.MarkAsInvalidDataFormat();
                DataRow.MarkAsError();

                return default(bool);
            }
            else if (dataCell.Value == null)
            {
                return default(bool);
            }
            else
            {
                var selcetedValue = (int)dataCell.Value;
                if (selcetedValue == 1 || selcetedValue == 0)
                {
                    return selcetedValue == 1;
                }
                else
                {
                    dataCell.MarkAsInvalidDataFormat();
                    DataRow.MarkAsError();
                    return default(bool);
                }
            }
        }

        /// <summary>
        /// The IntValue
        /// </summary>
        /// <param name="fieldName">The fieldName<see cref="string"/></param>
        /// <param name="isRequired">The isRequired<see cref="bool"/></param>
        /// <returns>The <see cref="int"/></returns>
        public int IntValue(string fieldName, bool isRequired = false)
        {
            var dataCell = FieldCells[fieldName.ToLower()];
            if (isRequired && dataCell.Value == null)
            {
                dataCell.MarkAsRequired();
                DataRow.MarkAsError();

                return default(int);
            }
            else if (!isRequired && dataCell.Value == null)
            {
                return default(int);
            }
            else if (!(dataCell.Value is int))
            {
                dataCell.MarkAsInvalidDataFormat();
                DataRow.MarkAsError();

                return default(int);
            }

            return (int)dataCell.Value;
        }

        /// <summary>
        /// The DoubleValue
        /// </summary>
        /// <param name="fieldName">The fieldName<see cref="string"/></param>
        /// <param name="isRequired">The isRequired<see cref="bool"/></param>
        /// <returns>The <see cref="double"/></returns>
        public double DoubleValue(string fieldName, bool isRequired = false)
        {
            var dataCell = FieldCells[fieldName.ToLower()];
            if (isRequired && dataCell.Value == null)
            {
                dataCell.MarkAsRequired();
                DataRow.MarkAsError();

                return default(double);
            }
            else if (!(dataCell.Value is double) || !(dataCell.Value is int))
            {
                dataCell.MarkAsInvalidDataFormat();
                DataRow.MarkAsError();

                return default(double);
            }
            return (double)dataCell.Value;
        }

        /// <summary>
        /// The MarkAsError
        /// </summary>
        public void MarkAsError()
        {
            DataRow.MarkAsError();
        }

        /// <summary>
        /// The DateTimeValue
        /// </summary>
        /// <param name="fieldName">The fieldName<see cref="string"/></param>
        /// <param name="isRequired">The isRequired<see cref="bool"/></param>
        /// <returns>The <see cref="DateTime?"/></returns>
        public DateTime? DateTimeValue(string fieldName, bool isRequired = false)
        {
            var dataCell = FieldCells[fieldName.ToLower()];
            if (isRequired && dataCell.Value == null)
            {
                dataCell.MarkAsRequired();
                DataRow.MarkAsError();

                return null;
            }
            else if (dataCell.Value != null && !(dataCell.Value is DateTime))
            {
                dataCell.MarkAsInvalidDataFormat();
                DataRow.MarkAsError();

                return null;
            }
            else if (dataCell.Value == null)
            {
                return null;
            }

            return (DateTime)dataCell.Value;
        }
    }
}
