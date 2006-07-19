//
// <%=$cur_coll.name%>.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Generated by /CodeGen/cecil-gen.rb do not edit
// <%=Time.now%>
//
// (C) 2005 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

namespace <%=$cur_coll.target%> {

	using System;
	using System.Collections;
	using System.Collections.Specialized;

	using Mono.Cecil.Cil;

	using Hcp = Mono.Cecil.HashCodeProvider;
	using Cmp = System.Collections.Comparer;

	public sealed class <%=$cur_coll.name%> : NameObjectCollectionBase, <%=$cur_coll.intf%> {

		<%=$cur_coll.container%> m_container;

		public event <%=$cur_coll.item_name%>EventHandler On<%=$cur_coll.item_name%>Added;
		public event <%=$cur_coll.item_name%>EventHandler On<%=$cur_coll.item_name%>Removed;

		public <%=$cur_coll.type%> this [int index] {
			get { return this.BaseGet (index) as <%=$cur_coll.type%>; }
			set { this.BaseSet (index, value); }
		}

		object IIndexedCollection.this [int index] {
			get { return this.BaseGet (index); }
		}

		public <%=$cur_coll.type%> this [string fullName] {
			get { return this.BaseGet (fullName) as <%=$cur_coll.type%>; }
			set { this.BaseSet (fullName, value); }
		}

		public <%=$cur_coll.container%> Container {
			get { return m_container; }
		}

		public bool IsSynchronized {
			get { return (this as ICollection).IsSynchronized; }
		}

		public object SyncRoot {
			get { return (this as ICollection).SyncRoot; }
		}

		public <%=$cur_coll.name%> (<%=$cur_coll.container%> container) :
			base (Hcp.Instance, Cmp.Default)
		{
			m_container = container;
		}

		public void Add (<%=$cur_coll.type%> value)
		{
			if (value == null)
				throw new ArgumentNullException ("value");
<% if ($cur_coll.name == "TypeDefinitionCollection") %>
			if (this.Contains (value))
				throw new ArgumentException ("Duplicated value");
<% end %>
			if (On<%=$cur_coll.item_name%>Added != null)
				On<%=$cur_coll.item_name%>Added (this, new <%=$cur_coll.item_name%>EventArgs (value));

			this.BaseAdd (value.FullName, value);
		}

		public void Clear ()
		{
			if (On<%=$cur_coll.item_name%>Removed != null)
				foreach (<%=$cur_coll.type%> item in this)
					On<%=$cur_coll.item_name%>Removed (this, new <%=$cur_coll.item_name%>EventArgs (item));
			this.BaseClear ();
		}

		public bool Contains (<%=$cur_coll.type%> value)
		{
			return Contains (value.FullName);
		}

		public bool Contains (string fullName)
		{
			return this.BaseGet (fullName) != null;
		}

		public int IndexOf (<%=$cur_coll.type%> value)
		{
			string [] keys = this.BaseGetAllKeys ();
			return Array.IndexOf (keys, value.FullName, 0, keys.Length);
		}

		public void Remove (<%=$cur_coll.type%> value)
		{
			if (On<%=$cur_coll.item_name%>Removed != null && this.Contains (value))
				On<%=$cur_coll.item_name%>Removed (this, new <%=$cur_coll.item_name%>EventArgs (value));
			this.BaseRemove (value.FullName);
		}

		public void RemoveAt (int index)
		{
			if (On<%=$cur_coll.item_name%>Removed != null)
				On<%=$cur_coll.item_name%>Removed (this, new <%=$cur_coll.item_name%>EventArgs (this [index]));
			this.BaseRemoveAt (index);
		}

		public void CopyTo (Array ary, int index)
		{
			(this as ICollection).CopyTo (ary, index);
		}

		public new IEnumerator GetEnumerator ()
		{
			return this.BaseGetAllValues ().GetEnumerator ();
		}
<% if !$cur_coll.visitor.nil? then %>
		public void Accept (<%=$cur_coll.visitor%> visitor)
		{
			visitor.<%=$cur_coll.visitThis%> (this);
		}
<% end %>
#if CF_1_0 || CF_2_0
		internal object [] BaseGetAllValues ()
		{
			object [] values = new object [this.Count];
			for (int i=0; i < values.Length; ++i) {
				values [i] = this.BaseGet (i);
			}
			return values;
		}
#endif
	}
}
