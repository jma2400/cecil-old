﻿//
// MergeContext.cs
//
// Author:
//	 Massimiliano Mantione (massi@ximian.com)
//
// (C) 2006 Novell
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

using System;
using System.Collections.Generic;

using Mono.Cecil;

namespace Mono.Merge {


	public class InternalSymbolTable {

		MethodDefinition entryPoint = null;
		public MethodDefinition EntryPoint {
			get { return entryPoint; }
		}


		Dictionary<string,MethodDefinition> methods = new Dictionary<string,MethodDefinition> ();
		public MethodDefinition InsertMethod (MethodDefinition method, bool clone) {
			if (methods.ContainsKey (method.Name)) {
				Console.Error.WriteLine ("Error: duplicated method {0}", method.Name);
				return null;
			} else {
				if (clone) {
					method = method.Clone ();
				}
				methods [method.Name] = method;

				if (method.Name == ".start") {
					//TODO More sanity checks here...
					Console.WriteLine ("*** Method '.start' found!");
					entryPoint = method;
				} else {
					Console.WriteLine ("*** Method {0} is not named '.start'", method.Name);
				}

				return method;
			}
		}
		public MethodDefinition GetMethod (string name) {
			if (methods.ContainsKey (name)) {
				return methods [name];
			} else {
				return null;
			}
		}

		Dictionary<string,FieldDefinition> fields = new Dictionary<string,FieldDefinition> ();
		public FieldDefinition InsertField (FieldDefinition field, bool clone) {
			if (fields.ContainsKey (field.Name)) {
				Console.Error.WriteLine ("Error: duplicated field {0}", field.Name);
				return null;
			} else {
				if (clone) {
					field = field.Clone ();
				}
				fields [field.Name] = field;
				return field;
			}
		}
		public FieldDefinition GetField (string name) {
			if (fields.ContainsKey (name)) {
				return fields [name];
			} else {
				return null;
			}
		}
	}
}
