/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Thu Feb 24 05:08:25 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    [RId (0x15)]
    internal sealed class PropertyMapTable : IMetadataTable {

        private RowCollection m_rows;

        public PropertyMapRow this [int index] {
            get { return m_rows [index] as PropertyMapRow; }
            set { m_rows [index] = value; }
        }

        public RowCollection Rows {
            get { return m_rows; }
            set { m_rows = value; }
        }

        public void Accept (IMetadataTableVisitor visitor)
        {
            visitor.Visit (this);
            this.Rows.Accept (visitor.GetRowVisitor ());
        }
    }

    internal sealed class PropertyMapRow : IMetadataRow {

        public static readonly int RowSize = 8;
        public static readonly int RowColumns = 2;

        public uint Parent;
        public uint PropertyList;

        public int Size {
            get { return PropertyMapRow.RowSize; }
        }

        public int Columns {
            get { return PropertyMapRow.RowColumns; }
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
