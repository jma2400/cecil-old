//
// TypeDefinitionCollection.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Generated by /CodeGen/cecil-gen.rb do not edit
// Wed Apr 19 19:55:04 CEST 2006
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

namespace Mono.Cecil {

	using System;
	using System.Collections;
	using System.Collections.Specialized;

	using Mono.Cecil.Cil;

	using Hcp = Mono.Cecil.HashCodeProvider;
	using Cmp = System.Collections.Comparer;

	public sealed class TypeDefinitionCollection : NameObjectCollectionBase, ITypeDefinitionCollection {

		ModuleDefinition m_container;

		public event TypeDefinitionEventHandler OnTypeDefinitionAdded;
		public event TypeDefinitionEventHandler OnTypeDefinitionRemoved;

		public TypeDefinition this [int index] {
			get { return this.BaseGet (index) as TypeDefinition; }
			set { this.BaseSet (index, value); }
		}

		object IIndexedCollection.this [int index] {
			get { return this.BaseGet (index); }
		}

		public TypeDefinition this [string fullName] {
			get { return this.BaseGet (fullName) as TypeDefinition; }
			set { this.BaseSet (fullName, value); }
		}

		public ModuleDefinition Container {
			get { return m_container; }
		}

		public bool IsSynchronized {
			get { return (this as ICollection).IsSynchronized; }
		}

		public object SyncRoot {
			get { return (this as ICollection).SyncRoot; }
		}

		public TypeDefinitionCollection (ModuleDefinition container) :
			base (Hcp.Instance, Cmp.Default)
		{
			m_container = container;
		}

		public void Add (TypeDefinition value)
		{
			if (value == null)
				throw new ArgumentNullException ("value");

			if (this.Contains (value))
				throw new ArgumentException ("Duplicated value");

			if (OnTypeDefinitionAdded != null)
				OnTypeDefinitionAdded (this, new TypeDefinitionEventArgs (value));

			this.BaseAdd (value.FullName, value);
		}

		public void Clear ()
		{
			if (OnTypeDefinitionRemoved != null)
				foreach (TypeDefinition item in this)
					OnTypeDefinitionRemoved (this, new TypeDefinitionEventArgs (item));
			this.BaseClear ();
		}

		public bool Contains (TypeDefinition value)
		{
			return Contains (value.FullName);
		}

		public bool Contains (string fullName)
		{
			return this.BaseGet (fullName) != null;
		}

		public int IndexOf (TypeDefinition value)
		{
			string [] keys = this.BaseGetAllKeys ();
			return Array.IndexOf (keys, value.FullName, 0, keys.Length);
		}

		public void Remove (TypeDefinition value)
		{
			if (OnTypeDefinitionRemoved != null && this.Contains (value))
				OnTypeDefinitionRemoved (this, new TypeDefinitionEventArgs (value));
			this.BaseRemove (value.FullName);
		}

		public void RemoveAt (int index)
		{
			if (OnTypeDefinitionRemoved != null)
				OnTypeDefinitionRemoved (this, new TypeDefinitionEventArgs (this [index]));
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

		public void Accept (IReflectionVisitor visitor)
		{
			visitor.VisitTypeDefinitionCollection (this);
		}

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
