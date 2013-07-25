using System;
using swf = System.Windows.Forms;
using Eto.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Eto.Platform.Windows.Forms.Controls
{
	public class GridViewHandler : GridHandler<GridView>, IGridView
	{
		CollectionHandler collection;
		
		public GridViewHandler ()
		{
		}

		protected override object GetItemAtRow (int row)
		{
			if (collection == null) return null;
			return collection.Collection[row];
		}

		class CollectionHandler : DataStoreChangedHandler<object, IDataStore>
		{
			public GridViewHandler Handler { get; set; }
			
			public override void AddRange (IEnumerable<object> items)
			{
				Handler.SetRowCount(Collection.Count);
			}
			
			public override void AddItem (object item)
			{
				Handler.IncrementRowCountBy(1);
			}

			public override void InsertItem (int index, object item)
			{
				Handler.IncrementRowCountBy(1);
			}

			public override void RemoveItem (int index)
			{
				Handler.IncrementRowCountBy(-1);
			}

			public override void RemoveAllItems ()
			{
				Handler.SetRowCount(0);
			}
		}

		private void SetRowCount(int rowCount)
		{
			Control.RowCount = rowCount;
			Control.Invalidate();
		}

		private void IncrementRowCountBy(int increment)
		{
			Control.RowCount += increment;
			Control.Invalidate();
		}

		public IDataStore DataStore {
			get { return collection != null ? collection.Collection : null; }
			set {
				if (collection != null)
					collection.Unregister ();
				collection = new CollectionHandler { Handler = this };
				collection.Register (value);
			}
		}
	}
}

