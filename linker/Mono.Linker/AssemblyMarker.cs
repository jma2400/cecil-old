//
// AssemblyMarker.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// (C) 2006 Jb Evain
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

namespace Mono.Linker {

	using System.Collections;

	using Mono.Cecil;

	public class AssemblyMarker {

		AssemblyAction _action;
		AssemblyDefinition _assembly;

		IDictionary _typeMarkers;

		public AssemblyDefinition Assembly {
			get { return _assembly;  }
		}

		public AssemblyAction Action {
			get { return _action; }
		}

		public AssemblyMarker (AssemblyAction action, AssemblyDefinition assembly)
		{
			_action = action;
			_assembly = assembly;

			_typeMarkers = new Hashtable ();
		}

		public TypeMarker Mark (TypeDefinition type)
		{
			if (type.Module != _assembly.MainModule)
				throw new LinkException ("Could not get a marker for a different assembly");

			TypeMarker marker = (TypeMarker) _typeMarkers [type.FullName];
			if (marker == null) {
				marker = new TypeMarker (type);
				_typeMarkers.Add (type.FullName, marker);
			}

			return marker;
		}

		public TypeDefinition Resolve (TypeReference type)
		{
			return _assembly.MainModule.Types [type.FullName];
		}

		public FieldDefinition Resolve (FieldReference field)
		{
			TypeDefinition type = Resolve (GetGenericDefinition (field.DeclaringType));
			return GetField (type.Fields, field);
		}

		FieldDefinition GetField (ICollection collection, FieldReference reference)
		{
			foreach (FieldDefinition field in collection) {
				if (field.Name != reference.Name)
					continue;

				if (field.FieldType.FullName != field.FieldType.FullName)
					continue;

				return field;
			}

			return null;
		}

		public MethodDefinition Resolve (MethodReference method)
		{
			TypeDefinition type = Resolve(GetGenericDefinition (method.DeclaringType));
			if (method.Name == MethodDefinition.Cctor || method.Name == MethodDefinition.Ctor)
				return GetMethod (type.Constructors, method);
			else
				return GetMethod (type.Methods, method);
		}

		TypeReference GetGenericDefinition (TypeReference type)
		{
			TypeReference t = type;
			while (t is GenericInstanceType)
				t = ((GenericInstanceType) t).ElementType;
			return t;
		}

		MethodDefinition GetMethod (ICollection collection, MethodReference reference)
		{
			foreach (MethodDefinition meth in collection) {
				if (meth.Name != reference.Name)
					continue;

				if (meth.Parameters.Count != reference.Parameters.Count)
					continue;

				if (meth.ReturnType.ReturnType.FullName != reference.ReturnType.ReturnType.FullName)
					continue;

				for (int i = 0; i < meth.Parameters.Count; i++)
					if (meth.Parameters [i].ParameterType.FullName != reference.Parameters [i].ParameterType.FullName)
						continue;

				return meth;
			}

			return null;
		}

		public TypeMarker [] GetTypes ()
		{
			TypeMarker [] markers = new TypeMarker [_typeMarkers.Count];
			_typeMarkers.Values.CopyTo (markers, 0);
			return markers;
		}

		public override string ToString ()
		{
			return "Assembly(" + _assembly.Name.FullName + ")";
		}
	}
}