namespace Zinger2.Service.Internal
{
    public class ColumnInfo
    {
        public string Name { get; set; } = string.Empty;
        public string CSharpType { get; set; } = string.Empty;
        public int? Size { get; set; }
        public bool IsNullable { get; set; }
        public int Index { get; set; }

        public string PascalCaseName
        {
            get
            {
                string name = PropertyName;
                return name[..1].ToUpper() + name[1..];
            }
        }

        public string PropertyName => (Index > 0) ? $"{Name}{Index}" : Name;

        public string ToColumnName() => (Name.StartsWith("@")) ? Name.Substring(1) : Name;

        public IEnumerable<string> GetAttributes()
        {
            if (CSharpType.Equals("string"))
            {
                if (Size.HasValue)
                {
                    yield return $"[MaxLength({Size})]";
                }

                if (!IsNullable)
                {
                    yield return $"[Required]";
                }
            }
        }
    }
}
